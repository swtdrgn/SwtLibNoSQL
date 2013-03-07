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

        public AmazonDynamoDBClient Connect()
        {
            return new AmazonDynamoDBClient(_account);
        }

        public override NoSQLTable CreateTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public override NoSQLTable GetTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<NoSQLTable> ListTables()
        {
            throw new NotImplementedException();
        }
    }
}
