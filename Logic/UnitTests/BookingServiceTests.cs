using Core.Context;
using Core.Models;
using Core.ViewModels;
using Logic.Interfaces;
using Logic.Services;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.UnitTests
{

    [TestFixture]
    public class BookingServiceTests
    {
        private Mock<IEventService> _mockEventService;
        private IEventService _bookingService; // Assuming you have a BookingService

        [SetUp]
        public void Setup()
        {
            _mockEventService = new Mock<IEventService>();
            // Initialize your BookingService here, potentially injecting the mock EventService
            // _bookingService = new EventService(_mockEventService.Object,new BookEventDbContext());
        }

        [Test]
        public async Task CanBookEvent_UserNotAlreadyBooked_CapacityNotFull()
        {
            // Arrange
            int eventId = 1;
            string userEmail = "test@example.com";
            int eventCapacity = 10;
            int bookedCount = 5;

            _mockEventService.Setup(service => service.GetBookedEventCount(eventId))
                                .ReturnsAsync(bookedCount);
            _mockEventService.Setup(service => service.CheckForExistingBooking(eventId, userEmail))
                                .ReturnsAsync(false);
            _mockEventService.Setup(service => service.GetBookedEventCount(eventId))
                                .ReturnsAsync(eventCapacity);
            _mockEventService.Setup(service => service.SaveEventBooking(It.IsAny<BookEventViewModel>()))
                                .ReturnsAsync(true); // Assume booking saves successfully

            // Act
            var bookingViewModel = new BookEventViewModel
            {
                EventId = eventId,
                FirstName = "Test",
                SurName = "User",
                Email = userEmail,
                PhoneNumber = "1234567890"
            };
            var bookingResult = await _bookingService.SaveEventBooking(bookingViewModel);

            // Assert
            Assert.That(bookingResult);
            _mockEventService.Verify(service => service.SaveEventBooking(It.IsAny<BookEventViewModel>()), Times.Once);
        }

        [Test]
        public async Task CannotBookEvent_UserAlreadyBooked()
        {
            // Arrange
            int eventId = 1;
            string userEmail = "test@example.com";

            _mockEventService.Setup(service => service.CheckForExistingBooking(eventId, userEmail))
                                .ReturnsAsync(true);

            // Act
            var bookingViewModel = new BookEventViewModel
            {
                EventId = eventId,
                FirstName = "Test",
                SurName = "User",
                Email = userEmail,
                PhoneNumber = "1234567890"
            };
            var bookingResult = await _bookingService.SaveEventBooking(bookingViewModel);

            // Assert
            Assert.That(bookingResult);
            
            _mockEventService.Verify(service => service.SaveEventBooking(It.IsAny<BookEventViewModel>()), Times.Never);
        }

    }
    
}
