using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AsycTaskTimerTest
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        
        static async Task Main(string[] args)
        {
            
            async Task<bool> testJson()
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var stringTask = await client.GetStringAsync("https://cat-fact.herokuapp.com/facts");
                    return stringTask.Length > 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Call back returns ==> {0} at {1} err:----> {3}", false, DateTime.Now, e);
                    return false;
                }
            }

            async Task firstTask()
            {
                var firstTimer = new System.Timers.Timer {Interval = 4200, AutoReset = true, Enabled = true};
                firstTimer.Elapsed += (object source, System.Timers.ElapsedEventArgs e) =>
                {
                    var curDate = DateTime.Now;
                    Console.WriteLine("----> Started new call-01 at {0} <----", curDate);
                    Task.Factory.StartNew(async () => Console.WriteLine("===>> Call back 1 returns ==> {0} at {1} then ended by {2}", await testJson(), curDate, DateTime.Now));
                };
            }
            
            async Task secTask()
            {
                var secTimer = new System.Timers.Timer {Interval = 2500, AutoReset = true, Enabled = true};
                secTimer.Elapsed += (object source, System.Timers.ElapsedEventArgs e) =>
                {
                    var curDate = DateTime.Now;
                    Console.WriteLine("----> Started new call-02 at {0} <----", curDate);
                    Task.Factory.StartNew(async () => Console.WriteLine("===>> Call back 2 returns ==> {0} at {1} then ended by {2}", await testJson(), curDate, DateTime.Now));
                };
            }

            await firstTask();
            await secTask();
            
            Console.Read();
        }
    }
}
