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
        //TestCase 1
        [InlineData(9, 9, true)]
        //TestCase 2
        [InlineData(9, 21, false)]
        //TestCase 3
        [InlineData(21, 21, true)]
        //TestCase 4
        [InlineData(9, 10, false)]
        //TestCase 5
        [InlineData(9, 20, false)]
        //TestCase 6
        [InlineData(10, 21, false)]
        //TestCase 7
        [InlineData(20, 21, false)]
        //TestCase 8
        [InlineData(10, 10, false)]
        //TestCase 9
        [InlineData(10, 20, false)]
        //TestCase 10
        [InlineData(20, 20, false)]
        [Theory]
        public void CreateBooking_DecisionTable(int start, int end, bool shouldSucceed)
        {
            var (manager, repository) = Fakes;
            manager.CreateBooking(new Booking
                    {StartDate = DateTime.Today.AddDays(start), EndDate = DateTime.Today.AddDays(end)}).Should()
                .Be(shouldSucceed);
            ((FakeBookingRepository) repository).addWasCalled.Should().Be(shouldSucceed);
        }

        [InlineData(15, 16)]
        [Theory]
        public void CreateBooking_WhileFullyBooked_Fails(int start, int end)
        {
            var (manager, repository) = Fakes;
            manager.CreateBooking(new Booking
                    {StartDate = DateTime.Today.AddDays(start), EndDate = DateTime.Today.AddDays(end)}).Should()
                .BeFalse();
            ((FakeBookingRepository) repository).addWasCalled.Should().BeFalse();
        }

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

        [Theory]
        [InlineData(1, 0, true, 0)]
        [InlineData(-1, 1, true, 0)]
        [InlineData(11, 12, false, -1)]
        [InlineData(5, 6, false, 1)]
        public void FindAvailableRoom_AllPaths(int start, int end, bool shouldThrow, int result)
        {
            if (shouldThrow)
                Assert.Throws<ArgumentException>(() =>
                {
                    Fakes.manager.FindAvailableRoom(DateTime.Today.AddDays(start), DateTime.Today.AddDays(end));
                });
            else
                Fakes.manager.FindAvailableRoom(DateTime.Today.AddDays(start), DateTime.Today.AddDays(end)).Should()
                    .Be(result);
        }

        [Theory]
        [InlineData(5, 15)]
        [InlineData(15, 16)]
        [InlineData(15, 25)]
        [InlineData(5, 25)]
        public void FindAvailableRoom_RoomsAlreadyBooked_ReturnsMinusOne(int start, int end)
        {
            Fakes.manager.FindAvailableRoom(DateTime.Today.AddDays(start), DateTime.Today.AddDays(end)).Should().Be(-1);
        }

        [Theory]
        [InlineData(1, 0, true, 0)]
        [InlineData(0, 1, false, 0)]
        [InlineData(11, 12, false, 2)]
        public void GetFullyOccupiedDates_AllPaths(int start, int end, bool shouldThrow, int resultCount)
        {
            if (shouldThrow)
                Assert.Throws<ArgumentException>(() =>
                {
                    Fakes.manager.GetFullyOccupiedDates(DateTime.Today.AddDays(start), DateTime.Today.AddDays(end));
                });
            else
                Fakes.manager.GetFullyOccupiedDates(DateTime.Today.AddDays(start), DateTime.Today.AddDays(end)).Should()
                    .HaveCount(resultCount);
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
        public void CreateBooking_InThePast_ThrowsArgumentException()
        {
            var (manager, repository) = Fakes;
            Assert.Throws<ArgumentException>(() => manager.CreateBooking(new Booking
                {StartDate = DateTime.Today.AddDays(-1), EndDate = DateTime.Today.AddDays(1)}));
            ((FakeBookingRepository) repository).addWasCalled.Should().BeFalse();
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
        public void FindAvailableRoom_InvalidDates_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                Fakes.manager.FindAvailableRoom(DateTime.MaxValue, DateTime.MinValue));
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
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            var date = DateTime.Today;
            Assert.Throws<ArgumentException>(() => Fakes.manager.FindAvailableRoom(date, date));
        }

        [Fact(Skip = "Edge case, fix implemenation later")]
        public void GetFullyOccupiedDates_All()
        {
            var dates = Fakes.manager.GetFullyOccupiedDates(DateTime.MinValue, DateTime.MaxValue);
            dates.Should().HaveCount(10);
        }

        [Fact]
        public void GetFullyOccupiedDates_InvalidDates_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                Fakes.manager.GetFullyOccupiedDates(DateTime.MaxValue, DateTime.MinValue));
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
    }
}