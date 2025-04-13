using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class BookEventViewModel
    {
        
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Event { get; set; }
        public int EventId { get; set; }
        public bool IsFullCapacity { get; set; }
    }
}
