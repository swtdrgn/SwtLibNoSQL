using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwtLib.DynamoDB
{
    public class DynamoDBTable : NoSQLTable
    {
        string _name;

        public DynamoDBTable(string tableName)
        {
            _name = tableName;
        }

        public string Name
        {
            get { return _name; }
        }

        public void Drop()
        {
            throw new NotImplementedException();
        }

        public void Insert(NoSQLTableEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Insert<T>(NoSQLTableEntity entity) where T : NoSQLTableEntity
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string partitionKey, string rowKey) where T : NoSQLTableEntity
        {
            throw new NotImplementedException();
        }

        public void Delete(NoSQLTableEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
