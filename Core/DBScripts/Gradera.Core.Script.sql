/****** Object:  Table [Vicdude_victor].[Accessright]    Script Date: 2016-03-19 09:14:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vicdude_victor].[Accessright](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[ClubId] [int] NOT NULL DEFAULT ((1)),
 CONSTRAINT [PK_Accessright] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Vicdude_victor].[Accessright_Right]    Script Date: 2016-03-19 09:14:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vicdude_victor].[Accessright_Right](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccessrightId] [int] NOT NULL,
	[AccessType] [int] NOT NULL,
	[AccessTypeRight] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Vicdude_victor].[Account]    Script Date: 2016-03-19 09:14:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vicdude_victor].[Account](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](500) NOT NULL,
	[Password] [nvarchar](500) NULL,
	[FirstName] [nvarchar](500) NULL,
	[LastName] [nvarchar](500) NULL,
	[ClubId] [int] NOT NULL DEFAULT ((1)),
	[Image] [nvarchar](500) NULL,
	[Gender] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UN_UserName] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Vicdude_victor].[Account_Information]    Script Date: 2016-03-19 09:14:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vicdude_victor].[Account_Information](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[Email] [nvarchar](500) NULL,
	[Phone] [nvarchar](50) NULL,
	[Occupation] [nvarchar](max) NULL,
	[Street] [nvarchar](500) NULL,
	[Zip] [nvarchar](50) NULL,
	[City] [nvarchar](max) NULL,
	[Grade] [int] NOT NULL DEFAULT ((1)),
	[Birthday] [datetime] NOT NULL DEFAULT (getdate()),
	[Weight] [nvarchar](500) NULL DEFAULT (''),
	[Theme] [nvarchar](50) NOT NULL DEFAULT ('default'),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [Vicdude_victor].[Account_Information_Generic]    Script Date: 2016-03-19 09:14:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vicdude_victor].[Account_Information_Generic](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [Vicdude_victor].[Account_Information_Generic_Value]    Script Date: 2016-03-19 09:14:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vicdude_victor].[Account_Information_Generic_Value](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountInformationGenericId] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
	[Value] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [Vicdude_victor].[Account_Session]    Script Date: 2016-03-19 09:14:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vicdude_victor].[Account_Session](
	[Token] [nvarchar](50) NOT NULL,
	[AccountId] [int] NOT NULL,
	[LoginDate] [datetime] NOT NULL DEFAULT (getdate()),
	[ExpirationDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Vicdude_victor].[AccountAccess]    Script Date: 2016-03-19 09:14:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vicdude_victor].[AccountAccess](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[AccessID] [int] NOT NULL,
 CONSTRAINT [PK_AccountAccess] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Vicdude_victor].[Club]    Script Date: 2016-03-19 09:14:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vicdude_victor].[Club](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Settings] [nvarchar](max) NULL,
	[Image] [nvarchar](500) NULL,
	[ShortName] [nvarchar](500) NULL,
	[Description] [nvarchar](max) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_Club] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [Vicdude_victor].[ClubLinkModule]    Script Date: 2016-03-19 09:14:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vicdude_victor].[ClubLinkModule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClubId] [int] NOT NULL,
	[ModuleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Vicdude_victor].[LoginLog]    Script Date: 2016-03-19 09:14:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vicdude_victor].[LoginLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClubId] [int] NOT NULL,
	[Date] [datetime] NOT NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Vicdude_victor].[Module]    Script Date: 2016-03-19 09:14:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vicdude_victor].[Module](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[AccessTypeId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [Vicdude_victor].[ModuleLink]    Script Date: 2016-03-19 09:14:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Vicdude_victor].[ModuleLink](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Sref] [nvarchar](500) NOT NULL,
	[Text] [nvarchar](500) NOT NULL,
	[AccessType] [int] NOT NULL,
	[AccessTypeRight] [int] NOT NULL,
	[IsAdminLink] [bit] NOT NULL DEFAULT ((0)),
	[SpecificClubId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [Vicdude_victor].[Accessright]  WITH CHECK ADD FOREIGN KEY([ClubId])
REFERENCES [Vicdude_victor].[Club] ([Id])
GO
ALTER TABLE [Vicdude_victor].[Accessright_Right]  WITH CHECK ADD FOREIGN KEY([AccessrightId])
REFERENCES [Vicdude_victor].[Accessright] ([ID])
GO
ALTER TABLE [Vicdude_victor].[Account]  WITH CHECK ADD FOREIGN KEY([ClubId])
REFERENCES [Vicdude_victor].[Club] ([Id])
GO
ALTER TABLE [Vicdude_victor].[Account_Information]  WITH CHECK ADD  CONSTRAINT [FK__Account_I__Accou__37A5467C] FOREIGN KEY([AccountId])
REFERENCES [Vicdude_victor].[Account] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [Vicdude_victor].[Account_Information] CHECK CONSTRAINT [FK__Account_I__Accou__37A5467C]
GO
ALTER TABLE [Vicdude_victor].[Account_Information_Generic_Value]  WITH CHECK ADD FOREIGN KEY([AccountInformationGenericId])
REFERENCES [Vicdude_victor].[Account_Information_Generic] ([Id])
GO


CREATE TABLE Contact
(
Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
FirstName NVARCHAR(MAX),
LastName NVARCHAR(MAX),
Email NVARCHAR(MAX),
Phone NVARCHAR(MAX),
IsDeleted BIT DEFAULT(0),
IsUnsubscribed BIT DEFAULT(0),
UnsubscribeDate DATETIME
)