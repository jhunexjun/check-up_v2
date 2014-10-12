using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Check_upTests;
using System.Collections;
using Check_up.classes;
using Check_up;
using System.Data;
using MySql.Data.MySqlClient;

namespace Check_upTests.forms
{
    [TestClass]
    public class ItemMasterDataTest
    {
        [TestInitialize]
        public void setUp()
        {
            userTests user = new userTests();
            user.setUp();
            user.addUser();
        }

        [TestMethod]
        public void addItem()
        {
           Assert.IsTrue( functions.createDefaultRecordsIntoDB());

            vars.terminalId = "MAIN";
            vars.user_id = 1;

            Hashtable items = new Hashtable();
            items.Add("vatable", "N");
            items.Add("qtyPrPrchsUoM", 6);
            items.Add("qtyPrSaleUoM", 1);
            items.Add("prchsUoM", "BOX");
            items.Add("saleUoM", "PC");
            items.Add("varWeightItm", "N");
            items.Add("minStock", 0); // zero means no minimum
            items.Add("maxStock", 0); // zero means no maximum
            items.Add("createdBy", 0);
            items.Add("description", "Long Bike");
            items.Add("shortName", "Bike");
            items.Add("vendor", "V-MI2200");
            items.Add("remarks", "This is remarks.");
            items.Add("deactivated", "N");

            // for prices 0 is purchase order, 1 is retail
            Hashtable prices = new Hashtable();
            prices.Add(0, "120.50");
            prices.Add(1, "99.50");

            ArrayList barcodes = new ArrayList();
            barcodes.Add("987654321");
            barcodes.Add("1234567890");

            ItemMasterData itemMasterData = new ItemMasterData();
            Assert.IsTrue(itemMasterData.addItem(items, prices, barcodes));

            DataTable dt = new DataTable();
            string sql = "select * from itemmasterdata left join barcode using(itemcode) left join pricelist using(itemcode)";
            MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Assert.AreEqual(dt.Rows.Count, 4, "Number of records count are not the same.");
            Assert.AreEqual(dt.Rows[0]["itemCode"], "MAINITM1");
            Assert.AreEqual(dt.Rows[0]["description"], "Long Bike");
            Assert.AreEqual(dt.Rows[0]["shortName"], "Bike");
            Assert.AreEqual(dt.Rows[0]["vatable"], "N");
            Assert.AreEqual(dt.Rows[0]["vendor"], "V-MI2200");
            Assert.AreEqual(dt.Rows[0]["qtyPrPrchsUoM"], 6);
            Assert.AreEqual(dt.Rows[0]["qtyPrSaleUoM"], 1);
            Assert.AreEqual(dt.Rows[0]["prchsUoM"], "BOX");
            Assert.AreEqual(dt.Rows[0]["saleUoM"], "PC");
            Assert.AreEqual(dt.Rows[0]["varWeightItm"], "N");
            Assert.AreEqual(dt.Rows[0]["remarks"], "This is remarks.");
            Assert.AreEqual(Convert.ToDecimal(dt.Rows[0]["minStock"]), 0m);
            Assert.AreEqual(Convert.ToDecimal(dt.Rows[0]["maxStock"]), 0m);
            Assert.AreEqual(dt.Rows[0]["deactivated"], "N");
            Assert.IsNotNull(dt.Rows[0]["createDate"]);
            Assert.AreEqual(dt.Rows[0]["createdBy"], 1);
            Assert.AreEqual(dt.Rows[0]["updateDate"].ToString(), "");
            Assert.AreEqual(dt.Rows[0]["updatedBy"].ToString(), "");
            Assert.AreEqual(dt.Rows[0]["trans"].ToString(), "");
            Assert.AreEqual(dt.Rows[0]["exported"].ToString(), "");
        }

        [TestMethod]
        public void updateItem()
        {

        }
    }
}
