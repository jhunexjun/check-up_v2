using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using Check_up.classes;
using Check_up;
using System.Data;
using MySql.Data.MySqlClient;

namespace Check_upTests.forms
{
    [TestClass]
    public class WarehouseTest
    {
        [TestInitialize]
        public void setUp()
        {
            userTests user = new userTests();
            user.setUp();
            user.addUser();
        }

        [TestMethod]
        public void addWarehouse()
        {
            Hashtable ht = new Hashtable();
            ht.Add("code", "US-NY");
            ht.Add("name", "Main Warehouse");
            ht.Add("branchType", "Main");
            ht.Add("ftp_url", "ftp://192.168.1.100/incoming/");
            ht.Add("ftp_username", "ftp_anonymous");
            ht.Add("ftp_password", "ftp_password");
            ht.Add("deactivated", "N");
            ht.Add("createdBy", 0);

            Warehouse wh = new Warehouse();
            wh.addWarehouse(ht);

            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "select * from warehouse";
            cmd.Connection = vars.MySqlConnection;
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);

            Assert.AreEqual(dt.Rows[0]["id"], 1);   // this is assumed to be 1
            Assert.AreEqual(dt.Rows[0]["code"], "US-NY");
            Assert.AreEqual(dt.Rows[0]["name"], "Main Warehouse");
            Assert.AreEqual(dt.Rows[0]["branchType"], "Main");
            Assert.AreEqual(dt.Rows[0]["ftp_url"], "ftp://192.168.1.100/incoming/");
            Assert.AreEqual(dt.Rows[0]["ftp_username"], "ftp_anonymous");
            Assert.AreEqual(dt.Rows[0]["ftp_password"], "ftp_password");
            Assert.AreEqual(dt.Rows[0]["deactivated"], "N");
            Assert.AreEqual(dt.Rows[0]["createdBy"], 0);
        }

        [TestMethod]
        public void updateWarehouse()
        {

        }
    }
}
