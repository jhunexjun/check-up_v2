using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace Check_upTests
{
    public static class vars2
    {
        internal static MySqlConnection MySqlConnection;
        internal struct db_credentials
        {
            internal string server, database, username, password;
        }
    }
}
