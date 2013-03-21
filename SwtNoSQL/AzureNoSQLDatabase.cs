using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SwtLib.Azure;

namespace SwtLib
{
    public class AzureNoSQLDatabase : NoSQLDatabase
    {
        NoSQLAccount _account;

        public AzureNoSQLDatabase(NoSQLAccount account)
        {
            _account = account;
        }

        public CloudTableClient Connect()
        {
            return _account.AzureAccount.CreateCloudTableClient();
        }

        public NoSQLTable GetTable(string tableName)
        {
            if (AzureTable.Exist(this, tableName))
            {
                return new AzureTable(this, tableName);
            }
            else
            {
                return null;
            }

        }

        public NoSQLTable CreateTable(string tableName)
        {
            AzureTable table = new AzureTable(this, tableName);
            table.CreateTable();
            return table;
        }


        public ICollection<NoSQLTable> ListTables()
        {
            throw new NotImplementedException();
        }

        public bool DeleteTable(string tableName)
        {
            throw new NotImplementedException();
        }
    }
}
