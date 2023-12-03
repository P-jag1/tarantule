using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Areas.Logged.Controllers
{
    public class AboutUsController : Controller
    {
        // GET: Logged/AboutUs
        public ActionResult Index()
        {
            return View();
        }
    }
}