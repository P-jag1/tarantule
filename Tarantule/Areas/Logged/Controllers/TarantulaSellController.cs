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

namespace WebApplication1.Areas.Logged.Controllers
{
    public class TarantulaSellController : Controller
    {        
        // GET: Logged/TarantulaSell
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

            if (user.Role.Identificator == "chovatel")
                return View("IndexChovatel", tarantulaSells);

            if (Request.IsAjaxRequest())
            {
                return PartialView(tarantulaSells);
            }

            return View(tarantulaSells);
        
        }

        [Authorize(Roles = "admin, moderator")]
        public ActionResult IndexModerator(int? page)
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
        
        public ActionResult UserIndex(int? page)
        {
            int itemsOnPage = 5;
            int pg = page.HasValue ? page.Value : 1;
            int totalTarantulaSells;

            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            TarantuleSellDao sells = new TarantuleSellDao();
            IList<TarantuleSell> tarantulaHelp = sells.GetTarantulaSellPaged(itemsOnPage, pg, out totalTarantulaSells);
            IList<TarantuleSell> tarantulaSells = new List<TarantuleSell>();

            foreach (TarantuleSell tar in tarantulaHelp)
            {
                if (tar.User_id.Id == user.Id)                
                   tarantulaSells.Add(tar);           
                
            }   

            ViewBag.Pages = (int)Math.Ceiling((double)tarantulaSells.Count / (double)itemsOnPage);
            ViewBag.CurrentPage = pg;          

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

        [Authorize(Roles = "admin, moderator, chovatel")]
        public ActionResult Create()
        {           
            TarantuleSellDao sell = new TarantuleSellDao();

            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);
            ViewBag.id = user.Role.Id;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, moderator, chovatel")]
        public ActionResult Add(TarantuleSell tarantule, HttpPostedFileBase picture)
        {

            if (ModelState.IsValid)
            {
                if (picture != null)
                {
                    if (picture.ContentType == "image/jpeg" || picture.ContentType == "image/png")
                    {
                        Image image = Image.FromStream(picture.InputStream);

                        if (image.Height > 200 || image.Width > 200)
                        {
                            Image smallImage = ImageHelper.ScaleImage(image, 200, 200);
                            Bitmap b = new Bitmap(smallImage);

                            Guid guid = Guid.NewGuid();
                            string imageName = guid.ToString() + ".jpg";

                            b.Save(Server.MapPath("~/uploads/tarantule/" + imageName), ImageFormat.Jpeg);

                            smallImage.Dispose();
                            b.Dispose();

                            tarantule.ImageName = imageName;
                        }
                        else
                        {
                            picture.SaveAs(Server.MapPath("~/uploads/tarantule/" + picture.FileName));
                        }
                    }
                }

                TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);
                tarantule.User_id = user;

                TarantuleSellDao TarantuleDao = new TarantuleSellDao();
                TarantuleDao.Create(tarantule);

                TempData["message-success"] = "Nová nabídka byla úspěšně přidána";

            }
            else
            {
                return View("Create", tarantule);
            }

            TarantuleUser userh = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            if (userh.Role.Identificator == "chovatel")
                return RedirectToAction("UserIndex");

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin, moderator, chovatel")]
        public ActionResult Edit(int id)
        {
            TarantuleSellDao TarantuleDao = new TarantuleSellDao();
            TarantuleSell tar = TarantuleDao.GetById(id);

            TarantuleUser user = new TarantuleUserDao().GetById(tar.User_id.Id);
            TarantuleUser userhelp = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            if (userhelp.Role.Identificator == "chovatel")
            {
                if (tar.User_id.Id == userhelp.Id)
                {
                    ViewBag.User = user.Id;

                    ViewBag.imageName = tar.ImageName;


                    return View(tar);

                }

                return RedirectToAction("UserIndex");
            }
            else
            {
                ViewBag.User = user.Id;

                ViewBag.imageName = tar.ImageName;


                return View(tar);
            }

        }      

        [Authorize(Roles = "admin, moderator, chovatel")]
        [HttpPost]
        public ActionResult Update(TarantuleSell tarantule, HttpPostedFileBase picture)
        {
            try
            {
                if (picture != null)
                {
                    if (picture.ContentType == "image/jpeg" || picture.ContentType == "image/png")
                    {
                        Image image = Image.FromStream(picture.InputStream);

                        Guid guid = Guid.NewGuid();
                        string imageName = guid.ToString() + ".jpg";

                        if (image.Height > 200 || image.Width > 200)
                        {
                            Image smallImage = ImageHelper.ScaleImage(image, 200, 200);
                            Bitmap b = new Bitmap(smallImage);

                            b.Save(Server.MapPath("~/uploads/tarantule/" + imageName), ImageFormat.Jpeg);

                            smallImage.Dispose();
                            b.Dispose();

                        }
                        else
                        {
                            picture.SaveAs(Server.MapPath("~/uploads/tarantule/" + picture.FileName));
                        }

                        //System.IO.File.Delete(Server.MapPath("~/uploads/tarantule/" + tarantule.ImageName));

                        tarantule.ImageName = imageName;
                    }
                }
                else
                {
                    string imageName = Convert.ToString(TempData["ImageName"]);
                    tarantule.ImageName = imageName;
                }
                
                int id = Convert.ToInt32(TempData["Data1"]);
                TarantuleUser user = new TarantuleUserDao().GetById(id);
                tarantule.User_id = user;

                TarantuleSellDao tarantuleDao = new TarantuleSellDao();
                tarantuleDao.Update(tarantule);

                TempData["message-success"] = "Prodej " + tarantule.Name + " byl upraven";
            }
            catch (Exception)
            {               
                throw;
            }

            TarantuleUser userh = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            if (userh.Role.Identificator == "chovatel")
                return RedirectToAction("UserIndex");

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin, moderator, chovatel")]
        public ActionResult Delete(int id)
        {
            try
            {
                TarantuleSellDao tarantuleDao = new TarantuleSellDao();
                TarantuleSell tarantule = tarantuleDao.GetById(id);                

                TempData["message-success"] = "Prodej " + tarantule.Name + " byl smazán";

                TarantuleUser userhelp = new TarantuleUserDao().GetByLogin(User.Identity.Name);

                if (userhelp.Role.Identificator == "chovatel")
                {
                     if (tarantule.User_id.Id == userhelp.Id)
                {
                    tarantuleDao.Delete(tarantule);

                    return RedirectToAction("UserIndex"); ;
                }

                return RedirectToAction("UserIndex");                    
                }
                else
                {
                    tarantuleDao.Delete(tarantule);

                    return RedirectToAction("Index"); 
                }
            }
            catch (Exception exception)
            {
                
                throw;
            }
        }

        public ActionResult Search(string phrase)
        {
            TarantuleSellDao sell = new TarantuleSellDao();
            IList<TarantuleSell> TarantuleSell = sell.SearchTarantulaSells(phrase);
            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            if (user.Role.Identificator == "chovatel")
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("IndexChovatel", TarantuleSell);
                }

                return View("IndexChovatel", TarantuleSell);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Index", TarantuleSell);
                }

                return View("Index", TarantuleSell);
            }
        }

        public ActionResult SearchModerator(string phrase)
        {
            TarantuleSellDao sell = new TarantuleSellDao();
            IList<TarantuleSell> TarantuleSell = sell.SearchTarantulaSells(phrase);
            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);
          
                if (Request.IsAjaxRequest())
                {
                    return PartialView("IndexModerator", TarantuleSell);
                }

                return View("IndexModerator", TarantuleSell);
            
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
