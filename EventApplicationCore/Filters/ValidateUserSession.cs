using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApplicationCore.Filters
{
    public class ValidateUserSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetString("RoleID"))))
            {

                string UserCurrentRole = (string)context.HttpContext.Session.GetString("RoleID");

                if (UserCurrentRole != "2") //User Role = 2
                {
                    ViewResult result = new ViewResult();
                    result.ViewName = "Error";

                    var controller = context.Controller as Controller;
                    controller.ViewData["ErrorMessage"] = "Invalid User";
                    controller.HttpContext.Session.Clear();
                    context.Result = result;
                }

            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                var controller = context.Controller as Controller;
                controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                controller.HttpContext.Session.Clear();
                context.Result = result;

            }
        }
    }
}
