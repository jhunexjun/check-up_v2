using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace Check_up.classes
{
    public class ItemMasterData
    {
        private Hashtable formatParams(Hashtable ht)
        {
            if (ht.Contains("description"))
                ht["description"] = "'" + ht["description"] + "'";
            else
                ht["description"] = "null";

            if (ht.Contains("shortName"))
                ht["shortName"] = "'" + ht["shortName"] + "'";
            else
                ht["shortName"] = "null";

            if (ht.Contains("vatable"))
                ht["vatable"] = "'" + ht["vatable"] + "'";
            else
                ht["vatable"] = "null";

            // not yet done

            return ht;
        }

        public bool addItem(Hashtable ht)
        {
            if (!ht.Contains("itemCode"))
            {
                MessageBox.Show("Please indicate item code in the hash.");
                return false;
            }

            ht = formatParams(ht);
            
            return true;
        }
    }
}
