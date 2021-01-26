using Newtonsoft.Json;
using Practice.Models;
using Practice.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practice.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        UserRepository userRepo = new UserRepository();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetUsersList()
        {
            return Json(JsonConvert.SerializeObject(userRepo.GetAll()));
        }

        [HttpPost]
        public JsonResult GetUserInfoById(int id)
        {
            return Json(JsonConvert.SerializeObject(userRepo.Get(id)));
        }

        [HttpPost]
        public JsonResult UpdateUser(user u)
        {
            u.updatedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                return Json(JsonConvert.SerializeObject(userRepo.Update(u)));
            }
            else
            {
                return Json(false);
            }
        }

        [HttpPost]

        public JsonResult CreateNewUser(user u)
        {
            u.updatedDate = DateTime.Now;
            //return Json(JsonConvert.SerializeObject(u));
            if (ModelState.IsValid)
            {
                return Json(JsonConvert.SerializeObject(userRepo.Insert(u)));
            }
            else
            {
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult DeleteUserById(int id)
        {
            userRepo.Delete(id);
            return Json(true);
        }
    }
}