using System;
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
    public partial class frmInventoryTransfer : Form
    {
        DataTable table; DataTable dt;
        MySqlCommand cmd; MySqlDataAdapter da;
        database db = new database();
        private frmDialog frmDialogForm;
        private frmCrystalReportViewer crystalReportViewerForm;
        string sql;
        int i, rowCount;

        public frmInventoryTransfer()
        {
            InitializeComponent();
            dgvItems.CellEndEdit += new DataGridViewCellEventHandler(dgvItems_CellEndEdit);
        }

        private void resetCellValues(int rowIndex)
        {
            // retail prices
            dgvItems.Rows[rowIndex].Cells["netPrcRtl"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["grossPrcRtl"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["prcntDscntRtl"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["amtDscntRtl"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["rowNetTotalRtl"].Value = "0.00";
            dgvItems.Rows[rowIndex].Cells["rowGrossTotalRtl"].Value = "0.00";

            // purchase prices
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
            sql = "SELECT itemCode,description,vatable,qtyPrPrchsUoM, qtyPrSaleUoM 'Qty Per Retail UoM',netPrice netBscPrchsPrc";
            sql += " ,(select netPrice from pricelist where pricelist.itemCode=A.itemCode and pricelistCode=1) 'Net Retail Price Per PC'";
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
                ht["qtyPrPrchsUoM"] = dt.Rows[0]["qtyPrPrchsUoM"];
                ht["realBsNetPrchsPrc"] = dt.Rows[0]["netBscPrchsPrc"];
                ht["netPricePerPcRtl"] = dt.Rows[0]["Net Retail Price Per PC"];
                ht["qtyPrRtlUoM"] = dt.Rows[0]["Qty Per Retail UoM"];
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

            // purchase info
            dgvItems.Rows[rowIndex].Cells["qtyPrPrchsUoM"].Value = Decimal.Parse(ht["qtyPrPrchsUoM"].ToString()).ToString(vars.format);
            dgvItems.Rows[rowIndex].Cells["realBsNetPrchsPrc"].Value = Decimal.Parse(ht["realBsNetPrchsPrc"].ToString()).ToString(vars.format);

            // retail info
            dgvItems.Rows[rowIndex].Cells["qtyPrRtlUoM"].Value = Decimal.Parse(ht["qtyPrRtlUoM"].ToString()).ToString(vars.format);
            dgvItems.Rows[rowIndex].Cells["realBsNetPrcRtl"].Value = Decimal.Parse(ht["netPricePerPcRtl"].ToString()).ToString(vars.format);
        }

        private void computeForBaseUoM(int rowIndex)
        {
            dgvItems.Rows[rowIndex].Cells["prcntDscntRtl"].Value = 0.00;
            dgvItems.Rows[rowIndex].Cells["amtDscntRtl"].Value = 0.00;

            dgvItems.Rows[rowIndex].Cells["prcntDscnt"].Value = 0.00;
            dgvItems.Rows[rowIndex].Cells["amtDscnt"].Value = 0.00;
            computeForQtyCol(rowIndex);
        }

        private void computeForQtyCol(int rowIndex)
        {
            if (itemCodeIsFound(fx.null2EmptyStr(dgvItems.Rows[rowIndex].Cells["itemCode"].Value)))
            {
                string varVatable, varBaseUoM;
                decimal varQty, varRealBsNetPrchsPrc, varRealBsGrossPrchsPrc, varQtyPrPrchsUoM, varRealNetPrchsPrc, varRealGrossPrchsPrc, varGrossPrchsPrc, varRowNetTotal, varRowGrossTotal, varNetPrchsPrc;
                decimal qtyPrRtlUoM, realBsNetRtlPrc, realBsGrossRtlPrc, realNetRtlPrc, realGrossRtlPrc, netRtlPrc, grossRtlPrc, rowNetRtlTotal, rowGrossRtlTotal;

                try
                {
                    varQty = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["Qty"].Value.ToString());
                    varVatable = dgvItems.Rows[rowIndex].Cells["vatable"].Value.ToString();
                    varRealBsNetPrchsPrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["realBsNetPrchsPrc"].Value.ToString());
                    varQtyPrPrchsUoM = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["qtyPrPrchsUoM"].Value.ToString());
                    varBaseUoM = dgvItems.Rows[rowIndex].Cells["baseUoM"].Value.ToString();

                    qtyPrRtlUoM = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["qtyPrRtlUoM"].Value.ToString());
                    realBsNetRtlPrc = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["realBsNetPrcRtl"].Value.ToString());
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // retail prices
                realBsGrossRtlPrc = realBsNetRtlPrc * ((varVatable == "Y") ? 1.12m : 1);
                realNetRtlPrc = realBsNetRtlPrc * ((varBaseUoM == "Y") ? 1 : qtyPrRtlUoM);
                realGrossRtlPrc = realNetRtlPrc * ((varVatable == "Y") ? 1.12m : 1);
                netRtlPrc = realNetRtlPrc * varQty;
                grossRtlPrc = realGrossRtlPrc * varQty;
                rowGrossRtlTotal = grossRtlPrc;
                rowNetRtlTotal = netRtlPrc;

                // retail prices
                dgvItems.Rows[rowIndex].Cells["realBsGrossPrcRtl"].Value = realBsGrossRtlPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["realNetPrcRtl"].Value = realNetRtlPrc.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["realGrossPrcRtl"].Value = realGrossRtlPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["grossPrcRtl"].Value = grossRtlPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["netPrcRtl"].Value = netRtlPrc.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["prcntDscntRtl"].Value = 0.00;
                dgvItems.Rows[rowIndex].Cells["amtDscntRtl"].Value = 0.00;
                dgvItems.Rows[rowIndex].Cells["rowNetTotalRtl"].Value = rowNetRtlTotal.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["rowGrossTotalRtl"].Value = rowGrossRtlTotal.ToString(vars.grossFormat);

                //purchase prices
                varRealBsGrossPrchsPrc = varRealBsNetPrchsPrc * ((varVatable == "Y") ? 1.12m : 1);
                varRealNetPrchsPrc = varRealBsNetPrchsPrc * ((varBaseUoM == "Y") ? 1 : varQtyPrPrchsUoM);
                varRealGrossPrchsPrc = varRealNetPrchsPrc * ((varVatable == "Y") ? 1.12m : 1);
                varNetPrchsPrc = varRealNetPrchsPrc * varQty;
                varGrossPrchsPrc = varRealGrossPrchsPrc * varQty;
                varRowGrossTotal = varGrossPrchsPrc;
                varRowNetTotal = varNetPrchsPrc;

                //purchase prices
                dgvItems.Rows[rowIndex].Cells["realBsGrossPrchsPrc"].Value = varRealBsGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["realNetPrchsPrc"].Value = varRealNetPrchsPrc.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["realGrossPrchsPrc"].Value = varRealGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["grossPrchsPrc"].Value = varGrossPrchsPrc.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["netPrchsPrc"].Value = varNetPrchsPrc.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["prcntDscnt"].Value = 0.00;
                dgvItems.Rows[rowIndex].Cells["amtDscnt"].Value = 0.00;
                dgvItems.Rows[rowIndex].Cells["rowNetTotal"].Value = varRowNetTotal.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["rowGrossTotal"].Value = varRowGrossTotal.ToString(vars.grossFormat);
            }
        }

        private void computeForPrcntDscntRtl(int rowIndex)
        {
            if (itemCodeIsFound(fx.null2EmptyStr(dgvItems.Rows[rowIndex].Cells["itemCode"].Value)))
            {
                string varVatable, varBaseUoM;
                decimal varQty, varRealBsNetPrcRtl, varRealBsGrossPrcRtl, varQtyPrUoMRtl, varRealNetPrcRtl, varRealGrossPrcRtl, varGrossPrcRtl, varRowNetTotalRtl, varRowGrossTotalRtl, varNetPrcRtl, varPrcntDscntRtl, varAmtDscntRtl;

                try
                {
                    varPrcntDscntRtl = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["prcntDscntRtl"].Value.ToString());
                    varQty = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["Qty"].Value.ToString());
                    varVatable = dgvItems.Rows[rowIndex].Cells["vatable"].Value.ToString();
                    varRealBsNetPrcRtl = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["realBsNetPrcRtl"].Value.ToString());
                    varQtyPrUoMRtl = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["qtyPrRtlUoM"].Value.ToString());
                    varBaseUoM = dgvItems.Rows[rowIndex].Cells["baseUoM"].Value.ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                varRealBsGrossPrcRtl = varRealBsNetPrcRtl * ((varVatable == "Y") ? 1.12m : 1);
                varRealNetPrcRtl = varRealBsNetPrcRtl * ((varBaseUoM == "Y") ? 1 : varQtyPrUoMRtl);
                varRealGrossPrcRtl = varRealNetPrcRtl * ((varVatable == "Y") ? 1.12m : 1);
                varNetPrcRtl = varRealNetPrcRtl * varQty;
                varGrossPrcRtl = varRealGrossPrcRtl * varQty;
                varRowGrossTotalRtl = varGrossPrcRtl * (1 - (varPrcntDscntRtl / 100));
                varRowNetTotalRtl = varRowGrossTotalRtl / ((varVatable == "Y") ? 1.12m : 1);
                varAmtDscntRtl = varGrossPrcRtl - varRowGrossTotalRtl;

                dgvItems.Rows[rowIndex].Cells["realBsGrossPrcRtl"].Value = varRealBsGrossPrcRtl.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["realNetPrcRtl"].Value = varRealNetPrcRtl.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["realGrossPrcRtl"].Value = varRealGrossPrcRtl.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["grossPrcRtl"].Value = varGrossPrcRtl.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["netPrcRtl"].Value = varNetPrcRtl.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["amtDscntRtl"].Value = varAmtDscntRtl.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["rowNetTotalRtl"].Value = varRowNetTotalRtl.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["rowGrossTotalRtl"].Value = varRowGrossTotalRtl.ToString(vars.grossFormat);
            }
        }

        private void computeForAmtDscntRtl(int rowIndex)
        {
            if (itemCodeIsFound(fx.null2EmptyStr(dgvItems.Rows[rowIndex].Cells["itemCode"].Value)))
            {
                string varVatable, varBaseUoM;
                decimal varQty, varRealBsNetPrcRtl, varRealBsGrossPrcRtl, varQtyPrRtlUoM, varRealNetPrcRtl, varRealGrossPrcRtl, varGrossPrcRtl, varRowNetTotal, varRowGrossTotal, varNetPrcRtl, varPrcntDscntRtl, varAmtDscntRtl;

                try
                {
                    varAmtDscntRtl = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["amtDscntRtl"].Value.ToString());
                    varQty = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["Qty"].Value.ToString());
                    varVatable = dgvItems.Rows[rowIndex].Cells["vatable"].Value.ToString();
                    varRealBsNetPrcRtl = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["realBsNetPrcRtl"].Value.ToString());
                    varQtyPrRtlUoM = Decimal.Parse(dgvItems.Rows[rowIndex].Cells["qtyPrRtlUoM"].Value.ToString());
                    varBaseUoM = dgvItems.Rows[rowIndex].Cells["baseUoM"].Value.ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                varRealBsGrossPrcRtl = varRealBsNetPrcRtl * ((varVatable == "Y") ? 1.12m : 1);
                varRealNetPrcRtl = varRealBsNetPrcRtl * ((varBaseUoM == "Y") ? 1 : varQtyPrRtlUoM);
                varRealGrossPrcRtl = varRealNetPrcRtl * ((varVatable == "Y") ? 1.12m : 1);
                varNetPrcRtl = varRealNetPrcRtl * varQty;
                varGrossPrcRtl = varRealGrossPrcRtl * varQty;
                varRowGrossTotal = varGrossPrcRtl - varAmtDscntRtl;
                varRowNetTotal = (varVatable == "Y") ? (varRowGrossTotal / 1.12m) : varRowGrossTotal;
                varPrcntDscntRtl = (varAmtDscntRtl / varGrossPrcRtl) * 100;

                dgvItems.Rows[rowIndex].Cells["realBsGrossPrcRtl"].Value = varRealBsGrossPrcRtl.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["realNetPrcRtl"].Value = varRealNetPrcRtl.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["realGrossPrcRtl"].Value = varRealGrossPrcRtl.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["grossPrcRtl"].Value = varGrossPrcRtl.ToString(vars.grossFormat);
                dgvItems.Rows[rowIndex].Cells["netPrcRtl"].Value = varNetPrcRtl.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["prcntDscntRtl"].Value = varPrcntDscntRtl.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["rowNetTotalRtl"].Value = varRowNetTotal.ToString(vars.format);
                dgvItems.Rows[rowIndex].Cells["rowGrossTotalRtl"].Value = varRowGrossTotal.ToString(vars.grossFormat);
            }
        }

        private void computeForPrcntPrchsDscnt(int rowIndex)
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

        private void computeForAmtPrchsDscnt(int rowIndex)
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
            // retail prices
            computeDocAmtDscntRtl();
            computeDocNetTotalRtl();
            computeDocGrossTotalRtl();
            computeDocPrcntDscntRtl();
            
            // purchase prices
            computeDocAmtPrchsDscnt();
            computeDocNetPrchsTotal();
            computeDocGrossPrhcsTotal();
            computeDocPrcntPrchsDscnt();

            computeItemCount();
        }

        private void computeDocPrcntDscntRtl()
        {
            decimal prcntDscnt = 0; int cnt = 0;
            rowCount = dgvItems.Rows.Count;
            for (i = 0; i < rowCount; i++)
            {
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value) != "")
                {
                    prcntDscnt = prcntDscnt + fx.null2Decimal(dgvItems.Rows[i].Cells["prcntDscntRtl"].Value);
                    cnt++;
                }
            }

            if (cnt == 0)
                txtTotalPrcntDscntRtl.Text = "0";
            else
                txtTotalPrcntDscntRtl.Text = (prcntDscnt / cnt).ToString(vars.format);
        }
        private void computeDocAmtDscntRtl()
        {
            decimal amtDscnt = 0.00m;
            rowCount = dgvItems.Rows.Count;
            for (i = 0; i < rowCount; i++)
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value) != "")
                    amtDscnt = amtDscnt + fx.null2Decimal(dgvItems.Rows[i].Cells["amtDscntRtl"].Value);

            txtTotalAmtDscntRtl.Text = amtDscnt.ToString(vars.format);
        }
        private void computeDocNetTotalRtl()
        {
            decimal netTotal = 0.00m;
            rowCount = dgvItems.Rows.Count;
            for (i = 0; i < rowCount; i++)
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value) != "")
                    netTotal = netTotal + fx.null2Decimal(dgvItems.Rows[i].Cells["rowNetTotalRtl"].Value);

            txtNetTotalRtl.Text = netTotal.ToString(vars.format);
        }
        private void computeDocGrossTotalRtl()
        {
            decimal grossTotal = 0.00m;
            rowCount = dgvItems.Rows.Count;
            for (i = 0; i < rowCount; i++)
            {
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value) != "")
                    grossTotal = grossTotal + fx.null2Decimal(dgvItems.Rows[i].Cells["rowGrossTotalRtl"].Value);
            }

            txtGrossTotalRtl.Text = grossTotal.ToString(vars.grossFormat);
        }
        
        private void computeDocPrcntPrchsDscnt()
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
                txtTotalPrcntPrchsDscnt.Text = "0";
            else
                txtTotalPrcntPrchsDscnt.Text = (prcntDscnt / cnt).ToString(vars.format);
        }
        private void computeDocAmtPrchsDscnt()
        {
            decimal amtDscnt = 0.00m;
            rowCount = dgvItems.Rows.Count;
            for (i = 0; i < rowCount; i++)
            {
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value) != "")
                    amtDscnt = amtDscnt + fx.null2Decimal(dgvItems.Rows[i].Cells["amtDscnt"].Value);
            }

            txtTotalAmtPrchsDscnt.Text = amtDscnt.ToString(vars.format);
        }
        private void computeDocNetPrchsTotal()
        {
            decimal netTotal = 0.00m;
            rowCount = dgvItems.Rows.Count;
            for (i = 0; i < rowCount; i++)
            {
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value) != "")
                    netTotal = netTotal + fx.null2Decimal(dgvItems.Rows[i].Cells["rowNetTotal"].Value);
            }

            txtNetPrchsTotal.Text = netTotal.ToString(vars.format);
        }
        private void computeDocGrossPrhcsTotal()
        {
            decimal grossTotal = 0.00m;
            rowCount = dgvItems.Rows.Count;
            for (i = 0; i < rowCount; i++)
            {
                if (fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value) != "")
                    grossTotal = grossTotal + fx.null2Decimal(dgvItems.Rows[i].Cells["rowGrossTotal"].Value);
            }

            txtGrossPrchsTotal.Text = grossTotal.ToString(vars.grossFormat);
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
                        sql = "SELECT itemCode,description,vatable,qtyPrPrchsUoM, qtyPrSaleUoM 'Qty Per Retail UoM',netPrice netBscPrchsPrc";
                        sql += " ,(select netPrice from pricelist where pricelist.itemCode=A.itemCode and pricelistCode=1) 'Net Retail Price Per PC'";
                        sql += " FROM itemmasterdata A JOIN pricelist B USING(itemCode)";
                        sql += " WHERE pricelistCode=0 AND deactivated='N'";
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
                        computeForPrcntPrchsDscnt(e.RowIndex);

                    //when amtDscnt is modified
                    if (dgvItems.Columns[e.ColumnIndex].Name == "amtDscnt")
                        computeForAmtPrchsDscnt(e.RowIndex);

                    // when column prcntDscntRtl is modified
                    if (dgvItems.Columns[e.ColumnIndex].Name == "prcntDscntRtl")
                        computeForPrcntDscntRtl(e.RowIndex);

                    // when column amtDscntRtl is modified
                    if (dgvItems.Columns[e.ColumnIndex].Name == "amtDscntRtl")
                        computeForAmtDscntRtl(e.RowIndex);
                }

                //every edit of the cell must compute other values
                computeDocument();
                computeItemCount();
            }
        }

        private void setToDefaultState()
        {
            cleanUpUI();
            btnFind.Text = "&Find";
            btnPrint.Enabled = false;
            txtInventoryTransferNo.Focus();
        }

        private void setToAfterSaveState()
        {
            txtPostingDate.ReadOnly = true;
            cboComboFrom.Enabled = false; cboComboTo.Enabled = false;
            dateTimePicker1.Enabled = false;
            dgvItems.ReadOnly = true;
            dgvItems.AllowUserToAddRows = false;
            txtRemarks1.Enabled = false;
            btnFind.Text = "&OK";
            btnPrint.Enabled = true;
        }

        private void setCntrlToOKState()
        {
            txtInventoryTransferNo.ReadOnly = true;
            txtPostingDate.ReadOnly = true;
            dateTimePicker1.Enabled = false;
            txtRemarks1.ReadOnly = true;
            dgvItems.ReadOnly = true;

            btnFind.Text = "&OK";
            btnPrint.Enabled = true;
            cboComboFrom.Enabled = false;
            cboComboTo.Enabled = false;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (btnFind.Text == "&Find")
            {
                //sql = "SELECT docId,frmWHouse,toWHouse,DATE_FORMAT(postingDate, '%m/%d/%Y') postingDate,totalPrcntDscnt,totalAmtDscnt,ROUND(netTotal,2) netTotal,ROUND(grossTotal,2) grossTotal,remarks1,remarks2 FROM inventorytransfer WHERE ";
                sql = "SELECT docId,frmWHouse,toWHouse,DATE_FORMAT(postingDate, '%m/%d/%Y') postingDate, totalPrcntDscnt 'Total Discount in %',totalAmtDscnt 'Total Discount in Amt',ROUND(netTotal,2) 'Net Total',ROUND(grossTotal,2) 'Gross Total', totalPrcntDscntRtl,totalAmtDscntRtl,ROUND(netTotalRtl,2) netTotalRtl,ROUND(grossTotalRtl,2) grossTotalRtl,remarks1,remarks2 FROM inventorytransfer WHERE ";

                if (txtInventoryTransferNo.Text.Trim() != "" && !txtInventoryTransferNo.Text.Contains("*"))
                {
                    sql += "docId='" + txtInventoryTransferNo.Text.Trim().Replace("'", "''") + "'";
                    dt = new DataTable();
                    cmd = new MySqlCommand(sql, vars.MySqlConnection);
                    da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count < 1)
                    {
                        MessageBox.Show(this, "No matching record found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    txtInventoryTransferNo.Text = dt.Rows[0]["docId"].ToString();
                    cboComboFrom.Text = dt.Rows[0]["frmWHouse"].ToString();
                    cboComboTo.Text = dt.Rows[0]["toWHouse"].ToString();
                    txtPostingDate.Text = dt.Rows[0]["postingDate"].ToString();
                    txtRemarks1.Text = dt.Rows[0]["remarks1"].ToString();
                    txtRemarks2.Text = dt.Rows[0]["remarks2"].ToString();

                    //retail prices
                    txtTotalPrcntDscntRtl.Text = Decimal.Parse(dt.Rows[0]["totalPrcntDscntRtl"].ToString()).ToString(vars.format);
                    txtTotalAmtDscntRtl.Text = Decimal.Parse(dt.Rows[0]["totalAmtDscntRtl"].ToString()).ToString(vars.format);
                    txtNetTotalRtl.Text = Decimal.Parse(dt.Rows[0]["netTotalRtl"].ToString()).ToString(vars.format);
                    txtGrossTotalRtl.Text = Decimal.Parse(dt.Rows[0]["grossTotalRtl"].ToString()).ToString(vars.format);
                    
                    // purchase prices
                    txtTotalPrcntPrchsDscnt.Text = Decimal.Parse(dt.Rows[0]["Total Discount in %"].ToString()).ToString(vars.format);
                    txtTotalAmtPrchsDscnt.Text = Decimal.Parse(dt.Rows[0]["Total Discount in Amt"].ToString()).ToString(vars.format);
                    txtNetPrchsTotal.Text = Decimal.Parse(dt.Rows[0]["Net Total"].ToString()).ToString(vars.format);
                    txtGrossPrchsTotal.Text = Decimal.Parse(dt.Rows[0]["Gross Total"].ToString()).ToString(vars.format);

                    sql = "SELECT * FROM inventorytransfer_item WHERE docId='" + txtInventoryTransferNo.Text.Trim() + "' ORDER BY indx";
                    db = new database();
                    dt = db.select(sql, vars.MySqlConnection);
                    rowCount = dt.Rows.Count;
                    for (i = 0; i < rowCount; i++)
                    {
                        dgvItems.Rows.Add();
                        dgvItems.Rows[i].Cells["itemCode"].Value = dt.Rows[i]["itemCode"].ToString();
                        dgvItems.Rows[i].Cells["description"].Value = dt.Rows[i]["description"].ToString();
                        dgvItems.Rows[i].Cells["vatable"].Value = dt.Rows[i]["vatable"].ToString();
                        dgvItems.Rows[i].Cells["Qty"].Value = Decimal.Parse(dt.Rows[i]["Qty"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["baseUoM"].Value = dt.Rows[i]["baseUoM"].ToString();

                        //retail prices
                        dgvItems.Rows[i].Cells["qtyPrRtlUoM"].Value = Decimal.Parse(dt.Rows[i]["qtyPrRtlUoM"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["realBsGrossPrcRtl"].Value = Decimal.Parse(dt.Rows[i]["realBsGrossPrcRtl"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["realNetPrcRtl"].Value = Decimal.Parse(dt.Rows[i]["realNetPrcRtl"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["realGrossPrcRtl"].Value = Decimal.Parse(dt.Rows[i]["realGrossPrcRtl"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["prcntDscntRtl"].Value = Decimal.Parse(dt.Rows[i]["prcntDscntRtl"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["grossPrcRtl"].Value = Decimal.Parse(dt.Rows[i]["grossPrcRtl"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["netPrcRtl"].Value = Decimal.Parse(dt.Rows[i]["netPrcRtl"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["amtDscntRtl"].Value = Decimal.Parse(dt.Rows[i]["amtDscntRtl"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["rowNetTotalRtl"].Value = Decimal.Parse(dt.Rows[i]["rowNetTotalRtl"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["rowGrossTotalRtl"].Value = Decimal.Parse(dt.Rows[i]["rowGrossTotalRtl"].ToString()).ToString(vars.grossFormat);

                        // purchase prices
                        dgvItems.Rows[i].Cells["realBsNetPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["realBsNetPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["qtyPrPrchsUoM"].Value = Decimal.Parse(dt.Rows[i]["qtyPrPrchsUoM"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["realBsGrossPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["realBsGrossPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["realNetPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["realNetPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["realGrossPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["realGrossPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["prcntDscnt"].Value = Decimal.Parse(dt.Rows[i]["prcntDscnt"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["grossPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["grossPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["netPrchsPrc"].Value = Decimal.Parse(dt.Rows[i]["netPrchsPrc"].ToString()).ToString(vars.format);
                        dgvItems.Rows[i].Cells["amtDscnt"].Value = Decimal.Parse(dt.Rows[i]["amtDscnt"].ToString()).ToString(vars.format);
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
                    txtInventoryTransferNo.Text = frmDialogForm.selectedValue;
                }
            }
            else if (btnFind.Text == "&OK")
            {
                this.Close();
            }
            else if (btnFind.Text == "&Update")
            {
                db = new database();
                sql = "UPDATE inventorytransfer SET remarks2='" + txtRemarks2.Text.Trim().Replace("'", "''") + "',updateDate=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s'),updatedBy=" + vars.user_id + " WHERE docId='" + txtInventoryTransferNo.Text.Trim() + "'";

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

                    Hashtable header = new Hashtable();
                    header.Add("terminalId", vars.terminalId);
                    header.Add("frmWHouse", cboComboFrom.Text);
                    header.Add("toWHouse", cboComboTo.Text);
                    header.Add("createdBy", vars.user_id);

                    DateTime dateTime = DateTime.Parse(txtPostingDate.Text.Trim());
                    header.Add("postingDate", dateTime.ToString("yyyy/MM/dd"));
                    header.Add("remarks1", txtRemarks1.Text.Trim());
                    header.Add("remarks2", txtRemarks2.Text.Trim());

                    try
                    {
                        //purchase prices
                        header.Add("totalPrcntDscnt", Decimal.Parse(txtTotalPrcntPrchsDscnt.Text));
                        header.Add("totalAmtDscnt", Decimal.Parse(txtTotalAmtPrchsDscnt.Text));
                        header.Add("netTotal", Decimal.Parse(txtNetPrchsTotal.Text));
                        header.Add("grossTotal", Decimal.Parse(txtGrossPrchsTotal.Text));
                        //retail prices
                        header.Add("totalPrcntDscntRtl", Decimal.Parse(txtTotalPrcntDscntRtl.Text));
                        header.Add("totalAmtDscntRtl", Decimal.Parse(txtTotalAmtDscntRtl.Text));
                        header.Add("netTotalRtl", Decimal.Parse(txtNetTotalRtl.Text));
                        header.Add("grossTotalRtl", Decimal.Parse(txtGrossTotalRtl.Text));
                    }
                    catch (ExecutionEngineException ex)
                    {
                        MessageBox.Show(this, ex.ToString());
                        return;
                    }

                    DataTable table = new DataTable();
                    table.Columns.Add("docId");
                    table.Columns.Add("indx");
                    table.Columns.Add("itemCode");
                    table.Columns.Add("description");
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
                    table.Columns.Add("qtyPrRtlUoM");
                    table.Columns.Add("realBsNetPrcRtl");
                    table.Columns.Add("realBsGrossPrcRtl");
                    table.Columns.Add("realNetPrcRtl");
                    table.Columns.Add("realGrossPrcRtl");
                    table.Columns.Add("netPrcRtl");
                    table.Columns.Add("grossPrcRtl");
                    table.Columns.Add("prcntDscntRtl");
                    table.Columns.Add("amtDscntRtl");
                    table.Columns.Add("rowNetTotalRtl");
                    table.Columns.Add("rowGrossTotalRtl");

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
                                row["itemCode"] = dgvItems.Rows[i].Cells["itemCode"].Value.ToString();
                                row["description"] = dgvItems.Rows[i].Cells["description"].Value.ToString();
                                row["qty"] = Decimal.Parse(dgvItems.Rows[i].Cells["Qty"].Value.ToString());
                                row["vatable"] = dgvItems.Rows[i].Cells["vatable"].Value.ToString();
                                row["baseUoM"] = fx.null2EmptyStr(dgvItems.Rows[i].Cells["baseUoM"].Value);
                                //purchase prices
                                row["qtyPrPrchsUoM"] = Decimal.Parse(dgvItems.Rows[i].Cells["qtyPrPrchsUoM"].Value.ToString());
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
                                //retail
                                row["qtyPrRtlUoM"] = Decimal.Parse(dgvItems.Rows[i].Cells["qtyPrRtlUoM"].Value.ToString());
                                row["realBsNetPrcRtl"] = Decimal.Parse(dgvItems.Rows[i].Cells["realBsNetPrcRtl"].Value.ToString());
                                row["realBsGrossPrcRtl"] = Decimal.Parse(dgvItems.Rows[i].Cells["realBsGrossPrcRtl"].Value.ToString());
                                row["realNetPrcRtl"] = Decimal.Parse(dgvItems.Rows[i].Cells["realNetPrcRtl"].Value.ToString());
                                row["realGrossPrcRtl"] = Decimal.Parse(dgvItems.Rows[i].Cells["realGrossPrcRtl"].Value.ToString());
                                row["prcntDscntRtl"] = Decimal.Parse(dgvItems.Rows[i].Cells["prcntDscntRtl"].Value.ToString());
                                row["grossPrcRtl"] = Decimal.Parse(dgvItems.Rows[i].Cells["grossPrcRtl"].Value.ToString());
                                row["netPrcRtl"] = Decimal.Parse(dgvItems.Rows[i].Cells["netPrcRtl"].Value.ToString());
                                row["amtDscntRtl"] = Decimal.Parse(dgvItems.Rows[i].Cells["amtDscntRtl"].Value.ToString());
                                row["rowNetTotalRtl"] = Decimal.Parse(dgvItems.Rows[i].Cells["rowNetTotalRtl"].Value.ToString());
                                row["rowGrossTotalRtl"] = Decimal.Parse(dgvItems.Rows[i].Cells["rowGrossTotalRtl"].Value.ToString());

                                table.Rows.Add(row);
                            }
                        }
                    }
                    catch (ExecutionEngineException ex)
                    {
                        MessageBox.Show(this, ex.ToString());
                        return;
                    }

                    InventoryTransfer inventoryTransfer = new InventoryTransfer();
                    if (inventoryTransfer.addInventoryTransfer(header, table))
                    {
                        MessageBox.Show(this, "Saving has been successful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        sql = "SELECT docId FROM inventorytransfer ORDER BY id DESC LIMIT 1";
                        DataTable dt = new DataTable();
                        MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);
                        txtInventoryTransferNo.Text = dt.Rows[0][0].ToString();
                        
                        setToAfterSaveState();
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

        private void frmInventoryTransfer_Load(object sender, EventArgs e)
        {
            sql = "SELECT code FROM warehouse WHERE deactivated='N'";
            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            foreach (DataRow row in dt.Rows)
            {
                cboComboFrom.Items.Add(row["code"]);
                cboComboTo.Items.Add(row["code"]);
            }
        }

        private void registerControls()
        {
            //let's register all controls and DB column names. If new control is added, probably it has to be added here.
            table = new DataTable();
            table.Columns.Add("controls", typeof(string));     //check txt* name
            table.Columns.Add("DBcolumnName", typeof(string)); //check db column name
            table.Columns.Add("Value", typeof(string));        //value of txt*.text

            table.Rows.Add("txtInventoryTransferNo", "docId", "");
            table.Rows.Add("txtRemarks1", "remarks1", "");
            table.Rows.Add("txtRemarks2", "remarks2", "");
        }

        //this validates entries if they are indeed correctly computed. Some of this maybe transferred to the class as duplicate in the future.
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

            if (cboComboFrom.Text == cboComboTo.Text)
            {
                MessageBox.Show(this, "Recipient warehouse is the same as the origin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
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
            
            //checking warehouse is important
            if (cboComboFrom.SelectedIndex == -1 || cboComboTo.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Please select warehouse.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                cboComboFrom.Focus();
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
                    sql += "SELECT itemCode FROM item_warehouse WHERE itemCode = '" + dgvItems.Rows[i].Cells["itemCode"].Value + "' AND whCode = '" + cboComboFrom.Text + "' AND ifnull(inStock,0) < @varBaseQty;";
                    dt = db.select(sql, vars.MySqlConnection);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show(this, "Insufficient inventory for item " + dgvItems.Rows[i].Cells["itemCode"].Value, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else
                    {
                        db = new database(); dt = new DataTable();
                        sql = "SELECT itemCode FROM item_warehouse WHERE itemCode = '" + dgvItems.Rows[i].Cells["itemCode"].Value + "' AND whCode = '" + cboComboFrom.Text + "'";
                        dt = db.select(sql, vars.MySqlConnection);
                        if (dt.Rows.Count < 1)  // if there's no data it is assumed that inventory is zero.
                        {
                            MessageBox.Show(this, "Insufficient inventory for item " + dgvItems.Rows[i].Cells["itemCode"].Value, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                }
            }

            //check item master data properties
            rowCount = (dgvItems.Rows.Count - 1);
            for (i = 0; i < rowCount; i++)
            {
                itemCode = fx.null2EmptyStr(dgvItems.Rows[i].Cells["itemCode"].Value);
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

            return true;
        }

        private void editMode()
        {
            if (vars.role == 0) //if Superuser
                btnFind.Text = "&Update";
        }

        private void cleanUpUI()
        {
            txtInventoryTransferNo.Text = "";
            txtPostingDate.Text = "";
            txtInventoryTransferNo.ReadOnly = false;
            txtPostingDate.ReadOnly = false;
            txtRemarks1.ReadOnly = false;
            dgvItems.Rows.Clear();
            dgvItems.AllowUserToAddRows = false;
            txtRemarks1.Text = ""; txtRemarks2.Text = "";
            txtInventoryTransferNo.Focus();
            cboComboFrom.Enabled = true;
            cboComboTo.Enabled = true;
            cboComboFrom.SelectedIndex = -1;
            cboComboTo.SelectedIndex = -1;

            lblitemsCount.Text = "0 item(s) found.";
            txtTotalPrcntPrchsDscnt.Text = "0.00";
            txtTotalAmtPrchsDscnt.Text = "0.00";
            txtNetPrchsTotal.Text = "0.00";
            txtGrossPrchsTotal.Text = "0.00";
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnFind.Text = "&Save";
            btnPrint.Enabled = false;
            cleanUpUI();
            dateTimePicker1.Enabled = true;
            txtPostingDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            dgvItems.ReadOnly = false;
            dgvItems.AllowUserToAddRows = true;
            txtInventoryTransferNo.ReadOnly = true;
            cboComboFrom.Focus();
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
            crystal_reports.InventoryTransfer objReport;
            this.Cursor = Cursors.WaitCursor;

            sql = "SELECT frmWHouse, toWHouse, docId,DATE_FORMAT(postingDate, '%m/%d/%Y') postingDate,totalPrcntDscntRtl,totalAmtDscntRtl,netTotalRtl,grossTotalRtl,remarks1,remarks2,B.* FROM inventorytransfer A JOIN inventorytransfer_item B USING(docId) WHERE docId='" + txtInventoryTransferNo.Text.Trim() + "' ORDER BY indx";
            db = new database();
            dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);

            objReport = new crystal_reports.InventoryTransfer();
            objReport.SetDataSource(dt);

            crystalReportViewerForm = new frmCrystalReportViewer();
            crystalReportViewerForm.CrystalReportViewer1.ReportSource = objReport;

            crystalReportViewerForm.MdiParent = this.MdiParent;
            crystalReportViewerForm.Show();
            crystalReportViewerForm.Text = "Inventory Transfer Report";
            this.Cursor = Cursors.Default;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            txtPostingDate.Text = dateTimePicker1.Value.ToString("MM/dd/yyyy");
        }
    }
}
