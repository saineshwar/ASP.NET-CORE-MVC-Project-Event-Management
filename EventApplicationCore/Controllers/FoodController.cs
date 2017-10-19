using EventApplicationCore.Filters;
using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;


namespace EventApplicationCore.Controllers
{
    [ValidateAdminSession]
    public class FoodController : Controller
    {
        IFood _IFood;
        IDishtypes _IDishtypes;

        private readonly IHostingEnvironment _environment;
        public FoodController(IFood IFood, IDishtypes IDishtypes, IHostingEnvironment hostingEnvironment)
        {
            _environment = hostingEnvironment;
            _IFood = IFood;
            _IDishtypes = IDishtypes;
        }

        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                Food food = new Food();
                food.FoodType = "1";
                food.MealType = "1";
                return View(food);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Food Food)
        {
            var newFileName = string.Empty;

            if (HttpContext.Request.Form.Files != null)
            {
                var fileName = string.Empty;
                string PathDB = string.Empty;

                var files = HttpContext.Request.Form.Files;

                if (files == null)
                {
                    ModelState.AddModelError("", "Upload Food Photo !");
                    return View();
                }

                if (!ModelState.IsValid)
                {
                    return View("Add");
                }

                var uploads = Path.Combine(_environment.WebRootPath, "FoodImages");

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        var FileExtension = Path.GetExtension(fileName);
                        newFileName = myUniqueFileName + FileExtension;
                        fileName = Path.Combine(_environment.WebRootPath, "FoodImages") + $@"\{newFileName}";
                        PathDB = "FoodImages/" + newFileName;
                        using (FileStream fs = System.IO.File.Create(fileName))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }

                Food objfood = new Food
                {
                    FoodFilename = newFileName,
                    FoodFilePath = PathDB,
                    FoodID = 0,
                    FoodName = Food.FoodName,
                    FoodCost = Food.FoodCost,
                    FoodType = Food.FoodType,
                    MealType = Food.MealType,
                    DishType = Food.DishType,
                    Createdate = DateTime.Now,
                    Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                };

                _IFood.SaveFood(objfood);

                TempData["FoodMessage"] = "Food Saved Successfully";
                ModelState.Clear();
                return View(new Food());

            }
            return View(Food);
        }

        public JsonResult BindDishtype()
        {
            try
            {
                var liDishtype = _IDishtypes.GetDishtypeList();
                return Json(data: liDishtype);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Validating VenueName is duplicate or not
        /// </summary>
        /// <param name="venueName"></param>
        /// <returns></returns>
        public JsonResult CheckDishNameExists(string DishName, string FoodType)
        {
            try
            {
                var isDishNameExists = _IFood.CheckDishNameAlready(DishName, FoodType);
                if (isDishNameExists)
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
                    return RedirectToAction("ViewAllFoods", "AllFood");
                }

                Food FoodEdit = _IFood.FoodByID(Convert.ToInt32(id));

                return View("Edit", FoodEdit);
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
        public IActionResult Edit(Food Food)
        {
            var newFileName = string.Empty;
            string PathDB = string.Empty;

            if (!ModelState.IsValid)
            {
                return View("Food");
            }

            if (HttpContext.Request.Form.Files[0].Length > 0)
            {
                var fileName = string.Empty;

                var files = HttpContext.Request.Form.Files;

                if (files == null)
                {
                    ModelState.AddModelError("", "Upload Dish Photo !");
                    return View();
                }

                var uploads = Path.Combine(_environment.WebRootPath, "FoodImages");

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        var FileExtension = Path.GetExtension(fileName);
                        newFileName = myUniqueFileName + FileExtension;
                        fileName = Path.Combine(_environment.WebRootPath, "FoodImages") + $@"\{newFileName}";
                        PathDB = "FoodImages/" + newFileName;
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
                Food objfood = new Food
                {
                    FoodFilename = newFileName,
                    FoodFilePath = PathDB,
                    FoodID = Food.FoodID,
                    FoodName = Food.FoodName,
                    FoodCost = Food.FoodCost,
                    FoodType = Food.FoodType,
                    MealType = Food.MealType,
                    DishType = Food.DishType,
                    Createdate = DateTime.Now,
                    Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                };

                _IFood.UpdateFood(objfood);

                TempData["VenueUpdateMessage"] = "Venue Saved Successfully";
                ModelState.Clear();
                return View(new Equipment());
            }
            else
            {

                Food objfood = new Food
                {
                    FoodID = Food.FoodID,
                    FoodName = Food.FoodName,
                    FoodCost = Food.FoodCost,
                    FoodType = Food.FoodType,
                    MealType = Food.MealType,
                    DishType = Food.DishType,
                    Createdate = DateTime.Now,
                    Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                };

                _IFood.UpdateFood(objfood);

                TempData["FoodUpdateMessage"] = "Food Item Saved Successfully";
                ModelState.Clear();
                return View(new Food());
            }

        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("ViewAllFoods", "AllFlower");
                }

                int result = _IFood.DeleteFood(Convert.ToInt32(id));

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
