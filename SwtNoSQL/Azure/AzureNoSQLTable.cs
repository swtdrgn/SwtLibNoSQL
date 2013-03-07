using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace SwtLib.Azure
{
    public class AzureNoSQLTable : NoSQLTable
    {
        AzureNoSQLDatabase _database;
        string _tableName;

        public AzureNoSQLTable(AzureNoSQLDatabase database, string tableName)
        {
            _database = database;
            _tableName = tableName;
        }

        public static bool Exist(AzureNoSQLDatabase database, string tableName) { return database.Connect().GetTableReference(tableName).Exists(); }
        private CloudTableClient TableClient { get { return _database.Connect(); } }
        public CloudTable Table { get { return TableClient.GetTableReference(_tableName); } }
        public void CreateTable() { Table.CreateIfNotExists(); }
        public void Drop() { Table.DeleteIfExists(); }
        public string Name { get { return _tableName; } }

        public void Insert(NoSQLTableEntity entity)
        {
            var insertOp = TableOperation.Insert(entity);
            Table.Execute(insertOp);
        }

        public void Insert<T>(NoSQLTableEntity entity) where T : NoSQLTableEntity
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string partitionKey, string rowKey) where T : NoSQLTableEntity
        {
            var getOp = TableOperation.Retrieve<T>(partitionKey,rowKey);
            var response = Table.Execute(getOp);
            if (response != null)
            {
                return response.Result as T;
            }
            else
            {
                return null;
            }
        }

        public void Delete(NoSQLTableEntity entity)
        {
            var deleteOp = TableOperation.Delete(entity);
            Table.Execute(deleteOp);
        }
    }
}
