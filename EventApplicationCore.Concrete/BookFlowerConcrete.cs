using EventApplicationCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventApplicationCore.Model;

namespace EventApplicationCore.Concrete
{
    public class BookFlowerConcrete : IBookFlower
    {
        private DatabaseContext _context;

        public BookFlowerConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public int BookFlower(BookingFlower bookingflower)
        {
            try
            {
                if (bookingflower != null)
                {
                    _context.BookingFlower.Add(bookingflower);
                    _context.SaveChanges();
                    return bookingflower.BookingFlowerID;
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
