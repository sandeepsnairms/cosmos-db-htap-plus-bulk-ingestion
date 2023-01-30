using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using BulkCallProcessor;
using BulkCallIngestWinApp;

namespace BulkCallIngestWinApp
{
    public partial class Form1 : Form
    {

        int offset;
        int itempersecond;
        int subscriberVariance;

        BulkCallProcessor.CallLogger bcp;
        BulkCallProcessor.FakeCalls fc;

        Dictionary<string, string> countryCode;
        UnBilledColl subscriberList;

        int dCount;


        public Form1()
        {
            InitializeComponent();
            timer1 = new extTimer();
            timer1.Tick += Timer1_Tick;
            timer1.Interval = 1000; //1 sec

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

            //read Offset
            offset = 90;
            int.TryParse(config["Offset"], out offset);
            //Offset = offset;

            //read SubscriberVariance
            subscriberVariance = 9999;
            int.TryParse(config["SubscriberVariance"], out subscriberVariance);

            //read Item Persecond
            itempersecond = 9999;
            int.TryParse(config["ItemPerSecond"], out itempersecond);

            //read country codes from CSV
            countryCode = File.ReadLines("Country.csv").Select(line => line.Split(',')).ToDictionary(data => data[0], data => data[1]);

            //load listr of  subscribers from JSON
            subscriberList = LoadSubscribersfromFile();

            bcp = new BulkCallProcessor.CallLogger(endpointUrl, authKey, databaseName, containerName);
            fc = new BulkCallProcessor.FakeCalls();

        }

        private UnBilledColl LoadSubscribersfromFile()
        {
            var json = System.IO.File.ReadAllText("subscriber.txt");
            return JsonConvert.DeserializeObject<UnBilledColl>(json);
        }

        private void Timer1_Tick(object? sender, EventArgs e)
        {
            List<Call> CallDocs = new List<Call>();
            for (int i = 1; i <= timer1.ingRate; i++)
            {
                dCount++;
                //generate fake calls
                CallDocs.Add(fc.GenerateFakeCall(subscriberVariance, offset, subscriberList, countryCode));
            }
            Debug.WriteLine($"Generated {timer1.ingRate} Fake Calls");

            Task.Run(async () => { await bcp.InsertCalls(CallDocs); });
        }

        private void start_Click(object sender, EventArgs e)
        {
            dCount = 0;
            int i;
            if (int.TryParse(ingRate.Text, out i))
            {
                timer1.ingRate = i;
                timer1.Enabled = true;
            }
        }

        private void stop_Click(object sender, EventArgs e)
        {
            timer1.Enabled= false;
            MessageBox.Show($"Ingested {dCount} documents");
        }

        private void update_Click(object sender, EventArgs e)
        {
            int i;
            if (int.TryParse(ingRate.Text, out i))
            {
                timer1.ingRate = i;
            }
        }

        private void ingRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void singleInsert_Click(object sender, EventArgs e)
        {
            int i;
            if (int.TryParse(ingRate.Text, out i))
            {
                timer1.ingRate = i;
                Timer1_Tick(null, null);
            }
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void prepSubscriber_Click(object sender, EventArgs e)
        {        

            Bogus.Faker f = new Bogus.Faker();
            Dictionary<string,string> dic = new Dictionary<string,string>();
            while(dic.Count<10000)
            {
                string p2, p3;
                int intVal;

                intVal = f.Random.Number(50, 750); ;
                p2 = string.Format(String.Format("{0:000}", intVal));

                intVal = f.Random.Number(1250, 6550); ;
                p3 = string.Format(String.Format("{0:000}", intVal));

                string tmpsubscriberId = "091-97#-" + p2 + "-" + p3;
                string subscriberId = f.Phone.PhoneNumber(tmpsubscriberId);
                dic.TryAdd(subscriberId, subscriberId);
            }

            UnBilledColl ucoll=new UnBilledColl();
            ucoll.Items = new List<UnBilled>();
            foreach (string s in dic.Keys)
            {
                UnBilled u = new UnBilled();
                u.id = s;
                u.incoming = 0;
                u.outgoing = 0;

                ucoll.Items.Add(u);
            }

            var json = JsonConvert.SerializeObject(ucoll);

            System.IO.File.WriteAllText("subscriber.txt", json);
            MessageBox.Show("Upload complete");
        }

        private void upSubscriber_Click(object sender, EventArgs e)
        {
            Task.Run(async () => { await bcp.InsertSubscriber(subscriberList); });//.GetAwaiter().GetResult();
            MessageBox.Show("Upload complete");
        }
    }
}



