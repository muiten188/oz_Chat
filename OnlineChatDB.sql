USE [OZChat]
GO
/****** Object:  Table [dbo].[ApplicationGroups]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationGroups](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Description] [nvarchar](250) NULL,
	[OrderBy] [int] NULL,
	[Status] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationGroups] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApplicationRoleGroups]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationRoleGroups](
	[GroupId] [int] NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationRoleGroups] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApplicationRoles]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](256) NULL,
	[OrderBy] [int] NULL,
	[Status] [bit] NULL,
	[IsDelete] [bit] NULL,
	[IsDefault] [bit] NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApplicationUserClaims]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationUserClaims](
	[UserId] [nvarchar](128) NOT NULL,
	[Id] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[ApplicationUser_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.ApplicationUserClaims] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApplicationUserGroups]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationUserGroups](
	[UserId] [nvarchar](128) NOT NULL,
	[GroupId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationUserGroups] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApplicationUserLogins]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationUserLogins](
	[UserId] [nvarchar](128) NOT NULL,
	[LoginProvider] [nvarchar](max) NULL,
	[ProviderKey] [nvarchar](max) NULL,
	[ApplicationUser_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.ApplicationUserLogins] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApplicationUserRoles]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
	[ApplicationUser_Id] [nvarchar](128) NULL,
	[IdentityRole_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.ApplicationUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApplicationUsers]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationUsers](
	[Id] [nvarchar](128) NOT NULL,
	[FullName] [nvarchar](256) NULL,
	[Avartar] [nvarchar](256) NULL,
	[CommonPassword] [nvarchar](128) NULL,
	[Email] [nvarchar](max) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.ApplicationUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Groups]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NULL,
 CONSTRAINT [PK_dbo.Groups] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HubUserGroups]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HubUserGroups](
	[UserID] [nvarchar](128) NOT NULL,
	[GroupID] [int] NOT NULL,
	[IsCreater] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.HubUserGroups] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HubUsers]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HubUsers](
	[ID] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[UserName] [nvarchar](256) NULL,
	[FullName] [nvarchar](256) NULL,
	[Avatar] [nvarchar](256) NULL,
	[ConnectionId] [nvarchar](max) NULL,
	[Connected] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.HubUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MessageGroups]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MessageGroups](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](max) NULL,
	[FromID] [nvarchar](128) NULL,
	[GroupID] [int] NOT NULL,
	[FromFullName] [nvarchar](256) NULL,
	[FromAvatar] [nvarchar](256) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[StrDateTime] [nvarchar](50) NULL,
	[IsFile] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.MessageGroups] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MessagePrivates]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MessagePrivates](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](max) NULL,
	[FromID] [nvarchar](128) NULL,
	[ReceiveID] [nvarchar](128) NULL,
	[FromFullName] [nvarchar](256) NULL,
	[FromAvatar] [nvarchar](256) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[StrDateTime] [nvarchar](50) NULL,
	[IsFile] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.MessagePrivates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NewMessageGroups]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewMessageGroups](
	[UserID] [nvarchar](128) NOT NULL,
	[GroupID] [int] NOT NULL,
	[MessageID] [int] NOT NULL,
	[Count] [int] NOT NULL,
 CONSTRAINT [PK_dbo.NewMessageGroups] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[GroupID] ASC,
	[MessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserMessagePrivates]    Script Date: 6/11/2018 3:21:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserMessagePrivates](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FromUserID] [nvarchar](max) NULL,
	[RecieveUserID] [nvarchar](max) NULL,
	[NewMessage] [int] NOT NULL,
 CONSTRAINT [PK_dbo.UserMessagePrivates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
INSERT [dbo].[ApplicationRoles] ([Id], [Name], [Description], [OrderBy], [Status], [IsDelete], [IsDefault], [Discriminator]) VALUES (N'156008a0-b519-4580-b14e-82d1649ba73f', N'Mod', NULL, NULL, NULL, NULL, NULL, N'IdentityRole')
INSERT [dbo].[ApplicationRoles] ([Id], [Name], [Description], [OrderBy], [Status], [IsDelete], [IsDefault], [Discriminator]) VALUES (N'3123f0f8-a4e8-4877-b8d2-5cac308feb58', N'Admin', NULL, NULL, NULL, NULL, NULL, N'IdentityRole')
INSERT [dbo].[ApplicationRoles] ([Id], [Name], [Description], [OrderBy], [Status], [IsDelete], [IsDefault], [Discriminator]) VALUES (N'5cf3881e-3b21-4e78-a3b6-1fbd03cbc536', N'User', NULL, NULL, NULL, NULL, NULL, N'IdentityRole')
INSERT [dbo].[ApplicationUserRoles] ([UserId], [RoleId], [Discriminator], [ApplicationUser_Id], [IdentityRole_Id]) VALUES (N'99150f81-0753-4257-abf1-b7c406594853', N'3123f0f8-a4e8-4877-b8d2-5cac308feb58', N'IdentityUserRole', NULL, NULL)
INSERT [dbo].[ApplicationUserRoles] ([UserId], [RoleId], [Discriminator], [ApplicationUser_Id], [IdentityRole_Id]) VALUES (N'eea15d75-9e9c-4a79-948a-0cdd5f917210', N'156008a0-b519-4580-b14e-82d1649ba73f', N'IdentityUserRole', NULL, NULL)
INSERT [dbo].[ApplicationUsers] ([Id], [FullName], [Avartar], [CommonPassword], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'99150f81-0753-4257-abf1-b7c406594853', N'OZ Corp', N'/Content/plugins/dist/img/avatar5.png', NULL, N'oz123@gmail.com', 0, N'AGBFjtYSCwOgbyZxjlKIjyKCb/26wjF/5p8fxA102ZMo7HlcOU5+NCly4nj1eyhuAg==', N'ffa590a6-e995-4c48-a8d5-3a622379e34a', NULL, 0, 0, NULL, 0, 0, N'oz123@gmail.com')
INSERT [dbo].[ApplicationUsers] ([Id], [FullName], [Avartar], [CommonPassword], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'eea15d75-9e9c-4a79-948a-0cdd5f917210', N'Nguyễn Tiến Hoàng', N'/Uploads/0e9c47d4-263c-4682-b395-17817bf1fc0c.jpg', N'123456', N'tienhoang1211@gmail.com', 0, N'AInAOayh+IWGQnWSNfPBpgD8DqxjDTVKmuHipOcQYffToRPR2PlNaf/UkS5v4lJvQw==', N'c80b030b-ac1a-44ea-929f-0746fee9a8da', NULL, 0, 0, NULL, 0, 0, N'tienhoang1211@gmail.com')
INSERT [dbo].[HubUsers] ([ID], [Email], [UserName], [FullName], [Avatar], [ConnectionId], [Connected]) VALUES (N'99150f81-0753-4257-abf1-b7c406594853', N'oz123@gmail.com', N'oz123@gmail.com', N'OZ Corp', N'/Content/plugins/dist/img/avatar5.png', N'', 0)
INSERT [dbo].[HubUsers] ([ID], [Email], [UserName], [FullName], [Avatar], [ConnectionId], [Connected]) VALUES (N'eea15d75-9e9c-4a79-948a-0cdd5f917210', N'tienhoang1211@gmail.com', N'tienhoang1211@gmail.com', N'Nguyễn Tiến Hoàng', N'/Uploads/0e9c47d4-263c-4682-b395-17817bf1fc0c.jpg', N'', 0)
SET IDENTITY_INSERT [dbo].[MessagePrivates] ON 

INSERT [dbo].[MessagePrivates] ([ID], [Content], [FromID], [ReceiveID], [FromFullName], [FromAvatar], [CreatedDate], [StrDateTime], [IsFile]) VALUES (1, N'xfdssdds', N'99150f81-0753-4257-abf1-b7c406594853', N'eea15d75-9e9c-4a79-948a-0cdd5f917210', N'OZ Corp', N'/Content/plugins/dist/img/avatar5.png', CAST(N'2018-06-11 15:16:45.240' AS DateTime), N'11/06/2018 15:16', 0)
INSERT [dbo].[MessagePrivates] ([ID], [Content], [FromID], [ReceiveID], [FromFullName], [FromAvatar], [CreatedDate], [StrDateTime], [IsFile]) VALUES (2, N'dsbsdbfhdshbf', N'99150f81-0753-4257-abf1-b7c406594853', N'eea15d75-9e9c-4a79-948a-0cdd5f917210', N'OZ Corp', N'/Content/plugins/dist/img/avatar5.png', CAST(N'2018-06-11 15:16:50.007' AS DateTime), N'11/06/2018 15:16', 0)
INSERT [dbo].[MessagePrivates] ([ID], [Content], [FromID], [ReceiveID], [FromFullName], [FromAvatar], [CreatedDate], [StrDateTime], [IsFile]) VALUES (3, N'sddfdhfbgdfg', N'99150f81-0753-4257-abf1-b7c406594853', N'eea15d75-9e9c-4a79-948a-0cdd5f917210', N'OZ Corp', N'/Content/plugins/dist/img/avatar5.png', CAST(N'2018-06-11 15:16:52.847' AS DateTime), N'11/06/2018 15:16', 0)
SET IDENTITY_INSERT [dbo].[MessagePrivates] OFF
SET IDENTITY_INSERT [dbo].[UserMessagePrivates] ON 

INSERT [dbo].[UserMessagePrivates] ([ID], [FromUserID], [RecieveUserID], [NewMessage]) VALUES (1, N'99150f81-0753-4257-abf1-b7c406594853', N'eea15d75-9e9c-4a79-948a-0cdd5f917210', 0)
SET IDENTITY_INSERT [dbo].[UserMessagePrivates] OFF
ALTER TABLE [dbo].[ApplicationRoleGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.ApplicationGroups_GroupId] FOREIGN KEY([GroupId])
REFERENCES [dbo].[ApplicationGroups] ([ID])
GO
ALTER TABLE [dbo].[ApplicationRoleGroups] CHECK CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.ApplicationGroups_GroupId]
GO
ALTER TABLE [dbo].[ApplicationRoleGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.ApplicationRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[ApplicationRoles] ([Id])
GO
ALTER TABLE [dbo].[ApplicationRoleGroups] CHECK CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.ApplicationRoles_RoleId]
GO
ALTER TABLE [dbo].[ApplicationUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationUserClaims_dbo.ApplicationUsers_ApplicationUser_Id] FOREIGN KEY([ApplicationUser_Id])
REFERENCES [dbo].[ApplicationUsers] ([Id])
GO
ALTER TABLE [dbo].[ApplicationUserClaims] CHECK CONSTRAINT [FK_dbo.ApplicationUserClaims_dbo.ApplicationUsers_ApplicationUser_Id]
GO
ALTER TABLE [dbo].[ApplicationUserGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.ApplicationGroups_GroupId] FOREIGN KEY([GroupId])
REFERENCES [dbo].[ApplicationGroups] ([ID])
GO
ALTER TABLE [dbo].[ApplicationUserGroups] CHECK CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.ApplicationGroups_GroupId]
GO
ALTER TABLE [dbo].[ApplicationUserGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.ApplicationUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[ApplicationUsers] ([Id])
GO
ALTER TABLE [dbo].[ApplicationUserGroups] CHECK CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.ApplicationUsers_UserId]
GO
ALTER TABLE [dbo].[ApplicationUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationUserLogins_dbo.ApplicationUsers_ApplicationUser_Id] FOREIGN KEY([ApplicationUser_Id])
REFERENCES [dbo].[ApplicationUsers] ([Id])
GO
ALTER TABLE [dbo].[ApplicationUserLogins] CHECK CONSTRAINT [FK_dbo.ApplicationUserLogins_dbo.ApplicationUsers_ApplicationUser_Id]
GO
ALTER TABLE [dbo].[ApplicationUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationUserRoles_dbo.ApplicationRoles_IdentityRole_Id] FOREIGN KEY([IdentityRole_Id])
REFERENCES [dbo].[ApplicationRoles] ([Id])
GO
ALTER TABLE [dbo].[ApplicationUserRoles] CHECK CONSTRAINT [FK_dbo.ApplicationUserRoles_dbo.ApplicationRoles_IdentityRole_Id]
GO
ALTER TABLE [dbo].[ApplicationUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationUserRoles_dbo.ApplicationUsers_ApplicationUser_Id] FOREIGN KEY([ApplicationUser_Id])
REFERENCES [dbo].[ApplicationUsers] ([Id])
GO
ALTER TABLE [dbo].[ApplicationUserRoles] CHECK CONSTRAINT [FK_dbo.ApplicationUserRoles_dbo.ApplicationUsers_ApplicationUser_Id]
GO
ALTER TABLE [dbo].[HubUserGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.HubUserGroups_dbo.Groups_GroupID] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([ID])
GO
ALTER TABLE [dbo].[HubUserGroups] CHECK CONSTRAINT [FK_dbo.HubUserGroups_dbo.Groups_GroupID]
GO
ALTER TABLE [dbo].[HubUserGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.HubUserGroups_dbo.HubUsers_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[HubUsers] ([ID])
GO
ALTER TABLE [dbo].[HubUserGroups] CHECK CONSTRAINT [FK_dbo.HubUserGroups_dbo.HubUsers_UserID]
GO
ALTER TABLE [dbo].[MessageGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.MessageGroups_dbo.Groups_GroupID] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([ID])
GO
ALTER TABLE [dbo].[MessageGroups] CHECK CONSTRAINT [FK_dbo.MessageGroups_dbo.Groups_GroupID]
GO
ALTER TABLE [dbo].[MessagePrivates]  WITH CHECK ADD  CONSTRAINT [FK_dbo.MessagePrivates_dbo.HubUsers_FromID] FOREIGN KEY([FromID])
REFERENCES [dbo].[HubUsers] ([ID])
GO
ALTER TABLE [dbo].[MessagePrivates] CHECK CONSTRAINT [FK_dbo.MessagePrivates_dbo.HubUsers_FromID]
GO
ALTER TABLE [dbo].[NewMessageGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.NewMessageGroups_dbo.Groups_GroupID] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([ID])
GO
ALTER TABLE [dbo].[NewMessageGroups] CHECK CONSTRAINT [FK_dbo.NewMessageGroups_dbo.Groups_GroupID]
GO
ALTER TABLE [dbo].[NewMessageGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.NewMessageGroups_dbo.HubUsers_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[HubUsers] ([ID])
GO
ALTER TABLE [dbo].[NewMessageGroups] CHECK CONSTRAINT [FK_dbo.NewMessageGroups_dbo.HubUsers_UserID]
GO
ALTER TABLE [dbo].[NewMessageGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.NewMessageGroups_dbo.MessageGroups_MessageID] FOREIGN KEY([MessageID])
REFERENCES [dbo].[MessageGroups] ([ID])
GO
ALTER TABLE [dbo].[NewMessageGroups] CHECK CONSTRAINT [FK_dbo.NewMessageGroups_dbo.MessageGroups_MessageID]
GO
