namespace CRMOZ.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationGroups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        Description = c.String(maxLength: 250),
                        OrderBy = c.Int(),
                        Status = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ApplicationRoleGroups",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GroupId, t.RoleId })
                .ForeignKey("dbo.ApplicationGroups", t => t.GroupId)
                .ForeignKey("dbo.ApplicationRoles", t => t.RoleId)
                .Index(t => t.GroupId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ApplicationRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(maxLength: 256),
                        OrderBy = c.Int(),
                        Status = c.Boolean(),
                        IsDelete = c.Boolean(),
                        IsDefault = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.ApplicationRoles", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.ApplicationUserGroups",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.GroupId })
                .ForeignKey("dbo.ApplicationGroups", t => t.GroupId)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(maxLength: 256),
                        Avartar = c.String(maxLength: 256),
                        CommonPassword = c.String(maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserClaims",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Id = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Connection",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        ConnectionID = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.HubUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.HubUsers",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        UserName = c.String(maxLength: 256),
                        FullName = c.String(maxLength: 256),
                        Avatar = c.String(maxLength: 256),
                        Connected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MessagePrivates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        FromID = c.String(maxLength: 128),
                        ReceiveID = c.String(maxLength: 128),
                        FromFullName = c.String(maxLength: 256),
                        FromAvatar = c.String(maxLength: 256),
                        CreatedDate = c.DateTime(nullable: false),
                        StrDateTime = c.String(maxLength: 50),
                        IsFile = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.HubUsers", t => t.FromID)
                .Index(t => t.FromID);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MessageGroups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        FromID = c.String(maxLength: 128),
                        GroupID = c.Int(nullable: false),
                        FromFullName = c.String(maxLength: 256),
                        FromAvatar = c.String(maxLength: 256),
                        CreatedDate = c.DateTime(nullable: false),
                        StrDateTime = c.String(maxLength: 50),
                        IsFile = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Groups", t => t.GroupID)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.HubUserGroups",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        GroupID = c.Int(nullable: false),
                        IsCreater = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.GroupID })
                .ForeignKey("dbo.Groups", t => t.GroupID)
                .ForeignKey("dbo.HubUsers", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.NewMessageGroups",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        GroupID = c.Int(nullable: false),
                        MessageID = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.GroupID, t.MessageID })
                .ForeignKey("dbo.Groups", t => t.GroupID)
                .ForeignKey("dbo.HubUsers", t => t.UserID)
                .ForeignKey("dbo.MessageGroups", t => t.MessageID)
                .Index(t => t.UserID)
                .Index(t => t.GroupID)
                .Index(t => t.MessageID);
            
            CreateTable(
                "dbo.UserMessagePrivates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FromUserID = c.String(),
                        RecieveUserID = c.String(),
                        NewMessage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserRoles", "IdentityRole_Id", "dbo.ApplicationRoles");
            DropForeignKey("dbo.NewMessageGroups", "MessageID", "dbo.MessageGroups");
            DropForeignKey("dbo.NewMessageGroups", "UserID", "dbo.HubUsers");
            DropForeignKey("dbo.NewMessageGroups", "GroupID", "dbo.Groups");
            DropForeignKey("dbo.HubUserGroups", "UserID", "dbo.HubUsers");
            DropForeignKey("dbo.HubUserGroups", "GroupID", "dbo.Groups");
            DropForeignKey("dbo.MessageGroups", "GroupID", "dbo.Groups");
            DropForeignKey("dbo.Connection", "UserID", "dbo.HubUsers");
            DropForeignKey("dbo.MessagePrivates", "FromID", "dbo.HubUsers");
            DropForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.ApplicationRoles");
            DropForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups");
            DropIndex("dbo.NewMessageGroups", new[] { "MessageID" });
            DropIndex("dbo.NewMessageGroups", new[] { "GroupID" });
            DropIndex("dbo.NewMessageGroups", new[] { "UserID" });
            DropIndex("dbo.HubUserGroups", new[] { "GroupID" });
            DropIndex("dbo.HubUserGroups", new[] { "UserID" });
            DropIndex("dbo.MessageGroups", new[] { "GroupID" });
            DropIndex("dbo.MessagePrivates", new[] { "FromID" });
            DropIndex("dbo.Connection", new[] { "UserID" });
            DropIndex("dbo.ApplicationUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "GroupId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "UserId" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "RoleId" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "GroupId" });
            DropTable("dbo.UserMessagePrivates");
            DropTable("dbo.NewMessageGroups");
            DropTable("dbo.HubUserGroups");
            DropTable("dbo.MessageGroups");
            DropTable("dbo.Groups");
            DropTable("dbo.MessagePrivates");
            DropTable("dbo.HubUsers");
            DropTable("dbo.Connection");
            DropTable("dbo.ApplicationUserLogins");
            DropTable("dbo.ApplicationUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.ApplicationUserGroups");
            DropTable("dbo.ApplicationUserRoles");
            DropTable("dbo.ApplicationRoles");
            DropTable("dbo.ApplicationRoleGroups");
            DropTable("dbo.ApplicationGroups");
        }
    }
}
