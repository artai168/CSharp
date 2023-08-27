using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TopStar_Invoice_Maker_SQLSever.Controllers
{
    class DistrictController : DB.DBSettings
    {
        SqlConnection sqlseverConnection;
        public List<string> district_String_List;


        public DistrictController()
        {
            
        }

        public void load_String_List()
        {
            district_String_List = new List<string>();
            Controllers.DataMapping.District_Table district_Table = new DataMapping.District_Table();

            string str_Sql = "SELECT " + district_Table.Sub_District + " FROM " + district_Table.TableName + " ";
            str_Sql += "ORDER BY " + district_Table.Sub_District + " ASC";


            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    while (theData.Read())
                    {
                        district_String_List.Add(theData[district_Table.Sub_District].ToString());
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                finally
                {
                    //accessConnection.Dispose();
                }
            }

        }
    }
}
