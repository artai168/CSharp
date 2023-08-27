using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TopStar_Invoice_Maker_SQLSever.Controllers
{
    class Supplier_Controller : DB.DBSettings
    {
        SqlConnection sqlseverConnection;


        public void ADD_Supplier(Models.Supplier inSupplier)
        {
            string strSql = "dbo.usp_Supplier_Add";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                // Create a Command object to call procedure : usp_Invoice_Add
                SqlCommand cmd = new SqlCommand(strSql, sqlseverConnection);

                Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
                string companyName = specialCharter.validString(inSupplier.Company);

                // Command Type is StoredProcedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@In_Supplier_ID", SqlDbType.NVarChar).Value = inSupplier.Supplier_ID;
                cmd.Parameters.Add("@In_Company", SqlDbType.NVarChar).Value = companyName;
                cmd.Parameters.Add("@In_Currency", SqlDbType.NVarChar).Value = inSupplier.Currency;
                cmd.Parameters.Add("@In_Exchange_Rate", SqlDbType.VarChar).Value = inSupplier.Exchange_Rage;

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

        public List<Models.Supplier> loadList()
        {
            Controllers.DataMapping.Supplier_Table supplier_Table = new DataMapping.Supplier_Table();

            string str_Sql = "SELECT * FROM " + supplier_Table.TableName;
            str_Sql += " ORDER BY " + supplier_Table.Supplier_ID;

            List<Models.Supplier> supplier_List = new List<Models.Supplier>();

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    while (theData.Read())
                    {
                        supplier_List.Add(new Models.Supplier
                        {
                            Supplier_ID = theData[supplier_Table.Supplier_ID].ToString(),
                            Company = theData[supplier_Table.Company].ToString(),
                            Currency = theData[supplier_Table.Currency].ToString(),
                            Exchange_Rage = theData[supplier_Table.Exchange_Rage].ToString()
                        });
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
            }
            return supplier_List;
        }

    }
}
