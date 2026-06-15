#  Book Store Management 

A .NET 10 Console Application using Entity Framework Core, built with Clean Architecture.

## Applied Patterns & Architecture
* **Clean (Onion) Architecture**
* **Generic Repository & Unit of Work (UoW)**
* **Specification Pattern:** Handled filtering, pagination, and solved the N+1 problem.
* **CQRS Concept:** Commands via Repositories, Queries directly via DbContext (`AsNoTracking`) for maximum performance.

## How to Run

1. **Database Connection:** Check the connection string in `Presentation/appsettings.json`. It uses a local SQL Server instance by default.

2. **Apply Migrations:** Open Package Manager Console (PMC) in Visual Studio, set the **Default Project** to `Persistence`, and run:
   ```powershell
   Update-Database
   ```
3-Start the App: Set Presentation as the Startup Project and hit Run.
 (Note: The database will automatically seed sample data on the first run, and the Interactive Dashboard will launch).
