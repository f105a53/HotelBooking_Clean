using System.Threading.Tasks;
using FluentAssertions;
using HotelBooking.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace HotelBooking.IntegrationTests
{
    public class MvcTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        public MvcTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly WebApplicationFactory<Startup> _factory;

        [Fact]
        public async Task StartupAsync()
        {
            _factory.Should().NotBeNull();
            var client = _factory.CreateClient();
            client.Should().NotBeNull();
            var result = await client.GetAsync("/");
            result.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}