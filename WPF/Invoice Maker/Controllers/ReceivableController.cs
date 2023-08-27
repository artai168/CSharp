using System;
using System.Data;
using System.Data.SqlClient;

namespace TopStar_Invoice_Maker_SQLSever.Controllers
{
    class ReceivableController : DB.DBSettings
    {
        SqlConnection sqlseverConnection;

        public ReceivableController()
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

        public void SaveItem(Models.Receivable inReceivable)
        {
            Add_Receivable_Procedure(inReceivable);
        }

        private void Add_Receivable_Procedure(Models.Receivable inReceivable)
        {
            string strSql = "dbo.usp_Receivable_Add";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                // Create a Command object to call procedure : usp_Invoice_Add
                SqlCommand cmd = new SqlCommand(strSql, sqlseverConnection);

                Controllers.Functional.SpecialCharter specialCharter = new Functional.SpecialCharter();
                string strRemark = specialCharter.validString(inReceivable.Receivable_Remark);


                // Command Type is StoredProcedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@In_Receivable_ID", SqlDbType.NVarChar).Value = inReceivable.Receivable_Record_ID;
                cmd.Parameters.Add("@In_Received_Date", SqlDbType.DateTime2).Value = inReceivable.Receivable_Date;
                cmd.Parameters.Add("@In_Amount", SqlDbType.Money).Value = inReceivable.Receivable_Amount;
                cmd.Parameters.Add("@In_Charges", SqlDbType.Money).Value = inReceivable.Receivable_Charges;
                cmd.Parameters.Add("@In_CustomerID", SqlDbType.NVarChar).Value = inReceivable.Receivable_Customer_ID;
                cmd.Parameters.Add("@In_Received_Method", SqlDbType.NVarChar).Value = inReceivable.Receivable_Method;
                cmd.Parameters.Add("@In_BankIn_Date", SqlDbType.DateTime2).Value = inReceivable.Receivable_Bankin_Date;
                cmd.Parameters.Add("@In_Remark", SqlDbType.NVarChar).Value = strRemark;
                
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

        public string Get_Max_ReceivableNo(string inYear, string inMonth)
        {
            /*
             SELECT MAX(INVOICE_NO) AS INVOICE_NO
                FROM INV_DATA 
                WHERE (((Month([INVOICE_DATE]))= 4) AND ((Year([INVOICE_DATE]))= 2018));
             */

            Controllers.DataMapping.Receivable_Table receivable_Table = new DataMapping.Receivable_Table();
            string strResult = "";
            string str_Sql = "SELECT MAX(" + receivable_Table.Receivable_Record_ID + ") AS " + receivable_Table.Receivable_Record_ID ;
            str_Sql += " FROM " + receivable_Table.TableName;
            str_Sql += " WHERE (((Month([" + receivable_Table.Receivable_Date + "])= " + inMonth + ") AND ((Year([" + receivable_Table.Receivable_Date + "])= " + inYear + ")))) ";

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    while (theData.Read())
                    {
                        strResult = theData[receivable_Table.Receivable_Record_ID].ToString();
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

        public Models.Receivable GetReceivableRecord(string inReceivableID)
        {
            Controllers.DataMapping.Receivable_Table receivable_Table = new DataMapping.Receivable_Table();

            string str_Sql = "SELECT ";
            str_Sql += receivable_Table.TableName + "." + receivable_Table.Receivable_Record_ID + ", ";
            str_Sql += receivable_Table.TableName + "." + receivable_Table.Receivable_Amount + ", ";
            str_Sql += "CONVERT(varchar," + receivable_Table.TableName + "." + receivable_Table.Receivable_Bankin_Date + ", 105) AS " + receivable_Table.Receivable_Bankin_Date + ", "; 
            str_Sql += receivable_Table.TableName + "." + receivable_Table.Receivable_Charges + ", ";
            str_Sql += receivable_Table.TableName + "." + receivable_Table.Receivable_Customer_ID + ", ";
            str_Sql += "CONVERT(varchar," + receivable_Table.TableName + "." + receivable_Table.Receivable_Date + ", 105) AS " + receivable_Table.Receivable_Date + ", ";
            str_Sql += receivable_Table.TableName + "." + receivable_Table.Receivable_Method + ", ";
            str_Sql += receivable_Table.TableName + "." + receivable_Table.Receivable_Remark + " ";
            str_Sql += "FROM " + receivable_Table.TableName;
            str_Sql += " WHERE " + receivable_Table.Receivable_Record_ID + " = '" + inReceivableID + "'";

            Models.Receivable theResult = new Models.Receivable();

            using (sqlseverConnection = new SqlConnection(DB_Connection_String))
            {
                SqlCommand cmd = new SqlCommand(str_Sql, sqlseverConnection);
                try
                {
                    sqlseverConnection.Open();
                    SqlDataReader theData = cmd.ExecuteReader();
                    while (theData.Read())
                    {
                        theResult = new Models.Receivable
                        {
                            Receivable_Record_ID = theData[receivable_Table.Receivable_Record_ID].ToString(),
                            Receivable_Amount = theData[receivable_Table.Receivable_Amount].ToString(),
                            Receivable_Bankin_Date = theDateFormat(theData[receivable_Table.Receivable_Bankin_Date].ToString()),
                            Receivable_Charges = theData[receivable_Table.Receivable_Charges].ToString(),
                            Receivable_Customer_ID = theData[receivable_Table.Receivable_Customer_ID].ToString(),
                            Receivable_Date = theDateFormat(theData[receivable_Table.Receivable_Date].ToString()),
                            Receivable_Method = theData[receivable_Table.Receivable_Method].ToString(),
                            Receivable_Remark = theData[receivable_Table.Receivable_Remark].ToString()
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
                return theResult;
            }
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
                string format = "d-M-yyyy";
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

    }
}
