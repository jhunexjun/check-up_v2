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

        private bool addWarehouse()
        {
            //Because this module should not accept same from and to warehouse, we have to have two warehouses.
            //Note: Initial warehouse 'MAIN' is already been added in setUp()
            Hashtable ht = new Hashtable();
            ht.Add("code", "US-FL");
            ht.Add("name", "US Florida Warehouse");
            ht.Add("branchType", "Branch");
            ht.Add("createdBy", vars.username);
            ht.Add("deactivated", vars.username);
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
            ht.Add("code", "S-1");
            ht.Add("BPName", "ABC Enterprises");
            ht.Add("createdBy", "admin");

            BusinessPartner bp = new BusinessPartner();
            if ( ! bp.addBusinessPartner(ht))
                return false;

            // second record
            ht.Clear();
            ht.Add("BPType", 0);
            ht.Add("code", "S-2");
            ht.Add("BPName", "XYZ Corporation");
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
            items.Add("qtyPrPrchsUoM", 2);
            items.Add("qtyPrSaleUoM", 2);
            items.Add("prchsUoM", "BX");
            items.Add("saleUoM", "BX");
            items.Add("varWeightItm", "N");
            items.Add("minStock", 2); // zero means no minimum
            items.Add("maxStock", 50); // zero means no maximum
            items.Add("createdBy", vars.username);
            items.Add("description", "Item 1");
            items.Add("vendor", "S-1");
            items.Add("deactivated", "N");
            items.Add("remarks", "remarks1");
            

            // for prices 0 is purchase order, 1 is retail
            Hashtable prices = new Hashtable();
            prices.Add(0, 2);
            prices.Add(1, 3);

            ItemMasterData itemMasterData = new ItemMasterData();
            if ( ! itemMasterData.addItem(items, prices))
                return false;

            //second record
            items.Clear();
            items.Add("vatable", "N");
            items.Add("qtyPrPrchsUoM", 3);
            items.Add("qtyPrSaleUoM", 3);
            items.Add("prchsUoM", "BX");
            items.Add("saleUoM", "BX");
            items.Add("varWeightItm", "N");
            items.Add("minStock", 2); // zero means no minimum
            items.Add("maxStock", 50); // zero means no maximum
            items.Add("createdBy", vars.username);
            items.Add("description", "Item 2");
            items.Add("vendor", "S-2");
            items.Add("deactivated", "N");

            // for prices 0 is purchase order, 1 is retail
            prices = new Hashtable();
            prices.Add(0, 2);
            prices.Add(1, 3);

            itemMasterData = new ItemMasterData();
            if ( ! itemMasterData.addItem(items, prices))
                return false;

            //third record
            items.Clear();
            items.Add("vatable", "N");
            items.Add("qtyPrPrchsUoM", 4);
            items.Add("qtyPrSaleUoM", 4);
            items.Add("prchsUoM", "BX");
            items.Add("saleUoM", "BX");
            items.Add("varWeightItm", "N");
            items.Add("minStock", 2); // zero means no minimum
            items.Add("maxStock", 50); // zero means no maximum
            items.Add("createdBy", vars.username);
            items.Add("description", "Item 3");
            items.Add("vendor", "S-1");
            items.Add("deactivated", "N");

            // for prices 0 is purchase order, 1 is retail
            prices = new Hashtable();
            prices.Add(0, 2);
            prices.Add(1, 3);

            itemMasterData = new ItemMasterData();
            if (!itemMasterData.addItem(items, prices))
                return false;
            else
                return true;
        }

        // We cannot successfully Transfer inventory if we don't have items delivered.
        private bool addDeliveryReceipt()
        {
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
            header.Add("netTotal", 180);
            header.Add("grossTotal", 180);

            DataRow row;

            //1st record
            row = table.NewRow();
            row["indx"] = 0;
            row["vendorCode"] = "S-1";
            row["vendorName"] = "ABC Enterprises";
            row["itemCode"] = "MAINITM1";
            row["description"] = "Item 2";
            row["warehouseRow"] = "MAIN";
            row["vatable"] = "N";
            row["realBsNetPrchsPrc"] = 2.00;
            row["realBsGrossPrchsPrc"] = 2.00;
            row["realNetPrchsPrc"] = 2.00;
            row["realGrossPrchsPrc"] = 2.00;
            row["qty"] = 10;
            row["baseUoM"] = "N";
            row["qtyPrPrchsUoM"] = 2;
            row["prcntDscnt"] = 0.00;
            row["amtDscnt"] = 0.00;
            row["netPrchsPrc"] = 40.00;
            row["grossPrchsPrc"] = 40.00;
            row["rowNetTotal"] = 40.00;
            row["rowGrossTotal"] = 40.00;

            table.Rows.Add(row);

            //2nd record
            row = table.NewRow();
            row["indx"] = 1;
            row["vendorCode"] = "S-1";
            row["vendorName"] = "ABC Corporation";
            row["itemCode"] = "MAINITM2";
            row["description"] = "Item 2";
            row["warehouseRow"] = "MAIN";
            row["vatable"] = "N";
            row["realBsNetPrchsPrc"] = 2.00;
            row["realBsGrossPrchsPrc"] = 2.00;
            row["realNetPrchsPrc"] = 2.00;
            row["realGrossPrchsPrc"] = 2.00;
            row["qty"] = 10;
            row["baseUoM"] = "N";
            row["qtyPrPrchsUoM"] = 3;
            row["prcntDscnt"] = 0.00;
            row["amtDscnt"] = 0.00;
            row["netPrchsPrc"] = 60.00;
            row["grossPrchsPrc"] = 60.00;
            row["rowNetTotal"] = 60.00;
            row["rowGrossTotal"] = 60.00;
            table.Rows.Add(row);

            // 3rd record
            row = table.NewRow();
            row["indx"] = 2;
            row["vendorCode"] = "S-1";
            row["vendorName"] = "ABC Corporation";
            row["itemCode"] = "MAINITM3";
            row["description"] = "Item 3";
            row["warehouseRow"] = "MAIN";
            row["vatable"] = "N";
            row["realBsNetPrchsPrc"] = 2.00;
            row["realBsGrossPrchsPrc"] = 2.00;
            row["realNetPrchsPrc"] = 2.00;
            row["realGrossPrchsPrc"] = 2.00;
            row["qty"] = 10;
            row["baseUoM"] = "N";
            row["qtyPrPrchsUoM"] = 4;
            row["prcntDscnt"] = 0.00;
            row["amtDscnt"] = 0.00;
            row["netPrchsPrc"] = 80.00;
            row["grossPrchsPrc"] = 80.00;
            row["rowNetTotal"] = 80.00;
            row["rowGrossTotal"] = 80.00;
            table.Rows.Add(row);

            DeliveryReceipt deliveryReceipt = new DeliveryReceipt();
            Assert.IsTrue(deliveryReceipt.addDeliveryReceipt(header, table));

            return true;
        }

        [TestMethod]
        public void testAddInventoryTransfer()
        {
            Assert.IsTrue(functions.createInitialRecordsForTheTests());

            // let's assume that we have logged in. Note this should comes first as this is being used in below functions.
            vars.terminalId = "MAIN"; vars.username = "admin";

            //add these pre-requisites records.
            Assert.IsTrue(addWarehouse());
            Assert.IsTrue(addBusinessPartners());
            Assert.IsTrue(registerItems());
            Assert.IsTrue(addDeliveryReceipt()); // We cannot successfully Transfer inventory if we don't have items delivered.


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
            header.Add("netTotal", 18.00);
            header.Add("grossTotal", 18.00);
            //retail prices
            header.Add("totalPrcntDscntRtl", 0.00);
            header.Add("totalAmtDscntRtl", 0.00);
            header.Add("netTotalRtl", 27.00);
            header.Add("grossTotalRtl", 27.00);

            DataRow row;
            
            //first record
            row = table.NewRow();
            row["indx"] = 0;
            row["itemCode"] = "MAINITM1";
            row["description"] = "Item 1";
            row["qty"] = 2;
            row["vatable"] = "N";
            row["baseUoM"] = "N";
            //purchase prices
            row["qtyPrPrchsUoM"] = 2;
            row["realBsNetPrchsPrc"] = 2;
            row["realBsGrossPrchsPrc"] = 2;
            row["realNetPrchsPrc"] = 4; // has to be removed soon
            row["realGrossPrchsPrc"] = 4; // has to be removed soon
            row["prcntDscnt"] = 0.00;
            row["amtDscnt"] = 0;
            row["grossPrchsPrc"] = 4;  // has to be removed soon
            row["netPrchsPrc"] = 4;  // has to be removed soon            
            row["rowNetTotal"] = 4;
            row["rowGrossTotal"] = 4;
            //retail
            row["qtyPrRtlUoM"] = 2;
            row["realBsNetPrcRtl"] = 3;
            row["realBsGrossPrcRtl"] = 3;
            row["realNetPrcRtl"] = 6;
            row["realGrossPrcRtl"] = 6;
            row["prcntDscntRtl"] = 0;
            row["grossPrcRtl"] = 6; // has to be removed soon
            row["netPrcRtl"] = 6; // has to be removed soon
            row["amtDscntRtl"] = 0;
            row["rowNetTotalRtl"] = 6;
            row["rowGrossTotalRtl"] = 6;
            table.Rows.Add(row);

            //second record
            row = table.NewRow();
            row["indx"] = 1;
            row["itemCode"] = "MAINITM2";
            row["description"] = "Item 2";
            row["qty"] = 3;
            row["vatable"] = "N";
            row["baseUoM"] = "N";
            //purchase prices
            row["qtyPrPrchsUoM"] = 3;
            row["realBsNetPrchsPrc"] = 2;
            row["realBsGrossPrchsPrc"] = 2;
            row["realNetPrchsPrc"] = 6; // has to be removed soon
            row["realGrossPrchsPrc"] = 6; // has to be removed soon
            row["prcntDscnt"] = 0.00;
            row["amtDscnt"] = 0;
            row["grossPrchsPrc"] = 6;  // has to be removed soon
            row["netPrchsPrc"] = 6;  // has to be removed soon            
            row["rowNetTotal"] = 6;
            row["rowGrossTotal"] = 6;
            //retail
            row["qtyPrRtlUoM"] = 3;
            row["realBsNetPrcRtl"] = 3;
            row["realBsGrossPrcRtl"] = 3;
            row["realNetPrcRtl"] = 9;
            row["realGrossPrcRtl"] = 9;
            row["grossPrcRtl"] = 9; // has to be removed soon
            row["netPrcRtl"] = 9; // has to be removed soon
            row["prcntDscntRtl"] = 0;
            row["amtDscntRtl"] = 0;
            row["rowNetTotalRtl"] = 9;
            row["rowGrossTotalRtl"] = 9;
            table.Rows.Add(row);

            //third record
            row = table.NewRow();
            row["indx"] = 2;
            row["itemCode"] = "MAINITM3";
            row["description"] = "Item 3";
            row["qty"] = 4;
            row["vatable"] = "N";
            row["baseUoM"] = "N";
            //purchase prices
            row["qtyPrPrchsUoM"] = 4;
            row["realBsNetPrchsPrc"] = 2;
            row["realBsGrossPrchsPrc"] = 2;
            row["realNetPrchsPrc"] = 8; // has to be removed soon
            row["realGrossPrchsPrc"] = 8; // has to be removed soon
            row["prcntDscnt"] = 0.00;
            row["amtDscnt"] = 0;
            row["grossPrchsPrc"] = 8;  // has to be removed soon
            row["netPrchsPrc"] = 8;  // has to be removed soon            
            row["rowNetTotal"] = 8;
            row["rowGrossTotal"] = 8;
            //retail
            row["qtyPrRtlUoM"] = 4;
            row["realBsNetPrcRtl"] = 3;
            row["realBsGrossPrcRtl"] = 3;
            row["realNetPrcRtl"] = 12;
            row["realGrossPrcRtl"] = 12;
            row["grossPrcRtl"] = 12; // has to be removed soon
            row["netPrcRtl"] = 12; // has to be removed soon
            row["prcntDscntRtl"] = 0;
            row["amtDscntRtl"] = 0;
            row["rowNetTotalRtl"] = 12;
            row["rowGrossTotalRtl"] = 12;
            table.Rows.Add(row);

            InventoryTransfer inventoryTransfer = new InventoryTransfer();
            Assert.IsTrue(inventoryTransfer.addInventoryTransfer(header, table));
            
            MySqlCommand cmd = new MySqlCommand("select * from inventorytransfer", vars.MySqlConnection);
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Assert.AreEqual(dt.Rows[0]["id"], 1); //first record starts with 1
            Assert.AreEqual(dt.Rows[0]["docId"], "MAIN1");
            Assert.AreEqual(dt.Rows[0]["frmWHouse"], "MAIN");
            Assert.AreEqual(dt.Rows[0]["toWHouse"], "US-FL");
            Assert.AreEqual(dt.Rows[0]["postingDate"], DateTime.Today);
            Assert.AreEqual(dt.Rows[0]["remarks1"], DBNull.Value);
            Assert.AreEqual(dt.Rows[0]["remarks2"], DBNull.Value);
            Assert.AreEqual(dt.Rows[0]["totalPrcntDscnt"], 0m);
            Assert.AreEqual(dt.Rows[0]["totalAmtDscnt"], 0m);
            Assert.AreEqual(dt.Rows[0]["netTotal"], 18m);
            Assert.AreEqual(dt.Rows[0]["grossTotal"], 18m);
            Assert.AreEqual(dt.Rows[0]["totalPrcntDscntRtl"], 0m);
            Assert.AreEqual(dt.Rows[0]["totalAmtDscntRtl"], 0m);
            Assert.AreEqual(dt.Rows[0]["netTotalRtl"], 27m);
            Assert.AreEqual(dt.Rows[0]["grossTotalRtl"], 27m);
            Assert.IsNotNull(dt.Rows[0]["createDate"]);
            Assert.IsNotNull(dt.Rows[0]["createdBy"]);
            Assert.AreEqual("admin", dt.Rows[0]["createdBy"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["updateDate"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["updatedBy"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["transmitted"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["exported"]);

            cmd = new MySqlCommand("select * from inventorytransfer_item order by indx", vars.MySqlConnection);
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt = new DataTable());
            Assert.AreEqual("MAIN1", dt.Rows[0]["docId"]);
            Assert.AreEqual("MAIN1", dt.Rows[1]["docId"]);
            Assert.AreEqual("MAIN1", dt.Rows[2]["docId"]);
            Assert.AreEqual(0, dt.Rows[0]["indx"]);
            Assert.AreEqual(1, dt.Rows[1]["indx"]);
            Assert.AreEqual(2, dt.Rows[2]["indx"]);
            Assert.AreEqual("MAINITM1", dt.Rows[0]["itemCode"]);
            Assert.AreEqual("MAINITM2", dt.Rows[1]["itemCode"]);
            Assert.AreEqual("MAINITM3", dt.Rows[2]["itemCode"]);
            Assert.AreEqual("Item 1", dt.Rows[0]["description"]);
            Assert.AreEqual("Item 2", dt.Rows[1]["description"]);
            Assert.AreEqual("Item 3", dt.Rows[2]["description"]);
            Assert.AreEqual(2m, dt.Rows[0]["realBsNetPrchsPrc"]);
            Assert.AreEqual(2m, dt.Rows[1]["realBsNetPrchsPrc"]);
            Assert.AreEqual(2m, dt.Rows[2]["realBsNetPrchsPrc"]);
            Assert.AreEqual(2m, dt.Rows[0]["realBsGrossPrchsPrc"]);
            Assert.AreEqual(2m, dt.Rows[1]["realBsGrossPrchsPrc"]);
            Assert.AreEqual(2m, dt.Rows[2]["realBsGrossPrchsPrc"]);
            Assert.AreEqual(4m, dt.Rows[0]["realNetPrchsPrc"]);
            Assert.AreEqual(6m, dt.Rows[1]["realNetPrchsPrc"]);
            Assert.AreEqual(8m, dt.Rows[2]["realNetPrchsPrc"]);
            Assert.AreEqual(4m, dt.Rows[0]["realGrossPrchsPrc"]);
            Assert.AreEqual(6m, dt.Rows[1]["realGrossPrchsPrc"]);
            Assert.AreEqual(8m, dt.Rows[2]["realGrossPrchsPrc"]);
            Assert.AreEqual(2m, dt.Rows[0]["qty"]);
            Assert.AreEqual(3m, dt.Rows[1]["qty"]);
            Assert.AreEqual(4m, dt.Rows[2]["qty"]);
            Assert.AreEqual("N", dt.Rows[0]["baseUoM"]);
            Assert.AreEqual("N", dt.Rows[1]["baseUoM"]);
            Assert.AreEqual("N", dt.Rows[2]["baseUoM"]);
            Assert.AreEqual(2m, dt.Rows[0]["qtyPrPrchsUoM"]);
            Assert.AreEqual(3m, dt.Rows[1]["qtyPrPrchsUoM"]);
            Assert.AreEqual(4m, dt.Rows[2]["qtyPrPrchsUoM"]);
            Assert.AreEqual(0m, dt.Rows[0]["prcntDscnt"]);
            Assert.AreEqual(0m, dt.Rows[1]["prcntDscnt"]);
            Assert.AreEqual(0m, dt.Rows[2]["prcntDscnt"]);
            Assert.AreEqual(0m, dt.Rows[0]["amtDscnt"]);
            Assert.AreEqual(0m, dt.Rows[1]["amtDscnt"]);
            Assert.AreEqual(0m, dt.Rows[2]["amtDscnt"]);
            Assert.AreEqual(4m, dt.Rows[0]["netPrchsPrc"]);
            Assert.AreEqual(6m, dt.Rows[1]["netPrchsPrc"]);
            Assert.AreEqual(8m, dt.Rows[2]["netPrchsPrc"]);
            Assert.AreEqual(4m, dt.Rows[0]["grossPrchsPrc"]);
            Assert.AreEqual(6m, dt.Rows[1]["grossPrchsPrc"]);
            Assert.AreEqual(8m, dt.Rows[2]["grossPrchsPrc"]);
            Assert.AreEqual(4m, dt.Rows[0]["rowNetTotal"]);
            Assert.AreEqual(6m, dt.Rows[1]["rowNetTotal"]);
            Assert.AreEqual(8m, dt.Rows[2]["rowNetTotal"]);
            Assert.AreEqual(4m, dt.Rows[0]["rowGrossTotal"]);
            Assert.AreEqual(6m, dt.Rows[1]["rowGrossTotal"]);
            Assert.AreEqual(8m, dt.Rows[2]["rowGrossTotal"]);

            Assert.AreEqual(2m, dt.Rows[0]["qtyPrRtlUoM"]);
            Assert.AreEqual(3m, dt.Rows[1]["qtyPrRtlUoM"]);
            Assert.AreEqual(4m, dt.Rows[2]["qtyPrRtlUoM"]);
            Assert.AreEqual(3m, dt.Rows[0]["realBsNetPrcRtl"]);
            Assert.AreEqual(3m, dt.Rows[1]["realBsNetPrcRtl"]);
            Assert.AreEqual(3m, dt.Rows[2]["realBsNetPrcRtl"]);
            Assert.AreEqual(3m, dt.Rows[0]["realBsGrossPrcRtl"]);
            Assert.AreEqual(3m, dt.Rows[1]["realBsGrossPrcRtl"]);
            Assert.AreEqual(3m, dt.Rows[2]["realBsGrossPrcRtl"]);
            Assert.AreEqual(6m, dt.Rows[0]["realNetPrcRtl"]);
            Assert.AreEqual(9m, dt.Rows[1]["realNetPrcRtl"]);
            Assert.AreEqual(12m, dt.Rows[2]["realNetPrcRtl"]);
            Assert.AreEqual(6m, dt.Rows[0]["realGrossPrcRtl"]);
            Assert.AreEqual(9m, dt.Rows[1]["realGrossPrcRtl"]);
            Assert.AreEqual(12m, dt.Rows[2]["realGrossPrcRtl"]);
            Assert.AreEqual(6m, dt.Rows[0]["netPrcRtl"]);
            Assert.AreEqual(9m, dt.Rows[1]["netPrcRtl"]);
            Assert.AreEqual(12m, dt.Rows[2]["netPrcRtl"]);
            Assert.AreEqual(6m, dt.Rows[0]["grossPrcRtl"]);
            Assert.AreEqual(9m, dt.Rows[1]["grossPrcRtl"]);
            Assert.AreEqual(12m, dt.Rows[2]["grossPrcRtl"]);
            Assert.AreEqual(0m, dt.Rows[0]["prcntDscntRtl"]);
            Assert.AreEqual(0m, dt.Rows[1]["prcntDscntRtl"]);
            Assert.AreEqual(0m, dt.Rows[2]["prcntDscntRtl"]);
            Assert.AreEqual(0m, dt.Rows[0]["amtDscntRtl"]);
            Assert.AreEqual(0m, dt.Rows[1]["amtDscntRtl"]);
            Assert.AreEqual(0m, dt.Rows[2]["amtDscntRtl"]);
            Assert.AreEqual(6m, dt.Rows[0]["rowNetTotalRtl"]);
            Assert.AreEqual(9m, dt.Rows[1]["rowNetTotalRtl"]);
            Assert.AreEqual(12m, dt.Rows[2]["rowNetTotalRtl"]);
            Assert.AreEqual(6m, dt.Rows[0]["rowGrossTotalRtl"]);
            Assert.AreEqual(9m, dt.Rows[1]["rowGrossTotalRtl"]);
            Assert.AreEqual(12m, dt.Rows[2]["rowGrossTotalRtl"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["exported"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[1]["exported"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[2]["exported"]);

            cmd = new MySqlCommand("select inStock from item_warehouse where itemCode='MAINITM1' and whCode='MAIN'", vars.MySqlConnection);
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt = new DataTable());
            Assert.AreEqual(16D, dt.Rows[0]["inStock"]);
            cmd = new MySqlCommand("select inStock from item_warehouse where itemCode='MAINITM1' and whCode='US-FL'", vars.MySqlConnection);
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt = new DataTable());
            Assert.AreEqual(4D, dt.Rows[0]["inStock"]);
            cmd = new MySqlCommand("select inStock from item_warehouse where itemCode='MAINITM2' and whCode='MAIN'", vars.MySqlConnection);
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt = new DataTable());
            Assert.AreEqual(21D, dt.Rows[0]["inStock"]);
            cmd = new MySqlCommand("select inStock from item_warehouse where itemCode='MAINITM2' and whCode='US-FL'", vars.MySqlConnection);
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt = new DataTable());
            Assert.AreEqual(9D, dt.Rows[0]["inStock"]);
            cmd = new MySqlCommand("select inStock from item_warehouse where itemCode='MAINITM3' and whCode='MAIN'", vars.MySqlConnection);
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt = new DataTable());
            Assert.AreEqual(24D, dt.Rows[0]["inStock"]);
            cmd = new MySqlCommand("select inStock from item_warehouse where itemCode='MAINITM3' and whCode='US-FL'", vars.MySqlConnection);
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt = new DataTable());
            Assert.AreEqual(16D, dt.Rows[0]["inStock"]);
        }

        [TestMethod]
        public void testUpdateInventoryTransfer()
        {
            Assert.IsTrue(functions.createInitialRecordsForTheTests());

            // let's assume that we have logged in. Note this should comes first as this is being used in below functions.
            vars.terminalId = "MAIN"; vars.username = "admin";

            //add these pre-requisites records.
            Assert.IsTrue(addWarehouse());
            Assert.IsTrue(addBusinessPartners());
            Assert.IsTrue(registerItems());
            Assert.IsTrue(addDeliveryReceipt()); // We cannot successfully Transfer inventory if we don't have items delivered.


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
            header.Add("netTotal", 18);
            header.Add("grossTotal", 18);
            //retail prices
            header.Add("totalPrcntDscntRtl", 0);
            header.Add("totalAmtDscntRtl", 0);
            header.Add("netTotalRtl", 27);
            header.Add("grossTotalRtl", 27);

            DataRow row;

            //first record
            row = table.NewRow();
            row["indx"] = 0;
            row["itemCode"] = "MAINITM1";
            row["description"] = "Item 1";
            row["qty"] = 2;
            row["vatable"] = "N";
            row["baseUoM"] = "N";
            //purchase prices
            row["qtyPrPrchsUoM"] = 2;
            row["realBsNetPrchsPrc"] = 2;
            row["realBsGrossPrchsPrc"] = 2;
            row["realNetPrchsPrc"] = 4; // has to be removed soon
            row["realGrossPrchsPrc"] = 4; // has to be removed soon
            row["prcntDscnt"] = 0.00;
            row["amtDscnt"] = 0;
            row["grossPrchsPrc"] = 4;  // has to be removed soon
            row["netPrchsPrc"] = 4;  // has to be removed soon            
            row["rowNetTotal"] = 4;
            row["rowGrossTotal"] = 4;
            //retail
            row["qtyPrRtlUoM"] = 2;
            row["realBsNetPrcRtl"] = 3;
            row["realBsGrossPrcRtl"] = 3;
            row["realNetPrcRtl"] = 6;
            row["realGrossPrcRtl"] = 6;
            row["prcntDscntRtl"] = 0;
            row["grossPrcRtl"] = 6; // has to be removed soon
            row["netPrcRtl"] = 6; // has to be removed soon
            row["amtDscntRtl"] = 0;
            row["rowNetTotalRtl"] = 6;
            row["rowGrossTotalRtl"] = 6;
            table.Rows.Add(row);

            //second record
            row = table.NewRow();
            row["indx"] = 1;
            row["itemCode"] = "MAINITM2";
            row["description"] = "Item 2";
            row["qty"] = 3;
            row["vatable"] = "N";
            row["baseUoM"] = "N";
            //purchase prices
            row["qtyPrPrchsUoM"] = 3;
            row["realBsNetPrchsPrc"] = 2;
            row["realBsGrossPrchsPrc"] = 2;
            row["realNetPrchsPrc"] = 6; // has to be removed soon
            row["realGrossPrchsPrc"] = 6; // has to be removed soon
            row["prcntDscnt"] = 0.00;
            row["amtDscnt"] = 0;
            row["grossPrchsPrc"] = 6;  // has to be removed soon
            row["netPrchsPrc"] = 6;  // has to be removed soon            
            row["rowNetTotal"] = 6;
            row["rowGrossTotal"] = 6;
            //retail
            row["qtyPrRtlUoM"] = 3;
            row["realBsNetPrcRtl"] = 3;
            row["realBsGrossPrcRtl"] = 3;
            row["realNetPrcRtl"] = 9;
            row["realGrossPrcRtl"] = 9;
            row["grossPrcRtl"] = 9; // has to be removed soon
            row["netPrcRtl"] = 9; // has to be removed soon
            row["prcntDscntRtl"] = 0;
            row["amtDscntRtl"] = 0;
            row["rowNetTotalRtl"] = 9;
            row["rowGrossTotalRtl"] = 9;
            table.Rows.Add(row);

            //third record
            row = table.NewRow();
            row["indx"] = 2;
            row["itemCode"] = "MAINITM3";
            row["description"] = "Item 3";
            row["qty"] = 4;
            row["vatable"] = "N";
            row["baseUoM"] = "N";
            //purchase prices
            row["qtyPrPrchsUoM"] = 4;
            row["realBsNetPrchsPrc"] = 2;
            row["realBsGrossPrchsPrc"] = 2;
            row["realNetPrchsPrc"] = 8; // has to be removed soon
            row["realGrossPrchsPrc"] = 8; // has to be removed soon
            row["prcntDscnt"] = 0.00;
            row["amtDscnt"] = 0;
            row["grossPrchsPrc"] = 8;  // has to be removed soon
            row["netPrchsPrc"] = 8;  // has to be removed soon            
            row["rowNetTotal"] = 8;
            row["rowGrossTotal"] = 8;
            //retail
            row["qtyPrRtlUoM"] = 4;
            row["realBsNetPrcRtl"] = 3;
            row["realBsGrossPrcRtl"] = 3;
            row["realNetPrcRtl"] = 12;
            row["realGrossPrcRtl"] = 12;
            row["grossPrcRtl"] = 12; // has to be removed soon
            row["netPrcRtl"] = 12; // has to be removed soon
            row["prcntDscntRtl"] = 0;
            row["amtDscntRtl"] = 0;
            row["rowNetTotalRtl"] = 12;
            row["rowGrossTotalRtl"] = 12;
            table.Rows.Add(row);

            InventoryTransfer inventoryTransfer = new InventoryTransfer();
            Assert.IsTrue(inventoryTransfer.addInventoryTransfer(header, table));

            header.Add("remarks2", "Remarks2 has been updated.");
            header.Add("updatedBy", "admin");
            header.Add("docId", "MAIN1");

            Assert.IsTrue(inventoryTransfer.updateInventoryTransfer(header));

            MySqlCommand cmd = new MySqlCommand("select * from inventorytransfer", vars.MySqlConnection);
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual("Remarks2 has been updated.", dt.Rows[0]["remarks2"]);
            Assert.AreEqual("admin", dt.Rows[0]["updatedBy"]);
        }
    }
}
