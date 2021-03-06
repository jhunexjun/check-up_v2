﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Check_up.classes;
using MySql.Data.MySqlClient;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;

namespace Check_up.forms
{
    public partial class frmSalesInvoice : Form
    {
        DataTable table;
        DataTable dt;
        database db = new database();
        private frmDialog frmDialogForm;
        private frmCrystalReportViewer crystalReportViewerForm;
        string sql;
        int i, rowCount;

        MySqlCommand cmd; MySqlDataAdapter da;

        Hashtable header;

        public frmSalesInvoice()
        {
            InitializeComponent();
            dgvItems.CellEndEdit += new DataGridViewCellEventHandler(dgvItems_CellEndEdit);
        }

        private Hashtable getInfo(string itemCode)
        {
            Hashtable ht = new Hashtable();
            sql = "SELECT itemCode,description,saleUoM,qtyPrSaleUoM,qtyPrPrchsUoM,vatable, netPrice netBsPrchsPrc";
            sql += ",(SELECT netPrice";
            sql += "    FROM itemmasterdata JOIN pricelist USING(itemCode)";
            sql += "    WHERE priceListCode=1 AND itemmasterdata.itemCode=A.itemCode) netBsSalePrc";
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
                ht["netBsPrchsPrc"] = dt.Rows[0]["netBsPrchsPrc"];
                ht["saleUoM"] = dt.Rows[0]["saleUoM"].ToString();
                ht["qtyPrSaleUoM"] = dt.Rows[0]["qtyPrSaleUoM"];
                ht["qtyPrPrchsUoM"] = dt.Rows[0]["qtyPrPrchsUoM"];
                ht["netBsSalePrc"] = dt.Rows[0]["netBsSalePrc"];
                return ht;
            }
            else if (dt.Rows.Count > 1)
                return ht;
            else
                return ht;
        }

        //this function is used to get the corresponding db itemmasterdata.<column name>
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
                decimal varQty, varNetBsSalePrc, varGrossBsSalePrc, varNetBsPrchsPrc, varGrossBsPrchsPrc, varQtyPrSaleUoM, varNetSalePrice, varGrossSalePrice, varRowNetTotal, varRowGrossTotal;

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
                    varNetBsPrchsPrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["netBsPrchsPrc"].Value.ToString());
                    varSaleUoM = dgvItems.Rows[rowIndex].Cells["saleUoM"].Value.ToString();
                    varQtyPrSaleUoM = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["qtyPrSaleUoM"].Value.ToString());
                    varNetBsSalePrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["netBsSalePrc"].Value.ToString());
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // real = pricelist.netPrice and cannot be changed.
                varGrossBsSalePrc = varNetBsSalePrc * ((varVatable == "Y") ? 1.12m : 1);
                varGrossBsPrchsPrc = varNetBsPrchsPrc * ((varVatable == "Y") ? 1.12m : 1);
                varNetSalePrice = (varNetBsSalePrc * ((varBsUoM == "N") ? varQtyPrSaleUoM : 1)) * varQty;
                varGrossSalePrice = varNetSalePrice * ((varVatable == "Y") ? 1.12m : 1);
                varRowNetTotal = varNetSalePrice;
                varRowGrossTotal = varGrossSalePrice;

                dgvItems.Rows[rowIndex].Cells["grossBsPrchsPrc"].Value = varGrossBsPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["grossBsSalePrc"].Value = varGrossBsSalePrc.ToString(vars.grossFormat);
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
                decimal varQty, varAmtDscnt, varNetBsSalePrc, varGrossBsSalePrc, varNetBsPrchsPrc, varGrossBsPrchsPrc, varQtyPrSaleUoM, varPrcntDscnt, varNetSalePrice, varGrossSalePrice, varRowNetTotal, varRowGrossTotal;

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
                    varNetBsPrchsPrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["netBsPrchsPrc"].Value.ToString());
                    varSaleUoM = dgvItems.Rows[rowIndex].Cells["saleUoM"].Value.ToString();
                    varQtyPrSaleUoM = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["qtyPrSaleUoM"].Value.ToString());
                    varNetBsSalePrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["netBsSalePrc"].Value.ToString());
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // real = pricelist.netPrice and cannot be changed.
                varGrossBsSalePrc = varNetBsSalePrc * ((varVatable == "Y") ? 1.12m : 1);
                varGrossBsPrchsPrc = varNetBsPrchsPrc * ((varVatable == "Y") ? 1.12m : 1);
                varNetSalePrice = (varNetBsSalePrc * ((varBsUoM == "N") ? varQtyPrSaleUoM : 1)) * varQty;
                varGrossSalePrice = varNetSalePrice * ((varVatable == "Y") ? 1.12m : 1);
                varRowGrossTotal = varGrossSalePrice * (1 - (varPrcntDscnt / 100));
                varRowNetTotal = varRowGrossTotal / ((varVatable == "Y") ? 1.12m : 1);
                varAmtDscnt = varGrossSalePrice - varRowGrossTotal;

                dgvItems.Rows[rowIndex].Cells["grossBsPrchsPrc"].Value = varGrossBsPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["grossBsSalePrc"].Value = varGrossBsSalePrc.ToString(vars.grossFormat);
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
                decimal varQty, varAmtDscnt, varNetBsSalePrc, varGrossBsSalePrc, varNetBsPrchsPrc, varGrossBsPrchsPrc, varQtyPrSaleUoM, varPrcntDscnt, varNetSalePrice, varGrossSalePrice, varRowNetTotal, varRowGrossTotal;

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
                    varNetBsPrchsPrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["netBsPrchsPrc"].Value.ToString());
                    varSaleUoM = dgvItems.Rows[rowIndex].Cells["saleUoM"].Value.ToString();
                    varQtyPrSaleUoM = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["qtyPrSaleUoM"].Value.ToString());
                    varNetBsSalePrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["netBsSalePrc"].Value.ToString());
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // real = pricelist.netPrice and cannot be changed.
                varGrossBsSalePrc = varNetBsSalePrc * ((varVatable == "Y") ? 1.12m : 1);
                varGrossBsPrchsPrc = varNetBsPrchsPrc * ((varVatable == "Y") ? 1.12m : 1);
                varNetSalePrice = (varNetBsSalePrc * ((varBsUoM == "N") ? varQtyPrSaleUoM : 1)) * varQty;
                varGrossSalePrice = varNetSalePrice * ((varVatable == "Y") ? 1.12m : 1);
                varRowGrossTotal = varGrossSalePrice - varAmtDscnt;
                varRowNetTotal = varRowGrossTotal / ((varVatable == "Y") ? 1.12m : 1);
                varPrcntDscnt = 100 - (varRowGrossTotal / varGrossSalePrice) * 100;

                dgvItems.Rows[rowIndex].Cells["grossBsPrchsPrc"].Value = varGrossBsPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["grossBsSalePrc"].Value = varGrossBsSalePrc.ToString(vars.grossFormat);
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
            dgvItems.Rows[rowIndex].Cells["netBsPrchsPrc"].Value = Decimal.Parse(ht["netBsPrchsPrc"].ToString()).ToString(vars.format);
            dgvItems.Rows[rowIndex].Cells["saleUoM"].Value = ht["saleUoM"];
            dgvItems.Rows[rowIndex].Cells["qtyPrSaleUoM"].Value = Decimal.Parse(ht["qtyPrSaleUoM"].ToString()).ToString(vars.format);
            dgvItems.Rows[rowIndex].Cells["qtyPrPrchsUoM"].Value = Decimal.Parse(ht["qtyPrPrchsUoM"].ToString()).ToString(vars.format);
            dgvItems.Rows[rowIndex].Cells["netBsSalePrc"].Value = Decimal.Parse(ht["netBsSalePrc"].ToString()).ToString(vars.format);
        }

        private void resetCellValues(int rowIndex)
        {
            dgvItems.Rows[rowIndex].Cells["vatable"].Value = "";
            dgvItems.Rows[rowIndex].Cells["netBsPrchsPrc"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["grossBsPrchsPrc"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["saleUoM"].Value = "";
            dgvItems.Rows[rowIndex].Cells["qtyPrSaleUoM"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["netBsSalePrc"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["grossBsSalePrc"].Value = "0.00";
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
                        sql = "SELECT itemCode,description,saleUoM,qtyPrSaleUoM,qtyPrPrchsUoM,vatable, netPrice netBsPrchsPrc";
                        sql += ",(SELECT netPrice";
                        sql += "    FROM itemmasterdata JOIN pricelist USING(itemCode)";
                        sql += "    WHERE priceListCode=1 AND itemmasterdata.itemCode=A.itemCode) realNetBsSalePrc";
                        sql += " FROM itemmasterdata A JOIN pricelist B USING(itemCode)";
                        sql += " WHERE A.deactivated='N' AND pricelistCode=0";
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

                    //when base UoM changed
                    if (dgvItems.Columns[e.ColumnIndex].Name == "baseUoM")
                        computeForBaseUoM(e.RowIndex);

                    //when percent discount changed
                    if (dgvItems.Columns[e.ColumnIndex].Name == "prcntDscnt")
                        computeForPrcntDscnt(e.RowIndex);

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

        private void setCntrlToOKState()
        {
            txtCustomerCode.ReadOnly = true; txtCustomerName.ReadOnly = true;
            linkCustomerCode.Enabled = false; linkCustomerName.Enabled = false;
            dateTimePicker1.Enabled = false;
            cboWarehouse.Enabled = false;
            txtRemarks1.ReadOnly = true;
            txtSalesInvoiceNo.ReadOnly = true;
            txtPostingDate.ReadOnly = true;
            dgvItems.ReadOnly = true;
            btnFind.Text = "&OK";
            btnPrint.Enabled = true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (btnFind.Text == "&Find")
            {
                sql = "SELECT docId,customerCode,customerName,warehouse,DATE_FORMAT(postingDate, '%m/%d/%Y') postingDate,totalPrcntDscnt,totalAmtDscnt,ROUND(netTotal,2) netTotal, ROUND(grossTotal,2) grossTotal,remarks1,remarks2 FROM salesinvoice WHERE ";

                if (txtSalesInvoiceNo.Text.Trim() != "" && ! txtSalesInvoiceNo.Text.Contains("*"))
                {
                    sql += "docId='" + txtSalesInvoiceNo.Text.Trim().Replace("'", "''") + "'";
                    db = new database();
                    dt = new DataTable();
                    dt = db.select(sql, vars.MySqlConnection);
                    if (dt.Rows.Count < 1)
                    {
                        MessageBox.Show(this, "No matching record found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    txtSalesInvoiceNo.Text = dt.Rows[0]["docId"].ToString();
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

                    
                    sql = "SELECT * FROM salesinvoice_item WHERE docId='" + txtSalesInvoiceNo.Text.Trim() + "' ORDER BY indx";
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
                        dgvItems.Rows[i].Cells["netBsPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["netBsPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["grossBsPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["grossBsPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["netBsSalePrc"].Value = Decimal.Parse(dt.Rows[i]["netBsSalePrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["grossBsSalePrc"].Value = Decimal.Parse(dt.Rows[i]["grossBsSalePrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["Qty"].Value = Decimal.Parse(dt.Rows[i]["qty"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["baseUoM"].Value = dt.Rows[i]["baseUoM"].ToString();
                        dgvItems.Rows[i].Cells["prcntDscnt"].Value = Decimal.Parse(dt.Rows[i]["prcntDscnt"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["amtDscnt"].Value = Decimal.Parse(dt.Rows[i]["amtDscnt"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["netSalePrc"].Value = Decimal.Parse(dt.Rows[i]["netSalePrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["grossSalePrc"].Value = Decimal.Parse(dt.Rows[i]["grossSalePrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["rowNetTotal"].Value = Decimal.Parse(dt.Rows[i]["rowNetTotal"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["rowGrossTotal"].Value = Decimal.Parse(dt.Rows[i]["rowGrossTotal"].ToString()).ToString(vars.grossFormat);
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
                    txtSalesInvoiceNo.Text = frmDialogForm.selectedValue;                     
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
                    sql = "UPDATE salesinvoice SET remarks2='" + txtRemarks2.Text.Trim().Replace("'", "''") + "',updateDate=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s'),updatedBy='" + vars.username + "' WHERE docId='" + txtSalesInvoiceNo.Text.Trim() + "'";
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

                    /* header = new Hashtable();
                    
                    DateTime dateTime = DateTime.Parse(txtPostingDate.Text.Trim());
                    string strDateTime = dateTime.ToString("yyyy/MM/dd");

                    sql = "START TRANSACTION;";
                    sql += "SET @date=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s');";
                    sql += "SET @postingDate=DATE_FORMAT('" + strDateTime + "', '%Y-%m-%d');";
                    sql += "SET @username='" + vars.username + "';";
                    sql += "SET @newId=(SELECT CAST(lastNo+1 AS char(11)) FROM documents WHERE documentCode='SI');";
                    sql += "SET @docId=CONCAT('" + vars.terminalId + "', @newId);";
                    sql += "INSERT INTO salesinvoice(docId,customerCode,customerName,warehouse,postingDate,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal";
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
                    decimal varQtyPrSaleUoM, varQty, varNetSalePrc, varGrossSalePrc, varPrcntDscnt, varAmtDscnt, varNetPrchsPrc, varGrossPrchsPrc, varRowNetTotal, varRowGrossTotal, varRealBsNetSalePrc, varRealBsGrossSalePrc;
                    decimal varBaseQty;

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

                            varPrcntDscnt = fx.removeComma(dgvItems.Rows[i].Cells["prcntDscnt"].Value);
                            varAmtDscnt = fx.removeComma(dgvItems.Rows[i].Cells["amtDscnt"].Value);
                            varNetSalePrc = fx.removeComma(dgvItems.Rows[i].Cells["netSalePrc"].Value);
                            varGrossSalePrc = fx.removeComma(dgvItems.Rows[i].Cells["grossSalePrc"].Value);
                            varRowNetTotal = fx.removeComma(dgvItems.Rows[i].Cells["rowNetTotal"].Value);
                            varRowGrossTotal = fx.removeComma(dgvItems.Rows[i].Cells["rowGrossTotal"].Value);

                            varBaseQty = ((varBaseUoM == "N") ? varQtyPrSaleUoM * varQty : varQty);

                            sql += "INSERT INTO salesinvoice_item(docId,indx,itemCode,description,saleUoM,qtyPrSaleUoM,qty,baseUoM,prcntDscnt,amtDscnt,vatable,netSalePrc,grossSalePrc,realBsNetSalePrc,realBsGrossSalePrc,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal)";
                            sql += " VALUES(@docId," + i + ",'" + varItemCode + "','" + varDescription + "','" + varSaleUoM + "'," + varQtyPrSaleUoM + "," + varQty + ",'" + varBaseUoM + "'," + varPrcntDscnt + "," + varAmtDscnt + ",'" + varVatable + "'," + varNetSalePrc + "," + varGrossSalePrc + "," + varRealBsNetSalePrc + "," + varRealBsGrossSalePrc + "," + varNetPrchsPrc + "," + varGrossPrchsPrc + "," + varRowNetTotal + "," + varRowGrossTotal + ");";

                            sql += "UPDATE itemmasterdata SET trans='Y' WHERE itemCode='" + varItemCode + "';";
                            sql += "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES('" + varItemCode + "','" + cboWarehouse.Text + "'," + varBaseQty + ") ON DUPLICATE KEY UPDATE inStock=ifnull(inStock, 0)-" + varBaseQty + ";";
                        }
                    }
                    sql += "UPDATE documents SET lastNo=CAST(@newId AS UNSIGNED) WHERE documentCode='SI';";
                    sql += "COMMIT;";

                    db = new database();
                    if (db.executeNonQuery(sql, vars.MySqlConnection) > 0)
                    {
                        MessageBox.Show(this, "Saving has been successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        db = new database(); dt = new DataTable();
                        dt = db.select("SELECT docId FROM salesinvoice ORDER BY id DESC LIMIT 1", vars.MySqlConnection);
                        txtSalesInvoiceNo.Text = dt.Rows[0][0].ToString();
                        //setToAfterSaveState();
                        setCntrlToOKState();
                    } */

                    /* the above code is before. Now I'm making it OOP. */
                    try
                    {
                        header = new Hashtable();
                        header.Add("terminalId", vars.terminalId);
                        header.Add("customerCode", txtCustomerCode.Text.Trim());
                        header.Add("customerName", txtCustomerName.Text.Trim());
                        header.Add("warehouse", cboWarehouse.Text.Trim());

                        DateTime dateTime = DateTime.Parse(txtPostingDate.Text.Trim());
                        header.Add("postingDate", dateTime.ToString("yyyy/MM/dd"));
                        header.Add("totalPrcntDscnt", Decimal.Parse(txtTotalPrcntDscnt.Text.Trim()));
                        header.Add("totalAmtDscnt", Decimal.Parse(txtTotalAmtDscnt.Text.Trim()));
                        header.Add("netTotal", Decimal.Parse(txtNetTotal.Text.Trim()));
                        header.Add("grossTotal", Decimal.Parse(txtGrossTotal.Text.Trim()));
                        header.Add("remarks1", txtRemarks1.Text.Trim());
                        header.Add("remarks2", txtRemarks2.Text.Trim());
                        header.Add("createdBy", vars.username);
                    }
                    catch (ExecutionEngineException ex)
                    {
                        MessageBox.Show(this, ex.ToString());
                        return;
                    }

                    table = new DataTable();
                    table.Columns.Add("indx");
                    table.Columns.Add("itemcode");
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

                    rowCount = dgvItems.Rows.Count;
                    DataRow row;

                    try
                    {
                        for (i = 0; i < rowCount; i++)
                        {
                            if (!dgvItems.Rows[i].IsNewRow && dgvItems.Rows[i].Cells[0].Value.ToString() != "")
                            {
                                row = table.NewRow();
                                row["indx"] = i;
                                row["itemcode"] = dgvItems.Rows[i].Cells["itemcode"].Value.ToString();
                                row["description"] = dgvItems.Rows[i].Cells["description"].Value.ToString();
                                row["vatable"] = dgvItems.Rows[i].Cells["vatable"].Value.ToString();
                                row["saleUoM"] = dgvItems.Rows[i].Cells["saleUoM"].Value.ToString();
                                row["qtyPrPrchsUoM"] = Decimal.Parse(dgvItems.Rows[i].Cells["qtyPrPrchsUoM"].Value.ToString());
                                row["qtyPrSaleUoM"] = Decimal.Parse(dgvItems.Rows[i].Cells["qtyPrSaleUoM"].Value.ToString());
                                row["netBsPrchsPrc"] = Decimal.Parse(dgvItems.Rows[i].Cells["netBsPrchsPrc"].Value.ToString());
                                row["grossBsPrchsPrc"] = Decimal.Parse(dgvItems.Rows[i].Cells["grossBsPrchsPrc"].Value.ToString());
                                row["netBsSalePrc"] = Decimal.Parse(dgvItems.Rows[i].Cells["netBsSalePrc"].Value.ToString());
                                row["grossBsSalePrc"] = Decimal.Parse(dgvItems.Rows[i].Cells["grossBsSalePrc"].Value.ToString());
                                row["qty"] = Decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString());
                                row["baseUoM"] = fx.null2EmptyStr(dgvItems.Rows[i].Cells["baseUoM"].Value.ToString());
                                row["prcntDscnt"] = Decimal.Parse(dgvItems.Rows[i].Cells["prcntDscnt"].Value.ToString());
                                row["amtDscnt"] = Decimal.Parse(dgvItems.Rows[i].Cells["amtDscnt"].Value.ToString());
                                row["netSalePrc"] = Decimal.Parse(dgvItems.Rows[i].Cells["netSalePrc"].Value.ToString());
                                row["grossSalePrc"] = Decimal.Parse(dgvItems.Rows[i].Cells["grossSalePrc"].Value.ToString());
                                row["rowNetTotal"] = Decimal.Parse(dgvItems.Rows[i].Cells["rowNetTotal"].Value.ToString());
                                row["rowGrossTotal"] = Decimal.Parse(dgvItems.Rows[i].Cells["rowGrossTotal"].Value.ToString());
                            
                                table.Rows.Add(row);
                            } //end if
                        } // end for()
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.ToString());
                        return;
                    }
                    
                    SalesInvoice salesInvoice = new SalesInvoice();
                    if (salesInvoice.addSalesInvoice(header, table))
                    {
                        MessageBox.Show(this, "Saving has been successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        db = new database(); dt = new DataTable();
                        dt = db.select("SELECT docId FROM salesinvoice ORDER BY id DESC LIMIT 1", vars.MySqlConnection);
                        txtSalesInvoiceNo.Text = dt.Rows[0][0].ToString();
                        setCntrlToOKState();
                    }

                }
            }
        }

        private void setToDefaultState()
        {
            cleanUpUI();
            btnFind.Text = "&Find";
            btnPrint.Enabled = false;
            txtSalesInvoiceNo.Focus();
        }

        /* private void setToAfterSaveState()
        {
            txtPostingDate.ReadOnly = true;
            txtCustomerCode.ReadOnly = true; txtCustomerName.ReadOnly = true;
            cboWarehouse.Enabled = false;
            dateTimePicker1.Enabled = false;
            dgvItems.ReadOnly = true;
            dgvItems.AllowUserToAddRows = false;
            linkCustomerCode.Enabled = false; linkCustomerName.Enabled = false;
            txtRemarks1.Enabled = false;
            btnFind.Text = "&OK";
            btnPrint.Enabled = true;
        }
         */

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

            table.Rows.Add("txtSalesInvoiceNo", "docId", "");
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
            if (btnFind.Text == "&Save" && dgvItems.Rows.Count -1 == 0)
            {
                MessageBox.Show(this, "No items to save.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dgvItems.Focus();
                return false;
            }
            DateTime dateTime;
            if ( ! DateTime.TryParse(txtPostingDate.Text.Trim(), out dateTime))
            {
                MessageBox.Show(this, "Invalid posting date.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPostingDate.Focus();
                return false;
            }

            // check if there's warehouse chosen.
            if (cboWarehouse.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Please select warehouse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

            // We do not allow no inventory or greater than the inventory
            rowCount = dgvItems.Rows.Count;
            decimal varBaseQty, varQty, varQtyPrSaleUoM;
            for (i = 0; i < rowCount; i++)
            {
                itemCode = fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value);
                if (itemCode != "")
                {
                    varQty = Decimal.Parse(dgvItems.Rows[i].Cells["Qty"].Value.ToString());
                    varBaseUoM = dgvItems.Rows[i].Cells["baseUoM"].Value.ToString();
                    varQtyPrSaleUoM = Decimal.Parse(dgvItems.Rows[i].Cells["qtyPrSaleUoM"].Value.ToString());
                    varBaseQty = varQty * ((varBaseUoM == "N") ? varQtyPrSaleUoM : 1);

                    db = new database(); dt = new DataTable();
                    sql = "SET @varBaseQty=" + varBaseQty + ";";
                    sql += "SELECT itemCode FROM item_warehouse WHERE itemCode = '" + dgvItems.Rows[i].Cells["itemCode"].Value + "' AND whCode = '" + cboWarehouse.Text.Trim() + "' AND ifnull(inStock, 0) < @varBaseQty;";
                    dt = db.select(sql, vars.MySqlConnection);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show(this, "Insufficient inventory for item " + dgvItems.Rows[i].Cells["itemCode"].Value, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else //No inventory
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

            //Warning for minimum inventory
            rowCount = dgvItems.Rows.Count;
            for (i = 0; i < rowCount; i++)
            {
                itemCode = fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value);
                if (itemCode != "")
                {
                    varQty = Decimal.Parse(dgvItems.Rows[i].Cells["Qty"].Value.ToString());
                    varBaseUoM = dgvItems.Rows[i].Cells["baseUoM"].Value.ToString();
                    varQtyPrSaleUoM = Decimal.Parse(dgvItems.Rows[i].Cells["qtyPrSaleUoM"].Value.ToString());
                    varBaseQty = varQty * ((varBaseUoM == "N") ? varQtyPrSaleUoM : 1);

                    db = new database(); dt = new DataTable();
                    sql = "SET @varBaseQty=" + varBaseQty + ";";
                    sql += "SELECT itemCode FROM itemmasterdata JOIN item_warehouse USING(itemCode) WHERE itemCode = '" + itemCode + "' AND whCode = '" + cboWarehouse.Text + "' AND minStock > 0 AND minStock > (ifnull(inStock, 0) - @varBaseQty);";
                    dt = db.select(sql, vars.MySqlConnection);
                    if (dt.Rows.Count > 0)
                    {
                        DialogResult result = MessageBox.Show(this, "Warning: Minimum inventory level for item " + itemCode + " has reached. Do you want to proceed?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
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

            //check item master data properties
            rowCount = dgvItems.Rows.Count;
            for (i = 0; i < rowCount; i++)
            {
                itemCode = fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value);
                if (itemCode != "")
                {
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
            }

            //Qty should be greater than 0
            for (i = 0; i < rowCount; i++)
            {
                itemCode = fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value);
                if (itemCode != "")
                {
                    varQty = Decimal.Parse(dgvItems.Rows[i].Cells["Qty"].Value.ToString());
                    if (varQty <= 0)
                    {
                        MessageBox.Show(this, "Cannot contain less than or equal to zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtSalesInvoiceNo.Text = "";
            dateTimePicker1.Enabled = true;
            txtCustomerCode.Text = ""; txtCustomerName.Text = "";
            txtPostingDate.Text = "";
            linkCustomerCode.Enabled = false;
            linkCustomerName.Enabled = false;
            txtSalesInvoiceNo.ReadOnly = false;
            txtPostingDate.ReadOnly = false;
            txtCustomerCode.ReadOnly = false;
            txtCustomerName.ReadOnly = false;
            txtRemarks1.ReadOnly = false;
            dgvItems.Rows.Clear();
            dgvItems.ReadOnly = false;
            txtRemarks1.Text = ""; txtRemarks2.Text = "";
            txtSalesInvoiceNo.Focus();
            cboWarehouse.Enabled = true;
            cboWarehouse.SelectedIndex = -1;

            txtTotalPrcntDscnt.Text = "0.00";
            txtTotalAmtDscnt.Text = "0.00";
            txtNetTotal.Text = "0.00";
            txtGrossTotal.Text = "0.00";
            lblitemsCount.Text = "0 item(s) found.";
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
            txtSalesInvoiceNo.ReadOnly = true;
            txtCustomerCode.Focus();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFind.Text = "&Update";
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
            cboWarehouse.Focus();
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

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

        private void txtGrossTotal_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtCustomerCode_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            crystal_reports.arInvoice objReport;
            this.Cursor = Cursors.WaitCursor;

            sql = "SELECT docId,customerCode,customerName,warehouse,DATE_FORMAT(postingDate, '%m/%d/%Y') postingDate,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,remarks1,remarks2, B.* FROM salesinvoice A JOIN salesinvoice_item B using(docId) WHERE docId = '" + txtSalesInvoiceNo.Text.Trim() + "'";
            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);

            objReport = new crystal_reports.arInvoice();
            objReport.SetDataSource(dt);

            crystalReportViewerForm = new frmCrystalReportViewer();
            crystalReportViewerForm.CrystalReportViewer1.ReportSource = objReport;

            crystalReportViewerForm.MdiParent = this.MdiParent;
            crystalReportViewerForm.Show();
            crystalReportViewerForm.Text = "Sales Report";
            this.Cursor = Cursors.Default;
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

        private void frmARInvoice_Load(object sender, EventArgs e)
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
