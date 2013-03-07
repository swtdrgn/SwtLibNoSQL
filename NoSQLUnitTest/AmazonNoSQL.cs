using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Amazon.Runtime;

namespace NoSQL.UnitTest
{
    /// <summary>
    /// Summary description for AmazonNoSQL
    /// </summary>
    [TestClass]
    public class AmazonNoSQL
    {
        AWSCredentials _amazonAccount;

        public AmazonNoSQL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        /*public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            _amazonAccount = new BasicAWSCredentials(
            CreateTableForTesting();
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

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //
        }*/
    }
}
