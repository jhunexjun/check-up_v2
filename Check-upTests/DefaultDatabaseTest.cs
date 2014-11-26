using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using Check_up;
using MySql.Data.MySqlClient;
using System.Data;

namespace Check_upTests
{
    [TestClass]
    public class DefaultDatabaseTest
    {
        [TestMethod]
        public void testDefaultDatabase()
        {
            Hashtable ht = new Hashtable();
            ht = functions.readDbConfigFile();
            Assert.IsNotNull(ht);
            Assert.IsTrue(functions.dropAndCreateDatabase(ht));

            // we have to select db again because it was dropped above.
            vars.MySqlConnection.ChangeDatabase(ht["database"].ToString());

            //we want to make sure we don't drop live database.
            Assert.AreEqual("check-up_pmorcilladev", vars.MySqlConnection.Database.ToString());
            Assert.IsTrue(vars.MySqlConnection.Ping());
            Assert.IsTrue(functions.reloadDatabase());

            // before we can create default records first thing is the user because of the constraint.
            userTests user = new userTests();
            user.setUp();
            user.testAddUser();
            Assert.IsTrue(functions.createInitialRecordsForTheTestsForBranch());

            MySqlCommand cmd = new MySqlCommand("select * from warehouse order by id", vars.MySqlConnection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Assert.AreEqual(2, dt.Rows.Count);
            Assert.AreEqual("MAIN", dt.Rows[0]["code"]);
            Assert.AreEqual("Main branch", dt.Rows[0]["name"]);
            Assert.AreEqual("Main", dt.Rows[0]["branchType"]);
            Assert.AreEqual("N", dt.Rows[0]["deactivated"]); // the default value
            Assert.AreEqual("admin", dt.Rows[0]["createdBy"]);            
            Assert.AreEqual("BRANCH1", dt.Rows[1]["code"]);
            Assert.AreEqual("Branch One", dt.Rows[1]["name"]);
            Assert.AreEqual("Branch", dt.Rows[1]["branchType"]);
            Assert.AreEqual("N", dt.Rows[1]["deactivated"]); // the default value
            Assert.AreEqual("admin", dt.Rows[1]["createdBy"]);

            cmd = new MySqlCommand("select * from terminal", vars.MySqlConnection);
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt = new DataTable());
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual("BRANCH1", dt.Rows[0]["terminalId"]);
            Assert.AreEqual("BRANCH1", dt.Rows[0]["terminalName"]);
            Assert.AreEqual("BRANCH1", dt.Rows[0]["whCode"]);
            Assert.AreEqual("Branch", dt.Rows[0]["branchType"]);

            cmd = new MySqlCommand("select * from documents order by id", vars.MySqlConnection);
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt = new DataTable());
            Assert.AreEqual(9, dt.Rows.Count);
            Assert.AreEqual("PO", dt.Rows[0]["documentCode"]);
            Assert.AreEqual("Purchase Order", dt.Rows[0]["documentName"]);
            Assert.AreEqual(0, dt.Rows[0]["lastNo"]);
            Assert.AreEqual("SI", dt.Rows[1]["documentCode"]);
            Assert.AreEqual("Sales Invoice", dt.Rows[1]["documentName"]);
            Assert.AreEqual(0, dt.Rows[1]["lastNo"]);
            Assert.AreEqual("GRPO", dt.Rows[2]["documentCode"]);
            Assert.AreEqual("Goods Receipt PO", dt.Rows[2]["documentName"]);
            Assert.AreEqual(0, dt.Rows[2]["lastNo"]);
            Assert.AreEqual("IMD", dt.Rows[3]["documentCode"]);
            Assert.AreEqual("Item Master Data", dt.Rows[3]["documentName"]);
            Assert.AreEqual(0, dt.Rows[3]["lastNo"]);
            Assert.AreEqual("SR", dt.Rows[4]["documentCode"]);
            Assert.AreEqual("Sales Return", dt.Rows[4]["documentName"]);
            Assert.AreEqual(0, dt.Rows[4]["lastNo"]);
            Assert.AreEqual("GR", dt.Rows[5]["documentCode"]);
            Assert.AreEqual("Goods Return", dt.Rows[5]["documentName"]);
            Assert.AreEqual(0, dt.Rows[5]["lastNo"]);
            Assert.AreEqual("IT", dt.Rows[6]["documentCode"]);
            Assert.AreEqual("Inventory Transfer", dt.Rows[6]["documentName"]);
            Assert.AreEqual(0, dt.Rows[6]["lastNo"]);
            Assert.AreEqual("DR", dt.Rows[7]["documentCode"]);
            Assert.AreEqual("Delivery Receipt", dt.Rows[7]["documentName"]);
            Assert.AreEqual(0, dt.Rows[7]["lastNo"]);
            Assert.AreEqual("IP", dt.Rows[8]["documentCode"]);
            Assert.AreEqual("Inventory Posting", dt.Rows[8]["documentName"]);
            Assert.AreEqual(0, dt.Rows[8]["lastNo"]);

            cmd = new MySqlCommand("select * from users", vars.MySqlConnection);
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt = new DataTable());
            Assert.AreEqual(1, dt.Rows[0]["user_id"]); // because this is new row we assume that it's 1.
            Assert.AreEqual("admin", dt.Rows[0]["username"]);
            Assert.IsNotNull(dt.Rows[0]["password"]); // admin
            Assert.IsNotNull(dt.Rows[0]["createDate"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["updateDate"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["updatedBy"]);
            Assert.AreEqual("N", dt.Rows[0]["deactivated"]);
            Assert.AreEqual(0, dt.Rows[0]["role"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["exported"]);
        }
    }
}
