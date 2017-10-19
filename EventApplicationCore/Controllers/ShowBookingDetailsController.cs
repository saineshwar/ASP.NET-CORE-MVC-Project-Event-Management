using EventApplicationCore.Filters;
using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


namespace EventApplicationCore.Controllers
{
    [ValidateAdminSession]
    public class ShowBookingDetailsController : Controller
    {
        private IBookingVenue _IBookingVenue;
        private IVenue _IVenue;
        private ITotalbilling _ITotalbilling;

        public ShowBookingDetailsController(IBookingVenue IBookingVenue, IVenue IVenue, ITotalbilling ITotalbilling)
        {
            _IBookingVenue = IBookingVenue;
            _IVenue = IVenue;
            _ITotalbilling = ITotalbilling;
        }


        [HttpGet]
        public IActionResult AllBookings()
        {
            return View();
        }


        public JsonResult LoadAllBookings()
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
                var result = _IBookingVenue.ShowBookingDetail();

                if (result.Count() > 0)
                {
                    var v = _IBookingVenue.ShowAllBooking(sortColumn, sortColumnDir, searchValue);
                    recordsTotal = v.Count();
                    var data = v.Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
                else
                {
                    return Json("Failed");
                }
            }
            else
            {
                return Json("Failed");
            }
        }

   
        [HttpGet]
        public IActionResult BookingApproval(string BookingNo)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookingApproval(BillingModel BillingModel, string submit)
        {
            string value = string.Empty;

            if (string.IsNullOrEmpty(BillingModel.BookingNo))
            {
                return RedirectToAction("AllBookings", "ShowBookingDetails");
            }

            if (submit == "Approve")
            {
                value = "A";
            }
            else if (submit == "Reject")
            {
                value = "R";
            }
            else
            {
                value = "P";
            }


            var result = _IBookingVenue.UpdateBookingStatus(BillingModel.BookingNo, value);

            if (result > 0)
            {
                if (submit == "Approve")
                {
                    TempData["StatusMessage"] = BillingModel.BookingNo + "has been Approved";
                }
                else if (submit == "Reject")
                {
                    TempData["StatusMessage"] = BillingModel.BookingNo + "has been Cancelled";
                }
                var detailsBooking = _ITotalbilling.GetBillingDetailsbyBookingNo(BillingModel.BookingNo);
                return View(detailsBooking);

            }
            else
            {

                var detailsBooking = _ITotalbilling.GetBillingDetailsbyBookingNo(BillingModel.BookingNo);
                return View(detailsBooking);
            }


        }
    }
}
