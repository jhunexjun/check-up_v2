using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Check_up.classes
{
    public class BackupDatabase
    {
        private bool checkConfig(Hashtable ht)
        {
            if (!ht.Contains("datasource") || !ht.Contains("database") || !ht.Contains("username") || !ht.Contains("password"))
            {
                MessageBox.Show(Form.ActiveForm, "Config Error", "Database configuration error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            return true;
        }

        public bool backupDatabase(Hashtable ht, string absolutePath)
        {
            if (!checkConfig(ht))
                return false;

            absolutePath = @"" + absolutePath.Replace(@"\", @"\\");

            try
            {
                StreamWriter file = new StreamWriter(absolutePath);
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.FileName = @"C:\\xampp\\mysql\\bin\\mysqldump";
                proc.RedirectStandardInput = false;
                proc.RedirectStandardOutput = true;
                string cmd = string.Format(@" -u{0} -p{1} -h{2} {3}", ht["username"], ht["password"], ht["datasource"], ht["database"]);
                proc.Arguments = cmd;
                proc.UseShellExecute = false;
                proc.CreateNoWindow = true;
                Process p = Process.Start(proc);
                string res = p.StandardOutput.ReadToEnd();

                file.Write(res);
                p.WaitForExit();
                file.Close();
                p.Close();

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(Form.ActiveForm, "Database backup", e.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
