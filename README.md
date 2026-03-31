# Hospital Management API

I built this project to learn backend development and improve my .NET skills.
It's a REST API for managing hospital operations like patients, doctors and appointments.

## What I used
- ASP.NET Core 10
- Entity Framework Core + SQL Server
- JWT Authentication
- Clean Architecture
- Swagger

## What it does
- Register and manage patients and doctors
- Book appointments with conflict checking
- Role based access - Admin, Doctor, Patient
- Secure endpoints with JWT tokens

## How to run it

1. Clone the repo
2. Update connection string in appsettings.json to point to your SQL Server
3. Run migrations: dotnet ef database update
4. Run the app: dotnet run
5. Open http://localhost:5183 to see Swagger

## What I learned
- How Clean Architecture separates concerns into layers
- How JWT tokens work for authentication
- How Entity Framework Core maps C# classes to database tables
- How to build production-quality APIs with proper error handling

## Folder structure
- Domain: entities and interfaces
- Application: business logic and services  
- Infrastructure: database and repositories
- API: controllers and middleware
