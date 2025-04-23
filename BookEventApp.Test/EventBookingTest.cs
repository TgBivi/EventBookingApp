using Core.ViewModels;
using Logic.Interfaces;
using Moq;
using Xunit;

namespace BookEventApp.Test
{
    public class EventServiceTests
    {
        private readonly Mock<IEventService> _mockEventService;
        private readonly IEventService _bookingService;

        public EventServiceTests()
        {
            _mockEventService = new Mock<IEventService>();
            _bookingService = _mockEventService.Object;
        }

        [Fact]
        public async Task CanBookEvent_UserNotAlreadyBooked_CapacityNotFull()
        {
            // Arrange
            int eventId = 1;
            string userEmail = "test@examp78e.com";
            int eventCapacity = 10;
            int bookedCount = 5;

            _mockEventService.Setup(service => service.GetBookedEventCount(eventId)).ReturnsAsync(bookedCount);
            //.Returns<int>( x => Task.FromResult(x < eventCapacity));
            _mockEventService.Setup(service => service.CheckForExistingBooking(eventId, userEmail))
            .ReturnsAsync(false);
            _mockEventService.Setup(service => service.SaveEventBooking(It.IsAny<BookEventViewModel>()))
            .Returns<BookEventViewModel>(model => Task.FromResult(
            _mockEventService.Object.CheckForExistingBooking(model.EventId, model.Email).Result == false &&
            _mockEventService.Object.GetBookedEventCount(model.EventId).Result < eventCapacity
            ));

            var bookingViewModel = new BookEventViewModel
            {
                EventId = eventId,
                FirstName = "Test",
                SurName = "User",
                Email = userEmail,
                PhoneNumber = "1234567890"
            };
            // Act
            var bookingResult = await _bookingService.SaveEventBooking(bookingViewModel);

            // Assert
            Assert.True(bookingResult);
            _mockEventService.Verify(service => service.GetBookedEventCount(eventId), Times.Once());
            _mockEventService.Verify(service => service.CheckForExistingBooking(eventId, userEmail), Times.Once());
            _mockEventService.Verify(service => service.SaveEventBooking(It.IsAny<BookEventViewModel>()), Times.Once());
        }

        [Fact]
        public async Task CannotBookEvent_UserAlreadyBooked()
        {
            // Arrange
            int eventId = 1;
            string userEmail = "test@example.com";

            _mockEventService.Setup(service => service.CheckForExistingBooking(eventId, userEmail))
                            .ReturnsAsync(true);
            _mockEventService.Setup(service => service.SaveEventBooking(It.IsAny<BookEventViewModel>()))
                            .Returns<BookEventViewModel>(model => Task.FromResult(!_mockEventService.Object.CheckForExistingBooking(model.EventId, model.Email).Result));

            var bookingViewModel = new BookEventViewModel
            {
                EventId = eventId,
                FirstName = "Test",
                SurName = "User",
                Email = userEmail,
                PhoneNumber = "1234567890"
            };

            // Act
            var bookingResult = await _bookingService.SaveEventBooking(bookingViewModel);

            // Assert
            Assert.False(bookingResult);
            _mockEventService.Verify(service => service.CheckForExistingBooking(eventId, userEmail), Times.Once());
            _mockEventService.Verify(service => service.SaveEventBooking(It.IsAny<BookEventViewModel>()), Times.Once());
        }
    }
}