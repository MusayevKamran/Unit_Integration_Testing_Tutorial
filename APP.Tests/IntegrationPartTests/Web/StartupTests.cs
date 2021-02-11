using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using APP.Repositorys.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace APP.Tests.IntegrationPartTests.Web
{

    public class StartupTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        public StartupTests(WebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public void ServiceExtensions_RegisterServices_IsServicesRegistered()
        {
            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            Assert.NotNull(webHost);
            Assert.NotNull(webHost.Services.GetRequiredService<IBlogRepository>());
        }

        [Fact]
        public async Task HealthCheck_ReturnsStatusOk()
        {
            var response = await _httpClient.GetAsync("/healthcheck");

            response.EnsureSuccessStatusCode() ;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
