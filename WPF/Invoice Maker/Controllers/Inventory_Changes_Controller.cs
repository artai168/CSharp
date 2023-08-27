using System.Data.SqlClient;

namespace TopStar_Invoice_Maker_SQLSever.Controllers
{
    class Inventory_Changes_Controller :DB.DBSettings
    {
        SqlConnection sqlseverConnection;

        /*
        public List<Models.Inventory_Changes> List_Products(string inYear, string inMonth)
        {
            List<Models.Inventory_Changes> product_List = new List<Models.Inventory_Changes>();
            Controllers.DataMapping.Inventory_Changes_Table inventory_Changes_Table = new DataMapping.Inventory_Changes_Table();

            string str_Sql = $"SELECT SUM({inventory_Changes_Table.Qty})";
            str_Sql += $" FROM {inventory_Changes_Table.TableName}";
            str_Sql += $" WHERE ({inventory_Changes_Table.Product_ID} = @in_Product_ID ";
            str_Sql += $" AND ( (Month([{inventory_Changes_Table.Changes_Date}])={inMonth}) AND ((Year([{inventory_Changes_Table.Changes_Date}])={inYear}));";

            //((Month([INVOICE_DATE]))= " & inMonth & ") AND ((Year([INVOICE_DATE]))= " & inYear & ")"

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    while (theData.Read())
                    {
                        inventory_Lot_Detail = new Models.Inventory_Lot_Detail
                        {
                            UnitPrice = theData[inventory_Detail_Table.UnitPrice].ToString(),
                            NetPrice = theData[inventory_Detail_Table.NetPrice].ToString()
                        };
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

            return product_List;
        }
        */
    }
}
