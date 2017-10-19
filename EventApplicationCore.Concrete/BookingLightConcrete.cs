using EventApplicationCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventApplicationCore.Model;

namespace EventApplicationCore.Concrete
{
    public class BookingLightConcrete : IBookingLight
    {
        private DatabaseContext _context;

        public BookingLightConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public int BookingLight(BookingLight bookinglight)
        {
            try
            {
                if (bookinglight != null)
                {
                    _context.BookingLight.Add(bookinglight);
                    _context.SaveChanges();
                    return bookinglight.BookLightID;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
