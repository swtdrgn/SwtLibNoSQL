using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Amazon.Auth;

namespace SwtLib
{
    public class NoSQLAccount
    {
        CloudStorageAccount _azureAccount;

        public NoSQLAccount()
        {

        }

        public CloudStorageAccount AzureAccount
        {
            get { return _azureAccount; }
        }
    }
}
