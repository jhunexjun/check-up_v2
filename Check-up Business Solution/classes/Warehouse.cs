using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Check_up.classes
{
    public class Warehouse
    {
        private Hashtable formatParams(Hashtable ht)
        {
            if (ht.Contains("name"))
                ht["name"] = "'" + ht["name"] + "'";
            else
                ht["name"] = "null";

            if (ht.Contains("ftp_url"))
                ht["ftp_url"] = "'" + ht["ftp_url"] + "'";
            else
                ht["ftp_url"] = "null";

            if (ht.Contains("ftp_username"))
                ht["ftp_username"] = "'" + ht["ftp_username"] + "'";
            else
                ht["ftp_username"] = "null";

            if (ht.Contains("ftp_password"))
                ht["ftp_password"] = "'" + ht["ftp_password"] + "'";
            else
                ht["ftp_password"] = "null";

            if (ht.Contains("deactivated"))
                ht["deactivated"] = "'" + ht["deactivated"] + "'";
            else
                ht["deactivated"] = "null";

            if (!ht.Contains("createdBy"))
                ht["createdBy"] = "null";

            return ht;
        }

        public bool addWarehouse(Hashtable ht)
        {
            if (!ht.Contains("code"))
            {
                MessageBox.Show("Please indicate code in the hash.");
                return false;
            }

            if (!ht.Contains("branchType"))
            {
                MessageBox.Show("Please indicate branch type in the hash.");
                return false;
            }

            ht = formatParams(ht);

            string sql = "insert into warehouse(code,`name`,branchType,ftp_url,ftp_username,ftp_password,deactivated,createDate,createdBy)";
                    sql += " values('{0}', {1}, '{2}', {3}, {4}, {5}, {6}, DATE_FORMAT(now(), '%Y-%m-%d %H:%i:%s'), {7})";
                    sql = String.Format(sql, ht["code"], ht["name"], ht["branchType"], ht["ftp_url"], ht["ftp_username"], ht["ftp_password"], ht["deactivated"], ht["createdBy"]);

            MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
            if (cmd.ExecuteNonQuery() > 0)
                return true;
            else
                return false;
        }

        public bool updateWarehouse()
        {
            return false;
        }
    }
}
