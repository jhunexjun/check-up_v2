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
    public class SalesInvoiceTest
    {
        [TestInitialize]
        public void setUp()
        {
            userTests user = new userTests();
            user.setUp();
            user.testAddUser();
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
            if (!bp.addBusinessPartner(ht))
                return false;

            // second record
            ht.Clear();
            ht.Add("BPType", 1);
            ht.Add("code", "C-1");
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
            if (!itemMasterData.addItem(items, prices))
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
            items.Add("vendor", "S-1");
            items.Add("deactivated", "N");

            // for prices 0 is purchase order, 1 is retail
            prices = new Hashtable();
            prices.Add(0, 2.00);
            prices.Add(1, 3.00);

            itemMasterData = new ItemMasterData();
            if (!itemMasterData.addItem(items, prices))
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
            prices.Add(0, 2.00);
            prices.Add(1, 3.00);

            itemMasterData = new ItemMasterData();
            if (!itemMasterData.addItem(items, prices))
                return false;
            else
                return true;
        }

        // If no items delivered, no items to sell.
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
            header.Add("netTotal", 180.00);
            header.Add("grossTotal", 180.00);

            DataRow row;

            //1st record
            row = table.NewRow();
            row["indx"] = 0;
            row["vendorCode"] = "S-1";
            row["vendorName"] = "ABC Corporation";
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
            row["vendorName"] = "ABC Enterprises";
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

        private DataTable createTableColumns()
        {
            DataTable table = new DataTable();
            table.Columns.Add("docId");
            table.Columns.Add("indx");
            table.Columns.Add("itemCode");
            table.Columns.Add("description");
            table.Columns.Add("vatable");
            table.Columns.Add("saleUoM");
            table.Columns.Add("qtyPrPrchsUoM");
            table.Columns.Add("qtyPrSaleUoM");
            table.Columns.Add("netBsPrchsPrc");
            table.Columns.Add("grossBsPrchsPrc");
            table.Columns.Add("netBsSalePrc");
            table.Columns.Add("grossBsSalePrc");
            table.Columns.Add("qty");
            table.Columns.Add("baseUoM");
            table.Columns.Add("prcntDscnt");
            table.Columns.Add("amtDscnt");
            table.Columns.Add("netSalePrc");
            table.Columns.Add("grossSalePrc");
            table.Columns.Add("rowNetTotal");
            table.Columns.Add("rowGrossTotal");
            table.Columns.Add("exported");

            return table;
        }

        [TestMethod]
        public void testAddSalesInvoice()
        {
            Assert.IsTrue(functions.createInitialRecordsForTheTests());

            // let's assume that we have logged in. Note this should comes first as this is being used in below functions.
            vars.terminalId = "MAIN"; vars.username = "admin";

            //add these pre-requisites records.
            Assert.IsTrue(this.addBusinessPartners());
            Assert.IsTrue(registerItems());
            Assert.IsTrue(addDeliveryReceipt());

            // Let's check the inventory before we substract it for the sales invoice.
            MySqlCommand cmd = new MySqlCommand("select * from item_warehouse where itemCode = 'MAINITM1' AND whCode = 'MAIN'", vars.MySqlConnection);
            DataTable dt = new DataTable(); MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Assert.AreEqual(20D, dt.Rows[0]["inStock"]);

            //let's create the table for the row
            DataTable table = createTableColumns();

            //let's start now the data to get inserted.
            Hashtable header = new Hashtable();
            header.Add("terminalId", vars.terminalId);
            header.Add("customerCode", "C-1");
            header.Add("customerName", "ABC Corporation");
            header.Add("warehouse", "MAIN");

            DateTime datetime = DateTime.Today;
            header.Add("postingDate", datetime.ToString("yyyy/MM/dd"));
            header.Add("remarks1", "this is remarks 1.");
            header.Add("remarks2", null);
            header.Add("totalPrcntDscnt", 10m);
            header.Add("totalAmtDscnt", 20m);
            header.Add("netTotal", 180m);
            header.Add("grossTotal", 201.6m);
            header.Add("createdBy", "admin");

            DataRow row;
            row = table.NewRow();
            row["indx"] = 0;
            row["itemCode"] = "MAINITM1";
            row["description"] = "Item 1";
            row["vatable"] = "Y";
            row["saleUoM"] = "PC";
            row["qtyPrPrchsUoM"] = 1;
            row["qtyPrSaleUoM"] = 1;
            row["netBsPrchsPrc"] = 10;
            row["grossBsPrchsPrc"] = 11.2;
            row["netBsSalePrc"] = 100;
            row["grossBsSalePrc"] = 112;
            row["qty"] = 2;
            row["baseUoM"] = "N";
            row["prcntDscnt"] = 10;
            row["amtDscnt"] = 22.4;
            row["netSalePrc"] = 20;
            row["grossSalePrc"] = 10;
            row["rowNetTotal"] = 180;
            row["rowGrossTotal"] = 201.6;

            table.Rows.Add(row);

            SalesInvoice salesInvoice = new SalesInvoice();
            Assert.IsTrue(salesInvoice.addSalesInvoice(header, table));

            // Let's check the inventory after we substracted it for the sales invoice
            cmd = new MySqlCommand("select * from item_warehouse where itemCode = 'MAINITM1' AND whCode = 'MAIN'", vars.MySqlConnection);
            dt = new DataTable(); da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual(18D, dt.Rows[0]["inStock"]);

            cmd = new MySqlCommand("select * from salesinvoice", vars.MySqlConnection);
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Assert.AreEqual(1, dt.Rows[0]["id"]);
            Assert.AreEqual("MAIN1", dt.Rows[0]["docId"]);
            Assert.AreEqual("C-1", dt.Rows[0]["customerCode"]);
            Assert.AreEqual("ABC Corporation", dt.Rows[0]["customerName"]);
            Assert.AreEqual("MAIN", dt.Rows[0]["warehouse"]);
            Assert.AreEqual(DateTime.Today.ToString("G"), dt.Rows[0]["postingDate"].ToString());
            Assert.AreEqual("this is remarks 1.", dt.Rows[0]["remarks1"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["remarks2"]);
            Assert.AreEqual(10m, dt.Rows[0]["totalPrcntDscnt"]);
            Assert.AreEqual(20m, dt.Rows[0]["totalAmtDscnt"]);
            Assert.AreEqual(180m, dt.Rows[0]["netTotal"]);
            Assert.AreEqual(201.6m, dt.Rows[0]["grossTotal"]);
            Assert.IsNotNull(dt.Rows[0]["createDate"]);
            Assert.AreEqual("admin", dt.Rows[0]["createdBy"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["updateDate"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["updatedBy"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["exported"]);

            // let's check the row
            cmd = new MySqlCommand("select * from salesinvoice_item", vars.MySqlConnection);
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            Assert.AreEqual("MAIN1", dt.Rows[0]["docId"]);
            Assert.AreEqual("Item 1", dt.Rows[0]["description"]);
            Assert.AreEqual("Y", dt.Rows[0]["vatable"]);
            Assert.AreEqual("PC", dt.Rows[0]["saleUoM"]);
            Assert.AreEqual(1m, dt.Rows[0]["qtyPrPrchsUoM"]);
            Assert.AreEqual(1m, dt.Rows[0]["qtyPrSaleUoM"]);
            Assert.AreEqual(10m, dt.Rows[0]["netBsPrchsPrc"]);
            Assert.AreEqual(100m, dt.Rows[0]["netBsSalePrc"]);
            Assert.AreEqual(112m, dt.Rows[0]["grossBsSalePrc"]);
            Assert.AreEqual(2m, dt.Rows[0]["qty"]);
            Assert.AreEqual("N", dt.Rows[0]["baseUoM"]);
            Assert.AreEqual(10m, dt.Rows[0]["prcntDscnt"]);
            Assert.AreEqual(22.4m, dt.Rows[0]["amtDscnt"]);
            Assert.AreEqual(180m, dt.Rows[0]["rowNetTotal"]);
            Assert.AreEqual(201.6m, dt.Rows[0]["rowGrossTotal"]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["exported"]);
        }
    }
}
