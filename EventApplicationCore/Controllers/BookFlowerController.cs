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
    public class BookFlowerController : Controller
    {
        IBookFlower _IBookFlower;
        IFlower _IFlower;
        IBookingVenue _IBookingVenue;
        public BookFlowerController(IBookFlower IBookFlower, IFlower IFlower , IBookingVenue IBookingVenue)
        {
            _IBookFlower = IBookFlower;
            _IFlower = IFlower;
            _IBookingVenue = IBookingVenue;
        }


        [HttpGet]
        public IActionResult BookFlower()
        {
            try
            {
                BookingFlower bookingflower = new BookingFlower();
                bookingflower.FlowerList = _IFlower.GetAllFlower();
                SetSlider();
                return View(bookingflower);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookFlower(BookingFlower bookingflower)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("BookFood", bookingflower);
                }

                if (bookingflower != null && bookingflower.FlowerList != null)
                {
                    var result = 0;
                    var validateChecked = 0;

                    for (int i = 0; i < bookingflower.FlowerList.Count(); i++)
                    {
                        if (bookingflower.FlowerList[i].FlowerChecked)
                        {
                            validateChecked = 1;

                            BookingFlower objbookingflower = new BookingFlower()
                            {
                                FlowerID = Convert.ToInt32(bookingflower.FlowerList[i].FlowerID),
                                BookingID = Convert.ToInt32(HttpContext.Session.GetInt32("BookingID")),
                                Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID")),
                                CreatedDate = DateTime.Now
                            };

                            result = _IBookFlower.BookFlower(objbookingflower);
                            if (result > 0)
                            {
                                _IBookingVenue.UpdateBookingStatus(Convert.ToInt32(HttpContext.Session.GetInt32("BookingID")));
                            }
                        }
                    }

                    if (validateChecked == 0)
                    {
                        ModelState.AddModelError("", "You have not choose any Flower !");
                    }

                    SetSlider();

                    if (result > 0)
                    {
                        ModelState.Clear();
                        ViewData["BookingFlowerMessage"] = "Flower Booked Successfully";
                        return View("Success");
                    }
                    else
                    {
                        return View("BookFlower", bookingflower);
                    }
                }
                else
                {
                    return View("BookFlower", bookingflower);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [NonAction]
        private void SetSlider()
        {
            try
            {
                var Images = _IFlower.ShowAllFlower();
                ViewBag.SliderFlowerImages = Images;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
