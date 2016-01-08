using System.Web.Mvc;

namespace SSD.Web.Mvc.Exceptions
{
    public class ElmahHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var exceptionHandled = filterContext.ExceptionHandled;

            base.OnException(filterContext);

            // signal ELMAH to log the exception
            //if (!exceptionHandled && filterContext.ExceptionHandled)
            //    ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);
        }
    }
}
