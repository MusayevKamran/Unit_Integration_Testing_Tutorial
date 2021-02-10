using APP.Repositorys.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace APP.Tests.IntegrationPartTests.Web
{

    public class StartupTests
    {

        [Fact]
        public void ServiceExtensions_RegisterServices_IsServicesRegistered()
        {
            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            Assert.NotNull(webHost);
            Assert.NotNull(webHost.Services.GetRequiredService<IBlogRepository>());
        }
    }
}
