using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.Azure;
using Newtonsoft.Json;
namespace azureapi.IUtilities
{
    public class GenericAzureStorageMsgQEntity
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        CloudQueue queue;
        CloudQueueClient queueClient;

        public GenericAzureStorageMsgQEntity (string queueName){

            this.queueClient = storageAccount.CreateCloudQueueClient();
            this.queue = queueClient.GetQueueReference(queueName);
        }
        
        public List<string> readMsgQueue(string userid) {
            //Peek without changing visibility
            //Get message is invisible
            
            List<string> messages = new List<string>();
            foreach (var msg in queue.PeekMessages(20)) {
                 Dictionary<string,string> msgr = JsonConvert.DeserializeObject<Dictionary<string,string>>(msg.ToString());
                if (msgr.Where(x=>x.Key=="userid").First().Value == userid) {
                   
                    messages.Add(msg.ToString());
                    queue.DeleteMessage(msg);
                }
                
            }
            return messages;
        }

        public void addMsg(string JsonMessage)
        {
            CloudQueueMessage msg = new CloudQueueMessage(JsonMessage);
            queue.AddMessage(msg);
        }
    }
}