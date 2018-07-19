using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace CRMOZ.Web.Fcm
{
    public class FcmServer
    {
        public void pushNotification(string deviceId,string txttitle, string txtmsg)
        {
            string serverKey = "AAAAreJXXWQ:APA91bEmoEKQxcxZ02AqWBkeonvKCH5ceZbbsP_wV6EuCHVLb-ByPS0OfEsVt0I7bx1bdL1WfNONCm4WD6qYv3tgAmIZOqYSW3t5xvkV03sQIOr5zsttkCEbCRSi2Dq_Cp7dc4E0f6foWjCiQfRAqGVt-wqm8VFNJA";

            try
            {
                var result = "-1";
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                httpWebRequest.Method = "POST";
                oFcm oFcm = new oFcm();
                oFcm.to = deviceId;
                oFcm.notification.title = txttitle;
                oFcm.notification.body = txtmsg;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(oFcm);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                // return result;
            }
            catch (Exception ex)
            {
                //  Response.Write(ex.Message);
            }
        }
    }
}