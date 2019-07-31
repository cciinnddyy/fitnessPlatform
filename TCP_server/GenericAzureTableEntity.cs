using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Azure;
namespace tcplisten
{
    public interface IAzureTable {
        void setPartitionKey();
        void setRowKey();
    }

    //傳入 T 
    public class GenericAzureTableEntity<T> where T : TableEntity, IAzureTable,new ()
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        CloudTableClient tableClient;
        CloudTable table;
        public GenericAzureTableEntity(string tableName) {

            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference(tableName);
            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

        }

        public void BatchInset(List<T> entities)
        {

            //Entity group transaction

            try
            {

                List<List<T>> entitys = new List<List<T>>();

                if (entities.Count > 100)
                {
                    entitys = new List<List<T>>();
                    int times = entities.Count / 99;
                    int left = entities.Count % 99;
                    int i;
                    for (i = 1; i <= times; i++)
                    {
                        List<T> entii = entities.Skip((i - 1) * 99).Take(i * 99).ToList();
                        entitys.Add(entii);
                    }
                    if (left > 0)
                    {
                        List<T> enti = new List<T>();
                        enti = entities.Skip((i - 1) * 99).Take(left).ToList();
                        entitys.Add(enti);
                    }

                }
                else
                {
                    entitys.Add(entities);
                }


                foreach (List<T> t in entitys)
                {
                    TableBatchOperation insertbatch = new TableBatchOperation();
                    foreach (T entity in t)
                    {

                        insertbatch.Insert(entity);


                    }

                    table.ExecuteBatch(insertbatch);

                }


            }
            catch (Exception e)
            {
                

            }

        }

        public T Insert(T entity) {

            TableOperation insertop = TableOperation.Insert(entity);
            table.Execute(insertop);

            return entity;

        }

        //Return All entities with partition key equal to the parameter
        public List<T> ReadAll(string partitionkey) {

            List<T> entities = new List<T>();
            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionkey));
            entities = table.ExecuteQuery(query).ToList();

            return entities;

        }

        //public DateTime getstartdate(string macAdd) {
        //    DateTime dt = new DateTime();
        //    //table query
        //    return dt;
        //}

        public void setPartitionKey()
        {
            
        }

        public void setRowKey()
        {
           
        }

        //public List<T> processfromcontent(Dictionary<string,string> dic)
        //{
        //    List<T> listofEntities = new List<T>();

        //    return listofEntities ;
        //}
    }
}
