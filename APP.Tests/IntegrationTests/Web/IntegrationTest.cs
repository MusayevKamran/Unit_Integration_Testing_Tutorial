using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace APP.Tests.IntegrationTests.Web
{
    public class IntegrationTest
    {
        [Fact]
        public async Task BasicIntegrationTest()
        {
            // Arrange
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();

                    webHost.Configure(app => 
                        app.Run(async ctx => await ctx.Response.WriteAsync("Hello World!")));
                });

            var host = await hostBuilder.StartAsync();

            var client = host.GetTestClient();

            var response = await client.GetAsync("/");

            var responseString = await response.Content.ReadAsStringAsync();

            host.Dispose();

            Assert.Equal("Hello World!", responseString);
        }


    }
}
