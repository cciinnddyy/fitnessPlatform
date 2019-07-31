using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using azureapi.IUtilities;
using azureapi.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
namespace azureapi.Controllers
{
    public class stepController
    {
        //compare login data by user id
        //find mac address by user id in registerdetail table
        //RegistDetail Table contain userID and macAddress
        string macAddress = "C4:3E:BD:71:DE:C2";


        //GenericAzureStorageEntity<stepData> step = new GenericAzureStorageEntity<stepData>("stepTable");
        RegisterDetailController rctrl = new RegisterDetailController();
        //GetWeeklyCalories
        //Search by user ID => MacAddress
        public double[] GetWeeklyCalories()
        {
            GenericAzureStorageTableEntity<stepData> step = new GenericAzureStorageTableEntity<stepData>("stepTable");
            try
            {


                //List<stepData> datas = step.ReadAll(macAddress);

                //GetTheWeekConstraint
                DateTime current = DateTime.Now;

                int dayofweek = (int)current.DayOfWeek;


                DateTime startTime = current.AddDays(-7).AddDays(dayofweek);

                List<stepData> datas = step.ReadWeeklyData(macAddress, startTime);



                double[] calories = new double[datas.Count];

                string timezone = datas[0].timezone;






                //source data timezone 
                int count = 0;
                foreach (stepData data in datas)
                {

                    calories[count] = data.calories;
                    count++;
                }
                return calories;
            }
            catch (Exception e)
            {
                double[] exception = new double[1];
                exception[0] = 10.01;
                return exception;

            }


        }

        //The goal setting is set by the Gym Trainer
        //public double GetWeeklyCaloriesGoal() {

        //}



        public Goals getWeeklyCalories(string userid)
        {

            List<stepData> thisweek = getWeeklyEntitiesByUserID(userid);
            Goals goal = new Goals();

            if (thisweek.Count > 0)
            {
                goal.stepsGoal = thisweek.First().stepsGoal;
                goal.calorieGoal = thisweek.First().calorieGoal;
                goal.userid = userid;
                foreach (stepData data in thisweek)
                {

                    goal.calories += data.calories;
                    goal.steps += data.steps;


                }
            }

            return goal;
        }


        public string goalSetting(string userid)
        {

            GenericAzureStorageTableEntity<stepData> step = new GenericAzureStorageTableEntity<stepData>("stepTable");

            //Find this week data
            List<stepData> thisweek = getWeeklyEntitiesByUserID(userid);
            if (thisweek.Count > 0)
            {

                //Update entity

                foreach (stepData data in thisweek)
                {

                    try
                    {
                        var entity = new DynamicTableEntity(data.PartitionKey, data.RowKey);
                        entity.ETag = "*";

                        entity.Properties.Add("calorieGoal", new EntityProperty(400));
                        entity.Properties.Add("stepsGoal", new EntityProperty(10000));

                        var mergeOperation = TableOperation.Merge(entity);
                        step.cloudtable.Execute(mergeOperation);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                }
            }


            return "OK";
        }

        public string goalsetting(string goalobj)
        {
            GenericAzureStorageTableEntity<stepData> step = new GenericAzureStorageTableEntity<stepData>("stepTable");
            Dictionary<string, string> goalentity = JsonConvert.DeserializeObject<Dictionary<string, string>>(goalobj);

            string userid = goalentity.Where(x => x.Key == "userid").First().Value;
            int stepsgoal = int.Parse(goalentity.Where(x => x.Key == "stepsgoal").First().Value);
            int calgoal = int.Parse(goalentity.Where(x => x.Key == "calgoals").First().Value);
            List<stepData> thisweek = getWeeklyEntitiesByUserID(userid);
            if (thisweek.Count > 0)
            {

                //Update entity

                foreach (stepData data in thisweek)
                {

                    try
                    {
                        var entity = new DynamicTableEntity(data.PartitionKey, data.RowKey);
                        entity.ETag = "*";

                        entity.Properties.Add("calorieGoal", new EntityProperty(calgoal));
                        entity.Properties.Add("stepsGoal", new EntityProperty(stepsgoal));

                        var mergeOperation = TableOperation.Merge(entity);
                        step.cloudtable.Execute(mergeOperation);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                }

                return "ok";
            }

            else
            {
                return string.Empty;
            }



        }

        public List<stepData> getWeeklyEntitiesByUserID(string userid)
        {

            GenericAzureStorageTableEntity<stepData> step = new GenericAzureStorageTableEntity<stepData>("stepTable");


            Dictionary<string,DateTime> startdate = rctrl.getStartDateUTC(userid);
            DateTime nowUTC = DateTime.UtcNow;


            TimeSpan timespan = nowUTC.Subtract(startdate.Where(x => x.Key == "UTC").First().Value);
            int weeks = timespan.Days / 7;

            DateTime localdate = startdate.Where(x => x.Key == "Local").First().Value;

            List<stepData> entities = step.ReadAll(macAddress);
            List<stepData> thisweek = new List<stepData>();
            foreach (stepData data in entities)
            {

                String[] rowkeys = data.RowKey.ToString().Split('-');
                //userid-week-number-time
                //int week = int.Parse(rowkeys[1]);
                string userID = rowkeys[0].ToString();
                TimeSpan tspan = data.timestamp.Subtract(localdate);
                int week = tspan.Days / 7;

                if (userid == userID)
                {
                    if (week == weeks)
                    {
                        thisweek.Add(data);
                    }
                }
            }

            return thisweek;
        }


    }
}