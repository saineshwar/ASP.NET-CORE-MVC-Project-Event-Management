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
    [ValidateUserSession]
    public class BookingController : Controller
    {
        private IBookingVenue _IBookingVenue;
        private IVenue _IVenue;
        public BookingController(IBookingVenue IBookingVenue , IVenue IVenue)
        {
            _IBookingVenue = IBookingVenue;
            _IVenue = IVenue;
        }

        [HttpGet]
        public IActionResult BookingVenue()
        {

            SetSlider();

            return View(new BookingVenue());
        }


        [HttpPost]
        public JsonResult CheckBooking(BookingVenue objBV)
        {
            bool result = _IBookingVenue.checkBookingAvailability(objBV);
            if (result == false)
            {
                return Json("NotAvailable");
            }
            else
            {
                return Json("Available");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookingVenue(BookingVenue BookingVenue)
        {
            if (ModelState.IsValid)
            {

                BookingDetails BD = new BookingDetails
                {
                    BookingDate = BookingVenue.BookingDate,
                    Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID")),
                    CreatedDate = DateTime.Now,
                    BookingApproval = "P"
                };

                var result = _IBookingVenue.BookEvent(BD);

                BookingVenue BV = new BookingVenue
                {
                    VenueID = BookingVenue.VenueID,
                    EventTypeID = BookingVenue.EventTypeID,
                    GuestCount = BookingVenue.GuestCount,
                    Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID")),
                    CreatedDate = DateTime.Now,
                    BookingID = result
                };

                var VenueID = _IBookingVenue.BookVenue(BV);

                HttpContext.Session.SetInt32("BookingID", result);

                if (result > 0)
                {
                    SetSlider();
                    ModelState.Clear();
                    ViewData["BookingMessage"] = "Venue Booked Successfully";
                    return View("Success");
                }
                else
                {
                    SetSlider();
                    return View("BookingVenue", BookingVenue);
                }
            }
            else
            {
                SetSlider();
                return View("BookingVenue", BookingVenue);
            }
        }


        [HttpGet]
        public JsonResult ShowBookingDetails()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Session.GetString("UserID"))))
            {
                var result = _IBookingVenue.ShowBookingDetail(Convert.ToInt32(HttpContext.Session.GetString("UserID")));

                if (result.Count() > 0)
                {

                    List<BookingDetailTemp> resultnew = new List<BookingDetailTemp>();

                    foreach (var item in result)
                    {
                        BookingDetailTemp BT = new BookingDetailTemp();

                        DateTime? BDT = item.BookingDate;
                        string BookingDate = BDT.Value.ToString("dd/MM/yyyy");

                        DateTime? BDTA = item.BookingApprovalDate;
                        string BookingApprovalDate;

                        if (item.BookingApprovalDate == null)
                        {
                            BookingApprovalDate = "----------";
                        }
                        else
                        {
                            BookingApprovalDate = BDTA.Value.ToString("dd/MM/yyyy");
                        }
                        BT.BookingNo = item.BookingNo;
                        BT.BookingID = item.BookingID;
                        BT.BookingDate = BookingDate;

                        string BookingApproval;

                        if (item.BookingApproval == "P")
                        {
                            BookingApproval = "Pending";
                        }
                        else if (item.BookingApproval == "C")
                        {
                            BookingApproval = "Cancelled";
                        }
                        else
                        {
                            BookingApproval = "Approved";
                        }

                        BT.BookingApproval = BookingApproval;
                        BT.BookingApprovalDate = BookingApprovalDate;
                        resultnew.Add(BT);
                    }

                    return Json(resultnew);
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

      

       
        [NonAction]
        private void SetSlider()
        {
            var Images = (from images in _IVenue.ShowAllVenue()
                          select new Venue { VenueFilename = images.VenueFilename, VenueFilePath = images.VenueFilePath, VenueName = images.VenueName }).ToList();


            ViewBag.SliderImages = Images;
        }

    }
}
