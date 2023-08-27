using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.Functional
{
    class SpecialCharter
    {
        private string myString;

        public SpecialCharter(string inString)
        {
            myString = inString;
        }

        public SpecialCharter()
        {

        }

        public string validString(string inString)
        {
            myString = inString;
            checkString();
            return myString;
        }

        public string validString()
        {
            checkString();
            return myString;
        }

        private void checkString()
        {
            if (myString != null)
            {
                //myString = myString.Replace("'", "''");
                myString = myString.Replace("\"", "\"\"");
                myString = myString.Replace("\\", "\\\\");
                //myString = myString.Replace("%", "\\%");
                myString = myString.Replace("&amp;", "&");
            }
        }

        public bool IsNumber(string text)
        {
            if (text == null)
            {
                return false;
            }
            else
            {
                Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
                return regex.IsMatch(text);
            }
        }

        public string SQL_DateTime(string inString)
        {
            if (!string.IsNullOrEmpty(inString))
            {
                string format = "dd-MMM-yyyy";
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                DateTime dateResult = DateTime.ParseExact(inString, format, provider);
                System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
                return dateResult.ToString("yyyy-MM-dd", en);
            }
            else
            {
                return null;
            }
            
            /*
            var r = new Regex(@"^[0-9]{2}\d{6,7}$");
            var r2 = new Regex(@"^[0-9]{2}\d{6,7}$");
            if (!r.IsMatch(inString))
            {
                return dateTimeFormat1(inString);
            }
            if (!r2.IsMatch(inString))
            {
                return dateTimeFormat2(inString);
            }*/
        }


        private string dateTimeFormat1(string inString)
        {
            string format = "dd-MMM-yyyy";
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            DateTime dateResult = DateTime.ParseExact(inString, format, provider);
            System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
            return dateResult.ToString("yyyy-MM-dd", en);
        }

        private string dateTimeFormat2(string inString)
        {
            string format = "dd-mm-yyyy";
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            DateTime dateResult = DateTime.ParseExact(inString, format, provider);
            System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
            return dateResult.ToString("yyyy-MM-dd", en);
        }

        public string theDateFormat(string inDate)
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