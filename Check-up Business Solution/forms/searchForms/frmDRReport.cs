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
    public partial class frmDRReport : Form
    {
        DataTable dt;
        database db;
        private frmDialog frmDialogForm;
        StringBuilder sb = new StringBuilder();
        int i, rowCount;
        string focusedDateTxtbox;

        public frmDRReport()
        {
            InitializeComponent();
            txtVendorTo.Leave += new EventHandler(txtVendorTo_Leave);
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

        private void txtVendorTo_Leave(object sender, EventArgs e)
        {
            if (txtVendorTo.Text.Trim() != "")
                linkLabelVendor.Enabled = false;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            btnExport.Enabled = false;

            if (chkDR.Checked == false && chkVendor.Checked == false && chkDate.Checked == false)
            {
                MessageBox.Show(this, "Please choose selection.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT itemCode,description,realBsGrossPrchsPrc,qty,rowGrossTotal FROM deliveryreceipt JOIN deliveryreceipt_item USING(docId) WHERE");
            if (chkDR.Checked == true)
                sb.Append(" docId >= '" + txtDRNoFrm.Text.Trim() + "' AND docId <= '" + txtDRNoTo.Text.Trim() + "'");
            if ((chkDR.Checked == true && chkVendor.Checked == true) || (chkDR.Checked == true && chkDate.Checked == true))
                sb.Append(" AND");
            if (chkVendor.Checked == true)
                sb.Append(" vendorCode >= '" + txtVendorFrm.Text.Trim() + "' AND vendorCode <= '" + txtVendorTo.Text.Trim() + "'");
            if (chkVendor.Checked == true && chkDate.Checked == true)
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

                d = Decimal.Parse(dt.Rows[i]["realBsGrossPrchsPrc"].ToString());
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabelDeliveryReceipt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT docId,postingDate,warehouse,grossTotal,remarks1 FROM deliveryreceipt ORDER BY docId DESC");
            if (txtDRNoFrm.Text.Trim() == "" || txtDRNoTo.Text.Trim() == "")
            {
                frmDialogForm = new frmDialog();
                frmDialogForm.selectedValue = sb.ToString();
                frmDialogForm.ShowDialog();
                if (txtDRNoFrm.Text.Trim() == "")
                    txtDRNoFrm.Text = frmDialogForm.selectedValue;
                else
                    txtDRNoTo.Text = frmDialogForm.selectedValue;

                if (txtDRNoFrm.Text.Trim() != "" && txtDRNoTo.Text.Trim() != "")
                    linkLabelDeliveryReceipt.Enabled = false;
            }
        }

        private void linkLabelVendor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT code,BPName FROM businesspartner WHERE BPType = 0 ORDER BY code DESC, BPName");
            if (txtVendorFrm.Text.Trim() == "" || txtVendorTo.Text.Trim() == "")
            {
                frmDialogForm = new frmDialog();
                frmDialogForm.selectedValue = sb.ToString();
                frmDialogForm.ShowDialog();
                if (txtVendorFrm.Text.Trim() == "")
                    txtVendorFrm.Text = frmDialogForm.selectedValue;
                else
                    txtVendorTo.Text = frmDialogForm.selectedValue;

                if (txtVendorFrm.Text.Trim() != "" && txtVendorTo.Text.Trim() != "")
                    linkLabelVendor.Enabled = false;
            }
        }

        private void chkDeliveryReceipt_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDR.Checked == true)
            {
                txtDRNoFrm.Enabled = true;
                txtDRNoTo.Enabled = true;
                linkLabelDeliveryReceipt.Enabled = true;
            }
            else
            {
                txtDRNoFrm.Enabled = false;
                txtDRNoTo.Enabled = false;
                linkLabelDeliveryReceipt.Enabled = false;
            }

            if (txtDRNoFrm.Text.Trim() != "" && txtDRNoTo.Text.Trim() != "")
                linkLabelDeliveryReceipt.Enabled = false;
        }

        private void chkVendor_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVendor.Checked == true)
            {
                txtVendorFrm.Enabled = true;
                txtVendorTo.Enabled = true;
                linkLabelVendor.Enabled = true;
            }
            else
            {
                txtVendorFrm.Enabled = false;
                txtVendorTo.Enabled = false;
                linkLabelVendor.Enabled = false;
            }

            if (txtVendorFrm.Text.Trim() != "" && txtVendorTo.Text.Trim() != "")
                linkLabelVendor.Enabled = false;
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

        private void txtGRPONoFrm_TextChanged(object sender, EventArgs e)
        {
            if (txtDRNoFrm.Text.Trim() == "")
                linkLabelDeliveryReceipt.Enabled = true;
        }

        private void txtGRPONoTo_TextChanged(object sender, EventArgs e)
        {
            if (txtDRNoTo.Text.Trim() == "")
                linkLabelDeliveryReceipt.Enabled = true;
        }

        private void txtVendorFrm_TextChanged(object sender, EventArgs e)
        {
            if (txtVendorFrm.Text.Trim() == "")
                linkLabelVendor.Enabled = true;
        }

        private void txtVendorTo_TextChanged(object sender, EventArgs e)
        {
            if (txtVendorTo.Text.Trim() == "")
                linkLabelVendor.Enabled = true;
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
