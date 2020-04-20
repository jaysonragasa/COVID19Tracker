using COVID19Tracker.Library.APIClient.DataSources.Demo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace COVID19Tracker.Library.Tests
{
    [TestClass]
    public class TestCountryDetailedData
    {
        CountryDetailedData ccd = new CountryDetailedData();

        [TestMethod]
        [TestInitialize]
        public async Task TestGetDataByCountryName()
        {
            await ccd.GetDataByCountryCode("PH");

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task TestGetAllRegionsAsync()
        {
            await ccd.GetAllRegionsAsync();

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task TestGetAllCitiesAsync()
        {
            await ccd.GetAllCitiesAsync();

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task TestGetCitiesByRegionNameAsync()
        {
            await ccd.GetCitiesByRegionNameAsync("NCR");

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task TestGetDataByCityNameAsync()
        {
            await ccd.GetDataByCityNameAsync("City of Manila");

            Assert.IsTrue(true);
        }
    }
}
