using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;

namespace Check_up.classes
{
    public class Users
    {
        private Hashtable formatParams(Hashtable ht)
        {
            if (ht.Contains("fName"))
                ht["fName"] = "'" + ht["fName"] + "'";
            else
                ht["fName"] = "null";

            if (ht.Contains("midName"))
                ht["midName"] = "'" + ht["midName"] + "'";
            else
                ht["midName"] = "null";

            if (ht.Contains("lName"))
                ht["lName"] = "'" + ht["lName"] + "'";
            else
                ht["lName"] = "null";

            if (ht.Contains("email"))
                ht["email"] = "'" + ht["email"] + "'";
            else
                ht["email"] = "null";

            if (ht.Contains("address"))
                ht["address"] = "'" + ht["address"] + "'";
            else
                ht["address"] = "null";

            if (ht.Contains("gender"))
                ht["gender"] = "'" + ht["gender"] + "'";
            else
                ht["gender"] = "null";

            if (ht.Contains("deactivated"))
                ht["deactivated"] = "'" + ht["deactivated"] + "'";
            else
                ht["deactivated"] = "null";

            if (ht.Contains("picLocation"))
                ht["picLocation"] = "'" + ht["picLocation"].ToString().Replace(@"\", @"\\") + "'";
            else
                ht["picLocation"] = "null";

            // default role to 0 if not set
            if (!ht.Contains("role"))
                ht["role"] = 0;

            // default created by to 0 if not set. This should be not null. 0 means the primary/default username
            if (!ht.Contains("createdBy"))
                ht["createdBy"] = "0";

            if (ht.Contains("updatedBy"))
                ht["updatedBy"] = "'" + ht["updatedBy"] + "'";

            return ht;
        }

        public bool addUser(Hashtable ht)
        {
            ht = formatParams(ht);

            string sql = "insert into users(username,password,fName,midName,lName,email,address,gender,createDate,deactivated,picLocation,role,createdBy)";
            sql += "values('{0}','{1}',{2},{3},{4},{5},{6},{7}, DATE_FORMAT(now(), '%Y-%m-%d %H:%i:%s'),{8},{9},{10},'{11}')";
            sql = String.Format(sql, ht["username"], ht["password"], ht["fName"], ht["midName"], ht["lName"], ht["email"], ht["address"], ht["gender"], ht["deactivated"], ht["picLocation"], ht["role"], ht["createdBy"]);

            MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
            if (cmd.ExecuteNonQuery() > 0)
                return true;
            else
                return false;
        }

        public bool updateUser(Hashtable ht)
        {
            if (!ht.Contains("updatedBy"))
            {
                MessageBox.Show("Please indicate 'updated by' in the hash.");
                return false;
            }

            ht = formatParams(ht);

            int x = 10;
            string sql = "update users SET fName={0},midName={1},lName={2},email={3},address={4},gender={5},updateDate=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s'),picLocation={6},deactivated={7},role={8}, updatedBy={9}";
            if (ht.Contains("changePassword"))
            {
                sql += ",password='{" + x + "}'";
                x += 1;
            }
            sql += " where username='{" + x + "}' limit 1";

            if (ht.Contains("changePassword"))
                sql = String.Format(sql, ht["fName"], ht["midName"], ht["lName"], ht["email"], ht["address"], ht["gender"], ht["picLocation"], ht["deactivated"], ht["role"], ht["updatedBy"], ht["password"], ht["username"]);
            else
                sql = String.Format(sql, ht["fName"], ht["midName"], ht["lName"], ht["email"], ht["address"], ht["gender"], ht["picLocation"], ht["deactivated"], ht["role"], ht["updatedBy"], ht["username"]);

            MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
            if (cmd.ExecuteNonQuery() > 0)
                return true;
            else
                return false;
        }

        public DataTable selectUser(Hashtable ht)
        {
            if (!ht.Contains("username"))
            {
                MessageBox.Show("No 'username' in the hash.");
                return null;
            }

            string sql = "SELECT username Username,password,fName'First name',midName 'Middle',lName 'Last name',email,address,gender,picLocation,deactivated,role";
                sql += "FROM users WHERE username='{0}'";

            sql = String.Format(sql, ht["username"]);
            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }
        
    }
}
