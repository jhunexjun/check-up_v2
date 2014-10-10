using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class convertRole
{
    public static string role(int x)
    {
        if (x == 0)
            return "Superuser";
        else
            return "User";
    }
    
    public static int role(string x)
    {
        if (x == "Superuser")
            return 0;
        else
            return 1;
    }
}
