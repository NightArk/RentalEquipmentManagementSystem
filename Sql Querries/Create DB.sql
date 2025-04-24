CREATE DATABASE EquipmentRentalDB;
GO
USE [EquipmentRentalDB]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 3/26/2025 4:06:26 AM ******/
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
/****** Object:  Table [dbo].[Documents]    Script Date: 3/26/2025 4:06:27 AM ******/
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
/****** Object:  Table [dbo].[Equipment]    Script Date: 3/26/2025 4:06:27 AM ******/
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
/****** Object:  Table [dbo].[Feedback]    Script Date: 3/26/2025 4:06:27 AM ******/
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
/****** Object:  Table [dbo].[Logs]    Script Date: 3/26/2025 4:06:27 AM ******/
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
/****** Object:  Table [dbo].[Notifications]    Script Date: 3/26/2025 4:06:27 AM ******/
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
/****** Object:  Table [dbo].[RentalRequests]    Script Date: 3/26/2025 4:06:27 AM ******/
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
/****** Object:  Table [dbo].[RentalTransactions]    Script Date: 3/26/2025 4:06:27 AM ******/
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
/****** Object:  Table [dbo].[ReturnRecords]    Script Date: 3/26/2025 4:06:27 AM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 3/26/2025 4:06:27 AM ******/
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
ALTER TABLE [dbo].[Equipment]  WITH CHECK ADD CHECK  (([AvailabilityStatus]='Under Maintenance' OR [AvailabilityStatus]='Unavailable' OR [AvailabilityStatus]='Available'))
GO
ALTER TABLE [dbo].[Equipment]  WITH CHECK ADD CHECK  (([ConditionStatus]='Damaged' OR [ConditionStatus]='Good' OR [ConditionStatus]='New'))
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


INSERT INTO [dbo].[Categories] (Name, Description) VALUES
('Cameras', 'Digital and DSLR cameras'),
('Lighting', 'LED and flash lighting equipment'),
('Audio', 'Microphones and audio recorders'),
('Tripods', 'Camera tripods and stabilizers'),
('Drones', 'Aerial photography drones');


INSERT INTO [dbo].[Users] (Name, Email, PasswordHash, Role) VALUES
('Alice Smith', 'alice@example.com', 'root', 'Customer'),
('Bob Johnson', 'bob@example.com', 'root', 'Customer'),
('admin', 'Admin@example.com', 'root', 'Admin'),
('Diana Cruz', 'diana@example.com', 'root', 'Customer'),
('Ethan Scott', 'ethan@example.com', 'root', 'Customer'),
('manager', 'Manager@example.com', 'root', 'Manager');


INSERT INTO [dbo].[Equipment] (Name, Description, CategoryId, RentalPrice, AvailabilityStatus, ConditionStatus) VALUES
('Canon EOS R5', 'High-end mirrorless camera', 1, 150.00, 'Available', 'New'),
('Sony A7 III', 'Professional mirrorless camera', 1, 100.00, 'Available', 'Good'),
('Godox SL60W', 'LED video light', 2, 30.00, 'Under Maintenance', 'Good'),
('Zoom H6', 'Portable audio recorder', 3, 25.00, 'Available', 'Good'),
('DJI Mavic Air 2', 'Compact drone with 4K camera', 5, 200.00, 'Unavailable', 'Damaged');

INSERT INTO [dbo].[RentalRequests] (CustomerId, EquipmentId, RentalStartDate, ReturnDate, TotalCost) VALUES
(1, 1, '2025-04-20', '2025-04-22', 300.00),
(2, 2, '2025-04-19', '2025-04-21', 200.00),
(4, 4, '2025-04-18', '2025-04-20', 50.00),
(5, 5, '2025-04-21', '2025-04-24', 600.00),
(1, 3, '2025-04-20', '2025-04-20', 30.00);

INSERT INTO [dbo].[RentalTransactions] (RentalRequestId, AssignedEquipmentId, CustomerId, ActualRentalStartDate, ReturnDate, RentalPeriod, RentalFee, Deposit) VALUES
(1, 1, 1, '2025-04-20', '2025-04-22', 2, 300.00, 100.00),
(2, 2, 2, '2025-04-19', '2025-04-21', 2, 200.00, 80.00),
(3, 4, 4, '2025-04-18', '2025-04-20', 2, 50.00, 30.00),
(4, 5, 5, '2025-04-21', '2025-04-24', 3, 600.00, 150.00),
(5, 3, 1, '2025-04-20', '2025-04-20', 0, 30.00, 10.00);

INSERT INTO [dbo].[ReturnRecords] (RentalTransactionId, ActualReturnDate, ReturnCondition) VALUES
(1, '2025-04-22', 'Good'),
(2, '2025-04-22', 'Damaged'),
(3, '2025-04-21', 'Good'),
(4, '2025-04-25', 'Good'),
(5, '2025-04-20', 'Lost');

INSERT INTO [dbo].[Feedback] (UserId, EquipmentId, Rating, Comment) VALUES
(1, 1, 5, 'Excellent camera, highly recommend!'),
(2, 2, 4, 'Very good, minor scratches though'),
(4, 4, 5, 'Audio quality was top notch'),
(5, 5, 2, 'Drone was not working properly'),
(1, 3, 4, 'Lights were bright and effective');


INSERT INTO [dbo].[Documents] (RentalTransactionId, UserId, FileName, FileType, StoragePath) VALUES
(1, 1, 'rental_agreement1.pdf', 'PDF', '/docs/rentals/rental_agreement1.pdf'),
(2, 2, 'rental_agreement2.pdf', 'PDF', '/docs/rentals/rental_agreement2.pdf'),
(3, 4, 'receipt_audio.pdf', 'PDF', '/docs/rentals/receipt_audio.pdf'),
(4, 5, 'invoice_drone.pdf', 'PDF', '/docs/rentals/invoice_drone.pdf'),
(5, 1, 'rental_terms_light.pdf', 'PDF', '/docs/rentals/rental_terms_light.pdf');

INSERT INTO [dbo].[Logs] (UserId, Action, AffectedData, Source) VALUES
(1, 'Created Rental Request', 'RentalRequest ID 1', 'Web'),
(2, 'Uploaded Document', 'Document ID 2', 'Desktop'),
(4, 'Submitted Feedback', 'Feedback ID 3', 'Web'),
(5, 'Checked Notification', 'Notification ID 4', 'Desktop'),
(1, 'Updated Profile', 'User ID 1', 'Web');

INSERT INTO [dbo].[Notifications] (UserId, MessageContent, NotificationType) VALUES
(1, 'Your rental request has been approved.', 'Rental Approved'),
(2, 'Reminder: Equipment is due tomorrow.', 'Return Reminder'),
(4, 'Warning: Equipment is overdue.', 'Overdue Warning'),
(5, 'Reminder: Please return the drone.', 'Return Reminder'),
(1, 'Your rental transaction is complete.', 'Rental Approved');
