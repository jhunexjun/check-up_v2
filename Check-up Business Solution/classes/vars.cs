using MySql.Data.MySqlClient;

namespace Check_up
{
    public static class vars
    {
        public static MySqlConnection MySqlConnection;
        public struct db_credentials
        {
            public string server, database, username, password;
        }

        //set initial values
        public static bool loggedOn = false;
        // public static int user_id = 0;
        public static string username = null;
        public static int role;
        public static string terminalId = "";

        // rounding off to the right of decimal point
        public static int roundOff = 2;

        public static string format = "#,##0.00####";
        public static string grossFormat = "#,##0.00";

        public const string staticSalt = "33~xX";
    
    }
}