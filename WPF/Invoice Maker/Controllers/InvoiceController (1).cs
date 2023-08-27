using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers
{
    class InvoiceController : DB.DBSettings
    {
        SqlConnection sqlseverConnection;
        public List<Models.Invoice> clinic_List;

        public InvoiceController()
        {
            clinic_List = new List<Models.Invoice>();
        }

        private void ExecuteDB(string inSQL)
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

        private void Add_Invoice_Procedure(Models.Invoice inInvoice)
        {
            string strSql = "dbo.usp_Invoice_Add";
            
            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                // Create a Command object to call procedure : usp_Invoice_Add
                SqlCommand cmd = new SqlCommand(strSql, sqlseverConnection);

                Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
                string remark = specialCharter.validString(inInvoice.Remark);
                string clinicName = specialCharter.validString(inInvoice.Clinic_Name);
                string clinicAddress = specialCharter.validString(inInvoice.Address);
                string contactPerson = specialCharter.validString(inInvoice.ContactPerson);
                string ship_clinicName = specialCharter.validString(inInvoice.SHIPTO_Clinic_Name);
                string ship_clinicAddress = specialCharter.validString(inInvoice.SHIPTO_Address);
                string ship_contactPerson = specialCharter.validString(inInvoice.SHIPTO_ContactPerson);
                string invoice_Date = specialCharter.SQL_DateTime(inInvoice.Invoice_Date);
                string due_Date = specialCharter.SQL_DateTime(inInvoice.Due_Date);

                // Command Type is StoredProcedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@In_Invoice_No", SqlDbType.NVarChar).Value = inInvoice.Invoice_ID;
                cmd.Parameters.Add("@In_Invoice_Date", SqlDbType.DateTime2).Value = invoice_Date;
                cmd.Parameters.Add("@In_Sales_Person", SqlDbType.NVarChar).Value = inInvoice.Sales_Person;
                cmd.Parameters.Add("@In_Remark", SqlDbType.NVarChar).Value = remark;
                cmd.Parameters.Add("@In_Customer_ID", SqlDbType.NVarChar).Value = inInvoice.Clinic_ID;
                cmd.Parameters.Add("@In_Clinic", SqlDbType.NVarChar).Value = clinicName;
                cmd.Parameters.Add("@In_Address", SqlDbType.NVarChar).Value = clinicAddress;
                cmd.Parameters.Add("@In_Contact", SqlDbType.NVarChar).Value = contactPerson;
                cmd.Parameters.Add("@In_Tel", SqlDbType.NVarChar).Value = inInvoice.Telephone;
                cmd.Parameters.Add("@In_Ship_Clinic", SqlDbType.NVarChar).Value = ship_clinicName;
                cmd.Parameters.Add("@In_Ship_Address", SqlDbType.NVarChar).Value = ship_clinicAddress;
                cmd.Parameters.Add("@In_Ship_Tel", SqlDbType.NVarChar).Value = inInvoice.SHIPTO_Telephone;
                cmd.Parameters.Add("@In_Ship_Contact", SqlDbType.NVarChar).Value = ship_contactPerson;
                cmd.Parameters.Add("@In_Delivery", SqlDbType.NVarChar).Value = inInvoice.Delivery;
                cmd.Parameters.Add("@In_Terms", SqlDbType.NVarChar).Value = inInvoice.Payment_Terms;
                cmd.Parameters.Add("@In_District", SqlDbType.NVarChar).Value = inInvoice.District;
                cmd.Parameters.Add("@In_Customer_Ref_No", SqlDbType.NVarChar).Value = inInvoice.CustomerReferenceCode;
                cmd.Parameters.Add("@In_Discount", SqlDbType.NVarChar).Value = inInvoice.Discount;
                cmd.Parameters.Add("@In_Sub_Total", SqlDbType.NVarChar).Value = inInvoice.Sub_Total;
                cmd.Parameters.Add("@In_Due_Date", SqlDbType.DateTime2).Value = due_Date;
                cmd.Parameters.Add("@In_Inv_Status", SqlDbType.NVarChar).Value = inInvoice.INV_Status;

                try {
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

        private void Add_Invoice(Models.Invoice inInvoice)
        {
            Controllers.DataMapping.Invoice_Table clinic_Table = new DataMapping.Invoice_Table();
            Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
            string remark = specialCharter.validString(inInvoice.Remark);
            string clinicName = specialCharter.validString(inInvoice.Clinic_Name);
            string clinicAddress = specialCharter.validString(inInvoice.Address);
            string contactPerson = specialCharter.validString(inInvoice.ContactPerson);
            string ship_clinicName = specialCharter.validString(inInvoice.SHIPTO_Clinic_Name);
            string ship_clinicAddress = specialCharter.validString(inInvoice.SHIPTO_Address);
            string ship_contactPerson = specialCharter.validString(inInvoice.SHIPTO_ContactPerson);
            string invoice_Date = specialCharter.SQL_DateTime(inInvoice.Invoice_Date);
            string due_Date = specialCharter.SQL_DateTime(inInvoice.Due_Date);

            string str_Sql = "INSERT INTO " + clinic_Table.TableName;
            str_Sql += " ( ";
            str_Sql += clinic_Table.Invoice_ID + ", ";
            str_Sql += clinic_Table.Invoice_Date + ", ";
            str_Sql += clinic_Table.Remark + ", ";
            str_Sql += clinic_Table.Sales_Person + ", ";
            str_Sql += clinic_Table.Clinic_Name + ", ";
            str_Sql += clinic_Table.Clinic_ID + ", ";
            str_Sql += clinic_Table.Address + ", ";
            str_Sql += clinic_Table.ContactPerson + ", ";
            str_Sql += clinic_Table.Telephone + ", ";
            str_Sql += clinic_Table.SHIPTO_Clinic_Name + ", ";
            str_Sql += clinic_Table.SHIPTO_Address + ", ";
            str_Sql += clinic_Table.SHIPTO_ContactPerson + ", ";
            str_Sql += clinic_Table.SHIPTO_Telephone + ", ";
            str_Sql += clinic_Table.Payment_Terms + ", ";
            str_Sql += clinic_Table.District + ", ";
            str_Sql += clinic_Table.CustomerReferenceCode + ", ";
            str_Sql += clinic_Table.Discount + ", ";
            str_Sql += clinic_Table.Sub_Total + ", ";
            str_Sql += clinic_Table.Total + ", ";
            str_Sql += clinic_Table.Due_Date + ", ";
            str_Sql += clinic_Table.INV_Status + ", ";
            str_Sql += clinic_Table.Delivery + " ";
            str_Sql += ") ";
            str_Sql += "VALUES (";
            str_Sql += "'" + inInvoice.Invoice_ID + "', ";
            str_Sql += "'" + invoice_Date + "', ";
            str_Sql += "'" + remark + "', ";
            str_Sql += "'" + inInvoice.Sales_Person + "', ";
            str_Sql += "'" + clinicName + "', ";
            str_Sql += "'" + inInvoice.Clinic_ID + "', ";
            str_Sql += "'" + clinicAddress + "', ";
            str_Sql += "'" + contactPerson + "', ";
            str_Sql += "'" + inInvoice.Telephone + "', ";
            str_Sql += "'" + ship_clinicName + "', ";
            str_Sql += "'" + ship_clinicAddress + "', ";
            str_Sql += "'" + ship_contactPerson + "', ";
            str_Sql += "'" + inInvoice.SHIPTO_Telephone + "', ";
            str_Sql += "'" + inInvoice.Payment_Terms + "', ";
            str_Sql += "'" + inInvoice.District + "', ";
            str_Sql += "'" + inInvoice.CustomerReferenceCode + "', ";
            str_Sql += "'" + inInvoice.Discount + "', ";
            str_Sql += "'" + inInvoice.Sub_Total + "', ";
            str_Sql += "'" + inInvoice.Total + "', ";
            str_Sql += "'" + due_Date + "', ";
            str_Sql += "'" + inInvoice.INV_Status + "', ";
            str_Sql += "'" + inInvoice.Delivery + "' ";
            str_Sql += ") ";
            ExecuteDB(str_Sql);
        }

        private void Add_Invoice_Item(Models.Invoice_Item inInvoice_Item)
        {
            Controllers.DataMapping.Invoice_Item_Table invoice_Item_Table = new DataMapping.Invoice_Item_Table();
            Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
            string brand = specialCharter.validString(inInvoice_Item.Brand);
            string description = specialCharter.validString(inInvoice_Item.Product_Descruibtions);
            string remark = specialCharter.validString(inInvoice_Item.Item_Remark);

            string str_Sql = "INSERT INTO " + invoice_Item_Table.TableName;
            str_Sql += " ( ";
            str_Sql += invoice_Item_Table.Product_ID + ", ";
            str_Sql += invoice_Item_Table.Brand + ", ";
            str_Sql += invoice_Item_Table.Product_Descruibtions + ", ";
            str_Sql += invoice_Item_Table.Model + ", ";
            str_Sql += invoice_Item_Table.Unit + ", ";
            str_Sql += invoice_Item_Table.Qty + ", ";
            str_Sql += invoice_Item_Table.Unit_Price + ", ";
            str_Sql += invoice_Item_Table.Net_Price + ", ";
            str_Sql += invoice_Item_Table.Invoice_ID + ", ";
            str_Sql += invoice_Item_Table.Item_Remark + ", ";
            str_Sql += invoice_Item_Table.Total + " ";
            str_Sql += ") ";
            str_Sql += "VALUES (";
            str_Sql += "'" + inInvoice_Item.Product_ID + "', ";
            str_Sql += "'" + brand + "', ";
            str_Sql += "'" + description + "', ";
            str_Sql += "'" + inInvoice_Item.Model + "', ";
            str_Sql += "'" + inInvoice_Item.Unit + "', ";
            str_Sql += "'" + inInvoice_Item.Qty + "', ";
            str_Sql += "'" + inInvoice_Item.Unit_Price + "', ";
            str_Sql += "'" + inInvoice_Item.Net_Price + "', ";
            str_Sql += "'" + inInvoice_Item.Invoice_ID + "', ";
            str_Sql += "'" + remark + "', ";
            str_Sql += "'" + inInvoice_Item.Total + "' ";
            str_Sql += ") ";
            ExecuteDB(str_Sql);
        }

        private void Update_Invoice(Models.Invoice inInvoice)
        {
            Controllers.DataMapping.Invoice_Table clinic_Table = new DataMapping.Invoice_Table();
            Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
            string remark = specialCharter.validString(inInvoice.Remark);
            string clinicName = specialCharter.validString(inInvoice.Clinic_Name);
            string clinicAddress = specialCharter.validString(inInvoice.Address);
            string contactPerson = specialCharter.validString(inInvoice.ContactPerson);
            string ship_clinicName = specialCharter.validString(inInvoice.SHIPTO_Clinic_Name);
            string ship_clinicAddress = specialCharter.validString(inInvoice.SHIPTO_Address);
            string ship_contactPerson = specialCharter.validString(inInvoice.SHIPTO_ContactPerson);
            string invoice_Date = specialCharter.SQL_DateTime(inInvoice.Invoice_Date);
            string due_Date = specialCharter.SQL_DateTime(inInvoice.Due_Date);

            string str_Sql = "UPDATE " + clinic_Table.TableName;
            str_Sql += " SET ";
            str_Sql += clinic_Table.Invoice_Date + " = '" + invoice_Date + "', ";
            str_Sql += clinic_Table.Remark + " = '" + remark + "', ";
            str_Sql += clinic_Table.Sales_Person + " = '" + inInvoice.Sales_Person + "', ";
            str_Sql += clinic_Table.Clinic_Name + " = '" + clinicName + "', ";
            str_Sql += clinic_Table.Clinic_ID + " = '" + inInvoice.Clinic_ID + "', ";
            str_Sql += clinic_Table.Address + " = '" + clinicAddress + "', ";
            str_Sql += clinic_Table.ContactPerson + " = '" + contactPerson + "', ";
            str_Sql += clinic_Table.Telephone + " = '" + inInvoice.Telephone + "', ";
            str_Sql += clinic_Table.SHIPTO_Clinic_Name + " = '" + ship_clinicName + "', ";
            str_Sql += clinic_Table.SHIPTO_Address + " = '" + ship_clinicAddress + "', ";
            str_Sql += clinic_Table.SHIPTO_ContactPerson + " = '" + ship_contactPerson + "', ";
            str_Sql += clinic_Table.SHIPTO_Telephone + " = '" + inInvoice.SHIPTO_Telephone + "', ";
            str_Sql += clinic_Table.Payment_Terms + " = '" + inInvoice.Payment_Terms + "', ";
            str_Sql += clinic_Table.District + " = '" + inInvoice.District + "', ";
            str_Sql += clinic_Table.CustomerReferenceCode + " = '" + inInvoice.CustomerReferenceCode + "', ";
            str_Sql += clinic_Table.Discount + " = '" + inInvoice.Discount + "', ";
            str_Sql += clinic_Table.Sub_Total + " = '" + inInvoice.Sub_Total + "', ";
            str_Sql += clinic_Table.Total + " = '" + inInvoice.Total + "', ";
            str_Sql += clinic_Table.Due_Date + " = '" + due_Date + "', ";
            str_Sql += clinic_Table.INV_Status + " = '" + inInvoice.INV_Status + "', ";
            str_Sql += clinic_Table.Delivery + " = '" + inInvoice.Delivery + "' ";
            str_Sql += "WHERE (" + clinic_Table.Invoice_ID + " = '" + inInvoice.Invoice_ID + "')";

            ExecuteDB(str_Sql);
        }

        private void Update_Invoice_items(Models.Invoice_Item inInvoice_Item)
        {
            Controllers.DataMapping.Invoice_Item_Table invoice_Item_Table = new DataMapping.Invoice_Item_Table();
            Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
            string brand = specialCharter.validString(inInvoice_Item.Brand);
            string description = specialCharter.validString(inInvoice_Item.Product_Descruibtions);
            string remark = specialCharter.validString(inInvoice_Item.Item_Remark);

            if (string.IsNullOrEmpty(inInvoice_Item.Item_Remark.Trim()))
            {
                description += " (" + inInvoice_Item.Item_Remark + ")";
            }

            string str_Sql = "UPDATE " + invoice_Item_Table.TableName;
            str_Sql += " SET ";
            str_Sql += invoice_Item_Table.Product_ID + " = '" + inInvoice_Item.Product_ID + "', ";
            str_Sql += invoice_Item_Table.Brand + " = '" + brand + "', ";
            str_Sql += invoice_Item_Table.Product_Descruibtions + " = '" + description + "', ";
            str_Sql += invoice_Item_Table.Model + " = '" + inInvoice_Item.Model + "', ";
            str_Sql += invoice_Item_Table.Unit + " = '" + inInvoice_Item.Unit + "', ";
            str_Sql += invoice_Item_Table.Qty + " = '" + inInvoice_Item.Qty + "', ";
            str_Sql += invoice_Item_Table.Unit_Price + " = '" + inInvoice_Item.Unit_Price + "', ";
            str_Sql += invoice_Item_Table.Net_Price + " = '" + inInvoice_Item.Net_Price + "', ";
            str_Sql += invoice_Item_Table.Invoice_ID + " = '" + inInvoice_Item.Invoice_ID + "', ";
            str_Sql += invoice_Item_Table.Item_Remark + " = '" + inInvoice_Item.Item_Remark + "', ";
            str_Sql += invoice_Item_Table.Total + " = '" + inInvoice_Item.Total + "' ";
            str_Sql += "WHERE (" + invoice_Item_Table.Item_ID + " = '" + inInvoice_Item.Item_ID + "')";

            ExecuteDB(str_Sql);
        }

        private void delete_Invoice_items(string inInvoiceID)
        {
            Controllers.DataMapping.Invoice_Item_Table invoice_Item_Table = new DataMapping.Invoice_Item_Table();

            string str_Sql = "DELETE FROM " + invoice_Item_Table.TableName;
            str_Sql += " WHERE (" + invoice_Item_Table.Invoice_ID + " = '" + inInvoiceID + "')";

            ExecuteDB(str_Sql);
        }

        /*
        private void AddItem(Models.Invoice inInvoice)
        {
            Add_Invoice(inInvoice);

            foreach (Models.Invoice_Item tempItem in inInvoice.ItemList)
            {
                Add_Invoice_Item(tempItem);
            }
        }

        private void UpdateItem(Models.Invoice inInvoice)
        {
            Update_Invoice(inInvoice);
            delete_Invoice_items(inInvoice.Invoice_ID);
            foreach (Models.Invoice_Item tempItem in inInvoice.ItemList)
            {
                Add_Invoice_Item(tempItem);
            }
        }
        */

        private void Save_Invoice_N_Items(Models.Invoice inInvoice)
        {
            Add_Invoice_Procedure(inInvoice);
            foreach (Models.Invoice_Item tempItem in inInvoice.ItemList)
            {
                Add_Invoice_Item(tempItem);
            }
        }

        public string Get_Max_InvoiceNo()
        {
            //database = new DB.DBUtils();
            //sqlseverConnection = new SqlConnection();
            //sqlseverConnection = database.GetDBConnection();

            Controllers.DataMapping.Invoice_Table clinic_Table = new DataMapping.Invoice_Table();
            string strResult = "";
            string str_Sql = "SELECT MAX(" + clinic_Table.Invoice_ID + ") AS MAX_INV FROM " + clinic_Table.TableName;

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

        public bool invoiceExit(string in_Invoice_ID)
        {
            Controllers.DataMapping.Invoice_Table clinic_Table = new DataMapping.Invoice_Table();
            bool result = false;
            string str_Sql = "SELECT " + clinic_Table.Invoice_ID + " FROM " + clinic_Table.TableName;
            str_Sql += " WHERE " + clinic_Table.Invoice_ID + "=  '" + in_Invoice_ID + "'";

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

        public Models.Invoice GetInvoice(string inInvoiceID)
        {
            Models.Invoice tempInvoice = new Models.Invoice();
            tempInvoice = get_Invoice(inInvoiceID);
            tempInvoice.ItemList = getInvoiceItems(inInvoiceID);
            return tempInvoice;
        }

        private string theDateFormat(string inDate)
        {
            string tempDate = "";
            if (!string.IsNullOrWhiteSpace(inDate))
            {
                tempDate = converToDate(inDate);
            }
            else
            {
                tempDate = "";
            }

            return tempDate;
        }

        private string converToDate(string inString)
        {
            string result = "";
            try
            {
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                string dateString = inString;
                string format = "dd-mm-yyyy";
                DateTime dateResult = DateTime.ParseExact(dateString, format, provider);
                System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
                result = dateResult.ToString("dd-MMM-yyyy", en);
                string tempYEAR = dateResult.Year.ToString();
                return result;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            return result;
        }


        private Models.Invoice get_Invoice(string inInvoiceID)
        {
            Controllers.DataMapping.Invoice_Table clinic_Table = new DataMapping.Invoice_Table();
            Models.Invoice temp_Invoice = new Models.Invoice();

            string str_Sql = "SELECT ";
            str_Sql += clinic_Table.TableName+"."+ clinic_Table.Clinic_ID + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.Clinic_Name + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.Address + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.ContactPerson + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.Telephone + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.SHIPTO_Clinic_Name + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.SHIPTO_Address + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.SHIPTO_ContactPerson + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.SHIPTO_Telephone + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.Delivery + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.District + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.Payment_Terms + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.Invoice_ID + ", ";
            str_Sql += "CONVERT(varchar," + clinic_Table.TableName + "." + clinic_Table.Invoice_Date + ", 105) AS " + clinic_Table.Invoice_Date+ ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.Sales_Person + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.CustomerReferenceCode + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.Discount + ", ";
            str_Sql += "CONVERT(varchar," + clinic_Table.TableName + "." + clinic_Table.Due_Date + ", 105) AS " + clinic_Table.Due_Date + " , ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.INV_Status + ", ";
            str_Sql += clinic_Table.TableName + "." + clinic_Table.Remark + " ";
            str_Sql += " FROM " + clinic_Table.TableName;
            str_Sql += " WHERE " + clinic_Table.Invoice_ID + "=  '" + inInvoiceID + "'";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    if (theData.HasRows)
                    {
                        while (theData.Read())
                        {
                            temp_Invoice = new Models.Invoice
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
                                Payment_Terms = theData[clinic_Table.Payment_Terms].ToString(),
                                Invoice_ID = theData[clinic_Table.Invoice_ID].ToString(),
                                Invoice_Date = theDateFormat(theData[clinic_Table.Invoice_Date].ToString()),
                                Sales_Person = theData[clinic_Table.Sales_Person].ToString(),
                                CustomerReferenceCode = theData[clinic_Table.CustomerReferenceCode].ToString(),
                                Discount = theData[clinic_Table.Discount].ToString(),
                                Due_Date = theDateFormat(theData[clinic_Table.Due_Date].ToString()),
                                INV_Status = theData[clinic_Table.INV_Status].ToString(),
                                Remark = theData[clinic_Table.Remark].ToString()
                            };
                        }
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
            return temp_Invoice;
        }

        private List<Models.Invoice_Item> getInvoiceItems(string inInvoiceID)
        {
            Controllers.DataMapping.Invoice_Item_Table clinic_Item_Table = new DataMapping.Invoice_Item_Table();
            List<Models.Invoice_Item> temp_InvoiceList = new List<Models.Invoice_Item>();

            string str_Sql = "SELECT * FROM " + clinic_Item_Table.TableName;
            str_Sql += " WHERE " + clinic_Item_Table.Invoice_ID + "=  '" + inInvoiceID + "'";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    if (theData.HasRows)
                    {
                        while (theData.Read())
                        {
                            temp_InvoiceList.Add(new Models.Invoice_Item
                            {
                                Product_ID = theData[clinic_Item_Table.Product_ID].ToString(),
                                Brand = theData[clinic_Item_Table.Brand].ToString(),
                                Model = theData[clinic_Item_Table.Model].ToString(),
                                Product_Descruibtions = theData[clinic_Item_Table.Product_Descruibtions].ToString(),
                                Unit = theData[clinic_Item_Table.Unit].ToString(),
                                Unit_Price = theData[clinic_Item_Table.Unit_Price].ToString(),
                                Item_ID = theData[clinic_Item_Table.Item_ID].ToString(),
                                Invoice_ID = theData[clinic_Item_Table.Invoice_ID].ToString(),
                                Item_Remark = theData[clinic_Item_Table.Item_Remark].ToString(),
                                Net_Price = theData[clinic_Item_Table.Net_Price].ToString(),
                                Qty = theData[clinic_Item_Table.Qty].ToString(),
                                Total = theData[clinic_Item_Table.Total].ToString()
                            });
                        }
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
            return temp_InvoiceList;
        }

        public Models.Invoice get_the_Invoice(string inInvoiceID)
        {
            Controllers.DataMapping.Invoice_Table clinic_Table = new DataMapping.Invoice_Table();
            Models.Invoice temp_Invoice = new Models.Invoice();

            string str_Sql = "SELECT * FROM " + clinic_Table.TableName;
            str_Sql += " WHERE " + clinic_Table.Invoice_ID + "=  '" + inInvoiceID + "'";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    if (theData.HasRows)
                    {
                        while (theData.Read())
                        {
                            temp_Invoice = new Models.Invoice
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
                                Payment_Terms = theData[clinic_Table.Payment_Terms].ToString(),
                                Invoice_ID = theData[clinic_Table.Invoice_ID].ToString(),
                                Invoice_Date = theDateFormat(theData[clinic_Table.Invoice_Date].ToString()),
                                Sales_Person = theData[clinic_Table.Sales_Person].ToString(),
                                CustomerReferenceCode = theData[clinic_Table.CustomerReferenceCode].ToString(),
                                Discount = theData[clinic_Table.Discount].ToString(),
                                Due_Date = theDateFormat(theData[clinic_Table.Due_Date].ToString()),
                                INV_Status = theData[clinic_Table.INV_Status].ToString(),
                                Total = theData[clinic_Table.Total].ToString(),
                                Remark = theData[clinic_Table.Remark].ToString()
                            };
                        }
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
            return temp_Invoice;
        }

        public List<Models.Invoice> get_opened_InvoiceItems(string inCustomerID)
        {
            Controllers.DataMapping.Invoice_Table invoice_Table = new DataMapping.Invoice_Table();
            List<Models.Invoice> temp_InvoiceList = new List<Models.Invoice>();

            string str_Sql = "SELECT * FROM " + invoice_Table.TableName;
            str_Sql += " WHERE " + invoice_Table.Clinic_ID + "=  '" + inCustomerID + "'";
            str_Sql += " AND " + invoice_Table.INV_Status + "<>  'BALANCED'";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    if (theData.HasRows)
                    {
                        while (theData.Read())
                        {
                            temp_InvoiceList.Add(new Models.Invoice
                            {
                                Clinic_ID = theData[invoice_Table.Clinic_ID].ToString(),
                                Clinic_Name = theData[invoice_Table.Clinic_Name].ToString(),
                                Address = theData[invoice_Table.Address].ToString(),
                                ContactPerson = theData[invoice_Table.ContactPerson].ToString(),
                                Telephone = theData[invoice_Table.Telephone].ToString(),
                                SHIPTO_Clinic_Name = theData[invoice_Table.SHIPTO_Clinic_Name].ToString(),
                                SHIPTO_Address = theData[invoice_Table.SHIPTO_Address].ToString(),
                                SHIPTO_ContactPerson = theData[invoice_Table.SHIPTO_ContactPerson].ToString(),
                                SHIPTO_Telephone = theData[invoice_Table.SHIPTO_Telephone].ToString(),
                                Delivery = theData[invoice_Table.Delivery].ToString(),
                                District = theData[invoice_Table.District].ToString(),
                                Payment_Terms = theData[invoice_Table.Payment_Terms].ToString(),
                                Invoice_ID = theData[invoice_Table.Invoice_ID].ToString(),
                                Invoice_Date = theDateFormat(theData[invoice_Table.Invoice_Date].ToString()),
                                Sales_Person = theData[invoice_Table.Sales_Person].ToString(),
                                CustomerReferenceCode = theData[invoice_Table.CustomerReferenceCode].ToString(),
                                Discount = theData[invoice_Table.Discount].ToString(),
                                Due_Date = theDateFormat(theData[invoice_Table.Due_Date].ToString()),
                                INV_Status = theData[invoice_Table.INV_Status].ToString(),
                                Total = theData[invoice_Table.Total].ToString(),
                                Remark = theData[invoice_Table.Remark].ToString()
                            });
                        }
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
            return temp_InvoiceList;
        }

        public void SaveInvoice(Models.Invoice inInvoice)
        {
            //Controllers.Excel.InvoiceController excel_invoiceController = new Excel.InvoiceController();
            //excel_invoiceController.SaveInvoice(inInvoice);
            //excel_invoiceController.updateFile();

            int numOfRows = inInvoice.ItemList.Count();

            if (numOfRows <= 5)
            {
                Controllers.Excel.InvoiceMaker.InvoiceMaker05 invoiceMaker = new Controllers.Excel.InvoiceMaker.InvoiceMaker05(inInvoice);
                invoiceMaker.MakeNewInvoice();
            }
            else if ((numOfRows > 5) && (numOfRows <= 14))
            {
                Controllers.Excel.InvoiceMaker.InvoiceMaker10 invoiceMaker = new Controllers.Excel.InvoiceMaker.InvoiceMaker10(inInvoice);
                invoiceMaker.MakeNewInvoice();
            }
            else if ((numOfRows > 14) && (numOfRows <= 25))
            {
                Controllers.Excel.InvoiceMaker.InvoiceMaker20 invoiceMaker = new Controllers.Excel.InvoiceMaker.InvoiceMaker20(inInvoice);
                invoiceMaker.MakeNewInvoice();
            }
            else if ((numOfRows > 25) && (numOfRows <= 35))
            {
                Controllers.Excel.InvoiceMaker.InvoiceMaker30 invoiceMaker = new Controllers.Excel.InvoiceMaker.InvoiceMaker30(inInvoice);
                invoiceMaker.MakeNewInvoice();
            }
            else if ((numOfRows > 35) && (numOfRows <= 47))
            {
                Controllers.Excel.InvoiceMaker.InvoiceMaker40 invoiceMaker = new Controllers.Excel.InvoiceMaker.InvoiceMaker40(inInvoice);
                invoiceMaker.MakeNewInvoice();
            }
            else if ((numOfRows > 47) && (numOfRows <= 57))
            {
                Controllers.Excel.InvoiceMaker.InvoiceMaker50 invoiceMaker = new Controllers.Excel.InvoiceMaker.InvoiceMaker50(inInvoice);
                invoiceMaker.MakeNewInvoice();
            }


            Save_Invoice_N_Items(inInvoice);

            /*
            if (invoiceExit(inInvoice.Invoice_ID))
            {
                UpdateItem(inInvoice);
            }
            else
            {
                AddItem(inInvoice);

            }*/
        }

    }
}
