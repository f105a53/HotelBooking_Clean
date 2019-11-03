using System;
using System.Collections.Generic;
using FluentAssertions;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using TechTalk.SpecFlow;

namespace HotelBooking.UnitTests.SpecFlow
{
    [Binding]
    public class FullyOccupiedDatesSteps
    {
        private (DateTime start, DateTime end) booking;

        private List<DateTime> result;

        [Given(@"A booking between (.*) and (.*) exists")]
        public void GivenABookingBetweenAndExists(int start, int end)
        {
            booking = (DateTime.Today.AddDays(start), DateTime.Today.AddDays(end));
        }

        [When(@"I get fully occupied dates")]
        public void WhenIGetFullyOccupiedDates()
        {
            var bookingManager = new BookingManager(
                new FakeBookingRepository(booking.start, booking.end),
                new FakeRoomRepository());
            result = bookingManager.GetFullyOccupiedDates(DateTime.MinValue.AddDays(1), DateTime.MaxValue.AddDays(-1));
        }

        [Then(@"the result should have (.*) fully occupied days")]
        public void ThenTheResultShouldHaveFullyOccupiedDays(int amount)
        {
            result.Count.Should().Be(amount);
        }
    }
}