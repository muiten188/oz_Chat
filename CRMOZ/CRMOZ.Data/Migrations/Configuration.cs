namespace CRMOZ.Data.Migrations
{
    using CRMOZ.Model.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CRMOZ.Data.OZChatDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OZChatDbContext context)
        {
            //CreateUser(context);
            //CreateHubUser(context);

        }

        private void CreateUser(OZChatDbContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new OZChatDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new OZChatDbContext()));
            var user = new ApplicationUser { UserName = "oz123@gmail.com", Email = "oz123@gmail.com", FullName = "OZ Corp", Avartar = "/Content/plugins/dist/img/avatar5.png" };
            if (manager.Users.Count(x => x.UserName == "oz123@gmail") == 0)
            {
                manager.Create(user, "123456");

                if (!roleManager.Roles.Any())
                {
                    roleManager.Create(new IdentityRole { Name = "Admin" });
                    roleManager.Create(new IdentityRole { Name = "Mod" });
                    roleManager.Create(new IdentityRole { Name = "User" });
                }

                var adminUser = manager.FindByEmail("oz123@gmail.com");

                manager.AddToRoles(adminUser.Id, new string[] { "Admin" });
            }
        }

        private void CreateHubUser(OZChatDbContext context)
        {
            var user = context.Users.FirstOrDefault(p => p.Email == "oz123@gmail.com");
            if(user != null)
            {
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
                context.HubUsers.Add(hubUser);
                context.SaveChanges();
            }
        }
    }
}