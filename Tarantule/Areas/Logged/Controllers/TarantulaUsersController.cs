using DataAccess.Dao;
using DataAccess.Model;
using System;
using System.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tarantule.Areas.Logged.Controllers
{
    [Authorize]
    public class TarantulaUsersController : Controller
    {
        [Authorize(Roles = "admin")]
        public ActionResult Index(int? page)
        {
            int itemsOnPage = 5;
            int pg = page.HasValue ? page.Value : 1;
            int totalUsers;

            TarantuleUserDao userDao = new TarantuleUserDao();

            IList<TarantuleUser> users = userDao.GetUserPaged(itemsOnPage, pg, out totalUsers);

            ViewBag.Pages = (int)Math.Ceiling((double)totalUsers / (double)itemsOnPage);
            ViewBag.CurrentPage = pg;

            ViewBag.Roles = new TarantuleRoleDao().GetAll();



            if (Request.IsAjaxRequest())
            {
                return PartialView(users);
            }

            return View(users);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Detail(int id)
        {
            TarantuleUserDao userDao = new TarantuleUserDao();
            TarantuleUser user = userDao.GetById(id);

            if (Request.IsAjaxRequest())
            {
                return PartialView(user);
            }

            return View(user);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            TarantuleRoleDao TarantuleRoleDao = new TarantuleRoleDao();
            IList<TarantuleRole> roles = TarantuleRoleDao.GetAll();
            ViewBag.Role = roles;

            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]        
        public ActionResult Add(TarantuleUser user, int roleId)
        {
            if (ModelState.IsValid)
            {
                TarantuleRoleDao tarantuleRoleDao = new TarantuleRoleDao();
                user.Role = tarantuleRoleDao.GetById(roleId);

                IList<TarantuleUser> helpList = new TarantuleUserDao().GetAll();

                string help = user.Password;
                string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(help, "SHA1");

                string helpPass = Request["password1"];
                string hashHelpPass = FormsAuthentication.HashPasswordForStoringInConfigFile(helpPass, "SHA1");

                foreach (TarantuleUser tar in helpList)
                {
                    if (user.Login == tar.Login)
                    {
                        TempData["message-LoginFail"] = "Login už je zabraný";
                        IList<TarantuleRole> roles =  new TarantuleRoleDao().GetAll();
                        ViewBag.Role = roles;
                        return View("Create", user);
                    }
                }

                if (hashHelpPass == hash)
                {
                    user.Password = hash;
                }
                else
                {
                    TempData["message-danger"] = "Hesla se neshodují";
                    IList<TarantuleRole> roles =  new TarantuleRoleDao().GetAll();
                    ViewBag.Role = roles;
                    return View("Create", user);
                }


                    user.Password = hash;

                    TarantuleUserDao userDao1 = new TarantuleUserDao();
                    userDao1.Create(user);

                    TempData["message-success"] = "Nový uživatel byl přidán byla uspěšne přidán";

            }
            else
            {
                
                return View("Create", user);
            }


            return RedirectToAction("Index");
        }

       [Authorize(Roles = "admin, moderator, chovatel")]
        public ActionResult Edit(int id)
        {
            TarantuleUserDao userDao = new TarantuleUserDao();
            TarantuleRoleDao tarantuleRoleDao = new TarantuleRoleDao();

            TarantuleUser userhelp = new TarantuleUserDao().GetByLogin(User.Identity.Name);
            ViewBag.user = userhelp.Role.Identificator;



            TarantuleUser user = userDao.GetById(id);
            ViewBag.roles = tarantuleRoleDao.GetAll();
            ViewBag.pass = user.Password;

            if (userhelp.Role.Identificator == "chovatel" || userhelp.Role.Identificator == "moderator")
            {
                if (user.Id == userhelp.Id)
                {

                    return View(user);

                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(user);
            }
        }

       [Authorize(Roles = "admin, moderator, chovatel")]
       [HttpPost]
       public ActionResult Update(TarantuleUser user, int roleId)
       {
            TarantuleUser userhelp = new TarantuleUserDao().GetByLogin(User.Identity.Name);

           try
           {
               TarantuleUserDao userDao = new TarantuleUserDao();
               TarantuleRoleDao tarantuleRoleDao = new TarantuleRoleDao();
               TarantuleRole role = tarantuleRoleDao.GetById(roleId);

               user.Role = role;

               string helpHash = Convert.ToString(TempData["Pass"]);
               string help = user.Password;

               if (helpHash != help)
               {
                   string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(help, "SHA1");
                   user.Password = hash;
               }

               string pass =  Request["password3"];
               string pass2 = Request["password1"];
               string pass3 = Request["password2"];
               

               if (pass != null || pass2 != null || pass3 != null)
               {
                   if (pass != "" && pass2 != "" && pass3 != "")
                   {

                       string hash1 = FormsAuthentication.HashPasswordForStoringInConfigFile(pass, "SHA1");
                       string hash2 = FormsAuthentication.HashPasswordForStoringInConfigFile(pass2, "SHA1");
                       string hash3 = FormsAuthentication.HashPasswordForStoringInConfigFile(pass3, "SHA1");

                       if (hash1 == userhelp.Password)
                       {
                           if (hash2 == hash3)
                           {
                               user.Password = hash2;
                           }
                           else
                           {
                               TempData["message-error"] = "Nastala neshoda hesel";
                               ViewBag.user = userhelp.Role.Identificator;
                               ViewBag.roles = tarantuleRoleDao.GetAll();
                               return View("Edit", user);
                           }
                        }
                        else
                        {
                           TempData["message-error"] = "Nastala neshoda hesel";
                           ViewBag.user = userhelp.Role.Identificator;
                           ViewBag.roles = tarantuleRoleDao.GetAll();
                           return View("Edit", user);
                        }       
                      }
                      else if (pass != "")
                      {
                          TempData["message-error"] = "Nastala neshoda hesel";
                          ViewBag.user = userhelp.Role.Identificator;
                          ViewBag.roles = tarantuleRoleDao.GetAll();
                          return View("Edit", user);
                      }
                   else if (pass2 != "")
                   {
                       TempData["message-error"] = "Nastala neshoda hesel";
                       ViewBag.user = userhelp.Role.Identificator;
                       ViewBag.roles = tarantuleRoleDao.GetAll();
                       return View("Edit", user);
                   }
                   else if (pass3 != "")
                   {
                       TempData["message-error"] = "Nastala neshoda hesel";
                       ViewBag.user = userhelp.Role.Identificator;
                       ViewBag.roles = tarantuleRoleDao.GetAll();
                       return View("Edit", user);
                   }

               }

               userDao.Update(user);

               TempData["message-success"] = "Uživatel " + user.Name + " byl upraven";
           }
           catch (Exception)
           {
               throw;
           }

           if (userhelp.Role.Identificator == "chovatel" || userhelp.Role.Identificator == "moderator")
           {
               return RedirectToAction("Index", "Home");
           }
           else
           {
               return RedirectToAction("Index", "TarantulaUsers");
           }
       } 
    

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                TarantuleUserDao userDao = new TarantuleUserDao();
                TarantuleUser user = userDao.GetById(id);
             
                userDao.Delete(user);

                TempData["message-success"] = "Uživatel " + user.Name + " byl smazán";
            }
            catch (Exception exception)
            {

                throw;
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Role(int id, int? page)
        {

            IList<TarantuleUser> user = new TarantuleUserDao().GetUserInRoleId(id);          

            ViewBag.Roles = new TarantuleRoleDao().GetAll();

            return View("Index", user);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Search(string phrase)
        {
            TarantuleUserDao userDao = new TarantuleUserDao();
            IList<TarantuleUser> TarantuleUser = userDao.SearchUsers(phrase);

            if (Request.IsAjaxRequest())
            {
                ViewBag.Roles = new TarantuleRoleDao().GetAll();
                return PartialView("Index", TarantuleUser);
            }

            ViewBag.Roles = new TarantuleRoleDao().GetAll();
            return View("Index", TarantuleUser);
        }

        [Authorize(Roles = "admin")]
        public JsonResult SearchUser(string query)
        {
            TarantuleUserDao userDao = new TarantuleUserDao();
            IList<TarantuleUser> TarantuleUser = userDao.SearchUsers(query);

            List<string> logins = (from TarantuleUser user in TarantuleUser select user.Login).ToList();

            return Json(logins, JsonRequestBehavior.AllowGet);
        }
    }
}