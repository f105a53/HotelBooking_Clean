using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace HotelBooking.IntegrationTests
{
    public class WebApiTests : IClassFixture<WebApplicationFactory<WebApi.Startup>>
    {
        private readonly WebApplicationFactory<WebApi.Startup> _factory;

        public WebApiTests(WebApplicationFactory<WebApi.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public void Startup()
        {
            _factory.Should().NotBeNull();
            _factory.CreateClient().Should().NotBeNull();
        }
    }
}
