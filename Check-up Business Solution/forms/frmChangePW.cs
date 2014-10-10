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
    public partial class frmChangePW : Form
    {
        string pw, salt, hash;
        private string mPW;

        public frmChangePW()
        {
            InitializeComponent();
        }

        private void frmChangePW_Load(object sender, EventArgs e)
        {

        }

        public string password
        {
            get
            {
                return hash;
            }
            set
            {
                mPW = value;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (checkValues() == true)
                Close();
        }

        private bool checkValues()
        {
            if (txtOld.Text.Trim() == "" || txtNew.Text.Trim() == "" || txtConfirm.Text.Trim() == "")
            {
                MessageBox.Show(this, "Please fill-up the form completely", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (txtOld.Text.Trim().Length < 5 || txtNew.Text.Trim().Length < 5 || txtConfirm.Text.Trim().Length < 5)
            {
                MessageBox.Show(this, "At least 5 characters are needed.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (txtNew.Text.Trim() != txtConfirm.Text.Trim())
            {
                MessageBox.Show(this, "New and confirm passwords do not match.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (BCrypt.CheckPassword(txtOld.Text.Trim() + "33~xX", mPW))
            {
                pw = txtNew.Text.Trim() + vars.staticSalt;
                salt = BCrypt.GenerateSalt();
                hash = BCrypt.HashPassword(pw, salt);
                this.password = hash;
                return true;
            }
            else
            {
                MessageBox.Show(this, "Old Password is invalid.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

    }
}
