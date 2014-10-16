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
            foreach (DataRow row in dt.Rows)
            {

            }
            
            return dt;
        }

        private bool checkNotNullTableRow(DataTable tableRow)
        {
            return true;
        }

        private bool checkNotNullHeaders(Hashtable header)
        {
            if (!header.Contains("docId"))
            {
                MessageBox.Show("Please indicate 'Doc Id' key in the hash.");
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

            return true;
        }

        public bool addInventoryTransfer(Hashtable header, DataTable tableRow)
        {
            if (!checkNotNullHeaders(header))
                return false;

            if (!checkNotNullTableRow(tableRow))
                return false;

            header = formatHeaderParams(header);
            tableRow = formatRowDataParams(tableRow);

            string sql = "START TRANSACTION;";
            sql += "";
            sql = String.Format(sql, header["docId"]);
            sql += "COMMIT;";
            MySqlCommand cmd = new MySqlCommand(sql, vars.MySqlConnection);
            if (cmd.ExecuteNonQuery() > 1)
                return true;
            else
                return false;
        }
    }
}
