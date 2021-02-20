using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;
namespace NamesApi.Tests
{
    public static class ContentHelper
    {
        public static StringContent GetStringContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
        }
    }
    public class TestNameEntry
    {
        private readonly TestServer server;
        private readonly HttpClient client;
        private static IConfiguration getIConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                // .SetBasePath("/home/xdevx/XFiles/Dotnet/NamerApi/src")
                .AddJsonFile("appsettings.Development.json")
                .Build();
        }
        public TestNameEntry()
        {
            server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseConfiguration(getIConfiguration())
                .UseStartup<Startup>());
            client = server.CreateClient();
            
        }
        [Fact]
        // TODO: this test really belongs with controller tests, 
        // but this mock up is quite different for the time being, so gonna leave this here for now
        public async Task TestInvalidPostName_Returns400()
        {
            var request = new
            {
                Url = "/api/names",
                Body = new
                {
                    Name = "",
                    Weight = 0.3f
                }
            };

            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStreamAsync();

            // TODO: find a better way to do this assertion.
            Assert.Equal("BadRequest", response.StatusCode.ToString());
        }
    }
}