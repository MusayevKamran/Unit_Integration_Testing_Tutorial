using System.IO;
using System.Linq;
using System.Threading.Tasks;
using APP.Repositorys.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using Xunit;

namespace APP.Tests
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
