using System;
using System.Web;
using System.Web.Mvc;
using Westwind.Web;
using Westwind.Web.Mvc;

namespace ViewRendererSamples
{

    /// <summary>
    /// Module to handle Application.Error event processing
    /// 
    /// Comes from Westwind.Web library - just used for
    /// example purposes here to keep the sample
    /// very simple.
    /// 
    /// https://github.com/RickStrahl/WestwindToolkit/blob/master/Westwind.Web/WebErrorHandler/ApplicationErrorModule.cs
    /// </summary>
    public class ErrorModule : ApplicationErrorModule
    {

        protected override void OnInitializeErrorHandlingMode()
        {
            // typically you'd load this from a configuration object
            ErrorHandlingMode = ErrorHandlingModes.DeveloperErrorMessage;
            //.Default; .DeveloperErrorMessage; .ApplicationErrorMessage;
        }

        protected override void OnDisplayError(
                                WebErrorHandler errorHandler,
                                ErrorViewModel model)
        {
            var response = HttpContext.Current.Response;

            // Create an arbitrary controller instance
            var controller =
                ViewRenderer.CreateController<GenericController>();

            string html = ViewRenderer.RenderPartialView(
                                          "~/views/shared/Error.cshtml",
                                          model,
                                          controller.ControllerContext);

            HttpContext.Current.Server.ClearError();
            response.TrySkipIisCustomErrors = true;
            response.ClearContent();

            response.StatusCode = 500;
            response.Write(html);
        }
    }

    // *any* controller will do for template
    public class GenericController : Controller
    { }
}