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
    public partial class frmDeliveryReceipt : Form
    {
        DataTable table, dt;
        database db = new database();
        private frmDialog frmDialogForm;
        private frmCrystalReportViewer crystalReportViewerForm;
        string sql; int i, rowCount;

        MySqlCommand cmd; MySqlDataAdapter da;

        Hashtable header;

        public frmDeliveryReceipt()
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
            else if (str == "colVendorCode")
                return "code";
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

        private Hashtable getItemInfo(string itemCode)
        {
            Hashtable ht = new Hashtable();
            sql = "SELECT itemCode,description,vatable,qtyPrPrchsUoM,netPrice realBsNetPrchsPrc";
            sql += " FROM itemmasterdata A JOIN pricelist B USING(itemCode)";
            sql += " WHERE itemCode='" + itemCode + "' AND pricelistCode=0 AND deactivated='N'";

            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count == 1)
            {
                ht["itemCode"] = dt.Rows[0]["itemCode"].ToString();
                ht["description"] = dt.Rows[0]["description"].ToString();
                ht["vatable"] = dt.Rows[0]["vatable"].ToString();
                ht["qtyPrPrchsUoM"] = dt.Rows[0]["qtyPrPrchsUoM"].ToString();
                ht["realBsNetPrchsPrc"] = dt.Rows[0]["realBsNetPrchsPrc"].ToString();
                return ht;
            }
            else if (dt.Rows.Count > 1)
                return ht;
            else
                return ht;
        }

        private Hashtable getVendorInfo(string vendorCode)
        {
            Hashtable ht = new Hashtable();
            sql = "SELECT code,BPName FROM businesspartner WHERE code='" + vendorCode + "' AND BPType=0 AND deactivated='N'";
            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            if (dt.Rows.Count == 1)
            {
                ht["vendorCode"] = dt.Rows[0]["code"].ToString();
                ht["vendorName"] = dt.Rows[0]["BPName"].ToString();
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
                    string dbColumnName, returnedValue;

                    // if selecting vendors
                    if (dgvItems.Columns[e.ColumnIndex].Name == "colVendorCode" || dgvItems.Columns[e.ColumnIndex].Name == "colVendorName")
                    {
                        dbColumnName = getDbColumnName(dgvItems.Columns[e.ColumnIndex].Name);

                        if (dbColumnName == "code")
                        {
                            sql = "SELECT code,BPName FROM businesspartner WHERE BPType=0";

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
                                        dgvItems.Rows[e.RowIndex].Cells["colVendorCode"].Value = returnedValue;

                                        Hashtable ht = new Hashtable(getVendorInfo(returnedValue));
                                        if (ht != null)
                                        {
                                            dgvItems.Rows[e.RowIndex].Cells["colVendorCode"].Value = ht["vendorCode"];
                                            dgvItems.Rows[e.RowIndex].Cells["colVendorName"].Value = ht["vendorName"];
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
                                            dgvItems.Rows[e.RowIndex].Cells["colVendorCode"].Value = returnedValue;

                                            Hashtable ht = new Hashtable(getVendorInfo(returnedValue));
                                            if (ht != null)
                                            {
                                                resetCellValues(e.RowIndex);
                                                dgvItems.Rows[e.RowIndex].Cells["colVendorCode"].Value = ht["vendorCode"];
                                                dgvItems.Rows[e.RowIndex].Cells["colVendorName"].Value = ht["vendorName"];
                                            }
                                        }
                                    }
                                    else if (dt.Rows.Count == 1)
                                    {
                                        dgvItems.Rows[e.RowIndex].Cells["colVendorCode"].Value = dt.Rows[0]["code"].ToString();

                                        Hashtable ht = new Hashtable(getVendorInfo(dt.Rows[0]["code"].ToString()));
                                        if (ht != null)
                                        {
                                            resetCellValues(e.RowIndex);
                                            dgvItems.Rows[e.RowIndex].Cells["colVendorCode"].Value = ht["vendorCode"];
                                            dgvItems.Rows[e.RowIndex].Cells["colVendorName"].Value = ht["vendorName"];
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
                    }
                    else // if selecting items
                    {
                        dbColumnName = getDbColumnName(dgvItems.Columns[e.ColumnIndex].Name);

                        if (dbColumnName == "itemCode" || dbColumnName == "description")
                        {
                            sql = "SELECT itemCode,description,vatable,qtyPrPrchsUoM,netPrice netBscPrchsPrc";
                            sql += " FROM itemmasterdata A JOIN pricelist B USING(itemCode)";
                            sql += " WHERE pricelistCode=0 AND deactivated='N'";

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

                                        Hashtable ht = new Hashtable(getItemInfo(returnedValue));
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

                                            Hashtable ht = new Hashtable(getItemInfo(returnedValue));
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

                                        Hashtable ht = new Hashtable(getItemInfo(dt.Rows[0]["itemCode"].ToString()));
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
                    }                    
                }

                //when column Qty is modified
                if (dgvItems.Columns[e.ColumnIndex].Name == "Qty")
                    computeForQtyCol(e.RowIndex);

                //when prcntDscnt is modified
                if (dgvItems.Columns[e.ColumnIndex].Name == "prcntDscnt")
                    computeForPrcntDscnt(e.RowIndex);

                //when base UoM is modified
                if (dgvItems.Columns[e.ColumnIndex].Name == "baseUoM")
                    computeForBaseUoM(e.RowIndex);

                //when amt discount is modified
                if (dgvItems.Columns[e.ColumnIndex].Name == "amtDscnt")
                    computeForAmtDscnt(e.RowIndex);

                //every edit of the cell must compute other values
                computeDocument();
            }
        }

        private void frmDeliveryReceipt_Load(object sender, EventArgs e)
        {
            // if it's the main warehouse, they're allowed to create AR Invoice for different branches
            sql = "SELECT branchType FROM terminal";
            cmd = new MySqlCommand(sql, vars.MySqlConnection);
            dt = new DataTable();
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows[0][0].ToString() == "Main")
                sql = "SELECT `code` FROM warehouse WHERE deactivated='N'";
            else
                sql = "SELECT whCode AS code FROM terminal A JOIN warehouse B ON A.whCode=B.code WHERE deactivated='N'";

            cmd = new MySqlCommand(sql, vars.MySqlConnection);
            da = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
                cboWarehouse.Items.Add(row["code"]);

            // row warehouse. This is hidden for the meantime.
            sql = "SELECT whCode FROM terminal A JOIN warehouse B ON A.whCode=B.code WHERE deactivated='N'";
            cmd = new MySqlCommand(sql, vars.MySqlConnection);
            da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
                warehouse.Items.Add(row["whCode"]);
        }

        private void setCntrlsToDefaultMode()
        {
            cleanUpUI();
            btnFind.Text = "&Find";
            btnPrint.Enabled = false;
            txtDeliveryReceiptNo.Focus();
        }

        private void setCntrlToAfterSaveMode()
        {
            txtPostingDate.ReadOnly = true;
            dgvItems.AllowUserToAddRows = false;
            btnFind.Text = "&OK";
            btnPrint.Enabled = true;
            cboWarehouse.Enabled = false;
            dateTimePicker1.Enabled = false;
            txtRemarks1.Enabled = false;
        }

        private void setCntrlsToOKMode()
        {
            txtDeliveryReceiptNo.ReadOnly = true;
            txtPostingDate.ReadOnly = true;
            cboWarehouse.Enabled = false;
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
                sql = "SELECT docId,warehouse,DATE_FORMAT(postingDate, '%m/%d/%Y') postingDate,totalPrcntDscnt,totalAmtDscnt,ROUND(netTotal,2) netTotal,ROUND(grossTotal,2) grossTotal,remarks1,remarks2 FROM deliveryreceipt WHERE ";

                if (txtDeliveryReceiptNo.Text.Trim() != "" && !txtDeliveryReceiptNo.Text.Contains("*"))
                {
                    sql += "docId='" + txtDeliveryReceiptNo.Text.Trim().Replace("'", "''") + "'";
                    db = new database();
                    dt = new DataTable();
                    dt = db.select(sql, vars.MySqlConnection);
                    if (dt.Rows.Count < 1)
                    {
                        MessageBox.Show(this, "No matching record found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    txtDeliveryReceiptNo.Text = dt.Rows[0]["docId"].ToString();
                    cboWarehouse.Text = dt.Rows[0]["warehouse"].ToString();
                    txtPostingDate.Text = dt.Rows[0]["postingDate"].ToString();
                    txtRemarks1.Text = dt.Rows[0]["remarks1"].ToString();
                    txtRemarks2.Text = dt.Rows[0]["remarks2"].ToString();

                    decimal d;

                    d = Decimal.Parse(dt.Rows[0]["totalPrcntDscnt"].ToString());
                    txtTotalPrcntDscnt.Text = d.ToString(vars.format);

                    d = Decimal.Parse(dt.Rows[0]["totalAmtDscnt"].ToString());
                    txtTotalAmtDscnt.Text = d.ToString(vars.format);

                    d = Decimal.Parse(dt.Rows[0]["netTotal"].ToString());
                    txtNetTotal.Text = d.ToString(vars.format);

                    d = Decimal.Parse(dt.Rows[0]["grossTotal"].ToString());
                    txtGrossTotal.Text = d.ToString(vars.format);

                    sql = "SELECT * FROM deliveryreceipt_item WHERE docId='" + txtDeliveryReceiptNo.Text.Trim() + "' ORDER BY indx";
                    db = new database();
                    dt = db.select(sql, vars.MySqlConnection);
                    rowCount = dt.Rows.Count;
                    for (i = 0; i < rowCount; i++)
                    {
                        dgvItems.Rows.Add();
                        dgvItems.Rows[i].Cells["colVendorCode"].Value = dt.Rows[i]["vendorCode"].ToString();
                        dgvItems.Rows[i].Cells["colVendorName"].Value = dt.Rows[i]["vendorName"].ToString();
                        dgvItems.Rows[i].Cells["itemCode"].Value = dt.Rows[i]["itemCode"].ToString();
                        dgvItems.Rows[i].Cells["description"].Value = dt.Rows[i]["description"].ToString();
                        dgvItems.Rows[i].Cells["vatable"].Value = dt.Rows[i]["vatable"].ToString();
                        dgvItems.Rows[i].Cells["realBsNetPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["realBsNetPrchsPrc"].ToString()).ToString(vars.format);

                        d = Decimal.Parse(dt.Rows[i]["Qty"].ToString());
                        dgvItems.Rows[i].Cells["Qty"].Value = d.ToString(vars.format);

                        dgvItems.Rows[i].Cells["qtyPrPrchsUoM"].Value = Decimal.Parse(dt.Rows[i]["qtyPrPrchsUoM"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["baseUoM"].Value = dt.Rows[i]["baseUoM"].ToString();

                        d = Decimal.Parse(dt.Rows[i]["realBsGrossPrchsPrc"].ToString());
                        dgvItems.Rows[i].Cells["realBsGrossPrchsPrc"].Value = d.ToString(vars.format);

                        d = Decimal.Parse(dt.Rows[i]["realNetPrchsPrc"].ToString());
                        dgvItems.Rows[i].Cells["realNetPrchsPrc"].Value = d.ToString(vars.format);

                        d = Decimal.Parse(dt.Rows[i]["realGrossPrchsPrc"].ToString());
                        dgvItems.Rows[i].Cells["realGrossPrchsPrc"].Value = d.ToString(vars.format);

                        dgvItems.Rows[i].Cells["prcntDscnt"].Value = Decimal.Parse(dt.Rows[i]["prcntDscnt"].ToString()).ToString(vars.format);

                        d = Decimal.Parse(dt.Rows[i]["grossPrchsPrc"].ToString());
                        dgvItems.Rows[i].Cells["grossPrchsPrc"].Value = d.ToString(vars.format);

                        d = Decimal.Parse(dt.Rows[i]["netPrchsPrc"].ToString());
                        dgvItems.Rows[i].Cells["netPrchsPrc"].Value = d.ToString(vars.format);

                        d = Decimal.Parse(dt.Rows[i]["amtDscnt"].ToString());
                        dgvItems.Rows[i].Cells["amtDscnt"].Value = d.ToString(vars.format);

                        dgvItems.Rows[i].Cells["rowNetTotal"].Value = Decimal.Parse(dt.Rows[i]["rowNetTotal"].ToString()).ToString(vars.format);

                        d = Decimal.Parse(dt.Rows[i]["rowGrossTotal"].ToString());
                        dgvItems.Rows[i].Cells["rowGrossTotal"].Value = d.ToString(vars.grossFormat);
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
                        txtDeliveryReceiptNo.Focus();
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
                    txtDeliveryReceiptNo.Text = frmDialogForm.selectedValue;
                }
            }
            else if (btnFind.Text == "&OK")
            {
                this.Close();
            }
            else if (btnFind.Text == "&Update")
            {
                db = new database();
                sql = "UPDATE grpo SET remarks2='" + txtRemarks2.Text.Trim().Replace("'", "''") + "',updateDate=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s'),updatedBy='" + vars.username + "' WHERE docId='" + txtDeliveryReceiptNo.Text.Trim() + "'";

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

                    header = new Hashtable();
                    header.Add("terminalId", vars.terminalId);
                    header.Add("warehouse", cboWarehouse.Text);
                    
                    DateTime dateTime = DateTime.Parse(txtPostingDate.Text.Trim());
                    header.Add("postingDate", dateTime.ToString("yyyy/MM/dd"));

                    header.Add("totalPrcntDscnt", txtTotalPrcntDscnt.Text.Trim());
                    header.Add("totalAmtDscnt", txtTotalAmtDscnt.Text.Trim());
                    header.Add("netTotal", txtNetTotal.Text.Trim());
                    header.Add("grossTotal", txtGrossTotal.Text.Trim());
                    header.Add("remarks1", txtRemarks1.Text.Trim());
                    header.Add("remarks2", txtRemarks2.Text.Trim());
                    header.Add("createdBy", vars.username);

                    table = new DataTable();
                    table.Columns.Add("indx");
                    table.Columns.Add("vendorCode");
                    table.Columns.Add("vendorName");
                    table.Columns.Add("itemCode");
                    table.Columns.Add("description");
                    table.Columns.Add("warehouseRow");
                    table.Columns.Add("vatable");
                    table.Columns.Add("realBsNetPrchsPrc");
                    table.Columns.Add("realBsGrossPrchsPrc");
                    table.Columns.Add("realNetPrchsPrc");
                    table.Columns.Add("realGrossPrchsPrc");
                    table.Columns.Add("qty");
                    table.Columns.Add("baseUoM");
                    table.Columns.Add("qtyPrPrchsUoM");
                    table.Columns.Add("prcntDscnt");
                    table.Columns.Add("amtDscnt");
                    table.Columns.Add("netPrchsPrc");
                    table.Columns.Add("grossPrchsPrc");
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
                                row["vendorCode"] = dgvItems.Rows[i].Cells["colVendorCode"].Value.ToString();

                                row["vendorName"] = dgvItems.Rows[i].Cells["colVendorName"].Value.ToString();
                                row["vendorName"] = row["vendorName"].ToString().Replace("'", "''");

                                row["itemCode"] = dgvItems.Rows[i].Cells["itemCode"].Value.ToString();
                                row["description"] = dgvItems.Rows[i].Cells["description"].Value.ToString();
                                row["qty"] = Decimal.Parse(dgvItems.Rows[i].Cells["Qty"].Value.ToString());
                                row["vatable"] = dgvItems.Rows[i].Cells["vatable"].Value.ToString();
                                row["qtyPrPrchsUoM"] = Decimal.Parse(dgvItems.Rows[i].Cells["qtyPrPrchsUoM"].Value.ToString());
                                row["baseUoM"] = fx.null2EmptyStr(dgvItems.Rows[i].Cells["baseUoM"].Value);
                                row["realBsNetPrchsPrc"] = Decimal.Parse(dgvItems.Rows[i].Cells["realBsNetPrchsPrc"].Value.ToString());
                                row["realBsGrossPrchsPrc"] = Decimal.Parse(dgvItems.Rows[i].Cells["realBsGrossPrchsPrc"].Value.ToString());
                                row["realNetPrchsPrc"] = Decimal.Parse(dgvItems.Rows[i].Cells["realNetPrchsPrc"].Value.ToString());
                                row["realGrossPrchsPrc"] = Decimal.Parse(dgvItems.Rows[i].Cells["realGrossPrchsPrc"].Value.ToString());
                                row["prcntDscnt"] = Decimal.Parse(dgvItems.Rows[i].Cells["prcntDscnt"].Value.ToString());
                                row["grossPrchsPrc"] = Decimal.Parse(dgvItems.Rows[i].Cells["grossPrchsPrc"].Value.ToString());
                                row["netPrchsPrc"] = Decimal.Parse(dgvItems.Rows[i].Cells["netPrchsPrc"].Value.ToString());
                                row["amtDscnt"] = Decimal.Parse(dgvItems.Rows[i].Cells["amtDscnt"].Value.ToString());
                                row["rowNetTotal"] = Decimal.Parse(dgvItems.Rows[i].Cells["rowNetTotal"].Value.ToString());
                                row["rowGrossTotal"] = Decimal.Parse(dgvItems.Rows[i].Cells["rowGrossTotal"].Value.ToString());

                                // for the meantime we just deduct the qty from warehouse in the header so we just override it.
                                row["warehouseRow"] = header["warehouse"];

                                table.Rows.Add(row);
                            } // end if
                        } //end for
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.ToString());
                        return;
                    }
                    
                    DeliveryReceipt deliveryReceipt = new DeliveryReceipt();
                    if (deliveryReceipt.addDeliveryReceipt(header, table))
                    {
                        MessageBox.Show(this, "Saving has been successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        db = new database(); dt = new DataTable();
                        dt = db.select("SELECT docId FROM deliveryreceipt ORDER BY id DESC LIMIT 1", vars.MySqlConnection);
                        txtDeliveryReceiptNo.Text = dt.Rows[0][0].ToString();
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

            table.Rows.Add("txtDeliveryReceiptNo", "docId", "");
            table.Rows.Add("txtRemarks1", "remarks1", "");
            table.Rows.Add("txtRemarks2", "remarks2", "");
        }

        private bool checkValues()
        {
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
                        d = Decimal.Parse(dgvItems.Rows[i].Cells["Qty"].Value.ToString());
                        d = Decimal.Parse(dgvItems.Rows[i].Cells["prcntDscnt"].Value.ToString());
                    }
                    catch
                    {
                        MessageBox.Show(this, "Invalid amount found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            //check if there are double entries
            ArrayList itemCodeArray = new ArrayList();
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
            rowCount = (dgvItems.Rows.Count - 1);
            for (i = 0; i < rowCount; i++)
            {
                itemCode = fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value);
                db = new database(); dt = new DataTable();
                sql = "SELECT deactivated, maxStock FROM itemmasterdata WHERE itemCode='" + itemCode + "'";
                dt = db.select(sql, vars.MySqlConnection);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["deactivated"].ToString() == "Y")
                    {
                        MessageBox.Show(this, "Item code " + itemCode + " is deactivated. Cannot proceed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                        return false;
                    }

                    decimal varQty, varBaseQty, varQtyPrPrchsUoM, varMaxStock;
                    string varBaseUoM;

                    varQty = Decimal.Parse(dgvItems.Rows[i].Cells["Qty"].Value.ToString());
                    varQtyPrPrchsUoM = Decimal.Parse(dgvItems.Rows[i].Cells["qtyPrPrchsUoM"].Value.ToString());
                    varBaseUoM = dgvItems.Rows[i].Cells["baseUoM"].Value.ToString();

                    varBaseQty = ((varBaseUoM == "N") ? varQtyPrPrchsUoM * varQty : varQty);
                    varMaxStock = Decimal.Parse(dt.Rows[0]["maxStock"].ToString());

                    if ((varMaxStock > 0) && (varBaseQty > varMaxStock))
                    {
                        DialogResult result = MessageBox.Show(this, "Maximum stock level for item " + itemCode + " has reached. Do you still want to proceed?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                            return false;
                    }
                }
                else
                {
                    MessageBox.Show(this, "Item code " + itemCode + " is not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                    return false;
                }
            }

            //check header warehouse
            if (cboWarehouse.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Please select warehouse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                cboWarehouse.Focus();
                return false;
            }

            // if vendor code column is not empty
            for (i = 0; i < rowCount; i++ )
            {
                if (dgvItems.Rows[i].Cells["colVendorCode"].Value.ToString().Trim() == "")
                {
                    MessageBox.Show(this, "No Vendor Code at row #" + i++, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txtDeliveryReceiptNo.Text = "";
            dateTimePicker1.Enabled = true;
            cboWarehouse.Enabled = true; cboWarehouse.SelectedIndex = -1;
            txtPostingDate.Text = "";
            txtDeliveryReceiptNo.ReadOnly = false;
            txtPostingDate.ReadOnly = false;
            txtRemarks1.ReadOnly = false;
            dgvItems.Rows.Clear();
            dgvItems.AllowUserToAddRows = false;
            txtRemarks1.Text = ""; txtRemarks2.Text = "";
            txtDeliveryReceiptNo.Focus();

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
            dgvItems.ReadOnly = false;
            dgvItems.AllowUserToAddRows = true;
            txtDeliveryReceiptNo.ReadOnly = true;
            cboWarehouse.Focus();
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
                {
                    if (MessageBox.Show(this, "Are you sure you want to cancel this transaction?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        setCntrlsToDefaultMode();
                }
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
            computeDocument();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            crystal_reports.DeliveryReceipt objReport;
            this.Cursor = Cursors.WaitCursor;

            sql = "SELECT docId, A.warehouse, DATE_FORMAT(postingDate, '%m/%d/%Y') postingDate, totalPrcntDscnt, totalAmtDscnt, netTotal,grossTotal,remarks1,remarks2,B.* ,C.prchsUoM  FROM deliveryreceipt A JOIN deliveryreceipt_item B USING(docId) JOIN itemmasterdata C USING(itemCode) WHERE docId='" + txtDeliveryReceiptNo.Text.Trim() + "' ORDER BY indx";
            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);

            objReport = new crystal_reports.DeliveryReceipt();
            objReport.SetDataSource(dt);

            crystalReportViewerForm = new frmCrystalReportViewer();
            crystalReportViewerForm.CrystalReportViewer1.ReportSource = objReport;

            crystalReportViewerForm.MdiParent = this.MdiParent;
            crystalReportViewerForm.Show();
            crystalReportViewerForm.Text = "Delivery Receipt Report";
            this.Cursor = Cursors.Default;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            txtPostingDate.Text = dateTimePicker1.Value.ToString("MM/dd/yyyy");
        }

    }
}
