using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Assert = Xunit.Assert;

namespace WebStore.WebAPI.Tests.Controllers
{
    [TestClass]
    public class ValuesTests
    {
        private readonly WebApplicationFactory<Startup> _factory = new WebApplicationFactory<Startup>();
        [TestMethod]
        public async Task Values_IntegrityTests()
        {
            var client = _factory.CreateClient();

            string[] expectedData = Enumerable
                .Range(1, 10)
                .Select(i => $"Value - {i}")
                .ToArray();

            var response = await client.GetAsync("api/values");

            response.EnsureSuccessStatusCode();

            var actualData = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();

            int i = 0;
            foreach(var el in actualData)
            {
                Assert.Equal(expectedData[i++], el);
            }
        }
    }
}
