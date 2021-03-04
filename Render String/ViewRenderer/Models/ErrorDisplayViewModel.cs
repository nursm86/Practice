using Westwind.Web;

namespace ViewRendererSamples
{
    public class ErrorDisplayViewModel
    {
        public string Title = string.Empty;
        public string Message = string.Empty;
        public string RedirectTo = string.Empty;
        public int RedirectToTimeout = 10; // seconds
        public bool MessageIsHtml = false;
        public bool IsMessage = false;
        public ErrorHandlingModes ErrorHandlingMode = ErrorHandlingModes.Default;
    }
}
