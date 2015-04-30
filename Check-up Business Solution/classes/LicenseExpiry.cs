using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Check_up.classes
{
    class LicenseExpiry
    {
        string str_expiryDate;
        DateTime expiryDate = new DateTime();

        public LicenseExpiry()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT licenseExpiry FROM `terminal`", vars.MySqlConnection);
            str_expiryDate = (string)cmd.ExecuteScalar();
        }

        private bool decrypt(string result_expiry)
        {
            try
            {
                str_expiryDate = CryptorEngine.Decrypt(result_expiry);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(Form.ActiveForm, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool expiredLicense()
        {
            if (String.IsNullOrEmpty(str_expiryDate) || str_expiryDate.Trim().Length == 0)
                return true;
            else if (string.Compare(str_expiryDate, "UNLIMITED", true) == 0)
                return false;
            else if (this.decrypt(str_expiryDate))
            {
                DateTime currentDate = DateTime.Today.Date;
                expiryDate = Convert.ToDateTime(str_expiryDate);
                int i = DateTime.Compare(expiryDate, currentDate);
                if (i < 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
