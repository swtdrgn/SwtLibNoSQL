using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace SwtLib.Azure
{
    public class AzureTable : NoSQLTable
    {
        AzureNoSQLDatabase _database;
        string _tableName;

        public static bool Exist(AzureNoSQLDatabase database, string tableName) { return database.Connect().GetTableReference(tableName).Exists(); }
        private CloudTableClient TableClient { get { return _database.Connect(); } }
        public CloudTable GetTable() { return TableClient.GetTableReference(_tableName); }
        public void CreateTable() { GetTable().CreateIfNotExists(); }
        public void Drop() { GetTable().DeleteIfExists(); }
        public string Name { get { return _tableName; } }

        public AzureTable(AzureNoSQLDatabase database, string tableName)
        {
            _database = database;
            _tableName = tableName;
        }
    }
}
