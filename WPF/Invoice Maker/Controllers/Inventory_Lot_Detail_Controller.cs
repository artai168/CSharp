using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TopStar_Invoice_Maker_SQLSever.Controllers
{
    class Inventory_Lot_Detail_Controller : DB.DBSettings
    {
        SqlConnection sqlseverConnection;

        public List<Models.Inventory_Lot_Detail> Load_ItemList(string strLot_ID)
        {
            List<Models.Inventory_Lot_Detail> inventory_Lot_Details = new List<Models.Inventory_Lot_Detail>();

            Controllers.DataMapping.Inventory_Lot_Detail_Table inventory_Detail_Table = new DataMapping.Inventory_Lot_Detail_Table();
            Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();

            string str_Sql = $"SELECT ";
            str_Sql += $" {inventory_Detail_Table.Lot_ID}, {inventory_Detail_Table.Lot_Detial_ID}, {inventory_Detail_Table.Product_ID}, {inventory_Detail_Table.Qty}";
            str_Sql += $" , {inventory_Detail_Table.Unit}, {inventory_Detail_Table.UnitPrice}, {inventory_Detail_Table.NetPrice}, {inventory_Detail_Table.TotalAmount}";
            str_Sql += $" , CONVERT(varchar,{inventory_Detail_Table.Exp_Date}, 105) AS {inventory_Detail_Table.Exp_Date} , {inventory_Detail_Table.Remark}";
            str_Sql += $" FROM {inventory_Detail_Table.TableName} WHERE {inventory_Detail_Table.Lot_ID} = '{strLot_ID}';";


            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    while (theData.Read())
                    {
                        inventory_Lot_Details.Add(new Models.Inventory_Lot_Detail
                        {
                            Lot_ID = theData[inventory_Detail_Table.Lot_ID].ToString(),
                            Lot_Detial_ID = theData[inventory_Detail_Table.Lot_Detial_ID].ToString(),
                            Product_ID = theData[inventory_Detail_Table.Product_ID].ToString(),
                            Qty = theData[inventory_Detail_Table.Qty].ToString(),
                            Unit = theData[inventory_Detail_Table.Unit].ToString(),
                            UnitPrice = theData[inventory_Detail_Table.UnitPrice].ToString(),
                            NetPrice = theData[inventory_Detail_Table.NetPrice].ToString(),
                            TotalAmount = theData[inventory_Detail_Table.TotalAmount].ToString(),
                            Exp_Date = specialCharter.theDateFormat(theData[inventory_Detail_Table.Exp_Date].ToString()),
                            Remark = theData[inventory_Detail_Table.Remark].ToString()
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

            return inventory_Lot_Details;
        }

        public void Save_LotDetail(List<Models.Inventory_Lot_Detail> in_Lot_Detail, string strArrivalDate)
        {
            foreach (Models.Inventory_Lot_Detail inventory_Lot_Detail in in_Lot_Detail)
            {
                save_Lot_Detail_To_DB(inventory_Lot_Detail, strArrivalDate);
            }
        }

        private void save_Lot_Detail_To_DB(Models.Inventory_Lot_Detail inventory_Lot_Detail, string inArrivalDate)
        {
            string strSql = "dbo.usp_Inventory_Lot_Detail_Add";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                // Create a Command object to call procedure : usp_Invoice_Add
                SqlCommand cmd = new SqlCommand(strSql, sqlseverConnection);

                Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
                string strExp_Date = specialCharter.SQL_DateTime(inventory_Lot_Detail.Exp_Date);
                string strArrivalDate = specialCharter.SQL_DateTime(inArrivalDate);
                //string clinicName = specialCharter.validString(inClinic.Clinic_Name);

                decimal totalAmount = Convert.ToDecimal(inventory_Lot_Detail.NetPrice) * Convert.ToInt16(inventory_Lot_Detail.Qty);

                // Command Type is StoredProcedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@in_Lot_ID", SqlDbType.NVarChar).Value = inventory_Lot_Detail.Lot_ID;
                cmd.Parameters.Add("@in_Product_ID", SqlDbType.NVarChar).Value = inventory_Lot_Detail.Product_ID;
                cmd.Parameters.Add("@in_Qty", SqlDbType.Int).Value = inventory_Lot_Detail.Qty;
                cmd.Parameters.Add("@in_Unit", SqlDbType.NVarChar).Value = inventory_Lot_Detail.Unit;
                cmd.Parameters.Add("@in_UnitPrice", SqlDbType.Money).Value = inventory_Lot_Detail.UnitPrice;
                cmd.Parameters.Add("@in_NetPrice", SqlDbType.Money).Value = inventory_Lot_Detail.NetPrice;
                cmd.Parameters.Add("@in_TotalAmount", SqlDbType.Money).Value = totalAmount;
                cmd.Parameters.Add("@in_Exp_Date", SqlDbType.DateTime2).Value = strExp_Date;
                cmd.Parameters.Add("@in_Remark", SqlDbType.NVarChar).Value = inventory_Lot_Detail.Remark;
                cmd.Parameters.Add("@in_Arrived_Date", SqlDbType.DateTime2).Value = strArrivalDate;
                
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

        public Models.Inventory_Lot_Detail Get_Price_in_Inventory_Lot_Detail(string inProductID)
        {
            Models.Inventory_Lot_Detail inventory_Lot_Detail = new Models.Inventory_Lot_Detail();
            Controllers.DataMapping.Inventory_Lot_Detail_Table inventory_Detail_Table = new DataMapping.Inventory_Lot_Detail_Table();
            Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();

            string str_Sql = $"SELECT {inventory_Detail_Table.NetPrice}, {inventory_Detail_Table.UnitPrice}";
            str_Sql += $" FROM {inventory_Detail_Table.TableName}";
            str_Sql += $" WHERE {inventory_Detail_Table.Lot_Detial_ID} IN (";
            str_Sql += $" SELECT MAX({inventory_Detail_Table.Lot_Detial_ID})";
            str_Sql += $" FROM {inventory_Detail_Table.TableName}";
            str_Sql += $" WHERE {inventory_Detail_Table.Product_ID} = '{inProductID}');";

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
            return inventory_Lot_Detail;
        }


    }
}
