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
        public void Startup()
        {
            _factory.Should().NotBeNull();
            _factory.CreateClient().Should().NotBeNull();
        }
    }
}