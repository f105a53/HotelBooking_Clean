using System;
using FluentAssertions;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using TechTalk.SpecFlow;

namespace HotelBooking.UnitTests.SpecFlow
{
    [Binding]
    public class CreateBookingSteps
    {
        private readonly IBookingManager bookingManager = new BookingManager(
            new FakeBookingRepository(DateTime.Today.AddDays(10), DateTime.Today.AddDays(20)),
            new FakeRoomRepository());

        private readonly Booking booking = new Booking();
        private bool result;

        [Given(@"The end date is (.*)")]
        public void GivenTheEndDateIs(int end)
        {
            booking.EndDate = DateTime.Today.AddDays(end);
        }

        [Given(@"The start date is (.*)")]
        public void GivenTheStartDateIs(int start)
        {
            booking.StartDate = DateTime.Today.AddDays(start);
        }

        [Then(@"the booking result should be failure")]
        public void ThenTheBookingResultShouldBeFailure()
        {
            result.Should().BeFalse();
        }

        [Then(@"the booking result should be success")]
        public void ThenTheBookingResultShouldBeSuccess()
        {
            result.Should().BeTrue();
        }

        [When(@"I press book")]
        public void WhenIPressBook()
        {
            result = bookingManager.CreateBooking(booking);
        }
    }
}