using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
    public class BookingVenue
    {
        [Key]
        public int BookingVenueID { get; set; }

        [Required(ErrorMessage = "Select Venue")]
        [Display(Description = "Venue Type")]
        public int? VenueID { get; set; }

        [Required(ErrorMessage = "Select Event")]
        [Display(Description = " Event Type")]
        public int? EventTypeID { get; set; }

        [Required(ErrorMessage = "Required Guest Count")]
        [Display(Description = "No .Of Guest")]
        public string GuestCount { get; set; }

        public int? Createdby { get; set; }

        public DateTime? CreatedDate { get; set; }

        [NotMapped]
        public DateTime? BookingDate { get; set; }
        
        public int? BookingID { get; set; }
    }
}
