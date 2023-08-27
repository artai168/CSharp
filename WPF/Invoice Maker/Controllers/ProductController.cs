using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TopStar_Invoice_Maker_SQLSever.Controllers
{
    class ProductController : DB.DBSettings
    {
        SqlConnection sqlseverConnection;
        public List<Models.Product> product_List;

        public ProductController()
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

        public void SaveItem(Models.Product inProduct)
        {
            /*
            if (recordExit(inProduct))
            {
                UpdateItem(inProduct);
            }
            else
            {
                AddItem(inProduct);
            }*/
            Add_Product_Procedure(inProduct);
        }

        private void Add_Product_Procedure(Models.Product inProduct)
        {
            string strSql = "dbo.usp_Product_Add";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                // Create a Command object to call procedure : usp_Invoice_Add
                SqlCommand cmd = new SqlCommand(strSql, sqlseverConnection);

                Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
                string product_Desc = specialCharter.validString(inProduct.Product_Descruibtions);

                // Command Type is StoredProcedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@In_Product_ID", SqlDbType.NVarChar).Value = inProduct.Product_ID;
                cmd.Parameters.Add("@In_Brand", SqlDbType.NVarChar).Value = inProduct.Brand;
                cmd.Parameters.Add("@In_Model", SqlDbType.NVarChar).Value = inProduct.Model;
                cmd.Parameters.Add("@In_Product_Desc", SqlDbType.NVarChar).Value = product_Desc;
                cmd.Parameters.Add("@In_Unit", SqlDbType.NVarChar).Value = inProduct.Unit;
                cmd.Parameters.Add("@In_Unit_Price", SqlDbType.NVarChar).Value = inProduct.Unit_Price;
                cmd.Parameters.Add("@In_Cost", SqlDbType.NVarChar).Value = inProduct.Cost;
                
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

        private void UpdateItem(Models.Product inProduct)
        {
            Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
            string product_Desc = specialCharter.validString(inProduct.Product_Descruibtions);

            Controllers.DataMapping.Product_Table product_Table = new DataMapping.Product_Table();
            string str_Sql = "UPDATE " + product_Table.TableName;
            str_Sql += " SET ";
            str_Sql += product_Table.Brand + " = '" + inProduct.Brand + "', ";
            str_Sql += product_Table.Model + " = '" + inProduct.Model + "', ";
            str_Sql += product_Table.Product_Descruibtions + " = '" + product_Desc + "', ";
            str_Sql += product_Table.Unit + " = '" + inProduct.Unit + "', ";
            str_Sql += product_Table.Unit_Price + " = '" + inProduct.Unit_Price + "' ";
            str_Sql += product_Table.Cost + " = '" + inProduct.Cost + "' ";
            str_Sql += "WHERE (" + product_Table.Product_ID + " = '" + inProduct.Product_ID + "')";

            ExecuteDB(str_Sql);
        }

        private void AddItem(Models.Product inProduct)
        {
            Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
            string product_Desc = specialCharter.validString(inProduct.Product_Descruibtions);

            Controllers.DataMapping.Product_Table product_Table = new DataMapping.Product_Table();
            string str_Sql = "INSERT INTO " + product_Table.TableName;
            str_Sql += "( ";
            str_Sql += product_Table.Product_ID + ", ";
            str_Sql += product_Table.Brand + ", ";
            str_Sql += product_Table.Model + ", ";
            str_Sql += product_Table.Product_Descruibtions + ", ";
            str_Sql += product_Table.Unit + ", ";
            str_Sql += product_Table.Cost + ", ";
            str_Sql += product_Table.Unit_Price + " ";
            str_Sql += ") ";
            str_Sql += " VALUES (";
            str_Sql += "'" + inProduct.Product_ID + "', ";
            str_Sql += "'" + inProduct.Brand + "', ";
            str_Sql += "'" + inProduct.Model + "', ";
            str_Sql += "'" + product_Desc + "', ";
            str_Sql += "'" + inProduct.Unit + "', ";
            str_Sql += "'" + inProduct.Cost + "', ";
            str_Sql += "'" + inProduct.Unit_Price + "' ";
            str_Sql += ") ";

            ExecuteDB(str_Sql);
        }

        private bool recordExit(Models.Product inProduct)
        {
            bool result = false;
            Controllers.DataMapping.Product_Table product_Table = new DataMapping.Product_Table();

            string str_Sql = "SELECT * FROM Product";
            str_Sql += " WHERE (" + product_Table.Product_ID + " = '" + inProduct.Product_ID + "')";

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

        public List<string> LoadModelList(string inBrand)
        {
            List<string> resultList = new List<string>();
            Controllers.DataMapping.Product_Table product_Table = new DataMapping.Product_Table();

            string str_Sql = "SELECT " + product_Table.Model + " FROM Product";
            str_Sql += " WHERE " + product_Table.Brand + " = '" + inBrand + "' ";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    while (theData.Read())
                    {
                        resultList.Add(theData[product_Table.Model].ToString());
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
            return resultList;
        }

        public List<string> LoadBrandList()
        {
            List<string> resultList = new List<string>();
            Controllers.DataMapping.Product_Table product_Table = new DataMapping.Product_Table();

            string str_Sql = "SELECT " + product_Table.Brand + " FROM Product";
            str_Sql += " GROUP BY " + product_Table.Brand;

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    while (theData.Read())
                    {
                        resultList.Add(theData[product_Table.Brand].ToString());
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
            return resultList;
        }

        public void loadList()
        {
            string str_Sql = "SELECT * FROM Product";
            Controllers.DataMapping.Product_Table product_Table = new DataMapping.Product_Table();
            product_List = new List<Models.Product>();

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    while (theData.Read())
                    {
                        //System.Windows.MessageBox.Show(theData[clinic_ShortName].ToString() + " --> " + theData[clinic_NAME].ToString() + " : " + theData[clinic_Address].ToString());

                        product_List.Add(new Models.Product
                        {
                            Product_ID = theData[product_Table.Product_ID].ToString(),
                            Brand = theData[product_Table.Brand].ToString(),
                            Model = theData[product_Table.Model].ToString(),
                            Product_Descruibtions = theData[product_Table.Product_Descruibtions].ToString(),
                            Unit = theData[product_Table.Unit].ToString(),
                            Cost = theData[product_Table.Cost].ToString(),
                            Unit_Price = theData[product_Table.Unit_Price].ToString()
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
        }

        public void loadList( string inBrand)
        {
            Controllers.DataMapping.Product_Table product_Table = new DataMapping.Product_Table();

            string str_Sql = $"SELECT * FROM {product_Table.TableName} WHERE {product_Table.Brand} = '{inBrand}';";

            product_List = new List<Models.Product>();

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    while (theData.Read())
                    {

                        product_List.Add(new Models.Product
                        {
                            Product_ID = theData[product_Table.Product_ID].ToString(),
                            Brand = theData[product_Table.Brand].ToString(),
                            Model = theData[product_Table.Model].ToString(),
                            Product_Descruibtions = theData[product_Table.Product_Descruibtions].ToString(),
                            Unit = theData[product_Table.Unit].ToString(),
                            Cost = theData[product_Table.Cost].ToString(),
                            Unit_Price = theData[product_Table.Unit_Price].ToString()
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
        }
    }
}
