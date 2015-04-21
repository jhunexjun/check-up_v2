using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Check_up.classes
{
    class LicenseExpiry
    {
        private DateTime expiryDate = new DateTime();

        public LicenseExpiry()
        {
            string sql = "SELECT licenseExpiry FROM terminal";
            MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
            string result_expiryDate = (string)cmd.ExecuteScalar();
            result_expiryDate = CryptorEngine.Decrypt(result_expiryDate);
            expiryDate = Convert.ToDateTime(result_expiryDate);
        }

        public bool expiredLicense()
        {
            DateTime currentDate = DateTime.Today.Date;
            int i = DateTime.Compare(expiryDate, currentDate);
            if (i < 0)
                return true;
            else
                return false;
        }
    }
}
