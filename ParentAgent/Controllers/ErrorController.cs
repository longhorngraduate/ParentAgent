using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParentAgent.Controllers
{
    public class ErrorController : Controller
    {
        [AllowAnonymous]
        public ActionResult Sorry()
        {
            return View();
        }
    }
}