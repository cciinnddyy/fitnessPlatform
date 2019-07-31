using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using azureapi.IUtilities;
using azureapi.Models;
using Newtonsoft.Json;
namespace azureapi.Controllers
{
        public class MessagesController
        {
            GenericAzureStorageTableEntity<Messages> megcontrol = new GenericAzureStorageTableEntity<Messages>("Messages");

            public string saveMessage(string messagestr)
            {
                try
                {
                    Dictionary<string, string> megs = JsonConvert.DeserializeObject<Dictionary<string, string>>(messagestr);
                    Messages mg = new Messages();
                    mg.setRowKey();
                    mg.PartitionKey = megs.Where(x => x.Key == "pk").First().Value;
                    mg.Message = megs.Where(x => x.Key == "message").First().Value;
                    
                    megcontrol.Insert(mg);
                    return "OK";
                }
                catch (Exception e)
                {
                    return e.Message;
                }

            }
        
        public List<string> readMsg(string userid) {
            GenericAzureStorageMsgQEntity genMsg = new GenericAzureStorageMsgQEntity("Messages");
             List<string> msgs = genMsg.readMsgQueue(userid);

            return msgs;



        }
        }
    
}