using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Check_up.forms
{
    public partial class frmItemSearch : Form
    {
        database db; DataTable dt;
        int i, rowCount; string sql;

        public frmItemSearch()
        {
            InitializeComponent();
        }

        private void frmItemSearch_Load(object sender, EventArgs e)
        {
            sql = "SELECT description FROM itemmasterdata ORDER BY description";
            db = new database(); dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            
            rowCount = dt.Rows.Count;
            string[] description = new string[rowCount];
            string string_description;

            for (i = 0; i < rowCount; i++ )
            {
                string_description = dt.Rows[i][0].ToString().Trim();
                if (string_description != "")
                    description[i] = string_description;
            }

            var source = new AutoCompleteStringCollection();
            source.AddRange(description);

            txtDescription.AutoCompleteCustomSource = source;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (txtDescription.Text.Trim() == "")
            {
                dt.Rows.Clear();
                return;
            }            

            sql = "SET @description = '" + txtDescription.Text.Trim().Replace("*", "%") + "';";
            sql += "SELECT itemCode 'Item Code',Description";
            sql += ",(SELECT netPrice FROM pricelist WHERE itemCode=itemmasterdata.itemCode AND priceListCode=0) 'Purchase Price'";
            sql += ",(SELECT netPrice FROM pricelist WHERE itemCode=itemmasterdata.itemCode AND priceListCode=1) 'Retail Price'";
            sql += ",Vendor,qtyPrPrchsUoM 'Qty/Prchase UoM',qtyPrSaleUoM 'Qty/Sale UoM',prchsUoM 'Purchase UoM',saleUoM 'Sale UoM',minStock 'Min Stock',maxStock 'Max Stock',Deactivated";
            sql += " FROM itemmasterdata WHERE description LIKE CONCAT('%', @description, '%');";
            db = new database(); dt = new DataTable();
            dt = db.select(sql, vars.MySqlConnection);
            dataGridView1.DataSource = dt;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dt.Rows.Clear();
                txtDescription.Clear();
            }
            else
                Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            saveFileDialog1.InitialDirectory = Application.StartupPath;
            saveFileDialog1.Filter = "CSV (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 1;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string location = saveFileDialog1.FileName;
                writeCSV(dataGridView1, location);
            }
        }

        public void writeCSV(DataGridView gridIn, string outputFile)
        {
            //test to see if the DataGridView has any rows
            if (gridIn.RowCount > 0)
            {
                string value = "";
                DataGridViewRow dr = new DataGridViewRow();
                StreamWriter swOut = new StreamWriter(outputFile);

                //write header rows to csv
                for (int i = 0; i <= gridIn.Columns.Count - 1; i++)
                {
                    if (i > 0)
                    {
                        swOut.Write(",");
                    }
                    swOut.Write(gridIn.Columns[i].HeaderText);
                }

                swOut.WriteLine();

                //write DataGridView rows to csv
                for (int j = 0; j <= gridIn.Rows.Count - 1; j++)
                {
                    if (j > 0)
                    {
                        swOut.WriteLine();
                    }

                    dr = gridIn.Rows[j];

                    for (int i = 0; i <= gridIn.Columns.Count - 1; i++)
                    {
                        if (i > 0)
                        {
                            swOut.Write(",");
                        }

                        value = dr.Cells[i].Value.ToString();
                        //replace comma's with spaces
                        value = value.Replace(',', ' ');
                        //replace embedded newlines with spaces
                        value = value.Replace(Environment.NewLine, " ");

                        swOut.Write(value);
                    }
                }
                swOut.Close();
            }
        }
    }
}
