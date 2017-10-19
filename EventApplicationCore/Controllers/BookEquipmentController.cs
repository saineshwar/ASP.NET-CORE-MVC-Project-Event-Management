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
    public class BookEquipmentController : Controller
    {
        private IEquipment _IEquipment;
        private IBookEquipment _IBookEquipment;
        public BookEquipmentController(IEquipment IEquipment , IBookEquipment IBookEquipment)
        {
            _IEquipment = IEquipment;
            _IBookEquipment = IBookEquipment;
        }


        [HttpGet]
        public IActionResult Equipment()
        {
            try
            {
                BookingEquipment objeq = new BookingEquipment();
                objeq.EquipmentList = _IEquipment.GetAllEquipment();
                SetSlider();
                return View(objeq);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Equipment(BookingEquipment BookingEquipment)
        {
            try
            {
                var result = 0;
                var validateChecked = 0;

                if (!ModelState.IsValid)
                {
                    return View("Equipment", BookingEquipment);
                }

                for (int i = 0; i < BookingEquipment.EquipmentList.Count(); i++)
                {
                    if (BookingEquipment.EquipmentList[i].EquipmentChecked == true)
                    {
                        validateChecked = 1;

                        BookingEquipment bk = new BookingEquipment()
                        {
                            EquipmentID = Convert.ToInt32(BookingEquipment.EquipmentList[i].EquipmentID),
                            BookingID = Convert.ToInt32(HttpContext.Session.GetInt32("BookingID")),
                            Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID")),
                            CreatedDate = DateTime.Now,
                            BookingEquipmentID = 0,

                        };
                        result = _IBookEquipment.BookEquipment(bk);
                    }
                }

                if (validateChecked == 0)
                {
                    ModelState.AddModelError("", "You have not choose any Equipment !");
                }

                SetSlider();

                if (result > 0)
                {
                    ViewData["BookingEquipmentMessage"] = "Equipment Booked Successfully";
                    return View("Success");
                }
                else
                {
                    return View("Equipment", BookingEquipment);
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
                var Images = _IEquipment.ShowAllEquipment();
                ViewBag.SliderEquipmentImages = Images;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
