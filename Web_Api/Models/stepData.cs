using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using azureapi.IUtilities;
namespace azureapi.Models
{
    public class stepData : TableEntity, IAzureTable
    {



        public stepData() { }


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



        public int calorieGoal { get; set; }

        public int stepsGoal { get; set; }

        public void setPartitionKey()
        {
            //throw new Exception();

        }

        public void setRowKey()
        {

        }



    }

    public class Goals
    {
        public double calories;
        public long steps;
        public int stepsGoal;
        public int calorieGoal;
        public string userid;
    }
}