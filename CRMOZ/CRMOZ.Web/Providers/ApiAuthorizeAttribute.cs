using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CRMOZ.Web.Providers
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
            else
            {
                //Setting error message and status fode 403 for unauthorized user
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden) { Content = new StringContent(JsonConvert.SerializeObject(new { Message = "Authorization failed." })), StatusCode = HttpStatusCode.Forbidden };
            }

        }
    }
}