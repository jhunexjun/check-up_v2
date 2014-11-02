using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.IO;

namespace Check_up.forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string pattern = @"^[a-zA-Z0-9_-]{3,16}$";
            if (matchRegEx(txtUsername.Text.Trim(), pattern) && matchRegEx(txtPassword.Text.Trim(), pattern))
            {
                vars.db_credentials db_con = new vars.db_credentials();

                try
                {
                    using (StreamReader sr = new StreamReader("check-up.ini"))
                    {
                        string line; int position;

                        while ((line = sr.ReadLine()) != null)
                        {
                            position = line.IndexOf("=");
                            if (line.StartsWith("datasource"))
                                db_con.server = line.Substring(position + 1);
                            if (line.StartsWith("database"))
                                db_con.database = line.Substring(position + 1);
                            if (line.StartsWith("username"))
                                db_con.username = line.Substring(position + 1);
                            if (line.StartsWith("password"))
                            {
                                db_con.password = line.Substring(position + 1);
                                db_con.password = CryptorEngine.Decrypt(db_con.password);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    txtUsername.Focus();
                }


                string connectionString = "SERVER=" + db_con.server + ";DATABASE=" + db_con.database + ";UID=" + db_con.username + ";PASSWORD=" + db_con.password + ";Allow User Variables=True";
                vars.MySqlConnection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);

                bool connected = false;
                try
                {
                    vars.MySqlConnection.Open();
                    connected = true;
                }
                catch (MySqlException err)
                {
                    MessageBox.Show(err.Number + ": " + err.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUsername.Focus();
                }
                                
                if (connected == true)
                {
                    database query = new database();
                    string sql = "SET @username='" + txtUsername.Text.Trim() + "';";
                    sql += "SELECT username,password,deactivated,role FROM users where username=@username;";
                    DataTable dt = new DataTable();
                    dt = query.select(sql, vars.MySqlConnection);
                    
                    if (dt.Rows.Count == 1)
                    {
                        if (dt.Rows[0]["deactivated"].ToString() == "Y")
                        {
                            MessageBox.Show(this, "Account is deactivated.", "Message", MessageBoxButtons.OK,MessageBoxIcon.Information);
                            return;
                        }

                        string pw; bool doesPwMatched;
                        pw = txtPassword.Text + vars.staticSalt;
                        try
                        {
                            doesPwMatched = BCrypt.CheckPassword(pw, dt.Rows[0]["password"].ToString());
                        }
                        catch
                        {
                            MessageBox.Show("Invalid username and/or password.", "Log-in", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        if (!doesPwMatched)
                        {
                            MessageBox.Show("Invalid username and/or password.", "Log-in", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        vars.username = txtUsername.Text.Trim();
                        sql = "UPDATE users SET lastLogIn = DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s') WHERE username = '" + dt.Rows[0][0] + "'";
                        query.executeNonQuery(sql, vars.MySqlConnection);
                        vars.loggedOn = true;
                        vars.role = Convert.ToInt16(dt.Rows[0]["role"]);
                        vars.username = dt.Rows[0]["username"].ToString();

                        sql = "SELECT terminalId FROM terminal LIMIT 1";
                        dt = new DataTable();
                        dt = query.select(sql, vars.MySqlConnection);
                        if ( dt.Rows.Count < 1)
                        {
                            MessageBox.Show(this, "Terminal ID not found.", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return;
                        }

                        vars.terminalId = dt.Rows[0][0].ToString();

                        ToolStripMenuItem tsm;
                        tsm = (ToolStripMenuItem)this.MdiParent.MainMenuStrip.Items[0];
                        tsm.DropDownItems[0].Text = "&Log-out";
                        this.MdiParent.MainMenuStrip.Items["modulesToolStripMenuItem"].Enabled = true;
                        this.MdiParent.MainMenuStrip.Items["toolsToolStripMenuItem"].Enabled = true;                        

                        this.Close();
                    }
                    else    //Shouldn't accept more than 1 value for security reasons
                    {
                        MessageBox.Show("Invalid username and/or password.", "Log-in", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid username and/or password.", "Log-in", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtUsername.Focus();
        }

        private bool matchRegEx(string text, string pattern)
        {
            Regex r = new Regex(pattern);
            Match m = r.Match(text);
            if (m.Success)
            {
                return true;
            }
            return false;

        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            lblProductName.Text = Application.ProductName + " 2014";
            lblVersion.Text = "Version " + Application.ProductVersion;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form f = new frmDBConfig();
            f.ShowDialog();
        }
    }
}
