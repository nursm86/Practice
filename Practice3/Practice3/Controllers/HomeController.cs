using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser;
using Practice3.Models;
using RazorEngine;
using RazorEngine.Templating;

namespace Practice3.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Practice3DBEntities context = new Practice3DBEntities();
            List<Employee> model = context.Employees.ToList();
            return View(model);
            //return Content("<h1>it works</h1>");
        }
        public ActionResult Test()
        {
            return View();
        }

        public ActionResult RenderViewToString()
        {
            //https://github.com/Antaris/RazorEngine

            Practice3DBEntities context = new Practice3DBEntities();
            var model = context.Employees.ToList();
            var index = context.Views.Where(x => x.ViewName == "Index").FirstOrDefault().ViewContent;
            var test = context.Views.Where(x => x.ViewName == "Test").FirstOrDefault().ViewContent;
            var test2 = context.Views.Where(x => x.ViewName == "Test2").FirstOrDefault().ViewContent;
            //var layout = context.Views.Where(x => x.ViewName == "Layout").FirstOrDefault().ViewContent;

            //var Layout = Razor.Parse(layout);
            string templateFile = "I:/4rth year/intern/Practice3/Practice3/Views/Shared/_Layout.cshtml";
            //var result = Engine.Razor.RunCompile(new LoadedTemplateSource(templateFile, index), "ho", null, model);

            var result = Razor.Parse(index, model);
            //var templateResolver = Razor.Resolve("_Layout.cshtml");
            //return templateResolver.Run(new ExecuteContext());


            //var result2 = Razor.Parse(test);
            //var result3 = Razor.Parse(test2, model.FirstOrDefault());

            return Content(result);
        }

        static string RenderViewToString(ControllerContext context, string viewPath, object model = null, bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            //ViewResult.View = text;
            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            //Practice3DBEntities con = new Practice3DBEntities();
            //var text = con.Views.Where(x => x.ViewName == "Index").FirstOrDefault().ViewContent;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }
            return result;
        }

    }
}