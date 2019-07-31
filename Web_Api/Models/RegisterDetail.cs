using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using azureapi.IUtilities;
namespace azureapi.Models
{
    public class RegisterDetail : TableEntity, IAzureTable
    {
        //Use user id as partition key
        //PK for Trainer is TRAINER
        //PK for Trainee is TRAINEE-{Trainer's userid}
        //RK is UserID

        public string macaddress { get; set; }

        public string startdate { get; set; }

        public DateTime startdateUTC { get; set; }

        public string role { get; set; }

        public string students { get; set; }

        public double? weight { get; set; }

        public double? height { get; set; }

        public bool isstartdate { get; set; }

        public RegisterDetail()
        {
            this.macaddress = "";
            this.startdate = "";
            this.startdateUTC = DateTime.Now;
            this.role = "";
            this.students = "";
            this.weight = 0;
            this.height = 0;
            this.isstartdate = false;
        }


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