using Practice.Models;
using Practice.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practice.Controllers
{
    public class LoginController : Controller
    {
        UserRepository userRepo = new UserRepository();
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["uname"] != null)
            {
                return RedirectToAction("", "Home");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("uname");
            return View();
        }

        [HttpPost]
        public JsonResult Validate(user u)
        {
            int id = userRepo.Validate(u);
            if(id != -1)
            {
                Session["uname"] = u.userName;
                return Json(userRepo.Validate(u)); 
            }
            else
            {
                return Json(userRepo.Validate(u));
            }
            
        }

    }
}