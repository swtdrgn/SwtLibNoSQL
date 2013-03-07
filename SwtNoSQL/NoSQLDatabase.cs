using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwtLib
{
    public abstract class NoSQLDatabase
    {
        public static NoSQLDatabase Connect(Microsoft.WindowsAzure.Storage.CloudStorageAccount account) { return new Azure.AzureNoSQLDatabase(account); }
        public static NoSQLDatabase Connect(Amazon.Runtime.AWSCredentials account) { return new DynamoDB.DynamoDB(account); }
        public abstract NoSQLTable CreateTable(string tableName);
        public abstract NoSQLTable GetTable(string tableName);
        public abstract IEnumerable<NoSQLTable> ListTables();
    }
}
