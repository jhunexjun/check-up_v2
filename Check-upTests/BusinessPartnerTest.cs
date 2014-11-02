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
    public class BusinessPartnerTest
    {
        [TestInitialize]
        public void setUp()
        {
            userTests user = new userTests();
            user.setUp();
            user.addUser();
        }

        [TestMethod]
        public void addBusinessPartner()
        {
            Hashtable ht = new Hashtable();
            
            // supplier = 0; customer = 1;
            ht.Add("BPType", 0);
            ht.Add("code", "S-GNL011");
            ht.Add("BPName", "Glen Marketing Inc.");
            ht.Add("address", "USA");
            ht.Add("tel1", "3214455");
            ht.Add("tel2", "3451121");
            ht.Add("fax", "5644490");
            ht.Add("email", "glen@yahoo.com");
            ht.Add("website", "www.glen-marketing.biz");
            ht.Add("contactPerson", "John");
            ht.Add("deactivated", "N");
            ht.Add("remarks", "Nothing.");
            ht.Add("createdBy", "admin");

            BusinessPartner bp = new BusinessPartner();
            bp.addBusinessPartner(ht);

            DataTable dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "select * from businesspartner";
            cmd.Connection = vars.MySqlConnection;
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);

            Assert.AreEqual(dt.Rows[0]["code"], "S-GNL011");
            Assert.AreEqual(dt.Rows[0]["BPType"], 0);
            Assert.AreEqual(dt.Rows[0]["BPName"], "Glen Marketing Inc.");
            Assert.AreEqual(dt.Rows[0]["address"], "USA");
            Assert.AreEqual(dt.Rows[0]["tel1"], "3214455");
            Assert.AreEqual(dt.Rows[0]["tel2"], "3451121");
            Assert.AreEqual(dt.Rows[0]["fax"], "5644490");
            Assert.AreEqual(dt.Rows[0]["email"], "glen@yahoo.com");
            Assert.AreEqual(dt.Rows[0]["website"], "www.glen-marketing.biz");
            Assert.AreEqual(dt.Rows[0]["contactP"], "John");
            Assert.AreEqual(dt.Rows[0]["deactivated"], "N");
            Assert.AreEqual(dt.Rows[0]["remarks"], "Nothing.");
            Assert.AreEqual(dt.Rows[0]["createdBy"], "admin");
        }
    }
}
