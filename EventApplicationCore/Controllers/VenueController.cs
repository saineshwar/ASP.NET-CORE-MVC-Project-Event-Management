using EventApplicationCore.Filters;
using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;

namespace EventApplicationCore.Controllers
{
    [ValidateAdminSession]
    public class VenueController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private IVenue _IVenue;

        public VenueController(IVenue IVenue, IHostingEnvironment hostingEnvironment)
        {
            _IVenue = IVenue;
            _environment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new Venue());
        }

        /// <summary>
        /// Inserting Venue
        /// </summary>
        /// <param name="Venue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Venue Venue)
        {
            var newFileName = string.Empty;

            if (HttpContext.Request.Form.Files != null)
            {
                var fileName = string.Empty;
                string PathDB = string.Empty;

                var files = HttpContext.Request.Form.Files;

                if (files == null)
                {
                    ModelState.AddModelError("", "Upload Venue Photo !");
                    return View();
                }

                if (!ModelState.IsValid)
                {
                    return View("Venue");
                }

                var uploads = Path.Combine(_environment.WebRootPath, "VenueImages");

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        var FileExtension = Path.GetExtension(fileName);
                        newFileName = myUniqueFileName + FileExtension;
                        fileName = Path.Combine(_environment.WebRootPath, "VenueImages") + $@"\{newFileName}";
                        PathDB = "VenueImages/" + newFileName;
                        using (FileStream fs = System.IO.File.Create(fileName))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }

                Venue objvenue = new Venue
                {
                    VenueFilename = newFileName,
                    VenueFilePath = PathDB,
                    VenueID = 0,
                    VenueName = Venue.VenueName,
                    VenueCost = Venue.VenueCost,
                    Createdate = DateTime.Now,
                    Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                };

                _IVenue.SaveVenue(objvenue);

                TempData["VenueMessage"] = "Venue Saved Successfully";
                ModelState.Clear();
                return View(new Venue());

            }
            return View(Venue);
        }

        /// <summary>
        /// Validating VenueName is duplicate or not
        /// </summary>
        /// <param name="venueName"></param>
        /// <returns></returns>
        public JsonResult CheckVenueNameExists(string venueName)
        {
            try
            {
                var isVenueNameExists = _IVenue.CheckVenueNameAlready(venueName);
                if (isVenueNameExists)
                {
                    return Json(data: true);
                }
                else
                {
                    return Json(data: false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Venue Details to Update
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("ViewAllVenues", "AllVenue");
                }

                Venue venueEdit = _IVenue.VenueByID(Convert.ToInt32(id));

                return View("Edit", venueEdit);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update Venue
        /// </summary>
        /// <param name="Venue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Venue Venue)
        {
            var newFileName = string.Empty;
            string PathDB = string.Empty;

            if (!ModelState.IsValid)
            {
                return View("Venue");
            }

            if (HttpContext.Request.Form.Files[0].Length > 0)
            {
                var fileName = string.Empty;

                var files = HttpContext.Request.Form.Files;

                if (files == null)
                {
                    ModelState.AddModelError("", "Upload Venue Photo !");
                    return View();
                }

                var uploads = Path.Combine(_environment.WebRootPath, "VenueImages");

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        var FileExtension = Path.GetExtension(fileName);
                        newFileName = myUniqueFileName + FileExtension;
                        fileName = Path.Combine(_environment.WebRootPath, "VenueImages") + $@"\{newFileName}";
                        PathDB = "VenueImages/" + newFileName;
                        using (FileStream fs = System.IO.File.Create(fileName))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }

            }

            if (!string.IsNullOrEmpty(PathDB))
            {
                Venue objvenue = new Venue
                {
                    VenueFilename = newFileName,
                    VenueFilePath = PathDB,
                    VenueID = Venue.VenueID,
                    VenueName = Venue.VenueName,
                    VenueCost = Venue.VenueCost,
                    Createdate = DateTime.Now,
                    Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                };

                _IVenue.UpdateVenue(objvenue);

                TempData["VenueUpdateMessage"] = "Venue Saved Successfully";
                ModelState.Clear();
                return View(new Venue());
            }
            else
            {
                Venue objvenue = new Venue
                {
                    VenueID = Venue.VenueID,
                    VenueName = Venue.VenueName,
                    VenueCost = Venue.VenueCost,
                    Createdate = DateTime.Now,
                    Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                };

                _IVenue.UpdateVenue(objvenue);

                TempData["VenueUpdateMessage"] = "Venue Saved Successfully";
                ModelState.Clear();
                return View(new Venue());
            }

        }

        /// <summary>
        /// Delete Venue
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("ViewAllVenues", "AllVenue");
                }

                int result = _IVenue.DeleteVenue(Convert.ToInt32(id));

                if (result > 0)
                {
                    return Json(data: true);
                }
                else
                {
                    return Json(data: false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
