using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class BookedEvents
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string? SurName { get; set; }
        [Required]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        //[ForeignKey(nameof(Event))]
        public int EventId { get; set; }
        public DateTime BookingDate { get; set; } 
    }
}
