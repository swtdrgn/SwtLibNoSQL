using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Amazon.DynamoDB;
using Amazon.DynamoDB.Model;
using Amazon.DynamoDB.DataModel;

namespace SwtLib.DynamoDB
{
    public class DynamoDBTable : NoSQLTable
    {
        DynamoDB _database;
        string _name;

        DynamoDBContext _context;

        public DynamoDBTable(DynamoDB database, string tableName)
        {
            _database = database;
            _name = tableName;
            _context = new DynamoDBContext(Database);
        }

        public string Name { get { return _name; } }
        private AmazonDynamoDBClient Database { get { return _database.Client(); } }
        private DynamoDBContext Context { get { return _context; } }
        private void WaitUntilTableIsActive() { WaitUntilTableIsActive(Database, Name); }

        public void CreateTable()
        {
            var client = _database.Client();

            var request = new CreateTableRequest
            {
                KeySchema = new KeySchema
                {
                    HashKeyElement = new KeySchemaElement
                    {
                        AttributeName = "PartitionKey",
                        AttributeType = "S"
                    },
                    RangeKeyElement = new KeySchemaElement
                    {
                        AttributeName = "RowKey",
                        AttributeType = "S"
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 1,
                    WriteCapacityUnits = 1
                },
                TableName = Name
            };

            client.CreateTable(request);
            WaitUntilTableIsActive();
        }

        public void Drop()
        {
            var request = new DeleteTableRequest { TableName = Name };
            Database.DeleteTable(request);
            WaitUntilTableIsActive();
        }

        public void Insert(NoSQLTableEntity entity) { Context.Save(entity, new DynamoDBOperationConfig() { OverrideTableName = Name }); }
        public void Insert<T>(NoSQLTableEntity entity) where T : NoSQLTableEntity, new()
        {
            throw new NotImplementedException();
        }
        public T Get<T>(string partitionKey, string rowKey) where T : NoSQLTableEntity, new() { return Context.Load<T>(partitionKey, rowKey, new DynamoDBOperationConfig() { OverrideTableName = Name }); }
        public void Delete(NoSQLTableEntity entity) { Context.Delete(entity, new DynamoDBOperationConfig() { OverrideTableName = Name }); }

        private static void WaitUntilTableIsActive(AmazonDynamoDBClient client, string tableName)
        {
            string status = "";
            while (status != "ACTIVE")
            {
                System.Threading.Thread.Sleep(1000);
                try
                {
                    var res = client.DescribeTable(new DescribeTableRequest { TableName = tableName });
                    status = res.DescribeTableResult.Table.TableStatus;
                }
                catch { return; }
            }
        }

        public static bool Exist(AmazonDynamoDBClient database, string tableName)
        {
            try
            {
                database.DescribeTable(new DescribeTableRequest { TableName = tableName });
                return true;
            }
            catch { return false; }
        }
    }
}
