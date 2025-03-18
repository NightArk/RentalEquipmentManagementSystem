CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) CHECK (Role IN ('Admin', 'RentalManager', 'Customer')) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) UNIQUE NOT NULL,
    Description NVARCHAR(500)
);

CREATE TABLE Equipment (
    EquipmentID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(500),
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID) ON DELETE SET NULL,
    RentalPrice DECIMAL(10,2) NOT NULL,
    AvailabilityStatus NVARCHAR(50) CHECK (AvailabilityStatus IN ('Available', 'Rented', 'UnderMaintenance')) NOT NULL,
    ConditionStatus NVARCHAR(50) CHECK (ConditionStatus IN ('New', 'Good', 'Damaged', 'Lost')) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE RentalTransactions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RentalRequestId INT FOREIGN KEY REFERENCES RentalRequests(Id) ON DELETE NO ACTION,
    AssignedEquipmentId INT FOREIGN KEY REFERENCES Equipment(Id) ON DELETE NO ACTION,
    CustomerId INT FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    ActualRentalStartDate DATETIME NOT NULL,
    ReturnDate DATETIME NOT NULL,
    RentalPeriod INT NOT NULL,  
    RentalFee DECIMAL(10,2) NOT NULL,
    Deposit DECIMAL(10,2) NOT NULL,
    PaymentStatus NVARCHAR(50) CHECK (PaymentStatus IN ('Paid', 'Pending', 'Overdue')) DEFAULT 'Pending',
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE ReturnRecords (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RentalTransactionId INT FOREIGN KEY REFERENCES RentalTransactions(Id) ON DELETE CASCADE,
    ActualReturnDate DATETIME NOT NULL,
    ReturnCondition NVARCHAR(50) CHECK (ReturnCondition IN ('Good', 'Damaged', 'Lost')) NOT NULL,
    LateReturnFee DECIMAL(10,2) DEFAULT 0.00,
    AdditionalCharges DECIMAL(10,2) DEFAULT 0.00,
    CreatedAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE Feedback (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    EquipmentId INT FOREIGN KEY REFERENCES Equipment(Id) ON DELETE CASCADE,
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(500) NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE Documents (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RentalTransactionId INT FOREIGN KEY REFERENCES RentalTransactions(Id) ON DELETE NO ACTION,
    UserId INT FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    FileName NVARCHAR(255) NOT NULL,
    FileType NVARCHAR(50) NOT NULL,
    StoragePath NVARCHAR(500) NOT NULL,
    UploadedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Notifications (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    MessageContent NVARCHAR(500) NOT NULL,
    NotificationType NVARCHAR(50) CHECK (NotificationType IN ('Rental Approved', 'Return Reminder', 'Overdue Warning')) NOT NULL,
    Status NVARCHAR(50) CHECK (Status IN ('Read', 'Unread')) DEFAULT 'Unread',
    CreatedAt DATETIME DEFAULT GETDATE()
);
CREATE TABLE Logs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT FOREIGN KEY REFERENCES Users(Id) ON DELETE SET NULL,
    Action NVARCHAR(255) NOT NULL,
    Timestamp DATETIME DEFAULT GETDATE(),
    AffectedData NVARCHAR(255) NULL,
    Source NVARCHAR(50) CHECK (Source IN ('Web', 'Desktop'))
);
