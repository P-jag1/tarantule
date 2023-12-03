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
    public class TarantulaOfUserController : Controller
    {

        // GET: Logged/TarantulaOfUser
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

            if (user.Role.Identificator == "chovatel")
                return View("IndexChovatel", userTar);

            if (Request.IsAjaxRequest())
            {
                return PartialView(userTar);
            }

            return View(userTar);
        }

        public ActionResult IndexModerator(int? page)
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

        [Authorize(Roles = "admin, moderator, chovatel")]
        public ActionResult UserIndex(int? page)
        {
            int itemsOnPage = 5;
            int pg = page.HasValue ? page.Value : 1;
            int totalTarantulaSells;

            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            TarantuleOfUserDao userTarDao = new TarantuleOfUserDao();
            IList<TarantuleOfUser> tarantulaHelp = userTarDao.GetTarantulaOfUserPaged(itemsOnPage, pg, out totalTarantulaSells);
            IList<TarantuleOfUser> tarantulaOfUsers = new List<TarantuleOfUser>();

            foreach (TarantuleOfUser tar in tarantulaHelp)
            {
                if (tar.User_id.Id == user.Id)
                    tarantulaOfUsers.Add(tar);
            }

            ViewBag.Pages = (int)Math.Ceiling((double)tarantulaOfUsers.Count / (double)itemsOnPage);
            ViewBag.CurrentPage = pg;

            if (Request.IsAjaxRequest())
            {
                return PartialView(tarantulaOfUsers);
            }

            return View(tarantulaOfUsers);
        }

        public ActionResult Detail(int id)
        {

            TarantuleOfUserDao userTar = new TarantuleOfUserDao();
            TarantuleOfUser tar = userTar.GetById(id);

            IList<TarantuleCommentOfUser> com = new TarantuleCommentOfUserDao().GetAll();
            IList<TarantuleCommentOfUser> tarantulaCommentOfUsers = new List<TarantuleCommentOfUser>();

            foreach (TarantuleCommentOfUser tarCom in com)
            {
                if (tarCom.UserTar_id.Id == tar.Id)
                    tarantulaCommentOfUsers.Add(tarCom);
            }

            ViewBag.Comments = tarantulaCommentOfUsers;

            return View(tar);
        }

        [Authorize(Roles = "admin, moderator, chovatel")]
        public ActionResult Create()
        {
            TarantuleOfUserDao userTar = new TarantuleOfUserDao();

            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);
            ViewBag.id = user.Role.Id;

            return View();
        }     

        [HttpPost]
        [Authorize(Roles = "admin, moderator, chovatel")]
        public ActionResult Add(TarantuleOfUser tarantule, HttpPostedFileBase picture)
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

                TarantuleOfUserDao TarantuleDao = new TarantuleOfUserDao();
                TarantuleDao.Create(tarantule);

                TempData["message-success"] = "Nový záznam o vašem sklípkanovi byl přidán";

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
            TarantuleOfUserDao TarantuleDao = new TarantuleOfUserDao();
            TarantuleOfUser tar = TarantuleDao.GetById(id);

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
        public ActionResult Update(TarantuleOfUser tarantule, HttpPostedFileBase picture)
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

                TarantuleOfUserDao tarantuleDao = new TarantuleOfUserDao();
                tarantuleDao.Update(tarantule);

                TempData["message-success"] = "Záznam " + tarantule.Nickname + " byl upraven";
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
                TarantuleOfUserDao tarantuleDao = new TarantuleOfUserDao();
                TarantuleOfUser tarantule = tarantuleDao.GetById(id);

                TempData["message-success"] = "Záznam " + tarantule.Name + " byl smazán";

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
            TarantuleOfUserDao userTar = new TarantuleOfUserDao();
            IList<TarantuleOfUser> tarantuleOfUser = userTar.SearchTarantulaOfUsers(phrase);
            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            if (user.Role.Identificator == "chovatel")
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("IndexChovatel", tarantuleOfUser);
                }

                return View("IndexChovatel", tarantuleOfUser);
            }
            else
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Index", tarantuleOfUser);
                }

                return View("Index", tarantuleOfUser);
            }
        }

        public ActionResult SearchModerator(string phrase)
        {
            TarantuleOfUserDao userTar = new TarantuleOfUserDao();
            IList<TarantuleOfUser> tarantuleOfUser = userTar.SearchTarantulaOfUsers(phrase);
            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            
                if (Request.IsAjaxRequest())
                {
                    return PartialView("IndexModerator", tarantuleOfUser);
                }

                return View("IndexModerator", tarantuleOfUser);
            
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