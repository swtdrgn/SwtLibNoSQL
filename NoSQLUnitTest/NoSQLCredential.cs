using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.WindowsAzure.Storage;
using Amazon.Runtime;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoSQL.UnitTest
{
    public static class NoSQLCredential
    {
        static CloudStorageAccount _azureAccount;
        static AWSCredentials _awsAccount;

        static NoSQLCredential()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(@".\Credentials.xml");

            try
            {
                var attributes = xmldoc.GetElementsByTagName("Azure")[0].Attributes;
                _azureAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=" + attributes["AccountName"].Value + ";AccountKey=" + attributes["AccountKey"].Value);
            }
            catch { _azureAccount = null; }

            try
            {
                var element = xmldoc.GetElementsByTagName("AWS")[0];
                _awsAccount = new BasicAWSCredentials(element["AccessKey"].Value, element["SecretKey"].Value);
            }
            catch { _awsAccount = null; }
        }

        public static CloudStorageAccount Azure { get { return _azureAccount; } }
        public static AWSCredentials AWS { get { return _awsAccount; } }
    }
}
