using Core.Models;
using Core.ViewModels;
using Logic.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using NPoco;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.ModelsBuilder;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Website.Controllers;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace BookEvent.Controllers
{
    public class EventController : SurfaceController
    {
        private readonly IEventService _eventService;
        private readonly IPublishedValueFallback publishedValueFallback;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IContentService _contentService;
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        public EventController(IEventService eventService,
           IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            IPublishedValueFallback publishedValueFallback,
            UmbracoHelper umbracoHelper,
            IContentService contentService)
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _eventService = eventService;
            this.publishedValueFallback = publishedValueFallback;
            _umbracoHelper = umbracoHelper;
            _contentService = contentService;
            _umbracoContextAccessor = umbracoContextAccessor;
        }

        [HttpGet]
        public IActionResult Book(int id)
        {
            var isFullyBooked = false;
            var eventNode = UmbracoContext.Content?.GetById(id);
            if (eventNode == null || eventNode.ContentType.Alias != "event")
            {
                return NotFound();
            }
            //Get the capacity of the event selected by the user
            var eventCapacity = eventNode.Value<int>("capacity");
            var bookedEventCount = _eventService.GetBookedEventCount(id).Result;
            if (eventCapacity <= bookedEventCount)
            {
                isFullyBooked = true;
                
            }

            var modelv = new BookEventViewModel
            {
                EventId = id,
                Event = eventNode.Name,
                IsFullCapacity = isFullyBooked

            };
           
            return View("~/Views/BookEvent.cshtml", modelv);
        }

        [HttpPost]
        public async Task<IActionResult> BookEvent(BookEventViewModel data)
        {
            if (!ModelState.IsValid ) {
                TempData["Error"] = "Please provide valid input data.";
                return PartialView("_booking",data);
            }
            if (string.IsNullOrEmpty(data.FirstName))
            {
                TempData["Error"] = "First Name is required.";
                return PartialView("_booking", data);
            }
            if (string.IsNullOrEmpty(data.PhoneNumber))
            {
                TempData["Error"] = "Phone number field is required.";
                return PartialView("_booking", data);
            }
            if (data.EventId <= 0)
            {
                TempData["Error"] = "Invalid event selected.";
                return PartialView("_booking", data);
            }
            if (string.IsNullOrEmpty(data.Email))
            {
                TempData["Error"] = "Email field is required.";
                return PartialView("_booking", data);
            }
            //Check if the user already booked for this event using this email
            var emailAlreadyExists = await _eventService.CheckForExistingBooking(data.EventId, data.Email);

            if (emailAlreadyExists)
            {
                TempData["Error"] = "You already booked for this event.";
                return PartialView("_booking", data);
            }
            var result = await _eventService.SaveEventBooking(data);

            if (!result)
            {
                TempData["Error"] = "Error occurred while booking for this event.";
                return PartialView("_booking", data);

            }
            ModelState.Clear();
            TempData["Success"] = $"Successfully booked for this event {data.Event}.";
            return View("~/Views/BookEvent.cshtml",new BookEventViewModel());
        }


    }
}
