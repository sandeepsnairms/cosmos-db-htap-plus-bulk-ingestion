using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkCallProcessor
{
    public class FakeCalls
    {
        Faker f = new Faker();

        //device list
        Device dv1 = new Device("iOS 15", "iPhone 12");
        Device dv2 = new Device("Android 10", "OnePlus 7");

        int subVarianceIndex=0;
        int subVariancePrevIndex=1;

        public Call GenerateFakeCall(int subscriberVariance, int offset, UnBilledColl subscriberList, Dictionary<string, string> countryCode)
        {
            //id  
            string id = Guid.NewGuid().ToString();

            //subsriber id
            int intVal;

            subVarianceIndex += 10;

            if (subVarianceIndex > subscriberVariance)
            {
                subVarianceIndex = 10;
                subVariancePrevIndex = 0;
            }

            intVal = f.Random.Number(subVariancePrevIndex, subVarianceIndex);

            subVariancePrevIndex=subVarianceIndex;

            string subscriberId = subscriberList.Items[intVal].id;


            //fake call time and duration
            int duration = f.Random.Number(5, 500);
            DateTime dtStart;
            if (offset > 0)
            {
                dtStart = GenRandomTime(offset);
            }
            else
            {
                dtStart = DateTime.Now;
            }
            DateTime dtEnd = dtStart.AddSeconds(duration);


            //fake call location and calculate roaming
            int baselocationId = 5;
            int curentlocationId = f.Random.Int(1, 5);
            bool roaming = false;
            if (baselocationId != curentlocationId)
                roaming = true;

            //fake phone numbers
            string callFrom = String.Empty;
            string callTo = String.Empty;
            string callType = String.Empty;
            string phoneNumber = String.Empty;
            string country = string.Empty;

            bool international = f.Random.Bool();
            if (!international)
            {
                phoneNumber = f.Phone.PhoneNumber("091-X##-###-####");
                phoneNumber = phoneNumber.Replace("X", f.Random.Int(2, 9).ToString());
                callType = f.PickRandom<CallTypes>().ToString();
                country = "India";
            }
            else
            {
                int CountryIndex = f.Random.Int(1, 212);
                string CountryCode = countryCode.ElementAt(CountryIndex).Value;
                country = countryCode.ElementAt(CountryIndex).Key;
                phoneNumber = f.Phone.PhoneNumber("X##-###-####");
                phoneNumber = CountryCode + "-" + phoneNumber.Replace("X", f.Random.Int(2, 9).ToString());
                callType = "international";
            }

            //Set incoming/outgoing call
            bool incomingCall = f.Random.Bool();
            if (incomingCall == true)
            {
                callFrom = phoneNumber;
                callTo = subscriberId;
            }
            else
            {
                callTo = phoneNumber;
                callFrom = subscriberId;
            }

            //calc Billing Cycle
            string billCycle = $"{dtStart.ToString("MMM").ToUpper()}{dtStart.Year}";

            //gen partition key
            string pk = $"{subscriberId}.{billCycle}";

            Device currDevice;
            //Device Info            
            if (f.Random.Bool())
                currDevice = dv2;
            else
                currDevice = dv1;


            //initialize Record
            var callrecord = new Call(id, dtStart, dtEnd, duration, callFrom, callTo, callType, curentlocationId, baselocationId, roaming, incomingCall, subscriberId, country, billCycle, pk, currDevice);

            return callrecord;

        }

        private DateTime GenRandomTime(int offset)
        {
            Faker f = new Faker();
            int days = 0;
            int hours = f.Random.Number(0, 23);
            int minutes = f.Random.Number(0, 59);
            int seconds = f.Random.Number(0, 59);


            if (offset > 0)
                days = f.Random.Number(offset * -1, 0);

            return System.DateTime.Today.AddDays(days).AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
        }
    }
}
