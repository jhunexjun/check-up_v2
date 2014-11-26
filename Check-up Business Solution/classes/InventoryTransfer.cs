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
        MySqlCommand cmd = new MySqlCommand();

        private bool checkHeadersForAdd(Hashtable header)
        {
            // Note: This class should create the docId, give the terminal id instead.
            if (!header.Contains("terminalId"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'terminal id' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!header.Contains("frmWHouse"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'from warehouse' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!header.Contains("toWHouse"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'to warehouse' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!header.Contains("postingDate"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'Posting Date' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!header.Contains("totalPrcntDscnt"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'Total Percent Discount' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!header.Contains("totalAmtDscnt"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'Total amount discount' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!header.Contains("netTotal"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'net Total' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!header.Contains("grossTotal"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'Gross Total' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!header.Contains("totalPrcntDscntRtl"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'Total Percent Retail Discount' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!header.Contains("totalAmtDscntRtl"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'Total amount retail discount' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!header.Contains("netTotalRtl"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'net retail total' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!header.Contains("grossTotalRtl"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'gross retail total' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!header.Contains("createdBy"))
            {
                MessageBox.Show(Form.ActiveForm, "Please indicate 'created by' key in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (String.Compare(header["frmWHouse"].ToString(), header["toWHouse"].ToString()) == 0)
            {
                MessageBox.Show(Form.ActiveForm, "From and to warehouse should not be the same.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool checkRowsForAdd(Hashtable header, DataTable tableRows)
        {
            // list down all passed columns.
            ArrayList passedColumns = new ArrayList(tableRows.Columns.Count);
            foreach(DataColumn col in tableRows.Columns)
                passedColumns.Add(col.ColumnName);
                            
            // these are the important columns that should exist in the passed columns.
            ArrayList importantColumns = new ArrayList();
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
                    MessageBox.Show(Form.ActiveForm, "Row item code cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                string[] vatable = {"Y", "N"};
                if (!vatable.Contains(row["vatable"].ToString()))
                {
                    MessageBox.Show(Form.ActiveForm, "Vatable should only either be Y or N.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                decimal value;
                if (!Decimal.TryParse(row["realBsNetPrchsPrc"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "realBsNetPrchsPrc is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                else
                    if (Decimal.Parse(row["realBsNetPrchsPrc"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'realBsNetPrchsPrc'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }

                if (!Decimal.TryParse(row["realBsGrossPrchsPrc"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "realBsGrossPrchsPrc is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["realBsGrossPrchsPrc"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'realBsGrossPrchsPrc'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["realNetPrchsPrc"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "realNetPrchsPrc is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["realNetPrchsPrc"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'realNetPrchsPrc'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["realGrossPrchsPrc"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "realGrossPrchsPrc is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["realGrossPrchsPrc"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'realGrossPrchsPrc'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["qty"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "realNetPrchsPrc is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["qty"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'qty'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                string[] baseUoM = { "Y", "N" };
                if (!vatable.Contains(row["baseUoM"].ToString()))
                {
                    MessageBox.Show(Form.ActiveForm, "baseUoM should only either be Y or N.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!Decimal.TryParse(row["qtyPrPrchsUoM"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "qtyPrPrchsUoM is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["qtyPrPrchsUoM"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'qtyPrPrchsUoM'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["prcntDscnt"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "prcntDscnt is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["prcntDscnt"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'prcntDscnt'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                }

                if (!Decimal.TryParse(row["amtDscnt"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "amtDscnt is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["amtDscnt"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'amtDscnt'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["netPrchsPrc"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "netPrchsPrc is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["netPrchsPrc"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'netPrchsPrc'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                    

                if (!Decimal.TryParse(row["grossPrchsPrc"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "grossPrchsPrc is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["grossPrchsPrc"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'grossPrchsPrc'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["rowNetTotal"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "rowNetTotal is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["rowNetTotal"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'rowNetTotal'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["rowGrossTotal"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "rowGrossTotal is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["rowGrossTotal"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'rowGrossTotal'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["qtyPrRtlUoM"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "qtyPrRtlUoM is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["qtyPrRtlUoM"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'qtyPrRtlUoM'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["realBsNetPrcRtl"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "realBsNetPrcRtl is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["realBsNetPrcRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'realBsNetPrcRtl'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["realBsGrossPrcRtl"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "realBsGrossPrcRtl is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["realBsGrossPrcRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'realBsGrossPrcRtl'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["realNetPrcRtl"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "realNetPrcRtl is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["realNetPrcRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'realNetPrcRtl'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["realGrossPrcRtl"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "realGrossPrcRtl is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["realGrossPrcRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'realGrossPrcRtl'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["netPrcRtl"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "netPrcRtl is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["netPrcRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'netPrcRtl'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["grossPrcRtl"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "grossPrcRtl is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["grossPrcRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'grossPrcRtl'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["prcntDscntRtl"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "prcntDscntRtl is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["prcntDscntRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'prcntDscntRtl'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["amtDscntRtl"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "amtDscntRtl is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["amtDscntRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'amtDscntRtl'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["rowNetTotalRtl"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "rowNetTotalRtl is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["rowNetTotalRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'rowNetTotalRtl'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (!Decimal.TryParse(row["rowGrossTotalRtl"].ToString(), out value))
                {
                    MessageBox.Show(Form.ActiveForm, "rowGrossTotalRtl is not in decimal value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (Decimal.Parse(row["rowGrossTotalRtl"].ToString()) < 0m)
                    {
                        MessageBox.Show(Form.ActiveForm, "We do not allow negative value for 'rowGrossTotalRtl'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            } //end for foreach (DataRow row in tableRows.Rows)

            // We do not allow no inventory or greater than the inventory
            int rowCount = tableRows.Rows.Count, i;
            string varBaseUoM, sql, itemcode;

            MySqlCommand cmd; MySqlDataAdapter da; DataTable dt;

            decimal varBaseQty, varQty, qtyPrRtlUoM;
            for (i = 0; i < rowCount; i++)
            {
                itemcode = fx.null2EmptyStr(tableRows.Rows[i]["itemCode"]);
                if (itemcode != "")
                {
                    varQty = Decimal.Parse(tableRows.Rows[i]["Qty"].ToString());
                    varBaseUoM = tableRows.Rows[i]["baseUoM"].ToString();
                    qtyPrRtlUoM = Decimal.Parse(tableRows.Rows[i]["qtyPrRtlUoM"].ToString());
                    varBaseQty = varQty * ((varBaseUoM == "N") ? qtyPrRtlUoM : 1);

                    sql = "SELECT inStock FROM item_warehouse WHERE itemCode = '" + tableRows.Rows[i]["itemCode"].ToString() + "' AND whCode = '" + header["frmWHouse"].ToString() + "'";
                    cmd = new MySqlCommand(sql, vars.MySqlConnection);
                    da = new MySqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (Decimal.Parse(dt.Rows[0]["inStock"].ToString()) < varBaseQty)
                        {
                            MessageBox.Show(Form.ActiveForm, "Insufficient inventory for item " + tableRows.Rows[i]["itemCode"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show(Form.ActiveForm, "Insufficient inventory for item " + tableRows.Rows[i]["itemCode"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }                    
                }
            }

            //check item master data properties
            rowCount = rowCount - 1;
            for (i = 0; i < rowCount; i++)
            {
                itemcode = fx.null2EmptyStr(tableRows.Rows[i]["itemCode"]);
                sql = "SELECT deactivated FROM itemmasterdata WHERE itemCode='" + itemcode + "'";
                cmd = new MySqlCommand(sql, vars.MySqlConnection);
                da = new MySqlDataAdapter(cmd);
                da.Fill(dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["deactivated"].ToString() == "Y")
                    {
                        MessageBox.Show(Form.ActiveForm, "Item code " + itemcode + " is deactivated. Cannot proceed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); ;
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show(Form.ActiveForm, "Item code " + itemcode + " is not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                    return false;
                }
            }

            // if everything passed on check, returns true;
            return true;
        }

        // this checks the relationships of header and its rows data i.e. sum of rows are equal to the header's.
        private bool checkHeaderAndRows(Hashtable header, DataTable tableRows)
        {
            decimal total = 0; int rowsCount = tableRows.Rows.Count, i;
            for (i = 0; i < rowsCount; i++)
                total += Decimal.Parse(tableRows.Rows[i]["rowGrossTotalRtl"].ToString());

            if (total != Decimal.Parse(header["grossTotalRtl"].ToString()))
            {
                MessageBox.Show(Form.ActiveForm, "Price discrepancy on gross Total Retail.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            total = 0;
            for (i = 0; i < rowsCount; i++)
                total += Decimal.Parse(tableRows.Rows[i]["rowNetTotalRtl"].ToString());

            if (total != Decimal.Parse(header["netTotalRtl"].ToString()))
            {
                MessageBox.Show(Form.ActiveForm, "Price discrepancy on Net Total Retail.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

                total = 0;
            for (i = 0; i < rowsCount; i++)
                total += Decimal.Parse(tableRows.Rows[i]["prcntDscnt"].ToString());

            total /= 2;

            if (total != Decimal.Parse(header["totalPrcntDscnt"].ToString()))
            {
                MessageBox.Show(Form.ActiveForm, "Price discrepancy on total Percent Discount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //continue here
            
            return true;
        }

        //this is where we add inventory transaction records.
        public bool addInventoryTransfer(Hashtable header, DataTable tableRows)
        {
            if (!checkHeadersForAdd(header))
                return false;

            if (!checkRowsForAdd(header, tableRows))
                return false;

            int rowsCount = tableRows.Rows.Count;
            if (rowsCount < 1)
            {
                MessageBox.Show(Form.ActiveForm, "No data to record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!checkHeaderAndRows(header, tableRows))
                return false;

            DateTime dateTime = DateTime.Parse(header["postingDate"].ToString());
            header["postingDate"] = dateTime.ToString("yyyy/MM/dd");

            string sql;
            sql = "START TRANSACTION;";
            sql += "SET @newId=(SELECT CAST(lastNo+1 AS char(11)) FROM documents WHERE documentCode='IT');";
            sql += "SET @docId=CONCAT(@terminalId, @newId);";
            sql += "INSERT INTO inventorytransfer(docId,frmWHouse,toWHouse,postingDate,totalPrcntDscnt,totalAmtDscnt,netTotal,grossTotal,totalPrcntDscntRtl,totalAmtDscntRtl,netTotalRtl,grossTotalRtl,remarks1,remarks2,createdBy,createDate)";
            sql += " VALUES(@docId,@frmWHouse,@toWHouse,DATE_FORMAT(@postingDate, '%Y-%m-%d'),@totalPrcntDscnt,@totalAmtDscnt,@netTotal,@grossTotal,@totalPrcntDscntRtl,@totalAmtDscntRtl,@netTotalRtl,@grossTotalRtl,@remarks1,@remarks2,@createdBy,DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s'));";

            int i;
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
                //retail
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
                sql += "," + sql_realBsNetPrcRtl + "," + sql_realBsGrossPrcRtl + "," + sql_realNetPrcRtl + "," + sql_realGrossPrcRtl + "," + sql_qtyPrRtlUoM + "," + sql_prcntDscntRtl + "," + sql_netPrcRtl + "," + sql_grossPrcRtl + "," + sql_amtDscntRtl + "," + sql_rowNetTotalRtl + "," + sql_rowGrossTotalRtl + ");";

                sql += "SET @baseQty" + i + "=(case when " + sql_baseUoM + "='N' then " + sql_qtyPrPrchsUoM + "*" + sql_qty + " else " + sql_qty + " end);"; // ((varBaseUoM == "N") ? varQtyPrPrchsUoM * varQty : varQty);

                sql += "UPDATE itemmasterdata SET trans='Y' WHERE itemCode=" + sql_itemCode + ";";
                sql += "INSERT INTO item_warehouse(itemCode,whCode,inStock) VALUES(" + sql_itemCode + ", @toWHouse, @baseQty" + i + ") ON DUPLICATE KEY UPDATE inStock=ifnull(inStock, 0)+@baseQty" + i + ";";
                sql += "UPDATE item_warehouse SET inStock=ifnull(inStock, 0)-@baseQty" + i + " WHERE itemCode=" + sql_itemCode + " AND wHCode=@frmWHouse;";
            }
            sql += "UPDATE documents SET lastNo=CAST(@newId AS UNSIGNED) WHERE documentCode='IT';";
            sql += "COMMIT;";

            try
            {
                cmd.Connection = vars.MySqlConnection;
                cmd.CommandText = sql;
                cmd.Prepare();

                // header
                cmd.Parameters.AddWithValue("@postingDate", header["postingDate"]);
                cmd.Parameters.AddWithValue("@frmWHouse", header["frmWHouse"]);
                cmd.Parameters.AddWithValue("@toWHouse", header["toWHouse"]);
                cmd.Parameters.AddWithValue("@createdBy", header["createdBy"]);
                cmd.Parameters.AddWithValue("@terminalId", header["terminalId"]);
                cmd.Parameters.AddWithValue("@remarks1", header["remarks1"]);
                cmd.Parameters.AddWithValue("@remarks2", header["remarks2"]);
                cmd.Parameters.AddWithValue("@totalPrcntDscnt", Decimal.Parse(header["totalPrcntDscnt"].ToString()));
                cmd.Parameters.AddWithValue("@totalAmtDscnt", Decimal.Parse(header["totalAmtDscnt"].ToString()));
                cmd.Parameters.AddWithValue("@netTotal", Decimal.Parse(header["netTotal"].ToString()));
                cmd.Parameters.AddWithValue("@grossTotal", Decimal.Parse(header["grossTotal"].ToString()));
                cmd.Parameters.AddWithValue("@totalPrcntDscntRtl", Decimal.Parse(header["totalPrcntDscntRtl"].ToString()));
                cmd.Parameters.AddWithValue("@totalAmtDscntRtl", Decimal.Parse(header["totalAmtDscntRtl"].ToString()));
                cmd.Parameters.AddWithValue("@netTotalRtl", Decimal.Parse(header["netTotalRtl"].ToString()));
                cmd.Parameters.AddWithValue("@grossTotalRtl", Decimal.Parse(header["grossTotalRtl"].ToString()));

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
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool checkHeadersForUpdate(Hashtable header)
        {
            if (!header.Contains("remarks2"))
            {
                MessageBox.Show(Form.ActiveForm, "No 'remarks2' in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!header.Contains("updatedBy"))
            {
                MessageBox.Show(Form.ActiveForm, "No 'updated by' in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!header.Contains("docId"))
            {
                MessageBox.Show(Form.ActiveForm, "No 'doc id' in the hash.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public bool updateInventoryTransfer(Hashtable header) {
            if (!checkHeadersForUpdate(header))
                return false;

            string sql = "UPDATE inventorytransfer SET remarks2=@remarks2,updateDate=DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s'),updatedBy=@updatedBy WHERE docId=@docId";
            cmd = new MySqlCommand(sql, vars.MySqlConnection);
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@docId", header["docId"]);
            cmd.Parameters.AddWithValue("@remarks2", header["remarks2"]);
            cmd.Parameters.AddWithValue("@updatedBy", header["updatedBy"]);

            try 
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex) {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }
    }
}
