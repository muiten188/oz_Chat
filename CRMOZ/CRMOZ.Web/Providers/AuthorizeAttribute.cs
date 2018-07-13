using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Web;

namespace WebApisTokenAuth
{
    public class CAuthorizeAttribute : AuthorizeAttribute
    {
        //protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        //{

        //    if (!HttpContext.Current.User.Identity.IsAuthenticated)
        //    {
        //        base.HandleUnauthorizedRequest(actionContext);
        //    }
        //    else
        //    {
        //        //Setting error message and status fode 403 for unauthorized user
        //        actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden) { Content = new StringContent(JsonConvert.SerializeObject(new { Message = "Authorization failed."})), StatusCode = HttpStatusCode.Forbidden };
        //    }

        //}
        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            return base.AuthorizeHubConnection(hubDescriptor, request);
        }

        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            return base.AuthorizeHubMethodInvocation(hubIncomingInvokerContext, appliesToMethod);
        }
    }
}