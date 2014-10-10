using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Check_up.classes;
using System.Collections;

namespace Check_up.forms
{
    public partial class frmUsers : Form
    {
        DataTable table;
        private frmDialog frmDialogForm;
        bool changedPW;
        string picLocation, sql, password;
        string c, pw, salt, hash;

        public frmUsers()
        {
            InitializeComponent();
            txtFirst.TextChanged +=new EventHandler(txtFirst_TextChanged);
            txtMiddle.TextChanged += new EventHandler(txtMiddle_TextChanged);
            txtLast.TextChanged += new EventHandler(txtLast_TextChanged);
            txtEmail.TextChanged += new EventHandler(txtEmail_TextChanged);
            txtAddress.TextChanged += new EventHandler(txtAddress_TextChanged);
            cboGender.TextChanged +=new EventHandler(cboGender_TextChanged);
            cboRole.TextChanged += new EventHandler(cboRole_TextChanged);
            chkDeactivate.CheckedChanged += new EventHandler(checkBox1_CheckedChanged);
        }

        void cboRole_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtFirst_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void cboGender_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtMiddle_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtAddress_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtLast_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
            //throw new NotImplementedException();
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

        private void registerControls()
        {
            //let's register all controls and DB column names. If new control is added, probably it has to be added here.
            table = new DataTable();
            table.Columns.Add("controls", typeof(string));     //check txt* name
            table.Columns.Add("DBcolumnName", typeof(string)); //check db column name
            table.Columns.Add("Value", typeof(string));        //value of txt*.text

            table.Rows.Add("txtUsername", "username", "");
            table.Rows.Add("txtFirst", "fName", "");
            table.Rows.Add("txtMiddle", "midName", "");
            table.Rows.Add("txtLast", "lName", "");
            table.Rows.Add("txtEmail", "email", "");
            table.Rows.Add("txtAddress", "address", "");
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (btnFind.Text == "&Find")
            {
                sql = "SELECT username Username,password,fName'First name',midName 'Middle',lName 'Last name',email,address,gender,picLocation,deactivated,role  FROM users WHERE ";

                if (txtUsername.Text.Trim() != "" && !txtUsername.Text.Contains("*")) //txtUsername = "admin"
                {
                    sql += "username='" + txtUsername.Text.Trim() + "'";
                    database db = new database();
                    DataTable dt = new DataTable();
                    dt = db.select(sql, vars.MySqlConnection);
                    if (dt.Rows.Count < 1)
                    {
                        MessageBox.Show(this, "No matching records found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    txtFirst.Text = dt.Rows[0]["First name"].ToString();
                    txtMiddle.Text = dt.Rows[0]["Middle"].ToString();
                    txtLast.Text = dt.Rows[0]["Last name"].ToString();
                    txtEmail.Text = dt.Rows[0]["email"].ToString();
                    txtAddress.Text = dt.Rows[0]["address"].ToString();
                    cboGender.Text = dt.Rows[0]["gender"].ToString();
                    picLocation = dt.Rows[0]["picLocation"].ToString();
                    pictureBox1.ImageLocation = picLocation;
                    txtPassword.Text = dt.Rows[0]["password"].ToString();
                    password = dt.Rows[0]["password"].ToString();
                    if (dt.Rows[0]["deactivated"].ToString() == "Y")
                        chkDeactivate.Checked = true;
                    else
                        chkDeactivate.Checked = false;
                    cboRole.Text = convertRole.role(Convert.ToInt16(dt.Rows[0]["role"]));
                    
                    if (vars.role == 0 || txtUsername.Text.Trim() == vars.username)
                    {
                        linkChangePic.Enabled = true;
                        linkPassword.Enabled = true;
                    }

                    txtUsername.ReadOnly = true;
                    txtPassword.ReadOnly = true;
                    btnFind.Text = "&OK";
                }
                else
                {
                    registerControls();
                    bool found; found = false;

                    foreach (Control current_control in this.Controls)
                    {
                        if (current_control is TextBox && current_control.Text.Contains("*") && current_control.Name != txtPassword.Name)
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
                    txtUsername.Text = frmDialogForm.selectedValue;
                }
            }
            else if(btnFind.Text == "&OK") {
                this.Close();
            }
            else if (btnFind.Text == "&Update")
            {
                if (checkValues())
                {
                    pw = txtPassword.Text.Trim() + vars.staticSalt;
                    salt = BCrypt.GenerateSalt();
                    hash = BCrypt.HashPassword(pw, salt);

                    Hashtable ht = new Hashtable();
                    ht.Add("username", txtUsername.Text.Trim());
                    ht.Add("password", hash);
                    ht.Add("fName", txtFirst.Text.Trim());
                    ht.Add("midName", txtMiddle.Text.Trim());
                    ht.Add("lName", txtLast.Text.Trim());
                    ht.Add("email", txtEmail.Text.Trim());
                    ht.Add("address", txtAddress.Text.Trim());
                    ht.Add("gender", cboGender.Text.Trim());

                    string c = (chkDeactivate.Checked == true) ? "Y" : "N";
                    ht.Add("deactivated", c);

                    picLocation = picLocation.Replace(@"\", @"\\");
                    ht.Add("picLocation", picLocation);
                    ht.Add("role", convertRole.role(cboRole.Text));
                    ht.Add("createdBy", vars.user_id);

                    if (changedPW)
                        ht.Add("changePassword", true);

                    Users user = new Users();
                    if (user.updateUser(ht))
                    {
                        MessageBox.Show(this, "Saving has been successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cleanUpUI();
                        btnFind.Text = "&Find";
                    }
                }
            }
            else if (btnFind.Text == "&Save")
            {
                if (checkValues())
                {
                    if(MessageBox.Show(this, "Are you sure you want to save this?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    pw = txtPassword.Text.Trim() + vars.staticSalt;
                    salt = BCrypt.GenerateSalt();
                    hash = BCrypt.HashPassword(pw, salt);

                    Hashtable ht = new Hashtable();
                    ht.Add("username", txtUsername.Text.Trim());
                    ht.Add("password", hash);
                    ht.Add("fName", txtFirst.Text.Trim());
                    ht.Add("midName", txtMiddle.Text.Trim());
                    ht.Add("lName", txtLast.Text.Trim());
                    ht.Add("email", txtEmail.Text.Trim());
                    ht.Add("address", txtAddress.Text.Trim());
                    ht.Add("gender", cboGender.Text.Trim());

                    c = (chkDeactivate.Checked == true) ? "Y" : "N";
                    ht.Add("deactivated", c);
                    ht.Add("picLocation", picLocation);
                    ht.Add("role", convertRole.role(cboRole.Text));
                    ht.Add("createdBy", vars.user_id);

                    Users user = new Users();
                    if (user.addUser(ht))
                    {
                        MessageBox.Show(this, "Saving has been successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cleanUpUI();
                        btnFind.Text = "&Find";
                    }
                }
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtUsername.ReadOnly = false;
            txtPassword.ReadOnly = false;
            linkPassword.Enabled = false;
            linkChangePic.Enabled = true;
            btnFind.Text = "&Save";
            cleanUpUI();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            /* if (vars.role > 0) //0=admin.
                addToolStripMenuItem.Enabled = false;

            if (btnFind.Text != "&OK")
                editToolStripMenuItem.Enabled = false;

            if (this.btnFind.Text == "&OK")
            {
                if (txtUsername.Text.Trim() != vars.username && vars.role > 0)
                    editToolStripMenuItem.Enabled = false;
                else
                    editToolStripMenuItem.Enabled = true;
            } */

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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpeg)|*.jpg";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (picLocation != ofd.FileName)
                {
                    picLocation = ofd.FileName;
                    pictureBox1.Load(picLocation);
                    //if ((btnFind.Text == "&OK") && vars.username == txtUsername.Text.Trim())
                    //    btnFind.Text = "&Update";
                    editMode();
                }                
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtUsername.ReadOnly = true;
            txtPassword.ReadOnly = true;
            linkPassword.Enabled = true;
            linkChangePic.Enabled = true;
            btnFind.Text = "&Update";
        }

        private void linkPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmChangePW frmChangePW = new frmChangePW();
            frmChangePW.password = password;
            frmChangePW.ShowDialog();
            if (frmChangePW.password != "")
            {
                txtPassword.Text = frmChangePW.password;
                changedPW = true;
            }
            //btnFind.Text = "&Update";
            editMode();
        }

        private void cleanUpUI()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtFirst.Text = "";
            txtMiddle.Text = "";
            txtLast.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            cboGender.SelectedItem = "M";
            pictureBox1.Image = null;
            linkPassword.Enabled = false;
            linkChangePic.Enabled = false;
            txtUsername.ReadOnly = false;
            txtPassword.ReadOnly = false;
            chkDeactivate.Checked = false;
            cboRole.SelectedItem = "User";
            txtUsername.Focus();
        }

        private bool checkValues()
        {
            if (txtUsername.Text.Trim() == "" || txtPassword.Text.Trim() == "" || cboGender.Text.Trim() == "")
            {
                MessageBox.Show(this, "Please indicate username, password, and gender.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        private void editMode()
        {
            if (vars.role == 0) //if Superuser
                btnFind.Text = "&Update";
            else
                if (txtUsername.Text.Trim() == vars.username)
                    btnFind.Text = "&Update";
        }
    }
}
