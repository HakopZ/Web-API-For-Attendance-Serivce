using System.Data;
using System.Data.SqlClient;
using Test_2;
using Test_2.ScheduleSetup;

namespace SqlUnitTest
{
    public class UnitTest
    {
        static Uri baseAddress = new Uri("http://gmr-124-2-1:5247");

        static async Task<HttpResponseMessage> GetAsync(HttpClient request, string requestURL)
        {
            var response = await request.GetAsync(requestURL);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed");
            }
            return response;
        }
        [Fact]
        public async void GrabInstructors()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = baseAddress;

            var message = await GetAsync(client, client.BaseAddress + "App/GetInstructorInfos");
            var content = message.Content;
            var instructorIDs = await content.ReadAsAsync<List<int>>();
            Assert.True(instructorIDs.Count == 2);
        }
        [Fact]
        public async void GetCurrentSession()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = baseAddress;

            var message = await GetAsync(client, client.BaseAddress + "App/GetScheduleForTheDay");
            var content = message.Content;
            var instructorIDs = await content.ReadAsAsync<List<ScheduledClass>>();
            ;

        }
    }
}