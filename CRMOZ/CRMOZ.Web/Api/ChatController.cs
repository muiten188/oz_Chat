using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CRMOZ.Web.Api
{
    [RoutePrefix("api/chat")]
    public class ChatController : ApiControllerBase
    {
        private static IHubContext hubContext;
        ChatHub chatHub;
        public ChatController()
        {
            hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            chatHub = new ChatHub();
        }

        [Route("getuser")]
        [HttpGet]
        public void GetAllUser()
        {
            //chatHub.GetAllUser();
        }
    }
}
