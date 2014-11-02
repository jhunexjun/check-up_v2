using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Check_up;
using Check_up.classes;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;

namespace Check_upTests
{
    [TestClass]
    public class DeliveryReceiptTest
    {
        [TestInitialize]
        public void setUp()
        {
            userTests user = new userTests();
            user.setUp();
            user.addUser();
        }

        private bool addBusinessPartners()
        {
            Hashtable ht = new Hashtable();

            // supplier = 0; customer = 1;

            //first record
            ht.Add("BPType", 0);
            ht.Add("code", "s-15");
            ht.Add("BPName", "ERICSON");
            ht.Add("createdBy", "admin");

            BusinessPartner bp = new BusinessPartner();
            if (!bp.addBusinessPartner(ht))
                return false;

            // second record
            ht.Clear();
            ht.Add("BPType", 0);
            ht.Add("code", "S-16");
            ht.Add("BPName", "JONSON MANUFACTURING CO., INC.");
            ht.Add("createdBy", "admin");

            bp = new BusinessPartner();
            if (!bp.addBusinessPartner(ht))
                return false;
            else
                return true;
        }

        private bool registerItems()
        {
            Hashtable items = new Hashtable();
            
            //first record
            items.Add("vatable", "N");
            items.Add("qtyPrPrchsUoM", 1);
            items.Add("qtyPrSaleUoM", 1);
            items.Add("prchsUoM", "PC");
            items.Add("saleUoM", "PC");
            items.Add("varWeightItm", "N");
            items.Add("minStock", 0); // zero means no minimum
            items.Add("maxStock", 0); // zero means no maximum
            items.Add("createdBy", "admin");
            items.Add("description", "tube 200x17");
            items.Add("vendor", "s-15");
            items.Add("deactivated", "N");

            // for prices 0 is purchase order, 1 is retail
            Hashtable prices = new Hashtable();
            prices.Add(0, 82);
            prices.Add(1, 82);

            ItemMasterData itemMasterData = new ItemMasterData();
            if (!itemMasterData.addItem(items, prices))
                return false;

            //second record
            items.Clear();
            items.Add("vatable", "N");
            items.Add("qtyPrPrchsUoM", 1);
            items.Add("qtyPrSaleUoM", 1);
            items.Add("prchsUoM", "PC");
            items.Add("saleUoM", "PC");
            items.Add("varWeightItm", "N");
            items.Add("minStock", 0); // zero means no minimum
            items.Add("maxStock", 0); // zero means no maximum
            items.Add("createdBy", "admin");
            items.Add("description", "CHANGE PEDAL TMX TGO");
            items.Add("vendor", "S-16");
            items.Add("deactivated", "N");

            // for prices 0 is purchase order, 1 is retail
            prices = new Hashtable();
            prices.Add(0, 52);
            prices.Add(1, 52);

            itemMasterData = new ItemMasterData();
            if (!itemMasterData.addItem(items, prices))
                return false;
            else
                return true;
        }

        [TestMethod]
        public void addDeliveryReceipt()
        {
            Assert.IsTrue(functions.createDefaultRecordsForTheTests());

            // let's assume that we have logged in
            vars.terminalId = "MAIN"; vars.username = "admin";

            //add these pre-requisites.
            Assert.IsTrue(addBusinessPartners());
            Assert.IsTrue(registerItems());

             //let's create the table for the row
            DataTable table = new DataTable();
            table.Columns.Add("docId");
            table.Columns.Add("indx");
            table.Columns.Add("vendorCode");
            table.Columns.Add("vendorName");
            table.Columns.Add("itemCode");
            table.Columns.Add("description");
            table.Columns.Add("warehouseRow");
            table.Columns.Add("vatable");
            table.Columns.Add("realBsNetPrchsPrc");
            table.Columns.Add("realBsGrossPrchsPrc");
            table.Columns.Add("realNetPrchsPrc");
            table.Columns.Add("realGrossPrchsPrc");
            table.Columns.Add("qty");
            table.Columns.Add("baseUoM");
            table.Columns.Add("qtyPrPrchsUoM");
            table.Columns.Add("prcntDscnt");
            table.Columns.Add("amtDscnt");
            table.Columns.Add("netPrchsPrc");
            table.Columns.Add("grossPrchsPrc");
            table.Columns.Add("rowNetTotal");
            table.Columns.Add("rowGrossTotal");


            //let's start now the data to get inserted.
            Hashtable header = new Hashtable();
            header.Add("terminalId", vars.terminalId);
            header.Add("warehouse", "MAIN");
            header.Add("createdBy", vars.username);

            DateTime dateTime = DateTime.Today;
            header.Add("postingDate", dateTime.ToString("yyyy/MM/dd"));
            header.Add("remarks1", "This is remarks1.");
            header.Add("remarks2", "This is remarks2.");
            header.Add("totalPrcntDscnt", 0);
            header.Add("totalAmtDscnt", 0);
            header.Add("netTotal", 134);
            header.Add("grossTotal", 134);

            DataRow row;

            //first record
            row = table.NewRow();
            row["indx"] = 0;
            row["vendorCode"] = "s-15";
            row["vendorName"] = "ERICSON";
            row["itemCode"] = "MAINITM1";
            row["description"] = "tube 200x17";
            row["warehouseRow"] = "MAIN";
            row["vatable"] = "N";
            row["realBsNetPrchsPrc"] = 82;
            row["realBsGrossPrchsPrc"] = 82;
            row["realNetPrchsPrc"] = 82;
            row["realGrossPrchsPrc"] = 82;
            row["qty"] = 1;
            row["baseUoM"] = "N";
            row["qtyPrPrchsUoM"] = 1;
            row["prcntDscnt"] = 0.00;
            row["amtDscnt"] = 0.00;
            row["netPrchsPrc"] = 82;
            row["grossPrchsPrc"] = 82;            
            row["rowNetTotal"] = 82;
            row["rowGrossTotal"] = 82;
            
            table.Rows.Add(row);

            //second record
            row = table.NewRow();
            row["indx"] = 1;
            row["vendorCode"] = "S-16";
            row["vendorName"] = "JONSON MANUFACTURING CO., INC.";
            row["itemCode"] = "MAINITM2";
            row["description"] = "CHANGE PEDAL TMX TGO";
            row["warehouseRow"] = "MAIN";
            row["vatable"] = "N";
            row["realBsNetPrchsPrc"] = 52;
            row["realBsGrossPrchsPrc"] = 52;
            row["realNetPrchsPrc"] = 52;
            row["realGrossPrchsPrc"] = 52;
            row["qty"] = 1;
            row["baseUoM"] = "N";
            row["qtyPrPrchsUoM"] = 1;
            row["prcntDscnt"] = 0.00;
            row["amtDscnt"] = 0.00;
            row["netPrchsPrc"] = 52;
            row["grossPrchsPrc"] = 52;
            row["rowNetTotal"] = 52;
            row["rowGrossTotal"] = 52;

            table.Rows.Add(row);

            DeliveryReceipt deliveryReceipt = new DeliveryReceipt();
            Assert.IsTrue(deliveryReceipt.addDeliveryReceipt(header, table));

            MySqlCommand cmd = new MySqlCommand("select * from deliveryreceipt", vars.MySqlConnection);
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Assert.AreEqual(dt.Rows[0]["id"], 1); //firtst record starts with 1
            Assert.AreEqual(dt.Rows[0]["docId"], "MAIN1");
            Assert.AreEqual(dt.Rows[0]["warehouse"], "MAIN");
            Assert.AreEqual(dt.Rows[0]["postingDate"], DateTime.Today);
            Assert.AreEqual(dt.Rows[0]["remarks1"], "This is remarks1.");
            Assert.AreEqual(dt.Rows[0]["remarks2"], "This is remarks2.");
            Assert.AreEqual(dt.Rows[0]["totalPrcntDscnt"], 0m);
            Assert.AreEqual(dt.Rows[0]["totalAmtDscnt"], 0m);
            Assert.AreEqual(dt.Rows[0]["netTotal"], 134m);
            Assert.AreEqual(dt.Rows[0]["grossTotal"], 134m);
            Assert.IsNotNull(dt.Rows[0]["createDate"]);
            Assert.AreEqual(dt.Rows[0]["createdBy"], "admin");

            cmd = new MySqlCommand("select * from deliveryreceipt_item", vars.MySqlConnection);
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt = new DataTable());
            Assert.AreEqual(dt.Rows[0]["docId"], "MAIN1");
            Assert.AreEqual(dt.Rows[1]["docId"], "MAIN1");
            Assert.AreEqual(dt.Rows[0]["Indx"], 0);
            Assert.AreEqual(dt.Rows[1]["Indx"], 1);
            Assert.AreEqual(dt.Rows[0]["vendorCode"], "s-15");
            Assert.AreEqual(dt.Rows[1]["vendorCode"], "S-16");
            Assert.AreEqual(dt.Rows[0]["vendorName"], "ERICSON");
            Assert.AreEqual(dt.Rows[1]["vendorName"], "JONSON MANUFACTURING CO., INC.");
            Assert.AreEqual(dt.Rows[0]["itemCode"], "MAINITM1");
            Assert.AreEqual(dt.Rows[1]["itemCode"], "MAINITM2");
            Assert.AreEqual(dt.Rows[0]["description"], "tube 200x17");
            Assert.AreEqual(dt.Rows[1]["description"], "CHANGE PEDAL TMX TGO");
            Assert.AreEqual(dt.Rows[0]["warehouse"], "MAIN");
            Assert.AreEqual(dt.Rows[1]["warehouse"], "MAIN");
            Assert.AreEqual(dt.Rows[0]["vatable"], "N");
            Assert.AreEqual(dt.Rows[1]["vatable"], "N");
            Assert.AreEqual(dt.Rows[0]["realBsNetPrchsPrc"], 82m);
            Assert.AreEqual(dt.Rows[1]["realBsNetPrchsPrc"], 52m);
            Assert.AreEqual(dt.Rows[0]["realBsGrossPrchsPrc"], 82m);
            Assert.AreEqual(dt.Rows[1]["realBsGrossPrchsPrc"], 52m);
            Assert.AreEqual(dt.Rows[0]["realNetPrchsPrc"], 82m);
            Assert.AreEqual(dt.Rows[1]["realNetPrchsPrc"], 52m);
            Assert.AreEqual(dt.Rows[0]["realGrossPrchsPrc"], 82m);
            Assert.AreEqual(dt.Rows[1]["realGrossPrchsPrc"], 52m);
            Assert.AreEqual(dt.Rows[0]["qty"], 1m);
            Assert.AreEqual(dt.Rows[1]["qty"], 1m);
            Assert.AreEqual(dt.Rows[0]["baseUoM"], "N");
            Assert.AreEqual(dt.Rows[1]["baseUoM"], "N");
            Assert.AreEqual(dt.Rows[0]["qtyPrPrchsUoM"], 1m);
            Assert.AreEqual(dt.Rows[1]["qtyPrPrchsUoM"], 1m);
            Assert.AreEqual(dt.Rows[0]["prcntDscnt"], 0m);
            Assert.AreEqual(dt.Rows[1]["prcntDscnt"], 0m);
            Assert.AreEqual(dt.Rows[0]["amtDscnt"], 0m);
            Assert.AreEqual(dt.Rows[1]["amtDscnt"], 0m);
            Assert.AreEqual(dt.Rows[0]["netPrchsPrc"], 82m);
            Assert.AreEqual(dt.Rows[1]["netPrchsPrc"], 52m);
            Assert.AreEqual(dt.Rows[0]["realBsGrossPrchsPrc"], 82m);
            Assert.AreEqual(dt.Rows[1]["realBsGrossPrchsPrc"], 52m);
            Assert.AreEqual(dt.Rows[0]["grossPrchsPrc"], 82m);
            Assert.AreEqual(dt.Rows[1]["grossPrchsPrc"], 52m);
            Assert.AreEqual(dt.Rows[0]["rowNetTotal"], 82m);
            Assert.AreEqual(dt.Rows[1]["rowNetTotal"], 52m);
            Assert.AreEqual(dt.Rows[0]["rowGrossTotal"], 82m);
            Assert.AreEqual(dt.Rows[1]["rowGrossTotal"], 52m);
            Assert.AreEqual(dt.Rows[0]["exported"], DBNull.Value);
            Assert.AreEqual(dt.Rows[1]["exported"], DBNull.Value);
        }
    }
}
