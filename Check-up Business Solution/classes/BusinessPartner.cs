using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Check_up.classes
{
    public class BusinessPartner
    {
        private bool CheckBPType(Hashtable ht)
        {
            if (!ht.Contains("BPType"))
            {
                MessageBox.Show("Please indicate BP Type either 0 or 1 for supplier and customer respectively.");
                return false;
            }

            int[] BPType = {0, 1};

            if (!BPType.Contains(Int32.Parse(ht["BPType"].ToString())))
            {
                MessageBox.Show("Please indicate BP Type either 0 or 1 for supplier and customer respectively.");
                return false;
            }

            return true;
        }

        private Hashtable formatParams(Hashtable ht)
        {
            if (!CheckBPType(ht))
                return null;

            if (ht.Contains("BPName"))
                ht["BPName"] = "'" + ht["BPName"] + "'";
            else
                ht["BPName"] = "null";

            if (ht.Contains("address"))
                ht["address"] = "'" + ht["address"] + "'";
            else
                ht["address"] = "null";

            if (ht.Contains("tel1"))
                ht["tel1"] = "'" + ht["tel1"] + "'";
            else
                ht["tel2"] = "null";

             if (ht.Contains("tel2"))
                ht["tel2"] = "'" + ht["tel2"] + "'";
            else
                ht["tel2"] = "null";

            if (ht.Contains("fax"))
                ht["fax"] = "'" + ht["fax"] + "'";
            else
                ht["fax"] = "null";

            if (ht.Contains("email"))
                ht["email"] = "'" + ht["email"] + "'";
            else
                ht["email"] = "null";

            if (ht.Contains("website"))
                ht["website"] = "'" + ht["website"] + "'";
            else
                ht["website"] = "null";

            if (ht.Contains("contactPerson"))
                ht["contactPerson"] = "'" + ht["contactPerson"] + "'";
            else
                ht["contactPerson"] = "null";

            if (ht.Contains("deactivated"))
                ht["deactivated"] = "'" + ht["deactivated"] + "'";
            else
                ht["deactivated"] = "null";

            if (ht.Contains("remarks"))
                ht["remarks"] = "'" + ht["remarks"] + "'";
            else
                ht["remarks"] = "null";

            if (!ht.Contains("createdBy"))
                ht["createdBy"] = 0; // assume the superuser created this

            return ht;
        }

        public bool addBusinessPartner(Hashtable ht)
        {
            if (!ht.Contains("code"))
            {
                MessageBox.Show("Please indicate code in the hash table.");
                return false;
            }
            if (!ht.Contains("BPType"))
            {
                MessageBox.Show("Please indicate BP type in the hash table.");
                return false;
            }           

            ht = formatParams(ht);
            if (ht == null)
                return false;

            string sql = "insert into businesspartner(BPType,`code`,BPName,address,tel1,tel2,fax,email,website,contactP,deactivated,remarks,createDate,createdBy)";
            sql += " values({0}, '{1}', {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, DATE_FORMAT(now(), '%Y-%m-%d %H:%i:%s'), {12})";
            sql = String.Format(sql, ht["BPType"], ht["code"], ht["BPName"], ht["address"], ht["tel1"], ht["tel2"], ht["fax"], ht["email"], ht["website"], ht["contactPerson"], ht["deactivated"], ht["remarks"], ht["createdBy"]);

            MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
            if (cmd.ExecuteNonQuery() > 0)
                return true;
            else
                return false;
        }

        public bool updateBusinessPartner(Hashtable ht)
        {
            return false;
        }
    }
}
