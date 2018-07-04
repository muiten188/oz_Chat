using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CRMOZ.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //filterContext.Result = new RedirectToRouteResult(new
            //        RouteValueDictionary(new { controller = "Account", action = "Login" }));
            base.OnActionExecuting(filterContext);
        }
    }
}