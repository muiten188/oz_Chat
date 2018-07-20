using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMOZ.Web.Fcm
{
    public class oFcm
    {
        public string to;
        public notificationCustom data = new notificationCustom();
    }

    public class _notification
    {
        public string title;
        public string body;
        public string color = "#00ACD4";
        public string priority = "high";
        public string group = "GROUP";
        public string sound = "default";
        public Boolean show_in_foreground = true;
        public Guid id;
        public string userID;
        public int groupID;
    }
    public class notificationCustom
    {
        public _notification custom_notification = new _notification();
    }
}