using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Model;
using WebApplication1.Class;

namespace WebApplication1.Controllers
{
    
    public class TarantulaSpecieController : Controller
    {

        public ActionResult Index(int? page)
        {
            int itemsOnPage = 6;
            int pg = page.HasValue ? page.Value : 1;
            int totalTarantulaSpecies;

            TarantuleSpecieDao tarantulaDao = new TarantuleSpecieDao();
            IList<TarantuleSpecie> tar = tarantulaDao.GetTarantulaPaged(itemsOnPage, pg, out totalTarantulaSpecies);

            ViewBag.Pages = (int)Math.Ceiling((double)totalTarantulaSpecies / (double)itemsOnPage);
            ViewBag.CurrentPage = pg;

            if (Request.IsAjaxRequest())
            {
                return PartialView(tar);
            }

            return View(tar);
        }

        public ActionResult Detail(int id)
        {
            TarantuleSpecieDao tarantulaDao = new TarantuleSpecieDao();
            TarantuleSpecie tar = tarantulaDao.GetById(id);
            
            return View(tar);
        }

        public ActionResult Search(string phrase)
        {
            TarantuleSpecieDao TarantuleSpecieDao = new TarantuleSpecieDao();
            IList<TarantuleSpecie> TarantuleSpecie = TarantuleSpecieDao.SearchTarantulaSpecies(phrase);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Index", TarantuleSpecie);
            }

            return View("Index", TarantuleSpecie);
        }

        public JsonResult SearchTarantulaSpecies(string query)
        {
            TarantuleSpecieDao TarantuleSpecieDao = new TarantuleSpecieDao();
            IList<TarantuleSpecie> TarantuleSpecie = TarantuleSpecieDao.SearchTarantulaSpecies(query);

            List<string> names = (from TarantuleSpecie tar in TarantuleSpecie select tar.Name).ToList();

            return Json(names, JsonRequestBehavior.AllowGet);
        }

    }
}