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
    public partial class frmWarehouse : Form
    {
        DataTable table;
        private frmDialog frmDialogForm;
        string sql; int id = 0;

        public frmWarehouse()
        {
            InitializeComponent();
            txtWhCode.TextChanged += new EventHandler(txtWhCode_TextChanged);
            txtName.TextChanged += new EventHandler(txtName_TextChanged);
            chkDeactivate.CheckedChanged += new EventHandler(chkDeactivate_CheckedChanged);
        }

        private void frmWarehouse_Load(object sender, EventArgs e)
        {
        }

        void chkDeactivate_CheckedChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtName_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtWhCode_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (btnFind.Text == "&Find")
            {                
                if (txtWhCode.Text.Trim() != "" && !txtWhCode.Text.Contains("*"))
                {
                    sql = sql = "SELECT id,code,name,deactivated FROM warehouse WHERE code='" + txtWhCode.Text.Trim() + "'";
                    database db = new database();
                    DataTable dt = new DataTable();
                    dt = db.select(sql, vars.MySqlConnection);
                    if (dt.Rows.Count < 1)
                    {
                        MessageBox.Show(this, "No matching records found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    id = Convert.ToInt32(dt.Rows[0]["id"]);
                    txtWhCode.Text = dt.Rows[0]["code"].ToString();
                    txtName.Text = dt.Rows[0]["name"].ToString();
                    
                    if (dt.Rows[0]["deactivated"].ToString() == "Y")
                        chkDeactivate.Checked = true;
                    else
                        chkDeactivate.Checked = false;

                    if (vars.role > 0)
                    {
                        txtWhCode.ReadOnly = true;
                        txtName.ReadOnly = true;
                    }
                    btnFind.Text = "&OK";
                }
                else
                {
                    registerControls();
                    bool found = false;

                    foreach (Control current_control in this.Controls)
                    {
                        if (current_control is TextBox && current_control.Text.Contains("*"))
                        {
                            foreach (DataRow row in table.Rows)
                            {
                                if ((string)row["controls"] == current_control.Name.ToString() && current_control.Text != "")
                                {
                                    row["Value"] = current_control.Text; //insert the value if current_control is not ""
                                    found = true;
                                }
                            }
                        }
                    }

                    if (found == false)
                    {
                        MessageBox.Show(this, "No matching records found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        sql = "SELECT code,name,deactivated FROM warehouse WHERE ";
                        foreach (DataRow row in table.Rows)
                        {
                            if ((string)row["Value"] != "")
                                sql += row["DBcolumnName"] + " like '" + row["Value"].ToString().Replace("*", "%") + "'   AND ";
                        }
                    }

                    sql = sql.Remove(sql.Length - 7);
                    frmDialogForm = new frmDialog();
                    frmDialogForm.selectedValue = sql;
                    frmDialogForm.ShowDialog();
                    txtWhCode.Text = frmDialogForm.selectedValue;
                }
            }
            else if (btnFind.Text == "&OK")
            {
                this.Close();
            }
            else if (btnFind.Text == "&Update")
            {
                if (checkValues())
                {
                    string c = chkDeactivate.Checked == true ? "Y" : "N";

                    database db = new database();
                    sql = "UPDATE warehouse SET code='" + txtWhCode.Text.Trim() + "',name='" + txtName.Text.Trim() + "',updateDate=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s'),updatedBy=" + vars.user_id + ",deactivated='" + c + "' WHERE id=" + id;

                    if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                    {
                        MessageBox.Show(this, "Updating has been successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnFind.Text = "&OK";
                    }
                }
            }
            else if (btnFind.Text == "&Save")
            {
                if (checkValues())
                {
                    if (MessageBox.Show(this, "Are you sure you want to save this?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    string c;
                    c = (chkDeactivate.Checked == true) ? "Y" : "N";
                    sql = "INSERT INTO warehouse(code,name,deactivated,createDate,createdBy) ";
                    sql += "Values('" + txtWhCode.Text.Trim() + "','" + txtName.Text.Trim().Replace("'","''") + "','" + c + "',DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s')," + vars.user_id + ")";
                    database db = new database();
                    if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                    {
                        MessageBox.Show(this, "Saving has been successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cleanUpUI();
                        btnFind.Text = "&Find";
                    }
                }
            }
        }

        private void registerControls()
        {
            //let's register all controls and DB column names. If new control is added, probably it has to be added here.
            table = new DataTable();
            table.Columns.Add("controls", typeof(string));     //check txt* name
            table.Columns.Add("DBcolumnName", typeof(string)); //check db column name
            table.Columns.Add("Value", typeof(string));        //value of txt*.text

            table.Rows.Add("txtWhCode", "code", "");
            table.Rows.Add("txtName", "name", "");
        }

        private bool checkValues()
        {
            if (txtWhCode.Text.Trim() == "")
            {
                MessageBox.Show(this, "Please indicate warehouse code.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        private void editMode()
        {
            if (vars.role == 0) //if Superuser
                btnFind.Text = "&Update";
        }

        private void cleanUpUI()
        {
            txtWhCode.Text = "";
            txtName.Text = "";
            txtWhCode.ReadOnly = false;
            txtName.ReadOnly = false;
            txtWhCode.Focus();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFind.Text = "&Update";
        }

        private void addToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            btnFind.Text = "&Save";
            cleanUpUI();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (vars.role > 0) //0=admin.
            {
                addToolStripMenuItem.Enabled = false;
                editToolStripMenuItem.Enabled = false;
            }

            if (btnFind.Text != "&OK")
                editToolStripMenuItem.Enabled = false;
        }

        private void editToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            btnFind.Text = "&Update";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnFind.Text != "&Find")
            {
                cleanUpUI();
                btnFind.Text = "&Find";
            }
            else
                Close();
        }
    }
}
