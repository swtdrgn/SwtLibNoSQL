using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Amazon.Runtime;

using SwtLib;

namespace NoSQL.UnitTest
{
    /// <summary>
    /// Summary description for AmazonNoSQL
    /// </summary>
    [TestClass]
    public class AWSNoSQL
    {
        AWSCredentials _amazonAccount;
        NoSQLTable _table;

        public AWSNoSQL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [TestInitialize]
        public void Initialize()
        {
            _amazonAccount = NoSQLCredential.AWS;
            CreateTableForTesting();
        }

        [TestCleanup]
        public void ReleaseResources()
        {
            ReleaseTableName();
        }

        public void CreateTableForTesting()
        {
            var database = NoSQLDatabase.Connect(_amazonAccount);
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
            var database = NoSQLDatabase.Connect(_amazonAccount);

            _table.Drop(); // Delete.
            var verifyDeletedTable = database.GetTable(tableName); // Get.
            Assert.AreEqual(null, verifyDeletedTable); // Verify.
        }

        [TestMethod]
        public void TestRowOperations()
        {
        }
    }
}
