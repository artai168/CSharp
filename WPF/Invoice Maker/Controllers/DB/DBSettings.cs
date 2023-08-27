using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.DB
{
    public class DBSettings
    {
        private string datasource, database, username, password;
        public string DB_Connection_String;

        public DBSettings()
        {
            datasource = get_DB_IPAddress();
            database = "TopStarInvoice";
            username = "TopStar";
            password = "modern002HK";
            DB_Connection_String = setConnectionString();
        }

        private string setConnectionString()
        {
            string theString = @"Data Source=" + datasource
                        + ";Initial Catalog=" + database +
                        ";Persist Security Info=True;User ID=" + username
                        + ";Password=" + password;
            return theString;
        }

        private string get_DB_IPAddress()
        {
            string theIPAddress = "127.0.0.1,1433";
            string exitingIP = GetIPAddress();
            string[] subnet = exitingIP.Split('.');
            if (subnet[2] == "64")
            {
                theIPAddress = "192.168.64.234,1433";
            }
            else if (subnet[2] == "8")
            {
                theIPAddress = "192.168.8.30,1433";
            }
            return theIPAddress;
        }

        private string GetIPAddress()
        {
            string ipAddress = "";
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            ipAddress = Convert.ToString(ipHostInfo.AddressList.FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork));
            return ipAddress;
        }


        /*
        public SqlConnection GetDBConnection()
        {
            return DBSQLServerUtils.GetDBConnection(datasource, database, username, password);
        }*/
    }
}
