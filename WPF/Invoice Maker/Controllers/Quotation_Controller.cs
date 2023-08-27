using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace TopStar_Invoice_Maker_SQLSever.Controllers
{
    class Quotation_Controller : DB_Controller
    {
        SqlConnection sqlseverConnection;
        public List<Models.Quotation> quotation_List;

        public Quotation_Controller()
        {
            quotation_List = new List<Models.Quotation>();
        }


    }
}
