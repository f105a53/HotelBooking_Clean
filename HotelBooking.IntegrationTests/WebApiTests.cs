using System.Threading.Tasks;
using FluentAssertions;
using HotelBooking.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace HotelBooking.IntegrationTests
{
    public class WebApiTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        public WebApiTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly WebApplicationFactory<Startup> _factory;

        [Fact]
        public async Task Startup()
        {
            _factory.Should().NotBeNull();
            var client = _factory.CreateClient();
            client.Should().NotBeNull();
            var result = await client.GetAsync("/");
            result.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}