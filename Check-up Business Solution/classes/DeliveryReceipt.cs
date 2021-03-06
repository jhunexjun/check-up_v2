﻿using System;
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
    public class DeliveryReceipt
    {
        MySqlCommand cmd = new MySqlCommand();

        private bool checkHeadersForAdd(Hashtable header)
        {
            // Note: This class should create the docId, give the terminal id instead.
            if (!header.Contains("terminalId"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'terminal id' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("warehouse"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'warehouse' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("postingDate"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'Posting Date' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("totalPrcntDscnt"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'Total Percent Discount' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("totalAmtDscnt"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'Total amount discount' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("netTotal"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'net Total' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("grossTotal"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'Gross Total' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (!header.Contains("createdBy"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'created by' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            foreach(string col in importantColumns) {
                if (!tableRows.Columns.Contains(col))
                {
                    MessageBox.Show(Form.ActiveForm, "Column '" + col + "' was not passed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            // let's check other criterias
            string sql = "";
            foreach (DataRow row in tableRows.Rows)
            {
                if (row["indx"] == DBNull.Value)
                {
                    MessageBox.Show(Form.ActiveForm, "Row Index cannot be null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
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

                if (row["vendorCode"] == DBNull.Value)
                {
                    MessageBox.Show(Form.ActiveForm, "Row vendor code cannot be null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                sql = "select `code` from businesspartner where `code`=@vendorCode";
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@vendorCode", row["vendorCode"]);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count <= 0)
                {
                    MessageBox.Show(Form.ActiveForm, "No reference to vendor code '" + row["vendorCode"].ToString() + "'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        // this checks the relationships of header and its rows data i.e. sum of rows are equal to the header's.
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

            //continue here

            return true;
        }

        private Hashtable formatHeaders(Hashtable header)
        {
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
            sql = "SET @newId=(SELECT CAST(lastNo+1 AS char(11)) FROM documents WHERE documentCode='DR');";
            sql += "SET @docId=CONCAT(@terminalId, @newId);";
            sql += "INSERT INTO deliveryreceipt(docId,warehouse,postingDate,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,remarks1,remarks2,createdBy,createDate)";
            sql += " VALUES(@docId,@warehouse,@postingDate,@totalPrcntDscnt,@totalAmtDscnt,@netTotal,@grossTotal,@remarks1,@remarks2,@createdBy,@date);";

            int i;
            for (i = 0; i < rowsCount; i++)
            {
                string sql_indx = "@indx" + i;
                string sql_vendorCode = "@vendorCode" + i;
                string sql_vendorName = "@vendorName" + i;
                string sql_itemCode = "@itemCode" + i;
                string sql_description = "@description" + i;
                string sql_warehouseRow = "@warehouseRow" + i;
                string sql_vatable = "@vatable" + i;
                string sql_realBsNetPrchsPrc = "@realBsNetPrchsPrc" + i;
                string sql_realBsGrossPrchsPrc = "@realBsGrossPrchsPrc" + i;
                string sql_realNetPrchsPrc = "@realNetPrchsPrc" + i;
                string sql_realGrossPrchsPrc = "@realGrossPrchsPrc" + i;
                string sql_qty = "@qty" + i;
                string sql_baseUoM = "@baseUoM" + i;
                string sql_qtyPrPrchsUoM = "@qtyPrPrchsUoM" + i;
                string sql_prcntDscnt = "@prcntDscnt" + i;
                string sql_amtDscnt = "@amtDscnt" + i;
                string sql_netPrchsPrc = "@netPrchsPrc" + i;
                string sql_grossPrchsPrc = "@grossPrchsPrc" + i;
                string sql_rowNetTotal = "@rowNetTotal" + i;
                string sql_rowGrossTotal = "@rowGrossTotal" + i;
            
                sql += "SET @baseQty" + i + "=(case when " + sql_baseUoM + "='N' then " + sql_qtyPrPrchsUoM + "*" + sql_qty + " else " + sql_qty + " end);"; // varBaseQty = ((varBaseUoM == "N") ? varQtyPrPrchsUoM * varQty : varQty);
                sql += "INSERT INTO deliveryreceipt_item(docId,indx,vendorCode,vendorName,itemCode,description,warehouse,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal)";
                sql += " VALUES(@docId," + sql_indx + "," + sql_vendorCode + "," + sql_vendorName + "," + sql_itemCode + "," + sql_description + "," + sql_warehouseRow + "," + sql_vatable + "," + sql_realBsNetPrchsPrc + "," + sql_realBsGrossPrchsPrc + "," + sql_realNetPrchsPrc + "," + sql_realGrossPrchsPrc + "," + sql_qty + "," + sql_baseUoM + "," + sql_qtyPrPrchsUoM + "," + sql_prcntDscnt + "," + sql_amtDscnt + "," + sql_netPrchsPrc + "," + sql_grossPrchsPrc + "," + sql_rowNetTotal + "," + sql_rowGrossTotal + ");";
                sql += "UPDATE businesspartner SET trans='Y' WHERE code=" + sql_vendorCode + ";";
                sql += "UPDATE itemmasterdata SET trans='Y' WHERE itemCode=" + sql_itemCode + ";";
                sql += "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES(" + sql_itemCode + "," + sql_warehouseRow + ",@baseQty" + i + ") ON DUPLICATE KEY UPDATE inStock=ifnull(inStock, 0)+@baseQty" + i + ";";
            }
            sql += "UPDATE documents SET lastNo=CAST(@newId AS UNSIGNED) WHERE documentCode='DR';";

            MySqlTransaction trans = vars.MySqlConnection.BeginTransaction();

            try
            {
                cmd.Connection = vars.MySqlConnection;
                cmd.CommandText = sql;
                cmd.Prepare();

                //header
                cmd.Parameters.AddWithValue("@terminalId", header["terminalId"]);
                cmd.Parameters.AddWithValue("@warehouse", header["warehouse"]);
                cmd.Parameters.AddWithValue("@postingDate", header["postingDate"]);
                cmd.Parameters.AddWithValue("@totalPrcntDscnt", Decimal.Parse(header["totalPrcntDscnt"].ToString()));
                cmd.Parameters.AddWithValue("@totalAmtDscnt", Decimal.Parse(header["totalAmtDscnt"].ToString()));
                cmd.Parameters.AddWithValue("@netTotal", Decimal.Parse(header["netTotal"].ToString()));
                cmd.Parameters.AddWithValue("@grossTotal", Decimal.Parse(header["grossTotal"].ToString()));
                cmd.Parameters.AddWithValue("@remarks1", header["remarks1"]);
                cmd.Parameters.AddWithValue("@remarks2", header["remarks2"]);
                cmd.Parameters.AddWithValue("@createdBy", header["createdBy"]);

                //rows
                for (i = 0; i < rowsCount; i++)
                {
                    cmd.Parameters.AddWithValue("@indx" + i, tableRows.Rows[i]["indx"]);
                    cmd.Parameters.AddWithValue("@vendorCode" + i, tableRows.Rows[i]["vendorCode"]);
                    cmd.Parameters.AddWithValue("@vendorName" + i, tableRows.Rows[i]["vendorName"]);
                    cmd.Parameters.AddWithValue("@itemCode" + i, tableRows.Rows[i]["itemCode"]);
                    cmd.Parameters.AddWithValue("@description" + i, tableRows.Rows[i]["description"]);
                    cmd.Parameters.AddWithValue("@warehouseRow" + i, tableRows.Rows[i]["warehouseRow"]);
                    cmd.Parameters.AddWithValue("@vatable" + i, tableRows.Rows[i]["vatable"]);
                    cmd.Parameters.AddWithValue("@realBsNetPrchsPrc" + i, tableRows.Rows[i]["realBsNetPrchsPrc"]);
                    cmd.Parameters.AddWithValue("@realBsGrossPrchsPrc" + i, tableRows.Rows[i]["realBsGrossPrchsPrc"]);
                    cmd.Parameters.AddWithValue("@realNetPrchsPrc" + i, tableRows.Rows[i]["realNetPrchsPrc"]);
                    cmd.Parameters.AddWithValue("@realGrossPrchsPrc" + i, tableRows.Rows[i]["realGrossPrchsPrc"]);
                    cmd.Parameters.AddWithValue("@qty" + i, tableRows.Rows[i]["qty"]);
                    cmd.Parameters.AddWithValue("@baseUoM" + i, tableRows.Rows[i]["baseUoM"]);
                    cmd.Parameters.AddWithValue("@qtyPrPrchsUoM" + i, tableRows.Rows[i]["qtyPrPrchsUoM"]);
                    cmd.Parameters.AddWithValue("@prcntDscnt" + i, tableRows.Rows[i]["prcntDscnt"]);
                    cmd.Parameters.AddWithValue("@amtDscnt" + i, tableRows.Rows[i]["amtDscnt"]);
                    cmd.Parameters.AddWithValue("@netPrchsPrc" + i, tableRows.Rows[i]["netPrchsPrc"]);
                    cmd.Parameters.AddWithValue("@grossPrchsPrc" + i, tableRows.Rows[i]["grossPrchsPrc"]);
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
                catch (MySqlException ex2)
                {
                    MessageBox.Show(Form.ActiveForm, ex2.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
        }
    }
}
