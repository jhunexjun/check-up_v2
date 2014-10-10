using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Check_up
{
    public static class fx
    {
        internal static decimal null2Decimal(object x)
        {
            if (x == null || x.ToString() == "")
                return 0;
            else
            {
                try
                {
                    return Decimal.Parse(x.ToString());
                }
                catch
                {
                    return 0;
                }
            }
        }

        internal static string null2EmptyStr(object x)
        {
            if (x == null)
                return "";
            else
                return x.ToString();
        }

        internal static decimal removeComma(object x)
        {
            if (x == null)
                return 0.00m;
            else
            {
                x = x.ToString().Replace(",", "");
                return Decimal.Parse(x.ToString());
            }
        }

        internal static String getTimeStamp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmss");
        }

        // this will decide how many digit to the right of decimal point
        // default format is "#,##0."
        internal static string rndOff(decimal value)
        {
            int n = vars.roundOff;
            string formatString = "#,##0.";
            for (int i = 0; i < n; i++)
                formatString += "#";

            return formatString;
        }

        /* this will remove zeros to the right of a decimal point next after a non-zero digit
         * like 34.378000 will become 34.378
         */
        /* internal static decimal removeZeros(decimal value)
        {
            if (value == 0m)
                return 0.00m;

            if (value.ToString().EndsWith(".00"))
                return value;

            if (value.ToString().EndsWith(".0"))
                return value;

            string s = value.ToString();
            decimal trimmedValue;

            for (int i = (s.Length - 1); i >= 0; i--)
            {
                if (s[i].ToString() != ".")
                {
                    if (s[i].ToString() == "0")
                        s = s.Remove(i);
                    else
                        break;
                }
            }
            return trimmedValue = Convert.ToDecimal(s);
        } */
    }
}