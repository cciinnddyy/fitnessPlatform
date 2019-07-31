using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace tcplisten.Entities
{
    class pulseEntity : TableEntity, IAzureTable
    {
        public pulseEntity() { }


        public bool isDelete { get; set; }

        public string timezone { get; set; }

        public DateTime timestampForCompare { get; set; }

        public DateTime timestamp { get; set; }



        public string sourceNmae { get; set; }



        public string primaryKey { get; set; }

        public string gid { get; set; }



        public bool isModify { get; set; }

        public int pulse { get; set; }

        public bool isCommit { get; set; }
        public void setPartitionKey()
        {
            throw new NotImplementedException();
        }

        public void setRowKey()
        {
            throw new NotImplementedException();
        }
    }
}
