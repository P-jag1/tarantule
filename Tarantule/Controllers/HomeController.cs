using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Model;

namespace WebApplication1.Controllers
{
    
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {      

            return View();
        }

        public ActionResult AjaxRequest()
        {
            return View();
        }

    }
}