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
    public partial class frmSalesReturn : Form
    {
        DataTable table;
        DataTable dt;
        database db = new database();
        private frmDialog frmDialogForm;
        private frmCrystalReportViewer crystalReportViewerForm;
        string sql;
        int i, rowCount;

        public frmSalesReturn()
        {
            InitializeComponent();
            dgvItems.CellEndEdit += new DataGridViewCellEventHandler(dgvItems_CellEndEdit);
        }

        private Hashtable getInfo(string itemCode)
        {
            Hashtable ht = new Hashtable();
            sql = "SELECT itemCode,description,saleUoM,qtyPrSaleUoM,vatable, netPrice netPrchsPrc";
            sql += ",(SELECT netPrice";
            sql += "    FROM itemmasterdata JOIN pricelist USING(itemCode)";
            sql += "    WHERE priceListCode=1 AND itemmasterdata.itemCode=A.itemCode) realBsNetSalePrc";
            sql += " FROM itemmasterdata A JOIN pricelist B USING(itemCode)";
            sql += " WHERE itemCode='" + itemCode + "' AND pricelistCode=0";

            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count == 1)
            {
                ht["itemCode"] = dt.Rows[0]["itemCode"].ToString();
                ht["description"] = dt.Rows[0]["description"].ToString();
                ht["vatable"] = dt.Rows[0]["vatable"].ToString();
                ht["netPrchsPrc"] = dt.Rows[0]["netPrchsPrc"];
                ht["saleUoM"] = dt.Rows[0]["saleUoM"].ToString();
                ht["qtyPrSaleUoM"] = dt.Rows[0]["qtyPrSaleUoM"];
                ht["realBsNetSalePrc"] = dt.Rows[0]["realBsNetSalePrc"];
                return ht;
            }
            else if (dt.Rows.Count > 1)
                return ht;
            else
                return ht;
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

        private void computeForBaseUoM(int rowIndex)
        {
            dgvItems.Rows[rowIndex].Cells["prcntDscnt"].Value = 0.00;
            dgvItems.Rows[rowIndex].Cells["amtDscnt"].Value = 0.00;
            computeForQty(rowIndex);
        }

        private void computeForQty(int rowIndex)
        {
            if (itemCodeIsFound(fx.null2EmptyStr(dgvItems.Rows[rowIndex].Cells["itemCode"].Value)))
            {
                string varVatable, varSaleUoM, varBsUoM;
                decimal varQty, varRealBsNetSalePrc, varRealBsGrossSalePrc, varNetPrchsPrc, varGrossPrchsPrc, varQtyPrSaleUoM, varNetSalePrice, varGrossSalePrice, varRowNetTotal, varRowGrossTotal;

                try
                {
                    varQty = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["Qty"].Value.ToString());
                    if (varQty <= 0)
                    {
                        MessageBox.Show(this, "Cannot contain less than or equal to zero value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    varBsUoM = dgvItems.Rows[rowIndex].Cells["baseUoM"].Value.ToString();
                    varVatable = dgvItems.Rows[rowIndex].Cells["vatable"].Value.ToString();
                    varNetPrchsPrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["netPrchsPrc"].Value.ToString());
                    varSaleUoM = dgvItems.Rows[rowIndex].Cells["saleUoM"].Value.ToString();
                    varQtyPrSaleUoM = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["qtyPrSaleUoM"].Value.ToString());
                    varRealBsNetSalePrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["realBsNetSalePrc"].Value.ToString());
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // real = pricelist.netPrice and cannot be changed.
                varRealBsGrossSalePrc = varRealBsNetSalePrc * ((varVatable == "Y") ? 1.12m : 1);
                varGrossPrchsPrc = varNetPrchsPrc * ((varVatable == "Y") ? 1.12m : 1);
                varNetSalePrice = (varRealBsNetSalePrc * ((varBsUoM == "N") ? varQtyPrSaleUoM : 1)) * varQty;
                varGrossSalePrice = varNetSalePrice * ((varVatable == "Y") ? 1.12m : 1);
                varRowNetTotal = varNetSalePrice;
                varRowGrossTotal = varGrossSalePrice;

                dgvItems.Rows[rowIndex].Cells["grossPrchsPrc"].Value = varGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["realBsGrossSalePrc"].Value = varRealBsGrossSalePrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["prcntDscnt"].Value = "0.00";
                dgvItems.Rows[rowIndex].Cells["amtDscnt"].Value = "0.00";
                dgvItems.Rows[rowIndex].Cells["netSalePrc"].Value = varNetSalePrice.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["grossSalePrc"].Value = varGrossSalePrice.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["rowNetTotal"].Value = varRowNetTotal.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["rowGrossTotal"].Value = varRowGrossTotal.ToString(vars.grossFormat);
            }
        }

        private void computeForPrcntDscnt(int rowIndex)
        {
            if (itemCodeIsFound(fx.null2EmptyStr(dgvItems.Rows[rowIndex].Cells["itemCode"].Value)))
            {
                string varVatable, varSaleUoM, varBsUoM;
                decimal varQty, varAmtDscnt, varRealBsNetSalePrc, varRealBsGrossSalePrc, varNetPrchsPrc, varGrossPrchsPrc, varQtyPrSaleUoM, varPrcntDscnt, varNetSalePrice, varGrossSalePrice, varRowNetTotal, varRowGrossTotal;

                try
                {
                    varPrcntDscnt = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["prcntDscnt"].Value.ToString());
                    if (varPrcntDscnt < 0)
                    {
                        MessageBox.Show(this, "Cannot contain less than or equal to zero value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    varQty = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["Qty"].Value.ToString());
                    varBsUoM = dgvItems.Rows[rowIndex].Cells["baseUoM"].Value.ToString();
                    varVatable = dgvItems.Rows[rowIndex].Cells["vatable"].Value.ToString();
                    varNetPrchsPrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["netPrchsPrc"].Value.ToString());
                    varSaleUoM = dgvItems.Rows[rowIndex].Cells["saleUoM"].Value.ToString();
                    varQtyPrSaleUoM = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["qtyPrSaleUoM"].Value.ToString());
                    varRealBsNetSalePrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["realBsNetSalePrc"].Value.ToString());
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // real = pricelist.netPrice and cannot be changed.
                varRealBsGrossSalePrc = varRealBsNetSalePrc * ((varVatable == "Y") ? 1.12m : 1);
                varGrossPrchsPrc = varNetPrchsPrc * ((varVatable == "Y") ? 1.12m : 1);
                varNetSalePrice = (varRealBsNetSalePrc * ((varBsUoM == "N") ? varQtyPrSaleUoM : 1)) * varQty;
                varGrossSalePrice = varNetSalePrice * ((varVatable == "Y") ? 1.12m : 1);
                varRowGrossTotal = varGrossSalePrice * (1 - (varPrcntDscnt / 100));
                varRowNetTotal = varRowGrossTotal / ((varVatable == "Y") ? 1.12m : 1);
                varAmtDscnt = varGrossSalePrice - varRowGrossTotal;

                dgvItems.Rows[rowIndex].Cells["grossPrchsPrc"].Value = varGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["realBsGrossSalePrc"].Value = varRealBsGrossSalePrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["amtDscnt"].Value = varAmtDscnt;
                dgvItems.Rows[rowIndex].Cells["netSalePrc"].Value = varNetSalePrice.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["grossSalePrc"].Value = varGrossSalePrice.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["rowNetTotal"].Value = varRowNetTotal.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["rowGrossTotal"].Value = varRowGrossTotal.ToString(vars.grossFormat);
            }
        }

        private void computeForAmtDscnt(int rowIndex)
        {
            if (itemCodeIsFound(fx.null2EmptyStr(dgvItems.Rows[rowIndex].Cells["itemCode"].Value)))
            {
                string varVatable, varSaleUoM, varBsUoM;
                decimal varQty, varAmtDscnt, varRealBsNetSalePrc, varRealBsGrossSalePrc, varNetPrchsPrc, varGrossPrchsPrc, varQtyPrSaleUoM, varPrcntDscnt, varNetSalePrice, varGrossSalePrice, varRowNetTotal, varRowGrossTotal;

                try
                {
                    varAmtDscnt = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["amtDscnt"].Value.ToString());
                    if (varAmtDscnt < 0)
                    {
                        MessageBox.Show(this, "Cannot contain less than or equal to zero value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    varQty = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["Qty"].Value.ToString());
                    varBsUoM = dgvItems.Rows[rowIndex].Cells["baseUoM"].Value.ToString();
                    varVatable = dgvItems.Rows[rowIndex].Cells["vatable"].Value.ToString();
                    varNetPrchsPrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["netPrchsPrc"].Value.ToString());
                    varSaleUoM = dgvItems.Rows[rowIndex].Cells["saleUoM"].Value.ToString();
                    varQtyPrSaleUoM = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["qtyPrSaleUoM"].Value.ToString());
                    varRealBsNetSalePrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["realBsNetSalePrc"].Value.ToString());
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // real = pricelist.netPrice and cannot be changed.
                varRealBsGrossSalePrc = varRealBsNetSalePrc * ((varVatable == "Y") ? 1.12m : 1);
                varGrossPrchsPrc = varNetPrchsPrc * ((varVatable == "Y") ? 1.12m : 1);
                varNetSalePrice = (varRealBsNetSalePrc * ((varBsUoM == "N") ? varQtyPrSaleUoM : 1)) * varQty;
                varGrossSalePrice = varNetSalePrice * ((varVatable == "Y") ? 1.12m : 1);
                varRowGrossTotal = varGrossSalePrice - varAmtDscnt;
                varRowNetTotal = varRowGrossTotal / ((varVatable == "Y") ? 1.12m : 1);
                varPrcntDscnt = 100 - (varRowGrossTotal / varGrossSalePrice) * 100;

                dgvItems.Rows[rowIndex].Cells["grossPrchsPrc"].Value = varGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["realBsGrossSalePrc"].Value = varRealBsGrossSalePrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["prcntDscnt"].Value = varPrcntDscnt;
                dgvItems.Rows[rowIndex].Cells["netSalePrc"].Value = varNetSalePrice.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["grossSalePrc"].Value = varGrossSalePrice.ToString(vars.grossFormat);
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

        private void populateDataGridRow(Hashtable ht, int rowIndex)
        {
            dgvItems.Rows[rowIndex].Cells["itemCode"].Value = ht["itemCode"];
            dgvItems.Rows[rowIndex].Cells["description"].Value = ht["description"];
            dgvItems.Rows[rowIndex].Cells["vatable"].Value = ht["vatable"];
            dgvItems.Rows[rowIndex].Cells["netPrchsPrc"].Value = Decimal.Parse(ht["netPrchsPrc"].ToString()).ToString(vars.format);
            dgvItems.Rows[rowIndex].Cells["saleUoM"].Value = ht["saleUoM"];
            dgvItems.Rows[rowIndex].Cells["qtyPrSaleUoM"].Value = Decimal.Parse(ht["qtyPrSaleUoM"].ToString()).ToString(vars.format);
            dgvItems.Rows[rowIndex].Cells["realBsNetSalePrc"].Value = Decimal.Parse(ht["realBsNetSalePrc"].ToString()).ToString(vars.format);
        }
        
        private void resetCellValues(int rowIndex)
        {
            dgvItems.Rows[rowIndex].Cells["vatable"].Value = "";
            dgvItems.Rows[rowIndex].Cells["netPrchsPrc"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["grossPrchsPrc"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["saleUoM"].Value = "";
            dgvItems.Rows[rowIndex].Cells["qtyPrSaleUoM"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["realBsNetSalePrc"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["realBsGrossSalePrc"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["Qty"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["prcntDscnt"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["amtDscnt"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["netSalePrc"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["grossSalePrc"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["rowNetTotal"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["rowGrossTotal"].Value = "0.00";
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
                        sql = "SELECT itemCode,description,saleUoM,qtyPrSaleUoM,vatable, netPrice netPrchsPrc";
                        sql += ",(SELECT netPrice";
                        sql += "    FROM itemmasterdata JOIN pricelist USING(itemCode)";
                        sql += "    WHERE priceListCode=1 AND itemmasterdata.itemCode=A.itemCode) realNetBsSalePrc";
                        sql += " FROM itemmasterdata A JOIN pricelist B USING(itemCode)";
                        sql += " WHERE pricelistCode=0";
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
                                        resetCellValues(e.RowIndex);
                                        dgvItems.Rows[e.RowIndex].Cells["baseUoM"].Value = "Y"; //let's have an initial value
                                        dgvItems.Rows[e.RowIndex].Cells["Qty"].Value = "1"; //let's have an initial qty
                                        populateDataGridRow(ht, e.RowIndex);
                                        computeForQty(e.RowIndex);
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
                                            dgvItems.Rows[e.RowIndex].Cells["baseUoM"].Value = "Y"; //let's have an initial value
                                            dgvItems.Rows[e.RowIndex].Cells["Qty"].Value = "1"; //let's have an initial qty
                                            populateDataGridRow(ht, e.RowIndex);
                                            computeForQty(e.RowIndex);
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
                                        dgvItems.Rows[e.RowIndex].Cells["baseUoM"].Value = "Y"; //let's have an initial value
                                        dgvItems.Rows[e.RowIndex].Cells["Qty"].Value = "1"; //let's have an initial qty
                                        populateDataGridRow(ht, e.RowIndex);
                                        computeForQty(e.RowIndex);
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

                    //when column Qty changed
                    if (dgvItems.Columns[e.ColumnIndex].Name == "Qty")
                        computeForQty(e.RowIndex);

                    //when percent discount changed
                    if (dgvItems.Columns[e.ColumnIndex].Name == "prcntDscnt")
                        computeForPrcntDscnt(e.RowIndex);

                    //when base UoM changed
                    if (dgvItems.Columns[e.ColumnIndex].Name == "baseUoM")
                        computeForBaseUoM(e.RowIndex);

                    //when amount discount changed
                    if (dgvItems.Columns[e.ColumnIndex].Name == "amtDscnt")
                        computeForAmtDscnt(e.RowIndex);
                }

                //every edit of the cell must compute other values
                computeDocAmtDscnt();
                computeDocNetTotal();
                computeDocGrossTotal();
                computeDocPrcntDscnt();
                computeItemCount();
            }
        }

        private void setToDefaultState()
        {
            cleanUpUI();
            btnFind.Text = "&Find";
            btnPrint.Enabled = false;
            txtSalesReturnNo.Focus();
        }

        private void setCntrlToOKState()
        {
            txtPostingDate.ReadOnly = true;
            txtCustomerCode.ReadOnly = true; txtCustomerName.ReadOnly = true;
            cboWarehouse.Enabled = false;
            dateTimePicker1.Enabled = false;
            dgvItems.ReadOnly = true;
            dgvItems.AllowUserToAddRows = false;
            linkCustomerCode.Enabled = false; linkCustomerName.Enabled = false;
            btnFind.Text = "&OK";
            btnPrint.Enabled = true;
            txtRemarks1.ReadOnly = true;
            txtSalesReturnNo.ReadOnly = true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (btnFind.Text == "&Find")
            {
                sql = "SELECT docId,customerCode,customerName,warehouse,DATE_FORMAT(postingDate, '%m/%d/%Y') postingDate,totalPrcntDscnt,totalAmtDscnt,ROUND(netTotal,2) netTotal, ROUND(grossTotal,2) grossTotal,remarks1,remarks2 FROM salesreturn WHERE ";

                if (txtSalesReturnNo.Text.Trim() != "" && !txtSalesReturnNo.Text.Contains("*"))
                {
                    sql += "docId='" + txtSalesReturnNo.Text.Trim().Replace("'", "''") + "'";
                    db = new database();
                    dt = new DataTable();
                    dt = db.select(sql, vars.MySqlConnection);
                    if (dt.Rows.Count < 1)
                    {
                        MessageBox.Show(this, "No matching record found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    txtSalesReturnNo.Text = dt.Rows[0]["docId"].ToString();
                    txtCustomerCode.Text = dt.Rows[0]["customerCode"].ToString();
                    txtCustomerName.Text = dt.Rows[0]["customerName"].ToString();
                    cboWarehouse.Text = dt.Rows[0]["warehouse"].ToString();
                    txtPostingDate.Text = dt.Rows[0]["postingDate"].ToString();
                    txtRemarks1.Text = dt.Rows[0]["remarks1"].ToString();
                    txtRemarks2.Text = dt.Rows[0]["remarks2"].ToString();

                    txtTotalPrcntDscnt.Text = Decimal.Parse(dt.Rows[0]["totalPrcntDscnt"].ToString()).ToString(vars.format);
                    txtTotalAmtDscnt.Text = Decimal.Parse(dt.Rows[0]["totalAmtDscnt"].ToString()).ToString(vars.format);
                    txtNetTotal.Text = Decimal.Parse(dt.Rows[0]["netTotal"].ToString()).ToString(vars.format);
                    txtGrossTotal.Text = Decimal.Parse(dt.Rows[0]["grossTotal"].ToString()).ToString(vars.grossFormat);

                    
                    sql = "SELECT * FROM salesreturn_item WHERE docId='" + txtSalesReturnNo.Text.Trim() + "' ORDER BY indx";
                    db = new database();
                    dt = new DataTable();
                    dt = db.select(sql, vars.MySqlConnection);
                    rowCount = dt.Rows.Count;
                    for (i = 0; i < rowCount; i++)
                    {
                        dgvItems.Rows.Add();
                        dgvItems.Rows[i].Cells["itemCode"].Value = dt.Rows[i]["itemCode"].ToString();
                        dgvItems.Rows[i].Cells["description"].Value = dt.Rows[i]["description"].ToString();
                        dgvItems.Rows[i].Cells["vatable"].Value = dt.Rows[i]["vatable"].ToString();
                        dgvItems.Rows[i].Cells["saleUoM"].Value = dt.Rows[i]["saleUoM"].ToString();
                        dgvItems.Rows[i].Cells["qtyPrSaleUoM"].Value = Decimal.Parse(dt.Rows[i]["qtyPrSaleUoM"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["netPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["netPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["grossPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["grossPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["realBsNetSalePrc"].Value = Decimal.Parse(dt.Rows[i]["realBsNetSalePrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["realBsGrossSalePrc"].Value = Decimal.Parse(dt.Rows[i]["realBsGrossSalePrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["Qty"].Value = Decimal.Parse(dt.Rows[i]["qty"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["baseUoM"].Value = dt.Rows[i]["baseUoM"].ToString();
                        dgvItems.Rows[i].Cells["prcntDscnt"].Value = Decimal.Parse(dt.Rows[i]["prcntDscnt"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["amtDscnt"].Value = Decimal.Parse(dt.Rows[i]["amtDscnt"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["netSalePrc"].Value = Decimal.Parse(dt.Rows[i]["netSalePrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["grossSalePrc"].Value = Decimal.Parse(dt.Rows[i]["grossSalePrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["rowNetTotal"].Value = Decimal.Parse(dt.Rows[i]["rowNetTotal"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["rowGrossTotal"].Value = Decimal.Parse(dt.Rows[i]["rowGrossTotal"].ToString()).ToString(vars.format);
                    }

                    setCntrlToOKState();
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
                    txtSalesReturnNo.Text = frmDialogForm.selectedValue;
                }
            }
            else if (btnFind.Text == "&OK")
            {
                this.Close();
            }
            else if (btnFind.Text == "&Update")
            {
                if (checkValues())
                {
                    if (MessageBox.Show(this, "Are you sure you want to update this document?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;

                    db = new database();
                    sql = "UPDATE salesreturn SET remarks2='" + txtRemarks2.Text.Trim().Replace("'", "''") + "',updateDate=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s'),updatedBy='" + vars.username + "' WHERE docId='" + txtSalesReturnNo.Text.Trim() + "'";
                    if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                    {
                        MessageBox.Show(this, "Updating has been successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnFind.Text = "&OK";
                    }
                }
            }
            else if (btnFind.Text == "&Save")
            {
                if (checkValues())
                {
                    if (MessageBox.Show(this, "Are you sure you want to save this?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    DateTime dateTime = DateTime.Parse(txtPostingDate.Text.Trim());
                    string strDateTime = dateTime.ToString("yyyy/MM/dd");

                    sql = "START TRANSACTION;";
                    sql += "SET @date=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s');";
                    sql += "SET @postingDate=DATE_FORMAT('" + strDateTime + "', '%Y-%m-%d');";
                    sql += "SET @username='" + vars.username + "';";
                    sql += "SET @newId=(SELECT CAST(lastNo+1 AS char(11)) FROM documents WHERE documentCode='SR');";
                    sql += "SET @docId=CONCAT('" + vars.terminalId + "', @newId);";
                    sql += "INSERT INTO salesreturn(docId,customerCode,customerName,warehouse,postingDate,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal";
                    if (txtRemarks1.Text.Trim() != "")
                        sql += ",remarks1";
                    if (txtRemarks2.Text.Trim() != "")
                        sql += ",remarks2";
                    sql += ",createDate,createdBy)";
                    sql += " VALUES(@docId,'" + txtCustomerCode.Text.Trim().Replace("'", "''") + "','" + txtCustomerName.Text.Trim().Replace("'", "''") + "','" + cboWarehouse.Text + "',@postingDate," + txtTotalPrcntDscnt.Text.Replace(",", "").Trim() + "," + txtTotalAmtDscnt.Text.Replace(",", "").Trim() + "," + txtNetTotal.Text.Replace(",", "").Trim() + "," + txtGrossTotal.Text.Replace(",", "").Trim();
                    if (txtRemarks1.Text.Trim() != "")
                        sql += ",'" + txtRemarks1.Text.Trim() + "'";
                    if (txtRemarks2.Text.Trim() != "")
                        sql += ",'" + txtRemarks2.Text.Trim() + "'";
                    sql += ",@date,@username);";
                    sql += "UPDATE businesspartner SET trans='Y' WHERE code='" + txtCustomerCode.Text.Trim().Replace("'", "''") + "';";

                    rowCount = dgvItems.Rows.Count;
                    string varItemCode, varDescription, varSaleUoM, varVatable, varBaseUoM;
                    decimal varQtyPrSaleUoM, varQty, varNetSalePrc, varGrossSalePrc, varPrcntDscnt, varAmtDscnt, varNetPrchsPrc, varGrossPrchsPrc, varRowNetTotal, varRowGrossTotal, varRealBsNetSalePrc, varRealBsGrossSalePrc, varBaseQty;

                    for (i = 0; i < rowCount; i++)
                    {
                        if (!dgvItems.Rows[i].IsNewRow && dgvItems.Rows[i].Cells[0].Value.ToString() != "")
                        {
                            varItemCode = dgvItems.Rows[i].Cells["itemCode"].Value.ToString();
                            varDescription = dgvItems.Rows[i].Cells["description"].Value.ToString();
                            varVatable = dgvItems.Rows[i].Cells["vatable"].Value.ToString();
                            varNetPrchsPrc = fx.removeComma(dgvItems.Rows[i].Cells["netPrchsPrc"].Value);
                            varGrossPrchsPrc = fx.removeComma(dgvItems.Rows[i].Cells["grossPrchsPrc"].Value);
                            varSaleUoM = dgvItems.Rows[i].Cells["saleUoM"].Value.ToString();
                            varBaseUoM = fx.null2EmptyStr(dgvItems.Rows[i].Cells["baseUoM"].Value);
                            varQtyPrSaleUoM = fx.removeComma(dgvItems.Rows[i].Cells["qtyPrSaleUoM"].Value);
                            varRealBsNetSalePrc = fx.removeComma(dgvItems.Rows[i].Cells["realBsNetSalePrc"].Value);
                            varRealBsGrossSalePrc = fx.removeComma(dgvItems.Rows[i].Cells["realBsGrossSalePrc"].Value);
                            varQty = fx.removeComma(dgvItems.Rows[i].Cells["Qty"].Value);

                            varBaseQty = ((varBaseUoM == "N") ? varQtyPrSaleUoM * varQty : varQty);

                            varPrcntDscnt = fx.removeComma(dgvItems.Rows[i].Cells["prcntDscnt"].Value);
                            varAmtDscnt = fx.removeComma(dgvItems.Rows[i].Cells["amtDscnt"].Value);
                            varNetSalePrc = fx.removeComma(dgvItems.Rows[i].Cells["netSalePrc"].Value);
                            varGrossSalePrc = fx.removeComma(dgvItems.Rows[i].Cells["grossSalePrc"].Value);
                            varRowNetTotal = fx.removeComma(dgvItems.Rows[i].Cells["rowNetTotal"].Value);
                            varRowGrossTotal = fx.removeComma(dgvItems.Rows[i].Cells["rowGrossTotal"].Value);

                            sql += "INSERT INTO salesreturn_item(docId,indx,itemCode,description,saleUoM,qtyPrSaleUoM,qty,baseUoM,prcntDscnt,amtDscnt,vatable,netSalePrc,grossSalePrc,realBsNetSalePrc,realBsGrossSalePrc,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal)";
                            sql += " VALUES(@docId," + i + ",'" + varItemCode + "','" + varDescription + "','" + varSaleUoM + "'," + varQtyPrSaleUoM + "," + varQty + ",'" + varBaseUoM + "'," + varPrcntDscnt + "," + varAmtDscnt + ",'" + varVatable + "'," + varNetSalePrc + "," + varGrossSalePrc + "," + varRealBsNetSalePrc + "," + varRealBsGrossSalePrc + "," + varNetPrchsPrc + "," + varGrossPrchsPrc + "," + varRowNetTotal + "," + varRowGrossTotal + ");";

                            sql += "UPDATE itemmasterdata SET trans='Y' WHERE itemCode='" + varItemCode + "';";
                            sql += "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES('" + varItemCode + "','" + cboWarehouse.Text + "'," + varBaseQty + ") ON DUPLICATE KEY UPDATE inStock=ifnull(inStock, 0)+" + varBaseQty + ";";
                        }
                    }
                    sql += "UPDATE documents SET lastNo=CAST(@newId AS UNSIGNED) WHERE documentCode='SR';";
                    sql += "COMMIT;";

                    db = new database();
                    if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                    {
                        MessageBox.Show(this, "Saving has been successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        db = new database(); dt = new DataTable();
                        dt = db.select("SELECT docId FROM salesreturn ORDER BY id DESC LIMIT 1", vars.MySqlConnection);
                        txtSalesReturnNo.Text = dt.Rows[0][0].ToString();
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

        private void registerControls()
        {
            //let's register all controls and DB column names. If new control is added, probably it has to be added here.
            table = new DataTable();
            table.Columns.Add("controls", typeof(string));     //check txt* name
            table.Columns.Add("DBcolumnName", typeof(string)); //check db column name
            table.Columns.Add("Value", typeof(string));        //value of txt*.text

            table.Rows.Add("txtSalesReturnNo", "docId", "");
            table.Rows.Add("txtCustomerCode", "customerCode", "");
            table.Rows.Add("txtCustomerName", "customerName", "");
            table.Rows.Add("txtRemarks1", "remarks1", "");
            table.Rows.Add("txtRemarks2", "remarks2", "");
        }

        private bool checkValues()
        {
            if (txtCustomerCode.Text.Trim() == "")
            {
                MessageBox.Show(this, "No vendor code.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCustomerCode.Focus();
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

            // check if there's warehouse chosen.
            if (cboWarehouse.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Please select warehouse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboWarehouse.Focus();
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

            // check negative row gross total
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
                        MessageBox.Show(this, "Duplicate entries found for item code " + itemCode, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                        return false;
                    }
                }
            }

            // check negative qty
            for (i = 0; i < rowCount; i++)
            {
                itemCode = fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value);
                if (itemCodeIsFound(itemCode))
                {
                    if (Decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString()) <= 0)
                    {
                        MessageBox.Show(this, "Quantity must be greater than 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dgvItems.CurrentCell = dgvItems.Rows[i].Cells["qty"];
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
            txtSalesReturnNo.Text = "";
            dateTimePicker1.Enabled = true;
            txtCustomerCode.Text = ""; txtCustomerName.Text = "";
            txtPostingDate.Text = "";
            linkCustomerCode.Enabled = false;
            linkCustomerName.Enabled = false;
            txtSalesReturnNo.ReadOnly = false;
            txtPostingDate.ReadOnly = false;
            txtCustomerCode.ReadOnly = false;
            txtCustomerName.ReadOnly = false;
            txtRemarks1.ReadOnly = false;
            dgvItems.Rows.Clear();
            dgvItems.ReadOnly = false;
            txtRemarks1.Text = ""; txtRemarks2.Text = "";
            cboWarehouse.Enabled = true;
            cboWarehouse.SelectedIndex = -1;
            txtSalesReturnNo.Focus();

            txtTotalPrcntDscnt.Text = "0.00";
            txtTotalAmtDscnt.Text = "0.00";
            txtNetTotal.Text = "0.00";
            txtGrossTotal.Text = "0.00";
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

        private void linkCustomerCode_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtCustomerName.Text = "";
            sql = "SELECT code,BPName FROM businesspartner WHERE BPType=1 AND deactivated='N' AND code";

            if (txtCustomerCode.Text.Trim() != "" || txtCustomerCode.Text.Contains("*"))
            {
                sql += " like '" + txtCustomerCode.Text.Replace("*", "%").Trim() + "' ORDER BY code";
                frmDialogForm = new frmDialog();
                frmDialogForm.selectedValue = sql;
                frmDialogForm.ShowDialog();
                txtCustomerCode.Text = frmDialogForm.selectedValue;

                if (txtCustomerCode.Text.Trim() == "")
                    return;

                sql = "SELECT code,BPName FROM businesspartner WHERE BPType=1 AND deactivated='N' AND code='" + txtCustomerCode.Text + "'";
                db = new database(); dt = new DataTable();
                dt = db.select(sql, vars.MySqlConnection);
                txtCustomerCode.Text = dt.Rows[0]["code"].ToString();
                txtCustomerName.Text = dt.Rows[0]["BPName"].ToString();
                return;
            }
            else
            {
                sql += "='" + txtCustomerCode.Text.Replace("'", "''").Trim() + "' ORDER BY code";
            }

            db = new database(); dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count < 1)
            {
                MessageBox.Show(this, "No matching record found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //we expect only 1 output
            txtCustomerCode.Text = dt.Rows[0]["code"].ToString();
            txtCustomerName.Text = dt.Rows[0]["BPName"].ToString();
            dgvItems.Focus();
        }

        private void linkCustomerName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtCustomerCode.Text = "";
            sql = "SELECT code,BPName FROM businesspartner WHERE BPType=1 AND deactivated='N' AND BPName";

            if (txtCustomerName.Text.Trim() != "" || txtCustomerName.Text.Contains("*"))
            {
                sql += " like '" + txtCustomerName.Text.Replace("*", "%").Trim() + "' ORDER BY BPName";
                frmDialogForm = new frmDialog();
                frmDialogForm.selectedValue = sql;
                frmDialogForm.ShowDialog();
                txtCustomerCode.Text = frmDialogForm.selectedValue;

                if (txtCustomerCode.Text == "")
                    return;

                sql = "SELECT BPName FROM businesspartner WHERE BPType=1 AND deactivated='N' AND code='" + txtCustomerCode.Text + "'";
                db = new database(); dt = new DataTable();
                dt = db.select(sql, vars.MySqlConnection);
                if (dt.Rows.Count > 0)
                {
                    txtCustomerName.Text = dt.Rows[0][0].ToString();
                    cboWarehouse.Focus();
                }
            }
            else
            {
                sql += "='" + txtCustomerName.Text.Replace("'", "''").Trim() + "' ORDER BY BPName";

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
                    txtCustomerCode.Text = dt.Rows[0]["code"].ToString();
                    txtCustomerName.Text = dt.Rows[0]["BPName"].ToString();
                    dgvItems.Focus();
                }
                else
                {
                    frmDialogForm = new frmDialog();
                    frmDialogForm.selectedValue = sql;
                    frmDialogForm.ShowDialog();
                    txtCustomerCode.Text = frmDialogForm.selectedValue;
                    db = new database();
                    dt = new DataTable();
                    dt = db.select(sql, vars.MySqlConnection);
                    txtCustomerCode.Text = dt.Rows[0]["code"].ToString();
                    txtCustomerName.Text = dt.Rows[0]["BPName"].ToString();
                    dgvItems.Focus();
                }
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFind.Text = "&Save";
            btnPrint.Enabled = false;
            cleanUpUI();
            txtPostingDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            dgvItems.Enabled = true;
            dgvItems.AllowUserToAddRows = true;
            linkCustomerCode.Enabled = true;
            linkCustomerName.Enabled = true;
            txtSalesReturnNo.ReadOnly = true;
            txtCustomerCode.Focus();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFind.Text = "&Update";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            crystal_reports.salesReturn objReport;
            this.Cursor = Cursors.WaitCursor;

            sql = "SELECT A.docId,customerCode,customerName,warehouse,DATE_FORMAT(postingDate, '%m/%d/%Y') postingDate,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,remarks1,remarks2, B.* FROM salesreturn A JOIN salesreturn_item B USING(docId) WHERE A.docId='" + txtSalesReturnNo.Text.Trim() + "' ORDER BY indx";
            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);

            objReport = new crystal_reports.salesReturn();
            objReport.SetDataSource(dt);

            crystalReportViewerForm = new frmCrystalReportViewer();
            crystalReportViewerForm.CrystalReportViewer1.ReportSource = objReport;

            crystalReportViewerForm.MdiParent = this.MdiParent;
            crystalReportViewerForm.Show();
            crystalReportViewerForm.Text = "Sales Return Report";
            this.Cursor = Cursors.Default;
        }

        private void removeRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvItems.Rows.RemoveAt(dgvItems.CurrentRow.Index);
            computeDocument();
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

        private void frmSalesReturn_Load(object sender, EventArgs e)
        {
            // if it's a main warehouse, they're allowed to create AR Invoice for different branches
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
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            txtPostingDate.Text = dateTimePicker1.Value.ToString("MM/dd/yyyy");
        }
    }
}
