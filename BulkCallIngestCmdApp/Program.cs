using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace BulkCallIngestCmdApp
{
    public class Program
    {
        private static Timer timer;

        private static Worker w;

        static void Main(string[] args)
        {

            w = new Worker();
            w.Init();
            timer = new Timer(
                callback: new TimerCallback(TimerTask),
                state: w,
                dueTime: 1000,
                period: 1000);

            while (Console.Read() != 'q')
            {
                Task.Delay(1000).Wait();
            }

            timer.Dispose();
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: done.");
        }

        private static void TimerTask(object state)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: starting a new callback.");
            try
            {                 
                //var state = timerState as TimerState;
                //Interlocked.Increment(ref state.Counter);
                //Worker w = (Worker)state;
                w.Execute();
            }
            catch(Exception ex)
            { Console.WriteLine(ex.ToString()); }
        }
    }
    
}