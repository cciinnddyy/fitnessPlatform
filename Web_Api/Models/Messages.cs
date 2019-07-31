using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using azureapi.IUtilities;
namespace azureapi.Models
{
    public class Messages : TableEntity, IAzureTable
    {
        //pk is NO-trainerID

        public string Message { get; set; }


        public void setPartitionKey()
        {
            throw new NotImplementedException();
        }

        public void setRowKey()
        {
            this.RowKey = Guid.NewGuid().ToString();
        }
    }
}