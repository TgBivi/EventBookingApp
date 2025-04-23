using Core.Context;
using Core.Models;
using Core.ViewModels;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Logic.Services
{
    public class EventService : IEventService
    {
        private readonly BookEventDbContext _dbContext;

        public EventService(BookEventDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> SaveEventBooking(BookEventViewModel data)
        {
            try
            {
                if (data == null) return false;

                var booked = new BookedEvents
                {
                    SurName = data.SurName,
                    Email = data.Email,
                    EventId = data.EventId,
                    FirstName = data.FirstName,
                    PhoneNumber = data.PhoneNumber,
                    BookingDate = DateTime.Now,
                };

                await _dbContext.AddAsync(booked);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<int> GetBookedEventCount(int eventId)
        {
            try
            {
                if (eventId <= 0) return 0;

                var result = _dbContext.BookedEvents.Where(x => x.EventId == eventId).ToList();

                return result.Count;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> IsEventCapacityFull(int eventId, int capacity)
        {
            try
            {
                if (eventId <= 0 || capacity <= 0) return false;

                var eventsBooked = _dbContext.BookedEvents.Where(x => x.EventId == eventId).Count();

                if (capacity == eventsBooked)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<bool> CheckForExistingBooking(int eventId, string email)
        {
            try
            {
                if (eventId <= 0 || string.IsNullOrEmpty(email)) return true;

                var eventsBooked = await _dbContext.BookedEvents.FirstOrDefaultAsync(x => x.EventId == eventId && x.Email == email);

                
                return eventsBooked == null ?false:true;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
