using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwtLib
{
    public interface NoSQLTable
    {
        void Drop();


        void Insert(NoSQLTableEntity entity);
        void Insert<T>(NoSQLTableEntity entity) where T : NoSQLTableEntity;

        /// <summary>
        /// Fetches data from the NoSQL table.
        /// </summary>
        /// <typeparam name="T">Return type.</typeparam>
        /// <param name="partitionKey">Partition key.</param>
        /// <param name="rowKey">Row key.</param>
        /// <returns>Returns data as T if data is found. Otherwise, returns null.</returns>
        T Get<T>(string partitionKey, string rowKey) where T : NoSQLTableEntity;


        void Delete(NoSQLTableEntity entity);
    }
}
