using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMOZ.Web.Fcm
{
    public class oFcm
    {
        public string to;
        public notification notification=new notification();
    }

    public class notification
    {
        public string title;
        public string body;
    }
}