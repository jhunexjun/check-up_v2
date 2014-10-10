using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Check_up.forms
{
    public partial class frmDBConfig : Form
    {
        public frmDBConfig()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.ShowDialog();
            txtUserImageLocation.Text = folderBrowserDialog1.SelectedPath;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string datasource = txtDatasource.Text.Trim();
            string database = txtDatabase.Text.Trim();
            string username = txtUsername.Text.Trim();
            string pw = txtPw.Text.Trim();
            pw = CryptorEngine.Encrypt(pw);

            if ((datasource != "") && (database != "") && (username != "") && (pw != ""))
            {
                string[] lines = { "[database]", ";client configuration", "datasource=" + datasource, "database=" + database, "username=" + username, "password=" + pw, "\n;default user image directory", "user_image_location=" };
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\check-up.ini"))
                {
                    foreach (string line in lines)
                        file.WriteLine(line);
                }
                MessageBox.Show(this, "Successfully saved config file.", "Config file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
                MessageBox.Show(this, "Please enter required values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
