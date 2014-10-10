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
    public partial class frmSalesReport : Form
    {
        DataTable dt;
        database db;
        private frmDialog frmDialogForm;
        StringBuilder sb = new StringBuilder();
        int i, rowCount;
        string focusedDateTxtbox = "";

        public frmSalesReport()
        {
            InitializeComponent();
            txtCustomerTo.Leave += new EventHandler(txtCustomerTo_Leave);
            txtDateFrm.GotFocus += new EventHandler(txtDateFrm_GotFocus);
            txtDateTo.GotFocus += new EventHandler(txtDateTo_GotFocus);
        }

        void txtDateTo_GotFocus(object sender, EventArgs e)
        {
            focusedDateTxtbox = "txtDateTo";
        }

        void txtDateFrm_GotFocus(object sender, EventArgs e)
        {
            focusedDateTxtbox = "txtDateFrm";
        }

        void txtCustomerTo_Leave(object sender, EventArgs e)
        {
            if (txtCustomerTo.Text.Trim() != "")
                linkLabelCustomer.Enabled = false;
        }

        private void frmSearchSales_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            btnExport.Enabled = false;

            if (chkSalesInvoiceNo.Checked == false && chkCustomer.Checked == false && chkDate.Checked == false)
            {
                MessageBox.Show(this, "Please choose selection.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT itemCode,description,realBsGrossSalePrc,qty,rowGrossTotal FROM salesinvoice JOIN salesinvoice_item USING(docId) WHERE");
            if (chkSalesInvoiceNo.Checked == true)
                sb.Append(" docId >= '" + txtSalesInvoiceNoFrm.Text.Trim() + "' AND docId <= '" + txtSalesInvoiceNoTo.Text.Trim() + "'");
            if ((chkSalesInvoiceNo.Checked == true && chkCustomer.Checked == true) || (chkSalesInvoiceNo.Checked == true && chkDate.Checked == true))
                sb.Append(" AND");
            if (chkCustomer.Checked == true)
                sb.Append(" customerCode >= '" + txtCustomerFrm.Text.Trim() + "' AND customerCode <= '" + txtCustomerTo.Text.Trim() + "'");
            if (chkCustomer.Checked == true && chkDate.Checked == true)
                sb.Append(" AND");
            if (chkDate.Checked == true)
            {
                DateTime dTime;
                if (!DateTime.TryParse(txtDateFrm.Text.Trim(), out dTime))
                {
                    MessageBox.Show(this, "Invalid date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDateFrm.Focus();
                    return;
                }
                if (!DateTime.TryParse(txtDateTo.Text.Trim(), out dTime))
                {
                    MessageBox.Show(this, "Invalid date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDateTo.Focus();
                    return;
                }

                DateTime date = DateTime.Parse(txtDateFrm.Text.Trim());
                string strDateFrm = date.ToString("yyyy/MM/dd");

                date = DateTime.Parse(txtDateTo.Text.Trim());
                string strDateTo = date.ToString("yyyy/MM/dd");

                sb.Append(" postingDate >='" + strDateFrm + "' AND postingDate <= '" + strDateTo + "'");
            }


            sb.Append(" ORDER BY docId DESC, indx ASC");
            dt = new DataTable();
            db = new database();
            dt = db.select(sb.ToString(), vars.MySqlConnection);

            ListViewItem lvitem = null;
            rowCount = dt.Rows.Count;
            decimal d;
            for (i = 0; i < rowCount; i++)
            {
                lvitem = new ListViewItem(dt.Rows[i]["itemCode"].ToString());
                lvitem.SubItems.Add(dt.Rows[i]["description"].ToString());

                d = Decimal.Parse(dt.Rows[i]["realBsGrossSalePrc"].ToString());
                lvitem.SubItems.Add(d.ToString(vars.format));

                d = Decimal.Parse(dt.Rows[i]["qty"].ToString());
                lvitem.SubItems.Add(d.ToString(vars.format));

                d = Decimal.Parse(dt.Rows[i]["rowGrossTotal"].ToString());
                lvitem.SubItems.Add(d.ToString(vars.format));
                listView1.Items.AddRange(new ListViewItem[] { lvitem });
            }

            if (listView1.Items.Count > 0)
                btnExport.Enabled = true;

            rowCount = listView1.Columns.Count;
            for (i = 0; i < rowCount; i++)
                listView1.Columns[i].Width = -2;
        }

        private void linkLabelSalesInvoiceNo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT docId,customerCode,customerCode,postingDate,warehouse,grossTotal FROM salesinvoice ORDER BY docId DESC");
            if (txtSalesInvoiceNoFrm.Text.Trim() == "" || txtSalesInvoiceNoTo.Text.Trim() == "")
            {
                frmDialogForm = new frmDialog();
                frmDialogForm.selectedValue = sb.ToString();
                frmDialogForm.ShowDialog();
                if (txtSalesInvoiceNoFrm.Text.Trim() == "")
                    txtSalesInvoiceNoFrm.Text = frmDialogForm.selectedValue;
                else
                    txtSalesInvoiceNoTo.Text = frmDialogForm.selectedValue;

                if (txtSalesInvoiceNoFrm.Text.Trim() != "" && txtSalesInvoiceNoTo.Text.Trim() != "")
                    linkLabelSalesInvoiceNo.Enabled = false;
            }         
        }

        private void linkLabelCustomer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT code,BPName FROM businesspartner WHERE BPType = 1 ORDER BY code DESC, BPName");
            if (txtCustomerFrm.Text.Trim() == "" || txtCustomerTo.Text.Trim() == "")
            {
                frmDialogForm = new frmDialog();
                frmDialogForm.selectedValue = sb.ToString();
                frmDialogForm.ShowDialog();
                if (txtCustomerFrm.Text.Trim() == "")
                    txtCustomerFrm.Text = frmDialogForm.selectedValue;
                else
                    txtCustomerTo.Text = frmDialogForm.selectedValue;

                if (txtCustomerFrm.Text.Trim() != "" && txtCustomerTo.Text.Trim() != "")
                    linkLabelCustomer.Enabled = false;
            }
        }

        private void chkSalesInvoiceNo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSalesInvoiceNo.Checked == true)
            {
                txtSalesInvoiceNoFrm.Enabled = true;
                txtSalesInvoiceNoTo.Enabled = true;
                linkLabelSalesInvoiceNo.Enabled = true;
            }
            else
            {
                txtSalesInvoiceNoFrm.Enabled = false;
                txtSalesInvoiceNoTo.Enabled = false;
                linkLabelSalesInvoiceNo.Enabled = false;
            }

            if (txtSalesInvoiceNoFrm.Text.Trim() != "" && txtSalesInvoiceNoTo.Text.Trim() != "")
                linkLabelSalesInvoiceNo.Enabled = false;
        }

        private void chkCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCustomer.Checked == true)
            {
                txtCustomerFrm.Enabled = true;
                txtCustomerTo.Enabled = true;
                linkLabelCustomer.Enabled = true;
            }
            else
            {
                txtCustomerFrm.Enabled = false;
                txtCustomerTo.Enabled = false;
                linkLabelCustomer.Enabled = false;
            }

            if (txtCustomerFrm.Text.Trim() != "" && txtCustomerTo.Text.Trim() != "")
                linkLabelCustomer.Enabled = false;
        }

        private void txtSalesInvoiceNoFrm_TextChanged(object sender, EventArgs e)
        {
            if (txtSalesInvoiceNoFrm.Text.Trim() == "")
                linkLabelSalesInvoiceNo.Enabled = true;
        }

        private void txtSalesInvoiceNoTo_TextChanged(object sender, EventArgs e)
        {
            if (txtSalesInvoiceNoTo.Text.Trim() == "")
                linkLabelSalesInvoiceNo.Enabled = true;
        }

        private void txtCustomerFrm_TextChanged(object sender, EventArgs e)
        {
            if (txtCustomerFrm.Text.Trim() == "")
                linkLabelCustomer.Enabled = true;
        }

        private void txtCustomerTo_TextChanged(object sender, EventArgs e)
        {
            if (txtCustomerTo.Text.Trim() == "")
                linkLabelCustomer.Enabled = true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = Application.StartupPath;
            saveFileDialog1.Filter = "CSV (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 1;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string location = saveFileDialog1.FileName;
                writeCSV(listView1, location);

                MessageBox.Show(this, "Successfully exported to " + location + ".", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void writeCSV(ListView lv, string path)
        {
            if (lv.Items.Count > 0)
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    foreach (ListViewItem item in lv.Items)
                    {
                        string[] lvRowTexts = new string[] { item.Text, item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[2].Text, item.SubItems[3].Text };
                        string lineOfText = "", text = "";
                        foreach (string value in lvRowTexts)
                        {
                            if (text.Contains(" ") || text.Contains(","))
                                text = '"' + value + '"' + ',';
                            else
                                text = value + ',';

                            lineOfText = lineOfText + text;
                        }

                        lineOfText.Remove(lineOfText.Length - 1);

                        writer.WriteLine(lineOfText);
                    }
                }
            }
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked == true)
            {
                txtDateFrm.Enabled = true;
                txtDateTo.Enabled = true;
                dtPickerDate.Enabled = true;
            }
            else
            {
                txtDateFrm.Enabled = false;
                txtDateTo.Enabled = false;
                dtPickerDate.Enabled = false;
            }

            if (txtDateFrm.Text.Trim() != "" && txtDateTo.Text.Trim() != "")
                dtPickerDate.Enabled = false;
        }

        private void dtPickerDate_ValueChanged(object sender, EventArgs e)
        {
            if (focusedDateTxtbox == "txtDateFrm")
                txtDateFrm.Text = dtPickerDate.Value.ToString("MM/dd/yyyy");
            else if (focusedDateTxtbox == "txtDateTo")
                txtDateTo.Text = dtPickerDate.Value.ToString("MM/dd/yyyy");
            else
                txtDateFrm.Text = dtPickerDate.Value.ToString("MM/dd/yyyy");
        }

        private void txtDateFrm_TextChanged(object sender, EventArgs e)
        {
            dtPickerDate.Enabled = true;
        }

        private void txtDateTo_TextChanged(object sender, EventArgs e)
        {
            dtPickerDate.Enabled = true;
        }
    }
}
