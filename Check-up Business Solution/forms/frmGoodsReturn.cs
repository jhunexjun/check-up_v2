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
    public partial class frmGoodsReturn : Form
    {
        DataTable table;
        DataTable dt;
        database db = new database();
        private frmDialog frmDialogForm;
        private frmCrystalReportViewer crystalReportViewerForm;
        string sql;
        int i, rowCount;

        public frmGoodsReturn()
        {
            InitializeComponent();
            dgvItems.CellEndEdit += new DataGridViewCellEventHandler(dgvItems_CellEndEdit);
        }

        private void resetCellValues(int rowIndex)
        {
            dgvItems.Rows[rowIndex].Cells["netPrchsPrc"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["grossPrchsPrc"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["prcntDscnt"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["amtDscnt"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["rowNetTotal"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["rowGrossTotal"].Value = "0.00";
        }

        private string getDbColumnName(string str)
        {
            if (str == "itemCode")
                return "itemCode";
            else if (str == "description")
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
            sql = "SELECT itemCode,description,vatable,qtyPrPrchsUoM,netPrice realBsNetPrchsPrc";
            sql += " FROM itemmasterdata A JOIN pricelist B USING(itemCode)";
            sql += " WHERE itemCode='" + itemCode + "' AND pricelistCode=0 AND deactivated='N' AND vendor='" + txtVendorCode.Text.Trim() + "'";

            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count == 1)
            {
                ht["itemCode"] = dt.Rows[0]["itemCode"].ToString();
                ht["description"] = dt.Rows[0]["description"].ToString();
                ht["vatable"] = dt.Rows[0]["vatable"].ToString();
                ht["qtyPrPrchsUoM"] = dt.Rows[0]["qtyPrPrchsUoM"];
                ht["realBsNetPrchsPrc"] = dt.Rows[0]["realBsNetPrchsPrc"];
                return ht;
            }
            else if (dt.Rows.Count > 1)
                return ht;
            else
                return ht;
        }

        private void populateDataGridRow(Hashtable ht, int rowIndex)
        {
            dgvItems.Rows[rowIndex].Cells["itemCode"].Value = ht["itemCode"];
            dgvItems.Rows[rowIndex].Cells["description"].Value = ht["description"];
            dgvItems.Rows[rowIndex].Cells["vatable"].Value = ht["vatable"];
            dgvItems.Rows[rowIndex].Cells["qtyPrPrchsUoM"].Value = Decimal.Parse(ht["qtyPrPrchsUoM"].ToString()).ToString(vars.format);
            dgvItems.Rows[rowIndex].Cells["realBsNetPrchsPrc"].Value = Decimal.Parse(ht["realBsNetPrchsPrc"].ToString()).ToString(vars.format);
        }

        private void computeForBaseUoM(int rowIndex)
        {
            dgvItems.Rows[rowIndex].Cells["prcntDscnt"].Value = 0.00;
            dgvItems.Rows[rowIndex].Cells["amtDscnt"].Value = 0.00;
            computeForQtyCol(rowIndex);
        }

        private void computeForQtyCol(int rowIndex)
        {
            if (itemCodeIsFound(fx.null2EmptyStr(dgvItems.Rows[rowIndex].Cells["itemCode"].Value)))
            {
                string varVatable, varBaseUoM;
                decimal varQty, varRealBsNetPrchsPrc, varRealBsGrossPrchsPrc, varQtyPrPrchsUoM, varRealNetPrchsPrc, varRealGrossPrchsPrc, varGrossPrchsPrc, varRowNetTotal, varRowGrossTotal, varNetPrchsPrc, varPrcntDscnt, varAmtDscnt;

                try
                {
                    varQty = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["Qty"].Value.ToString());
                    varVatable = dgvItems.Rows[rowIndex].Cells["vatable"].Value.ToString();
                    varRealBsNetPrchsPrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["realBsNetPrchsPrc"].Value.ToString());
                    varQtyPrPrchsUoM = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["qtyPrPrchsUoM"].Value.ToString());
                    varBaseUoM = dgvItems.Rows[rowIndex].Cells["baseUoM"].Value.ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                varRealBsGrossPrchsPrc = varRealBsNetPrchsPrc * ((varVatable == "Y") ? 1.12m : 1);
                varRealNetPrchsPrc = varRealBsNetPrchsPrc * ((varBaseUoM == "Y") ? 1 : varQtyPrPrchsUoM);
                varRealGrossPrchsPrc = varRealNetPrchsPrc * ((varVatable == "Y") ? 1.12m : 1);
                varNetPrchsPrc = varRealNetPrchsPrc * varQty;
                varGrossPrchsPrc = varRealGrossPrchsPrc * varQty;
                varPrcntDscnt = 0;
                varAmtDscnt = 0;
                varRowGrossTotal = varGrossPrchsPrc;
                varRowNetTotal = varNetPrchsPrc;

                dgvItems.Rows[rowIndex].Cells["realBsGrossPrchsPrc"].Value = varRealBsGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["realNetPrchsPrc"].Value = varRealNetPrchsPrc.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["realGrossPrchsPrc"].Value = varRealGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["grossPrchsPrc"].Value = varGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["netPrchsPrc"].Value = varNetPrchsPrc.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["prcntDscnt"].Value = varPrcntDscnt.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["amtDscnt"].Value = varAmtDscnt.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["rowNetTotal"].Value = varRowNetTotal.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["rowGrossTotal"].Value = varRowGrossTotal.ToString(vars.grossFormat);
            }
        }

        private void computeForPrcntDscnt(int rowIndex)
        {
            if (itemCodeIsFound(fx.null2EmptyStr(dgvItems.Rows[rowIndex].Cells["itemCode"].Value)))
            {
                string varVatable, varBaseUoM;
                decimal varQty, varRealBsNetPrchsPrc, varRealBsGrossPrchsPrc, varQtyPrPrchsUoM, varRealNetPrchsPrc, varRealGrossPrchsPrc, varGrossPrchsPrc, varRowNetTotal, varRowGrossTotal, varNetPrchsPrc, varPrcntDscnt, varAmtDscnt;

                try
                {
                    varPrcntDscnt = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["prcntDscnt"].Value.ToString());
                    varQty = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["Qty"].Value.ToString());
                    varVatable = dgvItems.Rows[rowIndex].Cells["vatable"].Value.ToString();
                    varRealBsNetPrchsPrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["realBsNetPrchsPrc"].Value.ToString());
                    varQtyPrPrchsUoM = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["qtyPrPrchsUoM"].Value.ToString());
                    varBaseUoM = dgvItems.Rows[rowIndex].Cells["baseUoM"].Value.ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                varRealBsGrossPrchsPrc = varRealBsNetPrchsPrc * ((varVatable == "Y") ? 1.12m : 1);
                varRealNetPrchsPrc = varRealBsNetPrchsPrc * ((varBaseUoM == "Y") ? 1 : varQtyPrPrchsUoM);
                varRealGrossPrchsPrc = varRealNetPrchsPrc * ((varVatable == "Y") ? 1.12m : 1);
                varNetPrchsPrc = varRealNetPrchsPrc * varQty;
                varGrossPrchsPrc = varRealGrossPrchsPrc * varQty;
                varRowGrossTotal = varGrossPrchsPrc * (1 - (varPrcntDscnt / 100));
                varRowNetTotal = varRowGrossTotal / ((varVatable == "Y") ? 1.12m : 1);
                varAmtDscnt = varGrossPrchsPrc - varRowGrossTotal;

                dgvItems.Rows[rowIndex].Cells["realBsGrossPrchsPrc"].Value = varRealBsGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["realNetPrchsPrc"].Value = varRealNetPrchsPrc.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["realGrossPrchsPrc"].Value = varRealGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["grossPrchsPrc"].Value = varGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["netPrchsPrc"].Value = varNetPrchsPrc.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["amtDscnt"].Value = varAmtDscnt.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["rowNetTotal"].Value = varRowNetTotal.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["rowGrossTotal"].Value = varRowGrossTotal.ToString(vars.grossFormat);
            }
        }

        private void computeForAmtDscnt(int rowIndex)
        {
            if (itemCodeIsFound(fx.null2EmptyStr(dgvItems.Rows[rowIndex].Cells["itemCode"].Value)))
            {
                string varVatable, varBaseUoM;
                decimal varQty, varRealBsNetPrchsPrc, varRealBsGrossPrchsPrc, varQtyPrPrchsUoM, varRealNetPrchsPrc, varRealGrossPrchsPrc, varGrossPrchsPrc, varRowNetTotal, varRowGrossTotal, varNetPrchsPrc, varPrcntDscnt, varAmtDscnt;

                try
                {
                    varAmtDscnt = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["amtDscnt"].Value.ToString());
                    varQty = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["Qty"].Value.ToString());
                    varVatable = dgvItems.Rows[rowIndex].Cells["vatable"].Value.ToString();
                    varRealBsNetPrchsPrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["realBsNetPrchsPrc"].Value.ToString());
                    varQtyPrPrchsUoM = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["qtyPrPrchsUoM"].Value.ToString());
                    varBaseUoM = dgvItems.Rows[rowIndex].Cells["baseUoM"].Value.ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                varRealBsGrossPrchsPrc = varRealBsNetPrchsPrc * ((varVatable == "Y") ? 1.12m : 1);
                varRealNetPrchsPrc = varRealBsNetPrchsPrc * ((varBaseUoM == "Y") ? 1 : varQtyPrPrchsUoM);
                varRealGrossPrchsPrc = varRealNetPrchsPrc * ((varVatable == "Y") ? 1.12m : 1);
                varNetPrchsPrc = varRealNetPrchsPrc * varQty;
                varGrossPrchsPrc = varRealGrossPrchsPrc * varQty;
                varRowGrossTotal = varGrossPrchsPrc - varAmtDscnt;
                varRowNetTotal = (varVatable == "Y") ? (varRowGrossTotal / 1.12m) : varRowGrossTotal;
                varPrcntDscnt = (varAmtDscnt / varGrossPrchsPrc) * 100;

                dgvItems.Rows[rowIndex].Cells["realBsGrossPrchsPrc"].Value = varRealBsGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["realNetPrchsPrc"].Value = varRealNetPrchsPrc.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["realGrossPrchsPrc"].Value = varRealGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["grossPrchsPrc"].Value = varGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["netPrchsPrc"].Value = varNetPrchsPrc.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["prcntDscnt"].Value = varPrcntDscnt.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["rowNetTotal"].Value = varRowNetTotal.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["rowGrossTotal"].Value = varRowGrossTotal.ToString(vars.grossFormat);
            }
        }

        private void computeDocument()
        {
            computeDocAmtDscnt();
            computeDocNetTotal();
            computeDocGrossTotal();
            computeDocPrcntDscnt();
            computeItemCount();
        }
        private void computeDocPrcntDscnt()
        {
            decimal prcntDscnt = 0; int cnt = 0;
            rowCount = dgvItems.Rows.Count;
            for (i = 0; i < rowCount; i++)
            {
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value) != "")
                {
                    prcntDscnt = prcntDscnt + fx.null2Decimal(dgvItems.Rows[i].Cells["prcntDscnt"].Value);
                    cnt++;
                }
            }

            if (cnt == 0)
                txtTotalPrcntDscnt.Text = "0";
            else
                txtTotalPrcntDscnt.Text = (prcntDscnt / cnt).ToString(vars.format);
        }
        private void computeDocAmtDscnt()
        {
            decimal amtDscnt = 0.00m;
            rowCount = dgvItems.Rows.Count;
            for (i = 0; i < rowCount; i++)
            {
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value) != "")
                    amtDscnt = amtDscnt + fx.null2Decimal(dgvItems.Rows[i].Cells["amtDscnt"].Value);
            }

            txtTotalAmtDscnt.Text = amtDscnt.ToString(vars.format);
        }
        private void computeDocNetTotal()
        {
            decimal netTotal = 0.00m;
            rowCount = dgvItems.Rows.Count;
            for (i = 0; i < rowCount; i++)
            {
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value) != "")
                    netTotal = netTotal + fx.null2Decimal(dgvItems.Rows[i].Cells["rowNetTotal"].Value);
            }

            txtNetTotal.Text = netTotal.ToString(vars.format);
        }
        private void computeDocGrossTotal()
        {
            decimal grossTotal = 0.00m;
            rowCount = dgvItems.Rows.Count;
            for (i = 0; i < rowCount; i++)
            {
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value) != "")
                    grossTotal = grossTotal + fx.null2Decimal(dgvItems.Rows[i].Cells["rowGrossTotal"].Value);
            }

            txtGrossTotal.Text = grossTotal.ToString(vars.grossFormat);
        }
        private void computeItemCount()
        {
            rowCount = dgvItems.Rows.Count;
            int cnt = 0;
            for (i = 0; i < rowCount; i++)
            {
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value) != "")
                    cnt++;
            }

            lblitemsCount.Text = cnt + " item(s) found.";
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
                        sql = "SELECT itemCode,description,vatable,qtyPrPrchsUoM,netPrice netBscPrchsPrc";
                        sql += " FROM itemmasterdata A JOIN pricelist B USING(itemCode)";
                        sql += " WHERE pricelistCode=0 AND deactivated='N' AND vendor='" + txtVendorCode.Text.Trim() + "'";
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
                                    dgvItems.Rows[e.RowIndex].Cells["itemCode"].Value = returnedValue;

                                    Hashtable ht = new Hashtable(getInfo(returnedValue));
                                    if (ht != null)
                                    {
                                        dgvItems.Rows[e.RowIndex].Cells["Qty"].Value = "1"; //let's have an initial qty
                                        dgvItems.Rows[e.RowIndex].Cells["baseUoM"].Value = "N";
                                        populateDataGridRow(ht, e.RowIndex);
                                        computeForQtyCol(e.RowIndex);
                                    }
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
                                            dgvItems.Rows[e.RowIndex].Cells["Qty"].Value = "1"; //let's have an initial qty
                                            dgvItems.Rows[e.RowIndex].Cells["baseUoM"].Value = "N";
                                            populateDataGridRow(ht, e.RowIndex);
                                            computeForQtyCol(e.RowIndex);
                                        }
                                    }
                                }
                                else if (dt.Rows.Count == 1)
                                {
                                    dgvItems.Rows[e.RowIndex].Cells["itemCode"].Value = dt.Rows[0]["itemCode"].ToString();

                                    Hashtable ht = new Hashtable(getInfo(dt.Rows[0]["itemCode"].ToString()));
                                    if (ht != null)
                                    {
                                        resetCellValues(e.RowIndex);
                                        dgvItems.Rows[e.RowIndex].Cells["Qty"].Value = "1"; //let's have an initial qty
                                        dgvItems.Rows[e.RowIndex].Cells["baseUoM"].Value = "N";
                                        populateDataGridRow(ht, e.RowIndex);
                                        computeForQtyCol(e.RowIndex);
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

                    //when column Qty is modified
                    if (dgvItems.Columns[e.ColumnIndex].Name == "Qty")
                        computeForQtyCol(e.RowIndex);

                    //when prcntDscnt is modified
                    if (dgvItems.Columns[e.ColumnIndex].Name == "prcntDscnt")
                        computeForPrcntDscnt(e.RowIndex);

                    //when amtDscnt is modified
                    if (dgvItems.Columns[e.ColumnIndex].Name == "amtDscnt")
                        computeForAmtDscnt(e.RowIndex);

                    //when base UoM is modified
                    if (dgvItems.Columns[e.ColumnIndex].Name == "baseUoM")
                        computeForBaseUoM(e.RowIndex);
                }

                //every edit of the cell must compute other values
                computeDocAmtDscnt();
                computeDocNetTotal();
                computeDocGrossTotal();
                computeDocPrcntDscnt();
                computeItemCount();
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (btnFind.Text == "&Find")
            {
                sql = "SELECT docId,vendorCode,vendorName,warehouse,DATE_FORMAT(postingDate, '%m/%d/%Y') postingDate,totalPrcntDscnt,totalAmtDscnt,ROUND(netTotal,2) netTotal,ROUND(grossTotal,2) grossTotal,remarks1,remarks2 FROM goodsreturn WHERE ";

                if (txtGoodsReturnNo.Text.Trim() != "" && !txtGoodsReturnNo.Text.Contains("*"))
                {
                    sql += "docId='" + txtGoodsReturnNo.Text.Trim().Replace("'", "''") + "'";
                    db = new database();
                    dt = new DataTable();
                    dt = db.select(sql, vars.MySqlConnection);
                    if (dt.Rows.Count < 1)
                    {
                        MessageBox.Show(this, "No matching record found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    txtGoodsReturnNo.Text = dt.Rows[0]["docId"].ToString();
                    txtVendorCode.Text = dt.Rows[0]["vendorCode"].ToString();
                    txtVendorName.Text = dt.Rows[0]["vendorName"].ToString();
                    cboWarehouse.Text = dt.Rows[0]["warehouse"].ToString();
                    txtPostingDate.Text = dt.Rows[0]["postingDate"].ToString();
                    txtRemarks1.Text = dt.Rows[0]["remarks1"].ToString();
                    txtRemarks2.Text = dt.Rows[0]["remarks2"].ToString();

                    txtTotalPrcntDscnt.Text = Decimal.Parse(dt.Rows[0]["totalPrcntDscnt"].ToString()).ToString(vars.format);
                    txtTotalAmtDscnt.Text = Decimal.Parse(dt.Rows[0]["totalAmtDscnt"].ToString()).ToString(vars.format);
                    txtNetTotal.Text = Decimal.Parse(dt.Rows[0]["netTotal"].ToString()).ToString(vars.format);
                    txtGrossTotal.Text = Decimal.Parse(dt.Rows[0]["grossTotal"].ToString()).ToString(vars.format);

                    sql = "SELECT * FROM goodsreturn_item WHERE docId='" + txtGoodsReturnNo.Text.Trim() + "' ORDER BY indx";
                    db = new database();
                    dt = db.select(sql, vars.MySqlConnection);
                    rowCount = dt.Rows.Count;
                    for (i = 0; i < rowCount; i++)
                    {
                        dgvItems.Rows.Add();
                        dgvItems.Rows[i].Cells["itemCode"].Value = dt.Rows[i]["itemCode"].ToString();
                        dgvItems.Rows[i].Cells["description"].Value = dt.Rows[i]["description"].ToString();
                        dgvItems.Rows[i].Cells["warehouse"].Value = dt.Rows[i]["warehouse"].ToString();
                        dgvItems.Rows[i].Cells["vatable"].Value = dt.Rows[i]["vatable"].ToString();
                        dgvItems.Rows[i].Cells["realBsNetPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["realBsNetPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["Qty"].Value = Decimal.Parse(dt.Rows[i]["Qty"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["qtyPrPrchsUoM"].Value = Decimal.Parse(dt.Rows[i]["qtyPrPrchsUoM"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["baseUoM"].Value = dt.Rows[i]["baseUoM"].ToString();
                        dgvItems.Rows[i].Cells["realBsGrossPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["realBsGrossPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["realNetPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["realNetPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["realGrossPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["realGrossPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["prcntDscnt"].Value = Decimal.Parse(dt.Rows[i]["prcntDscnt"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["grossPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["grossPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["netPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["netPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["amtDscnt"].Value = Decimal.Parse(dt.Rows[i]["amtDscnt"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["rowNetTotal"].Value = Decimal.Parse(dt.Rows[i]["rowNetTotal"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["rowGrossTotal"].Value = Decimal.Parse(dt.Rows[i]["rowGrossTotal"].ToString()).ToString(vars.format);
                    }

                    setCntrlToOKState();
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
                    txtGoodsReturnNo.Text = frmDialogForm.selectedValue;
                }
            }
            else if (btnFind.Text == "&OK")
            {
                this.Close();
            }
            else if (btnFind.Text == "&Update")
            {
                db = new database();
                sql = "UPDATE goodsreturn SET remarks2='" + txtRemarks2.Text.Trim().Replace("'", "''") + "',updateDate=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s'),updatedBy=" + vars.user_id + " WHERE docId='" + txtGoodsReturnNo.Text.Trim() + "'";

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

                    decimal totalPrcntDscnt, totalAmtDscnt, netTotal, grossTotal;
                    totalPrcntDscnt = Decimal.Parse(txtTotalPrcntDscnt.Text);
                    totalAmtDscnt = Decimal.Parse(txtTotalAmtDscnt.Text);
                    netTotal = Decimal.Parse(txtNetTotal.Text);
                    grossTotal = Decimal.Parse(txtGrossTotal.Text);

                    DateTime dateTime = DateTime.Parse(txtPostingDate.Text.Trim());
                    string strDateTime = dateTime.ToString("yyyy/MM/dd");

                    sql = "START TRANSACTION;";
                    sql += "SET @date=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s');";
                    sql += "SET @postingDate=DATE_FORMAT('" + strDateTime + "', '%Y-%m-%d');";
                    sql += "SET @user_id=" + vars.user_id + ";";
                    sql += "SET @newId=(SELECT CAST(lastNo+1 AS char(11)) FROM documents WHERE documentCode='GR');";
                    sql += "SET @docId=CONCAT('" + vars.terminalId + "', @newId);";
                    sql += "SET @totalPrcntDscnt=" + totalPrcntDscnt + ";";
                    sql += "SET @totalAmtDscnt=" + totalAmtDscnt + ";";
                    sql += "SET @netTotal=" + netTotal + ";";
                    sql += "SET @grossTotal=" + grossTotal + ";";
                    sql += "INSERT INTO goodsreturn(docId,vendorCode,vendorName,warehouse,postingDate,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal";
                    if (txtRemarks1.Text.Trim() != "")
                        sql += ",remarks1";
                    if (txtRemarks2.Text.Trim() != "")
                        sql += ",remarks2";
                    sql += ",createDate,createdBy)";
                    sql += " VALUES(@docId,'" + txtVendorCode.Text.Trim().Replace("'", "''") + "','" + txtVendorName.Text.Trim().Replace("'", "''") + "','" + cboWarehouse.Text + "',@postingDate,@totalPrcntDscnt,@totalAmtDscnt,@netTotal,@grossTotal";
                    if (txtRemarks1.Text.Trim() != "")
                        sql += ",'" + txtRemarks1.Text.Trim() + "'";
                    if (txtRemarks2.Text.Trim() != "")
                        sql += ",'" + txtRemarks2.Text.Trim() + "'";
                    sql += ",@date,@user_id);";
                    sql += "UPDATE businesspartner SET trans='Y' WHERE code='" + txtVendorCode.Text.Trim().Replace("'", "''") + "';";

                    rowCount = dgvItems.Rows.Count;

                    for (i = 0; i < rowCount; i++)
                    {
                        if (!dgvItems.Rows[i].IsNewRow && dgvItems.Rows[i].Cells[0].Value.ToString() != "")
                        {
                            string varItemCode, varDescription, varVatable, varBaseUoM, varWhCode;
                            decimal varQty, varPrcntDscnt, varAmtDscnt, varNetPrchsPrc, varGrossPrchsPrc, varRowNetTotal, varRowGrossTotal, varQtyPrPrchsUoM, varRealNetPrchsPrc, varRealGrossPrchsPrc, varRealBsNetPrchsPrc, varRealBsGrossPrchsPrc, varBaseQty;

                            varItemCode = dgvItems.Rows[i].Cells["itemCode"].Value.ToString();
                            varDescription = dgvItems.Rows[i].Cells["description"].Value.ToString();
                            varWhCode = "-1"; // dgvItems.Rows[i].Cells["warehouse"].Value.ToString();
                            varQty = Decimal.Parse(dgvItems.Rows[i].Cells["Qty"].Value.ToString());
                            varVatable = dgvItems.Rows[i].Cells["vatable"].Value.ToString();
                            varQtyPrPrchsUoM = Decimal.Parse(dgvItems.Rows[i].Cells["qtyPrPrchsUoM"].Value.ToString());
                            varBaseUoM = fx.null2EmptyStr(dgvItems.Rows[i].Cells["baseUoM"].Value);
                            varRealBsNetPrchsPrc = Decimal.Parse(dgvItems.Rows[i].Cells["realBsNetPrchsPrc"].Value.ToString());
                            varRealBsGrossPrchsPrc = Decimal.Parse(dgvItems.Rows[i].Cells["realBsGrossPrchsPrc"].Value.ToString());
                            varRealNetPrchsPrc = Decimal.Parse(dgvItems.Rows[i].Cells["realNetPrchsPrc"].Value.ToString());
                            varRealGrossPrchsPrc = Decimal.Parse(dgvItems.Rows[i].Cells["realGrossPrchsPrc"].Value.ToString());
                            varPrcntDscnt = Decimal.Parse(dgvItems.Rows[i].Cells["prcntDscnt"].Value.ToString());
                            varGrossPrchsPrc = Decimal.Parse(dgvItems.Rows[i].Cells["grossPrchsPrc"].Value.ToString());
                            varNetPrchsPrc = Decimal.Parse(dgvItems.Rows[i].Cells["netPrchsPrc"].Value.ToString());
                            varAmtDscnt = Decimal.Parse(dgvItems.Rows[i].Cells["amtDscnt"].Value.ToString());
                            varRowNetTotal = Decimal.Parse(dgvItems.Rows[i].Cells["rowNetTotal"].Value.ToString());
                            varRowGrossTotal = Decimal.Parse(dgvItems.Rows[i].Cells["rowGrossTotal"].Value.ToString());

                            varBaseQty = ((varBaseUoM == "N") ? varQtyPrPrchsUoM * varQty : varQty);

                            // for the meantime we just deduct the qty from warehouse in the header so we just override it.
                            varWhCode = cboWarehouse.Text;

                            sql += "INSERT INTO goodsreturn_item(docId,indx,itemCode,description,warehouse,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal)";
                            sql += " VALUES(@docId," + i + ",'" + varItemCode + "','" + varDescription + "','" + varWhCode + "','" + varVatable + "'," + varRealBsNetPrchsPrc + "," + varRealBsGrossPrchsPrc + "," + varRealNetPrchsPrc + "," + varRealGrossPrchsPrc + "," + varQty + ",'" + varBaseUoM + "'," + varQtyPrPrchsUoM + "," + varPrcntDscnt + "," + varAmtDscnt + "," + varNetPrchsPrc + "," + varGrossPrchsPrc + "," + varRowNetTotal + "," + varRowGrossTotal + ");";

                            sql += "UPDATE itemmasterdata SET trans='Y' WHERE itemCode='" + varItemCode + "';";
                            sql += "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES('" + varItemCode + "','" + varWhCode + "'," + varBaseQty + ") ON DUPLICATE KEY UPDATE inStock=ifnull(inStock, 0)-" + varBaseQty + ";";
                        }
                    }
                    sql += "UPDATE documents SET lastNo=CAST(@newId AS UNSIGNED) WHERE documentCode='GR';";
                    sql += "COMMIT;";

                    db = new database();
                    if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                    {
                        MessageBox.Show(this, "Saving has been successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        db = new database(); dt = new DataTable();
                        dt = db.select("SELECT docId FROM goodsreturn ORDER BY id DESC LIMIT 1", vars.MySqlConnection);
                        txtGoodsReturnNo.Text = dt.Rows[0][0].ToString();
                        setCntrlToOKState();
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnFind.Text == "&Find")
                Close();
            else
            {
                if (btnFind.Text == "&OK")
                    setToDefaultState();
                else if (btnFind.Text == "&Save")
                {
                    if (MessageBox.Show(this, "Are you sure you want to cancel this transaction?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        setToDefaultState();
                }
                else if (btnFind.Text == "&Update")
                    setToDefaultState();
            }
        }

        private void setToDefaultState()
        {
            cleanUpUI();
            btnFind.Text = "&Find";
            btnPrint.Enabled = false;
            txtGoodsReturnNo.Focus();
        }

        private void setCntrlToOKState()
        {
            txtGoodsReturnNo.ReadOnly = true;
            txtPostingDate.ReadOnly = true;
            txtVendorCode.ReadOnly = true; txtVendorName.ReadOnly = true;
            cboWarehouse.Enabled = false;
            dateTimePicker1.Enabled = false;
            dgvItems.ReadOnly = true;
            dgvItems.AllowUserToAddRows = false;
            linkVendorCode.Enabled = false; linkVendorName.Enabled = false;
            txtRemarks1.ReadOnly = true;
            btnFind.Text = "&OK";
            btnPrint.Enabled = true;
        }

        private void registerControls()
        {
            //let's register all controls and DB column names. If new control is added, probably it has to be added here.
            table = new DataTable();
            table.Columns.Add("controls", typeof(string));     //check txt* name
            table.Columns.Add("DBcolumnName", typeof(string)); //check db column name
            table.Columns.Add("Value", typeof(string));        //value of txt*.text

            table.Rows.Add("txtGoodsReturnNo", "docId", "");
            table.Rows.Add("txtVendorCode", "vendorCode", "");
            table.Rows.Add("txtVendorName", "vendorName", "");
            table.Rows.Add("txtRemarks1", "remarks1", "");
            table.Rows.Add("txtRemarks2", "remarks2", "");
        }

        private bool checkValues()
        {
            if (txtVendorCode.Text.Trim() == "")
            {
                MessageBox.Show(this, "No vendor code.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtVendorCode.Focus();
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

            // reuseable variables
            decimal d; string itemCode;
            rowCount = dgvItems.Rows.Count;

            // check if all columns with numeric value is indeed numeric
            for (i = 0; i < rowCount; i++)
            {
                itemCode = fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value);
                if (itemCodeIsFound(itemCode))
                {
                    try
                    {
                        d = Decimal.Parse(dgvItems.Rows[i].Cells["amtDscnt"].Value.ToString());
                    }
                    catch
                    {
                        MessageBox.Show(this, "Invalid amount discount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            // check negative values
            for (i = 0; i < rowCount; i++)
            {
                itemCode = fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value);
                if (itemCodeIsFound(itemCode))
                {
                    if (Decimal.Parse(dgvItems.Rows[i].Cells["rowGrossTotal"].Value.ToString()) < 0)
                    {
                        MessageBox.Show(this, "Negative value found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dgvItems.CurrentCell = dgvItems.Rows[i].Cells["rowGrossTotal"];
                        return false;
                    }
                }
            }

            //check if there are double entries
            ArrayList itemCodeArray = new ArrayList();
            rowCount = dgvItems.Rows.Count;
            for (i = 0; i < rowCount; i++)
            {
                itemCode = fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value);
                if (itemCode != "")
                {
                    if (!itemCodeArray.Contains(itemCode))
                        itemCodeArray.Add(itemCode);
                    else
                    {
                        MessageBox.Show(this, "Duplicate entries found for item code " + itemCode, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            
            //check warehouse
            if (cboWarehouse.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Please select warehouse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboWarehouse.Focus();
                return false;
            }

            // BaseUoM should always come with value
            rowCount = dgvItems.Rows.Count;
            string varBaseUoM;
            for (i = 0; i < rowCount; i++)
            {
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value) != "")
                {
                    varBaseUoM = fx.null2EmptyStr(dgvItems.Rows[i].Cells["baseUoM"].Value);
                    if (varBaseUoM == "")
                    {
                        MessageBox.Show(this, "Please choose base UoM at row " + (i + 1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
            }

            // We do not allow no inventory or greater than the inventory
            rowCount = dgvItems.Rows.Count;
            decimal varBaseQty, varQty, varQtyPrPrchsUoM;
            for (i = 0; i < rowCount; i++)
            {
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value) != "")
                {
                    varQty = Decimal.Parse(dgvItems.Rows[i].Cells["Qty"].Value.ToString());
                    varBaseUoM = dgvItems.Rows[i].Cells["baseUoM"].Value.ToString();
                    varQtyPrPrchsUoM = Decimal.Parse(dgvItems.Rows[i].Cells["qtyPrPrchsUoM"].Value.ToString());
                    varBaseQty = varQty * ((varBaseUoM == "N") ? varQtyPrPrchsUoM : 1);

                    db = new database(); dt = new DataTable();
                    sql = "SET @varBaseQty=" + varBaseQty + ";";
                    sql += "SELECT itemCode FROM item_warehouse WHERE itemCode = '" + dgvItems.Rows[i].Cells["itemCode"].Value + "' AND whCode = '" + cboWarehouse.Text.Trim() + "' AND ifnull(inStock, 0) < @varBaseQty;";
                    dt = db.select(sql, vars.MySqlConnection);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show(this, "Insufficient inventory for item " + dgvItems.Rows[i].Cells["itemCode"].Value, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else
                    {
                        db = new database(); dt = new DataTable();
                        sql = "SELECT itemCode FROM item_warehouse WHERE itemCode = '" + dgvItems.Rows[i].Cells["itemCode"].Value + "' AND whCode = '" + cboWarehouse.Text.Trim() + "'";
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count < 1)
                        {
                            MessageBox.Show(this, "Insufficient inventory for item " + dgvItems.Rows[i].Cells["itemCode"].Value, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
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
            txtGoodsReturnNo.Text = "";
            dateTimePicker1.Enabled = true;
            txtVendorCode.Text = ""; txtVendorName.Text = "";
            txtPostingDate.Text = "";
            cboWarehouse.Enabled = true; cboWarehouse.SelectedIndex = -1;
            linkVendorCode.Enabled = false;
            linkVendorName.Enabled = false;
            txtGoodsReturnNo.ReadOnly = false;
            txtPostingDate.ReadOnly = false;
            txtVendorCode.ReadOnly = false;
            txtVendorName.ReadOnly = false;
            txtRemarks1.ReadOnly = false;
            dgvItems.Rows.Clear();
            txtRemarks1.Text = ""; txtRemarks2.Text = "";
            txtGoodsReturnNo.Focus();

            txtTotalPrcntDscnt.Text = "0.00";
            txtTotalAmtDscnt.Text = "0.00";
            txtNetTotal.Text = "0.00";
            txtGrossTotal.Text = "0.00";
            lblitemsCount.Text = "0 item(s) found.";
        }

        private void linkVendorCode_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtVendorName.Text = "";

            sql = "SELECT code,BPName FROM businesspartner WHERE BPType=0 AND deactivated='N' AND code";

            if (txtVendorCode.Text.Trim() != "" || txtVendorCode.Text.Contains("*"))
            {
                sql += " like '" + txtVendorCode.Text.Replace("*", "%").Trim() + "' ORDER BY code";
                frmDialogForm = new frmDialog();
                frmDialogForm.selectedValue = sql;
                frmDialogForm.ShowDialog();
                txtVendorCode.Text = frmDialogForm.selectedValue;

                if (txtVendorCode.Text.Trim() == "")
                    return;

                sql = "SELECT code,BPName FROM businesspartner WHERE BPType=0 AND deactivated='N' AND code='" + txtVendorCode.Text + "'";
                db = new database(); dt = new DataTable();
                dt = db.select(sql, vars.MySqlConnection);
                txtVendorName.Text = dt.Rows[0]["BPName"].ToString();
                return;
            }
            else
                sql += "='" + txtVendorCode.Text.Replace("'", "''").Trim() + "' ORDER BY code";

            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count < 1)
            {
                MessageBox.Show(this, "No matching record found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //we expect only 1 output
            txtVendorCode.Text = dt.Rows[0]["code"].ToString();
            txtVendorName.Text = dt.Rows[0]["BPName"].ToString();
            dgvItems.Focus();
        }

        private void linkVendorName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            sql = "SELECT code,BPName FROM businesspartner WHERE BPType=0 AND deactivated='N' AND BPName";

            if (txtVendorName.Text.Trim() != "" || txtVendorName.Text.Contains("*"))
            {
                sql += " like '" + txtVendorName.Text.Replace("*", "%").Trim() + "' ORDER BY code";
                frmDialogForm = new frmDialog();
                frmDialogForm.selectedValue = sql;
                frmDialogForm.ShowDialog();
                txtVendorCode.Text = frmDialogForm.selectedValue;

                if (txtVendorName.Text.Trim() == "")
                    return;

                sql = "SELECT BPName FROM businesspartner WHERE BPType=0 AND deactivated='N' AND code='" + txtVendorCode.Text + "'";
                db = new database(); dt = new DataTable();
                dt = db.select(sql, vars.MySqlConnection);
                txtVendorName.Text = dt.Rows[0][0].ToString();
                cboWarehouse.Focus();
            }
            else
            {
                sql += "='" + txtVendorName.Text.Replace("'", "''").Trim() + "' ORDER BY BPName";

                db = new database();
                DataTable dt = new DataTable();
                dt = db.select(sql, vars.MySqlConnection);
                if (dt.Rows.Count < 1)
                {
                    MessageBox.Show(this, "No matching record found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (dt.Rows.Count == 1)
                {
                    txtVendorCode.Text = dt.Rows[0]["code"].ToString();
                    txtVendorName.Text = dt.Rows[0]["BPName"].ToString();
                    cboWarehouse.Focus();
                }
                else
                {
                    frmDialogForm = new frmDialog();
                    frmDialogForm.selectedValue = sql;
                    frmDialogForm.ShowDialog();
                    txtVendorCode.Text = frmDialogForm.selectedValue;
                    db = new database();
                    dt = new DataTable();
                    dt = db.select(sql, vars.MySqlConnection);
                    txtVendorCode.Text = dt.Rows[0]["code"].ToString();
                    txtVendorName.Text = dt.Rows[0]["BPName"].ToString();
                    cboWarehouse.Focus();
                }
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFind.Text = "&Save";
            btnPrint.Enabled = false;
            cleanUpUI();
            txtPostingDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            dgvItems.ReadOnly = false;
            dgvItems.AllowUserToAddRows = true;
            linkVendorCode.Enabled = true;
            linkVendorName.Enabled = true;
            txtGoodsReturnNo.ReadOnly = true;
            txtVendorCode.Focus();
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

        private void frmGoodsReturn_Load(object sender, EventArgs e)
        {
            // if it's a main warehouse, they're allowed to create AR Invoice for different branches
            // header warehouse
            sql = "SELECT branchType FROM terminal";
            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows[0][0].ToString() == "Main")
                sql = "SELECT code FROM warehouse WHERE deactivated='N'";
            else
                sql = "SELECT whCode AS code FROM terminal A JOIN warehouse B ON A.whCode=B.code WHERE deactivated='N'";

            db = new database(); dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            foreach (DataRow row in dt.Rows)
                cboWarehouse.Items.Add(row["code"]);

            // row warehouse
            sql = "SELECT code FROM warehouse WHERE deactivated='N'";
            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            foreach (DataRow row in dt.Rows)
                warehouse.Items.Add(row["code"]);
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
            computeDocument();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            crystal_reports.goodsReturn objReport;
            this.Cursor = Cursors.WaitCursor;

            sql = "SELECT docId,vendorCode,vendorName,DATE_FORMAT(postingDate, '%m/%d/%Y') postingDate,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,remarks1,remarks2, B.* FROM goodsreturn A JOIN goodsreturn_item B USING(docId) WHERE docId='" + txtGoodsReturnNo.Text.Trim() + "' ORDER BY indx";
            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);

            objReport = new crystal_reports.goodsReturn();
            objReport.SetDataSource(dt);

            crystalReportViewerForm = new frmCrystalReportViewer();
            crystalReportViewerForm.CrystalReportViewer1.ReportSource = objReport;

            crystalReportViewerForm.MdiParent = this.MdiParent;
            crystalReportViewerForm.Show();
            crystalReportViewerForm.Text = "Goods Return Report";
            this.Cursor = Cursors.Default;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            txtPostingDate.Text = dateTimePicker1.Value.ToString("MM/dd/yyyy");
        }
    }
}
