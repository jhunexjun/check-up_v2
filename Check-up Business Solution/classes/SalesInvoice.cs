using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using Check_up.classes;

namespace Check_up.classes
{
    public class SalesInvoice
    {
        private bool checkHeadersForAdd(Hashtable header)
        {
            if(!header.Contains("customerCode"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'Customer code' in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("warehouse"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'warehouse' in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("postingDate"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'posting date' in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("totalPrcntDscnt"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'total percent discount' in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("totalAmtDscnt"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'total amount discount' in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("netTotal"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'net total' in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("grossTotal"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'gross total' in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("netTotal"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'net total' in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("createdBy"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'created by' in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private bool checkRowsForAdd(DataTable tableRows)
        {
            // these are the important columns that should exist in the passed columns.
            ArrayList importantColumns = new ArrayList();
            importantColumns.Add("indx");
            importantColumns.Add("itemCode");
            importantColumns.Add("description");
            importantColumns.Add("warehouseRow");
            importantColumns.Add("vatable");
            importantColumns.Add("realBsNetPrchsPrc");
            importantColumns.Add("realBsGrossPrchsPrc");
            importantColumns.Add("realNetPrchsPrc");
            importantColumns.Add("realGrossPrchsPrc");
            importantColumns.Add("qty");
            importantColumns.Add("baseUoM");
            importantColumns.Add("qtyPrPrchsUoM");
            importantColumns.Add("prcntDscnt");
            importantColumns.Add("amtDscnt");
            importantColumns.Add("netPrchsPrc");
            importantColumns.Add("grossPrchsPrc");
            importantColumns.Add("rowNetTotal");
            importantColumns.Add("rowGrossTotal");

            // now let's compare the two
            foreach (string col in importantColumns)
            {
                if (!tableRows.Columns.Contains(col))
                {
                    MessageBox.Show(Form.ActiveForm, "Column '" + col + "' was not passed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            // let's check other criterias
            foreach (DataRow row in tableRows.Rows)
            {
                if (row["indx"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row Index cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                int result;
                if (!int.TryParse(row["indx"].ToString(), out result))
                {
                    MessageBox.Show(Form.ActiveForm, "Row index must be numeric.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["itemCode"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row itemcode cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if(row["vatable"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row vatable cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["saleUoM"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row 'sale UOM' cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["qtyPrSaleUoM"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row 'Qty Per Sale UoM' cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["netPrchsPrc"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row 'net purchase price' cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["grossPrchsPrc"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row 'gross purchase Price' cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["realBsNetSalePrc"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row 'real base net sale price' cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["realBsGrossSalePrc"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row 'real base gross sale price' cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["qty"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row 'quantity' cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["baseUoM"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row 'base UOM' cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["prcntDscnt"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row 'percent discount' cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["amtDscnt"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row 'amount discount' cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["netSalePrc"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row 'net sale price' cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["grossSalePrc"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row 'gross sale price' cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["rowNetTotal"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row 'net total' cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (row["rowGrossTotal"].ToString() == "")
                {
                    MessageBox.Show(Form.ActiveForm, "Row 'row gross total' cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            
            return true;
        }

        private bool checkHeadersAndRowsForAdd(Hashtable header, DataTable tableRows)
        {
            decimal total = 0; int rowsCount = tableRows.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
                total += Decimal.Parse(tableRows.Rows[i]["rowGrossTotal"].ToString());

            if (total != Decimal.Parse(header["grossTotal"].ToString()))
            {
                MessageBox.Show(Form.ActiveForm, "Price discrepancy on Gross Total.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            total = 0;
            for (int i = 0; i < rowsCount; i++)
                total += Decimal.Parse(tableRows.Rows[i]["prcntDscnt"].ToString());

            total /= rowsCount;

            if (total != Decimal.Parse(header["totalPrcntDscnt"].ToString()))
            {
                MessageBox.Show(Form.ActiveForm, "Price discrepancy on totalPrcntDscnt.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            total = 0;
            for (int i = 0; i < rowsCount; i++)
                total += Decimal.Parse(tableRows.Rows[i]["rowNetTotal"].ToString());

            if (total != Decimal.Parse(header["netTotal"].ToString()))
            {
                MessageBox.Show(Form.ActiveForm, "Price discrepancy on Net Total.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            // continue here...

            return true;
        }

        private Hashtable formatHeaders(Hashtable header)
        {
            if (!header.Contains("customerName"))
                header["customerName"] = null;
            if (!header.Contains("remarks1"))
                header["remarks1"] = null;
            if (!header.Contains("remarks2"))
                header["remarks2"] = null;

            return header;
        }

        public bool addDeliveryReceipt(Hashtable header, DataTable tableRows)
        {
            if (!checkHeadersForAdd(header))
                return false;

            if (!checkRowsForAdd(tableRows))
                return false;

            int rowsCount = tableRows.Rows.Count;
            if (rowsCount < 1)
            {
                MessageBox.Show(Form.ActiveForm, "No data to record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (!checkHeadersAndRowsForAdd(header, tableRows))
                return false;

            header = formatHeaders(header);

            DateTime dateTime = DateTime.Parse(header["postingDate"].ToString());
            header["postingDate"] = dateTime.ToString("yyyy/MM/dd");

            string sql;
            sql = "SET @date=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s');";
            sql += "SET @newId=(SELECT CAST(lastNo+1 AS char(11)) FROM documents WHERE documentCode='SI');";
            sql += "SET @docId=CONCAT(@terminalId, @newId);";
            sql += "INSERT INTO salesinvoice(docId,customerCode,customerName,warehouse,postingDate,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,remarks1,remarks2,createDate,createdBy)";
            sql += " VALUES(@docId,@customerCode,@customerName,@warehouse,@postingDate,@totalPrcntDscnt,@totalAmtDscnt,@netTotal,@grossTotal,@remarks1,@remarks2,@date,@createdby)";
            sql += "UPDATE businesspartner SET trans='Y' WHERE code=@customerCode;";

            int i;
            for (i = 0; i < rowsCount; i++)
            {
                string indx = "@indx" + i;
                string itemcode = "@itemcode" + i;
                string description = "@description" + 1;
                string vatable = "@vatable" + i;
                string netPrchsPrc = "@netPrhcsPrc" + i;
                string grossPrchsPrc = "@grossPrchsPrc" + i;
                string saleUoM = "@saleUoM" + i;
                string baseUoM = "@baseUoM" + i;
                string qtyPrSaleUoM = "@qtyPrSaleUoM" + i;
                string realBsNetSalePrc = "@realBsNetSalePrc" + i;
                string realBsGrossSalePrc = "@realBsGrossSalePrc" + i;
                string qty = "@qty" + i;
                string prcntDscnt = "@prcntDscnt" + i;
                string amtDscnt = "@amtDscnt" + i;
                string netSalePrc = "@netSalePrc" + i;
                string grossSalePrc = "@grossSalePrc" + i;
                string rowNetTotal = "@rowNetTotal" + i;
                string rowGrossTotal = "@rowGrossTotal" + i;

                sql += "SET @baseQty" + i + "=(case when " + baseUoM + "='N' then " + qtyPrSaleUoM + "*" + qty + " else " + qty + " end);"; // varBaseQty = ((varBaseUoM == "N") ? varQtyPrSaleUoM * varQty : varQty);
                sql += "INSERT INTO salesinvoice_item(docId,indx,itemCode,description,saleUoM,qtyPrSaleUoM,qty,baseUoM,prcntDscnt,amtDscnt,vatable,netSalePrc,grossSalePrc,realBsNetSalePrc,realBsGrossSalePrc,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal)";
                sql += " VALUES(@docId," + indx + "," + itemcode + "," + description + "," + saleUoM + "," + qtyPrSaleUoM + "," + qty + "," + baseUoM + "," + prcntDscnt + "," + amtDscnt + "," + vatable + "," + netSalePrc + "," + grossSalePrc + "," + realBsNetSalePrc + "," + realBsGrossSalePrc + "," + netPrchsPrc + "," + grossPrchsPrc + "," + rowNetTotal + "," + rowGrossTotal + ");";
                sql += "UPDATE itemmasterdata SET trans='Y' WHERE itemCode=" + itemcode + ";";
                sql += "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES(" + itemcode + ",@warehouse,@baseQty) ON DUPLICATE KEY UPDATE inStock=ifnull(inStock, 0)-@baseQty" + i + ";";
            }
            sql += "UPDATE documents SET lastNo=CAST(@newId AS UNSIGNED) WHERE documentCode='SI';";

            MySqlTransaction trans = vars.MySqlConnection.BeginTransaction();

            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
                cmd.Prepare();

                //header
                cmd.Parameters.AddWithValue("@terminalId", header["terminalId"]);
                cmd.Parameters.AddWithValue("@customerCode", header["customerCode"]);
                cmd.Parameters.AddWithValue("@customerName", header["customerName"]);
                cmd.Parameters.AddWithValue("@warehouse", header["warehouse"]);
                cmd.Parameters.AddWithValue("@postingDate", header["postingDate"]);
                cmd.Parameters.AddWithValue("@totalPrcntDscnt", header["totalPrcntDscnt"]);
                cmd.Parameters.AddWithValue("@totalAmtDscnt", header["totalAmtDscnt"]);
                cmd.Parameters.AddWithValue("@netTotal", header["netTotal"]);
                cmd.Parameters.AddWithValue("@grossTotal", header["grossTotal"]);
                cmd.Parameters.AddWithValue("@remarks1", header["remarks1"]);
                cmd.Parameters.AddWithValue("@remarks2", header["remarks2"]);
                cmd.Parameters.AddWithValue("@createdby", header["createdby"]);

                //rows
                for (i = 0; i < rowsCount; i++)
                {
                    cmd.Parameters.AddWithValue("@indx" + i, tableRows.Rows[i]["indx"]);
                    cmd.Parameters.AddWithValue("@itemcode" + i, tableRows.Rows[i]["itemcode"]);
                    cmd.Parameters.AddWithValue("@description" + i, tableRows.Rows[i]["description"]);
                    cmd.Parameters.AddWithValue("@vatable" + i, tableRows.Rows[i]["vatable"]);
                    cmd.Parameters.AddWithValue("@netPrchsPrc" + i, tableRows.Rows[i]["netPrchsPrc"]);
                    cmd.Parameters.AddWithValue("@grossPrchsPrc" + i, tableRows.Rows[i]["grossPrchsPrc"]);
                    cmd.Parameters.AddWithValue("@saleUoM" + i, tableRows.Rows[i]["saleUoM"]);
                    cmd.Parameters.AddWithValue("@baseUoM" + i, tableRows.Rows[i]["baseUoM"]);
                    cmd.Parameters.AddWithValue("@qtyPrSaleUoM" + i, tableRows.Rows[i]["qtyPrSaleUoM"]);
                    cmd.Parameters.AddWithValue("@realBsNetSalePrc" + i, tableRows.Rows[i]["realBsNetSalePrc"]);
                    cmd.Parameters.AddWithValue("@realBsGrossSalePrc" + i, tableRows.Rows[i]["realBsGrossSalePrc"]);
                    cmd.Parameters.AddWithValue("@qty" + i, tableRows.Rows[i]["qty"]);
                    cmd.Parameters.AddWithValue("@prcntDscnt" + i, tableRows.Rows[i]["prcntDscnt"]);
                    cmd.Parameters.AddWithValue("@amtDscnt" + i, tableRows.Rows[i]["amtDscnt"]);
                    cmd.Parameters.AddWithValue("@netSalePrc" + i, tableRows.Rows[i]["netSalePrc"]);
                    cmd.Parameters.AddWithValue("@grossSalePrc" + i, tableRows.Rows[i]["grossSalePrc"]);
                    cmd.Parameters.AddWithValue("@rowNetTotal" + i, tableRows.Rows[i]["rowNetTotal"]);
                    cmd.Parameters.AddWithValue("@rowGrossTotal" + i, tableRows.Rows[i]["rowGrossTotal"]);
                }

                cmd.ExecuteNonQuery();
                trans.Commit();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(Form.ActiveForm, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(Form.ActiveForm, "There was an error found, this will attempt to roll back transaction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                try
                {
                    trans.Rollback();
                    MessageBox.Show(Form.ActiveForm, "Rolling back transaction has been successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MySqlException ex1)
                {
                    MessageBox.Show(Form.ActiveForm, ex1.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
        }
    }
}
