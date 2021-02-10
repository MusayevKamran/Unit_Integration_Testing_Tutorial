using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace APP.Tests.IntegrationTests.Web
{
    public class AppsettingsTest
    {
        public IConfigurationRoot _configuration { get; set; }
        public AppsettingsTest()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
       
        [Fact]
        public void IsNotEmpty()
        {
            Assert.NotNull(_configuration);
        }

        [Fact]
        public void ConnectionStrings_CheckExistKeyBlogDbContextConnection()
        {
            var connectionStrings = _configuration["ConnectionStrings:BlogDbContextConnection"];

            Assert.NotNull(connectionStrings);
        }

        [Fact]
        public void ConnectionStrings_BlogDbContextConnectionValue()
        {
            var databaseConnectionString = "Server=DESKTOP-8O9C6MI;Database=TutorialDB;Trusted_Connection=True;MultipleActiveResultSets=true";

            var connectionStrings = _configuration["ConnectionStrings:BlogDbContextConnection"];

            Assert.Equal(databaseConnectionString, connectionStrings);
        }
    }
}
