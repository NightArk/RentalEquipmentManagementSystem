USE [master]
GO
/****** Object:  Database [EquipmentRentalDB]    Script Date: 04/05/2025 4:34:13 PM ******/
CREATE DATABASE [EquipmentRentalDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EquipmentRentalDB', FILENAME = N'C:\Users\MohdA\EquipmentRentalDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EquipmentRentalDB_log', FILENAME = N'C:\Users\MohdA\EquipmentRentalDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [EquipmentRentalDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EquipmentRentalDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EquipmentRentalDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [EquipmentRentalDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EquipmentRentalDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EquipmentRentalDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [EquipmentRentalDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EquipmentRentalDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [EquipmentRentalDB] SET  MULTI_USER 
GO
ALTER DATABASE [EquipmentRentalDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EquipmentRentalDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EquipmentRentalDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EquipmentRentalDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EquipmentRentalDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EquipmentRentalDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [EquipmentRentalDB] SET QUERY_STORE = OFF
GO
USE [EquipmentRentalDB]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 04/05/2025 4:34:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Documents]    Script Date: 04/05/2025 4:34:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Documents](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RentalTransactionId] [int] NULL,
	[UserId] [int] NULL,
	[FileName] [nvarchar](255) NOT NULL,
	[FileType] [nvarchar](50) NOT NULL,
	[StoragePath] [nvarchar](500) NOT NULL,
	[UploadedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Equipment]    Script Date: 04/05/2025 4:34:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equipment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[CategoryId] [int] NULL,
	[RentalPrice] [decimal](10, 2) NOT NULL,
	[AvailabilityStatus] [nvarchar](50) NOT NULL,
	[ConditionStatus] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 04/05/2025 4:34:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[EquipmentId] [int] NULL,
	[Rating] [int] NULL,
	[Comment] [nvarchar](500) NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 04/05/2025 4:34:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Action] [nvarchar](255) NOT NULL,
	[Timestamp] [datetime] NULL,
	[AffectedData] [nvarchar](255) NULL,
	[Source] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 04/05/2025 4:34:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[MessageContent] [nvarchar](500) NOT NULL,
	[NotificationType] [nvarchar](50) NOT NULL,
	[Status] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RentalRequests]    Script Date: 04/05/2025 4:34:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentalRequests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[EquipmentId] [int] NULL,
	[RentalStartDate] [datetime] NOT NULL,
	[ReturnDate] [datetime] NOT NULL,
	[TotalCost] [decimal](10, 2) NOT NULL,
	[Status] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RentalTransactions]    Script Date: 04/05/2025 4:34:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentalTransactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RentalRequestId] [int] NULL,
	[AssignedEquipmentId] [int] NULL,
	[CustomerId] [int] NULL,
	[ActualRentalStartDate] [datetime] NOT NULL,
	[ReturnDate] [datetime] NOT NULL,
	[RentalPeriod] [int] NOT NULL,
	[RentalFee] [decimal](10, 2) NOT NULL,
	[Deposit] [decimal](10, 2) NOT NULL,
	[PaymentStatus] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReturnRecords]    Script Date: 04/05/2025 4:34:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReturnRecords](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RentalTransactionId] [int] NULL,
	[ActualReturnDate] [datetime] NOT NULL,
	[ReturnCondition] [nvarchar](50) NOT NULL,
	[LateReturnFee] [decimal](10, 2) NULL,
	[AdditionalCharges] [decimal](10, 2) NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 04/05/2025 4:34:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](256) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Documents] ADD  DEFAULT (getdate()) FOR [UploadedAt]
GO
ALTER TABLE [dbo].[Equipment] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Feedback] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Logs] ADD  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT ('Unread') FOR [Status]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[RentalRequests] ADD  DEFAULT ('Pending') FOR [Status]
GO
ALTER TABLE [dbo].[RentalRequests] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[RentalTransactions] ADD  DEFAULT ('Pending') FOR [PaymentStatus]
GO
ALTER TABLE [dbo].[RentalTransactions] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ReturnRecords] ADD  DEFAULT ((0.00)) FOR [LateReturnFee]
GO
ALTER TABLE [dbo].[ReturnRecords] ADD  DEFAULT ((0.00)) FOR [AdditionalCharges]
GO
ALTER TABLE [dbo].[ReturnRecords] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Documents]  WITH CHECK ADD FOREIGN KEY([RentalTransactionId])
REFERENCES [dbo].[RentalTransactions] ([Id])
GO
ALTER TABLE [dbo].[Documents]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Equipment]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD FOREIGN KEY([EquipmentId])
REFERENCES [dbo].[Equipment] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Logs]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RentalRequests]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RentalRequests]  WITH CHECK ADD FOREIGN KEY([EquipmentId])
REFERENCES [dbo].[Equipment] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RentalTransactions]  WITH CHECK ADD FOREIGN KEY([AssignedEquipmentId])
REFERENCES [dbo].[Equipment] ([Id])
GO
ALTER TABLE [dbo].[RentalTransactions]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RentalTransactions]  WITH CHECK ADD FOREIGN KEY([RentalRequestId])
REFERENCES [dbo].[RentalRequests] ([Id])
GO
ALTER TABLE [dbo].[ReturnRecords]  WITH CHECK ADD FOREIGN KEY([RentalTransactionId])
REFERENCES [dbo].[RentalTransactions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Equipment]  WITH CHECK ADD  CONSTRAINT [CK__Equipment__Avail__5165187F] CHECK  (([AvailabilityStatus]='Unavailable' OR [AvailabilityStatus]='Under Maintenance' OR [AvailabilityStatus]='Rented' OR [AvailabilityStatus]='Available'))
GO
ALTER TABLE [dbo].[Equipment] CHECK CONSTRAINT [CK__Equipment__Avail__5165187F]
GO
ALTER TABLE [dbo].[Equipment]  WITH CHECK ADD  CONSTRAINT [CK__Equipment__Condi__52593CB8] CHECK  (([ConditionStatus]='Damaged' OR [ConditionStatus]='Poor' OR [ConditionStatus]='Fair' OR [ConditionStatus]='Good' OR [ConditionStatus]='Excellent'))
GO
ALTER TABLE [dbo].[Equipment] CHECK CONSTRAINT [CK__Equipment__Condi__52593CB8]
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD CHECK  (([Rating]>=(1) AND [Rating]<=(5)))
GO
ALTER TABLE [dbo].[Logs]  WITH CHECK ADD CHECK  (([Source]='Desktop' OR [Source]='Web'))
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD CHECK  (([NotificationType]='Overdue Warning' OR [NotificationType]='Return Reminder' OR [NotificationType]='Rental Approved'))
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD CHECK  (([Status]='Unread' OR [Status]='Read'))
GO
ALTER TABLE [dbo].[RentalRequests]  WITH CHECK ADD CHECK  (([Status]='Rejected' OR [Status]='Approved' OR [Status]='Pending'))
GO
ALTER TABLE [dbo].[RentalTransactions]  WITH CHECK ADD CHECK  (([PaymentStatus]='Overdue' OR [PaymentStatus]='Pending' OR [PaymentStatus]='Paid'))
GO
ALTER TABLE [dbo].[ReturnRecords]  WITH CHECK ADD CHECK  (([ReturnCondition]='Lost' OR [ReturnCondition]='Damaged' OR [ReturnCondition]='Good'))
GO
USE [master]
GO
ALTER DATABASE [EquipmentRentalDB] SET  READ_WRITE 
GO
