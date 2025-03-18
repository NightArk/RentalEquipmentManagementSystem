# RentalEquipmentManagementSystem
# **Equipment Rental Management System**  

An advanced **Equipment Rental Management System** built using **Entity Framework Core (Database-First Approach)** and **SQL Server**. This system allows businesses to manage rental transactions, track equipment availability, handle customer requests, and automate notifications efficiently.  

## **Key Features**  
✅ **User Roles & Authentication** – Admins, Rental Managers, and Customers  
✅ **Equipment Catalog** – Categorized rental items with availability tracking  
✅ **Rental Requests & Approvals** – Customers request rentals, managers approve or reject  
✅ **Transaction Management** – Tracks rental fees, deposits, and rental periods  
✅ **Returns & Late Fees** – Monitors returns and applies penalties for late returns or damages  
✅ **Document Management** – Stores rental agreements and ID verification documents  
✅ **Customer Feedback** – Reviews and ratings for rented equipment  
✅ **Automated Notifications** – Payment reminders, return due dates, and system alerts  
✅ **Audit Logs** – Tracks all system actions for security  

## **Tech Stack**  
🔹 **Backend:** C#, .NET Core, Entity Framework Core  
🔹 **Database:** Microsoft SQL Server (Database-First Approach)  
🔹 **ORM:** Entity Framework Core  
🔹 **Development Tools:** SQL Server Management Studio (SSMS), Visual Studio  

## **Setup & Installation**  
1. Clone the repository:  
   ```sh
   git clone https://github.com/your-username/EquipmentRentalSystem.git
   ```
2. Set up the **SQL Server database** using the provided schema.  
3. Update the **connection string** in `appsettings.json`.  
4. Run the **Entity Framework Core scaffold command**:  
   ```sh
   Scaffold-DbContext "Server=YourServer;Database=EquipmentRentalDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Model -Context "ERManagementDBContext" -DataAnnotations
   ```
5. Start the application and enjoy! 🚀  
