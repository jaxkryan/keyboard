use master
go

ALTER DATABASE KeyboardVN SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
go

drop database if exists KeyboardVN 
go

create database KeyboardVN
go

use KeyboardVN
go

CREATE TABLE [User] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [firstName] nvarchar(50),
  [lastName] nvarchar(50),
  [street] varchar(50),
  [city] varchar(30),
  [province] varchar(30),
  [country] varchar(30),
  [UserName] [nvarchar](256) NULL,
  [NormalizedUserName] [nvarchar](256) NULL,
  [Email] [nvarchar](256) NULL,
  [NormalizedEmail] [nvarchar](256) NULL,
  [EmailConfirmed] [bit] NOT NULL,
  [PasswordHash] [nvarchar](max) NULL,
  [SecurityStamp] [nvarchar](max) NULL,
  [ConcurrencyStamp] [nvarchar](max) NULL,
  [PhoneNumber] [nvarchar](max) NULL,
  [PhoneNumberConfirmed] [bit] NOT NULL,
  [TwoFactorEnabled] [bit] NOT NULL,
  [LockoutEnd] [datetimeoffset](7) NULL,
  [LockoutEnabled] [bit] NOT NULL,
  [AccessFailedCount] [int] NOT NULL,
)
GO

CREATE TABLE [dbo].[UserClaims](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [UserId] [int] NOT NULL,
  [ClaimType] [nvarchar](max) NULL,
  [ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserClaims] PRIMARY KEY CLUSTERED 
(
  [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserClaims]  WITH CHECK ADD  CONSTRAINT [FK_UserClaims_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserClaims] CHECK CONSTRAINT [FK_UserClaims_User_UserId]
GO

CREATE TABLE [dbo].[Roles](
  [Id] [int] NOT NULL,
  [Name] [nvarchar](256) NULL,
  [NormalizedName] [nvarchar](256) NULL,
  [ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
  [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[UserRoles](
  [UserId] [int] NOT NULL,
  [RoleId] [int] NOT NULL,
  [CreatedDate] date
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
  [UserId] ASC,
  [RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles_RoleId]
GO

ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_User_UserId]
GO

CREATE TABLE [dbo].[RoleClaims](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [RoleId] [int] NOT NULL,
  [ClaimType] [nvarchar](max) NULL,
  [ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_RoleClaims] PRIMARY KEY CLUSTERED 
(
  [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[RoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RoleClaims] CHECK CONSTRAINT [FK_RoleClaims_Roles_RoleId]
GO

CREATE TABLE [dbo].[UserLogins](
  [LoginProvider] [nvarchar](128) NOT NULL,
  [ProviderKey] [nvarchar](128) NOT NULL,
  [ProviderDisplayName] [nvarchar](max) NULL,
  [UserId] [int] NOT NULL,
 CONSTRAINT [PK_UserLogins] PRIMARY KEY CLUSTERED 
(
  [LoginProvider] ASC,
  [ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserLogins]  WITH CHECK ADD  CONSTRAINT [FK_UserLogins_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserLogins] CHECK CONSTRAINT [FK_UserLogins_User_UserId]
GO

CREATE TABLE [dbo].[UserTokens](
  [UserId] [int] NOT NULL,
  [LoginProvider] [nvarchar](128) NOT NULL,
  [Name] [nvarchar](128) NOT NULL,
  [Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserTokens] PRIMARY KEY CLUSTERED 
(
  [UserId] ASC,
  [LoginProvider] ASC,
  [Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserTokens]  WITH CHECK ADD  CONSTRAINT [FK_UserTokens_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserTokens] CHECK CONSTRAINT [FK_UserTokens_User_UserId]
GO

CREATE TABLE [Product] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [categoryId] int NOT NULL,
  [brandId] int NOT NULL,
  [name] nvarchar(200),
  [image] nvarchar(max) NULL ,
  [description] nvarchar(4000),
  [price] float NOT NULL,
  [discount] float DEFAULT (0),
  [unitInStock] int NOT NULL DEFAULT (0)
)
GO

CREATE TABLE [Category] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [name] nvarchar(20) NOT NULL,
  [image] nvarchar(max)
)
GO

CREATE TABLE [Brand] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [name] nvarchar(20) NOT NULL,
  [image] nvarchar(max)
)
GO

CREATE TABLE [Order] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [userId] int NOT NULL,
  [receiver] nvarchar(50),
  [shipStreet] varchar(50),
  [shipCity] varchar(30),
  [shipProvince] varchar(30),
  [shipCountry] varchar(30),
  [shipEmail] varchar(50),
  [shipPhone] varchar(15),
  [status] nvarchar(255) NOT NULL,
  [createdTime] datetime
)
GO

CREATE TABLE [OrderDetail] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [orderId] int NOT NULL,
  [productId] int NOT NULL,
  [price] float NOT NULL,
  [quantity] int NOT NULL DEFAULT (1)
)
GO

CREATE TABLE [Cart] (
  [id] int PRIMARY KEY NOT NULL,
  [userId] int
)
GO

CREATE TABLE [CartItem] (
  [cartId] int NOT NULL,
  [productId] int NOT NULL,
  [quantity] int NOT NULL DEFAULT (1),
  PRIMARY KEY ([productId], [cartId])
)
GO

ALTER TABLE [Product] ADD FOREIGN KEY ([categoryId]) REFERENCES [Category] ([id])
GO

ALTER TABLE [Product] ADD FOREIGN KEY ([brandId]) REFERENCES [Brand] ([id])
GO

ALTER TABLE [Order] ADD FOREIGN KEY ([userId]) REFERENCES [User] ([id])
GO

ALTER TABLE [OrderDetail] ADD FOREIGN KEY ([orderId]) REFERENCES [Order] ([id])
GO

ALTER TABLE [OrderDetail] ADD FOREIGN KEY ([productId]) REFERENCES [Product] ([id])
GO

ALTER TABLE [Cart] ADD FOREIGN KEY ([userId]) REFERENCES [User] ([id])
GO

ALTER TABLE [CartItem] ADD FOREIGN KEY ([cartId]) REFERENCES [Cart] ([id])
GO

ALTER TABLE [CartItem] ADD FOREIGN KEY ([productId]) REFERENCES [Product] ([id])
GO


INSERT INTO [User]([firstName],[lastName],[street],[city],[province],[country],[UserName],[NormalizedUserName],[Email],[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnabled],[AccessFailedCount])
VALUES ('Huy', 'Nguyen', 'Thach That', 'Ha Noi', 'Hanoi', 'Vietnam', 'huynnthe176346@fpt.edu.vn', 'HUYNNTHE176346@FPT.EDU.VN', 'huynnthe176346@fpt.edu.vn', 'HUYNNTHE176346@FPT.EDU.VN', 'true', 'AQAAAAEAACcQAAAAEAnSX8hMOA15yNuUv9eALyy6vPWfLCqk1LpKCzP7u0Gp47K+HcM+ZGN3czYPkusgaQ==', 'ee3c73ef-a579-4d05-a27c-15d713f00cff', '0911362307', 'true', 'false', 'true', 0),
      ('Huy', 'Le', 'National Highway 1A', 'Vinh Tan Ward', 'Binh Thuan', 'Vietnam', 'huyldhe176275@fpt.edu.vn', 'HUYLDHE176275@FPT.EDU.VN', 'huyldhe176275@fpt.edu.vn', 'HUYLDHE176275@FPT.EDU.VN', 'true', 'AQAAAAEAACcQAAAAEAnSX8hMOA15yNuUv9eALyy6vPWfLCqk1LpKCzP7u0Gp47K+HcM+ZGN3czYPkusgaQ==', 'e741f649-611e-47c2-94d2-243e078345fe', '0292827403', 'true', 'false', 'true', 0),
      ('Hung', 'Nguyen', '10 Quang Trung St.', 'Binh Dinh Townlet', 'Binh Dinh', 'Vietnam', 'hungnthe176686@fpt.edu.vn', 'HUNGNTHE176686@FPT.EDU.VN', 'hungnthe176686@fpt.edu.vn', 'HUNGNTHE176686@FPT.EDU.VN', 'true', 'AQAAAAEAACcQAAAAEAnSX8hMOA15yNuUv9eALyy6vPWfLCqk1LpKCzP7u0Gp47K+HcM+ZGN3czYPkusgaQ==', '1cb3e3bd-7d68-4fcf-9835-84c56bb1f5ed', '0493891233', 'true', 'false', 'true', 0),
      ('Long', 'Nguyen', '266B Nguyen Tieu La St., Ward 8, Dist. 10', 'Ho Chi Minh City', 'Ho Chi Minh City', 'Vietnam', 'longndhe176282@fpt.edu.vn','LONGNDHE176282@FPT.EDU.VN', 'longndhe176282@fpt.edu.vn','LONGNDHE176282@FPT.EDU.VN', 'true', 'AQAAAAEAACcQAAAAEAnSX8hMOA15yNuUv9eALyy6vPWfLCqk1LpKCzP7u0Gp47K+HcM+ZGN3czYPkusgaQ==', 'd30529f0-0b25-45c4-8920-6f718d117145', '0292827403', 'true', 'false', 'true', 0),
      ('Nam', 'Nguyen', '30 Pho Duc Chinh Street, Truc Bach Ward', 'Ba Dinh District', 'Hanoi', 'Vietnam', 'nam@mail.com', 'NAM@MAIL.COM', 'nam@mail.com', 'NAM@MAIL.COM', 'true', 'AQAAAAEAACcQAAAAEAnSX8hMOA15yNuUv9eALyy6vPWfLCqk1LpKCzP7u0Gp47K+HcM+ZGN3czYPkusgaQ==', 'c51f5c80-0de1-47c7-a4db-d84b015b0019', '0292827403', 'true', 'false', 'true', 0),
      ('Hung', 'Thai', '9 Duong Thanh Street', 'Hoan Kiem District', 'Hanoi', 'Vietnam', 'hung@mail.com', 'HUNG@MAIL.COM', 'hung@mail.com', 'HUNG@MAIL.COM', 'true', 'AQAAAAEAACcQAAAAEAnSX8hMOA15yNuUv9eALyy6vPWfLCqk1LpKCzP7u0Gp47K+HcM+ZGN3czYPkusgaQ==', '162a58dd-3ced-47b6-8bbf-4ef854b0befb', '0292827403', 'true', 'false', 'true', 0),
      ('Kien', 'Dinh', '101 Le Loi Street, Ward 1', 'My Tho City', 'Tien Giang', 'Vietnam', 'kien@mail.com','KIEN@MAIL.COM', 'kien@mail.com','KIEN@MAIL.COM', 'true', 'AQAAAAEAACcQAAAAEAnSX8hMOA15yNuUv9eALyy6vPWfLCqk1LpKCzP7u0Gp47K+HcM+ZGN3czYPkusgaQ==', 'e4fd9d3f-190b-4e83-9c92-314f5e027e06', '0292827403', 'true', 'false', 'true', 0),
      ('Ngan', 'Nguyen', '279 Nguyen Van Luong, District 6', 'Ho Chi Minh City', 'Ho Chi Minh City', 'Vietnam', 'ngan@mail.com', 'NGAN@MAIL.COM', 'ngan@mail.com', 'NGAN@MAIL.COM', 'true', 'AQAAAAEAACcQAAAAEAnSX8hMOA15yNuUv9eALyy6vPWfLCqk1LpKCzP7u0Gp47K+HcM+ZGN3czYPkusgaQ==', '7627812a-c4b9-4b03-9566-f3a2b97c1f58', '0292827403', 'true', 'false', 'true', 0),
      ('Khanh', 'Pham', '14A Trung Trac Street, Le Hong Phong Ward', 'Le Hong Phong Ward', 'Thai Binh', 'Vietnam', 'khanh@mail.com', 'KHANH@MAIL.COM', 'khanh@mail.com', 'KHANH@MAIL.COM', 'true', 'AQAAAAEAACcQAAAAEAnSX8hMOA15yNuUv9eALyy6vPWfLCqk1LpKCzP7u0Gp47K+HcM+ZGN3czYPkusgaQ==', '79db83b8-acb1-47da-9384-09cdcf904a3e', '0292827403', 'true', 'false', 'true', 0),
      ('Cuong', 'Ngo', 'nha B15, Do thi moi Dai Kim, Hoang Mai', 'Ha Noi', 'Hanoi', 'Vietnam', 'cuong@mail.com', 'CUONG@MAIL.COM', 'cuong@mail.com', 'CUONG@MAIL.COM', 'true', 'AQAAAAEAACcQAAAAEAnSX8hMOA15yNuUv9eALyy6vPWfLCqk1LpKCzP7u0Gp47K+HcM+ZGN3czYPkusgaQ==', 'd8c36a25-3af9-4160-8275-480c2f7ea5a7', '0292827403', 'true', 'false', 'true', 0)

INSERT INTO [Category]([name],[image])
VALUES ('Mini', 'https://akko.vn/wp-content/uploads/2021/10/ban-phim-co-akko-3061-world-tour-tokyo-r2-001-280x280.jpg'),
	('TKL', 'https://akko.vn/wp-content/uploads/2023/06/ban-phim-co-akko-3087-rf-dragon-ball-z-goku-280x280.png'),
	('Fullsize', 'https://akko.vn/wp-content/uploads/2023/06/ban-phim-co-monsgeek-mg108-black-cyan-280x280.jpg')

INSERT INTO [Brand]([name],[image])
VALUES ('Others', ''),
	('Akko', 'data:image/jpeg;base64,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')

INSERT INTO [Product]([categoryId],[brandId],[name],[image],[description],[price],[discount],[unitInStock])
VALUES (2, 2, 'MonsGeek M1 QMK Black AKKO Switch v3 Cream Yellow Pro', 'https://product.hstatic.net/200000722513/product/thumbphim-_1__400bb1d5a7844e7db6bc12f7fd1db4a6_medium.jpg', 'MonsGeek M1 QMK Black has an appearance with a sturdy full aluminum shell. Use black as the main color to enhance the mysterious beauty and strength. Creating effects from dark to light based on the color arrangement of the keycaps makes the MonsGeek computer keyboard attractive at first sight.', 200, 0, 12)

USE master;
GO

ALTER DATABASE KeyboardVN SET MULTI_USER;
GO

ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = OFF
GO