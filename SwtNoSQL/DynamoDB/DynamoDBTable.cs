using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Amazon.DynamoDB;
using Amazon.DynamoDB.Model;

namespace SwtLib.DynamoDB
{
    public class DynamoDBTable : NoSQLTable
    {
        DynamoDB _database;
        string _name;

        public DynamoDBTable(DynamoDB database, string tableName)
        {
            _database = database;
            _name = tableName;
        }

        public string Name { get { return _name; } }

        public void CreateTable()
        {
            var client = _database.Connect();

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
            var client = _database.Connect();
            var request = new DeleteTableRequest { TableName = Name };
            client.DeleteTable(request);
            WaitUntilTableIsActive();
        }

        public void Insert(NoSQLTableEntity entity)
        {
            Dictionary<string, AttributeValue> columns = new Dictionary<string, AttributeValue>();
            throw new NotImplementedException();
        }

        public void Insert<T>(NoSQLTableEntity entity) where T : NoSQLTableEntity, new()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string partitionKey, string rowKey) where T : NoSQLTableEntity, new()
        {
            T returnEntity = null;

            var client = _database.Connect();
            var request = new GetItemRequest
            {
                TableName = Name,
                Key = new Key {
                    HashKeyElement = new AttributeValue { S = partitionKey },
                    RangeKeyElement = new AttributeValue { S = rowKey }
                }
            };
            var row = client.GetItem(request).GetItemResult.Item;

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                if (property.GetGetMethod(false) == null || property.GetSetMethod(false) == null) { continue; }
                AttributeValue value;
                row.TryGetValue(property.Name, out value);
            }
            return returnEntity;
        }

        public void Delete(NoSQLTableEntity entity)
        {
            throw new NotImplementedException();
        }

        private void WaitUntilTableIsActive()
        {
            WaitUntilTableIsActive(_database.Connect(), Name);
        }

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
