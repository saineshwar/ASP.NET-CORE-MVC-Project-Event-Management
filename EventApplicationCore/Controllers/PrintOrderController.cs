using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventApplicationCore.Interface;
using EventApplicationCore.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventApplicationCore.Controllers
{
    [ValidateUserSession]
    public class PrintOrderController : Controller
    {
        private ITotalbilling _ITotalbilling;
        public PrintOrderController(ITotalbilling ITotalbilling)
        {
            _ITotalbilling = ITotalbilling;
        }

        [HttpGet]
        public IActionResult Print(string BookingNo)
        {
            try
            {
                if (string.IsNullOrEmpty(BookingNo))
                {
                    return RedirectToAction("AllBookings", "ShowBookingDetails");
                }

                var details = _ITotalbilling.GetBillingDetailsbyBookingNo(BookingNo);

                return View(details);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
