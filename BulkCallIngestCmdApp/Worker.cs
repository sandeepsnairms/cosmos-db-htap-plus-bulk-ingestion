using Microsoft.Extensions.Configuration;
using BulkCallProcessor;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace BulkCallIngestCmdApp
{
    public class Worker 
    {
        int offset;
        int itempersecond;
        int subscriberVariance;

        BulkCallProcessor.CallLogger bcp;
        BulkCallProcessor.FakeCalls fc;

        Dictionary<string, string> countryCode;
        UnBilledColl subscriberList;

        public void Init()
        {

            IConfigurationRoot config;

            //load configuration
            var configuration = new ConfigurationBuilder()
                                                 .SetBasePath(Directory.GetCurrentDirectory())
                                                 .AddJsonFile($"appsettings.json");

            config = configuration.Build();


            var endpointUrl = config["endpoint"];
            var authKey = config["authKey"];
            var databaseName = config["databaseName"];
            var containerName = config["containerName"];

            try
            {
                //read Offset
                offset = 90;
                int.TryParse(config["Offset"], out offset);
                //Offset = offset;

                //read SubscriberVariance
                subscriberVariance = 9999;
                int.TryParse(config["SubscriberVariance"], out subscriberVariance);

                //read Item Persecond
                itempersecond = 1;
                int.TryParse(config["ItemPerSecond"], out itempersecond);

                //itempersecond = itempersecond * 2;//since timer invoked every 2 sec

                //read country codes from CSV
                countryCode = File.ReadLines("Country.csv").Select(line => line.Split(',')).ToDictionary(data => data[0], data => data[1]);

                //load listr of  subscribers from JSON
                subscriberList = LoadSubscribersfromFile();


                bcp = new BulkCallProcessor.CallLogger(endpointUrl, authKey, databaseName, containerName);
                fc = new BulkCallProcessor.FakeCalls();
            }
            finally
            {

            }
           
        }

        private UnBilledColl LoadSubscribersfromFile()
        {
            var json = System.IO.File.ReadAllText("subscriber.txt");
            return JsonConvert.DeserializeObject<UnBilledColl>(json);
        }

        public void Execute()
        {
            try
            {
                List<Call> CallDocs = new List<Call>();
                for (int i = 0; i < itempersecond; i++)
                {
                    Call c = fc.GenerateFakeCall(subscriberVariance, offset, subscriberList, countryCode);
                    CallDocs.Add(c);
                }
                Task.Run(async () => { await bcp.InsertCalls(CallDocs); });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           
        }
    }
}