using EventApplicationCore.Interface;
using EventApplicationCore.Library;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EventApplicationCore.Controllers
{
    public class RegistrationController : Controller
    {

        IRegistration _IRepository;
        IRoles _IRoles;
        public RegistrationController(IRegistration IRepository, IRoles IRoles)
        {
            _IRepository = IRepository;
            _IRoles = IRoles;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Registration()
        {
            try
            {
                Registration Registration = new Registration();
                Registration.Country = null;
                Registration.City = null;
                Registration.State = null;
                return View(Registration);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(Registration Registration)
        {
            try
            {
                var isUsernameExists = _IRepository.CheckUserNameExists(Registration.Username);

                if (isUsernameExists)
                {
                    ModelState.AddModelError("", errorMessage: "Username Already Used try unique one!");
                }
                else
                {
                    Registration.CreatedOn = DateTime.Now;
                    Registration.RoleID = _IRoles.getRolesofUserbyRolename("Users");
                    Registration.Password = EncryptionLibrary.EncryptText(Registration.Password);
                    Registration.ConfirmPassword = EncryptionLibrary.EncryptText(Registration.ConfirmPassword);
                    if (_IRepository.AddUser(Registration) > 0)
                    {
                        TempData["MessageRegistration"] = "Data Saved Successfully!";
                        return View(Registration);
                    }
                    else
                    {
                        return View(Registration);
                    }
                }

                return View(Registration);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public JsonResult CheckUserNameExists(string Username)
        {
            try
            {
                var isUsernameExists = _IRepository.CheckUserNameExists(Username);
                if (isUsernameExists)
                {
                    return Json(data: true);
                }
                else
                {
                    return Json(data: false);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
