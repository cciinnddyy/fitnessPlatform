using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcplisten.Entities;
using System.Globalization;
namespace tcplisten.Controllers
{
    class fitnessController : DataController<fitnessEntity>
    {
        public override List<fitnessEntity> processData(object[] contentObj, string macadd, string timeZone)
        {

            List<Dictionary<string, string>> dicfinal = new List<Dictionary<string, string>>();
            List<fitnessEntity> finalforSend = new List<fitnessEntity>();

            string userid = "trainee";


            string now = DateTime.UtcNow.ToShortTimeString();


            int count = 1;

            try
            {
                foreach (object i in contentObj)
                {


                    Dictionary<string, object> rawdata = (Dictionary<string, object>)i;

                    Dictionary<string, string> yy = CastDict(rawdata).ToDictionary(x => x.Key.ToString(), x => x.Value.ToString());

                    fitnessEntity entity = new fitnessEntity();

                    entity.isDelete = bool.Parse(yy.Where(x => x.Key == "isDelete").First().Value);

                    string timestamp = yy.Where(x => x.Key == "timestamp").First().Value;

                    string timestampForCompare = yy.Where(x => x.Key == "timestampForCompare").First().Value;


                    entity.timezone = timeZone;

                    entity.timestamp = convertTime(timestamp);

                    entity.timestampForCompare = convertTime(timestampForCompare);

                    entity.sp02 = int.Parse(yy.Where(x => x.Key == "spO2").First().Value);

                    entity.gid = yy.Where(x => x.Key == "gid").First().Value;

                    entity.isCommit = bool.Parse(yy.Where(x => x.Key == "isCommit").First().Value);

                    entity.isModify = bool.Parse(yy.Where(x => x.Key == "isModify").First().Value);

                    entity.sourceNmae = yy.Where(x => x.Key == "sourceName").First().Value;



                    entity.primaryKey = yy.Where(x => x.Key == "primaryKey").First().Value;

                    //MacAddress as Partition key?
                    entity.PartitionKey = macadd;

                    //Rowkey (Userid-week-numberoffile)
                    //userid 
                    //now => time upload to differentiate row key

                    string rowkey = generateRowkey(entity.timestamp, macadd, userid);
                    entity.RowKey = string.Format("{0}-{1}-{2}", rowkey, count, now);

                    finalforSend.Add(entity);



                    dicfinal.Add(yy);
                    count++;


                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return finalforSend;
        }



    }
}
