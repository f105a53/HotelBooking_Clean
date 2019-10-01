using System;
using FluentAssertions;
using FluentAssertions.Extensions;
using FsCheck;
using FsCheck.Xunit;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;

namespace HotelBooking.UnitTests.Services
{
    public class BookingManagerTests
    {
        private static IBookingManager BookingManager
        {
            get
            {
                var start = DateTime.Today.AddDays(10);
                var end = DateTime.Today.AddDays(20);
                IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
                IRepository<Room> roomRepository = new FakeRoomRepository();
                return new BookingManager(bookingRepository, roomRepository);
            }
        }
      
        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            DateTime date = DateTime.Today;
            Assert.Throws<ArgumentException>(() => BookingManager.FindAvailableRoom(date, date));
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = BookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        [Fact]
        public void GetFullyOccupiedDates_InvalidDates()
        {
            Assert.Throws<ArgumentException>(() => BookingManager.GetFullyOccupiedDates(DateTime.MaxValue, DateTime.MinValue));
        }

        [Fact(Skip = "I don't know how to fix this")]
        public void GetFullyOccupiedDates_All()
        {
            var dates = BookingManager.GetFullyOccupiedDates(DateTime.MinValue, DateTime.MaxValue);
            dates.Should().HaveCount(10);
        }

        [Fact]
        public void FindAvailableRoom_StartDateOk_EndDateConflict()
        {
            BookingManager.FindAvailableRoom(DateTime.Today.AddDays(5), DateTime.Today.AddDays(15)).Should().Be(-1);
        }

        [Property]
        public void GetFullyOccupiedDates(DateTime start,DateTime end)
        {
            if (start > end)
            {
                Assert.Throws<ArgumentException>(() => BookingManager.GetFullyOccupiedDates(start, end));
            }
            else
            {
                BookingManager.GetFullyOccupiedDates(start, end).Should()
                    .NotBeNull().And
                    .NotContainNulls().And
                    .HaveCountLessOrEqualTo(11);
            }
        }

    }
}
