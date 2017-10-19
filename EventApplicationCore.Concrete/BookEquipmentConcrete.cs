using EventApplicationCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventApplicationCore.Model;

namespace EventApplicationCore.Concrete
{
    public class BookEquipmentConcrete : IBookEquipment
    {
        private DatabaseContext _context;

        public BookEquipmentConcrete(DatabaseContext context)
        {
            _context = context;
        }
        public int BookEquipment(BookingEquipment BookingEquipment)
        {
            try
            {
                if (BookingEquipment != null)
                {
                    _context.BookingEquipment.Add(BookingEquipment);
                    _context.SaveChanges();
                    return BookingEquipment.BookingID;
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
