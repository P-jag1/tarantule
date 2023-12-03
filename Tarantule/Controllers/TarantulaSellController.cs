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
    
    public class TarantulaSellController : Controller
    {        

        public ActionResult Index(int? page)
        {           
            int itemsOnPage = 6;
            int pg = page.HasValue ? page.Value : 1;
            int totalTarantulaSells;

            TarantuleSellDao sells = new TarantuleSellDao();
            IList<TarantuleSell> tarantulaSells = sells.GetTarantulaSellPaged(itemsOnPage, pg, out totalTarantulaSells);

            ViewBag.Pages = (int)Math.Ceiling((double)totalTarantulaSells / (double)itemsOnPage);
            ViewBag.CurrentPage = pg;

            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);             

           
            if (Request.IsAjaxRequest())
            {
                return PartialView(tarantulaSells);
            }

            return View(tarantulaSells);
        
        }      

        public ActionResult Detail(int id)
        {
            TarantuleSellDao sell = new TarantuleSellDao();
            TarantuleSell tar = sell.GetById(id);
            
            return View(tar);
        }       

        public ActionResult Search(string phrase)
        {
            TarantuleSellDao sell = new TarantuleSellDao();
            IList<TarantuleSell> TarantuleSell = sell.SearchTarantulaSells(phrase);
            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Index", TarantuleSell);
            }

            return View("Index", TarantuleSell);
        }

        public JsonResult SearchTarantulaSells(string query)
        {
            TarantuleSellDao sells = new TarantuleSellDao();
            IList<TarantuleSell> sell = sells.SearchTarantulaSells(query);

            List<string> names = (from TarantuleSell tar in sell select tar.Name).ToList();

            return Json(names, JsonRequestBehavior.AllowGet);
        }       
    }
}
