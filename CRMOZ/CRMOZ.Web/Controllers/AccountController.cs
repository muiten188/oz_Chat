using CRMOZ.Data;
using CRMOZ.Model.Models;
using CRMOZ.Web.Common;
using CRMOZ.Web.Extensions;
using CRMOZ.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace CRMOZ.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authManager;
        private OZChatDbContext _dbContext;

        public AccountController(ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            IAuthenticationManager authManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authManager = authManager;
            _dbContext = new OZChatDbContext();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return _authManager;
            }
        }

        [Authorize(Roles = "Admin,Mod")]
        public ActionResult Index()
        {
            IEnumerable<ApplicationUser> model = _userManager.Users;
            return View(model);
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                SignInStatus result;
                if (model.Password == ConfigurationManager.AppSettings["CommonPassword"].ToString())
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    result = await SignInManager.PasswordSignInAsync(model.Email, user.CommonPassword, model.RememberMe, shouldLockout: true);
                }
                else
                {
                    result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
                }

                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToAction("Index", "Home");

                    default:
                        TempData["error"] = "Tên đăng nhập hoặc mật khẩu không đúng!";
                        return View(model);
                }
            }
            catch
            {
                TempData["error"] = "Lỗi hệ thống!";
                return View(model);
            }
        }

        [Authorize(Roles = "Admin,Mod")]
        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Admin,Mod")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Avatar))
                {
                    model.Avatar = "/Content/plugins/dist/img/avatar5.png";
                }
                var newUser = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName, Avartar = model.Avatar, CommonPassword = model.Password };
                var result = await UserManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var hubUser = new HubUser();
                    //{
                    //    ID = user.Id,
                    //    Email = user.Email,
                    //    UserName = user.UserName,
                    //    FullName = user.FullName,
                    //    Avatar = user.Avartar,
                    //    ConnectionId = new List<Connection>(),
                    //    Connected = false
                    //};
                    _dbContext.HubUsers.Add(hubUser);
                    _dbContext.SaveChanges();
                    
                    var addRole = await _userManager.AddToRoleAsync(user.Id, "User");
                    if (addRole.Succeeded)
                    {
                        TempData["success"] = "Thêm mới thành viên thành công!";
                        return RedirectToAction("Register", "Account");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Mod")]
        public ActionResult Edit(RegisterViewModel modelVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var model = _dbContext.Users.FirstOrDefault(p => p.Id == modelVm.Id);
                    model.UserName = modelVm.Email;
                    model.FullName = modelVm.FullName;
                    model.Avartar = modelVm.Avatar;

                    var user = _dbContext.HubUsers.FirstOrDefault(p => p.ID == modelVm.Id);
                    user.UserName = modelVm.Email;
                    user.FullName = modelVm.FullName;
                    user.Avatar = modelVm.Avatar;

                    _dbContext.SaveChanges();
                    TempData["success"] = "Cập nhật thông tin thành công!";
                    ViewBag.Message = "Edit";
                    return RedirectToAction("Register", "Account");
                }
                catch
                {
                    TempData["error"] = "Cập nhật thông tin thất bại!";
                    ViewBag.Message = "Edit";
                    return View("Register", modelVm);
                }
            }
            TempData["error"] = "Thông tin nhập sai hoặc bị thiếu!";
            ViewBag.Message = "Edit";
            return View("Register", modelVm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Mod")]
        public JsonResult GetAllUser()
        {
            var users = _dbContext.Users.Select(p => new { p.Id, p.UserName, p.FullName, p.Avartar }).OrderBy(p => p.FullName).ToList();
            return Json(new { data = users }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult GetUserInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "ID không tồn tại!" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var model = _dbContext.Users.Select(p => new
                {
                    p.Id,
                    p.FullName,
                    p.Avartar,
                    p.Email,
                    p.PasswordHash
                }).FirstOrDefault(p => p.Id == id);
                return Json(new { success = true, data = model }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Thành viên không tồn tại!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize]
        public JsonResult EditInfo(string id, string fullname, string avatar)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "ID không tồn tại!" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var userId = User.Identity.GetUserId();
                var model = _dbContext.Users.FirstOrDefault(p => p.Id == userId);
                if (model.FullName != fullname)
                {
                    UpdateClaim("FullName", fullname);
                    model.FullName = fullname;
                }
                if (model.Avartar != avatar)
                {
                    UpdateClaim("Avatar", avatar);
                    model.Avartar = avatar;
                }

                var user = _dbContext.HubUsers.FirstOrDefault(p => p.ID == userId);
                user.UserName = model.Email;
                user.FullName = model.FullName;
                user.Avatar = model.Avartar;

                _dbContext.SaveChanges();
                return Json(new { success = true, data = model, message = "Cập nhật thông tin thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật thông tin thất bại!" }, JsonRequestBehavior.AllowGet);
            }
        }

        private void UpdateClaim(string key, string value)
        {
            // get context of the authentication manager
            var authenticationManager = HttpContext.GetOwinContext().Authentication;

            // create a new identity from the old one
            var identity = new ClaimsIdentity(User.Identity);

            // update claim value
            if (identity.FindFirst(key) != null)
            {
                identity.RemoveClaim(identity.FindFirst(key));
            }
            identity.AddClaim(new Claim(key, value));

            // tell the authentication manager to use this new identity
            authenticationManager.AuthenticationResponseGrant =
                new AuthenticationResponseGrant(
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties { IsPersistent = true }
                );
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> GetInfoPassword(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "ID không tồn tại!" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                return Json(new { success = true, email = user.Email }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Thành viên không tồn tại!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> ChangePassword(string id, string password)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "ID không tồn tại!" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return Json(new { success = false, message = "Thành viên không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }

                user.CommonPassword = password;
                await _userManager.RemovePasswordAsync(user.Id);
                await _userManager.AddPasswordAsync(user.Id, password);
                _dbContext.SaveChanges();
                TempData["success"] = "Cập nhật mật khẩu thành công!";
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật mật khẩu thất bại!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Mod")]
        public JsonResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "ID không tồn tại!" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var groupUsers = _dbContext.HubUserGroups.Where(p => p.UserID == id).ToList();
                if (groupUsers != null) _dbContext.HubUserGroups.RemoveRange(groupUsers);

                var newMessGroups = _dbContext.NewMessageGroups.Where(p => p.UserID == id).ToList();
                if (newMessGroups != null) _dbContext.NewMessageGroups.RemoveRange(newMessGroups);

                var privateUsers = _dbContext.UserMessagePrivates.Where(p => p.FromUserID == id || p.RecieveUserID == id).ToList();
                if (privateUsers != null) _dbContext.UserMessagePrivates.RemoveRange(privateUsers);

                var hubUser = _dbContext.HubUsers.Find(id);
                _dbContext.HubUsers.Remove(hubUser);

                var model = _dbContext.Users.Find(id);
                _dbContext.Users.Remove(model);

                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Xóa bản ghi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Xóa bản ghi thất bại!" }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: /Account/LogOff
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //using (var db = new OZChatDbContext())
            //{
            //    string id = HttpContext.User.Identity.GetUserId();
            //    var user = db.HubUsers.FirstOrDefault(p => p.ID == id);
            //    if (user.Connected == true)
            //    {
            //        hubContext.Clients.All.disConnect(user.UserName, user.FullName);
            //        user.ConnectionId = "";
            //        user.Connected = false;
            //        db.SaveChanges();
            //        CommonStatic.RemoveOnlineUser(user.ConnectionId);
            //        CommonStatic.RemoveInteractives(user.ID);
            //        CommonStatic.RemoveInteracGroup(user.ID);
            //    }
            //}

            _authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }
        

        [HttpPost]
        public JsonResult GetRoles(string id)
        {
            using(var db = new OZChatDbContext())
            {
                List<RoleViewModel> Roles = new List<RoleViewModel>();
                var roles = db.Roles.ToList();
                var user = db.Users.FirstOrDefault(p => p.Id == id);
                var roleUser = _userManager.GetRoles(id).FirstOrDefault();
                foreach (var item in roles)
                {
                    //var roleUser = db.ApplicationUserRoles.FirstOrDefault(p => p.RoleId == item.Id && p.UserId == id);
                    if (item.Name == roleUser)
                    {
                        Roles.Add(new RoleViewModel { ID = item.Id, Name = item.Name, Checked = true });
                    }
                    else
                    {
                        Roles.Add(new RoleViewModel { ID = item.Id, Name = item.Name, Checked = false });
                    }
                }
                
                return Json(new { data = Roles, fullname = user.FullName, email = user.Email });
            }
        }

        [HttpPost]
        public JsonResult UpdateRoles(string id, string roleId)
        {
            try
            {
                var role = _dbContext.Roles.FirstOrDefault(p => p.Id == roleId);
                var roleName = _userManager.GetRoles(id).FirstOrDefault();
                _userManager.RemoveFromRole(id, roleName);
                _userManager.AddToRole(id, role.Name);
                return Json(new { success = true, message = "Cấp quyền thành công!" });
            }
            catch
            {
                return Json(new { success = false, message = "Cấp quyền thất bại!" });
            }
        }

        [HttpPost]
        public JsonResult UpdateCommonPassword(string password)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                foreach (XmlElement element in xmlDoc.DocumentElement)
                {
                    if (element.Name.Equals("appSettings"))
                    {
                        foreach (XmlNode node in element.ChildNodes)
                        {
                            if (node.Attributes[0].Value.Equals("CommonPassword"))
                            {
                                node.Attributes[1].Value = password;
                            }
                        }
                    }
                }

                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("appSettings");

                return Json(new { success = true, message = "Cập nhật mật khẩu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật mật khẩu thất bại!" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}