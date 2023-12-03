using DataAccess.Dao;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Areas.Logged.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        //tahle akce může být volaná pouze jinou akcí
        [ChildActionOnly]
        // GET: Admin/Menu
        public ActionResult Index()
        {
            TarantuleUserDao TarantuleUserDao = new TarantuleUserDao();
            TarantuleUser TarantuleUser = TarantuleUserDao.GetByLogin(User.Identity.Name);




            return View(TarantuleUser);
        }
    }
}