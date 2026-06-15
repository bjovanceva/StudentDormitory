# Student Dormitory System

A student dormitory management web application built with ASP.NET Core (.NET 8). The solution includes a web project (`StudentDormitoryApp.Web`) with Razor Pages and MVC controllers, a service layer, repository layer, and Identity-based authentication.

## Key features
- Student registration and login (ASP.NET Core Identity)
- Room listing and availability
- Application submission with document upload
- Stripe payment flow for application fees
- Email notifications (SMTP) with PDF attachment of the application
- Referent dashboard to review, approve, or reject applications

## Prerequisites
- .NET 8 SDK
- Visual Studio 2022 (latest updates) or `dotnet` CLI
- PostgreSQL (recommended for production) or SQL Server LocalDB for quick local testing
- An SMTP account for sending email (e.g., Gmail SMTP)
- Stripe account for payment processing (test keys OK for development)

## Configuration

Required settings:
- Database connection: set `ConnectionStrings:DefaultConnection`
- SMTP: configure `SmtpSettings` (server, port, username, password)
- Stripe: set publishable and secret keys

## Run the application

From Visual Studio 2022:
1. Open the solution.
2. Ensure environment variables or `.env` are configured.
3. Set `StudentDormitoryApp.Web` as the startup project.
4. Click Start Debugging (green play button)
