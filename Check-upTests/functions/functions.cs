using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Collections;
using Check_up;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Check_upTests
{
    public static class functions
    {
        private static string path = "";
        private static Hashtable ht;

        //used to read the database.ini file for the database login
        public static Hashtable readDbConfigFile()
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

                Hashtable ht = new Hashtable();
                ht.Add("datasource", db_con.server);
                ht.Add("database", db_con.database);
                ht.Add("username", db_con.username);
                ht.Add("password", db_con.password);

                return ht;
            }
            catch
            {
                return readDbConfigFile();
            }
        }

        //we need to make sure that a fresh new copy of database schema is used.
        public static bool dropAndCreateDatabase(Hashtable hashTable)
        {
            string connectionString = "SERVER=" + ht["datasource"] + ";UID=" + ht["username"] + ";PASSWORD=" + ht["password"] + ";Allow User Variables=True";
            vars.MySqlConnection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);

            try
            {
                vars.MySqlConnection.Open();
                string query = "drop database if exists `" + ht["database"] + "`;";
                query += "create database `" + ht["database"].ToString() + "`;";
                MySqlCommand cmd = new MySqlCommand(query, vars.MySqlConnection);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }

        public static bool dumpDatabase(Hashtable hashTable)
        {
            ht = hashTable;
            path = @"C:\\Users\\Jhunex\\Documents\\Visual Studio 2013\\Projects\\pmorcilladev\\Check-up Business Solution\\databases\\0schema.sql";
            
            StreamWriter file = new StreamWriter(path);
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.FileName = "C:\\xampp\\mysql\\bin\\mysqldump";
            proc.RedirectStandardInput = false;
            proc.RedirectStandardOutput = true;
            string cmd = string.Format(@" -d -u{0} -p{1} -h{2} {3}", ht["username"], ht["password"], ht["datasource"], "check-up_pmorcilladev");
            proc.Arguments = cmd;
            proc.UseShellExecute = false;
            proc.CreateNoWindow = true;
            Process p = Process.Start(proc);
            string res = p.StandardOutput.ReadToEnd();

            string pattern = " AUTO_INCREMENT=[0-9]+";
            string replacement = "";
            Regex regEx = new Regex(pattern);
            res = regEx.Replace(res, replacement);

            file.Write(res);
            p.WaitForExit();
            file.Close();
            p.Close();
            
            return true;
        }

        //this recreates database.
        public static bool reloadDatabase() {
            string query;

            using (StreamReader sr = new StreamReader(path))
                query = sr.ReadToEnd();

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, vars.MySqlConnection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
            
        }

        //before the system is used, default values are inserted into the db just like default login
        public static bool createInitialRecordsForTheTests()
        {
            string sql = "insert into warehouse(code,`name`,branchType, deactivated, createdBy) values('MAIN', 'Main branch', 'Main', 'N', 'admin');";
            sql += "insert into terminal values('MAIN', 'MAIN', '192.168.0.100', 'MAIN', 'Main');";
            sql += "insert into documents(documentCode,documentName,lastNo) values('PO', 'Purchase Order', 0)";
            sql += " ,('SI', 'Sales Invoice', 0)";
            sql += " ,('GRPO', 'Goods Receipt PO', 0)";
            sql += " ,('IMD', 'Item Master Data', 0)";
            sql += " ,('SR', 'Sales Return', 0)";
            sql += " ,('GR', 'Goods Return', 0)";
            sql += " ,('IT', 'Inventory Transfer', 0)";
            sql += " ,('DR', 'Delivery Receipt', 0)";
            sql += " ,('IP', 'Inventory Posting', 0);";

            MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
            if (cmd.ExecuteNonQuery() > 0)
                return true;
            else
                return false;
        }

        //this is similar as createDefaultRecordsForTheTests() but for imports coming to branch.
        public static bool createInitialRecordsForTheTestsForBranch()
        {
            string sql = "insert into warehouse(code,`name`,branchType,deactivated,createdBy) values('MAIN', 'Main branch', 'Main', 'N', 'admin')";
            sql += ",('BRANCH1','Branch One','Branch','N','admin');";
            sql += "insert into terminal values('BRANCH1', 'BRANCH1', '192.168.0.101', 'BRANCH1', 'Branch');";
            sql += "insert into documents(documentCode,documentName,lastNo) values('PO', 'Purchase Order', 0)";
            sql += " ,('SI', 'Sales Invoice', 0)";
            sql += " ,('GRPO', 'Goods Receipt PO', 0)";
            sql += " ,('IMD', 'Item Master Data', 0)";
            sql += " ,('SR', 'Sales Return', 0)";
            sql += " ,('GR', 'Goods Return', 0)";
            sql += " ,('IT', 'Inventory Transfer', 0)";
            sql += " ,('DR', 'Delivery Receipt', 0)";
            sql += " ,('IP', 'Inventory Posting', 0);";

            // 1 records has already been added by [TestInitialize]
            //sql += "insert into users(username,`password`)";
            //sql += " values('admin', '$2a$10$5gcpxMRDi2wKdZkwaZ.G/uwJeFpPLw6RVnIKCy48haolWoVvqiKJy');";

            MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
            if (cmd.ExecuteNonQuery() > 0)
                return true;
            else
                return false;
        }
    }
}


