# Rental Equipment Management System

A comprehensive solution for managing equipment rentals, featuring a desktop application, web-based interface, and backend services for tracking rentals, returns, and equipment inventory.

## 📋 Overview

The Rental Equipment Management System is an integrated platform designed to streamline equipment rental operations. It provides tools for managing equipment inventory, processing rental requests, tracking rental transactions, and handling return records.

## 🏗️ Project Structure

The solution consists of three main components:

### 1. **RentalEquipmentManagementApp** (Windows Forms Desktop Application)
A rich desktop application for equipment rental staff to manage day-to-day operations.

**Key Features:**
- **LoginForm** - User authentication and access control
- **MainDashboardForm** - Central hub for all operations
- **EquipmentManagementForm** - Add, edit, and manage equipment inventory
- **RentalRequestsForm** - View and process customer rental requests
- **RentalTransactionForm** - Record and manage rental transactions
- **ReturnRecordForm** - Track equipment returns and manage return records
- **AuthService** - Authentication and authorization handling

**Key Files:**
- `AuthService.cs` - Authentication logic
- `LoginForm.cs` / `LoginForm.Designer.cs` - Login UI
- `MainDashboardForm.cs` / `MainDashboardForm.Designer.cs` - Dashboard UI
- `EquipmentManagementForm.cs` - Equipment management UI
- `RentalTransactionForm.cs` / `RentalTransactionAdd.cs` - Transaction management
- `ReturnRecordForm.cs` / `ReturnRecordAddForm.cs` - Return record management
- `RentalRequestsForm.cs` - Rental request processing
- `RentalRecordGridItem.cs` / `RentalRequestGridItem.cs` / `RentalTransactionRow.cs` - Grid items for data display

## 💻 Technologies & Tech Stack

### Backend & Framework
- **C#** - Primary programming language
- **.NET Framework / .NET Core** - Application runtime
- **Entity Framework Core** - Object-Relational Mapping (ORM) for database access
- **ASP.NET Core** - Web application framework

### Desktop Application
- **Windows Forms** - UI framework for desktop application
- **GDI+** - Graphics rendering

### Web Application
- **ASP.NET Core MVC** - Web framework architecture
- **Razor Pages** - Server-side templating
- **Bootstrap** - Responsive CSS framework (in wwwroot)

### Database
- **SQL Server** - Relational database management system
- **T-SQL** - Database query language
- **SQL Server Management Studio** - Database administration tool

### Development Tools
- **Visual Studio** / **Visual Studio Code** - IDE
- **.NET CLI** - Command-line interface for .NET
- **NuGet** - Package manager for .NET

### Other Technologies
- **Entity Framework Migrations** - Database schema versioning
- **ASP.NET Identity** - User authentication and authorization

### 2. **RentalEquipmentManagementLogic** (Class Library)
Core business logic and data models shared across the application.

**Key Components:**
- **Models** - Database entities:
  - `User.cs` - User account information
  - `Equipment.cs` - Equipment inventory
  - `Category.cs` - Equipment categories
  - `RentalRequest.cs` - Customer rental requests
  - `RentalTransaction.cs` - Active rental records
  - `ReturnRecord.cs` - Equipment return history
  - `Feedback.cs` - Customer feedback
  - `Notification.cs` - System notifications
  - `Document.cs` - Document storage
  - `Log.cs` - System activity logs

- **Database:**
  - `EquipmentRentalDBContext.cs` - Entity Framework DbContext
  - SQL scripts for database setup

- **DTOs:**
  - `UserDto.cs` - Data transfer object for user data

### 3. **RentalEquipmentManagementWebApp** (ASP.NET Core Web Application)
Web-based interface for customers and administrators.

**Components:**
- **Controllers**:
  - `AccountController.cs` - User account management
  - `AdminController.cs` - Administrative functions
  - `EquipmentController.cs` - Equipment browsing and details
  - `HomeController.cs` - Home page and general navigation

- **Data**:
  - `ApplicationDbContext.cs` - Database context
  - `SeedData.cs` - Initial data population

- **Configuration**:
  - `appsettings.json` / `appsettings.Development.json` - Application settings
  - `Program.cs` - Application startup configuration

## 🗄️ Database

The system uses SQL Server with two databases:

- **EquipmentRentalDB** - Main application data (equipment, rentals, returns)
- **EquipmentRentalIdentityDB** - User authentication and identity data

Database setup scripts are located in the `Sql Querries/` folder:
- `EquipmentRentalDB.sql` - Main database schema
- `EquipmentRentalIdentityDB.sql` - Identity database schema

## 🚀 Getting Started

### Prerequisites
- .NET Framework or .NET Core (version as specified in .csproj files)
- SQL Server (local or remote)
- Visual Studio or Visual Studio Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd RentalEquipmentManagementSystem
   ```

2. **Set up the database**
   - Execute the SQL scripts from the `Sql Querries/` folder in SQL Server Management Studio
   - Update connection strings in `appsettings.json` files

3. **Build the solution**
   ```bash
   dotnet build RentalEquipmentManagementSystem.sln
   ```

4. **Run the desktop application**
   - Set `RentalEquipmentManagementApp` as the startup project
   - Press F5 or use `dotnet run`

5. **Run the web application**
   - Set `RentalEquipmentManagementWebApp` as the startup project
   - Press F5 or use `dotnet run`

## 📋 Features

### Desktop Application
- ✅ User authentication and role-based access
- ✅ Equipment inventory management
- ✅ Rental request processing
- ✅ Transaction recording
- ✅ Return record management
- ✅ Dashboard with key metrics

### Web Application
- ✅ User account management
- ✅ Equipment browsing and search
- ✅ Admin dashboard
- ✅ Responsive design

## 🔐 Security

- User authentication and authorization
- Role-based access control
- Input validation
- SQL injection prevention through Entity Framework

## 📝 Development Notes

- The solution uses Entity Framework for database access
- Windows Forms for the desktop UI
- ASP.NET Core for the web application
- Separation of concerns with dedicated business logic layer

## 📦 Dependencies

Core dependencies include:
- Entity Framework Core
- ASP.NET Core (for web app)
- Windows Forms (for desktop app)

See individual `.csproj` files for complete dependency lists.

## 🤝 Contributing

Guidelines for contributing to the project:
1. Follow the existing code structure and naming conventions
2. Create separate branches for new features
3. Test thoroughly before submitting changes
4. Update documentation as needed

## 📄 License

[Add your license information here]

## 📧 Support

For questions or issues, please contact the development team or submit an issue through the project repository.