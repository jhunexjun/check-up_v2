using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;

class DBConnect
{
    private MySqlConnection connection;
    private string server;
    private string database;
    private string uid;
    private string password;

    public DBConnect(string srvr, string db, string username, string pw)
    {
        server = srvr;
        database = db;
        uid = username;
        password = pw;
        
        Initialize();
    }
    private void Initialize()
    {
        string connectionString = "SERVER=" + server + ";DATABASE=" + database + ";UID=" + uid + ";PASSWORD=" + password + ";";

        connection = new MySqlConnection(connectionString);        
    }

    public bool openConnection()
    {
        try
        {
            connection.Open();
            return true;
        }
        catch (MySqlException e)
        {
            MessageBox.Show(e.Number + ": " + e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }
    }

    public void closeConnection()
    {
        connection.Clone();
    }
}