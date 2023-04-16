using Microsoft.Owin.Hosting;
using System.Web.Http;
using Owin;
using System.Net.Http.Headers;



/// <summary>
/// 
/// DEMO WITH WEATHER FORECAST BUT THAT HAS BEEN DELETED IN OTHER FORECAST
/// CODE IS FINE JUST NO MORE WEATHER FORECAST DATA
/// LOOK FOR REFERENCE
/// 
/// </summary>
/// 

namespace TestWebAPIWithConsole
{

    public class TimeSlot
    {
        public int ID { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public TimeSlot(int id, DateTime start, DateTime end)
        {
            ID = id;
            Start = start;
            End = end;
        }
    }
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
    internal class Program
    {
        static HttpClient client = new HttpClient();


        static string baseAddress = "http://GMR-124-2-1:5247/";

        static async Task<WeatherForecast> SendWeather(WeatherForecast weather)
        {
            var response = await client.PostAsJsonAsync(client.BaseAddress + "weatherForecast", weather);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<WeatherForecast>();
        }
        static async Task<List<TimeSlot>> GetTimeSlotAsync(string path)
        {
            List<TimeSlot> temp = new List<TimeSlot>();
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                temp = await response.Content.ReadAsAsync<List<TimeSlot>>();
            }
            return temp;
        }
        
        static async Task RunAsync()
        {
                // Update port # in the following line.
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await GetTimeSlotAsync(baseAddress + "App/Session/GetTimeslotInfos");
                if (response != null)
                {
                    ;
                    return;
                }
            

        }
        static void Main(string[] args)
        {

            RunAsync().Wait();
            
        }
    }
}
