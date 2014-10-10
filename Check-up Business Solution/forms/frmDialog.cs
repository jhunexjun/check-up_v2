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
    public partial class frmDialog : Form
    {
        private string sql;
        private string selectedItem;

        public frmDialog()
        {
            InitializeComponent();
            listView1.MouseDoubleClick += new MouseEventHandler(listView1_MouseDoubleClick);
        }

        private void frmDialog_Load(object sender, EventArgs e)
        {
            database db = new database();
            DataTable dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);

            foreach(DataColumn column in dt.Columns)
                listView1.Columns.Add(column.ColumnName);

            Int32 r = 0, c = 0;
            foreach (DataRow row in dt.Rows)
            {
                c = 0;
                listView1.Items.Add(dt.Rows[r][c].ToString());
                foreach (DataColumn column in dt.Columns)
                {
                    if (c == 0)
                    {
                        c++;
                        continue;
                    }
                    listView1.Items[r].SubItems.Add(row[c].ToString());
                    c++;
                }
                r++;
            }

            int columns_cnt = listView1.Columns.Count;
            for (c = 0; c < columns_cnt; c++)
                listView1.Columns[c].Width = -2;

            lblCount.Text = r + " record(s) found.";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            selectedItem = "";
        }

        public string selectedValue
        {
            get
            {
                return selectedItem;
            }
            set
            {
                sql = value;
                selectedItem = "";
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                selectedItem = listView1.FocusedItem.Text;
                Close();
            }
            else
            {
                MessageBox.Show(this, "Please select an item.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void listView1_MouseDoubleClick(object sender, System.EventArgs e)
        {
            selectedItem = listView1.FocusedItem.Text;
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
        
    }
}
