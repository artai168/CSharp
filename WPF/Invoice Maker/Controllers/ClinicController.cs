using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TopStar_Invoice_Maker_SQLSever.Controllers
{
    class ClinicController: DB.DBSettings
    {
        SqlConnection sqlseverConnection;
        public List<Models.Clinic> clinic_List;

        public ClinicController()
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

        public void SaveItem(Models.Clinic inClinic)
        {
            /*
            if (recordExit(inClinic))
            {
                UpdateItem(inClinic);
            }
            else
            {
                AddItem(inClinic);
            }*/
            Add_Clinic_Procedure(inClinic);
        }

        private void Add_Clinic_Procedure(Models.Clinic inClinic)
        {
            string strSql = "dbo.usp_Clinic_Add";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                // Create a Command object to call procedure : usp_Invoice_Add
                SqlCommand cmd = new SqlCommand(strSql, sqlseverConnection);

                Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
                string clinicName = specialCharter.validString(inClinic.Clinic_Name);
                string clinicAddress = specialCharter.validString(inClinic.Address);
                string contactPerson = specialCharter.validString(inClinic.ContactPerson);
                string ship_clinicName = specialCharter.validString(inClinic.SHIPTO_Clinic_Name);
                string ship_clinicAddress = specialCharter.validString(inClinic.SHIPTO_Address);
                string ship_contactPerson = specialCharter.validString(inClinic.SHIPTO_ContactPerson);

                // Command Type is StoredProcedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@In_Clinic_ID", SqlDbType.NVarChar).Value = inClinic.Clinic_ID;
                cmd.Parameters.Add("@In_Sales_Person", SqlDbType.NVarChar).Value = inClinic.SalesPerson;
                cmd.Parameters.Add("@In_Clinic", SqlDbType.NVarChar).Value = clinicName;
                cmd.Parameters.Add("@In_Address", SqlDbType.NVarChar).Value = clinicAddress;
                cmd.Parameters.Add("@In_Contact", SqlDbType.NVarChar).Value = contactPerson;
                cmd.Parameters.Add("@In_Tel", SqlDbType.NVarChar).Value = inClinic.Telephone;
                cmd.Parameters.Add("@In_Ship_Clinic", SqlDbType.NVarChar).Value = ship_clinicName;
                cmd.Parameters.Add("@In_Ship_Address", SqlDbType.NVarChar).Value = ship_clinicAddress;
                cmd.Parameters.Add("@In_Ship_Tel", SqlDbType.NVarChar).Value = inClinic.SHIPTO_Telephone;
                cmd.Parameters.Add("@In_Ship_Contact", SqlDbType.NVarChar).Value = ship_contactPerson;
                cmd.Parameters.Add("@In_Delivery", SqlDbType.NVarChar).Value = inClinic.Delivery;
                cmd.Parameters.Add("@In_Terms", SqlDbType.NVarChar).Value = inClinic.Payment_Terms;
                cmd.Parameters.Add("@In_Discount", SqlDbType.NVarChar).Value = inClinic.Discount;
                cmd.Parameters.Add("@In_District", SqlDbType.NVarChar).Value = inClinic.District;

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

        private void UpdateItem(Models.Clinic inClinic)
        {
            Controllers.DataMapping.Clinic_Table clinic_Table = new DataMapping.Clinic_Table();
            Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
            string clinicName = specialCharter.validString(inClinic.Clinic_Name);
            string clinicAddress = specialCharter.validString(inClinic.Address);
            string contactPerson = specialCharter.validString(inClinic.ContactPerson);
            string ship_clinicName = specialCharter.validString(inClinic.SHIPTO_Clinic_Name);
            string ship_clinicAddress = specialCharter.validString(inClinic.SHIPTO_Address);
            string ship_contactPerson = specialCharter.validString(inClinic.SHIPTO_ContactPerson);

            string str_Sql = "UPDATE " + clinic_Table.TableName;
            str_Sql += " SET ";
            str_Sql += clinic_Table.Clinic_Name + " = '" + clinicName + "', ";
            str_Sql += clinic_Table.Address + " = '" + clinicAddress + "', ";
            str_Sql += clinic_Table.ContactPerson + " = '" + contactPerson + "', ";
            str_Sql += clinic_Table.Telephone + " = '" + inClinic.Telephone + "', ";
            str_Sql += clinic_Table.SHIPTO_Clinic_Name + " = '" + ship_clinicName + "', ";
            str_Sql += clinic_Table.SHIPTO_Address + " = '" + ship_clinicAddress + "', ";
            str_Sql += clinic_Table.SHIPTO_ContactPerson + " = '" + ship_contactPerson + "', ";
            str_Sql += clinic_Table.SHIPTO_Telephone + " = '" + inClinic.SHIPTO_Telephone + "', ";
            str_Sql += clinic_Table.Delivery + " = '" + inClinic.Delivery + "', ";
            str_Sql += clinic_Table.District + " = '" + inClinic.District + "', ";
            str_Sql += clinic_Table.SalesPerson + " = '" + inClinic.SalesPerson + "', ";
            str_Sql += clinic_Table.Discount + " = '" + inClinic.Discount + "', ";
            str_Sql += clinic_Table.Payment_Terms + " = '" + inClinic.Payment_Terms + "' ";
            str_Sql += "WHERE (" + clinic_Table.Clinic_ID + " = '" + inClinic.Clinic_ID + "')";

            ExecuteDB(str_Sql);
        }

        private void AddItem(Models.Clinic inClinic)
        {

            Controllers.DataMapping.Clinic_Table clinic_Table = new DataMapping.Clinic_Table();
            Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
            string clinicName = specialCharter.validString(inClinic.Clinic_Name);
            string clinicAddress = specialCharter.validString(inClinic.Address);
            string contactPerson = specialCharter.validString(inClinic.ContactPerson);
            string ship_clinicName = specialCharter.validString(inClinic.SHIPTO_Clinic_Name);
            string ship_clinicAddress = specialCharter.validString(inClinic.SHIPTO_Address);
            string ship_contactPerson = specialCharter.validString(inClinic.SHIPTO_ContactPerson);

            string str_Sql = "INSERT INTO " + clinic_Table.TableName;
            str_Sql += "( ";
            str_Sql += clinic_Table.Clinic_ID + ", ";
            str_Sql += clinic_Table.Clinic_Name + ", ";
            str_Sql += clinic_Table.Address + ", ";
            str_Sql += clinic_Table.ContactPerson + ", ";
            str_Sql += clinic_Table.Telephone + ", ";
            str_Sql += clinic_Table.SHIPTO_Clinic_Name + ", ";
            str_Sql += clinic_Table.SHIPTO_Address + ", ";
            str_Sql += clinic_Table.SHIPTO_ContactPerson + ", ";
            str_Sql += clinic_Table.SHIPTO_Telephone + ", ";
            str_Sql += clinic_Table.Delivery + ", ";
            str_Sql += clinic_Table.District + ", ";
            str_Sql += clinic_Table.SalesPerson + ", ";
            str_Sql += clinic_Table.Discount + ", ";
            str_Sql += clinic_Table.Payment_Terms + " ";
            str_Sql += ") ";
            str_Sql += "VALUES (";
            str_Sql += " '" + inClinic.Clinic_ID + "', ";
            str_Sql += " '" + clinicName + "', ";
            str_Sql += " '" + clinicAddress + "', ";
            str_Sql += " '" + contactPerson + "', ";
            str_Sql += " '" + inClinic.Telephone + "', ";
            str_Sql += " '" + ship_clinicName + "', ";
            str_Sql += " '" + ship_clinicAddress + "', ";
            str_Sql += " '" + ship_contactPerson + "', ";
            str_Sql += " '" + inClinic.SHIPTO_Telephone + "', ";
            str_Sql += " '" + inClinic.Delivery + "', ";
            str_Sql += " '" + inClinic.District + "', ";
            str_Sql += " '" + inClinic.SalesPerson + "', ";
            str_Sql += " '" + inClinic.Discount + "', ";
            str_Sql += " '" + inClinic.Payment_Terms + "' ";
            str_Sql += ") ";

            ExecuteDB(str_Sql);
        }

        private bool recordExit(Models.Clinic inClinic)
        {
            bool result = false;
            Controllers.DataMapping.Clinic_Table clinic_Table = new DataMapping.Clinic_Table();

            string str_Sql = "SELECT * FROM " + clinic_Table.TableName;
            str_Sql += " WHERE (" + clinic_Table.Clinic_ID + " = '" + inClinic.Clinic_ID + "')";

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

        public void loadList()
        {
            Controllers.DataMapping.Clinic_Table clinic_Table = new DataMapping.Clinic_Table();

            string str_Sql = "SELECT * FROM " + clinic_Table.TableName;
            str_Sql += " ORDER BY " + clinic_Table.Clinic_ID;

            clinic_List = new List<Models.Clinic>();

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


                        clinic_List.Add(new Models.Clinic
                        {
                            Clinic_ID = theData[clinic_Table.Clinic_ID].ToString(),
                            Clinic_Name = theData[clinic_Table.Clinic_Name].ToString(),
                            Address = theData[clinic_Table.Address].ToString(),
                            ContactPerson = theData[clinic_Table.ContactPerson].ToString(),
                            Telephone = theData[clinic_Table.Telephone].ToString(),
                            SHIPTO_Clinic_Name = theData[clinic_Table.SHIPTO_Clinic_Name].ToString(),
                            SHIPTO_Address = theData[clinic_Table.SHIPTO_Address].ToString(),
                            SHIPTO_ContactPerson = theData[clinic_Table.SHIPTO_ContactPerson].ToString(),
                            SHIPTO_Telephone = theData[clinic_Table.SHIPTO_Telephone].ToString(),
                            Delivery = theData[clinic_Table.Delivery].ToString(),
                            District = theData[clinic_Table.District].ToString(),
                            SalesPerson = theData[clinic_Table.SalesPerson].ToString(),
                            Discount = theData[clinic_Table.Discount].ToString(),
                            Payment_Terms = theData[clinic_Table.Payment_Terms].ToString()
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
