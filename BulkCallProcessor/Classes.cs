using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkCallProcessor
{
    [Serializable]
    public class UnBilledColl
    {
        public List<UnBilled> Items { get; set; }
    }
    public class UnBilled
    {
        public string id { get; set; }
        public int outgoing { get; set; }
        public int incoming { get; set; }
    }



    public record Device(
        string OS,
        string Make
        );


    public record Call(
         string id,
            DateTime StartDateTime,
            DateTime EndDateTime,
            int DurationSec,
            string CallFrom,
            string CallTo,
            string CallType, //local,national,international
            int CallLocationId, //LocationId of current region
            int BaseLocationId, //Base location of the customer
            bool IsRoaming,
            bool IsIncoming,
            string SubscriberId,
            string DestCountry,
            string BillCycle,
            string pk, // Synthetic  keywith format subscriberId.BillCycle           
            Device device
        );

    enum CallTypes
    {
        local,
        national
    }

}
