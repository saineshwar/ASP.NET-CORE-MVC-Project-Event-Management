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
    public class CountryAPIController : Controller
    {
        private ICountry _ICountry;

        public CountryAPIController(ICountry ICountry)
        {
            _ICountry = ICountry;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Country> Get()
        {
            try
            {
                var listofCountry = _ICountry.ListofCountry();
                listofCountry.Insert(0, new Country { CountryID = 0, Name = "----Select----" });
                return listofCountry;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
