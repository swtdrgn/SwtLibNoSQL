using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace SwtLib
{
    /// <summary>
    /// Required inheritance to store class onto NoSQL database using this library.
    /// </summary>
    public abstract class NoSQLTableEntity : TableEntity
    {
        public string ETag
        {
            get { return "*"; }
        }
    }
}
