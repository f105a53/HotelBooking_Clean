using FluentAssertions;
using HotelBooking.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace HotelBooking.IntegrationTests
{
    public class MvcTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        public MvcTests(WebApplicationFactory<Mvc.Startup> factory)
        {
            _factory = factory;
        }

        private readonly WebApplicationFactory<Mvc.Startup> _factory;

        [Fact]
        public void Startup()
        {
            _factory.Should().NotBeNull();
            _factory.CreateClient().Should().NotBeNull();
        }
    }
}