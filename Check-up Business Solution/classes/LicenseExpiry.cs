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
        string result_expiryDate;
        DateTime expiryDate = new DateTime();

        public LicenseExpiry()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT licenseExpiry FROM `terminal`", vars.MySqlConnection);
            result_expiryDate = (string)cmd.ExecuteScalar();
        }

        private bool decrypt(string result_expiry)
        {
            try
            {
                result_expiry = CryptorEngine.Decrypt(result_expiry);
                expiryDate = Convert.ToDateTime(result_expiry);
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
            if (result_expiryDate.Trim() == "")
                return false;
            else if (this.decrypt(result_expiryDate))
            {
                DateTime currentDate = DateTime.Today.Date;
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
