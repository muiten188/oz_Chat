using CRMOZ.Data;
using CRMOZ.Model.Models;
using CRMOZ.Web.Common;
using CRMOZ.Web.Extensions;
using CRMOZ.Web.Fcm;
using CRMOZ.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using WebApisTokenAuth;

namespace CRMOZ.Web
{
    [CAuthorize]
    public class ChatHub : Hub
    {
        public static FcmServer fcmServer = new FcmServer();
        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;
            string id = Context.ConnectionId;
            UserConnect(id, name);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;
            string id = Context.ConnectionId;

            UserDisConnect(id, name);
            return base.OnDisconnected(stopCalled);
        }

        public void addDeviceTokenFCM(string deviceToken)
        {
            string id = Context.User.Identity.GetUserId();
            string name = Context.User.Identity.Name;
            using (var db = new OZChatDbContext())
            {
                var hubUser = db.HubUsers.FirstOrDefault(p => p.UserName == name);
                if (hubUser!=null&&db.FcmConnection.Where(p => p.DeviceToken == deviceToken)==null)
                {
                    FcmConnection oFcmConnection = new FcmConnection();
                    oFcmConnection.DeviceToken = deviceToken;
                    oFcmConnection.UserID = hubUser.ID;
                    db.FcmConnection.Add(oFcmConnection);
                    db.SaveChanges();
                }
            }
        }

        // Hàm xử lý nếu có user connect
        private void UserConnect(string connectId, string name)
        {
            using (var db = new OZChatDbContext())
            {
                var user = db.HubUsers.FirstOrDefault(p => p.UserName == name);
                Connection oConnection = new Connection();
                if (user.Connected != true)
                {
                    Clients.Others.connect(connectId, name, user.FullName,user.ID);
                    oConnection.UserID = user.ID;
                    oConnection.ConnectionID = connectId;
                    db.Connection.Add(oConnection);
                    user.Connected = true;
                    db.SaveChanges();
                    CommonStatic.AddOnlineUser(user);
                }
                else
                {
                    Clients.Others.connect(connectId, name, user.FullName, user.ID);
                    oConnection.UserID = user.ID;
                    oConnection.ConnectionID = connectId;
                    db.Connection.Add(oConnection);
                    db.SaveChanges();
                    CommonStatic.AddOnlineUser(user);
                }
                GetAllGroup();
                GetAllUser();
                GetAllMessageUser();
                JoinRoom();
            }
        }

        // Hàm xử lý nếu có user disconnect
        private void UserDisConnect(string connectId, string name)
        {
            using (var db = new OZChatDbContext())
            {
                var user = db.HubUsers.FirstOrDefault(p => p.UserName == name);
                if (user.Connected == true)
                {
                    Clients.Others.disConnect(name, user.FullName, user.ID);
                    var listConnection = db.Connection.Where(p => p.UserID == user.ID).ToList();
                    Boolean isRemoveConnection = false;
                    for(var i=0;i< listConnection.Count(); i++)
                    {
                        if (listConnection[i].ConnectionID == connectId)
                        {
                            db.Connection.Remove(listConnection[i]);
                            isRemoveConnection = true;
                        }
                    }
                    if(isRemoveConnection==true&&listConnection.Count==1)
                    {
                        user.Connected = false;
                        
                    }
                    db.SaveChanges();
                    if (isRemoveConnection == true && listConnection.Count == 1)
                    {
                        CommonStatic.RemoveOnlineUser(user.ID);
                    }
                   
                    CommonStatic.RemoveInteractives(user.ID);
                    CommonStatic.RemoveInteracGroup(user.ID);
                }
            }
            //LeaveRoom();
        }

        //// Hàm load toàn bộ user
        public void GetAllUser()
        {
            string id = Context.User.Identity.GetUserId();
            using (var db = new OZChatDbContext())
            {
                var users = db.HubUsers.Where(p => p.ID != id).ToList();
                Clients.Caller.alluser(users);
            }
        }

        ////Hàm lấy danh sách các user đã từng chat với bạn
        public void GetAllMessageUser()
        {
            try
            {
                int count = 0;
                string id = Context.User.Identity.GetUserId();
                List<HubUserViewModel> listHubUser = new List<HubUserViewModel>();
                using (var db = new OZChatDbContext())
                {
                    var recieveUsers = db.UserMessagePrivates.Where(p => p.RecieveUserID == id);
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
                        if (user != null && listHubUser.FirstOrDefault(p => p.ID == user.ID) == null)
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
                Clients.Caller.allMessageUser(listHubUser, count);
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

        ////Hàm lấy danh sách các user đã từng chat với bạn
        public void GetAllMessageUser(string receivedID,List<Connection> listConnection)
        {
            int count = 0;
            string id = Context.User.Identity.GetUserId();
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

                    count += item.NewMessage+1;
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
            for(var i = 0; i < listConnection.Count(); i++)
            {
                Clients.Client(listConnection[i].ConnectionID).allMessageUser(listHubUser, count);
            }
            
        }

        // ------------------- COMMON -----------------------

        //Load ra toàn bộ danh sách những user đã từng
        // nhắn tin với mình
        public void loadAllMessage()
        {
            GetAllMessageUser();
        }

        //Load ra toàn bộ danh sách các user
        public void loadAllContact()
        {
            GetAllUser();
        }

        // Load ra toàn bộ các nhóm mình mà là thành viên
        public void loadAllGroup()
        {
            GetAllGroup();
        }

        // ------------  PRIVATE MESSAGE --------------------

        // Hàm lấy ra tất cả tin nhắn giữa bạn và 1 user nào đó
        public async Task getAllMessagePrivate(string userId)
        {
            var id = Context.User.Identity.GetUserId();
            var connectId = Context.ConnectionId;
            using (var db = new OZChatDbContext())
            {
                var messagePrivates = db.MessagePrivates
                    .Where(p => (p.FromID == id && p.ReceiveID == userId) || p.FromID == userId && p.ReceiveID == id).OrderByDescending(p => p.ID).Take(50).ToList();

                await Clients.Caller.messagePrivates(messagePrivates.OrderBy(p => p.ID).ToList(), id);

                var userMessage = db.UserMessagePrivates.FirstOrDefault(p => p.FromUserID == userId && p.RecieveUserID == id);
                if (userMessage != null)
                {
                    Clients.Caller.devCountMessagePrivate(userId, userMessage.NewMessage);
                    userMessage.NewMessage = 0;
                    db.SaveChanges();
                }

                //Kiểm tra user bạn muốn tương tác có đang online không
                var onlineUser = CommonStatic.OnlineUsers.FirstOrDefault(p => p.ID == userId);
                CommonStatic.RemoveInteractives(id);
                if (onlineUser != null)
                {
                    //Nếu có thì thêm vào danh sách tương tác
                    Clients.Caller.userInteractive(userId, true);
                    CommonStatic.AddInteractive(id, userId);
                }
            }
            removeInteracGroup();
        }

        public void addMessagePrivate(string userId, string message)
        {
            string id = Context.User.Identity.GetUserId();
            string fullname = Context.User.Identity.GetUserFullName();
            string avatar = Context.User.Identity.GetUserAvatar();
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
                    IsFile = false
                };

                //Lấy ra user trong list user đang online
                var onlineUser = CommonStatic.OnlineUsers.FirstOrDefault(p => p.ID == userId);
                //Nếu user đang online
                var listConnectionReceive = db.Connection.Where(p => p.UserID == userId).ToList();
                var listConnectionSend = db.Connection.Where(p => p.UserID == id).ToList();
                if (onlineUser != null)
                {
                    // Nếu user đang online và đang tương tác với mình
                    if (CommonStatic.Interactives.FirstOrDefault(p => p.FromID == userId && p.ReceiveID == id) != null)
                    {
                        for(int i = 0; i < listConnectionSend.Count(); i++)
                        {
                            Clients.Client(listConnectionSend[i].ConnectionID).messagePrivate(messagePrivate, true);
                        }
                        
                        for(int i = 0; i < listConnectionReceive.Count; i++)
                        {
                            Clients.Client(listConnectionReceive[i].ConnectionID).messagePrivate(messagePrivate, false);
                        }
                        AddUserMessagePrivate(id, userId, false, listConnectionReceive);
                    }
                    // Nếu user đang online nhưng không tương tác với mình.
                    else
                    {
                        for (int i = 0; i < listConnectionSend.Count(); i++)
                        {
                            Clients.Client(listConnectionSend[i].ConnectionID).messagePrivate(messagePrivate, true);
                        }
                        for (int i = 0; i < listConnectionReceive.Count; i++)
                        {
                            Clients.Client(listConnectionReceive[i].ConnectionID).notificationMessage(fullname, message);
                        }
                        List<FcmConnection> listFcmConnection = db.FcmConnection.Where(p => p.UserID == userId).ToList();
                        for(var i=0;i< listFcmConnection.Count; i++)
                        {
                            FcmConnection oFcmConnection = listFcmConnection[i];
                            fcmServer.pushNotification(oFcmConnection.DeviceToken, fullname, message);
                        }
                        AddUserMessagePrivate(id, userId, true, listConnectionReceive);
                    }
                }
                // Người mình nhắn tin đang ko online.
                else
                {
                    for (int i = 0; i < listConnectionSend.Count(); i++)
                    {
                        Clients.Client(listConnectionSend[i].ConnectionID).messagePrivate(messagePrivate, true);
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
                //GetAllMessageUser(userId,listConnectionReceive);
            }
        }

        private void AddUserMessagePrivate(string fromUserId, string recieveId, bool isNew,List<Connection> listConnection)
        {
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
                        for(var i = 0; i < listConnection.Count(); i++)
                        {
                            Clients.Client(listConnection[i].ConnectionID).addCountMessagePrivate(fromUserId);
                        }
                        
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
                        GetAllMessageUser(recieveId,listConnection);
                    }

                }
                else
                {
                    // Nếu 2 user đã từng nhắn tin với nhau và đang không tương tác với nhau
                    if (isNew == true)
                    {
                        for(int i= 0; i < listConnection.Count(); i++)
                        {
                            Clients.Client(listConnection[i].ConnectionID).addCountMessagePrivate(fromUserId);
                        }
                        GetAllMessageUser();
                        GetAllMessageUser(recieveId, listConnection);
                        user.NewMessage += 1;
                        db.SaveChanges();
                    }
                }
            }
        }

        // -------- GROUP ----------------

        //Hàm thêm mới 1 nhóm
        private Group createGroup(string groupName)
        {
            string userId = Context.User.Identity.GetUserId();
            using (var _db = new OZChatDbContext())
            {
                var newGroup = new Group { Name = groupName };
                _db.Groups.Add(newGroup);

                var groupUser = new HubUserGroup { GroupID = newGroup.ID, UserID = userId, IsCreater = true };
                _db.HubUserGroups.Add(groupUser);
                _db.SaveChanges();

                return newGroup;
            }
        }

        //Thêm tin nhắn vào group
        public async Task addMessageToGroup(int groupId, string groupName, string message)
        {
            var userId = Context.User.Identity.GetUserId();
            var username = Context.User.Identity.Name;
            var fullname = Context.User.Identity.GetUserFullName();
            var avatar = Context.User.Identity.GetUserAvatar();
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
                    IsFile = false
                };

                // Gửi tin nhắn tới nhóm
                await Clients.OthersInGroup(groupName).groupMessage(newMessage, false);
                await Clients.Caller.groupMessage(newMessage, true);

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
                            for(int i = 0; i < listConnection.Count(); i++)
                            {
                                Clients.Client(listConnection[i].ConnectionID).addCountMessageGroup(groupId);
                            }
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

        // Hàm lấy ra toàn bộ nhóm mà user là thành viên
        private void GetAllGroup()
        {
            int totalMessage = 0;
            using (var db = new OZChatDbContext())
            {
                string userId = Context.User.Identity.GetUserId();
                var groupUser = db.HubUserGroups.Where(p => p.UserID == userId).Include(p => p.Group).ToList();
                if (groupUser != null)
                {
                    List<GroupViewModel> groups = new List<GroupViewModel>();
                    foreach (var item in groupUser)
                    {
                        var group = item.Group;
                        var newMessageGroup = db.NewMessageGroups.FirstOrDefault(p => p.GroupID == group.ID && p.UserID == userId);
                        if (newMessageGroup != null)
                        {
                            groups.Add(new GroupViewModel { ID = group.ID, Name = group.Name, Count = newMessageGroup.Count });
                            totalMessage += newMessageGroup.Count;
                        }
                        else
                        {
                            groups.Add(new GroupViewModel { ID = group.ID, Name = group.Name, Count = 0 });
                        }
                    }
                    Clients.Caller.allGroup(groups, totalMessage);
                }
            }
        }

        //Hàm lấy ra toàn bộ tin nhắn của 1 nhóm
        public async Task getAllGroupMessage(int groupId)
        {
            var userId = Context.User.Identity.GetUserId();
            using (var db = new OZChatDbContext())
            {
                var messages = db.MessageGroups.Where(p => p.GroupID == groupId).OrderByDescending(p => p.ID).Take(50).ToList();
                await Clients.Caller.allGroupMessage(messages.OrderBy(p => p.ID).ToList(), userId);

                var newMessage = db.NewMessageGroups.FirstOrDefault(p => p.UserID == userId && p.GroupID == groupId);
                if (newMessage != null)
                {
                    await Clients.Caller.devCountMessageGroup(groupId, newMessage.Count);
                    db.NewMessageGroups.Remove(newMessage);
                    db.SaveChanges();
                }
            }
            // Xóa tương tác cũ sau đó thêm tương tác mới
            CommonStatic.RemoveInteracGroup(userId);
            CommonStatic.AddInteracGroup(userId, groupId);
            removeInteracPrivate();
        }

        //Hàm lấy ra user 
        public async Task getUserForGroup()
        {
            await GetUserOutGroup();
        }

        //Hàm lấy ra tất cả thành viên của nhóm
        public async Task getListUserInGroup(int groupId)
        {
            await GetUserOfGroup(groupId);
        }

        //Thêm thành viên vào nhóm
        public async Task addUserGroup(List<string> listChecked, string groupName)
        {
            // Thêm nhóm
            if (!string.IsNullOrEmpty(groupName))
            {
                Group group = null;
                using (var _db = new OZChatDbContext())
                {
                    // Kiem tra group da ton tai chua
                    group = _db.Groups.FirstOrDefault(p => p.Name == groupName);

                    // Neu chua thi tao moi
                    if (group == null)
                    {
                        group = createGroup(groupName);
                        // Thêm danh sách user đc chọn vào nhóm
                        foreach (var item in listChecked)
                        {
                            var user = _db.Users.FirstOrDefault(p => p.UserName == item);

                            if (user != null)
                            {
                                var groupUser = _db.HubUserGroups.FirstOrDefault(p => p.GroupID == group.ID && p.UserID == user.Id);
                                if (groupUser == null)
                                {
                                    _db.HubUserGroups.Add(new HubUserGroup { GroupID = group.ID, UserID = user.Id, IsCreater = false });
                                }
                            }
                        }

                        Clients.Caller.createGroupSuccess(group.Name, group.ID);
                        await Clients.Caller.alertMessage("Tạo nhóm mới thành công!", true);
                        _db.SaveChanges();
                    }
                    else
                    {
                        await Clients.Caller.alertMessage("Nhóm " + groupName + " đã tồn tại!", false);
                    }
                }
            }
        }

        //Cập nhật thành viên của nhóm
        public async Task updateUserGroup(List<string> listChecked, string groupName)
        {
            if (!string.IsNullOrEmpty(groupName))
            {
                Group group = null;
                using (var _db = new OZChatDbContext())
                {
                    // Kiem tra group da ton tai chua
                    group = _db.Groups.FirstOrDefault(p => p.Name == groupName);

                    // Neu chua thi tao moi
                    if (group == null)
                    {
                        await Clients.Caller.alertMessage("Nhóm " + groupName + " không tồn tại!", false);
                    }
                    else
                    {
                        // Thêm danh sách user đc chọn vào nhóm
                        foreach (var item in listChecked)
                        {
                            var user = _db.Users.FirstOrDefault(p => p.UserName == item);

                            if (user != null)
                            {
                                var groupUser = _db.HubUserGroups.FirstOrDefault(p => p.GroupID == group.ID && p.UserID == user.Id);
                                if (groupUser == null)
                                {
                                    _db.HubUserGroups.Add(new HubUserGroup { GroupID = group.ID, UserID = user.Id, IsCreater = false });
                                }
                            }
                        }
                        _db.SaveChanges();
                        await GetUserOfGroup(group.ID);
                        await Clients.Caller.alertMessage("Thêm mới thành viên thành công!", true);
                    }
                }
            }
        }

        // Xóa thành viên khỏi nhóm
        public async Task removeUserFromGroup(string userId, int groupId, string groupName)
        {
            string id = Context.User.Identity.GetUserId();
            using (var db = new OZChatDbContext())
            {
                var groupUser = db.HubUserGroups.FirstOrDefault(p => p.UserID == userId && p.GroupID == groupId);
                if (groupUser != null)
                {
                    var creater = db.HubUserGroups.FirstOrDefault(p => p.UserID == id && p.GroupID == groupId);
                    if (creater.IsCreater == true)
                    {
                        db.HubUserGroups.Remove(groupUser);
                        db.SaveChanges();
                        await Clients.Caller.alertMessage("Thành viên đã được xóa khỏi nhóm!", true);
                        await GetUserOfGroup(groupId);
                    }
                    else
                    {
                        await Clients.Caller.alertMessage("Người tạo nhóm mới có quyền xóa!", false);
                    }
                }
            }
        }

        //Hàm rời khỏi nhóm
        public async Task outFromGroup(int groupId, string groupName)
        {
            string userId = Context.User.Identity.GetUserId();
            using (var db = new OZChatDbContext())
            {
                var userGroup = db.HubUserGroups.FirstOrDefault(p => p.UserID == userId && p.GroupID == groupId);
                if (userGroup != null)
                {
                    db.HubUserGroups.Remove(userGroup);
                    db.SaveChanges();
                    await Clients.Caller.successOutGroup(groupId, "Bạn đã rời khỏi nhóm " + groupName);
                }
                else
                {
                    await Clients.Caller.alertMessage("Nhóm " + groupName + " không tồn tại!", false);
                }
            }
        }

        // Hàm xóa nhóm
        public async Task removeGroup(int groupId, string groupName)
        {
            string userId = Context.User.Identity.GetUserId();
            using (var db = new OZChatDbContext())
            {
                var userGroup = db.HubUserGroups.FirstOrDefault(p => p.UserID == userId && p.GroupID == groupId && p.IsCreater == true);
                if (userGroup != null)
                {
                    var listUserGroup = db.HubUserGroups.Where(p => p.GroupID == groupId).ToList();
                    db.HubUserGroups.RemoveRange(listUserGroup);

                    var listMessGroup = db.MessageGroups.Where(p => p.GroupID == groupId).ToList();
                    db.MessageGroups.RemoveRange(listMessGroup);

                    var group = db.Groups.FirstOrDefault(p => p.ID == groupId);
                    if (group != null)
                    {
                        db.Groups.Remove(group);
                    }
                    db.SaveChanges();
                    await Clients.Caller.successOutGroup(groupId, "Nhóm " + groupName + " đã được xóa!");
                }
                else
                {
                    await Clients.Caller.alertMessage("Chỉ có người tạo nhóm mới được xóa!", false);
                }
            }
        }

        private async Task GetUserOfGroup(int groupId)
        {
            List<UserGroupViewModel> model = new List<UserGroupViewModel>();
            using (var db = new OZChatDbContext())
            {
                string userId = Context.User.Identity.GetUserId();
                var users = db.HubUsers.Select(p => new { p.UserName, p.FullName, p.ID }).ToList();

                var userGroups = db.HubUserGroups.Where(p => p.GroupID == groupId);

                foreach (var item in users)
                {
                    var userGroup = userGroups.FirstOrDefault(p => p.UserID == item.ID);
                    if (userGroup == null)
                    {
                        model.Add(new UserGroupViewModel { UserName = item.UserName, FullName = item.FullName, UserID = item.ID, IsChecked = false });
                    }
                    else
                    {
                        model.Add(new UserGroupViewModel { UserName = item.UserName, FullName = item.FullName, UserID = item.ID, IsChecked = true });
                    }
                }
            }
            await Clients.Caller.listUserInGroup(model.OrderByDescending(p => p.IsChecked == true).ToList());
        }

        private async Task GetUserOutGroup()
        {
            List<UserGroupViewModel> model = new List<UserGroupViewModel>();
            using (var db = new OZChatDbContext())
            {
                string userId = Context.User.Identity.GetUserId();
                var users = db.HubUsers.Select(p => new { p.UserName, p.FullName, p.ID })
                    .Where(p => p.ID != userId).ToList();

                foreach (var item in users)
                {
                    model.Add(new UserGroupViewModel { UserName = item.UserName, FullName = item.FullName, UserID = item.ID, IsChecked = false });
                }
            }
            await Clients.Caller.getAllUserForGroup(model);
        }

        public Task JoinRoom()
        {
            string userId = Context.User.Identity.GetUserId();
            using (var db = new OZChatDbContext())
            {
                var groupUsers = db.HubUserGroups.Include(p => p.Group).Where(p => p.UserID != userId).ToList();
                foreach (var item in groupUsers)
                {
                    string roomName = item.Group.Name;
                    Groups.Add(Context.ConnectionId, roomName);
                }
            }
            return null;
        }

        public Task LeaveRoom()
        {
            string userId = Context.User.Identity.GetUserId();
            using (var db = new OZChatDbContext())
            {
                var groupUsers = db.HubUserGroups.Include(p => p.Group).Where(p => p.UserID != userId).ToList();
                foreach (var item in groupUsers)
                {
                    string roomName = item.Group.Name;
                    Groups.Remove(Context.ConnectionId, roomName);
                }
            }
            return null;
        }

        public void removeInteracGroup()
        {
            string userId = Context.User.Identity.GetUserId();
            CommonStatic.RemoveInteracGroup(userId);
        }
        public void removeInteracPrivate()
        {
            string userId = Context.User.Identity.GetUserId();
            Clients.Caller.userInteractive(userId, false);
            CommonStatic.RemoveInteractives(userId);
        }
    }
}