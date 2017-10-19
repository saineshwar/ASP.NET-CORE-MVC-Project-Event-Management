using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApplicationCore.Model
{
    public class CompleteBookingModel
    {
        public Venue BookingVenue { get; set; }
        public List<Equipment> BookingEquipment { get; set; }
        public List<Food> BookingFood { get; set; }
        public List<Flower> BookingFlower { get; set; }
        public List<Light> BookingLight { get; set; }
        public BillingModel BillingModel { get; set; }
}
}
