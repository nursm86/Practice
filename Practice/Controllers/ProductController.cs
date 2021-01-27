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
    public class ProductController : Controller
    {
        ProductRepository proRepo = new ProductRepository();
        public ActionResult ProductList()
        {
            if (Session["uname"] == null)
            {
                TempData["errmsg"] = "Please Login first!!!";
                return RedirectToAction("", "Login");
            }
            return View();
        }

        public ActionResult ProductAddEdit()
        {
            if (Session["uname"] == null)
            {
                TempData["errmsg"] = "Please Login first!!!";
                return RedirectToAction("", "Login");
            }
            return View();
        }

        //******************************Web Api from here************************

        [HttpPost]
        public JsonResult GetAllCategory()
        {
            return Json(JsonConvert.SerializeObject(proRepo.GetAllCategories()));
        }

        [HttpGet]

        public JsonResult GetAllProduct()
        {
            string json = JsonConvert.SerializeObject(proRepo.GetAll());
            return Json(json,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProductById(int id)
        {
            return Json(JsonConvert.SerializeObject(proRepo.Get(id)));
        }
        [HttpPost]

        public JsonResult GetDeleteProductById(int id)
        {
            proRepo.Delete(id);
            return Json(true);
        }

        [HttpPost]
        public JsonResult SaveProduct(product p)
        {
            p.updatedDate = DateTime.Now;
            if (p.productId == 0)
            {
                p.createdDate = DateTime.Now;
                if (ModelState.IsValid)
                {
                    return Json(JsonConvert.SerializeObject(proRepo.InsertProduct(p)));
                }
                else
                {
                    return Json(false);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    return Json(JsonConvert.SerializeObject(proRepo.UpdateProduct(p)));
                }
                else
                {
                    return Json(false);
                }
            }

        }
    }
}