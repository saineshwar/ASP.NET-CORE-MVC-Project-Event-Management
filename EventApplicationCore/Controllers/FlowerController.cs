using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using EventApplicationCore.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventApplicationCore.Controllers
{
    [ValidateAdminSession]
    public class FlowerController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private IFlower _IFlower;

        public FlowerController(IFlower IFlower, IHostingEnvironment IHostingEnvironment)
        {
            _environment = IHostingEnvironment;
            _IFlower = IFlower;
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View(new Flower());
        }

        /// <summary>
        /// Inserting Flower
        /// </summary>
        /// <param name="Flower"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Flower Flower)
        {
            var newFileName = string.Empty;

            if (HttpContext.Request.Form.Files != null)
            {
                var fileName = string.Empty;
                string PathDB = string.Empty;

                var files = HttpContext.Request.Form.Files;

                if (files == null)
                {
                    ModelState.AddModelError("", "Upload Flower Photo !");
                    return View();
                }

                if (!ModelState.IsValid)
                {
                    return View("Add");
                }

                var uploads = Path.Combine(_environment.WebRootPath, "FlowerImages");

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        var FileExtension = Path.GetExtension(fileName);
                        newFileName = myUniqueFileName + FileExtension;
                        fileName = Path.Combine(_environment.WebRootPath, "FlowerImages") + $@"\{newFileName}";
                        PathDB = "FlowerImages/" + newFileName;
                        using (FileStream fs = System.IO.File.Create(fileName))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }

                Flower objEqu = new Flower
                {
                    FlowerFilename = newFileName,
                    FlowerFilePath = PathDB,
                    FlowerID = 0,
                    FlowerName = Flower.FlowerName,
                    FlowerCost = Flower.FlowerCost,
                    Createdate = DateTime.Now,
                    Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                };

                _IFlower.SaveFlower(objEqu);

                TempData["FLowerMessage"] = "FLower Saved Successfully";
                ModelState.Clear();
                return View(new Flower());

            }
            return View(Flower);
        }

        /// <summary>
        /// Validating VenueName is duplicate or not
        /// </summary>
        /// <param name="venueName"></param>
        /// <returns></returns>
        public JsonResult CheckFlowerNameExists(string FlowerName)
        {
            try
            {
                var isFlowerNameExists = _IFlower.CheckFlowerAlready(FlowerName);
                if (isFlowerNameExists)
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
                    return RedirectToAction("ViewAllFlowers", "AllFlower");
                }

                Flower FlowerEdit = _IFlower.GetFlowerByID(Convert.ToInt32(id));

                return View("Edit", FlowerEdit);
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// Update Flower
        /// </summary>
        /// <param name="Flower"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Flower Flower)
        {
            var newFileName = string.Empty;
            string PathDB = string.Empty;

            if (!ModelState.IsValid)
            {
                return View("Flower");
            }

            if (HttpContext.Request.Form.Files[0].Length > 0)
            {
                var fileName = string.Empty;

                var files = HttpContext.Request.Form.Files;

                if (files == null)
                {
                    ModelState.AddModelError("", "Upload Flower Photo !");
                    return View();
                }

                var uploads = Path.Combine(_environment.WebRootPath, "FlowerImages");

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        var FileExtension = Path.GetExtension(fileName);
                        newFileName = myUniqueFileName + FileExtension;
                        fileName = Path.Combine(_environment.WebRootPath, "FlowerImages") + $@"\{newFileName}";
                        PathDB = "FlowerImages/" + newFileName;
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
                Flower objflower = new Flower
                {
                    FlowerFilename = newFileName,
                    FlowerFilePath = PathDB,
                    FlowerID = Flower.FlowerID,
                    FlowerName = Flower.FlowerName,
                    Createdate = DateTime.Now,
                    FlowerCost = Flower.FlowerCost,
                    Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                };

                _IFlower.UpdateFlower(objflower);

                TempData["FlowerUpdateMessage"] = "Venue Saved Successfully";
                ModelState.Clear();
                return View(new Flower());
            }
            else
            {

                Flower objflower = new Flower
                {
                    FlowerID = Flower.FlowerID,
                    FlowerName = Flower.FlowerName,
                    Createdate = DateTime.Now,
                    FlowerCost = Flower.FlowerCost,
                    Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                };

                _IFlower.UpdateFlower(objflower);

                TempData["FlowerUpdateMessage"] = "FLower Saved Successfully";
                ModelState.Clear();
                return View(new Flower());
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
                    return RedirectToAction("ViewAllFoods", "AllFood");
                }

                int result = _IFlower.DeleteFlower(Convert.ToInt32(id));

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
