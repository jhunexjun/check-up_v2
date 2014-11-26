using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Check_up.classes
{
    public class ItemMasterData
    {
        private int priceList(string s)
        {
            s = s.ToLower();
            if (s == "purchase price")
                return 0;
            else if (s == "retail price")
                return 1;
            else
            {
                MessageBox.Show("Unrecognized price.");
                return -1;
            }
        }

        private string priceList(int i)
        {
            if (i == 0)
                return "Purchase price";
            else if (i == 1)
                return "Retail price";
            else
            {
                MessageBox.Show("Unrecognized price code.");
                return "";
            }
        }

        private Hashtable formatParams(Hashtable ht)
        {
            if (ht.Contains("description") && ht["description"].ToString() != "")
                ht["description"] = "'" + ht["description"].ToString().Replace("'", "''") + "'";
            else
                ht["description"] = "null";

            if (ht.Contains("shortName") && ht["shortName"].ToString() != "")
                ht["shortName"] = "'" + ht["shortName"].ToString().Replace("'", "''") + "'";
            else
                ht["shortName"] = "null";

           ht["vatable"] = "'" + ht["vatable"] + "'";

           if (ht.Contains("vendor") && ht["vendor"].ToString() != "")
                ht["vendor"] = "'" + ht["vendor"] + "'";
            else
                ht["vendor"] = "null";

           if (ht.Contains("remarks") && ht["remarks"].ToString() != "")
                ht["remarks"] = "'" + ht["remarks"].ToString().Replace("'", "''") + "'";
            else
                ht["remarks"] = "null";

            if (ht.Contains("deactivated"))
                ht["deactivated"] = "'" + ht["deactivated"] + "'";
            else
                ht["deactivated"] = "'N'"; // let's put a default value

            // if not create date, db should create it

            // updateDate and updatedBy are default to null

            ht["prchsUoM"] = "'" + ht["prchsUoM"] + "'";
            ht["saleUoM"] = "'" + ht["saleUoM"] + "'";
            ht["varWeightItm"] = "'" + ht["varWeightItm"] + "'";
            
            return ht;
        }

        //this checks the data passed for data integrity
        private bool checkPassedData(Hashtable itemMasterData)
        {
            if (!itemMasterData.Contains("vatable"))
            {
                MessageBox.Show("Please indicate vatable in the hash.");
                return false;
            }
            if (!itemMasterData.Contains("qtyPrPrchsUoM"))
            {
                MessageBox.Show("Please indicate quantity per purchase UoM in the hash.");
                return false;
            }
            if (!itemMasterData.Contains("qtyPrSaleUoM"))
            {
                MessageBox.Show("Please indicate quantity per sale UoM in the hash.");
                return false;
            }
            if (!itemMasterData.Contains("prchsUoM"))
            {
                MessageBox.Show("Please indicate purchase UoM in the hash.");
                return false;
            }
            if (!itemMasterData.Contains("saleUoM"))
            {
                MessageBox.Show("Please indicate purchase UoM in the hash.");
                return false;
            }
            if (!itemMasterData.Contains("varWeightItm"))
            {
                MessageBox.Show("Please indicate variable weight item in the hash.");
                return false;
            }
            if (!itemMasterData.Contains("minStock"))
            {
                MessageBox.Show("Please indicate minimum stock in the hash.");
                return false;
            }
            if (!itemMasterData.Contains("maxStock"))
            {
                MessageBox.Show("Please indicate max stock in the hash.");
                return false;
            }
            if (!itemMasterData.Contains("createdBy"))
            {
                MessageBox.Show("Please indicate 'created by' in the hash.");
                return false;
            }

            return true;
        }

        public bool addItem(Hashtable itemMasterData, Hashtable prices, ArrayList barcodes = null)
        {
            // if there's no values of the ff exit immediately as they are not nullable items.
            if (!checkPassedData(itemMasterData))
                return false;            

            itemMasterData = formatParams(itemMasterData);

            string sql = "START TRANSACTION;";
            sql += "SET @date=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s');";
            sql += "SET @username='" + vars.username + "';";
            sql += "SET @varWeightItm=" + itemMasterData["varWeightItm"] + ";";
            sql += "SET @newId=(SELECT CAST(lastNo+1 AS char(11)) FROM documents WHERE documentCode='IMD');";
            sql += "SET @itemCode=CONCAT('" + vars.terminalId + "', 'ITM', @newId);";
            if (itemMasterData.Contains("vendor"))
                sql += "SET @vendor=(SELECT code FROM businesspartner WHERE code=" + itemMasterData["vendor"] + " AND BPType=0 AND deactivated='N');";

            sql += "INSERT INTO itemmasterdata(itemCode,description,shortName,vatable,vendor,deactivated,qtyPrPrchsUoM,qtyPrSaleUoM,prchsUoM,saleUoM,varWeightItm,remarks,minStock,maxStock,createdBy,createDate)";
            sql += " VALUES(@itemCode," + itemMasterData["description"] + "," + itemMasterData["shortName"] + "," + itemMasterData["vatable"] + "," + itemMasterData["vendor"] + "," + itemMasterData["deactivated"] + "," + itemMasterData["qtyPrPrchsUoM"] + "," + itemMasterData["qtyPrSaleUoM"] + "," + itemMasterData["prchsUoM"] + "," + itemMasterData["saleUoM"] + "," + itemMasterData["varWeightItm"] + "," + itemMasterData["remarks"] + "," + itemMasterData["minStock"] + "," + itemMasterData["maxStock"] + ",@username,@date);";
            
            int priceListCode; double thePrice;
            foreach (DictionaryEntry item in prices)
            {
                priceListCode = (int)item.Key;
                thePrice = Convert.ToDouble(prices[item.Key]);
                sql += "INSERT INTO pricelist(itemCode,priceListCode,netPrice,createdBy,createDate) VALUES(@itemCode," + priceListCode + "," + thePrice + ", @username,@date);";
            }

            // int rowCount = barcodes.Count;
            if (barcodes != null)
                foreach( string barcode in barcodes)
                    sql += "INSERT INTO barcode(itemCode,barcode,createDate,createdBy) VALUES(@itemCode,'" + barcode + "',@date,@username);";

            sql += "UPDATE documents SET lastNo=CAST(@newId AS UNSIGNED) WHERE documentCode='IMD';";
            sql += " COMMIT;";

            MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
            if (cmd.ExecuteNonQuery() > 0)
                return true;
            else
                return false;
        }
    }
}
