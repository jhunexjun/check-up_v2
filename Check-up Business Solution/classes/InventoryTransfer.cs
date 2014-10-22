using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MySql.Data.MySqlClient;
using Check_up;
using Check_up.classes;
using System.Data;
using System.Windows.Forms;

namespace Check_up.classes
{
    public class InventoryTransfer
    {
        private Hashtable formatHeaderParams(Hashtable ht)
        {
            if (ht.Contains("remarks1"))
                ht["remarks1"] = "'" + ht["remarks1"] + "'";
            else
                ht["remarks1"] = "null";

            if (ht.Contains("remarks2"))
                ht["remarks2"] = "'" + ht["remarks2"] + "'";
            else
                ht["remarks2"] = "null";

            return ht;
        }

        private DataTable formatRowDataParams(DataTable dt)
        {
            return dt;
        }

        private bool checkHeaders(Hashtable header)
        {
            // Note: it is this class that should create the docId, give the terminal id instead.
            if (!header.Contains("terminalId"))
            {
                MessageBox.Show("Please indicate 'terminal id' key in the hash.");
                return false;
            }
            if (!header.Contains("frmWHouse"))
            {
                MessageBox.Show("Please indicate 'from warehouse' key in the hash.");
                return false;
            }
            if (!header.Contains("toWHouse"))
            {
                MessageBox.Show("Please indicate 'to warehouse' key in the hash.");
                return false;
            }
            if (!header.Contains("postingDate"))
            {
                MessageBox.Show("Please indicate 'Posting Date' key in the hash.");
                return false;
            }
            if (!header.Contains("totalPrcntDscnt"))
            {
                MessageBox.Show("Please indicate 'Total Percent Discount' key in the hash.");
                return false;
            }
            if (!header.Contains("totalAmtDscnt"))
            {
                MessageBox.Show("Please indicate 'Total amount discount' key in the hash.");
                return false;
            }
            if (!header.Contains("netTotal"))
            {
                MessageBox.Show("Please indicate 'net Total' key in the hash.");
                return false;
            }
            if (!header.Contains("grossTotal"))
            {
                MessageBox.Show("Please indicate 'Gross Total' key in the hash.");
                return false;
            }
            if (!header.Contains("totalPrcntDscntRtl"))
            {
                MessageBox.Show("Please indicate 'Total Percent Retail Discount' key in the hash.");
                return false;
            }
            if (!header.Contains("totalAmtDscntRtl"))
            {
                MessageBox.Show("Please indicate 'Total amount retail discount' key in the hash.");
                return false;
            }
            if (!header.Contains("netTotalRtl"))
            {
                MessageBox.Show("Please indicate 'net retail total' key in the hash.");
                return false;
            }
            if (!header.Contains("grossTotalRtl"))
            {
                MessageBox.Show("Please indicate 'gross retail total' key in the hash.");
                return false;
            }
            if (!header.Contains("createdBy"))
            {
                MessageBox.Show("Please indicate 'created by' key in the hash.");
                return false;
            }
            if (String.Compare(header["frmWHouse"].ToString(), header["toWHouse"].ToString()) == 0)
            {
                MessageBox.Show("From and to warehouse should not be the same.");
                return false;
            }

            return true;
        }

        private bool checkRows(DataTable tableRows)
        {
            // list down all passed columns.
            ArrayList passedColumns = new ArrayList(tableRows.Columns.Count);
            foreach(DataColumn col in tableRows.Columns)
                passedColumns.Add(col.ColumnName);
                            
            // these are the important columns that should exist in the passed columns.
            ArrayList importantColumns = new ArrayList();
            importantColumns.Add("docId");
            importantColumns.Add("indx");
            importantColumns.Add("itemCode");
            importantColumns.Add("description");
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
            importantColumns.Add("qtyPrRtlUoM");
            importantColumns.Add("realBsNetPrcRtl");
            importantColumns.Add("realBsGrossPrcRtl");
            importantColumns.Add("realNetPrcRtl");
            importantColumns.Add("realGrossPrcRtl");
            importantColumns.Add("netPrcRtl");
            importantColumns.Add("grossPrcRtl");
            importantColumns.Add("prcntDscntRtl");
            importantColumns.Add("amtDscntRtl");
            importantColumns.Add("rowNetTotalRtl");
            importantColumns.Add("rowGrossTotalRtl");

            // now let's compare the two
            foreach(string col in importantColumns) {
                if (!passedColumns.Contains(col))
                {
                    MessageBox.Show("Column '" + col + "' was not passed.");
                    return false;
                }
            }

            // let's check other criterias
            foreach (DataRow row in tableRows.Rows)
            {
                if (row["indx"].ToString() == "")
                {
                    MessageBox.Show("Row Index cannot be empty.");
                    return false;
                }

                int result;
                if (!int.TryParse(row["indx"].ToString(), out result))
                {
                    MessageBox.Show("Row index must be numeric.");
                    return false;
                }

                if (row["itemCode"].ToString() == "")
                {
                    MessageBox.Show("Row item code cannot be empty.");
                    return false;
                }

                string[] vatable = {"Y", "N"};
                if (!vatable.Contains(row["vatable"].ToString()))
                {
                    MessageBox.Show("Vatable should only either be Y or N.");
                    return false;
                }

                decimal value;
                if (!Decimal.TryParse(row["realBsNetPrchsPrc"].ToString(), out value))
                {
                    MessageBox.Show("realBsNetPrchsPrc is not in decimal value.");
                    return false;
                }
                else
                    if (Decimal.Parse(row["realBsNetPrchsPrc"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'realBsNetPrchsPrc'.");
                        return false;
                    }

                if (!Decimal.TryParse(row["realBsGrossPrchsPrc"].ToString(), out value))
                {
                    MessageBox.Show("realBsGrossPrchsPrc is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["realBsGrossPrchsPrc"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'realBsGrossPrchsPrc'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["realNetPrchsPrc"].ToString(), out value))
                {
                    MessageBox.Show("realNetPrchsPrc is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["realNetPrchsPrc"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'realNetPrchsPrc'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["realGrossPrchsPrc"].ToString(), out value))
                {
                    MessageBox.Show("realGrossPrchsPrc is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["realGrossPrchsPrc"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'realGrossPrchsPrc'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["qty"].ToString(), out value))
                {
                    MessageBox.Show("realNetPrchsPrc is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["qty"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'qty'.");
                        return false;
                    }
                }

                string[] baseUoM = { "Y", "N" };
                if (!vatable.Contains(row["baseUoM"].ToString()))
                {
                    MessageBox.Show("baseUoM should only either be Y or N.");
                    return false;
                }

                if (!Decimal.TryParse(row["qtyPrPrchsUoM"].ToString(), out value))
                {
                    MessageBox.Show("qtyPrPrchsUoM is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["qtyPrPrchsUoM"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'qtyPrPrchsUoM'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["prcntDscnt"].ToString(), out value))
                {
                    MessageBox.Show("prcntDscnt is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["prcntDscnt"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'prcntDscnt'.");
                        return false;
                    }

                }

                if (!Decimal.TryParse(row["amtDscnt"].ToString(), out value))
                {
                    MessageBox.Show("amtDscnt is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["amtDscnt"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'amtDscnt'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["netPrchsPrc"].ToString(), out value))
                {
                    MessageBox.Show("netPrchsPrc is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["netPrchsPrc"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'netPrchsPrc'.");
                        return false;
                    }
                }
                    

                if (!Decimal.TryParse(row["grossPrchsPrc"].ToString(), out value))
                {
                    MessageBox.Show("grossPrchsPrc is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["grossPrchsPrc"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'grossPrchsPrc'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["rowNetTotal"].ToString(), out value))
                {
                    MessageBox.Show("rowNetTotal is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["rowNetTotal"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'rowNetTotal'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["rowGrossTotal"].ToString(), out value))
                {
                    MessageBox.Show("rowGrossTotal is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["rowGrossTotal"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'rowGrossTotal'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["qtyPrRtlUoM"].ToString(), out value))
                {
                    MessageBox.Show("qtyPrRtlUoM is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["qtyPrRtlUoM"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'qtyPrRtlUoM'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["realBsNetPrcRtl"].ToString(), out value))
                {
                    MessageBox.Show("realBsNetPrcRtl is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["realBsNetPrcRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'realBsNetPrcRtl'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["realBsGrossPrcRtl"].ToString(), out value))
                {
                    MessageBox.Show("realBsGrossPrcRtl is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["realBsGrossPrcRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'realBsGrossPrcRtl'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["realNetPrcRtl"].ToString(), out value))
                {
                    MessageBox.Show("realNetPrcRtl is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["realNetPrcRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'realNetPrcRtl'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["realGrossPrcRtl"].ToString(), out value))
                {
                    MessageBox.Show("realGrossPrcRtl is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["realGrossPrcRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'realGrossPrcRtl'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["netPrcRtl"].ToString(), out value))
                {
                    MessageBox.Show("netPrcRtl is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["netPrcRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'netPrcRtl'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["grossPrcRtl"].ToString(), out value))
                {
                    MessageBox.Show("grossPrcRtl is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["grossPrcRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'grossPrcRtl'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["prcntDscntRtl"].ToString(), out value))
                {
                    MessageBox.Show("prcntDscntRtl is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["prcntDscntRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'prcntDscntRtl'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["amtDscntRtl"].ToString(), out value))
                {
                    MessageBox.Show("amtDscntRtl is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["amtDscntRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'amtDscntRtl'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["rowNetTotalRtl"].ToString(), out value))
                {
                    MessageBox.Show("rowNetTotalRtl is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["rowNetTotalRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'rowNetTotalRtl'.");
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["rowGrossTotalRtl"].ToString(), out value))
                {
                    MessageBox.Show("rowGrossTotalRtl is not in decimal value.");
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["rowGrossTotalRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show("We do not allow negative value for 'rowGrossTotalRtl'.");
                        return false;
                    }
                }
            } //end for foreach (DataRow row in tableRows.Rows)

            // if everything passed on check, returns true;
            return true;
        }

        // this check and relationships of header and its rows data i.e. sum of rows is equal to the header.
        private bool checkHeaderAndRows(Hashtable header, DataTable tableRows)
        {
            decimal total = 0; int rowsCount = tableRows.Rows.Count;
            for (int i = 0; i < rowsCount; i++)
                total += Decimal.Parse(tableRows.Rows[i]["rowGrossTotalRtl"].ToString());

            if (total != Decimal.Parse(header["grossTotalRtl"].ToString()))
            {
                MessageBox.Show("Price discrepancy on grossTotalRtl.");
                return false;
            }

            total = 0;
            for (int i = 0; i < rowsCount; i++)
                total += Decimal.Parse(tableRows.Rows[i]["prcntDscnt"].ToString());

            total /= 2;

            if (total != Decimal.Parse(header["totalPrcntDscnt"].ToString()))
            {
                MessageBox.Show("Price discrepancy on totalPrcntDscnt.");
                return false;
            }
            
            return true;
        }

        //this is where we add inventory transaction records.
        public bool addInventoryTransfer(Hashtable header, DataTable tableRows)
        {
            if (!checkHeaders(header))
                return false;

            if (!checkRows(tableRows))
                return false;

            int rowsCount = tableRows.Rows.Count;
            if (rowsCount < 1)
            {
                MessageBox.Show("No data to record.");
                return false;
            }

            header = formatHeaderParams(header);
            tableRows = formatRowDataParams(tableRows);

            string sql;
            sql = "START TRANSACTION;";
            sql += "SET @newId=(SELECT CAST(lastNo+1 AS char(11)) FROM documents WHERE documentCode='IT');";
            sql += "SET @docId=CONCAT(@terminalId, @newId);";
            sql += "INSERT INTO inventorytransfer(docId,frmWHouse,toWHouse,postingDate,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,totalPrcntDscntRtl,totalAmtDscntRtl,netTotalRtl,grossTotalRtl,remarks1,remarks2,createdBy)";
            sql += " VALUES(@docId,@frmWHouse,@toWHouse,DATE_FORMAT(@postingDate, '%Y-%m-%d'),@totalPrcntDscnt,@totalAmtDscnt,@netTotal,@grossTotal,@totalPrcntDscntRtl,@totalAmtDscntRtl,@netTotalRtl,@grossTotalRtl,@remarks1,@remarks2,@createdBy);";

            int i = 0;
            for (i = 0; i < rowsCount; i++)
            {
                string sql_indx = "@indx" + i;
                string sql_itemCode = "@itemCode" + i;
                string sql_description = "@description" + i;
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
                string sql_realBsNetPrcRtl = "@realBsNetPrcRtl" + i;
                string sql_realBsGrossPrcRtl = "@realBsGrossPrcRtl" + i;
                string sql_realNetPrcRtl = "@realNetPrcRtl" + i;
                string sql_realGrossPrcRtl = "@realGrossPrcRtl" + i;
                string sql_qtyPrRtlUoM = "@qtyPrRtlUoM" + i;
                string sql_prcntDscntRtl = "@prcntDscntRtl" + i;
                string sql_netPrcRtl = "@netPrcRtl" + i;
                string sql_grossPrcRtl = "@grossPrcRtl" + i;
                string sql_amtDscntRtl = "@amtDscntRtl" + i;
                string sql_rowNetTotalRtl = "@rowNetTotalRtl" + i;
                string sql_rowGrossTotalRtl = "@rowGrossTotalRtl" + i;

                sql += "INSERT INTO inventorytransfer_item(docId,indx,itemCode,description,vatable,realBsNetPrchsPrc,realBsGrossPrchsPrc,realNetPrchsPrc,realGrossPrchsPrc,qty,baseUoM,qtyPrPrchsUoM,prcntDscnt,amtDscnt,netPrchsPrc,grossPrchsPrc,rowNetTotal,rowGrossTotal";
                sql += ",realBsNetPrcRtl,realBsGrossPrcRtl,realNetPrcRtl,realGrossPrcRtl,qtyPrRtlUoM,prcntDscntRtl,netPrcRtl,grossPrcRtl,amtDscntRtl,rowNetTotalRtl,rowGrossTotalRtl)";
                sql += " VALUES(@docId," + sql_indx + "," + sql_itemCode + "," + sql_description + "," + sql_vatable + "," + sql_realBsNetPrchsPrc + "," + sql_realBsGrossPrchsPrc + "," + sql_realNetPrchsPrc + "," + sql_realGrossPrchsPrc + "," + sql_qty + "," + sql_baseUoM + "," + sql_qtyPrPrchsUoM + "," + sql_prcntDscnt + "," + sql_amtDscnt + "," + sql_netPrchsPrc + "," + sql_grossPrchsPrc + "," + sql_rowNetTotal + "," + sql_rowGrossTotal;
                sql += "," + sql_realBsNetPrcRtl + "," + sql_realBsGrossPrcRtl + "," + sql_realNetPrcRtl + "," + sql_realGrossPrcRtl + "," + sql_qtyPrRtlUoM + "," + sql_prcntDscntRtl + "," + sql_netPrcRtl + "," + sql_grossPrcRtl + "," + sql_amtDscnt + "," + sql_rowNetTotal + "," + sql_rowGrossTotal + ");";

                sql += "SET @baseQty" + i + "=(case when " + sql_baseUoM + "='N' then " + sql_qtyPrPrchsUoM + "*" + sql_qty + " else " + sql_qty + " end);"; // ((varBaseUoM == "N") ? varQtyPrPrchsUoM * varQty : varQty);

                sql += "UPDATE itemmasterdata SET trans='Y' WHERE itemCode=" + sql_itemCode + ";";
                sql += "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES(" + sql_itemCode + ", @toWHouse, @baseQty) ON DUPLICATE KEY UPDATE inStock=inStock+@baseQty;";
                sql += "UPDATE item_warehouse SET inStock=inStock-@baseQty WHERE itemCode=" + sql_itemCode + " AND wHCode=@frmWHouse;";
            }
            sql += "UPDATE documents SET lastNo=CAST(@newId AS UNSIGNED) WHERE documentCode='IT';";
            sql += "COMMIT;";

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = vars.MySqlConnection;
                cmd.CommandText = sql;
                cmd.Prepare();

                // header
                cmd.Parameters.AddWithValue("@postingDate", header["postingDate"]);
                cmd.Parameters.AddWithValue("@frmWHouse", header["frmWHouse"]);
                cmd.Parameters.AddWithValue("@toWHouse", header["toWHouse"]);
                cmd.Parameters.AddWithValue("@createdBy", header["createdBy"]);
                cmd.Parameters.AddWithValue("@terminalId", header["terminalId"]);
                cmd.Parameters.AddWithValue("@totalPrcntDscnt", header["totalPrcntDscnt"]);
                cmd.Parameters.AddWithValue("@totalAmtDscnt", header["totalAmtDscnt"]);
                cmd.Parameters.AddWithValue("@netTotal", header["netTotal"]);
                cmd.Parameters.AddWithValue("@grossTotal", header["grossTotal"]);
                cmd.Parameters.AddWithValue("@totalPrcntDscntRtl", header["totalPrcntDscntRtl"]);
                cmd.Parameters.AddWithValue("@totalAmtDscntRtl", header["totalAmtDscntRtl"]);
                cmd.Parameters.AddWithValue("@netTotalRtl", header["netTotalRtl"]);
                cmd.Parameters.AddWithValue("@grossTotalRtl", header["grossTotalRtl"]);

                // rows
                for (i = 0; i < rowsCount; i++)
                {
                    cmd.Parameters.AddWithValue("@indx" + i, tableRows.Rows[i]["indx"]);
                    cmd.Parameters.AddWithValue("@itemCode" + i, tableRows.Rows[i]["itemCode"]);
                    cmd.Parameters.AddWithValue("@description" + i, tableRows.Rows[i]["description"]);
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
                    cmd.Parameters.AddWithValue("@realBsNetPrcRtl" + i, tableRows.Rows[i]["realBsNetPrcRtl"]);
                    cmd.Parameters.AddWithValue("@realBsGrossPrcRtl" + i, tableRows.Rows[i]["realBsGrossPrcRtl"]);
                    cmd.Parameters.AddWithValue("@realNetPrcRtl" + i, tableRows.Rows[i]["realNetPrcRtl"]);
                    cmd.Parameters.AddWithValue("@realGrossPrcRtl" + i, tableRows.Rows[i]["realGrossPrcRtl"]);
                    cmd.Parameters.AddWithValue("@qtyPrRtlUoM" + i, tableRows.Rows[i]["qtyPrRtlUoM"]);
                    cmd.Parameters.AddWithValue("@prcntDscntRtl" + i, tableRows.Rows[i]["prcntDscntRtl"]);
                    cmd.Parameters.AddWithValue("@netPrcRtl" + i, tableRows.Rows[i]["netPrcRtl"]);
                    cmd.Parameters.AddWithValue("@grossPrcRtl" + i, tableRows.Rows[i]["grossPrcRtl"]);
                    cmd.Parameters.AddWithValue("@amtDscntRtl" + i, tableRows.Rows[i]["amtDscntRtl"]);
                    cmd.Parameters.AddWithValue("@rowNetTotalRtl" + i, tableRows.Rows[i]["rowNetTotalRtl"]);
                    cmd.Parameters.AddWithValue("@rowGrossTotalRtl" + i, tableRows.Rows[i]["rowGrossTotalRtl"]);
                }

                int result = cmd.ExecuteNonQuery();
                if (result > 1)
                    return true;
                else
                    return false;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
    }
}
