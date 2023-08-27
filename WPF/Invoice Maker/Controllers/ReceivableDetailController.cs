using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TopStar_Invoice_Maker_SQLSever.Controllers
{
    class ReceivableDetailController : DB.DBSettings
    {
        SqlConnection sqlseverConnection;

        public ReceivableDetailController()
        {

        }

        private void ExecuteDB(string inSQL)
        {
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

        public void SaveItem(Models.Receivable_Balance inReceivableBalance)
        {
            Add_Receivable_Procedure(inReceivableBalance);
        }

        private void Add_Receivable_Procedure(Models.Receivable_Balance inReceivableBalance)
        {
            string strSql = "dbo.usp_ReceivableDetail_Add";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                // Create a Command object to call procedure : usp_Invoice_Add
                SqlCommand cmd = new SqlCommand(strSql, sqlseverConnection);

                // Command Type is StoredProcedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@In_Receivable_ID", SqlDbType.NVarChar).Value = inReceivableBalance.Receivable_Record_ID;
                cmd.Parameters.Add("@In_InvoiceID", SqlDbType.NVarChar).Value = inReceivableBalance.InvoiceID;

                try
                {
                    sqlseverConnection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    sqlseverConnection.Close();
                    System.Windows.MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlseverConnection.Dispose();
                }
            }
        }

        public List<string> GetInvoicesIncluded(string inReceivableID)
        {
            Controllers.DataMapping.Receivable_Balance_Table receivable_Table = new DataMapping.Receivable_Balance_Table();

            string str_Sql = "SELECT ";
            str_Sql += " * ";
            str_Sql += "FROM " + receivable_Table.TableName;
            str_Sql += " WHERE " + receivable_Table.Receivable_Record_ID + " = '" + inReceivableID + "'";

            List<string> theResult =new List<string>();

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    while (theData.Read())
                    {
                        theResult.Add(theData[receivable_Table.InvoiceID].ToString());
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlseverConnection.Dispose();
                }
                return theResult;
            }
        }
    }
}
