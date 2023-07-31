using Microsoft.Owin.Hosting;
using System.Web.Http;
using Owin;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Security;

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

        
        static HttpClient client;


        static string baseAddress = "http://localhost:5247/";

        static async Task<WeatherForecast> SendWeather(WeatherForecast weather)
        {
            var response = await client.PostAsJsonAsync(client.BaseAddress + "weatherForecast", weather);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<WeatherForecast>();
        }
        static async Task<HttpResponseMessage> GetAsync(HttpClient request, string requestURL)
        {
            //List<T> temp = new List<TimeSlot>();
            var response = await request.GetAsync(requestURL);
            //HttpResponseMessage response = await client.GetAsync(path);
            if (!response.IsSuccessStatusCode)
            {
                ;
            }
            return response;
        }

        static async Task RunAsync()
        {
            //// Update port # in the following line.
            //NetworkCredential defaultCredential = CredentialCache.DefaultNetworkCredentials;
            ////NetworkCredential credential = new NetworkCredential("hakop.zarikyan", "GreatMinds217", "GMR.local");
            //var credentialCache = new CredentialCache() { { new Uri(baseAddress + "Computer/Test"), "Negotiate", credential} };
            HttpClientHandler handler = new HttpClientHandler()
            {
                UseDefaultCredentials = true,
                Credentials = CredentialCache.DefaultCredentials
            };
            handler.PreAuthenticate = true;
            
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(baseAddress);

          //  client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Negotiate");
            //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await GetAsync(client, "Computer/Test");
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
