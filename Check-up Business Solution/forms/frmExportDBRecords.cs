using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections;

namespace Check_up.forms
{
    public partial class frmExportDBRecords : Form
    {
        DataTable dt;
        database db;
        string sql; int i, rowCount;

        public frmExportDBRecords()
        {
            InitializeComponent();
            listView1.ItemChecked += new ItemCheckedEventHandler(listView1_ItemChecked);
        }

        void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            e.Item.Checked = true;
        }

        private void insertIntoExportedFiles(string filename, string fullPath)
        {
            fullPath = fullPath.Replace(@"\", @"\\");
            sql = "SET @date=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s');";
            sql += "INSERT INTO export_importfiles(traffic,filename,filepath,createdBy,createDate) VALUES('Out','" + filename + "','" + fullPath + "','" + vars.username + "',@date);";
            db = new database(); dt = new DataTable();
            db.executeNonQuery(sql, vars.MySqlConnection);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            DialogResult messageboxResult = MessageBox.Show(this, "Export files?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (messageboxResult != DialogResult.Yes)
                return;

            JsonSerializer serializer;
            ArrayList iDs_array;
            string tableName = "users_"; //with underscore
            string timeStamp = fx.getTimeStamp(DateTime.Now);
            string filename = tableName + timeStamp + ".json";
            string fullPath = Application.StartupPath + @"\json\Out\" + filename;

            //Let's export users records
            sql = "SELECT * from users WHERE exported IS NULL OR exported = 0 ORDER BY user_id";
            db = new database(); dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count > 0)
            {
                using (StreamWriter file = File.CreateText(fullPath))
                {
                    serializer = new JsonSerializer();
                    serializer.Serialize(file, dt);
                }
                rowCount = dt.Rows.Count;
                iDs_array = new ArrayList();
                for (i = 0; i < rowCount; i++)
                    iDs_array.Add(dt.Rows[i]["user_id"].ToString());

                rowCount = iDs_array.Count;
                for (i = 0; i < rowCount; i++)
                {
                    sql = "UPDATE users SET exported=1 WHERE user_id=" + iDs_array[i];
                    db = new database(); dt = new DataTable();
                    db.executeNonQuery(sql, vars.MySqlConnection);
                }

                insertIntoExportedFiles(filename, fullPath);
            }

            //Let's export warehouse records
            tableName = "warehouse_"; //with underscore
            filename = tableName + timeStamp + ".json";
            fullPath = Application.StartupPath + @"\json\Out\" + filename;
            sql = "SELECT * from warehouse WHERE exported IS NULL OR exported = 0 ORDER BY id";
            db = new database(); dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count > 0)
            {
                using (StreamWriter file = File.CreateText(fullPath))
                {
                    serializer = new JsonSerializer();
                    serializer.Serialize(file, dt);
                }
                rowCount = dt.Rows.Count;
                iDs_array = new ArrayList();
                for (i = 0; i < rowCount; i++)
                    iDs_array.Add(dt.Rows[i]["id"].ToString());

                rowCount = iDs_array.Count; sql = "";
                for (i = 0; i < rowCount; i++)
                    sql += "UPDATE warehouse SET exported=1 WHERE id=" + iDs_array[i] + ";";

                db = new database(); dt = new DataTable();
                db.executeNonQuery(sql, vars.MySqlConnection);

                insertIntoExportedFiles(filename, fullPath);
            }

            //Let's export businesspartner records.
            tableName = "businesspartner_"; //with underscore            
            filename = tableName + timeStamp + ".json";
            fullPath = Application.StartupPath + @"\json\Out\" + filename;

            sql = "SELECT * from businesspartner WHERE exported IS NULL OR exported = 0 ORDER BY `code`";            
            db = new database(); dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count > 0)
            {
                using (StreamWriter file = File.CreateText(fullPath))
                {
                    serializer = new JsonSerializer();
                    serializer.Serialize(file, dt);
                }
                rowCount = dt.Rows.Count;
                iDs_array = new ArrayList();
                for (i = 0; i < rowCount; i++)
                    iDs_array.Add(dt.Rows[i]["code"].ToString());

                rowCount = iDs_array.Count; sql = "";
                for (i = 0; i < rowCount; i++)
                    sql += "UPDATE businesspartner SET exported=1 WHERE `code`='" + iDs_array[i] + "';";

                db = new database(); dt = new DataTable();
                db.executeNonQuery(sql, vars.MySqlConnection);

                insertIntoExportedFiles(filename, fullPath);
            }

            //Let's export itemmasterdata records.
            tableName = "itemmasterdata_"; //with underscore            
            filename = tableName + timeStamp + ".json";
            fullPath = Application.StartupPath + @"\json\Out\" + filename;

            sql = "SELECT * from itemmasterdata WHERE exported IS NULL OR exported = 0 ORDER BY id";
            db = new database(); dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count > 0)
            {
                using (StreamWriter file = File.CreateText(fullPath))
                {
                    serializer = new JsonSerializer();
                    serializer.Serialize(file, dt);
                }
                rowCount = dt.Rows.Count;
                iDs_array = new ArrayList();
                for (i = 0; i < rowCount; i++)
                    iDs_array.Add(dt.Rows[i]["itemCode"].ToString());

                rowCount = iDs_array.Count; sql = "";
                for (i = 0; i < rowCount; i++)
                    sql += "UPDATE itemmasterdata SET exported=1 WHERE itemCode='" + iDs_array[i] + "';";

                db = new database(); dt = new DataTable();
                db.executeNonQuery(sql, vars.MySqlConnection);

                insertIntoExportedFiles(filename, fullPath);
            }

            //Let's export pricelist
            tableName = "pricelist_"; //with underscore            
            filename = tableName + timeStamp + ".json";
            fullPath = Application.StartupPath + @"\json\Out\" + filename;

            sql = "SELECT * from pricelist WHERE exported IS NULL OR exported = 0 ORDER BY itemCode, priceListCode";
            db = new database(); dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count > 0)
            {
                using (StreamWriter file = File.CreateText(fullPath))
                {
                    serializer = new JsonSerializer();
                    serializer.Serialize(file, dt);
                }
                rowCount = dt.Rows.Count;
                
                DataTable pricelist = new DataTable();
                DataColumn column;
                DataRow row;

                column = pricelist.Columns.Add("itemCode", typeof(String));
                pricelist.Columns.Add("priceListCode", typeof(Int16));

                for (i = 0; i < rowCount; i++)
                {
                    row = pricelist.NewRow();
                    row["itemCode"] = dt.Rows[i]["itemCode"].ToString();
                    row["priceListCode"] = dt.Rows[i]["priceListCode"].ToString();
                    pricelist.Rows.Add(row);
                }

                rowCount = pricelist.Rows.Count; sql = "";
                for (i = 0; i < rowCount; i++)
                    sql += "UPDATE pricelist SET exported=1 WHERE itemCode='" + pricelist.Rows[i]["itemCode"].ToString() + "' AND priceListCode=" + pricelist.Rows[i]["priceListCode"].ToString() + ";";
                
                db = new database(); dt = new DataTable();
                db.executeNonQuery(sql, vars.MySqlConnection);
                insertIntoExportedFiles(filename, fullPath);
            }

            // Let's export pricelisthistory
            tableName = "pricelisthistory_"; //with underscore            
            filename = tableName + timeStamp + ".json";
            fullPath = Application.StartupPath + @"\json\Out\" + filename;

            sql = "SELECT * from pricelisthistory WHERE exported IS NULL OR exported = 0 ORDER BY itemCode, priceListCode";
            db = new database(); dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count > 0)
            {
                using (StreamWriter file = File.CreateText(fullPath))
                {
                    serializer = new JsonSerializer();
                    serializer.Serialize(file, dt);
                }
                rowCount = dt.Rows.Count;

                DataTable pricelist = new DataTable();
                DataColumn column;
                DataRow row;

                column = pricelist.Columns.Add("itemCode", typeof(String));
                pricelist.Columns.Add("priceListCode", typeof(Int16));

                for (i = 0; i < rowCount; i++)
                {
                    row = pricelist.NewRow();
                    row["itemCode"] = dt.Rows[i]["itemCode"].ToString();
                    row["priceListCode"] = dt.Rows[i]["priceListCode"].ToString();
                    pricelist.Rows.Add(row);
                }

                rowCount = pricelist.Rows.Count; sql = "";
                for (i = 0; i < rowCount; i++)
                    sql += "UPDATE pricelisthistory SET exported=1 WHERE itemCode='" + pricelist.Rows[i]["itemCode"].ToString() + "' AND priceListCode=" + pricelist.Rows[i]["priceListCode"].ToString() + ";";

                db = new database(); dt = new DataTable();
                db.executeNonQuery(sql, vars.MySqlConnection);

                insertIntoExportedFiles(filename, fullPath);
            }


            //Let's export barcode
            tableName = "barcode_"; //with underscore            
            filename = tableName + timeStamp + ".json";
            fullPath = Application.StartupPath + @"\json\Out\" + filename;

            sql = "SELECT * from barcode WHERE exported IS NULL OR exported = 0 ORDER BY itemCode, barcode";
            db = new database(); dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count > 0)
            {
                using (StreamWriter file = File.CreateText(fullPath))
                {
                    serializer = new JsonSerializer();
                    serializer.Serialize(file, dt);
                }
                rowCount = dt.Rows.Count;

                DataTable barcode = new DataTable();
                DataColumn column;
                DataRow row;

                column = barcode.Columns.Add("itemCode", typeof(String));
                barcode.Columns.Add("barcode", typeof(String));

                for (i = 0; i < rowCount; i++)
                {
                    row = barcode.NewRow();
                    row["itemCode"] = dt.Rows[i]["itemCode"].ToString();
                    row["barcode"] = dt.Rows[i]["barcode"].ToString();
                    barcode.Rows.Add(row);
                }

                rowCount = barcode.Rows.Count; sql = "";
                for (i = 0; i < rowCount; i++)
                    sql += "UPDATE barcode SET exported=1 WHERE itemCode='" + barcode.Rows[i]["itemCode"].ToString() + "' AND barcode='" + barcode.Rows[i]["barcode"].ToString() + "';";

                db = new database(); dt = new DataTable();
                db.executeNonQuery(sql, vars.MySqlConnection);

                insertIntoExportedFiles(filename, fullPath);
            }

            // Let's export barcode history
            tableName = "barcodehistory_"; //with underscore            
            filename = tableName + timeStamp + ".json";
            fullPath = Application.StartupPath + @"\json\Out\" + filename;

            sql = "SELECT * from barcodehistory WHERE exported IS NULL OR exported = 0 ORDER BY itemCode, barcode";
            db = new database(); dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count > 0)
            {
                using (StreamWriter file = File.CreateText(fullPath))
                {
                    serializer = new JsonSerializer();
                    serializer.Serialize(file, dt);
                }
                rowCount = dt.Rows.Count;

                DataTable barcodehistory = new DataTable();
                DataColumn column;
                DataRow row;

                column = barcodehistory.Columns.Add("itemCode", typeof(String));
                barcodehistory.Columns.Add("barcode", typeof(String));

                for (i = 0; i < rowCount; i++)
                {
                    row = barcodehistory.NewRow();
                    row["itemCode"] = dt.Rows[i]["itemCode"].ToString();
                    row["barcode"] = dt.Rows[i]["barcode"].ToString();
                    barcodehistory.Rows.Add(row);
                }

                rowCount = barcodehistory.Rows.Count; sql = "";
                for (i = 0; i < rowCount; i++)
                    sql += "UPDATE barcodehistory SET exported=1 WHERE itemCode='" + barcodehistory.Rows[i]["itemCode"].ToString() + "' AND barcode='" + barcodehistory.Rows[i]["barcode"].ToString() + "';";

                db = new database(); dt = new DataTable();
                db.executeNonQuery(sql, vars.MySqlConnection);

                insertIntoExportedFiles(filename, fullPath);
            }

                        
            foreach (ListViewItem item in listView1.Items)
            {
                if (!itemsToBeExcluded(item.Text))
                {
                    // let's export purchase order. Of course row data are included.
                    if (item.Text == "Purchase Orders" && item.Checked == true)
                    {
                        tableName = "purchaseorder_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from purchaseorder WHERE exported IS NULL OR exported = 0 ORDER BY id";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            iDs_array = new ArrayList();
                            for (i = 0; i < rowCount; i++)
                                iDs_array.Add(dt.Rows[i]["id"].ToString());

                            rowCount = iDs_array.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE purchaseorder SET exported=1 WHERE id=" + iDs_array[i] + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }

                        // purchaseorder_item
                        tableName = "purchaseorder-item_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from purchaseorder_item WHERE exported IS NULL OR exported = 0 ORDER BY docId,Indx";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            DataTable PO = new DataTable();
                            DataRow row;

                            PO.Columns.Add("docId", typeof(string));
                            PO.Columns.Add("indx", typeof(int));

                            for (i = 0; i < rowCount; i++)
                            {
                                row = PO.NewRow();
                                row["docId"] = dt.Rows[i]["docId"].ToString();
                                row["indx"] = dt.Rows[i]["indx"].ToString();
                                PO.Rows.Add(row);
                            }

                            rowCount = PO.Rows.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE purchaseorder_item SET exported=1 WHERE docId='" + PO.Rows[i]["docId"].ToString() + "' AND indx=" + PO.Rows[i]["indx"].ToString() + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }
                    }

                    // let's export Goods Receipt PO's
                    if (item.Text == "Goods Receipt PO's" && item.Checked == true)
                    {
                        tableName = "grpo_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from grpo WHERE exported IS NULL OR exported = 0 ORDER BY id";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            iDs_array = new ArrayList();
                            for (i = 0; i < rowCount; i++)
                                iDs_array.Add(dt.Rows[i]["id"].ToString());

                            rowCount = iDs_array.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE grpo SET exported=1 WHERE id=" + iDs_array[i] + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }

                        // grpo_item
                        tableName = "grpo-item_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from grpo_item WHERE exported IS NULL OR exported = 0 ORDER BY docId,Indx";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            DataTable GRPO = new DataTable();
                            DataRow row;

                            GRPO.Columns.Add("docId", typeof(string));
                            GRPO.Columns.Add("indx", typeof(int));

                            for (i = 0; i < rowCount; i++)
                            {
                                row = GRPO.NewRow();
                                row["docId"] = dt.Rows[i]["docId"].ToString();
                                row["indx"] = dt.Rows[i]["indx"].ToString();
                                GRPO.Rows.Add(row);
                            }

                            rowCount = GRPO.Rows.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE grpo_item SET exported=1 WHERE docId='" + GRPO.Rows[i]["docId"].ToString() + "' AND indx=" + GRPO.Rows[i]["indx"].ToString() + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }
                    }

                    // let's export Delivery Receipt. Of course row data are included.
                    if (item.Text == "Delivery Receipt" && item.Checked == true)
                    {
                        tableName = "deliveryreceipt_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from deliveryreceipt WHERE exported IS NULL OR exported = 0 ORDER BY id";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            iDs_array = new ArrayList();
                            for (i = 0; i < rowCount; i++)
                                iDs_array.Add(dt.Rows[i]["id"].ToString());

                            rowCount = iDs_array.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE deliveryreceipt SET exported=1 WHERE id=" + iDs_array[i] + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }

                        // deliveryreceipt_item
                        tableName = "deliveryreceipt-item_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from deliveryreceipt_item WHERE exported IS NULL OR exported = 0 ORDER BY docId,Indx";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            DataTable DR = new DataTable();
                            DataRow row;

                            DR.Columns.Add("docId", typeof(string));
                            DR.Columns.Add("indx", typeof(int));

                            for (i = 0; i < rowCount; i++)
                            {
                                row = DR.NewRow();
                                row["docId"] = dt.Rows[i]["docId"].ToString();
                                row["indx"] = dt.Rows[i]["indx"].ToString();
                                DR.Rows.Add(row);
                            }

                            rowCount = DR.Rows.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE deliveryreceipt_item SET exported=1 WHERE docId='" + DR.Rows[i]["docId"].ToString() + "' AND indx=" + DR.Rows[i]["indx"].ToString() + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }
                    }

                    // let's export Inventory Posting. Of course row data are included.
                    if (item.Text == "Inventory Posting" && item.Checked == true)
                    {
                        tableName = "inventoryposting_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from inventoryposting WHERE exported IS NULL OR exported = 0 ORDER BY id";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            iDs_array = new ArrayList();
                            for (i = 0; i < rowCount; i++)
                                iDs_array.Add(dt.Rows[i]["id"].ToString());

                            rowCount = iDs_array.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE inventoryposting SET exported=1 WHERE id=" + iDs_array[i] + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }

                        // inventoryposting_item
                        tableName = "inventoryposting-item_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from inventoryposting_item WHERE exported IS NULL OR exported = 0 ORDER BY docId,Indx";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            DataTable IP = new DataTable();
                            DataRow row;

                            IP.Columns.Add("docId", typeof(string));
                            IP.Columns.Add("indx", typeof(int));

                            for (i = 0; i < rowCount; i++)
                            {
                                row = IP.NewRow();
                                row["docId"] = dt.Rows[i]["docId"].ToString();
                                row["indx"] = dt.Rows[i]["indx"].ToString();
                                IP.Rows.Add(row);
                            }

                            rowCount = IP.Rows.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE inventoryposting_item SET exported=1 WHERE docId='" + IP.Rows[i]["docId"].ToString() + "' AND indx=" + IP.Rows[i]["indx"].ToString() + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }
                    }

                    // let's export Goods Returns
                    if (item.Text == "Goods Returns" && item.Checked == true)
                    {
                        tableName = "goodsreturn_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from goodsreturn WHERE exported IS NULL OR exported = 0 ORDER BY id";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            iDs_array = new ArrayList();
                            for (i = 0; i < rowCount; i++)
                                iDs_array.Add(dt.Rows[i]["id"].ToString());

                            rowCount = iDs_array.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE goodsreturn SET exported=1 WHERE id=" + iDs_array[i] + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }

                        // goodsreturn_item
                        tableName = "goodsreturn-item_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from goodsreturn_item WHERE exported IS NULL OR exported = 0 ORDER BY docId,Indx";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            DataTable goodsReturn = new DataTable();
                            DataRow row;

                            goodsReturn.Columns.Add("docId", typeof(string));
                            goodsReturn.Columns.Add("indx", typeof(int));

                            for (i = 0; i < rowCount; i++)
                            {
                                row = goodsReturn.NewRow();
                                row["docId"] = dt.Rows[i]["docId"].ToString();
                                row["indx"] = dt.Rows[i]["indx"].ToString();
                                goodsReturn.Rows.Add(row);
                            }

                            rowCount = goodsReturn.Rows.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE goodsreturn_item SET exported=1 WHERE docId='" + goodsReturn.Rows[i]["docId"].ToString() + "' AND indx=" + goodsReturn.Rows[i]["indx"].ToString() + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }
                    }

                    // let's export inventory transfer
                    if (item.Text == "Inventory Transfer" && item.Checked == true)
                    {
                        tableName = "inventorytransfer_";
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from inventorytransfer WHERE exported IS NULL OR exported = 0 ORDER BY id";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            iDs_array = new ArrayList();
                            for (i = 0; i < rowCount; i++)
                                iDs_array.Add(dt.Rows[i]["id"].ToString());

                            rowCount = iDs_array.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE inventorytransfer SET exported=1 WHERE id=" + iDs_array[i] + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }

                        // inventorytransfer_item
                        tableName = "inventorytransfer-item_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from inventorytransfer_item WHERE exported IS NULL OR exported = 0 ORDER BY docId,Indx";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            DataTable inventoryTransfer = new DataTable();
                            DataRow row;

                            inventoryTransfer.Columns.Add("docId", typeof(string));
                            inventoryTransfer.Columns.Add("indx", typeof(int));

                            for (i = 0; i < rowCount; i++)
                            {
                                row = inventoryTransfer.NewRow();
                                row["docId"] = dt.Rows[i]["docId"].ToString();
                                row["indx"] = dt.Rows[i]["indx"].ToString();
                                inventoryTransfer.Rows.Add(row);
                            }

                            rowCount = inventoryTransfer.Rows.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE inventorytransfer_item SET exported=1 WHERE docId='" + inventoryTransfer.Rows[i]["docId"].ToString() + "' AND indx=" + inventoryTransfer.Rows[i]["indx"].ToString() + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }
                    }

                    // let's export sales invoices
                    if (item.Text == "Sales Invoices" && item.Checked == true)
                    {
                        tableName = "salesinvoice_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from salesinvoice WHERE exported IS NULL OR exported = 0 ORDER BY id";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            iDs_array = new ArrayList();
                            for (i = 0; i < rowCount; i++)
                                iDs_array.Add(dt.Rows[i]["id"].ToString());

                            rowCount = iDs_array.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE salesinvoice SET exported=1 WHERE id=" + iDs_array[i] + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }

                        // salesinvoice_item
                        tableName = "salesinvoice-item_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from salesinvoice_item WHERE exported IS NULL OR exported = 0 ORDER BY docId,Indx";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            DataTable salesinvoice = new DataTable();
                            DataRow row;

                            salesinvoice.Columns.Add("docId", typeof(string));
                            salesinvoice.Columns.Add("indx", typeof(int));

                            for (i = 0; i < rowCount; i++)
                            {
                                row = salesinvoice.NewRow();
                                row["docId"] = dt.Rows[i]["docId"].ToString();
                                row["indx"] = dt.Rows[i]["indx"].ToString();
                                salesinvoice.Rows.Add(row);
                            }

                            rowCount = salesinvoice.Rows.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE salesinvoice_item SET exported=1 WHERE docId='" + salesinvoice.Rows[i]["docId"].ToString() + "' AND indx=" + salesinvoice.Rows[i]["indx"].ToString() + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }
                    }

                    // let's export sales returns
                    if (item.Text == "Sales Returns" && item.Checked == true)
                    {
                        tableName = "salesreturn_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from salesreturn WHERE exported IS NULL OR exported = 0 ORDER BY id";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            iDs_array = new ArrayList();
                            for (i = 0; i < rowCount; i++)
                                iDs_array.Add(dt.Rows[i]["id"].ToString());

                            rowCount = iDs_array.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE salesreturn SET exported=1 WHERE id=" + iDs_array[i] + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }

                        // salesreturn_item
                        tableName = "salesreturn-item_"; //with underscore            
                        filename = tableName + timeStamp + ".json";
                        fullPath = Application.StartupPath + @"\json\Out\" + filename;

                        sql = "SELECT * from salesreturn_item WHERE exported IS NULL OR exported = 0 ORDER BY docId,Indx";
                        db = new database(); dt = new DataTable();
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            using (StreamWriter file = File.CreateText(fullPath))
                            {
                                serializer = new JsonSerializer();
                                serializer.Serialize(file, dt);
                            }
                            rowCount = dt.Rows.Count;
                            DataTable salesReturn = new DataTable();
                            DataRow row;

                            salesReturn.Columns.Add("docId", typeof(string));
                            salesReturn.Columns.Add("indx", typeof(int));

                            for (i = 0; i < rowCount; i++)
                            {
                                row = salesReturn.NewRow();
                                row["docId"] = dt.Rows[i]["docId"].ToString();
                                row["indx"] = dt.Rows[i]["indx"].ToString();
                                salesReturn.Rows.Add(row);
                            }

                            rowCount = salesReturn.Rows.Count; sql = "";
                            for (i = 0; i < rowCount; i++)
                                sql += "UPDATE salesreturn_item SET exported=1 WHERE docId='" + salesReturn.Rows[i]["docId"].ToString() + "' AND indx=" + salesReturn.Rows[i]["indx"].ToString() + ";";

                            db = new database(); dt = new DataTable();
                            db.executeNonQuery(sql, vars.MySqlConnection);

                            insertIntoExportedFiles(filename, fullPath);
                        }
                    }
                }
            }

            MessageBox.Show(this, "Successfully exported files to " + Application.StartupPath + @"\json\Out\ directory." , "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        // items that will return true means it's a transactional tables.
        private bool itemsToBeExcluded(string s)
        {
            ArrayList excluded = new ArrayList();
            excluded.Add("Users");
            excluded.Add("Warehouse");
            excluded.Add("Item Master Data");
            excluded.Add("Price Lists");
            excluded.Add("Price Lists History");
            excluded.Add("Barcodes");
            excluded.Add("Barcodes History");

            if (excluded.Contains(s))
                return true;
            else
                return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
