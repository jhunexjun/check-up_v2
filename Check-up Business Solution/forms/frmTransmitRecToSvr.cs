using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Check_up.forms
{
    public partial class frmTransmitRecToSvr : Form
    {
        int i, rowCount;
        string sql;
        DataTable dt; database db;

        public frmTransmitRecToSvr()
        {
            InitializeComponent();
        }

        private void frmTransmitRecToSvr_Load(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(Application.StartupPath + @"\json\Out\", "*.json", SearchOption.TopDirectoryOnly);

            string filename = ""; int position;
            
            ListViewItem lvitem = null;
            foreach (string file in files)
            {
                position = file.LastIndexOf(@"\");
                filename = file.Substring(position + 1);
                lvitem = new ListViewItem(filename);
                lvitem.SubItems.Add(file);
                listView1.Items.AddRange(new ListViewItem[] { lvitem });
            }

            if (listView1.Items.Count <= 0)
                btnUpload.Enabled = false;

            rowCount = listView1.Columns.Count;
            for (i = 0; i < rowCount; i++)
                listView1.Columns[i].Width = -2;

            lblCount.Text = listView1.Items.Count + " item(s) found.";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "Are you sure you want to transmit this/these file(s) now to branches?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                sql = "SELECT code,ftp_username,ftp_password,ftp_url FROM warehouse WHERE deactivated='N' AND ftp_url IS NOT null";
                db = new database(); dt = new DataTable();
                dt = db.select(sql, vars.MySqlConnection);
                rowCount = dt.Rows.Count;

                string warehouse, user, pw, url;
                string currentFileName = "", currentFileFullPath = "";
                for(i = 0; i < rowCount; i++)
                {
                    warehouse = dt.Rows[i]["code"].ToString();
                    user = dt.Rows[i]["ftp_username"].ToString();
                    pw = dt.Rows[i]["ftp_password"].ToString();
                    url = dt.Rows[i]["ftp_url"].ToString();     // should be in proper format e.g. ftp://192.168.1.100/incoming/

                    ftp ftpWebRequest = new ftp(user, pw); 

                    for(int i2 = 0; i2 < listView1.Items.Count; i2++)
                    {
                        currentFileName = listView1.Items[i2].Text;
                        currentFileFullPath = listView1.Items[i2].SubItems[1].Text;

                        ftpWebRequest.urlAndFileName = url + currentFileName;
                        ftpWebRequest.filePath = currentFileFullPath;

                        if (ftpWebRequest.Upload())
                        {
                            sql = "START TRANSACTION;";
                            sql += "SET @date=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s');";
                            sql += "SET @filename='" + currentFileName + "';";
                            sql += "SET @exportedFileId = (SELECT id FROM export_importfiles WHERE filename=@filename LIMIT 1);";
                            sql += "INSERT INTO transmittedfiles(exportedFilesId,warehouse,createdBy,createDate) VALUES(@exportedFileId,'" + warehouse + "','" + vars.username + "', @date);";
                            sql += "UPDATE export_importfiles SET transmittedBy='" + vars.username + "' WHERE id=@exportedFileId;";
                            sql += "COMMIT;";

                            database db2 = new database(); DataTable dt2 = new DataTable();
                            db2.executeNonQuery(sql, vars.MySqlConnection);
                            File.Move(currentFileFullPath, Application.StartupPath + @"\json\Transmitted\" + currentFileName);
                            listView1.Items.RemoveAt(i2);
                            i2--;
                        }
                        else
                        {
                            result = MessageBox.Show(this, "Do you want to continue with the rest of the files?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.No)
                                break;
                        }
                    }
                }
            }
            if (listView1.Items.Count <= 0)
                btnUpload.Enabled = false;
        }
    }
}
