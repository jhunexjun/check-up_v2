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
    public class InventoryTransferTest
    {
        [TestInitialize]
        public void setUp()
        {
            userTests user = new userTests();
            user.setUp();
            user.testAddUser();
        }

        private bool addWarehouses()
        {
            //Because this module should not accept same from and to warehouse, we have to have two warehouses.
            //Note: Initial warehouse 'MAIN' is already been added in setUp()
            Hashtable ht = new Hashtable();
            ht.Add("code", "US-FL");
            ht.Add("name", "US Florida Warehouse");
            ht.Add("branchType", "Branch");
            Warehouse wh = new Warehouse();
            if (wh.addWarehouse(ht))
                return true;
            else
                return false;
        }

        private bool addBusinessPartners()
        {
            Hashtable ht = new Hashtable();

            // supplier = 0; customer = 1;

            //first record
            ht.Add("BPType", 0);
            ht.Add("code", "s-7");
            ht.Add("BPName", "XYZ Enterprises");
            ht.Add("createdBy", "admin");

            BusinessPartner bp = new BusinessPartner();
            if ( ! bp.addBusinessPartner(ht))
                return false;

            // second record
            ht.Clear();
            ht.Add("BPType", 0);
            ht.Add("code", "s-15");
            ht.Add("BPName", "ABC Corporation");
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
            items.Add("createdBy", vars.username);
            items.Add("description", "REAR SPROCKET  RS100 42T MOTOX");
            items.Add("vendor", "s-7");
            items.Add("deactivated", "N");

            // for prices 0 is purchase order, 1 is retail
            Hashtable prices = new Hashtable();
            prices.Add(0, 155);
            prices.Add(1, 300);

            ItemMasterData itemMasterData = new ItemMasterData();
            if ( ! itemMasterData.addItem(items, prices))
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
            items.Add("createdBy", vars.username);
            items.Add("description", "CHANGE PEDAL XRM");
            items.Add("vendor", "s-7");
            items.Add("deactivated", "N");

            // for prices 0 is purchase order, 1 is retail
            prices = new Hashtable();
            prices.Add(0, 92);
            prices.Add(1, 200);

            itemMasterData = new ItemMasterData();
            if ( ! itemMasterData.addItem(items, prices))
                return false;

            //third record
            items.Clear();
            items.Add("vatable", "N");
            items.Add("qtyPrPrchsUoM", 1);
            items.Add("qtyPrSaleUoM", 1);
            items.Add("prchsUoM", "PC");
            items.Add("saleUoM", "PC");
            items.Add("varWeightItm", "N");
            items.Add("minStock", 0); // zero means no minimum
            items.Add("maxStock", 0); // zero means no maximum
            items.Add("createdBy", vars.username);
            items.Add("description", "ALLEN BOLT");
            items.Add("vendor", "s-15");
            items.Add("deactivated", "N");

            // for prices 0 is purchase order, 1 is retail
            prices = new Hashtable();
            prices.Add(0, 8);
            prices.Add(1, 15);

            itemMasterData = new ItemMasterData();
            if (!itemMasterData.addItem(items, prices))
                return false;
            else
                return true;
        }

        [TestMethod]
        public void testAddInventoryTransfer()
        {
            Assert.IsTrue(functions.createDefaultRecordsForTheTests());

            // let's assume that we have logged in. Note this should comes first as this is being used in below functions.
            vars.terminalId = "MAIN"; vars.username = "admin";

            //add these pre-requisites records.
            Assert.IsTrue(addWarehouses());
            Assert.IsTrue(addBusinessPartners());
            Assert.IsTrue(registerItems());

            //let's create the table
            DataTable table = new DataTable();
            table.Columns.Add("docId");
            table.Columns.Add("indx");
            table.Columns.Add("itemCode");
            table.Columns.Add("description");
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
            table.Columns.Add("qtyPrRtlUoM");
            table.Columns.Add("realBsNetPrcRtl");
            table.Columns.Add("realBsGrossPrcRtl");
            table.Columns.Add("realNetPrcRtl");
            table.Columns.Add("realGrossPrcRtl");
            table.Columns.Add("netPrcRtl");
            table.Columns.Add("grossPrcRtl");
            table.Columns.Add("prcntDscntRtl");
            table.Columns.Add("amtDscntRtl");
            table.Columns.Add("rowNetTotalRtl");
            table.Columns.Add("rowGrossTotalRtl");


            //let's start now the data to get inserted.
            Hashtable header = new Hashtable();
            header.Add("terminalId", vars.terminalId);
            header.Add("frmWHouse", "MAIN");
            header.Add("toWHouse", "US-FL");
            header.Add("createdBy", vars.username);

            DateTime dateTime = DateTime.Today;
            header.Add("postingDate", dateTime.ToString("yyyy/MM/dd"));

            //purchase prices
            header.Add("totalPrcntDscnt", 0);
            header.Add("totalAmtDscnt", 0);
            header.Add("netTotal", 600);
            header.Add("grossTotal", 600);
            //retail prices
            header.Add("totalPrcntDscntRtl", 36.666667);
            header.Add("totalAmtDscntRtl", 606);
            header.Add("netTotalRtl", 654);
            header.Add("grossTotalRtl", 654);            

            DataRow row;
            
            //first record
            row = table.NewRow();
            row["indx"] = 0;
            row["itemCode"] = "MAINITM1";
            row["description"] = "REAR SPROCKET  RS100 42T MOTOX";
            row["qty"] = 2;
            row["vatable"] = "N";
            row["baseUoM"] = "N";
            //purchase prices
            row["qtyPrPrchsUoM"] = 1;
            row["realBsNetPrchsPrc"] = 155;
            row["realBsGrossPrchsPrc"] = 155;
            row["realNetPrchsPrc"] = 155;
            row["realGrossPrchsPrc"] = 155;
            row["prcntDscnt"] = 0.00;
            row["grossPrchsPrc"] = 310;
            row["netPrchsPrc"] = 310;
            row["amtDscnt"] = 300;
            row["rowNetTotal"] = 300;
            row["rowGrossTotal"] = 300;
            //retail
            row["qtyPrRtlUoM"] = 1;
            row["realBsNetPrcRtl"] = 300;
            row["realBsGrossPrcRtl"] = 300;
            row["realNetPrcRtl"] = 300;
            row["realGrossPrcRtl"] = 300;
            row["prcntDscntRtl"] = 50;
            row["grossPrcRtl"] = 600;
            row["netPrcRtl"] = 600;
            row["amtDscntRtl"] = 300;
            row["rowNetTotalRtl"] = 300;
            row["rowGrossTotalRtl"] = 300;

            table.Rows.Add(row);

            //second record
            row = table.NewRow();
            row["indx"] = 1;
            row["itemCode"] = "MAINITM2";
            row["description"] = "CHANGE PEDAL XRM";
            row["qty"] = 3;
            row["vatable"] = "N";
            row["baseUoM"] = "N";
            //purchase prices
            row["qtyPrPrchsUoM"] = 1;
            row["realBsNetPrchsPrc"] = 92;
            row["realBsGrossPrchsPrc"] = 92;
            row["realNetPrchsPrc"] = 92;
            row["realGrossPrchsPrc"] = 92;
            row["prcntDscnt"] = 0;
            row["grossPrchsPrc"] = 276;
            row["netPrchsPrc"] = 276;
            row["amtDscnt"] = 300;
            row["rowNetTotal"] = 300;
            row["rowGrossTotal"] = 300;
            //retail
            row["qtyPrRtlUoM"] = 1;
            row["realBsNetPrcRtl"] = 200;
            row["realBsGrossPrcRtl"] = 200;
            row["realNetPrcRtl"] = 200;
            row["realGrossPrcRtl"] = 200;
            row["prcntDscntRtl"] = 50;
            row["grossPrcRtl"] = 600;
            row["netPrcRtl"] = 600;
            row["amtDscntRtl"] = 300;
            row["rowNetTotalRtl"] = 300;
            row["rowGrossTotalRtl"] = 300;

            table.Rows.Add(row);

            //third record
            row = table.NewRow();
            row["indx"] = 2;
            row["itemCode"] = "MAINITM3";
            row["description"] = "ALLEN BOLT";
            row["qty"] = 4;
            row["vatable"] = "N";
            row["baseUoM"] = "N";
            //purchase prices
            row["qtyPrPrchsUoM"] = 1;
            row["realBsNetPrchsPrc"] = 8;
            row["realBsGrossPrchsPrc"] = 8;
            row["realNetPrchsPrc"] = 8;
            row["realGrossPrchsPrc"] = 8;
            row["prcntDscnt"] = 0;
            row["grossPrchsPrc"] = 32;
            row["netPrchsPrc"] = 32;
            row["amtDscnt"] = 6;
            row["rowNetTotal"] = 54;
            row["rowGrossTotal"] = 54;
            //retail
            row["qtyPrRtlUoM"] = 1;
            row["realBsNetPrcRtl"] = 15;
            row["realBsGrossPrcRtl"] = 15;
            row["realNetPrcRtl"] = 15;
            row["realGrossPrcRtl"] = 15;
            row["prcntDscntRtl"] = 10;
            row["grossPrcRtl"] = 60;
            row["netPrcRtl"] = 60;
            row["amtDscntRtl"] = 6;
            row["rowNetTotalRtl"] = 54;
            row["rowGrossTotalRtl"] = 54;

            table.Rows.Add(row);

            InventoryTransfer inventoryTransfer = new InventoryTransfer();
            Assert.IsTrue(inventoryTransfer.addInventoryTransfer(header, table));
            
            MySqlCommand cmd = new MySqlCommand("select * from inventorytransfer", vars.MySqlConnection);
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Assert.AreEqual(dt.Rows[0]["id"], 1); //firtst record starts with 1
            Assert.AreEqual(dt.Rows[0]["docId"], "MAIN1");
            Assert.AreEqual(dt.Rows[0]["frmWHouse"], "MAIN");
            Assert.AreEqual(dt.Rows[0]["toWHouse"], "US-FL");
            Assert.AreEqual(dt.Rows[0]["postingDate"], DateTime.Today);
            Assert.AreEqual(dt.Rows[0]["remarks1"], DBNull.Value);
            Assert.AreEqual(dt.Rows[0]["remarks2"], DBNull.Value);
            Assert.AreEqual(dt.Rows[0]["totalPrcntDscnt"], 0m);

            //continue...
        }

        [TestMethod]
        public void testUpdateInventoryTransfer()
        {
            Assert.IsTrue(functions.createDefaultRecordsForTheTests());

            // let's assume that we have logged in
            vars.terminalId = "MAIN"; vars.username = "admin";

            //add these pre-requisite records.
            Assert.IsTrue(addWarehouses());
            Assert.IsTrue(addBusinessPartners());
            Assert.IsTrue(registerItems());

            //let's create the table
            DataTable table = new DataTable();
            table.Columns.Add("docId");
            table.Columns.Add("indx");
            table.Columns.Add("itemCode");
            table.Columns.Add("description");
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
            table.Columns.Add("qtyPrRtlUoM");
            table.Columns.Add("realBsNetPrcRtl");
            table.Columns.Add("realBsGrossPrcRtl");
            table.Columns.Add("realNetPrcRtl");
            table.Columns.Add("realGrossPrcRtl");
            table.Columns.Add("netPrcRtl");
            table.Columns.Add("grossPrcRtl");
            table.Columns.Add("prcntDscntRtl");
            table.Columns.Add("amtDscntRtl");
            table.Columns.Add("rowNetTotalRtl");
            table.Columns.Add("rowGrossTotalRtl");


            //let's start now the data to get inserted.
            Hashtable header = new Hashtable();
            header.Add("terminalId", vars.terminalId);
            header.Add("frmWHouse", "MAIN");
            header.Add("toWHouse", "US-FL");
            header.Add("createdBy", vars.username);

            DateTime dateTime = DateTime.Today;
            header.Add("postingDate", dateTime.ToString("yyyy/MM/dd"));

            //purchase prices
            header.Add("totalPrcntDscnt", 0);
            header.Add("totalAmtDscnt", 0);
            header.Add("netTotal", 600);
            header.Add("grossTotal", 600);
            //retail prices
            header.Add("totalPrcntDscntRtl", 36.666667);
            header.Add("totalAmtDscntRtl", 606);
            header.Add("netTotalRtl", 654);
            header.Add("grossTotalRtl", 654);

            DataRow row;

            //first record
            row = table.NewRow();
            row["indx"] = 0;
            row["itemCode"] = "MAINITM1";
            row["description"] = "REAR SPROCKET  RS100 42T MOTOX";
            row["qty"] = 2;
            row["vatable"] = "N";
            row["baseUoM"] = "N";
            //purchase prices
            row["qtyPrPrchsUoM"] = 1;
            row["realBsNetPrchsPrc"] = 155;
            row["realBsGrossPrchsPrc"] = 155;
            row["realNetPrchsPrc"] = 155;
            row["realGrossPrchsPrc"] = 155;
            row["prcntDscnt"] = 0.00;
            row["grossPrchsPrc"] = 310;
            row["netPrchsPrc"] = 310;
            row["amtDscnt"] = 300;
            row["rowNetTotal"] = 300;
            row["rowGrossTotal"] = 300;
            //retail
            row["qtyPrRtlUoM"] = 1;
            row["realBsNetPrcRtl"] = 300;
            row["realBsGrossPrcRtl"] = 300;
            row["realNetPrcRtl"] = 300;
            row["realGrossPrcRtl"] = 300;
            row["prcntDscntRtl"] = 50;
            row["grossPrcRtl"] = 600;
            row["netPrcRtl"] = 600;
            row["amtDscntRtl"] = 300;
            row["rowNetTotalRtl"] = 300;
            row["rowGrossTotalRtl"] = 300;

            table.Rows.Add(row);

            //second record
            row = table.NewRow();
            row["indx"] = 1;
            row["itemCode"] = "MAINITM2";
            row["description"] = "CHANGE PEDAL XRM";
            row["qty"] = 3;
            row["vatable"] = "N";
            row["baseUoM"] = "N";
            //purchase prices
            row["qtyPrPrchsUoM"] = 1;
            row["realBsNetPrchsPrc"] = 92;
            row["realBsGrossPrchsPrc"] = 92;
            row["realNetPrchsPrc"] = 92;
            row["realGrossPrchsPrc"] = 92;
            row["prcntDscnt"] = 0;
            row["grossPrchsPrc"] = 276;
            row["netPrchsPrc"] = 276;
            row["amtDscnt"] = 300;
            row["rowNetTotal"] = 300;
            row["rowGrossTotal"] = 300;
            //retail
            row["qtyPrRtlUoM"] = 1;
            row["realBsNetPrcRtl"] = 200;
            row["realBsGrossPrcRtl"] = 200;
            row["realNetPrcRtl"] = 200;
            row["realGrossPrcRtl"] = 200;
            row["prcntDscntRtl"] = 50;
            row["grossPrcRtl"] = 600;
            row["netPrcRtl"] = 600;
            row["amtDscntRtl"] = 300;
            row["rowNetTotalRtl"] = 300;
            row["rowGrossTotalRtl"] = 300;

            table.Rows.Add(row);

            //third record
            row = table.NewRow();
            row["indx"] = 2;
            row["itemCode"] = "MAINITM3";
            row["description"] = "ALLEN BOLT";
            row["qty"] = 4;
            row["vatable"] = "N";
            row["baseUoM"] = "N";
            //purchase prices
            row["qtyPrPrchsUoM"] = 1;
            row["realBsNetPrchsPrc"] = 8;
            row["realBsGrossPrchsPrc"] = 8;
            row["realNetPrchsPrc"] = 8;
            row["realGrossPrchsPrc"] = 8;
            row["prcntDscnt"] = 0;
            row["grossPrchsPrc"] = 32;
            row["netPrchsPrc"] = 32;
            row["amtDscnt"] = 6;
            row["rowNetTotal"] = 54;
            row["rowGrossTotal"] = 54;
            //retail
            row["qtyPrRtlUoM"] = 1;
            row["realBsNetPrcRtl"] = 15;
            row["realBsGrossPrcRtl"] = 15;
            row["realNetPrcRtl"] = 15;
            row["realGrossPrcRtl"] = 15;
            row["prcntDscntRtl"] = 10;
            row["grossPrcRtl"] = 60;
            row["netPrcRtl"] = 60;
            row["amtDscntRtl"] = 6;
            row["rowNetTotalRtl"] = 54;
            row["rowGrossTotalRtl"] = 54;

            table.Rows.Add(row);

            InventoryTransfer inventoryTransfer = new InventoryTransfer();
            Assert.IsTrue(inventoryTransfer.addInventoryTransfer(header, table));

            header.Add("remarks2", "Remarks2 has been updated.");
            header.Add("updatedBy", "admin");
            header.Add("docId", "MAIN1");

            Assert.IsTrue(inventoryTransfer.updateInventoryTransfer(header));
        }
    }
}
