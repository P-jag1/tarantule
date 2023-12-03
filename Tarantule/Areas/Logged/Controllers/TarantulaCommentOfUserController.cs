using DataAccess.Dao;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarantule.Class;


namespace Tarantule.Areas.Logged.Controllers
{
    public class TarantulaCommentOfUserController : Controller
    {

        // GET: Logged/TarantulaCommentOfUser
        public ActionResult Index(int? page, int id)
        {
            int itemsOnPage = 20;
            int pg = page.HasValue ? page.Value : 1;
            //int totalTarantulaComments;

            TarantuleOfUser tarOfUser = new TarantuleOfUserDao().GetById(id);

            TarantuleCommentOfUserDao userTarDao = new TarantuleCommentOfUserDao();
            IList<TarantuleCommentOfUser> tarantulaHelp = userTarDao.GetAll();
            IList<TarantuleCommentOfUser> tarantulaCommentOfUsers = new List<TarantuleCommentOfUser>();

            TarantuleUser user = new TarantuleUserDao().GetByLogin(User.Identity.Name);

            foreach (TarantuleCommentOfUser tar in tarantulaHelp)
            {
                if (tar.UserTar_id.Id == tarOfUser.Id)
                    tarantulaCommentOfUsers.Add(tar);
            }

            ViewBag.Pages = (int)Math.Ceiling((double)tarantulaCommentOfUsers.Count / (double)itemsOnPage);
            ViewBag.CurrentPage = pg;
            ViewBag.Id = id;

            if (user.Role.Identificator == "chovatel")
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Index", tarantulaCommentOfUsers);
                }
                else
                {
                    return View("Index", tarantulaCommentOfUsers);
                }
                

            if (Request.IsAjaxRequest())
            {
                return PartialView("IndexAdmin", tarantulaCommentOfUsers);
            }

            return View("IndexAdmin", tarantulaCommentOfUsers);
        }

        [Authorize(Roles = "admin, moderator, chovatel")]
        public ActionResult Create(int id)
        {

            TarantuleCommentOfUserDao userComTar = new TarantuleCommentOfUserDao();
            IList<TarantuleCommentOfUser> Comtar = userComTar.GetAll();
            ViewBag.Categories = Comtar;
            ViewBag.Id = id;


            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, moderator, chovatel")]
        public ActionResult Add(TarantuleCommentOfUser comment)
        {

            comment.Extra = Convert.ToInt32(TempData["Data1"]);
            int id = comment.Extra;

            if (ModelState.IsValid)
            {
                comment.Time = DateTime.Now.ToString(); 

                TarantuleOfUser user = new TarantuleOfUserDao().GetById(id);
                comment.UserTar_id = user;

                TarantuleCommentOfUserDao TarantuleDao = new TarantuleCommentOfUserDao();
                TarantuleDao.Create(comment);

                TempData["message-success"] = "Komentář přidán";

            }
            else
            {
                return View("Create", comment);
            }


            return RedirectToAction("Index", new { id });
        }

        [Authorize(Roles = "admin, moderator")]
        public ActionResult Delete(int id, int tId)
        {

            int helpId = tId;

            try
            {
                TarantuleCommentOfUserDao tarantuleCommentDao = new TarantuleCommentOfUserDao();
                TarantuleCommentOfUser comment = tarantuleCommentDao.GetById(id);

                tarantuleCommentDao.Delete(comment);

                TempData["message-success"] = "Komentář byl smazán";
            }
            catch (Exception exception)
            {

                throw;
            }

            return RedirectToAction("Index", new { id = helpId});
        }
    }
}