using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwtLib.Azure
{
    public class AzureTable : NoSQLTable
    {
        public static bool Exist(AzureNoSQLDatabase database, string tableName)
        {
            return true;
        }

        public AzureTable(AzureNoSQLDatabase database, string name)
        {
        }

        public void CreateTable()
        {

        }
    }
}
