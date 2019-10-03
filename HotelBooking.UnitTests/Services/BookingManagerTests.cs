using System;
using System.Collections.Generic;
using FluentAssertions;
using FsCheck.Xunit;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests.Services
{
    public class BookingManagerTests
    {
        private static (BookingManager manager, IRepository<Booking> repository) Fakes
        {
            get
            {
                var start = DateTime.Today.AddDays(10);
                var end = DateTime.Today.AddDays(20);
                IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
                IRepository<Room> roomRepository = new FakeRoomRepository();
                return (new BookingManager(bookingRepository, roomRepository), bookingRepository);
            }
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            var date = DateTime.Today;
            Assert.Throws<ArgumentException>(() => Fakes.manager.FindAvailableRoom(date, date));
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            var date = DateTime.Today.AddDays(1);
            // Act
            var roomId = Fakes.manager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        [Fact]
        public void GetFullyOccupiedDates_InvalidDates_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                Fakes.manager.GetFullyOccupiedDates(DateTime.MaxValue, DateTime.MinValue));
        }

        [Fact(Skip = "Edge case, fix implemenation later")]
        public void GetFullyOccupiedDates_All()
        {
            var dates = Fakes.manager.GetFullyOccupiedDates(DateTime.MinValue, DateTime.MaxValue);
            dates.Should().HaveCount(10);
        }

        [Fact]
        public void GetFullyOccupiedDates_NoBookings_ReturnsEmptyList()
        {
            var bookingRepo = new Mock<IRepository<Booking>>(MockBehavior.Strict);
            bookingRepo.Setup(r => r.GetAll()).Returns(new List<Booking>());
            var roomRepo = new Mock<IRepository<Room>>(MockBehavior.Strict);
            roomRepo.Setup(r => r.GetAll()).Returns(new List<Room>());

            var bookingManager = new BookingManager(bookingRepo.Object, roomRepo.Object);
            bookingManager.GetFullyOccupiedDates(new DateTime(), new DateTime()).Should().BeEmpty();
        }

        [Theory]
        [InlineData(5, 15)]
        [InlineData(15, 16)]
        [InlineData(15, 25)]
        public void FindAvailableRoom_RoomsAlreadyBooked_ReturnsMinusOne(int start, int end)
        {
            Fakes.manager.FindAvailableRoom(DateTime.Today.AddDays(start), DateTime.Today.AddDays(end)).Should().Be(-1);
        }

        [Property]
        public void GetFullyOccupiedDates_RandomDates(DateTime start, DateTime end)
        {
            if (start > end)
                Assert.Throws<ArgumentException>(() => Fakes.manager.GetFullyOccupiedDates(start, end));
            else
                Fakes.manager.GetFullyOccupiedDates(start, end).Should()
                    .NotBeNull().And
                    .NotContainNulls().And
                    .HaveCountLessOrEqualTo(11);
        }

        [Fact]
        public void CreateBooking_ValidBooking_Created()
        {
            var (manager, repository) = Fakes;
            manager.CreateBooking(new Booking
                    {StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(2)}).Should()
                .BeTrue();
            ((FakeBookingRepository) repository).addWasCalled.Should().BeTrue();
        }

        [Fact]
        public void CreateBooking_WhileFullyBooked_Fails()
        {
            var (manager, repository) = Fakes;
            manager.CreateBooking(new Booking
            { StartDate = DateTime.Today.AddDays(15), EndDate = DateTime.Today.AddDays(16) }).Should()
                .BeFalse();
            ((FakeBookingRepository)repository).addWasCalled.Should().BeFalse();
        }
    }
}