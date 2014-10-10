using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System;

public class database
{
    private DataTable _dt;
    private MySqlCommand _cmd;
    private MySqlDataAdapter _da;

    public DataTable select(string sql, MySqlConnection con)
    {
        _dt = new DataTable();
        _cmd = new MySqlCommand(sql, con);
        _da = new MySqlDataAdapter(_cmd);
        _da.Fill(_dt);
        return _dt;
    }

    public int executeNonQuery(string sql, MySqlConnection con)
    {
        int _affected = 0;

        _cmd = new MySqlCommand(sql, con);
        try
        {
            _affected = _cmd.ExecuteNonQuery();            
            return _affected;
        }
        catch (MySqlException err)
        {
            MessageBox.Show(err.Number + ": " + err.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 0;
        }
    }
}