using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Amazon.DynamoDB;
using Amazon.DynamoDB.Model;
using Amazon.Runtime;

namespace SwtLib.DynamoDB
{
    public class DynamoDB : NoSQLDatabase
    {
        AWSCredentials _account;

        public DynamoDB(AWSCredentials account) { _account = account; }

        public AmazonDynamoDBClient Connect() { return new AmazonDynamoDBClient(_account); }

        public override NoSQLTable CreateTable(string tableName)
        {
            var table = new DynamoDBTable(this, tableName);
            table.CreateTable();
            return table;
        }

        public override NoSQLTable GetTable(string tableName)
        {
            if (DynamoDBTable.Exist(Connect(), tableName))
            {
                return new DynamoDBTable(this, tableName);
            }
            else
            {
                return null;
            }
        }

        public override IEnumerable<NoSQLTable> ListTables()
        {
            throw new NotImplementedException();
        }
    }
}
