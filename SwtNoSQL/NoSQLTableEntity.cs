using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;
using Amazon.DynamoDB.DataModel;
using System.Reflection;

using Microsoft.WindowsAzure.Storage;
using System.Diagnostics.CodeAnalysis;
using Microsoft.WindowsAzure.Storage.Table.Protocol;

namespace SwtLib
{
    /// <summary>
    /// Required inheritance to store class onto NoSQL database using this library.
    /// </summary>
    [DynamoDBTableAttribute("Default")]
    //public abstract class NoSQLTableEntity : ITableEntity
    public abstract class NoSQLTableEntity : TableEntity
    {
        [DynamoDBHashKey]
        public new string PartitionKey
        {
            get { return base.PartitionKey; }
            set { base.PartitionKey = value; }
        }

        [DynamoDBRangeKey]
        public new string RowKey
        {
            get { return base.RowKey; }
            set { base.RowKey = value; }
        }

        public string ETag
        {
            get { return "*"; }
        }

        //public DateTimeOffset Timestamp { get { return DateTimeOffset.UtcNow; } set { } }

        /*[DynamoDBProperty("Timestamp")]
        public new string Timestamp
        {
            get { return base.Timestamp.ToString(); }
            set { base.Timestamp = DateTimeOffset.Parse(value); }
        }*/
    }
}
