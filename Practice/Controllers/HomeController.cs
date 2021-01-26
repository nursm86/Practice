using Newtonsoft.Json;
using Practice.Models;
using Practice.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Practice.Controllers
{
    public class HomeController : Controller
    {
        ProductRepository proRepo = new ProductRepository();
        public ActionResult Index()
        {
            if(Session["uname"] == null)
            {
                TempData["errmsg"] = "Please Login first!!!";
                return RedirectToAction("", "Login");
            }
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Products()
        {
            return View();
        }

        public ActionResult ProductInfo()
        {
            if (Session["uname"] == null)
            {
                TempData["errmsg"] = "Please Login first!!!";
                return RedirectToAction("", "Login");
            }
            return View();
        }
        [HttpPost]
        
        public JsonResult GetProductInfoById(int id)
        {
            return Json(JsonConvert.SerializeObject(proRepo.Get(id)));
        }
        [HttpPost]

        public JsonResult DeleteProductById(int id)
        {
            proRepo.Delete(id);
            return Json(true);
        }

        [HttpPost]
        public JsonResult CreateNewProduct(product p)
        {
            p.createdDate = DateTime.Now;
            p.updatedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                return Json(JsonConvert.SerializeObject(proRepo.InsertProduct(p)));
            }
            else
            {
                return Json(false);
            }
            
        }
        [HttpPost]
        public JsonResult UpdateProduct(product p)
        {
            p.updatedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                return Json(JsonConvert.SerializeObject(proRepo.UpdateProduct(p)));
            }
            else
            {
                return Json(false);
            }
        }

        [HttpPost]

        public JsonResult GetProductsList(int totalShow, int pageNo)
        {
            string json = JsonConvert.SerializeObject(proRepo.GetAll());
            return Json(json);
        }

        public ActionResult CreateNewProduct()
        {
            if (Session["uname"] == null)
            {
                TempData["errmsg"] = "Please Login first!!!";
                return RedirectToAction("", "Login");
            }
            return View();
        }
    }
}