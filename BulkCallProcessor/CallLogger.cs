using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

using System.Threading.Tasks;


using Microsoft.Azure.Cosmos;
using System.Net;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using Database = Microsoft.Azure.Cosmos.Database;
using Container = Microsoft.Azure.Cosmos.Container;
using System.Linq;
using Newtonsoft.Json;

namespace BulkCallProcessor
{      

    public class CallLogger
    {              

        Database database;
        Container container;


        public CallLogger(string endpointUrl, string authKey, string databaseName, string containerName)
        {        

            CosmosClient client = GetBulkClientInstance(endpointUrl, authKey);

            database = client.GetDatabase(databaseName);
            container = database.GetContainer(containerName);
        }

        private static CosmosClient GetBulkClientInstance(string endpoint,string authKey)
        {

            return new CosmosClient(endpoint, authKey, new CosmosClientOptions() { AllowBulkExecution = true, ConnectionMode = ConnectionMode.Direct });
        }

        

        public async Task InsertSubscriber(UnBilledColl subscriberList)
        {           
            var containerUnBilled = database.GetContainer("UnBilledCallRecords");
            await CreateSubscribersConcurrentlyAsync(containerUnBilled, subscriberList);

        }

        public async Task InsertCalls(List<Call> CallDocs)
        {
            await CreateItemsConcurrentlyAsync(container, CallDocs);

        }
        private async Task CreateSubscribersConcurrentlyAsync(Container container, UnBilledColl subscriberList)
        {
            BulkOperations<UnBilled> bulkOperations = new BulkOperations<UnBilled>(subscriberList.Items.Count);
            foreach (UnBilled ucall in subscriberList.Items)
            {
                bulkOperations.Tasks.Add(CaptureOperationResponse(container.CreateItemAsync(ucall, new PartitionKey(ucall.id)), ucall));
            }
            BulkOperationResponse<UnBilled> bulkOperationResponse = await bulkOperations.ExecuteAsync();
        }

        
        public class BulkOperations<T>
        {
            public readonly List<Task<OperationResponse<T>>> Tasks;

            private readonly Stopwatch stopwatch = Stopwatch.StartNew();

            public BulkOperations(int operationCount)
            {
                this.Tasks = new List<Task<OperationResponse<T>>>(operationCount);
            }

            public async Task<BulkOperationResponse<T>> ExecuteAsync()
            {
                await Task.WhenAll(this.Tasks);
                this.stopwatch.Stop();
                return new BulkOperationResponse<T>()
                {
                    TotalTimeTaken = this.stopwatch.Elapsed,
                    TotalRequestUnitsConsumed = this.Tasks.Sum(task => task.Result.RequestUnitsConsumed),
                    SuccessfulDocuments = this.Tasks.Count(task => task.Result.IsSuccessful),
                    Failures = this.Tasks.Where(task => !task.Result.IsSuccessful).Select(task => (task.Result.Item, task.Result.CosmosException)).ToList()
                };
            }
        }
        int intfailed;
        private async Task CreateItemsConcurrentlyAsync(Container container, List<Call> CallDocs )
        {
            try
            {
                BulkOperations<Call> bulkOperations = new BulkOperations<Call>(CallDocs.Count);
                foreach (Call call in CallDocs)
                {
                    bulkOperations.Tasks.Add(CaptureOperationResponse(container.CreateItemAsync(call, new PartitionKey(call.pk)), call));
                }
                BulkOperationResponse<Call> bulkOperationResponse = await bulkOperations.ExecuteAsync();

                intfailed = 0;
                if (bulkOperationResponse.Failures != null)
                {
                    intfailed = bulkOperationResponse.Failures.Count;
                }
                Console.Write($"Processed: {CallDocs.Count} Success: {bulkOperationResponse.SuccessfulDocuments} Failures: {intfailed}   RU: {bulkOperationResponse.TotalRequestUnitsConsumed}   TimeTaken:{bulkOperationResponse.TotalTimeTaken}");
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        public class BulkOperationResponse<T>
        {
            public TimeSpan TotalTimeTaken { get; set; }
            public int SuccessfulDocuments { get; set; } = 0;
            public double TotalRequestUnitsConsumed { get; set; } = 0;

            public IReadOnlyList<(T, Exception)>? Failures { get; set; }
        }

        public class OperationResponse<T>
        {
            public T? Item { get; set; }
            public double RequestUnitsConsumed { get; set; } = 0;
            public bool IsSuccessful { get; set; }
            public Exception? CosmosException { get; set; }
        }

        private static async Task<OperationResponse<T>> CaptureOperationResponse<T>(Task<ItemResponse<T>> task, T item)
        {
            try
            {
                ItemResponse<T> response = await task;
                return new OperationResponse<T>()
                {
                    Item = item,
                    IsSuccessful = true,
                    RequestUnitsConsumed = task.Result.RequestCharge
                };
            }
            catch (Exception ex)
            {
                if (ex is CosmosException cosmosException)
                {
                    return new OperationResponse<T>()
                    {
                        Item = item,
                        RequestUnitsConsumed = cosmosException.RequestCharge,
                        IsSuccessful = false,
                        CosmosException = cosmosException
                    };
                }

                return new OperationResponse<T>()
                {
                    Item = item,
                    IsSuccessful = false,
                    CosmosException = ex
                };
            }
        }

        
    }
}

  
