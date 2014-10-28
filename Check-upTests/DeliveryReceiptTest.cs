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
            ht.Add("code", "s-7");
            ht.Add("BPName", "XYZ Enterprises");
            ht.Add("createdBy", 0);

            BusinessPartner bp = new BusinessPartner();
            if (!bp.addBusinessPartner(ht))
                return false;

            // second record
            ht.Clear();
            ht.Add("BPType", 0);
            ht.Add("code", "s-15");
            ht.Add("BPName", "ABC Corporation");
            ht.Add("createdBy", 0);

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
            items.Add("createdBy", vars.user_id);
            items.Add("description", "REAR SPROCKET  RS100 42T MOTOX");
            items.Add("vendor", "s-7");
            items.Add("deactivated", "N");

            // for prices 0 is purchase order, 1 is retail
            Hashtable prices = new Hashtable();
            prices.Add(0, 155);
            prices.Add(1, 300);

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
            items.Add("createdBy", vars.user_id);
            items.Add("description", "CHANGE PEDAL XRM");
            items.Add("vendor", "s-7");
            items.Add("deactivated", "N");

            // for prices 0 is purchase order, 1 is retail
            prices = new Hashtable();
            prices.Add(0, 92);
            prices.Add(1, 200);

            itemMasterData = new ItemMasterData();
            if (!itemMasterData.addItem(items, prices))
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
            items.Add("createdBy", vars.user_id);
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
        public void addDeliveryReceipt()
        {

        }
    }
}
