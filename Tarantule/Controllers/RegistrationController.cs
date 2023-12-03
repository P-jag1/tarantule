using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Dao;
using DataAccess.Model;
using System.Web.Security;

namespace Tarantule.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(TarantuleUser user)
        {
            if (ModelState.IsValid)
            {
                TarantuleRoleDao tarantuleRoleDao = new TarantuleRoleDao();
                TarantuleRole role = tarantuleRoleDao.GetById(3);

                IList<TarantuleUser> helpList = new TarantuleUserDao().GetAll();

                user.Role = role;

                string help = user.Password;
                string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(help, "SHA1");

                string helpPass = Request["password1"];
                string hashHelpPass = FormsAuthentication.HashPasswordForStoringInConfigFile(helpPass, "SHA1");

                foreach(TarantuleUser tar in helpList)
                {
                    if (user.Login == tar.Login)
                    {
                        TempData["message-LoginFail"] = "Login už je zabraný";
                        return View("Index", user);
                    }
                }

                if (hashHelpPass == hash)
                {
                    user.Password = hash;

                    TarantuleUserDao userDao = new TarantuleUserDao();
                    userDao.Create(user);

                    TempData["message-success"] = "Nový uživatel byl uspěšně přidán";
                }
                else
                {
                    TempData["message-danger"] = "Hesla se neshodují";
                    return View("Index", user);                   
                }
            
            }
            else
            {
                return View("Index", user);
            }


            return RedirectToAction("Index");
        }
    }
}