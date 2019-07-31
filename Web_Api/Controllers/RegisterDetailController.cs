using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using azureapi.IUtilities;
using azureapi.Models;
using System.Globalization;
using Newtonsoft.Json;
namespace azureapi.Controllers
{
    public class RegisterDetailController
    {
        enum roles { trainer, trainee }
        GenericAzureStorageTableEntity<RegisterDetail> registerdetail = new GenericAzureStorageTableEntity<RegisterDetail>("RegisterDetail");

        public Dictionary<string,DateTime> getStartDateUTC(string userid)
        {

            List<RegisterDetail> details = registerdetail.readbyRowkey(userid);
            Dictionary<string,DateTime> startdates = new Dictionary<string, DateTime>();
            if (details.Count > 0)
            {
                DateTime startdateutc = details.Select(x => x.startdateUTC).First();

                DateTime startdatelocal = DateTime.Parse(details.Select(x=>x.startdate).First());

                startdates.Add("UTC",startdateutc);
                startdates.Add("Local",startdatelocal);
                return startdates;
            }
            else
            {
                startdates.Add("UTC",DateTime.Now);
                startdates.Add("Local",DateTime.Now);
                return startdates;
            }

        }

        public string saveregistdetail(string entityjson)
        {

            Dictionary<string, string> entity = JsonConvert.DeserializeObject<Dictionary<string, string>>(entityjson);

            RegisterDetail registentity = new RegisterDetail();

            registentity.role = entity.Where(x => x.Key == "role").First().Value;

            registentity.RowKey = entity.Where(x => x.Key == "userid").First().Value;

            if (registentity.role == roles.trainer.ToString())
            {
                registentity.PartitionKey = "TRAINER";

            }
            else
            {

                string trainerID = entity.Where(x => x.Key == "trainer").First().Value;


                registentity.PartitionKey = string.Format("TRAINEE-{0}", trainerID);

                registentity.macaddress = entity.Where(x => x.Key == "macaddress").First().Value;

                registentity.height = double.Parse(entity.Where(x => x.Key == "height").First().Value);

                registentity.weight = double.Parse(entity.Where(x => x.Key == "weight").First().Value);

                CultureInfo culture = CultureInfo.InvariantCulture;
                //registentity.startdate = Convert.ToDateTime(entity.Where(x => x.Key == "startdateLocal").First().Value,culture);
                try
                {
                    registentity.startdate = DateTime.ParseExact(entity.Where(x => x.Key == "startdateLocal").First().Value, "dd/MM/yyyy HH:mm:ss", culture).ToString();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                registentity.startdateUTC = Convert.ToDateTime(entity.Where(x => x.Key == "startdateUTC").First().Value);

                registentity.isstartdate = true;

            }
            try
            {
                string result = registerdetail.InsertOrMerge(registentity);
                return result;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return e.Message;
            }


        }

        public string[] GetTrainersList(string userid)
        {
            try
            {
                List<RegisterDetail> trainers = registerdetail.ReadAll("TRAINER");

                List<RegisterDetail> trainees = registerdetail.readbyRowkey(userid);
                string trainersid = string.Empty;

                if (trainees.Count > 0)
                {

                    string pk = trainees.Select(x => x.PartitionKey).First();
                    string[] pks = pk.Split('-');
                    trainersid = pks[1];


                }


                string[] trainerid = trainers.Select(x => x.RowKey).ToArray();
                int indexofid = -1;

                if (trainersid != string.Empty)
                {
                    indexofid = Array.IndexOf(trainerid, trainersid);
                }

                //if the last element of trainerid is 0 then this user is not trained by anyone

                List<string> a = trainerid.ToList();
                a.Add(indexofid.ToString());
                trainerid = a.ToArray();




                return trainerid;
            }
            catch (Exception e)
            {
                string[] a = new string[1];
                a[0] = e.Message;
                return a;
            }

        }

        public string getuserinfo(string userid)
        {


            List<RegisterDetail> lists = registerdetail.readbyRowkey(userid);
            if (lists.Count > 0)
            {
                return JsonConvert.SerializeObject(lists.First());
            }
            else
            {
                return string.Empty;
            }
        }

        public string[] getstudentsinfo(string userid)
        {
            string pk = string.Format("TRAINEE-{0}", userid);
            List<RegisterDetail> students = registerdetail.ReadAll(pk);

            string[] studentsarray = students.Select(x => x.RowKey).ToArray();


            return studentsarray;



        }
    }
}