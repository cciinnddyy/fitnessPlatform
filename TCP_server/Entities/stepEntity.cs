using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using System.Runtime.Serialization;
namespace tcplisten.Entities
{
    public class stepEntity : TableEntity, IAzureTable
    {


        public stepEntity() { }


        public bool IsDelete { get; set; }

        public DateTime timestampForCompare { get; set; }

        public DateTime timestamp { get; set; }

        public string timezone { get; set; }

        public double distance { get; set; }

        public string sourceNmae { get; set; }

        

        public List<string> manageSteps { get; set; }

        public string primaryKey { get; set; }

        public string gid { get; set; }

        public double calories { get; set; }

        public bool isModify { get; set; }

        public long steps { get; set; }

        public bool isCommit { get; set; }

        public void setPartitionKey()
        {
            //throw new Exception();

        }

        public void setRowKey()
        {

        }

        //public DateTime convertTime(string timestamp)
        //{

        //    DateTime dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddMilliseconds(double.Parse(timestamp));

        //    return dt;
        //}
    }
}
