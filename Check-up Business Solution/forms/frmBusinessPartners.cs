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
    public partial class frmBusinessPartners : Form
    {
        DataTable table;
        private frmDialog frmDialogForm;
        string sql;

        public frmBusinessPartners()
        {
            InitializeComponent();
            txtName.TextChanged += new EventHandler(txtName_TextChanged);
            txtAddress.TextChanged += new EventHandler(txtAddress_TextChanged);
            txtTel1.TextChanged += new EventHandler(txtTel1_TextChanged);
            txtTel2.TextChanged += new EventHandler(txtTel2_TextChanged);
            txtFaxNo.TextChanged += new EventHandler(txtFaxNo_TextChanged);
            txtEmail.TextChanged += new EventHandler(txtEmail_TextChanged);
            txtWebsite.TextChanged += new EventHandler(txtWebsite_TextChanged);
            txtContactP.TextChanged += new EventHandler(txtContactP_TextChanged);
            txtRemarks.TextChanged += new EventHandler(txtRemarks_TextChanged);
            chkDeactivate.CheckedChanged += new EventHandler(chkDeactivate_CheckedChanged);
        }

        void chkDeactivate_CheckedChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtRemarks_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtContactP_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtWebsite_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtFaxNo_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtTel2_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtTel1_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtAddress_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtName_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        private void frmBusinessPartners_Load(object sender, EventArgs e)
        {

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (btnFind.Text == "&Find")
            {
                sql = "SELECT code,BPType,BPname,address,tel1,tel2,fax,email,website,contactP,deactivated,remarks FROM businesspartner WHERE ";

                if (txtBPCode.Text.Trim() != "" && !txtBPCode.Text.Contains("*"))
                {
                    sql += "code='" + txtBPCode.Text.Trim().Replace("'","''") + "'";
                    database db = new database();
                    DataTable dt = new DataTable();
                    dt = db.select(sql, vars.MySqlConnection);
                    if (dt.Rows.Count < 1)
                    {
                        MessageBox.Show(this, "No matching record found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    txtBPCode.Text = dt.Rows[0]["code"].ToString();
                    txtName.Text = dt.Rows[0]["BPName"].ToString();
                    txtAddress.Text = dt.Rows[0]["address"].ToString();
                    txtTel1.Text = dt.Rows[0]["tel1"].ToString();
                    txtTel2.Text = dt.Rows[0]["tel2"].ToString();
                    txtFaxNo.Text = dt.Rows[0]["fax"].ToString();
                    txtEmail.Text = dt.Rows[0]["email"].ToString();
                    txtWebsite.Text = dt.Rows[0]["website"].ToString();
                    txtContactP.Text = dt.Rows[0]["contactP"].ToString();
                    txtRemarks.Text = dt.Rows[0]["remarks"].ToString();
                    if (dt.Rows[0]["deactivated"].ToString() == "Y")
                        chkDeactivate.Checked = true;
                    else
                        chkDeactivate.Checked = false;
                    //if (Convert.ToInt16(dt.Rows[0]["BPType"]) == 0)
                    cboBPType.SelectedIndex = Convert.ToInt16(dt.Rows[0]["BPType"]);

                    /* if (vars.role > 0)
                    {
                        txtWhCode.ReadOnly = true;
                        txtName.ReadOnly = true;
                    } */
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
                    txtBPCode.Text = frmDialogForm.selectedValue;
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
                    sql = "UPDATE businesspartner SET BPname='" + txtName.Text.Trim().Replace("'","''") + "',address='" + txtAddress.Text.Trim().Replace("'","''") + "',tel1='" + txtTel1.Text.Trim() + "',tel2='" + txtTel2.Text.Trim() + "',fax='" + txtFaxNo.Text.Trim() + "',email='" + txtEmail.Text.Trim() + "',website='" + txtWebsite.Text.Trim() + "',contactP='" + txtContactP.Text.Trim() + "',remarks='" + txtRemarks.Text.Trim().Replace("'","''") + "',deactivated='" + c + "',updateDate=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s'),updatedBy='" + vars.username + "' WHERE code='" + txtBPCode.Text.Trim() + "'";

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

                    string c; int type;
                    c = (chkDeactivate.Checked == true) ? "Y" : "N";
                    type = BPCode(cboBPType.Text);
                    sql = "INSERT INTO businesspartner(code,BPType,BPname,address,tel1,tel2,fax,email,website,contactP,deactivated,remarks,createDate,createdBy) ";
                    sql += "Values('" + txtBPCode.Text.Trim() + "'," + type + ",'" + txtName.Text.Trim().Replace("'","''") + "','" + txtAddress.Text.Trim().Replace("'","''") + "','" + txtTel1.Text.Trim() + "','" + txtTel2.Text.Trim() + "','" + txtFaxNo.Text.Trim() + "','" + txtEmail.Text.Trim() + "','" + txtWebsite.Text.Trim() + "','" + txtContactP.Text.Trim() + "','" + c + "','" + txtRemarks.Text.Trim().Replace("'","''") + "',DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s'),'" + vars.username + "')";
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

            table.Rows.Add("txtBPCode", "code", "");
            table.Rows.Add("txtName", "BPName", "");
            table.Rows.Add("txtAddress", "address", "");
            table.Rows.Add("txtTel1", "tel1", "");
            table.Rows.Add("txtTel2", "tel2", "");
            table.Rows.Add("txtFaxNo", "fax", "");
            table.Rows.Add("txtEmail", "email", "");
            table.Rows.Add("txtWebsite", "website", "");
            table.Rows.Add("txtContactP", "contactP", "");
            table.Rows.Add("txtRemarks", "name", "");
        }

        private bool checkValues()
        {
            if (txtBPCode.Text.Trim() == "")
            {
                MessageBox.Show(this, "Please indicate business partner code.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBPCode.Focus();
                return false;
            }
            if (cboBPType.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Please indicate business partner type.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboBPType.Focus();
                return false;
            }
            if (txtBPCode.Text.Contains("'"))
            {
                MessageBox.Show(this, "Invalid character.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtBPCode.Focus();
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
            txtBPCode.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtTel1.Text = "";
            txtTel2.Text = "";
            txtFaxNo.Text = "";
            txtEmail.Text = "";
            txtWebsite.Text = "";
            txtContactP.Text = "";
            txtRemarks.Text = "";
            cboBPType.SelectedIndex = -1;
            chkDeactivate.Checked = false;
            txtBPCode.ReadOnly = false;
            txtBPCode.Focus();
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

        private int BPCode(string s)
        {
            if (s == "Supplier")
            {
                return 0;
            }
            else if (s == "Customer")
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        private string BPCode(int i)
        {
            if (i == 0)
                return "Supplier";
            else if (i == 1)
                return "Customer";
            else
                return "";
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFind.Text = "&Save";
            cleanUpUI();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFind.Text = "&Update";
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (vars.role > 0) //0=admin.
            {
                addToolStripMenuItem.Enabled = false;
                editToolStripMenuItem.Enabled = false;
            }

            if (btnFind.Text == "&Find")
            {
                addToolStripMenuItem.Enabled = true;
                editToolStripMenuItem.Enabled = false;
            }
            else if (btnFind.Text == "&Save" || btnFind.Text == "&Update")
            {
                addToolStripMenuItem.Enabled = false;
                editToolStripMenuItem.Enabled = false;
            }
            else // == "&OK"
            {
                addToolStripMenuItem.Enabled = true;
                editToolStripMenuItem.Enabled = true;
            }
        }
    }
}
