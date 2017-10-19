using EventApplicationCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventApplicationCore.Model;

namespace EventApplicationCore.Concrete
{
    public class TotalbillingConcrete : ITotalbilling
    {
        private DatabaseContext _context;

        public TotalbillingConcrete(DatabaseContext context)
        {
            _context = context;
        }


        public BillingModel GetBillingDetailsbyBookingNo(string BookingNo)
        {
            if (BookingNo != null)
            {

                var BookingDT = (from BD in _context.BookingDetails
                                 where BD.BookingNo == BookingNo
                                 select new { BD.BookingNo, BD.BookingDate, BD.BookingID }).Single();


                int? TotalVenueCost = (from BD in _context.BookingVenue
                                       join Vn in _context.Venue on BD.VenueID equals Vn.VenueID
                                       where BD.BookingID == BookingDT.BookingID
                                       select Vn.VenueCost).Sum();

                int? TotalEquipmentCost = (from BD in _context.BookingEquipment
                                           join Eq in _context.Equipment on BD.EquipmentID equals Eq.EquipmentID
                                           where BD.BookingID == BookingDT.BookingID
                                           select Eq.EquipmentCost).Sum();

                int? TotalFoodCost = (from BD in _context.BookingFood
                                      join Fo in _context.Food on BD.DishName equals Fo.FoodID
                                      where BD.BookingID == BookingDT.BookingID
                                      select Fo.FoodCost).Sum();

                int? TotalFlowerCost = (from BD in _context.BookingFlower
                                        join Fl in _context.Flower on BD.FlowerID equals Fl.FlowerID
                                        where BD.BookingID == BookingDT.BookingID
                                        select Fl.FlowerCost).Sum();

                int? TotalLightCost = (from BD in _context.BookingLight
                                       join Lg in _context.Light on BD.LightIDSelected equals Lg.LightID
                                       where BD.BookingID == BookingDT.BookingID
                                       select Lg.LightCost).Sum();


                int? TotalAmount = TotalVenueCost +
                                   TotalEquipmentCost +
                                   TotalFoodCost +
                                   TotalFlowerCost +
                                   TotalLightCost;

                DateTime? BDT = BookingDT.BookingDate;
                string BookingDate = BDT.Value.ToString("dd/MM/yyyy");

                BillingModel billingmodel = new BillingModel()
                {
                    BookingNo = BookingDT.BookingNo,
                    BookingDate = BookingDate,
                    BookingID = BookingDT.BookingID,
                    TotalVenueCost = TotalVenueCost,
                    TotalEquipmentCost = TotalEquipmentCost,
                    TotalFoodCost = TotalFoodCost,
                    TotalFlowerCost = TotalFlowerCost,
                    TotalLightCost = TotalLightCost,
                    TotalAmount = TotalAmount
                };

                return billingmodel;
            }

            return new BillingModel();

        }

        public CompleteBookingModel ShowCompleteBookingDetailsbyBookingNo(string BookingNo)
        {
            CompleteBookingModel objbooking = new CompleteBookingModel();

            var BookingDT = (from BD in _context.BookingDetails
                             where BD.BookingNo == BookingNo
                             select new { BD.BookingNo, BD.BookingDate, BD.BookingID }).Single();

            objbooking.BookingVenue = (from BD in _context.BookingVenue
                                       join Vn in _context.Venue on BD.VenueID equals Vn.VenueID
                                       where BD.BookingID == BookingDT.BookingID
                                       select new Venue { VenueName = Vn.VenueName, VenueCost = Vn.VenueCost }).SingleOrDefault();

            objbooking.BookingEquipment = (from BD in _context.BookingEquipment
                                           join Eq in _context.Equipment on BD.EquipmentID equals Eq.EquipmentID
                                           where BD.BookingID == BookingDT.BookingID
                                           select new Equipment { EquipmentName = Eq.EquipmentName, EquipmentCost = Eq.EquipmentCost }).ToList();

            objbooking.BookingFood = (from BD in _context.BookingFood
                                      join Fo in _context.Food on BD.DishName equals Fo.FoodID
                                      where BD.BookingID == BookingDT.BookingID
                                      select new Food
                                      {
                                          FoodName = Fo.FoodName,
                                          FoodCost = Fo.FoodCost,
                                          FoodTypeName = (Fo.FoodType == "1" ? "Veg" : "Non-Veg"),
                                          MealTypeName = (Fo.MealType == "1" ? "Lunch" : "Dinner"),
                                          DishTypeName = Fo.DishTypeName,

                                      }).ToList();

            objbooking.BookingFlower = (from BD in _context.BookingFlower
                                        join Fl in _context.Flower on BD.FlowerID equals Fl.FlowerID
                                        where BD.BookingID == BookingDT.BookingID
                                        select new Flower { FlowerName = Fl.FlowerName, FlowerCost = Fl.FlowerCost }).ToList();


            objbooking.BookingLight = (from BD in _context.BookingLight
                                       join Lg in _context.Light on BD.LightIDSelected equals Lg.LightID
                                       where BD.BookingID == BookingDT.BookingID
                                       select new Light { LightName = Lg.LightName ,LightCost = Lg.LightCost,

                                            LightTypeName = (Lg.LightType == "1" ? "In Door" : "Out Door"),
                                       }).ToList();


            int? TotalVenueCost = (from BD in _context.BookingVenue
                                   join Vn in _context.Venue on BD.VenueID equals Vn.VenueID
                                   where BD.BookingID == BookingDT.BookingID
                                   select Vn.VenueCost).Sum();

            int? TotalEquipmentCost = (from BD in _context.BookingEquipment
                                       join Eq in _context.Equipment on BD.EquipmentID equals Eq.EquipmentID
                                       where BD.BookingID == BookingDT.BookingID
                                       select Eq.EquipmentCost).Sum();

            int? TotalFoodCost = (from BD in _context.BookingFood
                                  join Fo in _context.Food on BD.DishName equals Fo.FoodID
                                  where BD.BookingID == BookingDT.BookingID
                                  select Fo.FoodCost).Sum();

            int? TotalFlowerCost = (from BD in _context.BookingFlower
                                    join Fl in _context.Flower on BD.FlowerID equals Fl.FlowerID
                                    where BD.BookingID == BookingDT.BookingID
                                    select Fl.FlowerCost).Sum();

            int? TotalLightCost = (from BD in _context.BookingLight
                                   join Lg in _context.Light on BD.LightIDSelected equals Lg.LightID
                                   where BD.BookingID == BookingDT.BookingID
                                   select Lg.LightCost).Sum();


            int? TotalAmount = TotalVenueCost +
                               TotalEquipmentCost +
                               TotalFoodCost +
                               TotalFlowerCost +
                               TotalLightCost;

            DateTime? BDT = BookingDT.BookingDate;
            string BookingDate = BDT.Value.ToString("dd/MM/yyyy");

            BillingModel billingmodel = new BillingModel()
            {
                BookingNo = BookingDT.BookingNo,
                BookingDate = BookingDate,
                BookingID = BookingDT.BookingID,
                TotalVenueCost = TotalVenueCost,
                TotalEquipmentCost = TotalEquipmentCost,
                TotalFoodCost = TotalFoodCost,
                TotalFlowerCost = TotalFlowerCost,
                TotalLightCost = TotalLightCost,
                TotalAmount = TotalAmount
            };

             objbooking.BillingModel = billingmodel;

            return objbooking;

        }

    }
}
