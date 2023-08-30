using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Entities
{
    public class DB_Table : Interfaces.Data.ITable<String, DateTime>
    {
        public string TableName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class T_DB_Table : Interfaces.Data.ITable<String, String>
    {
        public string TableName { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }

        public T_DB_Table()
        {
            TableName = "";
            CreateDate = "Create_Date";
            UpdateDate = "Update_Date";
        }
    }
}
