using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.DB
{
    class DBController
    {
        public string DB_Server;
        public string DB_Name;
        public string DB_User_Name;
        public string DB_Password;
        public string DB_Connection_String;

        public DBController()
        {
            DB_Server = "192.168.64.234,1433";
            DB_Name = "TopStarInvoice";
            DB_User_Name = "TopStar";
            DB_Password = "THEPASSWORD";
            DB_Connection_String = @"Data Source=" + DB_Server + ";Initial Catalog="
                                    + DB_Name + ";Persist Security Info=True;User ID=" 
                                    + DB_User_Name + ";Password=" + DB_Password;
        }
    }
}
