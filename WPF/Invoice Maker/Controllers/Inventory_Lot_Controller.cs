using System;
using System.Data;
using System.Data.SqlClient;


namespace TopStar_Invoice_Maker_SQLSever.Controllers
{
    class Inventory_Lot_Controller : DB.DBSettings
    {
        SqlConnection sqlseverConnection;

        public string Get_Max_InventoryNo()
        {
            //database = new DB.DBUtils();
            //sqlseverConnection = new SqlConnection();
            //sqlseverConnection = database.GetDBConnection();

            Controllers.DataMapping.Inventory_Lot_Table inventory_Table = new DataMapping.Inventory_Lot_Table();
            string strResult = "";
            string str_Sql = "SELECT MAX(" + inventory_Table.Lot_ID + ") AS MAX_INV FROM " + inventory_Table.TableName;

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    while (theData.Read())
                    {
                        strResult = theData["MAX_INV"].ToString();
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
            return strResult;
        }

        public bool checkInventoryExit(string inInventoryNo)
        {
            Controllers.DataMapping.Inventory_Lot_Table inventory_Table = new DataMapping.Inventory_Lot_Table();
            bool result = false;
            string str_Sql = $"SELECT * FROM {inventory_Table.TableName} WHERE {inventory_Table.Lot_ID} = '{inInventoryNo}';";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    if (theData.HasRows)
                    {
                        result = true;
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
            return result;
        }

        public Models.Inventory_Lot GetInventory(string inInventoryNo)
        {

            Controllers.DataMapping.Inventory_Lot_Table inventory_Table = new DataMapping.Inventory_Lot_Table();
            Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
            Models.Inventory_Lot result = new Models.Inventory_Lot();
            string str_Sql = $"SELECT ";
            str_Sql += $" {inventory_Table.Lot_ID}, {inventory_Table.Purchase_ID}, {inventory_Table.Supplier_ID}";
            str_Sql += $" ,{inventory_Table.Supplier_Ref}, {inventory_Table.Currency}, {inventory_Table.ExchangeRate}";
            str_Sql += $" , CONVERT(varchar, {inventory_Table.Arrival_Date}, 105) AS {inventory_Table.Arrival_Date}";
            str_Sql += $" FROM {inventory_Table.TableName}";
            str_Sql += $" WHERE {inventory_Table.Lot_ID} = '{inInventoryNo}';";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    while (theData.Read())
                    {
                        result = new Models.Inventory_Lot
                        {
                            Lot_ID = theData[inventory_Table.Lot_ID].ToString(),
                            Purchase_ID = theData[inventory_Table.Purchase_ID].ToString(),
                            Supplier_ID = theData[inventory_Table.Supplier_ID].ToString(),
                            Supplier_Ref = theData[inventory_Table.Supplier_Ref].ToString(),
                            Arrival_Date = specialCharter.theDateFormat(theData[inventory_Table.Arrival_Date].ToString()),
                            Currency = theData[inventory_Table.Currency].ToString(),
                            ExchangeRate = theData[inventory_Table.ExchangeRate].ToString()
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
            return result;
        }

        public void Save_Inventory_Lot(Models.Inventory_Lot inventory_Lot)
        {
            string strSql = "dbo.usp_Inventory_Lot_Add";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                // Create a Command object to call procedure : usp_Invoice_Add
                SqlCommand cmd = new SqlCommand(strSql, sqlseverConnection);

                Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
                //string clinicName = specialCharter.validString(inClinic.Clinic_Name);

                // Command Type is StoredProcedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@in_Lot_ID", SqlDbType.NVarChar).Value = inventory_Lot.Lot_ID;
                cmd.Parameters.Add("@in_Purchase_ID", SqlDbType.NVarChar).Value = inventory_Lot.Purchase_ID;
                cmd.Parameters.Add("@in_Supplier_ID", SqlDbType.NVarChar).Value = inventory_Lot.Supplier_ID;
                cmd.Parameters.Add("@in_Supplier_Ref", SqlDbType.NVarChar).Value = inventory_Lot.Supplier_Ref;
                cmd.Parameters.Add("@in_Currency", SqlDbType.NVarChar).Value = inventory_Lot.Currency;
                cmd.Parameters.Add("@in_ExchangeRate", SqlDbType.SmallMoney).Value = inventory_Lot.ExchangeRate;
                cmd.Parameters.Add("@in_Arrived_Date", SqlDbType.DateTime2).Value = inventory_Lot.Arrival_Date;
               
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
    }
}
