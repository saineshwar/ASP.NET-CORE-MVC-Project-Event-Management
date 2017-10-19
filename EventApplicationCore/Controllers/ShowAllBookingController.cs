using EventApplicationCore.Filters;
using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;



namespace EventApplicationCore.Controllers
{
    [ValidateUserSession]
    public class ShowAllBookingController : Controller
    {
        private IBookingVenue _IBookingVenue;
        private ITotalbilling _ITotalbilling;
        public ShowAllBookingController(IBookingVenue IBookingVenue, ITotalbilling ITotalbilling)
        {
            _IBookingVenue = IBookingVenue;
            _ITotalbilling = ITotalbilling;
        }

        [HttpGet]
        public IActionResult AllBookings()
        {
            return View();
        }

        public JsonResult LoadAllBookings()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Session.GetString("UserID"))))
                {

                    var v = _IBookingVenue.ShowAllBookingUser(sortColumn, sortColumnDir, searchValue, Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                    recordsTotal = v.Count();
                    var data = v.Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
                else
                {
                    return Json("Failed");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult CancelBooking(int BookingID)
        {
            try
            {
                var result = _IBookingVenue.CancelBooking(BookingID);

                if (result > 0)
                {
                    return Json(data: true);
                }

                return Json(data: false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult ShowOrder(string BookingNo)
        {
            try
            {
                CompleteBookingModel completebookingmodel = _ITotalbilling.ShowCompleteBookingDetailsbyBookingNo(BookingNo);

                return View(completebookingmodel);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
