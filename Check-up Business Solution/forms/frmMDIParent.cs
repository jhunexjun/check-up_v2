using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Net;
using System.Diagnostics;
using Check_up.classes;
using System.Collections;
using MySql.Data.MySqlClient;

namespace Check_up.forms
{
    public partial class frmMDIParent : Form
    {
        public static bool forceCloseApp = false;

        public frmMDIParent()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(frmMDIParent_FormClosing);
            loginToolStripMenuItem.TextChanged += new EventHandler(loginToolStripMenuItem_TextChanged);
        }

        void loginToolStripMenuItem_TextChanged(object sender, EventArgs e)
        {
            if (loginToolStripMenuItem.Text == "&Log-out")
            {
                toolStripStatusLabel1.Text = "server: " + vars.MySqlConnection.DataSource;
                toolStripStatusLabel2.Text = "  Database: " + vars.MySqlConnection.Database;
                toolStripStatusLabel3.Text = "  Username: " + vars.username;
            }
            else
            {
                toolStripStatusLabel1.Text = "";
                toolStripStatusLabel2.Text = "";
                toolStripStatusLabel3.Text = "";
            }
        }

        void frmMDIParent_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!forceCloseApp)
            {
                if (DialogResult.Yes == MessageBox.Show("Are you sure you want to close this application?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    e.Cancel = false;
                else
                    e.Cancel = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName + " 2014";            

            loginToolStripMenuItem_Click(sender, e);
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmLogin = new frmLogin();
            if (!frmOpen("frmLogin") && this.loginToolStripMenuItem.Text == "&Log-in")
            {                
                frmLogin.MdiParent = this;
                frmLogin.Show();
                return;
            }

            if (this.loginToolStripMenuItem.Text == "&Log-out")
            {
                if (DialogResult.Yes == MessageBox.Show("Are you sure you want to log-out?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    Form[] childForms = this.MdiChildren;
                    foreach (Form childForm in childForms)
                    {
                        if (childForm.Name != "frmLogin")
                            childForm.Close();
                    }

                    loginToolStripMenuItem.Text = "&Log-in";
                    modulesToolStripMenuItem.Enabled = false;
                    toolsToolStripMenuItem.Enabled = false;
                    this.toolStripStatusLabel1.Text = "";
                    this.toolStripStatusLabel3.Text = "";

                    vars.loggedOn = false;
                    vars.role = 0;
                    vars.username = "";

                    frmLogin.MdiParent = this;
                    frmLogin.Show();
                }
            }
        }

        private bool frmOpen(string frmNameProperty)
        {
            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                if (frm.Name == frmNameProperty)
                    return true;
            }
            return false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmUsers();
            f.MdiParent = this;
            f.Show();
        }

        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmEmployees();
            f.MdiParent = this;
            f.Show();
        }

        private void warehouseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form f = new frmWarehouse();
            f.MdiParent = this;
            f.Show();
        }

        private void businessPartnersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmBusinessPartners();
            f.MdiParent = this;
            f.Show();
        }

        private void itemMasterrDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmItemMasterData();
            f.MdiParent = this;
            f.Show();
        }

        private void poToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmPO();
            f.MdiParent = this;
            f.Show();
        }

        private void acToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmSalesInvoice();
            f.MdiParent = this;
            f.Show();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Form f = new frmGRPO();
            f.MdiParent = this;
            f.Show();
        }

        private void returnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmSalesReturn();
            f.MdiParent = this;
            f.Show();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            Form f = new frmGoodsReturn();
            f.MdiParent = this;
            f.Show();
        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmInventoryTransfer();
            f.MdiParent = this;
            f.Show();
        }

        private void uploadRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmExportDBRecords();
            f.MdiParent = this;
            f.Show();
        }

        private void transmitRecordsToServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmTransmitRecToSvr();
            f.MdiParent = this;
            f.Show();
        }

        private void downloadRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmImportFiles();
            f.MdiParent = this;
            f.Show();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            Form f = new frmItemSearch();
            f.MdiParent = this;
            f.Show();
        }

        private void salesReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmSalesReport();
            f.MdiParent = this;
            f.Show();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string downloadFullUrl = "";
            var filenames = new List<string>();
            Version newVersion = null;
            string xmlUrl = "http://jhunexjun.hostei.com/check-up/updates.xml";
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(xmlUrl);
                reader.MoveToContent();
                string elementName = ""; // the tag
                if ((reader.NodeType == XmlNodeType.Element) && reader.Name == "check-up")
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                            elementName = reader.Name;
                        else
                        {
                            if ((reader.NodeType == XmlNodeType.Text) && (reader.HasValue))
                            {
                                switch (elementName)
                                {
                                    case "version":
                                        newVersion = new Version(reader.Value);
                                        break;
                                    case "downloadUrl":
                                        downloadFullUrl = reader.Value;
                                        break;
                                    case "filename":
                                        filenames.Add(reader.Value);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            Version applicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (applicationVersion.CompareTo(newVersion) < 0)
            {
                DialogResult dr = MessageBox.Show(this, "Version " + newVersion.ToString() + " of Check-up is now available. Do you want to download it now?", "New version", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    Uri baseUri = new Uri(downloadFullUrl);
                    if (baseUri.Scheme != Uri.UriSchemeFtp)
                    {
                        MessageBox.Show(this, "Uri is not a valid format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
                    folderBrowserDialog1.ShowDialog();

                    if (folderBrowserDialog1.SelectedPath != "")
                    {
                        Uri fullUri = null; Uri relativeUri = null;
                        int i = filenames.Count, c = 0;

                        foreach (string filename in filenames)
                        {
                            relativeUri = new Uri(filename, UriKind.Relative);
                            fullUri = new Uri(baseUri, relativeUri);
                            ftp ftpDownload = new ftp("a2278315", "Systems33");

                            if (ftpDownload.download(fullUri.ToString(), folderBrowserDialog1.SelectedPath + @"\" + relativeUri.ToString()))
                            {
                                // let's not use this for now
                                //unInstallSoftware();
                                
                                c++;
                            }
                        }

                        if (c == 0)
                            MessageBox.Show(this, "Nothing has been downloaded. Try downloading again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (c > 0 && c == i)
                            MessageBox.Show(this, "All updates has been successfully downloaded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else if (c > 0 && c  < i)
                            MessageBox.Show(this, "One or more updates were not successully downloaded. Please download all.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                        MessageBox.Show(this, "No location has been selected. Downloading unsuccessful. Re-do the process if you want to download it.", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
                MessageBox.Show(this, "This software is up-to-date.", "Message.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // let's not use this for now
        private void unInstallSoftware()
        {
            Process p = new Process(); 
            p.StartInfo.FileName = "msiexec.exe"; 
            p.StartInfo.Arguments = "/x \"" + Application.StartupPath + "\\updates\\Check-up Business Solutions.msi\"/qf";
            MessageBox.Show(p.StartInfo.ToString());
            p.Start(); 
        }

        private void purchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmPOReport();
            f.MdiParent = this;
            f.Show();
        }

        private void goodsReceiptPOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmGRPOReport();
            f.MdiParent = this;
            f.Show();
        }

        private void inventoryTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmInventoryTransferReport();
            f.MdiParent = this;
            f.Show();
        }

        private void deliveryReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmDeliveryReceipt();
            f.MdiParent = this;
            f.Show();
        }

        private void inventoryPostingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmInventoryPosting();
            f.MdiParent = this;
            f.Show();
        }

        private void salesReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmSalesReturnReport();
            f.MdiParent = this;
            f.Show();
        }

        private void drToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new frmDRReport();
            f.MdiParent = this;
            f.Show();
        }

        private void inventoryPostingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form f = new frmInventoryPostingReport();
            f.MdiParent = this;
            f.Show();
        }

        private void backupDatabaseRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to backup database now?", "Backup Database", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                return;

            Hashtable ht = new Hashtable();

            try
            {
                using (StreamReader sr = new StreamReader("check-up.ini"))
                {
                    string line; int position;

                    while ((line = sr.ReadLine()) != null)
                    {
                        position = line.IndexOf("=");
                        if (line.StartsWith("datasource"))
                            ht["datasource"] = line.Substring(position + 1);
                        if (line.StartsWith("database"))
                            ht["database"] = line.Substring(position + 1);
                        if (line.StartsWith("username"))
                            ht["username"] = line.Substring(position + 1);
                        if (line.StartsWith("password"))
                        {
                            ht["password"] = line.Substring(position + 1);
                            ht["password"] = CryptorEngine.Decrypt(ht["password"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            BackupDatabase dbBackup = new BackupDatabase();
            string timeStamp = fx.getTimeStamp(DateTime.Now);

            string absolutePath = Application.StartupPath + @"\database\" + timeStamp + ".sql";
            if (dbBackup.backupDatabase(ht, absolutePath))
                MessageBox.Show(this, "Successfully dumped database records to " + absolutePath + ".", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
