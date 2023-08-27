using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.Functional
{
    class Invoice_No_Creator
    {
        private string old_Ref;

        public Invoice_No_Creator(string inString)
        {
            old_Ref = inString;
        }

        private string theYear()
        {
            System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
            var result = DateTime.Now.ToString("yyyy", en);
            return result.ToString();
        }

        private string exit_ref()
        {
            return old_Ref.Replace("TS-S" + theYear(), "");
        }

        private string addOneNum(string inString)
        {
            int tempNum = 0;
            if (!string.IsNullOrWhiteSpace(inString))
            {
                tempNum = Convert.ToInt16(inString) + 1;
            }
            else
            {
                tempNum = 1;
            }

            return string.Format("{0:0000}", tempNum);
        }

        public string NewRef()
        {
            if (theSameYear())
            {
                return "TS-S" + theYear() + addOneNum(exit_ref());
            }
            else
            {
                return $"TS-S{theYear()}0001";
            }
            
        }

        private bool theSameYear()
        {
            bool result = true;
            string exitingYear = old_Ref.Substring(4, 4);
            string thisYear = theYear();
            if (exitingYear != thisYear)
            {
                result = false;
            }
            return result;
        }
    }
}
