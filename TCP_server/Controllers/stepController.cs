using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcplisten.Entities;
using System.Globalization;
namespace tcplisten.Controllers
{
    public class StepController : DataController<stepEntity>
    {
        public override List<stepEntity> processData(object[] contentObj, string macadd, string timeZone)

        {
            List<Dictionary<string, string>> dicfinal = new List<Dictionary<string, string>>();
            List<stepEntity> finalforSend = new List<stepEntity>();
            string userid = "trainee";

            //Generate RowKey

            string now = DateTime.UtcNow.ToShortTimeString();


            int count = 1;

            foreach (object i in contentObj)
            {


                Dictionary<string, object> rawdata = (Dictionary<string, object>)i;

                Dictionary<string, string> yy = CastDict(rawdata).ToDictionary(x => x.Key.ToString(), x => x.Value.ToString());

                stepEntity entity = new stepEntity();

                entity.IsDelete = bool.Parse(yy.Where(x => x.Key == "isDelete").First().Value);

                string timestamp = yy.Where(x => x.Key == "timestamp").First().Value;

                string timestampForCompare = yy.Where(x => x.Key == "timestampForCompare").First().Value;

                entity.timezone = timeZone;

                entity.timestamp = convertTime(timestamp);

                entity.timestampForCompare = convertTime(timestampForCompare);

                entity.calories = double.Parse(yy.Where(x => x.Key == "calories").First().Value);

                entity.distance = double.Parse(yy.Where(x => x.Key == "distance").First().Value);

                entity.gid = yy.Where(x => x.Key == "gid").First().Value;

                entity.isCommit = bool.Parse(yy.Where(x => x.Key == "isCommit").First().Value);

                entity.isModify = bool.Parse(yy.Where(x => x.Key == "isModify").First().Value);

                entity.sourceNmae = yy.Where(x => x.Key == "sourceName").First().Value;

                entity.steps = long.Parse(yy.Where(x => x.Key == "steps").First().Value);

                entity.primaryKey = yy.Where(x => x.Key == "primaryKey").First().Value;

                //MacAddress as Partition key?
                entity.PartitionKey = macadd;


                string rowkey = generateRowkey(entity.timestamp, macadd, userid);
                entity.RowKey = string.Format("{0}-{1}-{2}", rowkey, count, now);


                finalforSend.Add(entity);



                dicfinal.Add(yy);
                count++;


            }

            return finalforSend;
        }




    }
}
