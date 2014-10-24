using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;

namespace Check_up.forms
{
    public partial class frmInventoryPosting : Form
    {
        DataTable table;
        DataTable dt;
        database db = new database();
        private frmDialog frmDialogForm;
        private frmCrystalReportViewer crystalReportViewerForm;
        string sql;
        int i, rowCount, prevWhouseSelectedIndex = 0;

        public frmInventoryPosting()
        {
            InitializeComponent();
            dgvItems.CellEndEdit += new DataGridViewCellEventHandler(dgvItems_CellEndEdit);

            db = new database(); dt = new DataTable();
            dt = db.select("SELECT code from warehouse WHERE deactivated='N'", vars.MySqlConnection);
            foreach (DataRow row in dt.Rows)
            {
                cboWarehouse.Items.Add(dt.Rows[i][0].ToString());
                i++;
            }
        }

        private void resetCellValues(int rowIndex)
        {
            dgvItems.Rows[rowIndex].Cells["colCurrentQty"].Value = "";
            dgvItems.Rows[rowIndex].Cells["colCountedQty"].Value = "";
            dgvItems.Rows[rowIndex].Cells["colVariance"].Value = "";
        }

        private string getDbColumnName(string str)
        {
            if (str == "colItemCode")
                return "itemCode";
            else if (str == "colItemDescription")
                return "description";
            else
                return "";
        }

        private bool itemCodeIsFound(string itemCode)
        {
            sql = "SELECT itemCode FROM itemmasterData WHERE itemCode='" + itemCode + "' AND deactivated='N'";
            database db = new database();
            if (db.select(sql, vars.MySqlConnection).Rows.Count > 0)
                return true;
            else
                return false;
        }

        private Hashtable getInfo(string itemCode)
        {
            Hashtable ht = new Hashtable();
            sql = "SET @itemCode='" + itemCode + "';";
            sql += "SET @warehouse='" + cboWarehouse.Text + "';";
            sql += "SELECT itemCode,description,vatable,IFNULL(C.inStock , 0) inStock";
            sql += ",(SELECT netPrice FROM pricelist WHERE pricelist.itemCode = A.itemCode AND priceListCode = 0) AS prchsPrc";
            sql += ",(SELECT netPrice FROM pricelist WHERE pricelist.itemCode = A.itemCode AND priceListCode = 1) AS retailPrc";
            sql += " FROM itemmasterdata A LEFT JOIN";
            sql += " (";
            sql += "SELECT itemCode,B.inStock";
            sql += " FROM itemmasterdata INNER JOIN item_warehouse B USING(itemCode) WHERE deactivated='N' AND B.whCode=@warehouse AND itemCode=@itemCode";
            sql += ") AS C USING(itemCode)";
            sql += "WHERE deactivated='N' AND A.itemcode=@itemCode ;";

            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count == 1)
            {
                ht["itemCode"] = dt.Rows[0]["itemCode"].ToString();
                ht["description"] = dt.Rows[0]["description"].ToString();
                ht["vatable"] = dt.Rows[0]["vatable"].ToString();
                ht["inStock"] = dt.Rows[0]["inStock"].ToString();
                ht["prchsPrc"] = dt.Rows[0]["prchsPrc"].ToString();
                ht["retailPrc"] = dt.Rows[0]["retailPrc"].ToString();
                return ht;
            }
            else if (dt.Rows.Count > 1)
                return ht;
            else
                return ht;
        }

        private void populateDataGridRow(Hashtable ht, int rowIndex)
        {
            dgvItems.Rows[rowIndex].Cells["colItemDescription"].Value = ht["description"];
            dgvItems.Rows[rowIndex].Cells["colVatable"].Value = ht["vatable"];
            dgvItems.Rows[rowIndex].Cells["colCurrentQty"].Value = ht["inStock"];
            dgvItems.Rows[rowIndex].Cells["colPrchsPrc"].Value = ht["prchsPrc"];
            dgvItems.Rows[rowIndex].Cells["colRetailPrc"].Value = ht["retailPrc"];
        }

        private void computeItemCount()
        {
            rowCount = dgvItems.Rows.Count;
            int cnt = 0;
            for (i = 0; i < rowCount; i++)
            {
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["colItemCode"].Value) != "")
                    cnt++;
            }

            lblitemsCount.Text = cnt + " item(s) found.";
        }

        private void computeForCountedQty(int rowIndex)
        {
            if (itemCodeIsFound(fx.null2EmptyStr(dgvItems.Rows[rowIndex].Cells["colItemCode"].Value)))
            {
                decimal varCurrentQty, varCountedQty, varVariance;

                try
                {
                    varCurrentQty = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["colCurrentQty"].Value.ToString());
                    varCountedQty = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["colCountedQty"].Value.ToString());
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                varVariance = varCountedQty - varCurrentQty;
                dgvItems.Rows[rowIndex].Cells["colVariance"].Value = varVariance;
            }
        }

        void dgvItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (btnFind.Text == "&Save")
            {
                if (dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    string dbColumnName = getDbColumnName(dgvItems.Columns[e.ColumnIndex].Name);
                    if (dbColumnName == "itemCode" || dbColumnName == "description")
                    {
                        sql = "SELECT itemCode,description FROM itemmasterdata WHERE deactivated='N'";
                        string returnedValue;

                        if (dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        {
                            if (dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Contains("*"))
                            {
                                sql += " AND " + dbColumnName + " LIKE '" + dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("*", "%") + "' ORDER BY " + dbColumnName;
                                frmDialogForm = new frmDialog();
                                frmDialogForm.selectedValue = sql;
                                frmDialogForm.ShowDialog();
                                returnedValue = frmDialogForm.selectedValue;

                                if (returnedValue != "")
                                {
                                    dgvItems.Rows[e.RowIndex].Cells["colItemCode"].Value = returnedValue;

                                    Hashtable ht = new Hashtable(getInfo(returnedValue));
                                    if (ht != null)
                                        populateDataGridRow(ht, e.RowIndex);
                                }
                            }
                            else //doesn't contain asterisk
                            {
                                sql += " AND " + dbColumnName + "='" + dgvItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() + "' ORDER BY " + dbColumnName;

                                db = new database();
                                dt = new DataTable();
                                dt = db.select(sql, vars.MySqlConnection);
                                if (dt.Rows.Count > 1)
                                {
                                    frmDialogForm = new frmDialog();
                                    frmDialogForm.selectedValue = sql;
                                    frmDialogForm.ShowDialog();
                                    returnedValue = frmDialogForm.selectedValue;
                                    if (returnedValue != "")
                                    {
                                        dgvItems.Rows[e.RowIndex].Cells["itemCode"].Value = returnedValue;

                                        Hashtable ht = new Hashtable(getInfo(returnedValue));
                                        if (ht != null)
                                        {
                                            resetCellValues(e.RowIndex);
                                            dgvItems.Rows[e.RowIndex].Cells["colItemDescription"].Value = ht["description"];
                                            populateDataGridRow(ht, e.RowIndex);
                                        }
                                    }
                                }
                                else if (dt.Rows.Count == 1)
                                {
                                    dgvItems.Rows[e.RowIndex].Cells["colItemCode"].Value = dt.Rows[0]["itemCode"].ToString();

                                    Hashtable ht = new Hashtable(getInfo(dt.Rows[0]["itemCode"].ToString()));
                                    if (ht != null)
                                    {
                                        resetCellValues(e.RowIndex);
                                        populateDataGridRow(ht, e.RowIndex);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(this, "No matching record found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    resetCellValues(e.RowIndex);
                                }
                            }
                        }
                    }

                    //when column colCountedQty is modified
                    if (dgvItems.Columns[e.ColumnIndex].Name == "colCountedQty")
                        computeForCountedQty(e.RowIndex);

                }

                //every edit of the cell must compute other values
                computeItemCount();
            }
        }

        private void setCntrlsToDefaultMode()
        {
            cleanUpUI();
            btnFind.Text = "&Find";
            btnPrint.Enabled = false;
            txtInvPostingNo.Focus();
        }

        private void setCntrlToAfterSaveMode()
        {
            txtPostingDate.ReadOnly = true;
            dateTimePicker1.Enabled = false;
            txtCountDateTime.ReadOnly = true;
            dateTimePicker2.Enabled = false;
            cboWarehouse.Enabled = false;
            dgvItems.ReadOnly = true;
            dgvItems.AllowUserToAddRows = false;
            txtRemarks1.Enabled = false;
            btnFind.Text = "&OK";
            btnPrint.Enabled = true;
        }

        private void setCntrlsToOKMode()
        {
            txtInvPostingNo.ReadOnly = true;
            txtPostingDate.ReadOnly = true;
            dateTimePicker1.Enabled = false;
            txtCountDateTime.ReadOnly = true;
            cboWarehouse.Enabled = false;
            dateTimePicker2.Enabled = false;
            txtRemarks1.ReadOnly = true;
            dgvItems.ReadOnly = true;
            dateTimePicker1.Enabled = false;
            btnFind.Text = "&OK";
            btnPrint.Enabled = true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (btnFind.Text == "&Find")
            {
                sql = "SELECT docId,postingDate,countDateTime,warehouse,remarks1 FROM inventoryposting WHERE ";

                if (txtInvPostingNo.Text.Trim() != "" && !txtInvPostingNo.Text.Contains("*"))
                {
                    sql += "docId='" + txtInvPostingNo.Text.Trim().Replace("'", "''") + "'";
                    db = new database();
                    dt = new DataTable();
                    dt = db.select(sql, vars.MySqlConnection);
                    if (dt.Rows.Count < 1)
                    {
                        MessageBox.Show(this, "No matching record found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    txtInvPostingNo.Text = dt.Rows[0]["docId"].ToString();
                    txtPostingDate.Text = dt.Rows[0]["postingDate"].ToString();
                    txtCountDateTime.Text = dt.Rows[0]["countDateTime"].ToString();
                    cboWarehouse.Text = dt.Rows[0]["warehouse"].ToString();
                    txtRemarks1.Text = dt.Rows[0]["remarks1"].ToString();

                    sql = "SELECT * FROM inventoryposting_item WHERE docId='" + txtInvPostingNo.Text.Trim() + "' ORDER BY indx";
                    db = new database();
                    dt = db.select(sql, vars.MySqlConnection);
                    rowCount = dt.Rows.Count;

                    decimal d;

                    for (i = 0; i < rowCount; i++)
                    {
                        dgvItems.Rows.Add();
                        dgvItems.Rows[i].Cells["colItemCode"].Value = dt.Rows[i]["itemCode"].ToString();
                        dgvItems.Rows[i].Cells["colItemDescription"].Value = dt.Rows[i]["description"].ToString();
                        dgvItems.Rows[i].Cells["colVatable"].Value = dt.Rows[i]["vatable"].ToString();
                        dgvItems.Rows[i].Cells["colCurrentQty"].Value = Decimal.Parse(dt.Rows[i]["currentQty"].ToString()).ToString(vars.format);

                        d = Decimal.Parse(dt.Rows[i]["countedQty"].ToString());
                        dgvItems.Rows[i].Cells["colCountedQty"].Value = d.ToString(vars.format);

                        dgvItems.Rows[i].Cells["colVariance"].Value = Decimal.Parse(dt.Rows[i]["varianceQty"].ToString()).ToString(vars.format);

                        d = Decimal.Parse(dt.Rows[i]["prchsPrc"].ToString());
                        dgvItems.Rows[i].Cells["colPrchsPrc"].Value = d.ToString(vars.format);

                        d = Decimal.Parse(dt.Rows[i]["retailPrc"].ToString());
                        dgvItems.Rows[i].Cells["colRetailPrc"].Value = d.ToString(vars.format);
                    }

                    setCntrlsToOKMode();
                    computeItemCount();
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
                        txtInvPostingNo.Focus();
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
                    txtInvPostingNo.Text = frmDialogForm.selectedValue;
                }
            }
            else if (btnFind.Text == "&OK")
            {
                this.Close();
            }
            else if (btnFind.Text == "&Update")
            {
                db = new database();
                sql = "UPDATE grpo SET remarks1='" + txtRemarks1.Text.Trim().Replace("'", "''") + "',updateDate=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s'),updatedBy=" + vars.user_id + " WHERE docId='" + txtInvPostingNo.Text.Trim() + "'";

                if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                {
                    MessageBox.Show(this, "Updating has been successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnFind.Text = "&OK";
                }
            }
            else if (btnFind.Text == "&Save")
            {
                if (checkValues())
                {
                    if (MessageBox.Show(this, "Are you sure you want to save this?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    DateTime dateTime; string strPostingDate, strCountDateTime;

                    try
                    {
                        dateTime = DateTime.Parse(txtPostingDate.Text.Trim());
                        strPostingDate = dateTime.ToString("yyyy/MM/dd");

                        dateTime = DateTime.Parse(txtCountDateTime.Text.Trim());
                        strCountDateTime = dateTime.ToString("yyyy/MM/dd H:mm");
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(this, err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    sql = "START TRANSACTION;";
                    sql += "SET @date=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s');";
                    sql += "SET @postingDate=DATE_FORMAT('" + strPostingDate + "', '%Y-%m-%d');";
                    sql += "SET @user_id=" + vars.user_id + ";";
                    sql += "SET @newId=(SELECT CAST(lastNo+1 AS char(11)) FROM documents WHERE documentCode='IP');";
                    sql += "SET @docId=CONCAT('" + vars.terminalId + "', @newId);";
                    sql += "SET @countDateTime=DATE_FORMAT('" + strCountDateTime + "', '%Y-%m-%d %H:%i');";
                    sql += "SET @warehouse='" + cboWarehouse.Text + "';";
                    sql += "INSERT INTO inventoryPosting(docId,postingDate,countDateTime,warehouse";
                    if (txtRemarks1.Text.Trim() != "")
                        sql += ",remarks1";
                    sql += ",createDate,createdBy)";
                    sql += " VALUES(@docId,@postingDate,@countDateTime,@warehouse";
                    if (txtRemarks1.Text.Trim() != "")
                        sql += ",'" + txtRemarks1.Text.Trim() + "'";
                    sql += ",@date,@user_id);";

                    rowCount = dgvItems.Rows.Count;

                    for (i = 0; i < rowCount; i++)
                    {
                        if (!dgvItems.Rows[i].IsNewRow && dgvItems.Rows[i].Cells[0].Value.ToString() != "")
                        {
                            string varItemCode, varDescription, varVatable;
                            decimal varCurrentQty, varCountedQty, varVariance, varPrchsPrc, varRetailPrc;

                            varItemCode = dgvItems.Rows[i].Cells["colItemCode"].Value.ToString();
                            varDescription = dgvItems.Rows[i].Cells["colItemDescription"].Value.ToString();
                            varVatable = dgvItems.Rows[i].Cells["colVatable"].Value.ToString();
                            varCurrentQty = Decimal.Parse(dgvItems.Rows[i].Cells["colCurrentQty"].Value.ToString());
                            varCountedQty = Decimal.Parse(dgvItems.Rows[i].Cells["colCountedQty"].Value.ToString());
                            varVariance = Decimal.Parse(dgvItems.Rows[i].Cells["colVariance"].Value.ToString());
                            varPrchsPrc = Decimal.Parse(dgvItems.Rows[i].Cells["colPrchsPrc"].Value.ToString());
                            varRetailPrc = Decimal.Parse(dgvItems.Rows[i].Cells["colRetailPrc"].Value.ToString());

                            sql += "INSERT INTO inventoryPosting_item(docId,indx,itemCode,description,vatable,currentQty,countedQty,varianceQty,prchsPrc,retailPrc)";
                            sql += " VALUES(@docId," + i + ",'" + varItemCode + "','" + varDescription + "','" + varVatable + "'," + varCurrentQty + "," + varCountedQty + "," + varVariance + "," + varPrchsPrc + "," + varRetailPrc + ");";

                            sql += "UPDATE itemmasterdata SET trans='Y' WHERE itemCode='" + varItemCode + "';";
                            sql += "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES('" + varItemCode + "','" + cboWarehouse.Text + "'," + varVariance + ") ON DUPLICATE KEY UPDATE";
                            if (varVariance < 0)
                                sql += " inStock=ifnull(inStock, 0)-" + Math.Abs(varVariance) + ";";
                            else
                                sql += " inStock=ifnull(inStock, 0)+" + varVariance + ";";
                        }
                    }
                    sql += "UPDATE documents SET lastNo=CAST(@newId AS UNSIGNED) WHERE documentCode='IP';";
                    sql += "COMMIT;";

                    db = new database();
                    if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                    {
                        MessageBox.Show(this, "Saving has been successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        db = new database(); dt = new DataTable();
                        dt = db.select("SELECT docId FROM inventoryposting ORDER BY id DESC LIMIT 1", vars.MySqlConnection);
                        txtInvPostingNo.Text = dt.Rows[0][0].ToString();
                        setCntrlToAfterSaveMode();
                    }
                }
            }
        }

        private void registerControls()
        {
            //let's register all controls and DB column names. If new control is added, probably it has to be added here.
            table = new DataTable();
            table.Columns.Add("controls", typeof(string));     //check txt* name
            table.Columns.Add("DBcolumnName", typeof(string)); //check db column name
            table.Columns.Add("Value", typeof(string));        //value of txt*.text

            table.Rows.Add("txtInvPostingNo", "docId", "");
            table.Rows.Add("txtRemarks1", "remarks1", "");
        }

        private bool checkValues()
        {
            if (txtPostingDate.Text.Trim() == "")
            {
                MessageBox.Show(this, "No posting date.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPostingDate.Focus();
                return false;
            }
            if (btnFind.Text == "&Save" && (dgvItems.Rows.Count - 1) == 0)
            {
                MessageBox.Show(this, "No items to save.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dgvItems.Focus();
                return false;
            }
            DateTime dateTime;
            if (!DateTime.TryParse(txtPostingDate.Text.Trim(), out dateTime))
            {
                MessageBox.Show(this, "Invalid posting date.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPostingDate.Focus();
                return false;
            }
            if (!DateTime.TryParse(txtCountDateTime.Text.Trim(), out dateTime))
            {
                MessageBox.Show(this, "Invalid count date and/or time.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPostingDate.Focus();
                return false;
            }

            // count date should be less than the current date
            int i = DateTime.Compare(DateTime.Today, dateTime);
            if (i < 0)
            {
                MessageBox.Show(this, "Count date should be less than the current date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCountDateTime.Focus();
                return false;
            }


            if (cboWarehouse.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Please choose warehoouse", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboWarehouse.Focus();
                return false;
            }

            // reuseable variables
            decimal d; string itemCode;
            rowCount = dgvItems.Rows.Count;

            // check if all columns with numeric value is indeed numeric
            for (i = 0; i < rowCount; i++)
            {
                itemCode = fx.null2EmptyStr(dgvItems.Rows[i].Cells["colItemCode"].Value);
                if (itemCodeIsFound(itemCode))
                {
                    try
                    {
                        d = Decimal.Parse(dgvItems.Rows[i].Cells["colCurrentQty"].Value.ToString());
                        d = Decimal.Parse(dgvItems.Rows[i].Cells["colCountedQty"].Value.ToString());
                        d = Decimal.Parse(dgvItems.Rows[i].Cells["colVariance"].Value.ToString());
                        d = Decimal.Parse(dgvItems.Rows[i].Cells["colPrchsPrc"].Value.ToString());
                        d = Decimal.Parse(dgvItems.Rows[i].Cells["colRetailPrc"].Value.ToString());
                    }
                    catch
                    {
                        MessageBox.Show(this, "Invalid value. Should be in numeric format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            //check if there are double entries
            ArrayList itemCodeArray = new ArrayList();
            for (i = 0; i < rowCount; i++)
            {
                itemCode = fx.null2EmptyStr(dgvItems.Rows[i].Cells["colItemCode"].Value);
                if (itemCode != "")
                {
                    if (!itemCodeArray.Contains(itemCode))
                        itemCodeArray.Add(itemCode);
                    else
                    {
                        MessageBox.Show(this, "Duplicate entries found for item code " + itemCode, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                        return false;
                    }
                }
            }

            //check item master data properties
            rowCount = (dgvItems.Rows.Count - 1);
            for (i = 0; i < rowCount; i++)
            {
                itemCode = fx.null2EmptyStr(dgvItems.Rows[i].Cells["colItemCode"].Value);
                db = new database(); dt = new DataTable();
                sql = "SELECT deactivated FROM itemmasterdata WHERE itemCode='" + itemCode + "'";
                dt = db.select(sql, vars.MySqlConnection);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["deactivated"].ToString() == "Y")
                    {
                        MessageBox.Show(this, "Item code " + itemCode + " is deactivated. Cannot proceed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show(this, "Item code " + itemCode + " is not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                    return false;
                }
            }

            // Counted Qty should be positive
            for (i = 0; i < rowCount; i++)
            {
                itemCode = fx.null2EmptyStr(dgvItems.Rows[i].Cells["colItemCode"].Value);
                if (itemCodeIsFound(itemCode))
                {
                    d = Decimal.Parse(dgvItems.Rows[i].Cells["colCountedQty"].Value.ToString());
                    if (d < 0)
                    {
                        MessageBox.Show(this, "Counted Qty for " + itemCode + " should be postive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                        return false;
                    }
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
            txtInvPostingNo.Text = "";
            txtInvPostingNo.ReadOnly = false;
            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = true;
            txtPostingDate.Text = "";
            txtPostingDate.ReadOnly = false;
            txtCountDateTime.Text = "";
            txtCountDateTime.ReadOnly = false;
            cboWarehouse.Enabled = true;
            cboWarehouse.SelectedIndex = -1;
            txtRemarks1.ReadOnly = false;
            dgvItems.Rows.Clear();
            dgvItems.AllowUserToAddRows = false;
            txtRemarks1.Text = "";
            lblitemsCount.Text = "0 item(s) found.";
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            txtPostingDate.Text = dateTimePicker1.Value.ToString("MM/dd/yyyy");
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            txtCountDateTime.Text = dateTimePicker2.Value.ToString("MM/dd/yyyy hh:mm tt");
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFind.Text = "&Save";
            btnPrint.Enabled = false;
            cleanUpUI();
            txtPostingDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            dgvItems.ReadOnly = false;
            txtPostingDate.Focus();
            txtInvPostingNo.ReadOnly = true;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFind.Text = "&Update";
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
            if (btnFind.Text == "&Find")
                Close();
            else
            {
                if (btnFind.Text == "&OK")
                    setCntrlsToDefaultMode();
                else if (btnFind.Text == "&Save")
                    if (MessageBox.Show(this, "Are you sure you want to cancel this transaction?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        setCntrlsToDefaultMode();
                else if (btnFind.Text == "&Update")
                    setCntrlsToDefaultMode();
            }
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if (dgvItems.Rows.Count < 1)
            {
                removeRowToolStripMenuItem.Enabled = false;
                return;
            }
            else
                removeRowToolStripMenuItem.Enabled = true;

            if ((dgvItems.Rows.Count - 1) == 0 || btnFind.Text != "&Save")
            {
                removeRowToolStripMenuItem.Enabled = false;
                return;
            }
            else
                removeRowToolStripMenuItem.Enabled = true;

            if (dgvItems.CurrentRow.Index == (dgvItems.Rows.Count - 1))
            {
                removeRowToolStripMenuItem.Enabled = false;
                return;
            }
            else
                removeRowToolStripMenuItem.Enabled = true;
        }

        private void removeRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvItems.Rows.RemoveAt(dgvItems.CurrentRow.Index);
            computeItemCount();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            crystal_reports.inventoryPosting objReport;
            this.Cursor = Cursors.WaitCursor;

            sql = "SELECT docId,warehouse,DATE_FORMAT(postingDate, '%m-%d-%Y') postingDate, DATE_FORMAT(countDateTime, '%Y-%m-%d %H:%i') countDateTime,remarks1,itemCode,description,vatable,currentQty, countedQty, varianceQty FROM inventoryposting A JOIN inventoryposting_item B USING(docId) WHERE docId='" + txtInvPostingNo.Text.Trim() + "' ORDER BY indx;";
            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);

            objReport = new crystal_reports.inventoryPosting();
            objReport.SetDataSource(dt);

            crystalReportViewerForm = new frmCrystalReportViewer();
            crystalReportViewerForm.CrystalReportViewer1.ReportSource = objReport;

            crystalReportViewerForm.MdiParent = this.MdiParent;
            crystalReportViewerForm.Show();
            crystalReportViewerForm.Text = "Inventory Posting Report";
            this.Cursor = Cursors.Default;
        }

        private void cboWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWarehouse.SelectedIndex > -1)
                dgvItems.AllowUserToAddRows = true;
            
            if (prevWhouseSelectedIndex > -1 && cboWarehouse.SelectedIndex != prevWhouseSelectedIndex && (dgvItems.Rows.Count - 1) > 0 && btnFind.Text != "&OK")
            {
                DialogResult result = MessageBox.Show(this, "If you change the warehouse will delete all encoded items. Are you sure you want to do this?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    cboWarehouse.SelectedIndex = prevWhouseSelectedIndex;
                else
                    dgvItems.Rows.Clear();
            }
        }

    }
}
