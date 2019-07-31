using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using System.Runtime.Serialization;
using System.Collections;
using tcplisten.Entities;
namespace tcplisten
{

    public abstract class DataController<T>
    {
        abstract public List<T> processData(object[] objsContent, string macadd, string timeZone);

        public IEnumerable<DictionaryEntry> CastDict(IDictionary dictionary)
        {
            foreach (DictionaryEntry entry in dictionary)
            {
                yield return entry;
            }
        }

        public DateTime convertTime(string timestamp)
        {

            DateTime dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddMilliseconds(double.Parse(timestamp));

            return dt;
        }

        public string generateRowkey(DateTime recordtime,string macaddress,string userid) {

            //GenericAzureTableEntity<RegistDetail> registTable = new GenericAzureTableEntity<RegistDetail>("RegisterDetail");
            //getstartdate and userid in the registerdetail table

            //List<RegistDetail> registerdetail = registTable.ReadAll(macaddress);
            //RegistDetail onedetail = new RegistDetail();

            //if (registerdetail.Count > 0)
            //{
            //    onedetail = registerdetail.Where(m => m.userid == userid).First();
            //}
            //else {
            //    onedetail = registerdetail.First();
            //}

            //DateTime startdate = onedetail.startdate;
            DateTime startdate = DateTime.Parse("03-11-2019");
            TimeSpan span =recordtime.Subtract(startdate);
            int week = span.Days / 7;

            string key = string.Format("{0}-{1}", userid, week);
            
            return key;
        }
        
    }







}
