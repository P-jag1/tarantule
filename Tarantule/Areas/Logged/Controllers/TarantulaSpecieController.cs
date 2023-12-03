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
    [Authorize]
    public class TarantulaSpecieController : Controller
    {
        // GET: Books
        public ActionResult Index(int? page)
        {
            int itemsOnPage = 6;
            int pg = page.HasValue ? page.Value : 1;
            int totalTarantulaSpecies;

            TarantuleSpecieDao TarantulaSpecieDao = new TarantuleSpecieDao();
            IList<TarantuleSpecie> tarantulaSpecies = TarantulaSpecieDao.GetTarantulaPaged(itemsOnPage, pg, out totalTarantulaSpecies);

            ViewBag.Pages = (int)Math.Ceiling((double)totalTarantulaSpecies / (double)itemsOnPage);
            ViewBag.CurrentPage = pg;

            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            if(user.Role.Identificator == "chovatel")
                return View("IndexChovatel", tarantulaSpecies);

            if (Request.IsAjaxRequest())
            {
                return PartialView(tarantulaSpecies);
            }

            return View(tarantulaSpecies);
        }

        public ActionResult IndexModerator(int? page)
        {
            int itemsOnPage = 6;
            int pg = page.HasValue ? page.Value : 1;
            int totalTarantulaSpecies;

            TarantuleSpecieDao TarantulaSpecieDao = new TarantuleSpecieDao();
            IList<TarantuleSpecie> tarantulaSpecies = TarantulaSpecieDao.GetTarantulaPaged(itemsOnPage, pg, out totalTarantulaSpecies);

            ViewBag.Pages = (int)Math.Ceiling((double)totalTarantulaSpecies / (double)itemsOnPage);
            ViewBag.CurrentPage = pg;

            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            if (Request.IsAjaxRequest())
            {
                return PartialView(tarantulaSpecies);
            }

            return View(tarantulaSpecies);
        }

        public ActionResult Detail(int id)
        {
            TarantuleSpecieDao tarantulaSpecieDao = new TarantuleSpecieDao();
            TarantuleSpecie tar = tarantulaSpecieDao.GetById(id);
            
            return View(tar);
        }

        [Authorize(Roles = "admin, moderator")]
        public ActionResult Create()
        {           
            TarantuleSpecieDao tarantulaSpecieDao = new TarantuleSpecieDao();

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, moderator")]
        public ActionResult Add(TarantuleSpecie tarantule, HttpPostedFileBase picture)
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

                    TarantuleSpecieDao TarantuleDao = new TarantuleSpecieDao();
                    TarantuleDao.Create(tarantule);

                    TempData["message-success"] = "Sklípkan byl uspesne pridan";

                }
                else
                {
                    return View("Create", tarantule);
            }


                return RedirectToAction("Index");
        }        

        [Authorize(Roles = "admin, moderator")]
        public ActionResult Edit(int id)
        {
            TarantuleSpecieDao TarantuleDao = new TarantuleSpecieDao();           

            TarantuleSpecie tar = TarantuleDao.GetById(id);
            ViewBag.imageName = tar.ImageName;

            return View(tar);
        }

        [Authorize(Roles = "admin, moderator")]
        [HttpPost]
        public ActionResult Update(TarantuleSpecie tarantule, HttpPostedFileBase picture)
        {
            try
            {
                TarantuleSpecieDao tarantuleDao = new TarantuleSpecieDao();

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

                tarantuleDao.Update(tarantule);

                TempData["message-success"] = "Sklípkan " + tarantule.Name + " byl upraven";
            }
            catch (Exception)
            {
                
                throw;
            }
           
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin, moderator")]
        public ActionResult Delete(int id)
        {
            try
            {
                TarantuleSpecieDao tarantuleDao = new TarantuleSpecieDao();
                TarantuleSpecie tarantule = tarantuleDao.GetById(id);

                tarantuleDao.Delete(tarantule);

                TempData["message-success"] = "Sklípkan " + tarantule.Name + " byl smazán";
            }
            catch (Exception exception)
            {
                
                throw;
            }

            return RedirectToAction("Index");
        }

        public ActionResult Country(string name, int? page)
        {

            IList<TarantuleSpecie> tar = new TarantuleSpecieDao().GetByCountry(name);
            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            if (user.Role.Identificator == "chovatel")
                return View("IndexChovatel", tar);


            return View("Index", tar);
        }

        public ActionResult CountryModerator(string name, int? page)
        {

            IList<TarantuleSpecie> tar = new TarantuleSpecieDao().GetByCountry(name);           

            return View("IndexModerator", tar);
        }

        public ActionResult Search(string phrase)
        {
            TarantuleSpecieDao TarantuleSpecieDao = new TarantuleSpecieDao();
            IList<TarantuleSpecie> TarantuleSpecie = TarantuleSpecieDao.SearchTarantulaSpecies(phrase);
            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            if (user.Role.Identificator == "chovatel")
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("IndexChovatel", TarantuleSpecie);
                }

                return View("IndexChovatel", TarantuleSpecie);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Index", TarantuleSpecie);
                }

                return View("Index", TarantuleSpecie);
            }

        }

        public ActionResult SearchModerator(string phrase)
        {
            TarantuleSpecieDao TarantuleSpecieDao = new TarantuleSpecieDao();
            IList<TarantuleSpecie> TarantuleSpecie = TarantuleSpecieDao.SearchTarantulaSpecies(phrase);
            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);

           
            
                if (Request.IsAjaxRequest())
                {
                    return PartialView("IndexModerator", TarantuleSpecie);
                }

                return View("IndexModerator", TarantuleSpecie);
            

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