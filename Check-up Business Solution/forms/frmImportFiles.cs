﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Check_up.forms
{
    public partial class frmImportFiles : Form
    {
        string filename = "", fullPath = "", sql = "";
        int position;
        database db; DataTable dt;

        MySqlCommand cmd;

        public frmImportFiles()
        {
            InitializeComponent();
        }

        private string transformString4SQL(string s)
        {
            if (s == "")
                s = "null";
            else
                s = "'" + s.Replace("'", "''") + "'";

            return s;
        }

        private string transformDate4SQL(string s)
        {
            if (s == "")
                s = "null";
            else
            {
                DateTime dateTime = DateTime.Parse(s);
                s = "'" + dateTime.ToString("yyyy/MM/dd HH:mm:ss") + "'";
            }

            return s;
        }

        private string transformDate4SQLNotNull(string s)
        {
            DateTime dateTime = DateTime.Parse(s);
            s = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
            return s;
        }

        private DataTable sortFiles(string[] filesFullPath)
        {
            DataTable filesList = new DataTable();
            DataRow row;

            filesList.Columns.Add("No", typeof(int));
            filesList.Columns.Add("Filename", typeof(string));
            filesList.Columns.Add("FileFullPath", typeof(string));

            foreach(string fileFullPath in filesFullPath)
            {
                position = fileFullPath.LastIndexOf(@"\");
                filename = fileFullPath.Substring(position + 1);

                row = filesList.NewRow();
                if (filename.StartsWith("users_"))
                    row["No"] = 1;
                if (filename.StartsWith("warehouse_"))
                    row["No"] = 2;
                if (filename.StartsWith("businesspartner_"))
                    row["No"] = 3;
                if (filename.StartsWith("itemmasterdata_"))
                    row["No"] = 4;
                if (filename.StartsWith("pricelist_"))
                    row["No"] = 5;
                if (filename.StartsWith("pricelisthistory_"))
                    row["No"] = 6;
                if (filename.StartsWith("barcode_"))
                    row["No"] = 7;
                if (filename.StartsWith("barcodehistory_"))
                    row["No"] = 8;
                if (filename.StartsWith("purchaseorder_"))
                    row["No"] = 9;
                if (filename.StartsWith("purchaseorder-item_"))
                    row["No"] = 10;
                if (filename.StartsWith("grpo_"))
                    row["No"] = 11;
                if (filename.StartsWith("grpo-item_"))
                    row["No"] = 12;
                if (filename.StartsWith("inventorytransfer_"))
                    row["No"] = 13;
                if (filename.StartsWith("inventorytransfer-item_"))
                    row["No"] = 14;
                if (filename.StartsWith("goodsreturn_"))
                    row["No"] = 15;
                if (filename.StartsWith("goodsreturn-item_"))
                    row["No"] = 16;
                if (filename.StartsWith("salesinvoice_"))
                    row["No"] = 17;
                if (filename.StartsWith("salesinvoice-item_"))
                    row["No"] = 18;
                if (filename.StartsWith("salesreturn_"))
                    row["No"] = 19;
                if (filename.StartsWith("salesreturn-item_"))
                    row["No"] = 20;
                if (filename.StartsWith("deliveryreceipt_"))
                    row["No"] = 21;
                if (filename.StartsWith("deliveryreceipt-item_"))
                    row["No"] = 22;
                if (filename.StartsWith("inventoryposting_"))
                    row["No"] = 23;
                if (filename.StartsWith("inventoryposting-item_"))
                    row["No"] = 24;

                row["Filename"] = filename;
                row["FileFullPath"] = fileFullPath;
                filesList.Rows.Add(row);
            }

            // sort filesList
            DataView dv = filesList.DefaultView;
            dv.Sort = "No";
            DataTable sortedDT = dv.ToTable();
            return sortedDT;
        }

        private void frmImportFiles_Load(object sender, EventArgs e)
        {
            string[] files;
            try
            {
                files = Directory.GetFiles(Application.StartupPath + @"\json\In\", "*.json", SearchOption.TopDirectoryOnly);
            }
            catch
            {
                return;
            }

            DataTable dt = sortFiles(files);

            ListViewItem lvitem = null;
            foreach (DataRow row in dt.Rows)
            {
                filename = row["Filename"].ToString();
                lvitem = new ListViewItem(filename);
                lvitem.SubItems.Add(row["FileFullPath"].ToString());
                listView1.Items.AddRange(new ListViewItem[] { lvitem });
            }

            if (listView1.Items.Count < 1)
                btnImport.Enabled = false;

            int rowCount = listView1.Columns.Count;
            for (int i = 0; i < rowCount; i++)
                listView1.Columns[i].Width = -2;

            lblCount.Text = listView1.Items.Count + " item(s) found.";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void recordImportedFiles(string filename, string filePath)
        {
            fullPath = fullPath.Replace(@"\", @"\\");
            sql = "INSERT INTO export_importfiles(traffic,filename,filepath,createdBy) VALUES('In','" + filename + "','" + fullPath + "','" + vars.username + "')";
            db = new database(); dt = new DataTable();
            db.executeNonQuery(sql, vars.MySqlConnection);
        }

        private void moveFile(ref int i, ref string filename)
        {
            listView1.Items.RemoveAt(i);
            i--;
            try
            {
                File.Move(fullPath, Application.StartupPath + @"\json\Processed\" + filename);
            }
            catch (IOException)
            {
                int dotPosition = filename.LastIndexOf(".");
                filename = filename.Substring(0, dotPosition);

                string pattern = @".*\((\d+)\)\.json";
                Match match = Regex.Match(filename, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    Int16 n = Convert.ToInt16(match.Groups[1].Value);
                    filename = filename + "(" + (n + 1) + ")";
                }
                else //error here. we still have to check if filename of index 2 etc exists.
                    filename = filename + "(2)";

                filename = filename + ".json";
                File.Move(fullPath, Application.StartupPath + @"\json\Processed\" + filename);
            }
        }

        /* we should not allow to continue importing when constrainsts fail. */
        private void btnImport_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "Import the files on the list now?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            string tableName, defaultError = "cannot be empty string or null.";
            int recordsCount = 0, a, cntProcessed;

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                cntProcessed = 0;

                fullPath = listView1.Items[i].SubItems[1].Text;
                filename = listView1.Items[i].Text;
                position = filename.IndexOf("_");
                tableName = filename.Substring(0, position);

                using (StreamReader file = File.OpenText(fullPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    dt = (DataTable)serializer.Deserialize(file, typeof(DataTable));
                }

                // Note
                // 1. Because these data are coming from other branch, we have to assume they've been exported.
                // 2. If db.executeNonQuery() outputs > 0 it means it's a new record so we have to take effect against the inventory.

                if (tableName == "users")
                {
                    string username, password, fName, midName, lName, email, address, gender, lastLogIn, createDate, updateDate, updatedBy = "null", createdBy = "null", picLocation, deactivated, role = "1";
                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        username = dt.Rows[a]["username"].ToString();
                        password = dt.Rows[a]["password"].ToString();

                        fName = transformString4SQL(dt.Rows[a]["fName"].ToString());
                        midName = transformString4SQL(dt.Rows[a]["midName"].ToString());
                        lName = transformString4SQL(dt.Rows[a]["lName"].ToString());
                        email = transformString4SQL(dt.Rows[a]["email"].ToString());
                        address = transformString4SQL(dt.Rows[a]["address"].ToString());
                        gender = transformString4SQL(dt.Rows[a]["gender"].ToString());
                        lastLogIn = transformDate4SQL(dt.Rows[a]["lastLogIn"].ToString());
                        createDate = transformDate4SQL(dt.Rows[a]["createDate"].ToString());
                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        // we just allow users.createdBy to be nullable.
                        createdBy = transformString4SQL(dt.Rows[a]["createdBy"].ToString());

                        picLocation = transformString4SQL(dt.Rows[a]["picLocation"].ToString().Replace(@"\", @"\\"));
                        deactivated = transformString4SQL(dt.Rows[a]["deactivated"].ToString());

                        sql = "INSERT INTO users(username,password,fName,midName,lName,email,address,gender,lastLogIn,createDate,updateDate,updatedBy,createdBy,picLocation,deactivated,role,exported)";
                        sql += " VALUES('" + username + "','" + password + "'," + fName + "," + midName + "," + lName + "," + email + "," + address + "," + gender + "," + lastLogIn + "," + createDate + "," + updateDate + "," + updatedBy + ",'" + createdBy + "'," + picLocation + "," + deactivated + "," + role + ",1)";
                        sql += " ON DUPLICATE KEY UPDATE user_id=LAST_INSERT_ID(user_id), password=VALUES(password),fName=VALUES(fName),midName=VALUES(midName),lName=VALUES(lName),email=VALUES(email),address=VALUES(address),gender=VALUES(gender),lastLogIn=VALUES(lastLogIn),updateDate=VALUES(updateDate),updatedBy=VALUES(updatedBy)";
                        sql += ",picLocation=VALUES(picLocation),deactivated=VALUES(deactivated),role=VALUES(role)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import warehouse
                cntProcessed = 0;

                if (tableName == "warehouse")
                {
                    string code, name, branchType, ftp_url, ftp_username, ftp_password, deactivated, createDate, updateDate, createdBy, updatedBy, trans;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        code = dt.Rows[a]["code"].ToString();
                        name = dt.Rows[a]["name"].ToString();
                        branchType = dt.Rows[a]["branchType"].ToString();

                        ftp_url = null;
                        if (dt.Rows[a]["ftp_url"] != DBNull.Value)
                        ftp_url = dt.Rows[a]["ftp_url"].ToString();

                        ftp_username = null;
                        if (dt.Rows[a]["ftp_username"] != DBNull.Value)
                            ftp_username = dt.Rows[a]["ftp_username"].ToString();

                        ftp_password = null;
                        if (dt.Rows[a]["ftp_password"] != DBNull.Value)
                            ftp_password = dt.Rows[a]["ftp_password"].ToString();

                        deactivated = dt.Rows[a]["deactivated"].ToString();

                        DateTime dateTime;
                        createDate = null;
                        if (dt.Rows[a]["createDate"] != DBNull.Value)
                        {
                            dateTime = DateTime.Parse(dt.Rows[a]["createDate"].ToString());
                            createDate = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
                        }

                        updateDate = null;
                        if (dt.Rows[a]["updateDate"] != DBNull.Value)
                        {
                            dateTime = DateTime.Parse(dt.Rows[a]["updateDate"].ToString());
                            updateDate = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
                        }

                        if (dt.Rows[a]["createdBy"] != DBNull.Value)
                            createdBy = dt.Rows[a]["createdBy"].ToString();
                        else
                            createdBy = "admin";

                        if (dt.Rows[a]["updatedBy"] != DBNull.Value)
                            updatedBy = dt.Rows[a]["updatedBy"].ToString();
                        else
                            updatedBy = null;

                        if (dt.Rows[a]["trans"] != DBNull.Value)
                            trans = dt.Rows[a]["trans"].ToString();
                        else
                            trans = null;

                        sql = "INSERT INTO warehouse(`code`,`name`,branchType,ftp_url,ftp_username,ftp_password,deactivated,createDate,createdBy,updateDate,updatedBy,trans)";
                        sql += " VALUES(@code, @name, @branchType, @ftp_url, @ftp_username, @ftp_password, @deactivated, @createDate, @createdBy, @updateDate, @updatedBy, @trans)";
                        sql += " ON DUPLICATE KEY UPDATE id=LAST_INSERT_ID(id),name=VALUES(name),branchType=VALUES(branchType),ftp_url=VALUES(ftp_url),ftp_username=VALUES(ftp_username),ftp_password=VALUES(ftp_password),deactivated=VALUES(deactivated),updateDate=VALUES(updateDate),updatedBy=VALUES(updatedBy),trans=VALUES(trans)";

                        cmd = new MySqlCommand(sql, vars.MySqlConnection);
                        cmd.Prepare();
                        cmd.Parameters.AddWithValue("@code", code);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@branchType", branchType);
                        cmd.Parameters.AddWithValue("@ftp_url", ftp_url);
                        cmd.Parameters.AddWithValue("@ftp_username", ftp_username);
                        cmd.Parameters.AddWithValue("@ftp_password", ftp_password);
                        cmd.Parameters.AddWithValue("@deactivated", deactivated);
                        cmd.Parameters.AddWithValue("@createDate", createDate);
                        cmd.Parameters.AddWithValue("@createdBy", createdBy);
                        cmd.Parameters.AddWithValue("@updateDate", updateDate);
                        cmd.Parameters.AddWithValue("@updatedBy", updatedBy);
                        cmd.Parameters.AddWithValue("@trans", trans);

                        if (cmd.ExecuteNonQuery() > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import business partner
                cntProcessed = 0;
                if (tableName == "businesspartner")
                {
                    string BPType,code,BPName,address,tel1,tel2,fax,email,website,contactP,deactivated,remarks,createDate,createdBy,updateDate,updatedBy,trans,transmitted;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        if (dt.Rows[a]["BPType"] == DBNull.Value || dt.Rows[a]["BPType"].ToString() == "")
                        {
                            MessageBox.Show(this, "businesspartner.BPType " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            BPType = dt.Rows[a]["BPType"].ToString();

                        if (dt.Rows[a]["code"] == DBNull.Value || dt.Rows[a]["code"].ToString() == "")
                        {
                            MessageBox.Show(this, "businesspartner.code " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            code = dt.Rows[a]["code"].ToString();

                        BPName = transformString4SQL(dt.Rows[a]["BPName"].ToString());
                        address = transformString4SQL(dt.Rows[a]["address"].ToString());
                        tel1 = transformString4SQL(dt.Rows[a]["tel1"].ToString());
                        tel2 = transformString4SQL(dt.Rows[a]["tel2"].ToString());
                        fax = transformString4SQL(dt.Rows[a]["fax"].ToString());
                        email = transformString4SQL(dt.Rows[a]["email"].ToString());
                        website = transformString4SQL(dt.Rows[a]["website"].ToString());
                        contactP = transformString4SQL(dt.Rows[a]["contactP"].ToString());
                        deactivated = transformString4SQL(dt.Rows[a]["deactivated"].ToString());
                        remarks = transformString4SQL(dt.Rows[a]["remarks"].ToString());

                        deactivated = transformString4SQL(dt.Rows[a]["deactivated"].ToString());
                        createDate = transformDate4SQL(dt.Rows[a]["createDate"].ToString());

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "itemmasterdata.createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();

                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());
                        trans = transformString4SQL(dt.Rows[a]["trans"].ToString());
                        transmitted = transformString4SQL(dt.Rows[a]["transmitted"].ToString());

                        sql = "INSERT INTO businesspartner(BPType,`code`,BPName,address,tel1,tel2,fax,email,website,contactP,deactivated,remarks,createDate,createdBy,updateDate,updatedBy,trans,exported,transmitted)";
                        sql += " VALUES(" + BPType + ",'" + code + "'," + BPName + "," + address + "," + tel1 + "," + tel2 + "," + fax + "," + email + "," + website + "," + contactP + "," + deactivated + "," + remarks + "," + createDate + ",'" + createdBy + "'," + updateDate + "," + updatedBy + "," + trans + ", 1, 1)"; // because this is incoming, exported = 1. This is also assumed to be transmitted.
                        sql += " ON DUPLICATE KEY UPDATE `code`=VALUES(`code`),BPType=VALUES(BPType),address=VALUES(address),tel1=VALUES(tel1),tel2=VALUES(tel2),fax=VALUES(fax),email=VALUES(email),website=VALUES(website),contactP=VALUES(contactP),deactivated=VALUES(deactivated),remarks=VALUES(remarks),createDate=VALUES(createDate),createdBy=VALUES(createdBy),updateDate=VALUES(updateDate),updatedBy=VALUES(updatedBy),trans=VALUES(trans),exported=VALUES(exported),transmitted=VALUES(transmitted)";

                        using (StreamWriter file = File.CreateText(@"c:\temp\sql.txt"))
                        {
                            file.Write(sql);
                        }


                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import Item master data
                cntProcessed = 0;
                if (tableName == "itemmasterdata")
                {
                    string itemCode,description,shortName,vatable,vendor,qtyPrPrchsUoM,qtyPrSaleUoM,prchsUoM,saleUoM,varWeightItm,remarks,minStock,maxStock,deactivated,createDate,createdBy,updateDate,updatedBy,trans;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        itemCode = dt.Rows[a]["itemCode"].ToString();

                        description = transformString4SQL(dt.Rows[a]["description"].ToString());
                        shortName = transformString4SQL(dt.Rows[a]["shortName"].ToString());

                        if (dt.Rows[a]["vatable"] == DBNull.Value || dt.Rows[a]["vatable"].ToString() == "")
                        {
                            MessageBox.Show(this, "itemmasterdata.vatable " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            vatable = dt.Rows[a]["vatable"].ToString();

                        vendor = transformString4SQL(dt.Rows[a]["vendor"].ToString());

                        qtyPrPrchsUoM = dt.Rows[a]["qtyPrPrchsUoM"].ToString();
                        qtyPrSaleUoM = dt.Rows[a]["qtyPrSaleUoM"].ToString();
                        prchsUoM = dt.Rows[a]["prchsUoM"].ToString();
                        saleUoM = dt.Rows[a]["saleUoM"].ToString();
                        varWeightItm = dt.Rows[a]["varWeightItm"].ToString();
                        remarks = transformString4SQL(dt.Rows[a]["remarks"].ToString());
                        minStock = dt.Rows[a]["minStock"].ToString();
                        maxStock = dt.Rows[a]["maxStock"].ToString();
                        deactivated = transformString4SQL(dt.Rows[a]["deactivated"].ToString());
                        createDate = transformDate4SQL(dt.Rows[a]["createDate"].ToString());

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "itemmasterdata.createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();

                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());
                        trans = transformString4SQL(dt.Rows[a]["trans"].ToString());

                        sql = "INSERT INTO itemmasterdata(itemCode,description,shortName,vatable,vendor,qtyPrPrchsUoM,qtyPrSaleUoM,prchsUoM,saleUoM,varWeightItm,remarks,minStock,maxStock,deactivated,createDate,createdBy,updateDate,updatedBy,trans,exported)";
                        sql += " VALUES('" + itemCode + "'," + description + "," + shortName + ",'" + vatable + "'," + vendor + "," + qtyPrPrchsUoM + "," + qtyPrSaleUoM + ",'" + prchsUoM + "','" + saleUoM + "','" + varWeightItm + "'," + remarks + "," + minStock + "," + maxStock + "," + deactivated + "," + createDate + ",'" + createdBy + "'," + updateDate + "," + updatedBy + "," + trans + ",1)";
                        sql += " ON DUPLICATE KEY UPDATE id=LAST_INSERT_ID(id),description=VALUES(description),shortName=VALUES(shortName),vatable=VALUES(vatable),vendor=VALUES(vendor),varWeightItm=VALUES(varWeightItm),remarks=VALUES(remarks),minStock=VALUES(minStock),maxStock=VALUES(maxStock),deactivated=VALUES(deactivated),updateDate=VALUES(updateDate),updatedBy=VALUES(updatedBy),trans=VALUES(trans)";
                        
                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import Price lists
                cntProcessed = 0;
                if (tableName == "pricelist")
                {
                    string itemCode, priceListCode, netPrice, createDate, createdBy;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        if (dt.Rows[a]["itemCode"] == DBNull.Value || dt.Rows[a]["itemCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "pricelist.itemCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            itemCode = dt.Rows[a]["itemCode"].ToString();

                        if (dt.Rows[a]["priceListCode"] == DBNull.Value || dt.Rows[a]["priceListCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "pricelist.priceListCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            priceListCode = dt.Rows[a]["priceListCode"].ToString();

                        if (dt.Rows[a]["netPrice"] == DBNull.Value || dt.Rows[a]["netPrice"].ToString() == "")
                        {
                            MessageBox.Show(this, "pricelist.netPrice " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netPrice = dt.Rows[a]["netPrice"].ToString();

                        createDate = transformDate4SQL(dt.Rows[a]["createDate"].ToString());

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "pricelist.createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();                        

                        sql = "INSERT INTO pricelist(itemCode,priceListCode,netPrice,createDate,createdBy,exported)";
                        sql += " VALUES('" + itemCode + "'," + priceListCode + "," + netPrice + "," + createDate + ",'" + createdBy + "',1)";
                        sql += " ON DUPLICATE KEY UPDATE netPrice=VALUES(netPrice)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import price list history
                cntProcessed = 0;
                if (tableName == "pricelisthistory")
                {
                    string itemCode, priceListCode, netPrice,cp_createDate,cp_createdBy, createDate, createdBy;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        if (dt.Rows[a]["itemCode"] == DBNull.Value || dt.Rows[a]["itemCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "pricelisthistory.itemCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            itemCode = dt.Rows[a]["itemCode"].ToString();

                        if (dt.Rows[a]["priceListCode"] == DBNull.Value || dt.Rows[a]["priceListCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "pricelisthistory.priceListCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            priceListCode = dt.Rows[a]["priceListCode"].ToString();

                        if (dt.Rows[a]["netPrice"] == DBNull.Value || dt.Rows[a]["netPrice"].ToString() == "")
                        {
                            MessageBox.Show(this, "pricelisthistory.netPrice " + defaultError, "Error", MessageBoxButtons.OK , MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netPrice = dt.Rows[a]["netPrice"].ToString();

                        cp_createDate = transformDate4SQL(dt.Rows[a]["cp_createDate"].ToString());

                        if (dt.Rows[a]["cp_createdBy"] == DBNull.Value || dt.Rows[a]["cp_createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "pricelisthistory.cp_createdBy " + defaultError, "Error",  MessageBoxButtons.OK , MessageBoxIcon.Error);
                            return;
                        }
                        else
                            cp_createdBy = dt.Rows[a]["cp_createdBy"].ToString();

                        createDate = transformDate4SQL(dt.Rows[a]["createDate"].ToString());

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "pricelisthistory.createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();

                        sql = "INSERT INTO pricelisthistory(itemCode,priceListCode,netPrice,cp_createDate,cp_createdBy,createDate,createdBy,exported)";
                        sql += " VALUES('" + itemCode + "'," + priceListCode + "," + netPrice + "," + cp_createDate + ",'" + cp_createdBy + "'," + createDate + ",'" + createdBy + "',1)";
                        sql += " ON DUPLICATE KEY UPDATE netPrice=VALUES(netPrice)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }
                // let's import barcode
                cntProcessed = 0;
                if (tableName == "barcode")
                {
                    string itemCode, barcode, createDate, createdBy;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        if (dt.Rows[a]["itemCode"] == DBNull.Value || dt.Rows[a]["itemCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "barcode.itemCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            itemCode = dt.Rows[a]["itemCode"].ToString();

                        if (dt.Rows[a]["barcode"] == DBNull.Value || dt.Rows[a]["barcode"].ToString() == "")
                        {
                            MessageBox.Show(this, "barcode.barcode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            barcode = dt.Rows[a]["barcode"].ToString();

                        createDate = transformDate4SQL(dt.Rows[a]["createDate"].ToString());

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "barcode.createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();

                        sql = "INSERT INTO barcode(itemCode,barcode,createDate,createdBy,exported)";
                        sql += " VALUES('" + itemCode + "','" + barcode + "'," + createDate + ",'" + createdBy + "',1)";
                        sql += " ON DUPLICATE KEY UPDATE barcode=VALUES(barcode)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }
                
                // let's import barcode history
                cntProcessed = 0;
                if (tableName == "barcodehistory")
                {
                    string itemCode, barcode, cp_createDate, cp_createdBy, createDate, createdBy;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        if (dt.Rows[a]["itemCode"] == DBNull.Value || dt.Rows[a]["itemCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "barcodehistory.itemCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            itemCode = dt.Rows[a]["itemCode"].ToString();

                        if (dt.Rows[a]["barcode"] == DBNull.Value || dt.Rows[a]["barcode"].ToString() == "")
                        {
                            MessageBox.Show(this, "barcodehistory.barcode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            barcode = dt.Rows[a]["barcode"].ToString();

                        cp_createDate = transformDate4SQL(dt.Rows[a]["cp_createDate"].ToString());

                        if (dt.Rows[a]["cp_createdBy"] == DBNull.Value || dt.Rows[a]["cp_createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "barcodehistory.cp_createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            cp_createdBy = dt.Rows[a]["cp_createdBy"].ToString();

                        createDate = transformDate4SQL(dt.Rows[a]["createDate"].ToString());

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "barcodehistory.createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();

                        sql = "INSERT INTO barcodehistory(itemCode,barcode,cp_createDate,cp_createdBy,createDate,createdBy,exported)";
                        sql += " VALUES('" + itemCode + "','" + barcode + "'," + cp_createDate + ",'" + cp_createdBy + "'," + createDate + ",'" + createdBy + "',1)";
                        sql += " ON DUPLICATE KEY UPDATE barcode=VALUES(barcode)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import PO
                cntProcessed = 0;
                if (tableName == "purchaseorder")
                {
                    string docId,vendorCode,vendorName,postingDate,warehouse,remarks1,remarks2,createDate,createdBy,updateDate,updatedBy;
                    decimal totalPrcntDscnt, totalAmtDscnt, netTotal, grossTotal;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["vendorCode"] == DBNull.Value || dt.Rows[a]["vendorCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder.vendorCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            vendorCode = dt.Rows[a]["vendorCode"].ToString();

                        vendorName = transformString4SQL(dt.Rows[a]["vendorName"].ToString());

                        if (dt.Rows[a]["postingDate"] == DBNull.Value || dt.Rows[a]["postingDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder.postingDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            postingDate = transformDate4SQLNotNull(dt.Rows[a]["postingDate"].ToString());

                        if (dt.Rows[a]["warehouse"] == DBNull.Value || dt.Rows[a]["warehouse"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder.warehouse " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            warehouse = dt.Rows[a]["warehouse"].ToString();

                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());
                        remarks2 = transformString4SQL(dt.Rows[a]["remarks2"].ToString());
                        totalPrcntDscnt = Convert.ToDecimal(dt.Rows[a]["totalPrcntDscnt"] ?? 0);
                        totalAmtDscnt = Convert.ToDecimal(dt.Rows[a]["totalAmtDscnt"] ?? 0);
                        netTotal = Convert.ToDecimal(dt.Rows[a]["netTotal"] ?? 0);
                        grossTotal = Convert.ToDecimal(dt.Rows[a]["grossTotal"] ?? 0);

                        if (dt.Rows[a]["createDate"] == DBNull.Value || dt.Rows[a]["createDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder.createDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createDate = transformDate4SQLNotNull(dt.Rows[a]["createDate"].ToString());

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder.createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();

                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO purchaseorder(docId,vendorCode,vendorName,postingDate,warehouse,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy,exported)";
                        sql += " VALUES('" + docId + "','" + vendorCode + "'," + vendorName + ",'" + postingDate + "','" + warehouse + "'," + remarks1 + "," + remarks2 + "," + totalPrcntDscnt + "," + totalAmtDscnt + "," + netTotal + "," + grossTotal + ",'" + createDate + "','" + createdBy + "'," + updateDate + "," + updatedBy + ",1)";
                        sql += " ON DUPLICATE KEY UPDATE id=VALUES(id),remarks1=VALUES(remarks1),remarks2=VALUES(remarks2),updateDate=VALUES(updateDate),updatedBy=VALUES(updatedBy)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let import Purchase Order rows
                cntProcessed = 0;
                if (tableName == "purchaseorder-item")
                {
                    string docId,indx,itemCode,description,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        indx = dt.Rows[a]["indx"].ToString();
                        itemCode = dt.Rows[a]["itemCode"].ToString();
                        description = dt.Rows[a]["description"].ToString();
                        vatable = dt.Rows[a]["vatable"].ToString();
                        realBsNetPrchsPrc = dt.Rows[a]["realBsNetPrchsPrc"].ToString();
                        realBsGrossPrchsPrc = dt.Rows[a]["realBsGrossPrchsPrc"].ToString();
                        realNetPrchsPrc = dt.Rows[a]["realNetPrchsPrc"].ToString();
                        realGrossPrchsPrc = dt.Rows[a]["realGrossPrchsPrc"].ToString();
                        qty = dt.Rows[a]["qty"].ToString();
                        baseUoM = dt.Rows[a]["baseUoM"].ToString();
                        qtyPrPrchsUoM = dt.Rows[a]["qtyPrPrchsUoM"].ToString();
                        prcntDscnt = dt.Rows[a]["prcntDscnt"].ToString();
                        amtDscnt = dt.Rows[a]["amtDscnt"].ToString();
                        netPrchsPrc = dt.Rows[a]["netPrchsPrc"].ToString();
                        grossPrchsPrc = dt.Rows[a]["grossPrchsPrc"].ToString();
                        rowNetTotal = dt.Rows[a]["rowNetTotal"].ToString();
                        rowGrossTotal = dt.Rows[a]["rowGrossTotal"].ToString();

                        sql = "INSERT IGNORE INTO purchaseorder_item(docId,indx,itemCode,description,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal,exported)";
                        sql += " VALUES('" + docId + "'," + indx + ",'" + itemCode + "','" + description + "','" + vatable + "'," + realBsNetPrchsPrc + "," + realBsGrossPrchsPrc + "," + realNetPrchsPrc + "," + realGrossPrchsPrc + "," + qty + ",'" + baseUoM + "'," + qtyPrPrchsUoM + "," + prcntDscnt + "," + amtDscnt + "," + netPrchsPrc + "," + grossPrchsPrc + "," + rowNetTotal + "," + rowGrossTotal + ",1)";

                        db = new database();
                        db.executeNonQuery(sql, vars.MySqlConnection);
                        cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import GRPO.
                cntProcessed = 0;
                if (tableName == "grpo")
                {
                    string docId,vendorCode,vendorName,warehouse,postingDate,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        vendorCode = dt.Rows[a]["vendorCode"].ToString();
                        vendorName = transformString4SQL(dt.Rows[a]["vendorName"].ToString());
                        warehouse = dt.Rows[a]["warehouse"].ToString();
                        postingDate = transformDate4SQL(dt.Rows[a]["postingDate"].ToString());
                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());
                        remarks2 = transformString4SQL(dt.Rows[a]["remarks2"].ToString());
                        totalPrcntDscnt = dt.Rows[a]["totalPrcntDscnt"].ToString();
                        totalAmtDscnt = dt.Rows[a]["totalAmtDscnt"].ToString();
                        netTotal = dt.Rows[a]["netTotal"].ToString();
                        grossTotal = dt.Rows[a]["grossTotal"].ToString();
                        createDate = transformDate4SQL(dt.Rows[a]["createDate"].ToString());
                        createdBy = dt.Rows[a]["createdBy"].ToString();
                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO grpo(docId,vendorCode,vendorName,warehouse,postingDate,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy,exported)";
                        sql += " VALUES('" + docId + "','" + vendorCode + "'," + vendorName + ",'" + warehouse + "'," + postingDate + "," + remarks1 + "," + remarks2 + "," + totalPrcntDscnt + "," + totalAmtDscnt + "," + netTotal + "," + grossTotal + "," + createDate + "," + createdBy + "," + updateDate + "," + updatedBy + ",1)";
                        sql += " ON DUPLICATE KEY UPDATE id=VALUES(id),remarks1=VALUES(remarks1),remarks2=VALUES(remarks2),updateDate=VALUES(updateDate),updatedBy=VALUES(updatedBy)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                decimal baseQty;

                // let's import grpo items
                // This should affect inventory so we need to have to add/deduct item_warehouse table.
                cntProcessed = 0;
                if (tableName == "grpo-item")
                {
                    string docId,indx,itemCode,description,warehouse,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        indx = dt.Rows[a]["indx"].ToString();
                        itemCode = dt.Rows[a]["itemCode"].ToString();
                        description = transformString4SQL(dt.Rows[a]["description"].ToString());
                        warehouse = dt.Rows[a]["warehouse"].ToString();
                        vatable = dt.Rows[a]["vatable"].ToString();
                        realBsNetPrchsPrc = dt.Rows[a]["realBsNetPrchsPrc"].ToString();
                        realBsGrossPrchsPrc = dt.Rows[a]["realBsGrossPrchsPrc"].ToString();
                        realNetPrchsPrc = dt.Rows[a]["realNetPrchsPrc"].ToString();
                        realGrossPrchsPrc = dt.Rows[a]["realGrossPrchsPrc"].ToString();
                        qty = dt.Rows[a]["qty"].ToString();
                        baseUoM = dt.Rows[a]["baseUoM"].ToString();
                        qtyPrPrchsUoM = dt.Rows[a]["qtyPrPrchsUoM"].ToString();
                        prcntDscnt = dt.Rows[a]["prcntDscnt"].ToString();
                        amtDscnt = dt.Rows[a]["amtDscnt"].ToString();
                        netPrchsPrc = dt.Rows[a]["netPrchsPrc"].ToString();
                        grossPrchsPrc = dt.Rows[a]["grossPrchsPrc"].ToString();
                        rowNetTotal = dt.Rows[a]["rowNetTotal"].ToString();
                        rowGrossTotal = dt.Rows[a]["rowGrossTotal"].ToString();

                        sql = "INSERT IGNORE INTO grpo_item(docId,indx,itemCode,description,warehouse,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal,exported)";
                        sql += " VALUES('" + docId + "'," + indx + ",'" + itemCode + "'," + description + ",'" + warehouse + "','" + vatable + "'," + realBsNetPrchsPrc + "," + realBsGrossPrchsPrc + "," + realNetPrchsPrc + "," + realGrossPrchsPrc + "," + qty + ",'" + baseUoM + "'," + qtyPrPrchsUoM + "," + prcntDscnt + "," + amtDscnt + "," + netPrchsPrc + "," + grossPrchsPrc + "," + rowNetTotal + "," + rowGrossTotal + ",1)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                        {
                            baseQty = ((baseUoM == "N") ? Decimal.Parse(qtyPrPrchsUoM) * Decimal.Parse(qty) : Decimal.Parse(qty));
                            sql = "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES('" + itemCode + "','" + warehouse + "'," + baseQty + ") ON DUPLICATE KEY UPDATE inStock=ifnull(inStock, 0)+" + baseQty;
                            db.executeNonQuery(sql, vars.MySqlConnection);
                            cntProcessed++;
                        }
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import Delivery Receipt.
                cntProcessed = 0;
                if (tableName == "deliveryreceipt")
                {
                    string docId, warehouse, postingDate, remarks1, remarks2, totalPrcntDscnt, totalAmtDscnt, netTotal, grossTotal, createDate, createdBy, updateDate, updatedBy;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        warehouse = dt.Rows[a]["warehouse"].ToString();
                        postingDate = transformDate4SQL(dt.Rows[a]["postingDate"].ToString());
                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());
                        remarks2 = transformString4SQL(dt.Rows[a]["remarks2"].ToString());
                        totalPrcntDscnt = dt.Rows[a]["totalPrcntDscnt"].ToString();
                        totalAmtDscnt = dt.Rows[a]["totalAmtDscnt"].ToString();
                        netTotal = dt.Rows[a]["netTotal"].ToString();
                        grossTotal = dt.Rows[a]["grossTotal"].ToString();
                        createDate = transformDate4SQL(dt.Rows[a]["createDate"].ToString());
                        createdBy = dt.Rows[a]["createdBy"].ToString();
                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO deliveryreceipt(docId,warehouse,postingDate,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy,exported)";
                        sql += " VALUES('" + docId + "','" + warehouse + "'," + postingDate + "," + remarks1 + "," + remarks2 + "," + totalPrcntDscnt + "," + totalAmtDscnt + "," + netTotal + "," + grossTotal + "," + createDate + "," + createdBy + "," + updateDate + "," + updatedBy + ",1)";
                        sql += " ON DUPLICATE KEY UPDATE id=VALUES(id),remarks1=VALUES(remarks1),remarks2=VALUES(remarks2),updateDate=VALUES(updateDate),updatedBy=VALUES(updatedBy)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import deliveryreceipt items
                // This should affect inventory so we need to have to add/deduct item_warehouse table.
                cntProcessed = 0;
                if (tableName == "deliveryreceipt-item")
                {
                    string docId, indx, vendorCode, vendorName, itemCode, description, warehouse, vatable, realBsNetPrchsPrc, realBsGrossPrchsPrc, realNetPrchsPrc, realGrossPrchsPrc, qty, baseUoM, qtyPrPrchsUoM, prcntDscnt, amtDscnt, netPrchsPrc, grossPrchsPrc, rowNetTotal, rowGrossTotal;
                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        indx = dt.Rows[a]["indx"].ToString();
                        vendorCode = dt.Rows[a]["vendorCode"].ToString();
                        vendorName = transformString4SQL(dt.Rows[a]["vendorName"].ToString());
                        itemCode = dt.Rows[a]["itemCode"].ToString();
                        description = transformString4SQL(dt.Rows[a]["description"].ToString());
                        warehouse = dt.Rows[a]["warehouse"].ToString();
                        vatable = dt.Rows[a]["vatable"].ToString();
                        realBsNetPrchsPrc = dt.Rows[a]["realBsNetPrchsPrc"].ToString();
                        realBsGrossPrchsPrc = dt.Rows[a]["realBsGrossPrchsPrc"].ToString();
                        realNetPrchsPrc = dt.Rows[a]["realNetPrchsPrc"].ToString();
                        realGrossPrchsPrc = dt.Rows[a]["realGrossPrchsPrc"].ToString();
                        qty = dt.Rows[a]["qty"].ToString();
                        baseUoM = dt.Rows[a]["baseUoM"].ToString();
                        qtyPrPrchsUoM = dt.Rows[a]["qtyPrPrchsUoM"].ToString();
                        prcntDscnt = dt.Rows[a]["prcntDscnt"].ToString();
                        amtDscnt = dt.Rows[a]["amtDscnt"].ToString();
                        netPrchsPrc = dt.Rows[a]["netPrchsPrc"].ToString();
                        grossPrchsPrc = dt.Rows[a]["grossPrchsPrc"].ToString();
                        rowNetTotal = dt.Rows[a]["rowNetTotal"].ToString();
                        rowGrossTotal = dt.Rows[a]["rowGrossTotal"].ToString();

                        sql = "INSERT IGNORE INTO deliveryreceipt_item(docId,indx,vendorCode,vendorName,itemCode,description,warehouse,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal,exported)";
                        sql += " VALUES('" + docId + "'," + indx + ", '" + vendorCode + "'," + vendorName + ",'" + itemCode + "'," + description + ",'" + warehouse + "','" + vatable + "'," + realBsNetPrchsPrc + "," + realBsGrossPrchsPrc + "," + realNetPrchsPrc + "," + realGrossPrchsPrc + "," + qty + ",'" + baseUoM + "'," + qtyPrPrchsUoM + "," + prcntDscnt + "," + amtDscnt + "," + netPrchsPrc + "," + grossPrchsPrc + "," + rowNetTotal + "," + rowGrossTotal + ",1)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                        {
                            baseQty = ((baseUoM == "N") ? Decimal.Parse(qtyPrPrchsUoM) * Decimal.Parse(qty) : Decimal.Parse(qty));
                            sql = "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES('" + itemCode + "','" + warehouse + "'," + baseQty + ") ON DUPLICATE KEY UPDATE inStock=ifnull(inStock, 0)+" + baseQty;
                            db.executeNonQuery(sql, vars.MySqlConnection);
                            cntProcessed++;
                        }
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import Inventory Posting.
                cntProcessed = 0;
                if (tableName == "inventoryposting")
                {
                    string docId, postingDate, countDateTime, warehouse, remarks1, createDate, createdBy, updateDate, updatedBy;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        postingDate = transformDate4SQL(dt.Rows[a]["postingDate"].ToString());
                        countDateTime = transformDate4SQL(dt.Rows[a]["countDateTime"].ToString());
                        warehouse = dt.Rows[a]["warehouse"].ToString();                        
                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());
                        createDate = transformDate4SQL(dt.Rows[a]["createDate"].ToString());
                        createdBy = dt.Rows[a]["createdBy"].ToString();
                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO inventoryposting(docId,postingDate,countDateTime,warehouse,remarks1,createDate,createdBy,updateDate,updatedBy,exported)";
                        sql += " VALUES('" + docId + "'," + postingDate + "," + countDateTime + ",'" + warehouse + "'," + remarks1 + "," + createDate + "," + createdBy + "," + updateDate + "," + updatedBy + ",1)";
                        sql += " ON DUPLICATE KEY UPDATE id=VALUES(id),remarks1=VALUES(remarks1),remarks2=VALUES(remarks2),updateDate=VALUES(updateDate),updatedBy=VALUES(updatedBy)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import Inventory Posting items
                // This should affect inventory so we need to have to add/deduct item_warehouse table.
                cntProcessed = 0;
                if (tableName == "inventoryposting-item")
                {
                    string docId, indx, itemCode, description, vatable, currentQty, countedQty, varianceQty, prchsPrc, retailPrc;
                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        indx = dt.Rows[a]["indx"].ToString();
                        itemCode = dt.Rows[a]["itemCode"].ToString();
                        description = transformString4SQL(dt.Rows[a]["description"].ToString());
                        vatable = dt.Rows[a]["vatable"].ToString();
                        currentQty = dt.Rows[a]["currentQty"].ToString();
                        countedQty = dt.Rows[a]["countedQty"].ToString();
                        varianceQty = dt.Rows[a]["varianceQty"].ToString();
                        prchsPrc = dt.Rows[a]["prchsPrc"].ToString();
                        retailPrc = dt.Rows[a]["retailPrc"].ToString();

                        sql = "INSERT IGNORE INTO inventoryposting_item(docId,indx,itemCode,description,vatable,currentQty,countedQty,varianceQty,prchsPrc,retailPrc,exported)";
                        sql += " VALUES('" + docId + "'," + indx + ",'" + itemCode + "'," + description + ",'" + vatable + "'," + currentQty + "," + countedQty + "," + varianceQty + "," + prchsPrc + "," + retailPrc + ",1)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                        {
                            sql = "SET @warehouse=(SELECT warehouse FROM inventoryposting WHERE docId='" + docId + "');";
                            sql += "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES('" + itemCode + "',@warehouse," + varianceQty + ") ON DUPLICATE KEY UPDATE inStock=inStock";
                            // inventoryposting_item.varianceQty can be negative
                            if (Int32.Parse(varianceQty) >= 0)
                                sql += sql + "+" + varianceQty;
                            else
                                sql += sql + varianceQty;
                            
                            db.executeNonQuery(sql, vars.MySqlConnection);
                            cntProcessed++;
                        }
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import Inventory Transfer
                cntProcessed = 0;
                if (tableName == "inventorytransfer")
                {
                    string docId,frmWHouse,toWHouse,postingDate,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        frmWHouse = dt.Rows[a]["frmWHouse"].ToString();
                        toWHouse = dt.Rows[a]["toWHouse"].ToString();
                        postingDate = transformDate4SQL(dt.Rows[a]["postingDate"].ToString());
                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());
                        remarks2 = transformString4SQL(dt.Rows[a]["remarks2"].ToString());
                        totalPrcntDscnt = dt.Rows[a]["totalPrcntDscnt"].ToString();
                        totalAmtDscnt = dt.Rows[a]["totalAmtDscnt"].ToString();
                        netTotal = dt.Rows[a]["netTotal"].ToString();
                        grossTotal = dt.Rows[a]["grossTotal"].ToString();
                        createDate = transformDate4SQL(dt.Rows[a]["createDate"].ToString());
                        createdBy = dt.Rows[a]["createdBy"].ToString();
                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO inventorytransfer(docId,frmWHouse,toWHouse,postingDate,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy,exported)";
                        sql += " VALUES('" + docId + "','" + frmWHouse + "','" + toWHouse + "'," + postingDate + "," + remarks1 + "," + remarks2 + "," + totalPrcntDscnt + "," + totalAmtDscnt + "," + netTotal + "," + grossTotal + "," + createDate + "," + createdBy + "," + updateDate + "," + updatedBy + ",1)";
                        sql += " ON DUPLICATE KEY UPDATE id=VALUES(id),remarks1=VALUES(remarks1),remarks2=VALUES(remarks2),updateDate=VALUES(updateDate),updatedBy=VALUES(updatedBy)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // import Inventory transfer items
                cntProcessed = 0;
                if (tableName == "inventorytransfer-item")
                {
                    string docId,indx,itemCode,description,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        indx = dt.Rows[a]["indx"].ToString();
                        itemCode = dt.Rows[a]["itemCode"].ToString();
                        description = transformString4SQL(dt.Rows[a]["description"].ToString());
                        vatable = dt.Rows[a]["vatable"].ToString();
                        realBsNetPrchsPrc = dt.Rows[a]["realBsNetPrchsPrc"].ToString();
                        realBsGrossPrchsPrc = dt.Rows[a]["realBsGrossPrchsPrc"].ToString();
                        realNetPrchsPrc = dt.Rows[a]["realNetPrchsPrc"].ToString();
                        realGrossPrchsPrc = dt.Rows[a]["realGrossPrchsPrc"].ToString();
                        qty = dt.Rows[a]["qty"].ToString();
                        baseUoM = dt.Rows[a]["baseUoM"].ToString();
                        qtyPrPrchsUoM = dt.Rows[a]["qtyPrPrchsUoM"].ToString();
                        prcntDscnt = dt.Rows[a]["prcntDscnt"].ToString();
                        amtDscnt = dt.Rows[a]["amtDscnt"].ToString();
                        netPrchsPrc = dt.Rows[a]["netPrchsPrc"].ToString();
                        grossPrchsPrc = dt.Rows[a]["grossPrchsPrc"].ToString();
                        rowNetTotal = dt.Rows[a]["rowNetTotal"].ToString();
                        rowGrossTotal = dt.Rows[a]["rowGrossTotal"].ToString();

                        sql = "INSERT IGNORE INTO inventorytransfer_item(docId,indx,itemCode,description,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal,exported)";
                        sql += " VALUES('" + docId + "'," + indx + ",'" + itemCode + "'," + description + ",'" + vatable + "'," + realBsNetPrchsPrc + "," + realBsGrossPrchsPrc + "," + realNetPrchsPrc + "," + realGrossPrchsPrc + "," + qty + ",'" + baseUoM + "'," + qtyPrPrchsUoM + "," + prcntDscnt + "," + amtDscnt + "," + netPrchsPrc + "," + grossPrchsPrc + "," + rowNetTotal + "," + rowGrossTotal + ",1)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                        {
                            baseQty = ((baseUoM == "N") ? Decimal.Parse(qtyPrPrchsUoM) * Decimal.Parse(qty) : Decimal.Parse(qty));
                            sql = "SET @frmWHouse=(SELECT frmWHouse FROM inventorytransfer WHERE docId='" + docId + "');";
                            sql += "SET @toWHouse=(SELECT frmWHouse FROM inventorytransfer WHERE docId='" + docId + "');";
                            sql += "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES('" + itemCode + "',@toWHouse," + baseQty + ") ON DUPLICATE KEY UPDATE inStock=ifnull(inStock, 0)+" + baseQty + ";";
                            sql += "UPDATE item_warehouse SET inStock=ifnull(inStock, 0)-" + baseQty + " WHERE itemCode='" + itemCode + "' AND wHCode=@frmWHouse;";
                            db.executeNonQuery(sql, vars.MySqlConnection);
                            cntProcessed++;
                        }
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import Goods Return
                cntProcessed = 0;
                if (tableName == "goodsreturn")
                {
                    string docId,vendorCode,vendorName,postingDate,warehouse,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        vendorCode = dt.Rows[a]["vendorCode"].ToString();
                        vendorName = transformString4SQL(dt.Rows[a]["vendorName"].ToString());
                        postingDate = dt.Rows[a]["postingDate"].ToString();
                        warehouse = dt.Rows[a]["warehouse"].ToString();
                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());
                        remarks2 = transformString4SQL(dt.Rows[a]["remarks2"].ToString());
                        totalPrcntDscnt = dt.Rows[a]["totalPrcntDscnt"].ToString();
                        totalAmtDscnt = dt.Rows[a]["totalAmtDscnt"].ToString();
                        netTotal = dt.Rows[a]["netTotal"].ToString();
                        grossTotal = dt.Rows[a]["grossTotal"].ToString();
                        createDate = dt.Rows[a]["createDate"].ToString();
                        createdBy = dt.Rows[a]["createdBy"].ToString();
                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO goodsreturn(docId,vendorCode,vendorName,postingDate,warehouse,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy,exported)";
                        sql += " VALUES('" + docId + "','" + vendorCode + "'," + vendorName + ",'" + postingDate + "','" + warehouse + "'," + remarks1 + "," + remarks2 + "," + totalPrcntDscnt + "," + totalAmtDscnt + "," + netTotal + "," + grossTotal + ",'" + createDate + "'," + createdBy + "," + updateDate + "," + updatedBy + ",1)";
                        sql += " ON DUPLICATE KEY UPDATE id=VALUES(id),remarks1=VALUES(remarks1),remarks2=VALUES(remarks2),updateDate=VALUES(updateDate),updatedBy=VALUES(updatedBy)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // import Goods Return items
                cntProcessed = 0;
                if (tableName == "goodsreturn-item")
                {
                    string docId,indx,itemCode,description,warehouse,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        indx = dt.Rows[a]["indx"].ToString();
                        itemCode = dt.Rows[a]["itemCode"].ToString();
                        description = transformString4SQL(dt.Rows[a]["description"].ToString());
                        warehouse = dt.Rows[a]["warehouse"].ToString();
                        vatable = dt.Rows[a]["vatable"].ToString();
                        realBsNetPrchsPrc = dt.Rows[a]["realBsNetPrchsPrc"].ToString();
                        realBsGrossPrchsPrc = dt.Rows[a]["realBsGrossPrchsPrc"].ToString();
                        realNetPrchsPrc = dt.Rows[a]["realNetPrchsPrc"].ToString();
                        realGrossPrchsPrc = dt.Rows[a]["realGrossPrchsPrc"].ToString();
                        qty = dt.Rows[a]["qty"].ToString();
                        baseUoM = dt.Rows[a]["baseUoM"].ToString();
                        qtyPrPrchsUoM = dt.Rows[a]["qtyPrPrchsUoM"].ToString();
                        prcntDscnt = dt.Rows[a]["prcntDscnt"].ToString();
                        amtDscnt = dt.Rows[a]["amtDscnt"].ToString();
                        netPrchsPrc = dt.Rows[a]["netPrchsPrc"].ToString();
                        grossPrchsPrc = dt.Rows[a]["grossPrchsPrc"].ToString();
                        rowNetTotal = dt.Rows[a]["rowNetTotal"].ToString();
                        rowGrossTotal = dt.Rows[a]["rowGrossTotal"].ToString();

                        sql = "INSERT IGNORE INTO goodsreturn_item(docId,indx,itemCode,description,warehouse,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal,exported)";
                        sql += " VALUES('" + docId + "'," + indx + ",'" + itemCode + "'," + description + ",'" + warehouse + "','" + vatable + "'," + realBsNetPrchsPrc + "," + realBsGrossPrchsPrc + "," + realNetPrchsPrc + "," + realGrossPrchsPrc + "," + qty + ",'" + baseUoM + "'," + qtyPrPrchsUoM + "," + prcntDscnt + "," + amtDscnt + "," + netPrchsPrc + "," + grossPrchsPrc + "," + rowNetTotal + "," + rowGrossTotal + ",1)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                        {
                            baseQty = ((baseUoM == "N") ? Decimal.Parse(qtyPrPrchsUoM) * Decimal.Parse(qty) : Decimal.Parse(qty));
                            sql = "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES('" + itemCode + "','" + warehouse + "'," + baseQty + ") ON DUPLICATE KEY UPDATE inStock=ifnull(inStock, 0)-" + baseQty;
                            db.executeNonQuery(sql, vars.MySqlConnection);
                            cntProcessed++;
                        }
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import Sales Invoice
                cntProcessed = 0;
                if (tableName == "salesinvoice")
                {
                    string docId,customerCode,customerName,warehouse,postingDate,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        customerCode = dt.Rows[a]["customerCode"].ToString();
                        customerName = transformString4SQL(dt.Rows[a]["customerName"].ToString());
                        warehouse = dt.Rows[a]["warehouse"].ToString();
                        postingDate = transformDate4SQL(dt.Rows[a]["postingDate"].ToString());
                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());
                        remarks2 = transformString4SQL(dt.Rows[a]["remarks2"].ToString());
                        totalPrcntDscnt = dt.Rows[a]["totalPrcntDscnt"].ToString();
                        totalAmtDscnt = dt.Rows[a]["totalAmtDscnt"].ToString();
                        netTotal = dt.Rows[a]["netTotal"].ToString();
                        grossTotal = dt.Rows[a]["grossTotal"].ToString();
                        createDate = transformDate4SQL(dt.Rows[a]["createDate"].ToString());
                        createdBy = dt.Rows[a]["createdBy"].ToString();
                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO salesinvoice(docId,customerCode,customerName,warehouse,postingDate,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy,exported)";
                        sql += " VALUES('" + docId + "','" + customerCode + "'," + customerName + ",'" + warehouse + "'," + postingDate + "," + remarks1 + "," + remarks2 + "," + totalPrcntDscnt + "," + totalAmtDscnt + "," + netTotal + "," + grossTotal + "," + createDate + "," + createdBy + "," + updateDate + "," + updatedBy + ",1)";
                        sql += " ON DUPLICATE KEY UPDATE id=VALUES(id),remarks1=VALUES(remarks1),remarks2=VALUES(remarks2),updateDate=VALUES(updateDate),updatedBy=VALUES(updatedBy)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // import sales invoice items
                cntProcessed = 0;
                if (tableName == "salesinvoice-item")
                {
                    string docId,indx,itemCode,description,vatable,saleUoM,qtyPrSaleUoM,netPrchsPrc,grossPrchsPrc,realBsNetSalePrc,realBsGrossSalePrc,qty,baseUoM,prcntDscnt,amtDscnt,netSalePrc,grossSalePrc,rowNetTotal,rowGrossTotal;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        indx = dt.Rows[a]["indx"].ToString();
                        itemCode = dt.Rows[a]["itemCode"].ToString();
                        description = transformString4SQL(dt.Rows[a]["description"].ToString());
                        vatable = dt.Rows[a]["vatable"].ToString();
                        saleUoM = dt.Rows[a]["saleUoM"].ToString();
                        qtyPrSaleUoM = dt.Rows[a]["qtyPrSaleUoM"].ToString();
                        netPrchsPrc = dt.Rows[a]["netPrchsPrc"].ToString();
                        grossPrchsPrc = dt.Rows[a]["grossPrchsPrc"].ToString();
                        realBsNetSalePrc = dt.Rows[a]["realBsNetSalePrc"].ToString();
                        realBsGrossSalePrc = dt.Rows[a]["realBsGrossSalePrc"].ToString();
                        qty = dt.Rows[a]["qty"].ToString();
                        baseUoM = dt.Rows[a]["baseUoM"].ToString();
                        prcntDscnt = dt.Rows[a]["prcntDscnt"].ToString();
                        amtDscnt = dt.Rows[a]["amtDscnt"].ToString();
                        netSalePrc = dt.Rows[a]["netSalePrc"].ToString();
                        grossSalePrc = dt.Rows[a]["grossSalePrc"].ToString();
                        rowNetTotal = dt.Rows[a]["rowNetTotal"].ToString();
                        rowGrossTotal = dt.Rows[a]["rowGrossTotal"].ToString();

                        sql = "INSERT IGNORE INTO salesinvoice_item(docId,indx,itemCode,description,vatable,saleUoM,qtyPrSaleUoM,netPrchsPrc,grossPrchsPrc,realBsNetSalePrc,realBsGrossSalePrc,qty,baseUoM,prcntDscnt,amtDscnt,netSalePrc,grossSalePrc,rowNetTotal,rowGrossTotal,exported)";
                        sql += " VALUES('" + docId + "'," + indx + ",'" + itemCode + "'," + description + ",'" + vatable + "','" + saleUoM + "'," + qtyPrSaleUoM + "," + netPrchsPrc + "," + grossPrchsPrc + "," + realBsNetSalePrc + "," + realBsGrossSalePrc + "," + qty + ",'" + baseUoM + "'," + prcntDscnt + "," + amtDscnt + "," + netSalePrc + "," + grossSalePrc + "," + rowNetTotal + "," + rowGrossTotal + ",1)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                        {
                            baseQty = ((baseUoM == "N") ? Decimal.Parse(qtyPrSaleUoM) * Decimal.Parse(qty) : Decimal.Parse(qty));
                            sql = "SET @warehouse=(SELECT warehouse FROM salesinvoice WHERE docId='" + docId + "');";
                            sql += "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES('" + itemCode + "',@warehouse," + baseQty + ") ON DUPLICATE KEY UPDATE inStock=ifnull(inStock, 0)-" + baseQty + ";";
                            db.executeNonQuery(sql, vars.MySqlConnection);
                            cntProcessed++;
                        }
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // let's import Sales Return
                cntProcessed = 0;
                if (tableName == "salesreturn")
                {
                    string docId,customerCode,customerName,warehouse,postingDate,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        customerCode = dt.Rows[a]["customerCode"].ToString();
                        customerName = transformString4SQL(dt.Rows[a]["customerName"].ToString());
                        warehouse = dt.Rows[a]["warehouse"].ToString();
                        postingDate = transformDate4SQL(dt.Rows[a]["postingDate"].ToString());
                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());
                        remarks2 = transformString4SQL(dt.Rows[a]["remarks2"].ToString());
                        totalPrcntDscnt = dt.Rows[a]["totalPrcntDscnt"].ToString();
                        totalAmtDscnt = dt.Rows[a]["totalAmtDscnt"].ToString();
                        netTotal = dt.Rows[a]["netTotal"].ToString();
                        grossTotal = dt.Rows[a]["grossTotal"].ToString();
                        createDate = transformDate4SQL(dt.Rows[a]["createDate"].ToString());
                        createdBy = dt.Rows[a]["createdBy"].ToString();
                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO salesreturn(docId,customerCode,customerName,warehouse,postingDate,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy,exported)";
                        sql += " VALUES('" + docId + "','" + customerCode + "'," + customerName + ",'" + warehouse + "'," + postingDate + "," + remarks1 + "," + remarks2 + "," + totalPrcntDscnt + "," + totalAmtDscnt + "," + netTotal + "," + grossTotal + "," + createDate + "," + createdBy + "," + updateDate + "," + updatedBy + ",1)";
                        sql += " ON DUPLICATE KEY UPDATE id=VALUES(id),remarks1=VALUES(remarks1),remarks2=VALUES(remarks2),updateDate=VALUES(updateDate),updatedBy=VALUES(updatedBy)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                            cntProcessed++;
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                // import Sales Return items
                cntProcessed = 0;
                if (tableName == "salesreturn-item")
                {
                    string docId,indx,itemCode,description,vatable,saleUoM,qtyPrSaleUoM,netPrchsPrc,grossPrchsPrc,realBsNetSalePrc,realBsGrossSalePrc,qty,baseUoM,prcntDscnt,amtDscnt,netSalePrc,grossSalePrc,rowNetTotal,rowGrossTotal;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        docId = dt.Rows[a]["docId"].ToString();
                        indx = dt.Rows[a]["indx"].ToString();
                        itemCode = dt.Rows[a]["itemCode"].ToString();
                        description = transformString4SQL(dt.Rows[a]["description"].ToString());
                        vatable = dt.Rows[a]["vatable"].ToString();
                        saleUoM = dt.Rows[a]["saleUoM"].ToString();
                        qtyPrSaleUoM = dt.Rows[a]["qtyPrSaleUoM"].ToString();
                        netPrchsPrc = dt.Rows[a]["netPrchsPrc"].ToString();
                        grossPrchsPrc = dt.Rows[a]["grossPrchsPrc"].ToString();
                        realBsNetSalePrc = dt.Rows[a]["realBsNetSalePrc"].ToString();
                        realBsGrossSalePrc = dt.Rows[a]["realBsGrossSalePrc"].ToString();
                        qty = dt.Rows[a]["qty"].ToString();
                        baseUoM = dt.Rows[a]["baseUoM"].ToString();
                        prcntDscnt = dt.Rows[a]["prcntDscnt"].ToString();
                        amtDscnt = dt.Rows[a]["amtDscnt"].ToString();
                        netSalePrc = dt.Rows[a]["netSalePrc"].ToString();
                        grossSalePrc = dt.Rows[a]["grossSalePrc"].ToString();
                        rowNetTotal = dt.Rows[a]["rowNetTotal"].ToString();
                        rowGrossTotal = dt.Rows[a]["rowGrossTotal"].ToString();

                        sql = "INSERT IGNORE INTO salesreturn_item(docId,indx,itemCode,description,vatable,saleUoM,qtyPrSaleUoM,netPrchsPrc,grossPrchsPrc,realBsNetSalePrc,realBsGrossSalePrc,qty,baseUoM,prcntDscnt,amtDscnt,netSalePrc,grossSalePrc,rowNetTotal,rowGrossTotal,exported)";
                        sql += " VALUES('" + docId + "'," + indx + ",'" + itemCode + "'," + description + ",'" + vatable + "','" + saleUoM + "'," + qtyPrSaleUoM + "," + netPrchsPrc + "," + grossPrchsPrc + "," + realBsNetSalePrc + "," + realBsGrossSalePrc + "," + qty + ",'" + baseUoM + "'," + prcntDscnt + "," + amtDscnt + "," + netSalePrc + "," + grossSalePrc + "," + rowNetTotal + "," + rowGrossTotal + ",1)";

                        db = new database();
                        if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                        {
                            baseQty = ((baseUoM == "N") ? Decimal.Parse(qtyPrSaleUoM) * Decimal.Parse(qty) : Decimal.Parse(qty));
                            sql = "SET @warehouse=(SELECT warehouse FROM salesreturn WHERE docId='" + docId + "');";
                            sql += "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES('" + itemCode + "',@warehouse," + baseQty + ") ON DUPLICATE KEY UPDATE inStock=ifnull(inStock, 0)+" + baseQty + ";";
                            db.executeNonQuery(sql, vars.MySqlConnection);
                            cntProcessed++;
                        }
                    }
                    if (cntProcessed == recordsCount)
                    {
                        recordImportedFiles(filename, fullPath);
                        moveFile(ref i, ref filename);
                    }
                }

                lblCount.Text = listView1.Items.Count + " item(s) found.";
            }

            if (listView1.Items.Count < 1)
                btnImport.Enabled = false;
        }
    }
}