using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        IConfigurationRoot config;
        string baseURL;

       [SetUp]
        public void Setup()
        {
            config = new ConfigurationBuilder().AddJsonFile("config.json").Build();
            baseURL = config["BaseURL"];
        }

        [Test]
        public void Test1()
        {
            TestContext.WriteLine($"Base URL: {baseURL}");
            Assert.Pass();
        }
    }
}