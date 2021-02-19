using Newtonsoft.Json;
using Practice.Models;
using Practice.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Practice.Controllers
{
    public class ProductController : Controller
    {
        ProductRepository proRepo = new ProductRepository();
        public ActionResult ProductList()
        {
            //if (Session["uname"] == null)
            //{
            //    TempData["errmsg"] = "Please Login first!!!";
            //    return RedirectToAction("", "Login");
            //}
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
        public JsonResult GetProductDataExtra()
        {
            dynamic dataExtra = new System.Dynamic.ExpandoObject();
            dataExtra.categories = JsonConvert.SerializeObject(proRepo.GetAllCategories());
            dataExtra.dealers = JsonConvert.SerializeObject(proRepo.GetAllDealers());
            return Json(dataExtra);
        }

        [HttpGet]

        public JsonResult GetAllProduct()
        {
            return Json(JsonConvert.SerializeObject(proRepo.GetAll()), JsonRequestBehavior.AllowGet);
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
            dynamic data = new System.Dynamic.ExpandoObject();
            string error = string.Empty;
            p.updatedDate = DateTime.Now;

            SaveProductLogic(p, out error);

            data.product = p;
            data.errors = error;
            return Json(JsonConvert.SerializeObject(data));
        }

        private bool IsValidToSaveProduct(product newObj, out string error)
        {
            error = "";
            if (newObj.productName.IsEmpty())
            {
                error += "Name Cannot be Empty";
                return false;
            }
            if (newObj.productName.Length > 50)
            {
                error += "Name exceeded its maximum length 50.";
                return false;
            }
            if (newObj.quantity <= 0)
            {
                error += "Please input a Valid Quantity.";
                return false;
            }
            if (newObj.price <= 0)
            {
                error += "Price is not Valid .";
                return false;
            }
            
            if (newObj.category1Id <= 0)
            {
                error += "Please select valid Category.";
                return false;
            }
            if (newObj.category2Id <= 0)
            {
                error += "Please select valid 2nd Category.";
                return false;
            }

            if (newObj.dealerId <= 0)
            {
                error += "Please select valid Dealer.";
                return false;
            }

            return true;
        }
        private bool SaveProductLogic(product newObj, out string error)
        {
            error = string.Empty;
            if (newObj == null)
            {
                error = "Product to save can't be null!";
                return false;
            }

            if (!IsValidToSaveProduct(newObj, out error))
                return false;

            if (newObj.productId != 0)
            {
                proRepo.UpdateProduct(newObj);
            }
            else
            {
                newObj.createdDate = DateTime.Now;
                proRepo.InsertProduct(newObj);
            }

            return true;
        }
    }
}