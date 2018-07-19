using CRMOZ.Model.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CRMOZ.Data
{
    public class OZChatDbContext : IdentityDbContext<ApplicationUser>
    {
        public OZChatDbContext() : base("OZChatConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<ApplicationGroup> ApplicationGroups { set; get; }
        public DbSet<ApplicationRole> ApplicationRoles { set; get; }
        public DbSet<ApplicationRoleGroup> ApplicationRoleGroups { set; get; }
        public DbSet<ApplicationUserGroup> ApplicationUserGroups { set; get; }
        public DbSet<ApplicationUserRoles> ApplicationUserRoles { set; get; }

        public DbSet<HubUser> HubUsers { set; get; }
        public DbSet<Connection> Connection{ set; get; }
        public DbSet<FcmConnection> FcmConnection { set; get; }
        public DbSet<Group> Groups { set; get; }
        public DbSet<HubUserGroup> HubUserGroups { set; get; }
        public DbSet<MessagePrivate> MessagePrivates { set; get; }
        public DbSet<MessageGroup> MessageGroups { set; get; }
        public DbSet<UserMessagePrivate> UserMessagePrivates { set; get; }
        public DbSet<NewMessageGroup> NewMessageGroups { set; get; }

        public static OZChatDbContext Create()
        {
            return new OZChatDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("ApplicationUserRoles");
            builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("ApplicationUserLogins");
            builder.Entity<IdentityRole>().ToTable("ApplicationRoles");
            builder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("ApplicationUserClaims");
            builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}