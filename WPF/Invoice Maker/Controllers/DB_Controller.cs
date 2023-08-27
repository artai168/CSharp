using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace TopStar_Invoice_Maker_SQLSever.Controllers
{
    class DB_Controller : DB.DBSettings
    {
        SqlConnection sqlseverConnection;

        protected void ExecuteDB(string inSQL)
        {
            sqlseverConnection = new SqlConnection();
            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                try
                {
                    sqlseverConnection.Open();
                    SqlCommand cmd = new SqlCommand(inSQL, sqlseverConnection);
                    cmd.ExecuteNonQuery();
                    sqlseverConnection.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlseverConnection.Dispose();
                }
            }
        }
    }
}
