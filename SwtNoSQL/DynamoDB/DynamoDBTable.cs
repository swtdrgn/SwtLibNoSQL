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
            var client = _database.Client;

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
            var client = _database.Client;
            var request = new DeleteTableRequest { TableName = Name };
            client.DeleteTable(request);
            WaitUntilTableIsActive();
        }

        public void Insert(NoSQLTableEntity entity)
        {
            Dictionary<string, AttributeValue> columns = new Dictionary<string, AttributeValue>();
            foreach (PropertyInfo property in entity.GetType().GetProperties())
            {
                if (property.GetGetMethod(false) == null || property.GetSetMethod(false) == null) { continue; }
                try
                {
                    dynamic value = property.GetValue(entity, null);
                    columns[property.Name] = ToAttributeValue(value);
                }
                catch { }
            }
            _database.Client.PutItem(new PutItemRequest() { TableName = _name, Item = columns });
        }

        private AttributeValue ToAttributeValue(int value) { return new AttributeValue() { N = value.ToString() }; }
        private AttributeValue ToAttributeValue(long value) { return new AttributeValue() { N = value.ToString() }; }
        private AttributeValue ToAttributeValue(double value) { return new AttributeValue() { N = value.ToString() }; }
        private AttributeValue ToAttributeValue(string value) { return new AttributeValue() { S = value }; }
        private AttributeValue ToAttributeValue(object value) { throw new NotImplementedException(); }

        private AttributeValue ToAttributeValue(List<int> value) { throw new NotImplementedException(); }
        private AttributeValue ToAttributeValue(List<long> value) { throw new NotImplementedException(); }
        private AttributeValue ToAttributeValue(List<float> value) { throw new NotImplementedException(); }
        private AttributeValue ToAttributeValue(List<double> value) { throw new NotImplementedException(); }
        private AttributeValue ToAttributeValue(List<string> value) { throw new NotImplementedException(); }

        public void Insert<T>(NoSQLTableEntity entity) where T : NoSQLTableEntity, new()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string partitionKey, string rowKey) where T : NoSQLTableEntity, new()
        {
            var client = _database.Client;
            var request = new GetItemRequest
            {
                TableName = Name,
                Key = new Key
                {
                    HashKeyElement = new AttributeValue { S = partitionKey },
                    RangeKeyElement = new AttributeValue { S = rowKey }
                }
            };
            var row = client.GetItem(request).GetItemResult.Item;

            if (row == null) { return null; }
            T returnEntity = new T();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                if (property.GetGetMethod(false) == null || property.GetSetMethod(false) == null) { continue; }
                AttributeValue value;
                row.TryGetValue(property.Name, out value);

                dynamic getValue = property.GetValue(returnEntity, null);
                
                //property.SetValue(returnEntity, Convert.ChangeType(value.N, property.GetType()), null);

                switch (Type.GetTypeCode(property.PropertyType))
                {
                    case TypeCode.Int16:
                        property.SetValue(returnEntity, ReadNumber<Int16>(value), null);
                        break;
                    case TypeCode.Int32:
                        property.SetValue(returnEntity, ReadNumber<Int32>(value), null);
                        break;
                    case TypeCode.Int64:
                        property.SetValue(returnEntity, ReadNumber<Int64>(value), null);
                        break;
                    case TypeCode.Double:
                        property.SetValue(returnEntity, ReadNumber<Double>(value), null);
                        break;
                    case TypeCode.String:
                        property.SetValue(returnEntity, value.S, null);
                        break;
                    default:
                        break;
                }
            }
            return returnEntity;
        }

        private static readonly MethodInfo ReadNumberReflectionMethod = typeof(DynamoDBTable).GetMethod("ReadNumber");
        private static T ReadNumber<T>(AttributeValue value)
        {
            return (T)Convert.ChangeType(value.N, typeof(T));
        }

        public void Delete(NoSQLTableEntity entity)
        {
            var request = new DeleteItemRequest()
            {
                TableName = _name,
                Key = new Key
                {
                    HashKeyElement = new AttributeValue { S = entity.PartitionKey },
                    RangeKeyElement = new AttributeValue { S = entity.RowKey }
                }
            };

            _database.Client.DeleteItem(request);
        }

        private void WaitUntilTableIsActive()
        {
            WaitUntilTableIsActive(_database.Client, Name);
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