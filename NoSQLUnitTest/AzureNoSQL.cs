using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using SwtLib;

namespace NoSQL.UnitTest
{
    /// <summary>
    /// Summary description for AzureNoSQL
    /// </summary>
    [TestClass]
    public class AzureNoSQL
    {
        CloudStorageAccount _azureAccount;
        NoSQLTable _table;

        public AzureNoSQL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [TestInitialize]
        public void Initialize()
        {
            _azureAccount = NoSQLCredential.Azure;
            CreateTableForTesting();
        }

        [TestCleanup]
        public void ReleaseResources()
        {
            ReleaseTableName();
        }

        public void CreateTableForTesting()
        {
            var database = NoSQLDatabase.Connect(_azureAccount);
            string tableBaseName = "TestNoSQLTable";
            string tableName = tableBaseName;

            var table = database.GetTable(tableName);
            int attempts = 0;

            // Find a table name that does not already exist in the database.
            while (table != null && attempts < 100)
            {
                tableName = tableBaseName + new Random().NextDouble();
                table = database.GetTable(tableName);
                attempts++;
            }

            if (attempts < 100)
            {
                database.CreateTable(tableName); // Create.
                _table = database.GetTable(tableName); // Get.
                Assert.AreNotEqual(null, _table); // Verify
            }
            else
            {
                // Too many attempts
                Assert.Fail("Cannot find a nonexistent table to create.");
            }
        }

        public void ReleaseTableName()
        {
            string tableName = _table.Name;
            var database = NoSQLDatabase.Connect(_azureAccount);

            _table.Drop(); // Delete.
            var verifyDeletedTable = database.GetTable(tableName); // Get.
            Assert.AreEqual(null, verifyDeletedTable); // Verify.
        }

        class TableRowTest : NoSQLTableEntity
        {
            public int Number { get; set; }
        }

        [TestMethod]
        public void TestRowOperations()
        {
            TableRowTest rowTest = new TableRowTest();
            rowTest.Number = new Random().Next(100);
            rowTest.PartitionKey = "PartitionKeyTest";
            rowTest.RowKey = "RowKeyTest";
            _table.Insert(rowTest);
            var getCreatedRow = _table.Get<TableRowTest>(rowTest.PartitionKey, rowTest.RowKey);
            Assert.AreEqual(rowTest.Number, getCreatedRow.Number);
            _table.Delete(rowTest);
            var getDeletedRow = _table.Get<TableRowTest>(rowTest.PartitionKey, rowTest.RowKey);
            Assert.AreEqual(null, getDeletedRow);
        }
    }
}
