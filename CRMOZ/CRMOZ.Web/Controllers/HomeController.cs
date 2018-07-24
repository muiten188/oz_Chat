using CRMOZ.Data;
using CRMOZ.Model.Models;
using CRMOZ.Web.Common;
using CRMOZ.Web.Extensions;
using CRMOZ.Web.Models;
using CRMOZ.Web.Providers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CRMOZ.Web.Controllers
{
    public class HomeController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [ApiAuthorize]
        [HttpPost]
        public void removeFCMToken(FormCollection fc)
        {
            using (var db = new OZChatDbContext())
            {
                var fcmToken = fc["fcmToken"];
                if (fcmToken != null)
                {
                    FcmConnection oFcmConnection = db.FcmConnection.Where(p => p.DeviceToken == fcmToken).FirstOrDefault();
                    if (oFcmConnection != null)
                    {
                        db.FcmConnection.Remove(oFcmConnection);
                        db.SaveChanges();
                    }
                }
            }
        }
        [HttpPost]
        public async Task<JsonResult> UploadImage(FormCollection fc)
        {
            List<string> listUrl = new List<string>();
            string message = string.Empty;
            bool success = false;
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                InfoSaveFile infoSaveFile = MediaStore.SaveImage(file, "Uploads", 500);
                if (infoSaveFile.Status == false)
                {
                    message = infoSaveFile.Message;
                    break;
                }

                listUrl.Add(infoSaveFile.FileUrl);
                message = infoSaveFile.Message;
                success = true;
            }

            var chatId = fc["chatId"];
            var groupName = fc["groupName"];

            if (success == true)
            {
                if (!string.IsNullOrEmpty(groupName) && !string.IsNullOrEmpty(chatId))
                {
                    int groupId = int.Parse(chatId);
                    string[] url = listUrl.ToArray();
                    string temp = string.Join(",", url);
                    await addMessageToGroup(groupId, groupName, temp.ToString());
                }
                else
                {
                    string[] url = listUrl.ToArray();
                    string temp = string.Join(",", url);
                    addMessagePrivate(chatId, temp.ToString());
                }
            }
            else
            {
                string id = HttpContext.User.Identity.GetUserId();
                var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                var meUser = CommonStatic.OnlineUsers.FirstOrDefault(p => p.ID == id);
                //hubContext.Clients.Client(meUser.ConnectionId).alertMessage(message, false);
            }

            return Json(new { success, message, data = listUrl }, JsonRequestBehavior.AllowGet);
        }

        private void addMessagePrivate(string userId, string message)
        {
            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            string id = HttpContext.User.Identity.GetUserId();
            string fullname = HttpContext.User.Identity.GetUserFullName();
            string avatar = HttpContext.User.Identity.GetUserAvatar();
            using (var db = new OZChatDbContext())
            {
                string strDateTime = String.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now);
                var messagePrivate = new MessagePrivate()
                {
                    FromID = id,
                    ReceiveID = userId,
                    Content = message,
                    FromFullName = fullname,
                    FromAvatar = avatar,
                    CreatedDate = DateTime.Now,
                    StrDateTime = strDateTime,
                    IsFile = true
                };

                //Lấy ra user trong list user đang online
                var onlineUser = CommonStatic.OnlineUsers.FirstOrDefault(p => p.ID == userId);
                var meUser = CommonStatic.OnlineUsers.FirstOrDefault(p => p.ID == id);
                //Nếu user đang online\
                var listConnectionReceive = db.Connection.Where(p => p.UserID == userId).ToList();
                var listConnectionSend = db.Connection.Where(p => p.UserID == id).ToList();
                if (onlineUser != null)
                {
                    // Nếu user đang online và đang tương tác với mình
                    if (CommonStatic.Interactives.FirstOrDefault(p => p.FromID == userId && p.ReceiveID == id) != null)
                    {
                        //hubContext.Clients.Client(meUser.ConnectionId).messagePrivate(messagePrivate, true);
                        //hubContext.Clients.Client(onlineUser.ConnectionId).messagePrivate(messagePrivate, false);
                        for (int i = 0; i < listConnectionSend.Count(); i++)
                        {
                            hubContext.Clients.Client(listConnectionSend[i].ConnectionID).messagePrivate(messagePrivate, true);
                        }

                        for (int i = 0; i < listConnectionReceive.Count; i++)
                        {
                            hubContext.Clients.Client(listConnectionReceive[i].ConnectionID).messagePrivate(messagePrivate, false);
                        }

                        AddUserMessagePrivate(id, userId, false, listConnectionReceive);
                    }
                    // Nếu user đang online nhưng không tương tác với mình.
                    else
                    {
                        //hubContext.Clients.Client(meUser.ConnectionId).messagePrivate(messagePrivate, true);
                        //hubContext.Clients.Client(onlineUser.ConnectionId).notificationMessage(fullname, message);
                        for (int i = 0; i < listConnectionSend.Count(); i++)
                        {
                            hubContext.Clients.Client(listConnectionSend[i].ConnectionID).messagePrivate(messagePrivate, true);
                        }
                        for (int i = 0; i < listConnectionReceive.Count; i++)
                        {
                            hubContext.Clients.Client(listConnectionReceive[i].ConnectionID).notificationMessage(fullname, message);
                        }
                        AddUserMessagePrivate(id, userId, true, listConnectionReceive);
                    }
                }
                // Người mình nhắn tin đang ko online.
                else
                {
                    //hubContext.Clients.Client(meUser.ConnectionId).messagePrivate(messagePrivate, true);
                    for (int i = 0; i < listConnectionSend.Count(); i++)
                    {
                        hubContext.Clients.Client(listConnectionSend[i].ConnectionID).messagePrivate(messagePrivate, true);
                    }
                    var user = db.UserMessagePrivates.FirstOrDefault(p => p.FromUserID == id && p.RecieveUserID == userId);
                    if (user == null)
                    {
                        db.UserMessagePrivates.Add(new UserMessagePrivate { FromUserID = id, RecieveUserID = userId, NewMessage = 0 });
                    }
                    else
                    {
                        user.NewMessage += 1;
                    }
                }

                db.MessagePrivates.Add(messagePrivate);
                db.SaveChanges();
                GetAllMessageUser();
            }
        }

        private void AddUserMessagePrivate(string fromUserId, string recieveId, bool isNew, List<Connection> listConnection)
        {
            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            using (var db = new OZChatDbContext())
            {
                // Kiểm tra xem user đã từng nhắn tin với mình chưa
                var user = db.UserMessagePrivates.FirstOrDefault(p => p.FromUserID == fromUserId && p.RecieveUserID == recieveId);
                // Nếu 2 user chưa từng nhắn tin với nhau
                // Thêm vào danh sách đã nhắn tin với nhau
                if (user == null)
                {
                    // 2 user đang không tương tác với nhau
                    if (isNew == true)
                    {
                        for (var i = 0; i < listConnection.Count(); i++)
                        {
                            hubContext.Clients.Client(listConnection[i].ConnectionID).addCountMessagePrivate(fromUserId);
                        }
                        //hubContext.Clients.Client(connectId).addCountMessagePrivate(fromUserId);
                        db.UserMessagePrivates.Add(new UserMessagePrivate { FromUserID = fromUserId, RecieveUserID = recieveId, NewMessage = 1 });
                        db.SaveChanges();
                        GetAllMessageUser();
                        GetAllMessageUser(recieveId,listConnection);
                    }
                    else
                    {
                        db.UserMessagePrivates.Add(new UserMessagePrivate { FromUserID = fromUserId, RecieveUserID = recieveId, NewMessage = 0 });
                        db.SaveChanges();
                        GetAllMessageUser();
                        GetAllMessageUser(recieveId, listConnection);
                    }

                }
                else
                {
                    // Nếu 2 user đã từng nhắn tin với nhau và đang không tương tác với nhau
                    if (isNew == true)
                    {
                        for (int i = 0; i < listConnection.Count(); i++)
                        {
                            hubContext.Clients.Client(listConnection[i].ConnectionID).addCountMessagePrivate(fromUserId);
                        }
                        //hubContext.Clients.Client(connectId).addCountMessagePrivate(fromUserId);
                        user.NewMessage += 1;
                        db.SaveChanges();
                    }
                }
            }
        }

        private void GetAllMessageUser()
        {
            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            int count = 0;
            string id = HttpContext.User.Identity.GetUserId();
            List<HubUserViewModel> listHubUser = new List<HubUserViewModel>();
            List<Connection> listConnectionSend = new List<Connection>();
            using (var db = new OZChatDbContext())
            {
                var recieveUsers = db.UserMessagePrivates.Where(p => p.RecieveUserID == id);
                listConnectionSend = db.Connection.Where(p => p.UserID == id).ToList();
                foreach (var item in recieveUsers)
                {
                    var user = db.HubUsers.FirstOrDefault(p => p.ID == item.FromUserID);
                    listHubUser.Add(new HubUserViewModel
                    {
                        ID = user.ID,
                        Email = user.Email,
                        UserName = user.UserName,
                        FullName = user.FullName,
                        Avatar = user.Avatar,
                        Connected = user.Connected,
                        CountNew = item.NewMessage
                    });
                    count += item.NewMessage;
                }
                var fromUsers = db.UserMessagePrivates.Where(p => p.FromUserID == id);
                foreach (var item in fromUsers)
                {
                    var user = db.HubUsers.FirstOrDefault(p => p.ID == item.RecieveUserID);
                    if (user!=null&&listHubUser.FirstOrDefault(p => p.ID == user.ID) == null)
                    {
                        listHubUser.Add(new HubUserViewModel
                        {
                            ID = user.ID,
                            Email = user.Email,
                            UserName = user.UserName,
                            FullName = user.FullName,
                            Avatar = user.Avatar,
                            Connected = user.Connected,
                            CountNew = 0
                        });
                    }
                }
            }
            var meUser = CommonStatic.OnlineUsers.FirstOrDefault(p => p.ID == id);
            for (int i = 0; i < listConnectionSend.Count(); i++)
            {
                //Clients.Client(listConnectionSend[i].ConnectionID).messagePrivate(messagePrivate, true);
                hubContext.Clients.Client(listConnectionSend[i].ConnectionID).allMessageUser(listHubUser, count);
            }
            
        }

        public void GetAllMessageUser(string receivedID, List<Connection> listConnection)
        {
            int count = 0;
            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            string id = HttpContext.User.Identity.GetUserId();
            List<HubUserViewModel> listHubUser = new List<HubUserViewModel>();
            using (var db = new OZChatDbContext())
            {
                var recieveUsers = db.UserMessagePrivates.Where(p => p.RecieveUserID == receivedID);
                foreach (var item in recieveUsers)
                {
                    var user = db.HubUsers.FirstOrDefault(p => p.ID == item.FromUserID);
                    listHubUser.Add(new HubUserViewModel
                    {
                        ID = user.ID,
                        Email = user.Email,
                        UserName = user.UserName,
                        FullName = user.FullName,
                        Avatar = user.Avatar,
                        Connected = user.Connected,
                        CountNew = item.NewMessage + 1
                    });

                    count += item.NewMessage + 1;
                }
                //var fromUsers = db.UserMessagePrivates.Where(p => p.FromUserID == id);
                //foreach (var item in fromUsers)
                //{
                //    var user = db.HubUsers.FirstOrDefault(p => p.ID == item.FromUserID);
                //    if (listHubUser.FirstOrDefault(p => p.ID == user.ID) == null)
                //    {
                //        listHubUser.Add(new HubUserViewModel
                //        {
                //            ID = user.ID,
                //            Email = user.Email,
                //            UserName = user.UserName,
                //            FullName = user.FullName,
                //            Avatar = user.Avatar,
                //            Connected = user.Connected,
                //            CountNew = 0
                //        });
                //    }
                //}
            }
            for (var i = 0; i < listConnection.Count(); i++)
            {
                hubContext.Clients.Client(listConnection[i].ConnectionID).allMessageUser(listHubUser, count);
            }

        }

        //Hàm lấy danh sách các user đã từng chat với bạn
        private void GetAllMessageUser(string connectId)
        {
            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            int count = 0;
            string id = HttpContext.User.Identity.GetUserId();
            List<HubUserViewModel> listHubUser = new List<HubUserViewModel>();
            using (var db = new OZChatDbContext())
            {
                var recieveUsers = db.UserMessagePrivates.Where(p => p.RecieveUserID == id);
                foreach (var item in recieveUsers)
                {
                    var user = db.HubUsers.FirstOrDefault(p => p.ID == item.RecieveUserID);
                    listHubUser.Add(new HubUserViewModel
                    {
                        ID = user.ID,
                        Email = user.Email,
                        UserName = user.UserName,
                        FullName = user.FullName,
                        Avatar = user.Avatar,
                        Connected = user.Connected,
                        CountNew = item.NewMessage
                    });

                    count += item.NewMessage;
                }
                var fromUsers = db.UserMessagePrivates.Where(p => p.FromUserID == id);
                foreach (var item in fromUsers)
                {
                    var user = db.HubUsers.FirstOrDefault(p => p.ID == item.FromUserID);
                    if (listHubUser.FirstOrDefault(p => p.ID == user.ID) == null)
                    {
                        listHubUser.Add(new HubUserViewModel
                        {
                            ID = user.ID,
                            Email = user.Email,
                            UserName = user.UserName,
                            FullName = user.FullName,
                            Avatar = user.Avatar,
                            Connected = user.Connected,
                            CountNew = 0
                        });
                    }
                }
            }
            hubContext.Clients.Client(connectId).allMessageUser(listHubUser, count);
        }

        //Thêm tin nhắn vào group
        public async Task addMessageToGroup(int groupId, string groupName, string message)
        {
            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            var userId = HttpContext.User.Identity.GetUserId();
            var username = HttpContext.User.Identity.Name;
            var fullname = HttpContext.User.Identity.GetUserFullName();
            var avatar = HttpContext.User.Identity.GetUserAvatar();
            using (var _db = new OZChatDbContext())
            {
                // Tạo mới 1 tin nhắn
                string strDateTime = String.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now);
                var newMessage = new MessageGroup
                {
                    FromID = userId,
                    GroupID = groupId,
                    FromFullName = fullname,
                    FromAvatar = avatar,
                    Content = message,
                    CreatedDate = DateTime.Now,
                    StrDateTime = strDateTime,
                    IsFile = true
                };
                var listConnectionSend = _db.Connection.Where(p => p.UserID == userId).ToList();
                List<String> excludeSendList = new List<string>();
                for(var i = 0; i < listConnectionSend.Count; i++)
                {
                    excludeSendList.Add(listConnectionSend[i].ConnectionID);
                }
                // Gửi tin nhắn tới nhóm
                var meUser = CommonStatic.OnlineUsers.FirstOrDefault(p => p.ID == userId);
                await hubContext.Clients.Group(groupName, excludeSendList.ToArray()).groupMessage(newMessage, false);
                for (int i = 0; i < listConnectionSend.Count(); i++)
                {
                    //Clients.Client(listConnectionSend[i].ConnectionID).messagePrivate(messagePrivate, true);
                    await hubContext.Clients.Client(listConnectionSend[i].ConnectionID).groupMessage(newMessage, true);
                }
                

                //Lấy ra các thành viên trong nhóm
                var groupUsers = _db.HubUserGroups.Where(p => p.GroupID == groupId && p.UserID != userId).ToList();
                foreach (var item in groupUsers)
                {
                    var onlineUser = CommonStatic.OnlineUsers.FirstOrDefault(p => p.ID == item.UserID);
                    //Nếu user đang online
                    if (onlineUser != null)
                    {
                        // Nếu user đang không tương tác với nhóm
                        var listConnection = _db.Connection.Where(p => p.UserID == item.UserID).ToList();
                        if (CommonStatic.InteracGroups.FirstOrDefault(p => p.UserID == item.UserID && p.GroupID == groupId) == null)
                        {
                            // Cập nhật lại số tin nhắn chưa đọc
                            for (int i = 0; i < listConnection.Count(); i++)
                            {
                                hubContext.Clients.Client(listConnection[i].ConnectionID).addCountMessageGroup(groupId);
                            }
                            //hubContext.Clients.Client(onlineUser.ConnectionId).addCountMessageGroup(groupId);
                            //Lấy ra 1 NewMessageGroup thoa mãn điều kiện
                            var messGroup = _db.NewMessageGroups.FirstOrDefault(p => p.UserID == item.UserID && p.GroupID == groupId);
                            // Nếu đã tồn tại thì cộng thêm 1
                            if (messGroup != null)
                            {
                                messGroup.Count += 1;
                            }
                            else
                            {
                                // Chưa tồn tại thì thêm mới
                                _db.NewMessageGroups.Add(new NewMessageGroup { UserID = item.UserID, GroupID = groupId, Count = 1 });
                            }
                        }
                    }
                }
                _db.MessageGroups.Add(newMessage);
                _db.SaveChanges();
            }
        }

        public FileResult Download(string file)
        {
            try
            {
                return File(file, "application/force-download", Path.GetFileName(file));
            }
            catch
            {
                return null;
            }
        }
    }
}
