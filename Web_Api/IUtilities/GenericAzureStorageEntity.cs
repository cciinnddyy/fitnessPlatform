using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Azure;
namespace azureapi.IUtilities
{
    public interface IAzureTable
    {
        void setPartitionKey();
        void setRowKey();
    }
    public class GenericAzureStorageTableEntity<T> where T : TableEntity, IAzureTable, new()
    {

        //傳入 T 
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse( CloudConfigurationManager.GetSetting("StorageConnectionString"));
        CloudTableClient tableClient;
        CloudTable table;

        public GenericAzureStorageTableEntity(string tableName)
        {

            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference(tableName);
            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

        }

        public CloudTable cloudtable { get { return this.table; } }

        public void BatchInsert(List<T> entities)
        {

            //Entity group transaction

            try
            {
                TableBatchOperation insertbatch = new TableBatchOperation();

                foreach (T entity in entities)
                {

                    insertbatch.Insert(entity);

                }

                table.ExecuteBatch(insertbatch);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

            }

        }

        public T Insert(T entity)
        {

            TableOperation insertop = TableOperation.Insert(entity);
            table.Execute(insertop);

            return entity;

        }

        //Return All entities with partition key equal to the parameter
        public List<T> ReadAll(string partitionkey)
        {

            List<T> entities = new List<T>();
            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionkey));

            entities = table.ExecuteQuery(query).ToList();

            return entities;

        }

        public List<T> ReadWeeklyData(string partitionkey, DateTime startTime)
        {
            List<T> entities = new List<T>();
            string query1 = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionkey);
            string query2 = TableQuery.GenerateFilterConditionForDate("timestamp", QueryComparisons.GreaterThanOrEqual, startTime);

            string finalfilter = TableQuery.CombineFilters(query1, TableOperators.And, query2);
            TableQuery<T> finalquery = new TableQuery<T>().Where(finalfilter);

            entities = table.ExecuteQuery(finalquery).ToList();
            return entities;
        }

        public string InsertOrMerge(T entity)
        {
            TableOperation tableop = TableOperation.InsertOrMerge(entity);
            TableResult result = table.Execute(tableop);

            return result.ToString();
        }
        public void setPartitionKey()
        {
            throw new NotImplementedException();
        }

        public void setRowKey()
        {
            throw new NotImplementedException();
        }

        //public List<T> processfromcontent(Dictionary<string,string> dic)
        //{
        //    List<T> listofEntities = new List<T>();

        //    return listofEntities ;
        //}

        public List<T> readbyRowkey(string rowkey)
        {
            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowkey));
            List<T> entity = new List<T>();
            try
            {
                entity = table.ExecuteQuery(query).ToList();
            }
            catch (Exception e)
            {

            }
            return entity;
        }
    }
}