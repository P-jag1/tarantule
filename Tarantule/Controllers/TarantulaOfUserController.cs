using DataAccess.Dao;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Class;

namespace WebApplication1.Controllers
{
    public class TarantulaOfUserController : Controller
    {
        // GET: TarantulaOfUser
        public ActionResult Index(int? page)
        {
            int itemsOnPage = 6;
            int pg = page.HasValue ? page.Value : 1;
            int totalTarantulaOfUsers;

            TarantuleOfUserDao userTarDao = new TarantuleOfUserDao();
            IList<TarantuleOfUser> userTar = userTarDao.GetTarantulaOfUserPaged(itemsOnPage, pg, out totalTarantulaOfUsers);

            ViewBag.Pages = (int)Math.Ceiling((double)totalTarantulaOfUsers / (double)itemsOnPage);
            ViewBag.CurrentPage = pg;

            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            if (Request.IsAjaxRequest())
            {
                return PartialView(userTar);
            }

            return View(userTar);
        }

        public ActionResult Detail(int id)
        {
            TarantuleOfUserDao userTar = new TarantuleOfUserDao();
            TarantuleOfUser tar = userTar.GetById(id);

            return View(tar);
        }
        public ActionResult Search(string phrase)
        {
            TarantuleOfUserDao userTar = new TarantuleOfUserDao();
            IList<TarantuleOfUser> tarantuleOfUser = userTar.SearchTarantulaOfUsers(phrase);
            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Index", tarantuleOfUser);
            }

            return View("Index", tarantuleOfUser);
        }

        public JsonResult SearchTarantulaOfUsers(string query)
        {
            TarantuleOfUserDao userTar = new TarantuleOfUserDao();
            IList<TarantuleOfUser> tarantuleOfUser = userTar.SearchTarantulaOfUsers(query);

            List<string> names = (from TarantuleOfUser tar in tarantuleOfUser select tar.Name).ToList();

            return Json(names, JsonRequestBehavior.AllowGet);
        }
    }
}