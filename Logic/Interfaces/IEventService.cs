using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IEventService
    {
        Task<bool> SaveEventBooking(BookEventViewModel data);
        Task<int> GetBookedEventCount(int eventId);
        Task<bool> IsEventCapacityFull(int eventId, int capacity);
        Task<bool> CheckForExistingBooking(int eventId, string email);
    }
}
