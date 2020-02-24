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
            
            async Task<bool> ProcessRepositories()
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var stringTask = client.GetStringAsync("https://adapt.mcpl.ga/api/v1/vehicle");
                var msg = await stringTask;
                return msg.Length > 1;
            }

            async Task firstTask()
            {
                void firstEvent(object source, System.Timers.ElapsedEventArgs e)
                {
                    Task.Factory.StartNew(async () => Console.WriteLine("Call back 1 returns ==> {0} at {1}", await ProcessRepositories(), DateTime.Now));
                }
                var firstTimer = new System.Timers.Timer {Interval = 3000, AutoReset = true, Enabled = true};
                firstTimer.Elapsed += firstEvent;
            }
            
            async Task secTask()
            {
                void secEvent(object source, System.Timers.ElapsedEventArgs e)
                {
                    Task.Factory.StartNew(async () => Console.WriteLine("Call back 2 returns ==> {0} at {1}", await ProcessRepositories(), DateTime.Now));
                }
                var secTimer = new System.Timers.Timer {Interval = 1500, AutoReset = true, Enabled = true};
                secTimer.Elapsed += secEvent;
            }

            await firstTask();
            await secTask();
            
            Console.Read();
        }
    }
}
