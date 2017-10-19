using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventApplicationCore.Interface;
using EventApplicationCore.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventApplicationCore.Controllers
{
    [Route("api/[controller]")]
    public class CityAPIController : Controller
    {

        private ICity _ICity;

        public CityAPIController(ICity ICity)
        {
            _ICity = ICity;
        }

        // POST api/values
        [HttpPost]
        public List<City> Post(string id)
        {
            try
            {
                var listofState = _ICity.ListofCity(Convert.ToInt32(id));
                return listofState;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
