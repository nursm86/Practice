using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using ViewRendererSamples.Models;
using Westwind.Web.Mvc;

namespace ViewRendererSamplesSamples.Controllers
{
    public class SamplesController : Controller
    {
        public ActionResult RenderViewToString()
        {
            var model = new EmailConfirmationModel()
            {
                FirstName = "John",
                LastName = "Doe",
                InvoiceNo = "DoeBoy1",
                Email = "doeboy@doefactory.com",
                OrderTotal = 299.33M,
                ItemSku="WWHELP",
                ItemDescription="Html Help Builder",
                StoreName = "West Wind Web Store"
            };
                        

            //string html = RenderViewToString(ControllerContext,
            //                                 "~/views/samples/ConfirmationEmail.cshtml",
            //                                 model,true);

            string html = ViewRenderer.RenderView("~/views/samples/ConfirmationEmail.cshtml", model);

            // Do something interesting with the HTML string
            // App.SendMail(model.Email,"Order Confirmation #" + model.InvoiceNo)

            // For demo just push it into the View and display the HTML
            ViewBag.RenderedHtml = html;

            return View();
        }

        [System.Web.Http.HttpGet]
        public ActionResult ThrowError()
        {
            
            string name = null;

            // Throw a null exception error on purpose!
            // if you're debugging keep stepping
            // and let the Error Module catch the error
            name = name.ToLower();

            return null;
        }


        /// <summary>
        /// Internal method that handles rendering of either partial or 
        /// or full views.
        /// </summary>
        /// <param name="viewPath">
        /// The path to the view to render. Either in same controller, shared by 
        /// name or as fully qualified ~/ path including extension
        /// </param>
        /// <param name="model">Model to render the view with</param>
        /// <param name="partial">Determines whether to render a full or partial view</param>
        /// <returns>String of the rendered view</returns>
        protected static string RenderViewToString(ControllerContext context, 
                                            string viewPath, 
                                            object model = null,
                                            bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View could not be found.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                            context.Controller.ViewData,
                                            context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }

        public static T CreateController<T>(RouteData routeData = null)
            where T : Controller, new()
        {
            // create a disconnected controller instance
            T controller = new T();

            // get context wrapper from HttpContext if available
            HttpContextBase wrapper;
            if (System.Web.HttpContext.Current != null)
                wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
            else
                throw new InvalidOperationException(
                    "Can't create Controller Context if no "+ 
                    "active HttpContext instance is available.");

            if (routeData == null)
                routeData = new RouteData();

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller") && 
                !routeData.Values.ContainsKey("Controller"))
                routeData.Values.Add("controller",
                                        controller.GetType().Name
                                                            .ToLower()
                                                            .Replace("controller", ""));

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }

    }
}
