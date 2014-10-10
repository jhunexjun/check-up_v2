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

namespace Check_upTests
{
    public static class functions
    {
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

        public static bool dropAndCreateDatabase(Hashtable ht)
        {
            string connectionString = "SERVER=" + ht["datasource"] + ";DATABASE=" + ht["database"] + ";UID=" + ht["username"] + ";PASSWORD=" + ht["password"] + ";Allow User Variables=True";
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

        public static bool reloadDatabase() {
            string query;
            string path = @"C:\Users\Jhunex\Documents\Visual Studio 2013\Projects\pmorcilladev\Check-up Business Solution\databases\0schema.sql";

            using (StreamReader sr = new StreamReader(path)) {
                query = sr.ReadToEnd();
            }

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
    }
}


