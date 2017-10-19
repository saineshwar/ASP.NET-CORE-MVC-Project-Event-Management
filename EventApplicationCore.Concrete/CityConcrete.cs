using EventApplicationCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventApplicationCore.Model;

namespace EventApplicationCore.Concrete
{
    public class CityConcrete : ICity
    {
        private DatabaseContext _context;

        public CityConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public List<City> ListofCity(int? ID)
        {
            if (ID == null)
            {
                return new List<City>();
            }

            var listofcities = (from cities in _context.City
                               where cities.StateID == ID
                               select cities).ToList();

            listofcities.Insert(0, new City { CityID = 0, CityName = "----Select----" });

            return listofcities;
        }
    }
}
