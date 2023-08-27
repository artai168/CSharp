using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.Functional
{
    class Receivable_ID_Creator
    {
        private string strYear;
        private string strMonth;
        

        public Receivable_ID_Creator()
        {

        }

        public Receivable_ID_Creator(string inDateTime)
        {
            setDateTime(inDateTime);
        }

        private void setDateTime(string inDateTime)
        {
            string dateString = inDateTime;
            CultureInfo provider = CultureInfo.InvariantCulture;
            
            DateTime dateTime = DateTime.ParseExact(dateString, "dd-MMM-yyyy", provider);
            System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
            strYear = dateTime.ToString("yyyy",en);
            strMonth = dateTime.ToString("MM",en);
        }

        private string getLatestDBRecord()
        {
            string result = "";
            ReceivableController receivableController = new ReceivableController();
            result = receivableController.Get_Max_ReceivableNo(strYear, strMonth);
            return result;
        }

        private string newID(string inID)
        {
            int tempNum = 0;
            string result = "";
            if (string.IsNullOrWhiteSpace(inID))
            {
                tempNum = tempNum + 1;
                result = strYear + strMonth + string.Format("{0:0000}", tempNum);
            }
            else
            {
                tempNum = Convert.ToInt32(inID) + 1;
                result = tempNum.ToString();
            }
            return result;
        }

        private string getMaximuumReceivable_ID()
        {
            string strResult = newID(getLatestDBRecord());

            return strResult;
        }

        public string getNewID()
        {
            return getMaximuumReceivable_ID();
        }

    }
}
