using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using MySql.Data.MySqlClient;
using Check_up.classes;

namespace Check_up.forms
{
    public partial class frmItemMasterData : Form
    {
        DataTable table;
        private frmDialog frmDialogForm;
        string sql; bool trans;
        Hashtable htPriceFromDB; Hashtable htNewPrice = new Hashtable();
        Hashtable htBarcodeFromDB; Hashtable htNewBarcode = new Hashtable();
        int i, rowCount;
        decimal d;

        public frmItemMasterData()
        {
            InitializeComponent();
            txtUnitPrice.TextChanged += new EventHandler(txtUnitPrice_TextChanged);
            txtUnitPrice.Leave += new EventHandler(txtUnitPrice_Leave);
            dgvBarcode.Leave += new EventHandler(dgvBarcode_Leave);
            dgvBarcode.EditModeChanged += new EventHandler(dgvBarcode_EditModeChanged);
        }

        void dgvBarcode_EditModeChanged(object sender, EventArgs e)
        {
            dgvBarcode_Leave(sender, e);
        }

        void dgvBarcode_Leave(object sender, EventArgs e)
        {
            htNewBarcode = new Hashtable();
            rowCount = dgvBarcode.Rows.Count;
            for (i = 0; i < rowCount; i++)
            {
                if (!dgvBarcode.Rows[i].IsNewRow && dgvBarcode.Rows[i].Cells[0].Value != null && dgvBarcode.Rows[i].Cells[0].Value.ToString().Trim() != "")
                    htNewBarcode.Add(i, dgvBarcode.Rows[i].Cells[0].Value.ToString().Trim());
            }
        }

        private void cboPriceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUnitPrice.Text = "";
            if (btnFind.Text == "&OK")
            {
                if (htPriceFromDB.ContainsKey((int)priceList(cboPriceList.Text)))
                    txtUnitPrice.Text = htNewPrice[(int)priceList(cboPriceList.Text)].ToString();
            }
            else if (btnFind.Text == "&Update")
            {
                if (htNewPrice.ContainsKey((int)priceList(cboPriceList.Text)))
                    txtUnitPrice.Text = htNewPrice[(int)priceList(cboPriceList.Text)].ToString();
            }
            else if (btnFind.Text == "&Save")
            {
                if (htNewPrice.ContainsKey((int)priceList(cboPriceList.Text)))
                    txtUnitPrice.Text = htNewPrice[(int)priceList(cboPriceList.Text)].ToString();
            }
        }

        private void txtShortName_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        private void txtVendor_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        private void txtRemarks_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        private void chkVatable_CheckedChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        private void chkVarWeightItm_CheckedChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (btnFind.Text == "&OK") editMode();
        }

        void txtUnitPrice_Leave(object sender, EventArgs e)
        {
            if (Decimal.TryParse(txtUnitPrice.Text, out d))
                htNewPrice[(int)priceList(cboPriceList.Text)] = txtUnitPrice.Text.Trim();
        }

        void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {
            //if (btnFind.Text == "&OK") editMode();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (btnFind.Text == "&Find")
            {
                sql = "SELECT A.itemCode,A.description,A.shortName,A.vatable,A.vendor,qtyPrPrchsUoM,qtyPrSaleUoM,prchsUoM,SaleUoM,A.deactivated,A.remarks,A.trans,minStock,maxStock";
                sql += " FROM itemmasterdata A WHERE ";
                if (txtItemCode.Text.Trim() != "" && !txtItemCode.Text.Contains("*"))
                {
                    sql += "itemCode='" + txtItemCode.Text.Trim() + "'";
                    database db = new database();
                    DataTable dt = new DataTable();
                    dt = db.select(sql, vars.MySqlConnection);
                    if (dt.Rows.Count < 1)
                    {
                        MessageBox.Show(this, "No matching record found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    txtItemCode.Text = dt.Rows[0]["itemCode"].ToString();
                    txtDescription.Text = dt.Rows[0]["description"].ToString();
                    txtShortName.Text = dt.Rows[0]["shortName"].ToString();
                    txtVendor.Text = dt.Rows[0]["vendor"].ToString();
                    txtRemarks.Text = dt.Rows[0]["remarks"].ToString();
                    trans = dt.Rows[0]["trans"].ToString() == "Y" ? true : false; //N and null are the same
                    txtPurchaseUoM.Text = dt.Rows[0]["prchsUoM"].ToString();
                    txtQtyPrPrchsUoM.Text = dt.Rows[0]["qtyPrPrchsUoM"].ToString();
                    txtSaleUoM.Text = dt.Rows[0]["saleUoM"].ToString();
                    txtQtyPrSalesUoM.Text = dt.Rows[0]["qtyPrSaleUoM"].ToString();
                    txtMinStock.Text = dt.Rows[0]["minStock"].ToString();
                    txtMaxStock.Text = dt.Rows[0]["maxStock"].ToString();
                    if (trans == true)
                    {
                        txtItemCode.ReadOnly = true;
                        txtVendor.ReadOnly = true;
                        linkVendor.Enabled = false;
                        txtPurchaseUoM.ReadOnly = true;
                        txtSaleUoM.ReadOnly = true;
                        txtQtyPrPrchsUoM.ReadOnly = true;
                        txtQtyPrSalesUoM.ReadOnly = true;
                    }
                    else
                        linkVendor.Enabled = true;

                    if (dt.Rows[0]["vatable"].ToString() == "Y")
                        chkVatable.Checked = true;
                    else
                        chkVatable.Checked = false;

                    if (dt.Rows[0]["deactivated"].ToString() == "Y")
                        chkDeactivate.Checked = true;
                    else
                        chkDeactivate.Checked = false;

                    db = new database();
                    dt = db.select("SELECT barcode FROM barcode WHERE itemCode='" + txtItemCode.Text + "'", vars.MySqlConnection);

                    htBarcodeFromDB = new Hashtable();
                    i = 0;
                    foreach (DataRow r in dt.Rows)
                    {
                        dgvBarcode.Rows.Add(r["barcode"].ToString());
                        htBarcodeFromDB.Add(i, r["barcode"].ToString());
                        i++;
                    }

                    sql = "SELECT priceListCode,netPrice FROM pricelist WHERE itemCode='" + txtItemCode.Text.Trim() + "' ORDER BY priceListCode";
                    db = new database(); dt = new DataTable();
                    dt = db.select(sql, vars.MySqlConnection);

                    htPriceFromDB = new Hashtable();
                    foreach (DataRow r in dt.Rows)
                    {
                        htPriceFromDB[r["priceListCode"]] = r["netPrice"];
                        htNewPrice[r["priceListCode"]] = r["netPrice"];
                    }

                    sql = "SELECT B.*,A.name FROM warehouse A JOIN item_warehouse B ON A.code=B.whCode WHERE itemCode='" + txtItemCode.Text.Trim() + "'";
                    db = new database(); dt = new DataTable();
                    dt = db.select(sql, vars.MySqlConnection);
                    rowCount = dt.Rows.Count;
                    for (i = 0; i < rowCount; i++)
                    {
                        dgvInventory.Rows.Add();
                        dgvInventory.Rows[i].Cells["whCode"].Value = dt.Rows[i]["itemCode"].ToString();
                        dgvInventory.Rows[i].Cells["whName"].Value = dt.Rows[i]["name"].ToString();
                        dgvInventory.Rows[i].Cells["inStock"].Value = dt.Rows[i]["inStock"].ToString();
                    }

                    btnFind.Text = "&OK";
                }
                else
                {
                    registerControls();
                    bool found = false;

                    foreach (Control current_control in this.Controls)
                    {
                        if (current_control is TextBox && current_control.Text.Contains("*"))
                        {
                            foreach (DataRow row in table.Rows)
                            {
                                if ((string)row["controls"] == current_control.Name.ToString() && current_control.Text != "")
                                {
                                    row["Value"] = current_control.Text; //insert the value if current_control is not ""
                                    found = true;
                                }
                            }
                        }
                    }

                    if (found == false)
                    {
                        MessageBox.Show(this, "No matching records found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            if ((string)row["Value"] != "")
                                sql += row["DBcolumnName"] + " like '" + row["Value"].ToString().Replace("*", "%") + "'   AND ";
                        }
                    }

                    sql = sql.Remove(sql.Length - 7);
                    frmDialogForm = new frmDialog();
                    frmDialogForm.selectedValue = sql;
                    frmDialogForm.ShowDialog();
                    txtItemCode.Text = frmDialogForm.selectedValue;
                }
            }
            else if (btnFind.Text == "&OK")
            {
                this.Close();
            }
            else if (btnFind.Text == "&Update")
            {
                if (MessageBox.Show(this, "Are you sure you want to update the records?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                if (checkValues())
                {
                    string deactivate = chkDeactivate.Checked == true ? "Y" : "N";
                    string v = chkVatable.Checked == true ? "Y" : "N";
                    string varWeightItem = chkVarWeightItm.Checked == true ? "Y" : "N";

                    database db = new database();
                    if (trans == false && txtVendor.Text.Trim() != "")
                    {
                        sql = "SELECT code FROM businesspartner WHERE code='" + txtVendor.Text.Trim() + "' AND BPType=0 AND deactivated='N';";
                        if (db.select(sql, vars.MySqlConnection).Rows.Count == 0)
                        {
                            MessageBox.Show(this, "Invalid Vendor code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }

                    db = new database();
                    sql = "START TRANSACTION;";
                    sql += "SET @date=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s');";
                    sql += "SET @user_id=" + vars.user_id + ";";
                    sql += "SET @varWeightItm='" + varWeightItem + "';";
                    sql += "SET @vendor=(SELECT code FROM businesspartner WHERE code='" + txtVendor.Text.Trim() + "' AND BPType=0 AND deactivated='N');";
                    sql += "UPDATE itemmasterdata SET itemCode='" + txtItemCode.Text.Trim() + "',description='" + txtDescription.Text.Trim().Replace("'", "''") + "',shortName='" + txtShortName.Text.Trim().Replace("'", "''") + "',vatable='" + v + "',varWeightItm='" + varWeightItem + "',deactivated='" + deactivate + "',updateDate=@date,updatedBy=@user_id";
                    if (txtRemarks.Text.Trim() != "")
                        sql += ",remarks='" + txtRemarks.Text.Trim().Replace("'", "''") + "'";
                    if (trans == false)
                        sql += ",qtyPrPrchsUoM=" + txtQtyPrPrchsUoM.Text.ToString() + ",qtyPrSaleUoM=" + txtQtyPrSalesUoM.Text.ToString() + ",prchsUoM='" + txtPurchaseUoM.Text.ToString() + "',SaleUoM='" + txtSaleUoM.Text.ToString() + "'";

                    sql += ",minStock=" + txtMinStock.Text.Trim() + ",maxStock=" + txtMaxStock.Text.Trim();
                    sql += ",vendor=@vendor";
                    sql += " WHERE itemCode='" + txtItemCode.Text.Trim() + "';";

                    //compare current barcode(s) and the retrieved barcode(s)
                    rowCount = (Int16)htNewBarcode.Count;
                    for (i = 0; i < rowCount; i++)
                    {
                        if (htNewBarcode[i].ToString() != htBarcodeFromDB[i].ToString())
                        {
                            sql += "INSERT INTO barcodehistory(itemCode,barcode,cp_createDate,cp_createdBy,createDate,createdBy) SELECT itemCode,barcode,createDate,createdBy,@date,@user_id FROM barcode WHERE itemCode='" + txtItemCode.Text.Trim() + "' AND barcode='" + htBarcodeFromDB[i].ToString() + "';";
                            sql += "UPDATE barcode SET barcode='" + htNewBarcode[i].ToString() + "' WHERE itemCode='" + txtItemCode.Text.Trim() + "' AND barcode='" + htBarcodeFromDB[i].ToString() + "';";
                        }
                    }

                    //we need this because when txtUnitPrice did not fire new price is not updated.
                    htNewPrice[(int)priceList(cboPriceList.Text)] = txtUnitPrice.Text.Trim();

                    //compare new price and previews price
                    rowCount = (Int16)htNewPrice.Count;
                    for (i = 0; i < rowCount; i++)
                    {
                        if (Convert.ToDecimal(htNewPrice[i]) != Convert.ToDecimal(htPriceFromDB[i]))
                        {
                            sql += "INSERT INTO pricelisthistory(itemCode,priceListCode,netPrice,cp_createDate,cp_createdBy,createDate,createdBy) SELECT itemCode,priceListCode,netPrice,createDate,createdBy,@date,@user_id FROM pricelist WHERE itemCode='" + txtItemCode.Text.Trim() + "' AND priceListCode=" + i + ";";
                            sql += "UPDATE pricelist SET netPrice=" + Convert.ToDecimal(htNewPrice[i]) + " WHERE itemCode='" + txtItemCode.Text.Trim() + "' AND priceListCode=" + i + ";";
                        }
                    }
                    
                    sql += "COMMIT;"; //Finally make it permanent

                    if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                    {
                        //copy values into htFromDB
                        rowCount = (Int16)htNewPrice.Count;
                        for (i = 0; i < rowCount; i++)
                            htPriceFromDB[i] = htNewPrice[i];

                        rowCount = (Int16)htNewBarcode.Count;
                        for (i = 0; i < rowCount; i++)
                            htBarcodeFromDB[i] = htNewBarcode[i];

                        btnFind.Text = "&OK";
                        dgvBarcode.Refresh();
                        MessageBox.Show(this, "Updating has been successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else if (btnFind.Text == "&Save")
            {
                if (checkValues())
                {
                    if (MessageBox.Show(this, "Are you sure you want to save this?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    Hashtable items = new Hashtable();
                    items.Add("vatable", chkVatable.Checked == true ? "Y" : "N");
                    items.Add("qtyPrPrchsUoM", txtQtyPrPrchsUoM.Text.Trim());
                    items.Add("qtyPrSaleUoM", txtQtyPrSalesUoM.Text.Trim());
                    items.Add("prchsUoM", txtPurchaseUoM.Text.Trim());
                    items.Add("saleUoM", txtSaleUoM.Text.Trim());
                    items.Add("varWeightItm", chkVarWeightItm.Checked == true ? "Y" : "N");
                    items.Add("minStock", txtMinStock.Text.Trim()); // zero means no minimum
                    items.Add("maxStock", txtMaxStock.Text.Trim()); // zero means no maximum
                    items.Add("createdBy", vars.user_id);
                    items.Add("description", txtDescription.Text.Trim());
                    items.Add("shortName", txtShortName.Text.Trim());
                    items.Add("vendor", txtVendor.Text.Trim());
                    items.Add("remarks", txtRemarks.Text.Trim());
                    items.Add("deactivated", (chkDeactivate.Checked == true) ? "Y" : "N");

                    ArrayList barcodes = new ArrayList();
                    rowCount = Convert.ToInt16(dgvBarcode.Rows.Count);
                    for (i = 0; i < rowCount; i++)
                        if (!dgvBarcode.Rows[i].IsNewRow)
                            if (dgvBarcode.Rows[i].Cells[0].Value.ToString().Trim() != "")
                                barcodes.Add(dgvBarcode.Rows[i].Cells[0].Value.ToString());


                    /*
                    sql = "START TRANSACTION;";
                    sql += "SET @date=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s');";
                    sql += "SET @user_id=" + vars.user_id + ";";
                    sql += "SET @varWeightItm='" + varWeightItem + "';";
                    sql += "SET @newId=(SELECT CAST(lastNo+1 AS char(11)) FROM documents WHERE documentCode='IMD');";
                    sql += "SET @itemCode=CONCAT('" + vars.terminalId + "', 'ITM', @newId);";
                    sql += "SET @vendor=(SELECT code FROM businesspartner WHERE code='" + txtVendor.Text.Trim() + "' AND BPType=0 AND deactivated='N');";
                    sql += "INSERT INTO itemmasterdata(itemCode";
                    if (txtDescription.Text.Trim() != "")
                        sql += ",description";
                    if (txtShortName.Text.Trim() != "")
                        sql += ",shortName";
                    
                    sql += ",vatable,vendor,deactivated,qtyPrPrchsUoM,qtyPrSaleUoM,prchsUoM,saleUoM,varWeightItm";

                    if (txtRemarks.Text.Trim() != "")
                        sql += ",remarks";

                    sql += ",minStock,maxStock";
                    
                    sql += ",createDate,createdBy)";
                    sql += " VALUES(@itemCode";
                    if (txtDescription.Text.Trim() != "")
                        sql +=",'" + txtDescription.Text.Trim().Replace("'", "''") + "'";
                    if (txtShortName.Text.Trim() != "")
                        sql += ",'" + txtShortName.Text.Trim().Replace("'","''") + "'";

                    sql += ",'" + v + "',@vendor,'" + deactivate + "'," + txtQtyPrPrchsUoM.Text.Trim() + "," + txtQtyPrSalesUoM.Text.Trim() + ",'" + txtPurchaseUoM.Text.Trim() + "','" + txtSaleUoM.Text.Trim() + "',@varWeightItm";
                    if (txtRemarks.Text.Trim() != "")
                        sql += ",'" + txtRemarks.Text.Trim().Replace("'", "''") + "'";

                    sql += "," + txtMinStock.Text.Trim() + "," + txtMaxStock.Text.Trim();
                    sql += ",@date,@user_id);";
                    
                    Int16 priceListCode; double p;
                    foreach(string item in cboPriceList.Items)
                    {
                        priceListCode = priceList(item);
                        p = Convert.ToDouble(htNewPrice[(int)priceListCode]);
                        sql += "INSERT INTO pricelist(itemCode,priceListCode,netPrice,createdBy) VALUES(@itemCode," + priceListCode + "," + p + "," + vars.user_id + ");";
                    }

                    rowCount = Convert.ToInt16(dgvBarcode.Rows.Count);
                    for (i = 0; i < rowCount; i++)
                    {
                        if (!dgvBarcode.Rows[i].IsNewRow)
                        {
                            if (dgvBarcode.Rows[i].Cells[0].Value.ToString().Trim() != "")
                                sql += "INSERT INTO barcode(itemCode,barcode,createDate,createdBy) VALUES(@itemCode,'" + dgvBarcode.Rows[i].Cells[0].Value.ToString() + "',@date,@user_id);";
                        }                        
                    }
                    sql += "UPDATE documents SET lastNo=CAST(@newId AS UNSIGNED) WHERE documentCode='IMD';";
                    sql += " COMMIT;"; //Finally commit it

                    database db = new database();
                    if (db.executeNonQuery(sql,vars.MySqlConnection) > 0)
                    {
                        MessageBox.Show(this, "Saving has been successful!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnFind.Text = "&Find";
                        cleanUpUI();
                    }
                     */

                    ItemMasterData itemmasterdata = new ItemMasterData();
                    if (itemmasterdata.addItem(items, htNewPrice, barcodes))
                    {
                        MessageBox.Show(this, "Saving has been successful!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnFind.Text = "&Find";
                        cleanUpUI();
                    }

                }
            }
        }

        private Int16 priceList(string s)
        {
            if (s == "Purchase price")
                return 0;
            else if (s == "Retail price")
                return 1;
            else
            {
                //MessageBox.Show(this, "Unrecognize price.", "Error price code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                MessageBox.Show(this, "Unrecognize price code.", "Error price code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return "";
            }
        }

        private void registerControls()
        {
            //let's register all controls and DB column names. If new control is added, probably it has to be added here.
            table = new DataTable();
            table.Columns.Add("controls", typeof(string));     //check txt* name
            table.Columns.Add("DBcolumnName", typeof(string)); //check db column name
            table.Columns.Add("Value", typeof(string));        //value of txt*.text

            table.Rows.Add("txtItemCode", "itemCode", "");
            table.Rows.Add("txtDescription", "description", "");
            table.Rows.Add("txtShortName", "shortName", "");
        }

        /*
         * this function checks the values to all possible scenarios in each field whether it has to be allowed or not.
         */
        private bool checkValues()
        {
            string strValue = "";

            if (txtVendor.Text.Contains("'") || txtItemCode.Text.Contains('"'))
            {
                MessageBox.Show(this, "Invalid character as vendor code.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtVendor.Focus();
                return false;
            }

            if (txtQtyPrPrchsUoM.Text.Trim() == "")
            {
                MessageBox.Show(this, "Qty per purchase UoM cannot be empty.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtQtyPrPrchsUoM.Focus();
                return false;
            }
            else
            {
                strValue = txtQtyPrPrchsUoM.Text.Trim();
                if ( ! Decimal.TryParse(strValue, out d))
                {
                    MessageBox.Show(this, "Qty per purchase UoM should be numeric.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtQtyPrPrchsUoM.Focus();
                    return false;
                }
            }
            
            if (txtQtyPrSalesUoM.Text.Trim() == "")
            {
                MessageBox.Show(this, "Qty per sales UoM cannot be empty.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtQtyPrSalesUoM.Focus();
                return false;
            }
            else
            {
                strValue = txtQtyPrSalesUoM.Text.Trim();
                if (!Decimal.TryParse(strValue, out d))
                {
                    MessageBox.Show(this, "Qty per sale UoM should be numeric.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtQtyPrSalesUoM.Focus();
                    return false;
                }
            }

            if (txtPurchaseUoM.Text.Trim() == "")
            {
                MessageBox.Show(this, "Purchase UoM cannot be empty.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPurchaseUoM.Focus();
                return false;
            }

            if (txtSaleUoM.Text.Trim() == "")
            {
                MessageBox.Show(this, "Sales UoM cannot be empty.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSaleUoM.Focus();
                return false;
            }

            strValue = txtMinStock.Text.Trim();
            if (strValue == "")
            {
                MessageBox.Show(this, "Minimum stock cannot be empty.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tabControl1.SelectedIndex = 1;
                txtMinStock.Focus();
                return false;
            }
            else
            {
                if ( ! Decimal.TryParse(strValue, out d))
                {
                    MessageBox.Show(this, "Value in minimum stock field should be numeric.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tabControl1.SelectedIndex = 1;
                    txtMinStock.Focus();
                    return false;
                }
            }

            strValue = txtMaxStock.Text.Trim();
            if (strValue == "")
            {
                MessageBox.Show(this, "Maximum stock cannot be empty.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tabControl1.SelectedIndex = 1;
                txtMaxStock.Focus();
                return false;
            }
            else
            {
                if (!Decimal.TryParse(strValue, out d))
                {
                    MessageBox.Show(this, "Value in maximum stock field should be numeric.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tabControl1.SelectedIndex = 1;
                    txtMaxStock.Focus();
                    return false;
                }
            }

            if ( ! Decimal.TryParse(txtQtyPrPrchsUoM.Text.Trim(), out d))
            {
                MessageBox.Show(this, "Qty per purchase UoM should be in numbers.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tabControl1.SelectedIndex = 1;
                txtQtyPrPrchsUoM.Focus();
                return false;
            }
            if ( ! Decimal.TryParse(txtQtyPrSalesUoM.Text.Trim(), out d))
            {
                MessageBox.Show(this, "Qty per sales UoM should be in numbers.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tabControl1.SelectedIndex = 1;
                txtQtyPrSalesUoM.Focus();
                return false;
            }

            // check for duplicate barcodes
            rowCount = (Int16)dgvBarcode.Rows.Count;
            Hashtable htBarcode = new Hashtable();
            for (i = 0; i < rowCount; i++)
            {
                if (!dgvBarcode.Rows[i].IsNewRow && ((string)dgvBarcode.Rows[i].Cells[0].Value != ""))
                {
                    if (htBarcode.Count == 0)
                        htBarcode.Add(i, (string)dgvBarcode.Rows[i].Cells[0].Value);
                    else
                    {
                        if (htBarcode.ContainsValue((string)dgvBarcode.Rows[i].Cells[0].Value))
                        {
                            MessageBox.Show(this, "Duplicate barcodes found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); ;
                            return false;
                        }
                        else
                            htBarcode.Add(i, (string)dgvBarcode.Rows[i].Cells[0].Value);
                    }
                }
            }

            // we will allow no vendor however if no corresponding value in business partners it should not allow.
            if (txtVendor.Text.Trim() != "")
            {
                sql = "SET @code='" + txtVendor.Text.Trim() + "';";
                sql += "SELECT code FROM businesspartner WHERE code=@code;";
                database db = new database(); DataTable dt = new DataTable();
                dt = db.select(sql, vars.MySqlConnection);
                if (dt.Rows.Count < 1)
                {
                    MessageBox.Show(this, "Vendor code is not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private void editMode()
        {
            if (vars.role == 0) //if Superuser
                btnFind.Text = "&Update";
        }

        private void cleanUpUI()
        {
            txtVendor.ReadOnly = false;
            txtItemCode.Text = ""; txtDescription.Text = ""; txtShortName.Text = ""; txtVendor.Text = "";
            txtUnitPrice.Text = ""; txtRemarks.Text = "";

            txtPurchaseUoM.Text = "PC";
            txtSaleUoM.Text = "PC";
            txtQtyPrPrchsUoM.Text = "1";
            txtQtyPrSalesUoM.Text = "1";
            txtMinStock.Text = "0";
            txtMaxStock.Text = "0";

            dgvBarcode.Rows.Clear(); dgvInventory.Rows.Clear();
            cboPriceList.SelectedIndex = -1; chkVatable.Checked = false; chkDeactivate.Checked = false; chkVarWeightItm.Checked = false;
            dgvBarcode.Rows.Clear(); dgvInventory.Rows.Clear(); linkVendor.Enabled = false;

            txtPurchaseUoM.ReadOnly = false;
            txtSaleUoM.ReadOnly = false;
            txtQtyPrPrchsUoM.ReadOnly = false;
            txtQtyPrSalesUoM.ReadOnly = false;

            txtItemCode.ReadOnly = false; txtItemCode.Focus();
            
            tabControl1.SelectedIndex = 0;

            htNewPrice.Clear();
            htNewBarcode.Clear();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFind.Text = "&Save";
            dgvBarcode.Rows.Clear();
            dgvBarcode.AllowUserToAddRows = true;
            dgvInventory.Rows.Clear();
            cleanUpUI(); // call this before linkVendor.Enabled = true
            txtItemCode.ReadOnly = true;
            linkVendor.Enabled = true;
            htPriceFromDB = new Hashtable();
            tabControl1.SelectedIndex = 1;
            txtDescription.Focus();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFind.Text = "&Update";
            dgvBarcode.AllowUserToAddRows = true;
            //we cannot edit the vendor if transaction already accurs.
            if (trans == true)
                linkVendor.Enabled = false;

            btnFind.Focus();
        }

        private void frmItemMasterData_Load(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (vars.role > 0) //0=admin.
            {
                addToolStripMenuItem.Enabled = false;
                editToolStripMenuItem.Enabled = false;
            }

            if (btnFind.Text == "&Find")
            {
                addToolStripMenuItem.Enabled = true;
                editToolStripMenuItem.Enabled = false;
            }
            else if (btnFind.Text == "&Save" || btnFind.Text == "&Update")
            {
                addToolStripMenuItem.Enabled = false;
                editToolStripMenuItem.Enabled = false;
            }
            else // == "&OK"
            {
                addToolStripMenuItem.Enabled = true;
                editToolStripMenuItem.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnFind.Text != "&Find")
            {
                btnFind.Text = "&Find"; //call this before cleanUpUI because of cboPriceList_SelectedIndexChanged()
                cleanUpUI();
                htPriceFromDB.Clear();
                htNewPrice.Clear();
            }
            else
                Close();
        }

        private void linkVendor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            sql = "SELECT code,BPName FROM businesspartner WHERE BPType=0 AND deactivated='N' AND code like '" + txtVendor.Text.Replace("*", "%").Trim() + "' ORDER BY BPName";
            frmDialogForm = new frmDialog();
            frmDialogForm.selectedValue = sql;
            frmDialogForm.ShowDialog();
            txtVendor.Text = frmDialogForm.selectedValue;
        }

        private void dgvBarcode_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
