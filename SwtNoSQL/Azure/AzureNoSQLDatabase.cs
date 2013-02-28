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
        CloudStorageAccount _account;

        public AzureNoSQLDatabase(NoSQLAccount account) { _account = account.AzureAccount; }
        public AzureNoSQLDatabase(CloudStorageAccount account) { _account = account; }
        public CloudTableClient Connect() { return _account.CreateCloudTableClient(); }

        public override NoSQLTable GetTable(string tableName)
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

        public override NoSQLTable CreateTable(string tableName)
        {
            AzureTable table = new AzureTable(this, tableName);
            table.CreateTable();
            return table;
        }


        public override IEnumerable<NoSQLTable> ListTables()
        {
            throw new NotImplementedException();
        }
    }
}
