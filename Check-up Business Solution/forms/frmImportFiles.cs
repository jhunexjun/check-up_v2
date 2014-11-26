using System;
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
            sql = "INSERT INTO export_importfiles(traffic,filename,filepath,createdBy, createDate) VALUES('In','" + filename + "','" + fullPath + "','" + vars.username + "', DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s'))";
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

        /* Note: 1. we should not allow to continue importing when constrainsts fail.
                 2. We always set exported = 1 as we assume data are coming from other branches. */
        private void btnImport_Click(object sender, EventArgs e)
        {
            // we want to refresh first the list before trying to import.
            listView1.Items.Clear();
            frmImportFiles_Load(sender, e);

            DialogResult result = MessageBox.Show(this, "Import the files on the list now? Please wait until it finished importing to avoid inventory problems.", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            btnImport.Enabled = false;

            string tableName, defaultError = "cannot be empty string or null.";
            int recordsCount = 0, a, cntProcessed;

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                cntProcessed = 0;

                fullPath = listView1.Items[i].SubItems[1].Text;
                filename = listView1.Items[i].Text;
                position = filename.IndexOf("_");
                tableName = filename.Substring(0, position);

                try
                {
                    using (StreamReader file = File.OpenText(fullPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        dt = (DataTable)serializer.Deserialize(file, typeof(DataTable));
                    }
                }
                catch (IOException ioException)
                {
                    MessageBox.Show(this, ioException.ToString(), "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
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
                        createDate = transformDate4SQLNotNull(dt.Rows[a]["createDate"].ToString());
                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        createdBy = dt.Rows[a]["createdBy"].ToString();

                        picLocation = transformString4SQL(dt.Rows[a]["picLocation"].ToString().Replace(@"\", @"\\"));
                        deactivated = transformString4SQL(dt.Rows[a]["deactivated"].ToString());

                        sql = "INSERT INTO users(username,password,fName,midName,lName,email,address,gender,lastLogIn,createDate,updateDate,updatedBy,createdBy,picLocation,deactivated,role,exported)";
                        sql += " VALUES('" + username + "','" + password + "'," + fName + "," + midName + "," + lName + "," + email + "," + address + "," + gender + "," + lastLogIn + ",'" + createDate + "'," + updateDate + "," + updatedBy + ",'" + createdBy + "'," + picLocation + "," + deactivated + "," + role + ",1)";
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

                        if (dt.Rows[a]["deactivated"] == DBNull.Value || dt.Rows[a]["deactivated"].ToString() == "")
                        {
                            MessageBox.Show(this, "warehouse.deactivated " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            deactivated = dt.Rows[a]["deactivated"].ToString();

                        DateTime dateTime;
                        if (dt.Rows[a]["createDate"] == DBNull.Value || dt.Rows[a]["createDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "warehouse.createDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
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

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "warehouse.createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();

                        if (dt.Rows[a]["updatedBy"] != DBNull.Value)
                            updatedBy = dt.Rows[a]["updatedBy"].ToString();
                        else
                            updatedBy = null;

                        if (dt.Rows[a]["trans"] != DBNull.Value)
                            trans = dt.Rows[a]["trans"].ToString();
                        else
                            trans = null;

                        sql = "INSERT INTO warehouse(`code`,`name`,branchType,ftp_url,ftp_username,ftp_password,deactivated,createDate,createdBy,updateDate,updatedBy,trans,exported)";
                        sql += " VALUES(@code, @name, @branchType, @ftp_url, @ftp_username, @ftp_password, @deactivated, @createDate, @createdBy, @updateDate, @updatedBy, @trans,@exported)";
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
                        cmd.Parameters.AddWithValue("@exported", 1);

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

                        if (dt.Rows[a]["totalPrcntDscnt"] == DBNull.Value || dt.Rows[a]["totalPrcntDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder.totalPrcntDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalPrcntDscnt = Convert.ToDecimal(dt.Rows[a]["totalPrcntDscnt"]);

                        if (dt.Rows[a]["totalAmtDscnt"] == DBNull.Value || dt.Rows[a]["totalAmtDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder.totalAmtDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalAmtDscnt = Convert.ToDecimal(dt.Rows[a]["totalAmtDscnt"]);

                        if (dt.Rows[a]["netTotal"] == DBNull.Value || dt.Rows[a]["netTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder.netTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netTotal = Convert.ToDecimal(dt.Rows[a]["netTotal"]);

                        if (dt.Rows[a]["grossTotal"] == DBNull.Value || dt.Rows[a]["grossTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder.grossTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossTotal = Convert.ToDecimal(dt.Rows[a]["grossTotal"]);

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
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["indx"] == DBNull.Value || dt.Rows[a]["indx"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.indx " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            indx = dt.Rows[a]["indx"].ToString();

                        if (dt.Rows[a]["itemCode"] == DBNull.Value || dt.Rows[a]["itemCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.itemCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            itemCode = dt.Rows[a]["itemCode"].ToString();


                        description = transformString4SQL(dt.Rows[a]["description"].ToString());

                        if (dt.Rows[a]["vatable"] == DBNull.Value || dt.Rows[a]["vatable"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.vatable " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            vatable = dt.Rows[a]["vatable"].ToString();

                        if (dt.Rows[a]["realBsNetPrchsPrc"] == DBNull.Value || dt.Rows[a]["realBsNetPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.realBsNetPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsNetPrchsPrc = dt.Rows[a]["realBsNetPrchsPrc"].ToString();

                        if (dt.Rows[a]["realBsGrossPrchsPrc"] == DBNull.Value || dt.Rows[a]["realBsGrossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.realBsGrossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsGrossPrchsPrc = dt.Rows[a]["realBsGrossPrchsPrc"].ToString();

                        if (dt.Rows[a]["realNetPrchsPrc"] == DBNull.Value || dt.Rows[a]["realNetPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.realNetPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realNetPrchsPrc = dt.Rows[a]["realNetPrchsPrc"].ToString();

                        if (dt.Rows[a]["realGrossPrchsPrc"] == DBNull.Value || dt.Rows[a]["realGrossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.realGrossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realGrossPrchsPrc = dt.Rows[a]["realGrossPrchsPrc"].ToString();

                        if (dt.Rows[a]["qty"] == DBNull.Value || dt.Rows[a]["qty"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.qty " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qty = dt.Rows[a]["qty"].ToString();

                        if (dt.Rows[a]["baseUoM"] == DBNull.Value || dt.Rows[a]["baseUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.baseUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            baseUoM = dt.Rows[a]["baseUoM"].ToString();

                        if (dt.Rows[a]["qtyPrPrchsUoM"] == DBNull.Value || dt.Rows[a]["qtyPrPrchsUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.qtyPrPrchsUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qtyPrPrchsUoM = dt.Rows[a]["qtyPrPrchsUoM"].ToString();

                        if (dt.Rows[a]["prcntDscnt"] == DBNull.Value || dt.Rows[a]["prcntDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.prcntDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            prcntDscnt = dt.Rows[a]["prcntDscnt"].ToString();

                        if (dt.Rows[a]["amtDscnt"] == DBNull.Value || dt.Rows[a]["amtDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.amtDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            amtDscnt = dt.Rows[a]["amtDscnt"].ToString();

                        if (dt.Rows[a]["netPrchsPrc"] == DBNull.Value || dt.Rows[a]["netPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.netPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netPrchsPrc = dt.Rows[a]["netPrchsPrc"].ToString();

                        if (dt.Rows[a]["grossPrchsPrc"] == DBNull.Value || dt.Rows[a]["grossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.grossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossPrchsPrc = dt.Rows[a]["grossPrchsPrc"].ToString();

                        if (dt.Rows[a]["rowNetTotal"] == DBNull.Value || dt.Rows[a]["rowNetTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.rowNetTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            rowNetTotal = dt.Rows[a]["rowNetTotal"].ToString();

                        if (dt.Rows[a]["rowGrossTotal"] == DBNull.Value || dt.Rows[a]["rowGrossTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "purchaseorder_item.rowGrossTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            rowGrossTotal = dt.Rows[a]["rowGrossTotal"].ToString();

                        sql = "INSERT IGNORE INTO purchaseorder_item(docId,indx,itemCode,description,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal,exported)";
                        sql += " VALUES('" + docId + "'," + indx + ",'" + itemCode + "'," + description + ",'" + vatable + "'," + realBsNetPrchsPrc + "," + realBsGrossPrchsPrc + "," + realNetPrchsPrc + "," + realGrossPrchsPrc + "," + qty + ",'" + baseUoM + "'," + qtyPrPrchsUoM + "," + prcntDscnt + "," + amtDscnt + "," + netPrchsPrc + "," + grossPrchsPrc + "," + rowNetTotal + "," + rowGrossTotal + ",1)";

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
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["vendorCode"] == DBNull.Value || dt.Rows[a]["vendorCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.vendorCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            vendorCode = dt.Rows[a]["vendorCode"].ToString();

                        vendorName = transformString4SQL(dt.Rows[a]["vendorName"].ToString());

                        if (dt.Rows[a]["warehouse"] == DBNull.Value || dt.Rows[a]["warehouse"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.warehouse " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            warehouse = dt.Rows[a]["warehouse"].ToString();

                        if (dt.Rows[a]["postingDate"] == DBNull.Value || dt.Rows[a]["postingDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.postingDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            postingDate = transformDate4SQLNotNull(dt.Rows[a]["postingDate"].ToString());

                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());
                        remarks2 = transformString4SQL(dt.Rows[a]["remarks2"].ToString());

                        if (dt.Rows[a]["totalPrcntDscnt"] == DBNull.Value || dt.Rows[a]["totalPrcntDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.totalPrcntDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalPrcntDscnt = dt.Rows[a]["totalPrcntDscnt"].ToString();

                        if (dt.Rows[a]["totalAmtDscnt"] == DBNull.Value || dt.Rows[a]["totalAmtDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.totalAmtDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalAmtDscnt = dt.Rows[a]["totalAmtDscnt"].ToString();

                        if (dt.Rows[a]["netTotal"] == DBNull.Value || dt.Rows[a]["netTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.netTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netTotal = dt.Rows[a]["netTotal"].ToString();

                        if (dt.Rows[a]["grossTotal"] == DBNull.Value || dt.Rows[a]["grossTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.grossTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossTotal = dt.Rows[a]["grossTotal"].ToString();

                        if (dt.Rows[a]["createDate"] == DBNull.Value || dt.Rows[a]["createDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.createDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createDate = transformDate4SQLNotNull(dt.Rows[a]["createDate"].ToString());

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();

                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO grpo(docId,vendorCode,vendorName,warehouse,postingDate,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy,exported)";
                        sql += " VALUES('" + docId + "','" + vendorCode + "'," + vendorName + ",'" + warehouse + "','" + postingDate + "'," + remarks1 + "," + remarks2 + "," + totalPrcntDscnt + "," + totalAmtDscnt + "," + netTotal + "," + grossTotal + ",'" + createDate + "','" + createdBy + "'," + updateDate + "," + updatedBy + ",1)";
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
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo_item.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["indx"] == DBNull.Value || dt.Rows[a]["indx"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo_item.indx " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            indx = dt.Rows[a]["indx"].ToString();

                        if (dt.Rows[a]["itemCode"] == DBNull.Value || dt.Rows[a]["itemCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo_item.itemCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            itemCode = dt.Rows[a]["itemCode"].ToString();

                        description = transformString4SQL(dt.Rows[a]["description"].ToString());

                        if (dt.Rows[a]["warehouse"] == DBNull.Value || dt.Rows[a]["warehouse"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo_item.warehouse " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            warehouse = dt.Rows[a]["warehouse"].ToString();

                        if (dt.Rows[a]["vatable"] == DBNull.Value || dt.Rows[a]["vatable"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.vatable " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            vatable = dt.Rows[a]["vatable"].ToString();

                        if (dt.Rows[a]["realBsNetPrchsPrc"] == DBNull.Value || dt.Rows[a]["realBsNetPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.realBsNetPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsNetPrchsPrc = dt.Rows[a]["realBsNetPrchsPrc"].ToString();

                        if (dt.Rows[a]["realBsGrossPrchsPrc"] == DBNull.Value || dt.Rows[a]["realBsGrossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.realBsGrossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsGrossPrchsPrc = dt.Rows[a]["realBsGrossPrchsPrc"].ToString();

                        if (dt.Rows[a]["realNetPrchsPrc"] == DBNull.Value || dt.Rows[a]["realNetPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.realNetPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realNetPrchsPrc = dt.Rows[a]["realNetPrchsPrc"].ToString();

                        if (dt.Rows[a]["realGrossPrchsPrc"] == DBNull.Value || dt.Rows[a]["realGrossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.realGrossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realGrossPrchsPrc = dt.Rows[a]["realGrossPrchsPrc"].ToString();

                        if (dt.Rows[a]["qty"] == DBNull.Value || dt.Rows[a]["qty"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.qty " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qty = dt.Rows[a]["qty"].ToString();

                        if (dt.Rows[a]["baseUoM"] == DBNull.Value || dt.Rows[a]["baseUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.baseUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            baseUoM = dt.Rows[a]["baseUoM"].ToString();

                        if (dt.Rows[a]["qtyPrPrchsUoM"] == DBNull.Value || dt.Rows[a]["qtyPrPrchsUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.qtyPrPrchsUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qtyPrPrchsUoM = dt.Rows[a]["qtyPrPrchsUoM"].ToString();

                        if (dt.Rows[a]["prcntDscnt"] == DBNull.Value || dt.Rows[a]["prcntDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.prcntDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            prcntDscnt = dt.Rows[a]["prcntDscnt"].ToString();

                        if (dt.Rows[a]["amtDscnt"] == DBNull.Value || dt.Rows[a]["amtDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.amtDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            amtDscnt = dt.Rows[a]["amtDscnt"].ToString();

                        if (dt.Rows[a]["netPrchsPrc"] == DBNull.Value || dt.Rows[a]["netPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.netPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netPrchsPrc = dt.Rows[a]["netPrchsPrc"].ToString();

                        if (dt.Rows[a]["grossPrchsPrc"] == DBNull.Value || dt.Rows[a]["grossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.grossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossPrchsPrc = dt.Rows[a]["grossPrchsPrc"].ToString();

                        if (dt.Rows[a]["rowNetTotal"] == DBNull.Value || dt.Rows[a]["rowNetTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.rowNetTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            rowNetTotal = dt.Rows[a]["rowNetTotal"].ToString();

                        if (dt.Rows[a]["rowGrossTotal"] == DBNull.Value || dt.Rows[a]["rowGrossTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "grpo.rowGrossTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
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
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["warehouse"] == DBNull.Value || dt.Rows[a]["warehouse"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt.warehouse " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            warehouse = dt.Rows[a]["warehouse"].ToString();

                        if (dt.Rows[a]["postingDate"] == DBNull.Value || dt.Rows[a]["postingDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt.postingDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            postingDate = transformDate4SQLNotNull(dt.Rows[a]["postingDate"].ToString());

                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());
                        remarks2 = transformString4SQL(dt.Rows[a]["remarks2"].ToString());

                        if (dt.Rows[a]["totalPrcntDscnt"] == DBNull.Value || dt.Rows[a]["totalPrcntDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt.totalPrcntDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalPrcntDscnt = dt.Rows[a]["totalPrcntDscnt"].ToString();

                        if (dt.Rows[a]["totalAmtDscnt"] == DBNull.Value || dt.Rows[a]["totalAmtDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt.totalAmtDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalAmtDscnt = dt.Rows[a]["totalAmtDscnt"].ToString();

                        if (dt.Rows[a]["netTotal"] == DBNull.Value || dt.Rows[a]["netTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt.netTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netTotal = dt.Rows[a]["netTotal"].ToString();

                        if (dt.Rows[a]["grossTotal"] == DBNull.Value || dt.Rows[a]["grossTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt.grossTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossTotal = dt.Rows[a]["grossTotal"].ToString();

                        if (dt.Rows[a]["createDate"] == DBNull.Value || dt.Rows[a]["createDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt.createDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createDate = transformDate4SQLNotNull(dt.Rows[a]["createDate"].ToString());

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt.createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();

                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO deliveryreceipt(docId,warehouse,postingDate,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy,exported)";
                        sql += " VALUES('" + docId + "','" + warehouse + "','" + postingDate + "'," + remarks1 + "," + remarks2 + "," + totalPrcntDscnt + "," + totalAmtDscnt + "," + netTotal + "," + grossTotal + ",'" + createDate + "','" + createdBy + "'," + updateDate + "," + updatedBy + ",1)";
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
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["indx"] == DBNull.Value || dt.Rows[a]["indx"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.indx " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            indx = dt.Rows[a]["indx"].ToString();

                        if (dt.Rows[a]["vendorCode"] == DBNull.Value || dt.Rows[a]["vendorCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.vendorCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            vendorCode = dt.Rows[a]["vendorCode"].ToString();

                        vendorName = transformString4SQL(dt.Rows[a]["vendorName"].ToString());

                        if (dt.Rows[a]["itemCode"] == DBNull.Value || dt.Rows[a]["itemCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.itemCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            itemCode = dt.Rows[a]["itemCode"].ToString();

                        description = transformString4SQL(dt.Rows[a]["description"].ToString());

                        if (dt.Rows[a]["warehouse"] == DBNull.Value || dt.Rows[a]["warehouse"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.warehouse " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            warehouse = dt.Rows[a]["warehouse"].ToString();

                        if (dt.Rows[a]["vatable"] == DBNull.Value || dt.Rows[a]["vatable"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.vatable " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            vatable = dt.Rows[a]["vatable"].ToString();

                        if (dt.Rows[a]["realBsNetPrchsPrc"] == DBNull.Value || dt.Rows[a]["realBsNetPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.realBsNetPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsNetPrchsPrc = dt.Rows[a]["realBsNetPrchsPrc"].ToString();

                        if (dt.Rows[a]["realBsGrossPrchsPrc"] == DBNull.Value || dt.Rows[a]["realBsGrossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.realBsGrossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsGrossPrchsPrc = dt.Rows[a]["realBsGrossPrchsPrc"].ToString();

                        if (dt.Rows[a]["realNetPrchsPrc"] == DBNull.Value || dt.Rows[a]["realNetPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.realNetPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realNetPrchsPrc = dt.Rows[a]["realNetPrchsPrc"].ToString();

                        if (dt.Rows[a]["realGrossPrchsPrc"] == DBNull.Value || dt.Rows[a]["realGrossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.realGrossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realGrossPrchsPrc = dt.Rows[a]["realGrossPrchsPrc"].ToString();

                        if (dt.Rows[a]["qty"] == DBNull.Value || dt.Rows[a]["qty"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.qty " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qty = dt.Rows[a]["qty"].ToString();

                        if (dt.Rows[a]["baseUoM"] == DBNull.Value || dt.Rows[a]["baseUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.baseUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            baseUoM = dt.Rows[a]["baseUoM"].ToString();

                        if (dt.Rows[a]["qtyPrPrchsUoM"] == DBNull.Value || dt.Rows[a]["qtyPrPrchsUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.qtyPrPrchsUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qtyPrPrchsUoM = dt.Rows[a]["qtyPrPrchsUoM"].ToString();

                        if (dt.Rows[a]["prcntDscnt"] == DBNull.Value || dt.Rows[a]["prcntDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.prcntDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            prcntDscnt = dt.Rows[a]["prcntDscnt"].ToString();

                        if (dt.Rows[a]["amtDscnt"] == DBNull.Value || dt.Rows[a]["amtDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.amtDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            amtDscnt = dt.Rows[a]["amtDscnt"].ToString();

                        if (dt.Rows[a]["netPrchsPrc"] == DBNull.Value || dt.Rows[a]["netPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.netPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netPrchsPrc = dt.Rows[a]["netPrchsPrc"].ToString();

                        if (dt.Rows[a]["grossPrchsPrc"] == DBNull.Value || dt.Rows[a]["grossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.grossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossPrchsPrc = dt.Rows[a]["grossPrchsPrc"].ToString();

                        if (dt.Rows[a]["rowNetTotal"] == DBNull.Value || dt.Rows[a]["rowNetTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.rowNetTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            rowNetTotal = dt.Rows[a]["rowNetTotal"].ToString();

                        if (dt.Rows[a]["rowGrossTotal"] == DBNull.Value || dt.Rows[a]["rowGrossTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "deliveryreceipt_item.rowGrossTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
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
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["postingDate"] == DBNull.Value || dt.Rows[a]["postingDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            postingDate = transformDate4SQLNotNull(dt.Rows[a]["postingDate"].ToString());

                        if (dt.Rows[a]["countDateTime"] == DBNull.Value || dt.Rows[a]["countDateTime"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting.countDateTime " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            countDateTime = transformDate4SQLNotNull(dt.Rows[a]["countDateTime"].ToString());

                        if (dt.Rows[a]["warehouse"] == DBNull.Value || dt.Rows[a]["warehouse"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting.warehouse " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            warehouse = dt.Rows[a]["warehouse"].ToString();

                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());

                        if (dt.Rows[a]["createDate"] == DBNull.Value || dt.Rows[a]["createDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting.createDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createDate = transformDate4SQLNotNull(dt.Rows[a]["createDate"].ToString());

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting.createDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();

                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO inventoryposting(docId,postingDate,countDateTime,warehouse,remarks1,createDate,createdBy,updateDate,updatedBy,exported)";
                        sql += " VALUES('" + docId + "','" + postingDate + "','" + countDateTime + "','" + warehouse + "'," + remarks1 + ",'" + createDate + "','" + createdBy + "'," + updateDate + "," + updatedBy + ",1)";
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
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting_item.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["indx"] == DBNull.Value || dt.Rows[a]["indx"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting_item.indx " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            indx = dt.Rows[a]["indx"].ToString();

                        if (dt.Rows[a]["itemCode"] == DBNull.Value || dt.Rows[a]["indx"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting_item.indx " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            itemCode = dt.Rows[a]["itemCode"].ToString();

                        description = transformString4SQL(dt.Rows[a]["description"].ToString());

                        if (dt.Rows[a]["vatable"] == DBNull.Value || dt.Rows[a]["vatable"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting_item.vatable " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            vatable = dt.Rows[a]["vatable"].ToString();

                        if (dt.Rows[a]["currentQty"] == DBNull.Value || dt.Rows[a]["currentQty"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting_item.currentQty " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            currentQty = dt.Rows[a]["currentQty"].ToString();

                        if (dt.Rows[a]["countedQty"] == DBNull.Value || dt.Rows[a]["countedQty"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting_item.countedQty " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            countedQty = dt.Rows[a]["countedQty"].ToString();

                        if (dt.Rows[a]["varianceQty"] == DBNull.Value || dt.Rows[a]["varianceQty"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting_item.varianceQty " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            varianceQty = dt.Rows[a]["varianceQty"].ToString();

                        if (dt.Rows[a]["prchsPrc"] == DBNull.Value || dt.Rows[a]["prchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting_item.prchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            prchsPrc = dt.Rows[a]["prchsPrc"].ToString();

                        if (dt.Rows[a]["retailPrc"] == DBNull.Value || dt.Rows[a]["retailPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventoryposting_item.retailPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
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
                                sql += sql + "-" + varianceQty;
                            
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
                    string docId, frmWHouse, toWHouse, postingDate, remarks1, remarks2, totalPrcntDscnt, totalAmtDscnt, netTotal, grossTotal, totalPrcntDscntRtl, totalAmtDscntRtl, netTotalRtl, grossTotalRtl, createDate, createdBy, updateDate, updatedBy;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["frmWHouse"] == DBNull.Value || dt.Rows[a]["frmWHouse"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer.frmWHouse " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            frmWHouse = dt.Rows[a]["frmWHouse"].ToString();

                        if (dt.Rows[a]["toWHouse"] == DBNull.Value || dt.Rows[a]["toWHouse"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer.toWHouse " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            toWHouse = dt.Rows[a]["toWHouse"].ToString();

                        if (dt.Rows[a]["postingDate"] == DBNull.Value || dt.Rows[a]["postingDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer.postingDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            postingDate = transformDate4SQLNotNull(dt.Rows[a]["postingDate"].ToString());

                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());
                        remarks2 = transformString4SQL(dt.Rows[a]["remarks2"].ToString());

                        if (dt.Rows[a]["totalPrcntDscnt"] == DBNull.Value || dt.Rows[a]["totalPrcntDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer.totalPrcntDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalPrcntDscnt = dt.Rows[a]["totalPrcntDscnt"].ToString();

                        if (dt.Rows[a]["totalAmtDscnt"] == DBNull.Value || dt.Rows[a]["totalAmtDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer.totalAmtDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalAmtDscnt = dt.Rows[a]["totalAmtDscnt"].ToString();

                        if (dt.Rows[a]["netTotal"] == DBNull.Value || dt.Rows[a]["netTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer.netTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netTotal = dt.Rows[a]["netTotal"].ToString();

                        if (dt.Rows[a]["grossTotal"] == DBNull.Value || dt.Rows[a]["grossTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer.grossTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossTotal = dt.Rows[a]["grossTotal"].ToString();

                        if (dt.Rows[a]["totalPrcntDscntRtl"] == DBNull.Value || dt.Rows[a]["totalPrcntDscntRtl"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer.totalPrcntDscntRtl " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalPrcntDscntRtl = dt.Rows[a]["totalPrcntDscntRtl"].ToString();

                        if (dt.Rows[a]["totalAmtDscntRtl"] == DBNull.Value || dt.Rows[a]["totalAmtDscntRtl"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer.totalAmtDscntRtl " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalAmtDscntRtl = dt.Rows[a]["totalAmtDscntRtl"].ToString();

                        if (dt.Rows[a]["netTotalRtl"] == DBNull.Value || dt.Rows[a]["netTotalRtl"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer.netTotalRtl " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netTotalRtl = dt.Rows[a]["netTotalRtl"].ToString();

                        if (dt.Rows[a]["grossTotalRtl"] == DBNull.Value || dt.Rows[a]["grossTotalRtl"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer.grossTotalRtl " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossTotalRtl = dt.Rows[a]["grossTotalRtl"].ToString();

                        if (dt.Rows[a]["createDate"] == DBNull.Value || dt.Rows[a]["createDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer.createDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createDate = transformDate4SQLNotNull(dt.Rows[a]["createDate"].ToString());

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer.createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();

                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO inventorytransfer(docId,frmWHouse,toWHouse,postingDate,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,totalPrcntDscntRtl,totalAmtDscntRtl,netTotalRtl,grossTotalRtl,createDate,createdBy,updateDate,updatedBy,exported)";
                        sql += " VALUES('" + docId + "','" + frmWHouse + "','" + toWHouse + "','" + postingDate + "'," + remarks1 + "," + remarks2 + "," + totalPrcntDscnt + "," + totalAmtDscnt + "," + netTotal + "," + grossTotal + "," + totalPrcntDscntRtl + "," + totalAmtDscntRtl + "," + netTotalRtl + "," + grossTotalRtl + ",'" + createDate + "','" + createdBy + "'," + updateDate + "," + updatedBy + ",1)";
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
                    string docId,indx,itemCode,description,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal,qtyPrRtlUoM,realBsNetPrcRtl,realBsGrossPrcRtl,realNetPrcRtl,realGrossPrcRtl,netPrcRtl,grossPrcRtl,prcntDscntRtl,amtDscntRtl,rowNetTotalRtl,rowGrossTotalRtl;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["indx"] == DBNull.Value || dt.Rows[a]["indx"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.indx " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            indx = dt.Rows[a]["indx"].ToString();

                        if (dt.Rows[a]["itemCode"] == DBNull.Value || dt.Rows[a]["itemCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.itemCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            itemCode = dt.Rows[a]["itemCode"].ToString();

                        description = transformString4SQL(dt.Rows[a]["description"].ToString());

                        if (dt.Rows[a]["vatable"] == DBNull.Value || dt.Rows[a]["vatable"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.vatable " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            vatable = dt.Rows[a]["vatable"].ToString();

                        if (dt.Rows[a]["realBsNetPrchsPrc"] == DBNull.Value || dt.Rows[a]["realBsNetPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.realBsNetPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsNetPrchsPrc = dt.Rows[a]["realBsNetPrchsPrc"].ToString();

                        if (dt.Rows[a]["realBsGrossPrchsPrc"] == DBNull.Value || dt.Rows[a]["realBsGrossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.realBsGrossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsGrossPrchsPrc = dt.Rows[a]["realBsGrossPrchsPrc"].ToString();

                        if (dt.Rows[a]["realNetPrchsPrc"] == DBNull.Value || dt.Rows[a]["realNetPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.realNetPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realNetPrchsPrc = dt.Rows[a]["realNetPrchsPrc"].ToString();

                        if (dt.Rows[a]["realGrossPrchsPrc"] == DBNull.Value || dt.Rows[a]["realGrossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.realGrossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realGrossPrchsPrc = dt.Rows[a]["realGrossPrchsPrc"].ToString();

                        if (dt.Rows[a]["qty"] == DBNull.Value || dt.Rows[a]["qty"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.qty " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qty = dt.Rows[a]["qty"].ToString();

                        if (dt.Rows[a]["baseUoM"] == DBNull.Value || dt.Rows[a]["baseUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.baseUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            baseUoM = dt.Rows[a]["baseUoM"].ToString();

                        if (dt.Rows[a]["qtyPrPrchsUoM"] == DBNull.Value || dt.Rows[a]["qtyPrPrchsUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.qtyPrPrchsUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qtyPrPrchsUoM = dt.Rows[a]["qtyPrPrchsUoM"].ToString();

                        if (dt.Rows[a]["prcntDscnt"] == DBNull.Value || dt.Rows[a]["prcntDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.prcntDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            prcntDscnt = dt.Rows[a]["prcntDscnt"].ToString();

                        if (dt.Rows[a]["amtDscnt"] == DBNull.Value || dt.Rows[a]["amtDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.amtDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            amtDscnt = dt.Rows[a]["amtDscnt"].ToString();

                        if (dt.Rows[a]["netPrchsPrc"] == DBNull.Value || dt.Rows[a]["netPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.netPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netPrchsPrc = dt.Rows[a]["netPrchsPrc"].ToString();

                        if (dt.Rows[a]["grossPrchsPrc"] == DBNull.Value || dt.Rows[a]["grossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.grossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossPrchsPrc = dt.Rows[a]["grossPrchsPrc"].ToString();

                        if (dt.Rows[a]["rowNetTotal"] == DBNull.Value || dt.Rows[a]["rowNetTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.rowNetTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            rowNetTotal = dt.Rows[a]["rowNetTotal"].ToString();

                        if (dt.Rows[a]["rowGrossTotal"] == DBNull.Value || dt.Rows[a]["rowGrossTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.rowGrossTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            rowGrossTotal = dt.Rows[a]["rowGrossTotal"].ToString();

                        if (dt.Rows[a]["qtyPrRtlUoM"] == DBNull.Value || dt.Rows[a]["qtyPrRtlUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.qtyPrRtlUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qtyPrRtlUoM = dt.Rows[a]["qtyPrRtlUoM"].ToString();

                        if (dt.Rows[a]["realBsNetPrcRtl"] == DBNull.Value || dt.Rows[a]["realBsNetPrcRtl"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.realBsNetPrcRtl " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsNetPrcRtl = dt.Rows[a]["realBsNetPrcRtl"].ToString();

                        if (dt.Rows[a]["realBsGrossPrcRtl"] == DBNull.Value || dt.Rows[a]["realBsGrossPrcRtl"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.realBsGrossPrcRtl " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsGrossPrcRtl = dt.Rows[a]["realBsGrossPrcRtl"].ToString();

                        if (dt.Rows[a]["realNetPrcRtl"] == DBNull.Value || dt.Rows[a]["realNetPrcRtl"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.realNetPrcRtl " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realNetPrcRtl = dt.Rows[a]["realNetPrcRtl"].ToString();

                        if (dt.Rows[a]["realGrossPrcRtl"] == DBNull.Value || dt.Rows[a]["realGrossPrcRtl"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.realGrossPrcRtl " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realGrossPrcRtl = dt.Rows[a]["realGrossPrcRtl"].ToString();

                        if (dt.Rows[a]["netPrcRtl"] == DBNull.Value || dt.Rows[a]["netPrcRtl"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.netPrcRtl " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netPrcRtl = dt.Rows[a]["netPrcRtl"].ToString();

                        if (dt.Rows[a]["grossPrcRtl"] == DBNull.Value || dt.Rows[a]["grossPrcRtl"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.grossPrcRtl " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossPrcRtl = dt.Rows[a]["grossPrcRtl"].ToString();

                        if (dt.Rows[a]["prcntDscntRtl"] == DBNull.Value || dt.Rows[a]["prcntDscntRtl"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.prcntDscntRtl " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            prcntDscntRtl = dt.Rows[a]["prcntDscntRtl"].ToString();

                        if (dt.Rows[a]["amtDscntRtl"] == DBNull.Value || dt.Rows[a]["amtDscntRtl"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.amtDscntRtl " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            amtDscntRtl = dt.Rows[a]["amtDscntRtl"].ToString();

                        if (dt.Rows[a]["rowNetTotalRtl"] == DBNull.Value || dt.Rows[a]["rowNetTotalRtl"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.rowNetTotalRtl " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            rowNetTotalRtl = dt.Rows[a]["rowNetTotalRtl"].ToString();

                        if (dt.Rows[a]["rowGrossTotalRtl"] == DBNull.Value || dt.Rows[a]["rowGrossTotalRtl"].ToString() == "")
                        {
                            MessageBox.Show(this, "inventorytransfer_item.rowGrossTotalRtl " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            rowGrossTotalRtl = dt.Rows[a]["rowGrossTotalRtl"].ToString();

                        sql = "INSERT IGNORE INTO inventorytransfer_item(docId,indx,itemCode,description,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal,qtyPrRtlUoM,realBsNetPrcRtl,realBsGrossPrcRtl,realNetPrcRtl,realGrossPrcRtl,netPrcRtl,grossPrcRtl,prcntDscntRtl,amtDscntRtl,rowNetTotalRtl,rowGrossTotalRtl,exported)";
                        sql += " VALUES('" + docId + "'," + indx + ",'" + itemCode + "'," + description + ",'" + vatable + "'," + realBsNetPrchsPrc + "," + realBsGrossPrchsPrc + "," + realNetPrchsPrc + "," + realGrossPrchsPrc + "," + qty + ",'" + baseUoM + "'," + qtyPrPrchsUoM + "," + prcntDscnt + "," + amtDscnt + "," + netPrchsPrc + "," + grossPrchsPrc + "," + rowNetTotal + "," + rowGrossTotal + "," + qtyPrRtlUoM + "," + realBsNetPrcRtl + "," + realBsGrossPrcRtl + "," + realNetPrcRtl + "," +realGrossPrcRtl + "," + netPrcRtl + "," + grossPrcRtl + "," + prcntDscntRtl + "," + amtDscntRtl + "," + rowNetTotalRtl + "," + rowGrossTotalRtl + ",1)";

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
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["vendorCode"] == DBNull.Value || dt.Rows[a]["vendorCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn.vendorCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            vendorCode = dt.Rows[a]["vendorCode"].ToString();

                        if (dt.Rows[a]["vendorName"] == DBNull.Value || dt.Rows[a]["vendorName"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn.vendorName " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            vendorName = transformString4SQL(dt.Rows[a]["vendorName"].ToString());

                        if (dt.Rows[a]["postingDate"] == DBNull.Value || dt.Rows[a]["postingDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn.postingDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            postingDate = transformDate4SQLNotNull(dt.Rows[a]["postingDate"].ToString());

                        if (dt.Rows[a]["warehouse"] == DBNull.Value || dt.Rows[a]["warehouse"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn.warehouse " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            warehouse = dt.Rows[a]["warehouse"].ToString();

                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());
                        remarks2 = transformString4SQL(dt.Rows[a]["remarks2"].ToString());

                        if (dt.Rows[a]["totalPrcntDscnt"] == DBNull.Value || dt.Rows[a]["totalPrcntDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn.totalPrcntDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalPrcntDscnt = dt.Rows[a]["totalPrcntDscnt"].ToString();

                        if (dt.Rows[a]["totalAmtDscnt"] == DBNull.Value || dt.Rows[a]["totalAmtDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn.totalAmtDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalAmtDscnt = dt.Rows[a]["totalAmtDscnt"].ToString();

                        if (dt.Rows[a]["netTotal"] == DBNull.Value || dt.Rows[a]["netTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn.netTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netTotal = dt.Rows[a]["netTotal"].ToString();

                        if (dt.Rows[a]["grossTotal"] == DBNull.Value || dt.Rows[a]["grossTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn.grossTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossTotal = dt.Rows[a]["grossTotal"].ToString();

                        if (dt.Rows[a]["createDate"] == DBNull.Value || dt.Rows[a]["createDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn.createDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createDate = transformDate4SQLNotNull(dt.Rows[a]["createDate"].ToString());

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn.createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();

                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO goodsreturn(docId,vendorCode,vendorName,postingDate,warehouse,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy,exported)";
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

                // import Goods Return items
                cntProcessed = 0;
                if (tableName == "goodsreturn-item")
                {
                    string docId,indx,itemCode,description,warehouse,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal;

                    recordsCount = dt.Rows.Count;
                    for (a = 0; a < recordsCount; a++)
                    {
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["indx"] == DBNull.Value || dt.Rows[a]["indx"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.indx " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            indx = dt.Rows[a]["indx"].ToString();

                        if (dt.Rows[a]["itemCode"] == DBNull.Value || dt.Rows[a]["itemCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.itemCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            itemCode = dt.Rows[a]["itemCode"].ToString();

                        description = transformString4SQL(dt.Rows[a]["description"].ToString());

                        if (dt.Rows[a]["warehouse"] == DBNull.Value || dt.Rows[a]["warehouse"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.warehouse " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            warehouse = dt.Rows[a]["warehouse"].ToString();

                        if (dt.Rows[a]["vatable"] == DBNull.Value || dt.Rows[a]["vatable"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.vatable " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            vatable = dt.Rows[a]["vatable"].ToString();

                        if (dt.Rows[a]["realBsNetPrchsPrc"] == DBNull.Value || dt.Rows[a]["realBsNetPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.realBsNetPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsNetPrchsPrc = dt.Rows[a]["realBsNetPrchsPrc"].ToString();

                        if (dt.Rows[a]["realBsGrossPrchsPrc"] == DBNull.Value || dt.Rows[a]["realBsGrossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.realBsGrossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsGrossPrchsPrc = dt.Rows[a]["realBsGrossPrchsPrc"].ToString();

                        if (dt.Rows[a]["realNetPrchsPrc"] == DBNull.Value || dt.Rows[a]["realNetPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.realNetPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realNetPrchsPrc = dt.Rows[a]["realNetPrchsPrc"].ToString();

                        if (dt.Rows[a]["realGrossPrchsPrc"] == DBNull.Value || dt.Rows[a]["realGrossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.realGrossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realGrossPrchsPrc = dt.Rows[a]["realGrossPrchsPrc"].ToString();

                        if (dt.Rows[a]["qty"] == DBNull.Value || dt.Rows[a]["qty"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.qty " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qty = dt.Rows[a]["qty"].ToString();

                        if (dt.Rows[a]["baseUoM"] == DBNull.Value || dt.Rows[a]["baseUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.baseUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            baseUoM = dt.Rows[a]["baseUoM"].ToString();

                        if (dt.Rows[a]["qtyPrPrchsUoM"] == DBNull.Value || dt.Rows[a]["qtyPrPrchsUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.qtyPrPrchsUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qtyPrPrchsUoM = dt.Rows[a]["qtyPrPrchsUoM"].ToString();

                        if (dt.Rows[a]["prcntDscnt"] == DBNull.Value || dt.Rows[a]["prcntDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.prcntDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            prcntDscnt = dt.Rows[a]["prcntDscnt"].ToString();

                        if (dt.Rows[a]["amtDscnt"] == DBNull.Value || dt.Rows[a]["amtDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.amtDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            amtDscnt = dt.Rows[a]["amtDscnt"].ToString();

                        if (dt.Rows[a]["netPrchsPrc"] == DBNull.Value || dt.Rows[a]["netPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.netPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netPrchsPrc = dt.Rows[a]["netPrchsPrc"].ToString();

                        if (dt.Rows[a]["grossPrchsPrc"] == DBNull.Value || dt.Rows[a]["grossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.grossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossPrchsPrc = dt.Rows[a]["grossPrchsPrc"].ToString();

                        if (dt.Rows[a]["rowNetTotal"] == DBNull.Value || dt.Rows[a]["rowNetTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.rowNetTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            rowNetTotal = dt.Rows[a]["rowNetTotal"].ToString();

                        if (dt.Rows[a]["rowGrossTotal"] == DBNull.Value || dt.Rows[a]["rowGrossTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "goodsreturn_item.rowGrossTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
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
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["customerCode"] == DBNull.Value || dt.Rows[a]["customerCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice.customerCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            customerCode = dt.Rows[a]["customerCode"].ToString();

                        customerName = transformString4SQL(dt.Rows[a]["customerName"].ToString());

                        if (dt.Rows[a]["warehouse"] == DBNull.Value || dt.Rows[a]["warehouse"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice.warehouse " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            warehouse = dt.Rows[a]["warehouse"].ToString();

                        if (dt.Rows[a]["postingDate"] == DBNull.Value || dt.Rows[a]["postingDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice.postingDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            postingDate = transformDate4SQLNotNull(dt.Rows[a]["postingDate"].ToString());

                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());
                        remarks2 = transformString4SQL(dt.Rows[a]["remarks2"].ToString());

                        if (dt.Rows[a]["totalPrcntDscnt"] == DBNull.Value || dt.Rows[a]["totalPrcntDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice.totalPrcntDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalPrcntDscnt = dt.Rows[a]["totalPrcntDscnt"].ToString();

                        if (dt.Rows[a]["totalAmtDscnt"] == DBNull.Value || dt.Rows[a]["totalAmtDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice.totalAmtDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalAmtDscnt = dt.Rows[a]["totalAmtDscnt"].ToString();

                        if (dt.Rows[a]["netTotal"] == DBNull.Value || dt.Rows[a]["netTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice.netTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netTotal = dt.Rows[a]["netTotal"].ToString();

                        if (dt.Rows[a]["grossTotal"] == DBNull.Value || dt.Rows[a]["grossTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice.grossTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossTotal = dt.Rows[a]["grossTotal"].ToString();

                        if (dt.Rows[a]["createDate"] == DBNull.Value || dt.Rows[a]["createDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice.createDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createDate = transformDate4SQLNotNull(dt.Rows[a]["createDate"].ToString());

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice.createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();

                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO salesinvoice(docId,customerCode,customerName,warehouse,postingDate,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy,exported)";
                        sql += " VALUES('" + docId + "','" + customerCode + "'," + customerName + ",'" + warehouse + "','" + postingDate + "'," + remarks1 + "," + remarks2 + "," + totalPrcntDscnt + "," + totalAmtDscnt + "," + netTotal + "," + grossTotal + ",'" + createDate + "','" + createdBy + "'," + updateDate + "," + updatedBy + ",1)";
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
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["indx"] == DBNull.Value || dt.Rows[a]["indx"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.indx " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            indx = dt.Rows[a]["indx"].ToString();

                        if (dt.Rows[a]["itemCode"] == DBNull.Value || dt.Rows[a]["itemCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.itemCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            itemCode = dt.Rows[a]["itemCode"].ToString();

                        description = transformString4SQL(dt.Rows[a]["description"].ToString());

                        if (dt.Rows[a]["vatable"] == DBNull.Value || dt.Rows[a]["vatable"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.vatable " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            vatable = dt.Rows[a]["vatable"].ToString();

                        if (dt.Rows[a]["saleUoM"] == DBNull.Value || dt.Rows[a]["saleUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.saleUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            saleUoM = dt.Rows[a]["saleUoM"].ToString();

                        if (dt.Rows[a]["qtyPrSaleUoM"] == DBNull.Value || dt.Rows[a]["qtyPrSaleUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.qtyPrSaleUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qtyPrSaleUoM = dt.Rows[a]["qtyPrSaleUoM"].ToString();

                        if (dt.Rows[a]["netPrchsPrc"] == DBNull.Value || dt.Rows[a]["netPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.netPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netPrchsPrc = dt.Rows[a]["netPrchsPrc"].ToString();

                        if (dt.Rows[a]["grossPrchsPrc"] == DBNull.Value || dt.Rows[a]["grossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.grossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossPrchsPrc = dt.Rows[a]["grossPrchsPrc"].ToString();

                        if (dt.Rows[a]["realBsNetSalePrc"] == DBNull.Value || dt.Rows[a]["realBsNetSalePrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.realBsNetSalePrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsNetSalePrc = dt.Rows[a]["realBsNetSalePrc"].ToString();

                        if (dt.Rows[a]["realBsGrossSalePrc"] == DBNull.Value || dt.Rows[a]["realBsGrossSalePrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.realBsGrossSalePrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsGrossSalePrc = dt.Rows[a]["realBsGrossSalePrc"].ToString();

                        if (dt.Rows[a]["qty"] == DBNull.Value || dt.Rows[a]["qty"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.qty " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qty = dt.Rows[a]["qty"].ToString();

                        if (dt.Rows[a]["baseUoM"] == DBNull.Value || dt.Rows[a]["baseUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.baseUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            baseUoM = dt.Rows[a]["baseUoM"].ToString();

                        if (dt.Rows[a]["prcntDscnt"] == DBNull.Value || dt.Rows[a]["prcntDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.prcntDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            prcntDscnt = dt.Rows[a]["prcntDscnt"].ToString();

                        if (dt.Rows[a]["amtDscnt"] == DBNull.Value || dt.Rows[a]["amtDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.amtDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            amtDscnt = dt.Rows[a]["amtDscnt"].ToString();

                        if (dt.Rows[a]["netSalePrc"] == DBNull.Value || dt.Rows[a]["netSalePrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.netSalePrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netSalePrc = dt.Rows[a]["netSalePrc"].ToString();

                        if (dt.Rows[a]["grossSalePrc"] == DBNull.Value || dt.Rows[a]["grossSalePrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.grossSalePrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossSalePrc = dt.Rows[a]["grossSalePrc"].ToString();

                        if (dt.Rows[a]["rowNetTotal"] == DBNull.Value || dt.Rows[a]["rowNetTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.rowNetTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            rowNetTotal = dt.Rows[a]["rowNetTotal"].ToString();

                        if (dt.Rows[a]["rowGrossTotal"] == DBNull.Value || dt.Rows[a]["rowGrossTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesinvoice_item.rowGrossTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
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
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["customerCode"] == DBNull.Value || dt.Rows[a]["customerCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn.customerCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            customerCode = dt.Rows[a]["customerCode"].ToString();

                        customerName = transformString4SQL(dt.Rows[a]["customerName"].ToString());

                        if (dt.Rows[a]["warehouse"] == DBNull.Value || dt.Rows[a]["warehouse"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn.warehouse " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            warehouse = dt.Rows[a]["warehouse"].ToString();

                        if (dt.Rows[a]["postingDate"] == DBNull.Value || dt.Rows[a]["postingDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn.postingDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            postingDate = transformDate4SQLNotNull(dt.Rows[a]["postingDate"].ToString());

                        remarks1 = transformString4SQL(dt.Rows[a]["remarks1"].ToString());
                        remarks2 = transformString4SQL(dt.Rows[a]["remarks2"].ToString());

                        if (dt.Rows[a]["totalPrcntDscnt"] == DBNull.Value || dt.Rows[a]["totalPrcntDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn.totalPrcntDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalPrcntDscnt = dt.Rows[a]["totalPrcntDscnt"].ToString();

                        if (dt.Rows[a]["totalAmtDscnt"] == DBNull.Value || dt.Rows[a]["totalAmtDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn.totalAmtDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            totalAmtDscnt = dt.Rows[a]["totalAmtDscnt"].ToString();

                        if (dt.Rows[a]["netTotal"] == DBNull.Value || dt.Rows[a]["netTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn.netTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netTotal = dt.Rows[a]["netTotal"].ToString();

                        if (dt.Rows[a]["grossTotal"] == DBNull.Value || dt.Rows[a]["grossTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn.grossTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossTotal = dt.Rows[a]["grossTotal"].ToString();

                        if (dt.Rows[a]["createDate"] == DBNull.Value || dt.Rows[a]["createDate"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn.createDate " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createDate = transformDate4SQLNotNull(dt.Rows[a]["createDate"].ToString());

                        if (dt.Rows[a]["createdBy"] == DBNull.Value || dt.Rows[a]["createdBy"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn.createdBy " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            createdBy = dt.Rows[a]["createdBy"].ToString();

                        updateDate = transformDate4SQL(dt.Rows[a]["updateDate"].ToString());
                        updatedBy = transformString4SQL(dt.Rows[a]["updatedBy"].ToString());

                        sql = "INSERT INTO salesreturn(docId,customerCode,customerName,warehouse,postingDate,remarks1,remarks2,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,createDate,createdBy,updateDate,updatedBy,exported)";
                        sql += " VALUES('" + docId + "','" + customerCode + "'," + customerName + ",'" + warehouse + "','" + postingDate + "'," + remarks1 + "," + remarks2 + "," + totalPrcntDscnt + "," + totalAmtDscnt + "," + netTotal + "," + grossTotal + ",'" + createDate + "','" + createdBy + "'," + updateDate + "," + updatedBy + ",1)";
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
                        if (dt.Rows[a]["docId"] == DBNull.Value || dt.Rows[a]["docId"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.docId " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            docId = dt.Rows[a]["docId"].ToString();

                        if (dt.Rows[a]["indx"] == DBNull.Value || dt.Rows[a]["indx"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.indx " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            indx = dt.Rows[a]["indx"].ToString();

                        if (dt.Rows[a]["itemCode"] == DBNull.Value || dt.Rows[a]["itemCode"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.itemCode " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            itemCode = dt.Rows[a]["itemCode"].ToString();

                        description = transformString4SQL(dt.Rows[a]["description"].ToString());

                        if (dt.Rows[a]["vatable"] == DBNull.Value || dt.Rows[a]["vatable"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.vatable " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            vatable = dt.Rows[a]["vatable"].ToString();

                        if (dt.Rows[a]["saleUoM"] == DBNull.Value || dt.Rows[a]["saleUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.saleUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            saleUoM = dt.Rows[a]["saleUoM"].ToString();

                        if (dt.Rows[a]["qtyPrSaleUoM"] == DBNull.Value || dt.Rows[a]["qtyPrSaleUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.qtyPrSaleUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qtyPrSaleUoM = dt.Rows[a]["qtyPrSaleUoM"].ToString();

                        if (dt.Rows[a]["netPrchsPrc"] == DBNull.Value || dt.Rows[a]["netPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.netPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netPrchsPrc = dt.Rows[a]["netPrchsPrc"].ToString();

                        if (dt.Rows[a]["grossPrchsPrc"] == DBNull.Value || dt.Rows[a]["grossPrchsPrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.grossPrchsPrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossPrchsPrc = dt.Rows[a]["grossPrchsPrc"].ToString();

                        if (dt.Rows[a]["realBsNetSalePrc"] == DBNull.Value || dt.Rows[a]["realBsNetSalePrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.realBsNetSalePrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsNetSalePrc = dt.Rows[a]["realBsNetSalePrc"].ToString();

                        if (dt.Rows[a]["realBsGrossSalePrc"] == DBNull.Value || dt.Rows[a]["realBsGrossSalePrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.realBsGrossSalePrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            realBsGrossSalePrc = dt.Rows[a]["realBsGrossSalePrc"].ToString();

                        if (dt.Rows[a]["qty"] == DBNull.Value || dt.Rows[a]["qty"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.qty " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            qty = dt.Rows[a]["qty"].ToString();

                        if (dt.Rows[a]["baseUoM"] == DBNull.Value || dt.Rows[a]["baseUoM"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.baseUoM " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            baseUoM = dt.Rows[a]["baseUoM"].ToString();

                        if (dt.Rows[a]["prcntDscnt"] == DBNull.Value || dt.Rows[a]["prcntDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.prcntDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            prcntDscnt = dt.Rows[a]["prcntDscnt"].ToString();

                        if (dt.Rows[a]["amtDscnt"] == DBNull.Value || dt.Rows[a]["amtDscnt"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.amtDscnt " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            amtDscnt = dt.Rows[a]["amtDscnt"].ToString();

                        if (dt.Rows[a]["netSalePrc"] == DBNull.Value || dt.Rows[a]["netSalePrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.netSalePrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            netSalePrc = dt.Rows[a]["netSalePrc"].ToString();

                        if (dt.Rows[a]["grossSalePrc"] == DBNull.Value || dt.Rows[a]["grossSalePrc"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.grossSalePrc " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            grossSalePrc = dt.Rows[a]["grossSalePrc"].ToString();

                        if (dt.Rows[a]["rowNetTotal"] == DBNull.Value || dt.Rows[a]["rowNetTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.rowNetTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            rowNetTotal = dt.Rows[a]["rowNetTotal"].ToString();

                        if (dt.Rows[a]["rowGrossTotal"] == DBNull.Value || dt.Rows[a]["rowGrossTotal"].ToString() == "")
                        {
                            MessageBox.Show(this, "salesreturn_item.rowGrossTotal " + defaultError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
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
            } // end for()

            if (listView1.Items.Count < 1)
            {
                btnImport.Enabled = false;
                MessageBox.Show(this, "All records have been successfully imported to " + Application.StartupPath + @"\json\Processed\", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                btnImport.Enabled = true;
        }
    }
}