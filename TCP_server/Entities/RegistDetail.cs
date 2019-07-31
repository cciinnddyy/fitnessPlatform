using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
namespace tcplisten.Entities
{
    class RegistDetail : TableEntity, IAzureTable
    {
        public DateTime startdate;
        public string macAddress;
        public string userid;
        public string password;

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
