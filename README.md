# Employee Portal Application

The **Employee Portal Application** is a .NET Core 9-based MVC Razor application designed to manage employee data efficiently. The application supports importing employee data, validations, authentication, and integration with external APIs. Built using **Clean Architecture**, it ensures scalability and maintainability.

## Features

### Core Features
- **Account Management:**
  - Login and logout user.
- **Employee Management:**
  - Add, edit, delete, and view employee details.
  - Upload and manage profile images.
- **Import Data:**
  - Supports Excel and CSV file uploads.
  - Validates data during upload and shows validation errors in a modal popup.
  - Skips invalid or empty rows and displays a summary of skipped rows.
- **Export Data:**
  - Export selected employee records in Excel, CSV, or PDF formats.
  - Print functionality .
- **Filtering and Sorting:**
  - Filter employees by name, gender, designation, date of birth, salary range.
  - Pagination and sorting are implemented at the database level.
- **Security:**
  - Authorized users can access management and export features.
  - Unauthorized access attempts trigger an email notification to `admin@gmail.com`.

### Additional Features
- Integration with `http://dummy.restapiexample.com/api/v1/employees` to display data in the "Employees From External API" page.
- Custom error pages and global exception handling.
- Login and logout functionality.

## Project Structure

This application follows the **Clean Architecture** design pattern. Below is the directory structure:
- **EmployeePortal.Application** - Business logic, services, and events 
- **EmployeePortal.Domain** - Core entities and domain models 
- **EmployeePortal.Infrastructure** - Database context, and external services integrations 
- **EmployeePortal.UI** - MVC Razor views, controllers, and client-side scripts

### Technologies Used
- **Backend:** ASP.NET Core 9
- **Frontend:** Razor Pages, jQuery, Bootstrap
- **Database:** SQL Server
- **File Handling:** NPOI for Excel
- **API Integration:** HttpClient
- **Authentication:** Cookie authentication

## Setup Instructions

### Prerequisites
1. .NET Core SDK 9
2. SQL Server

### Steps
1. Clone the repository
2. Setup database:
   - Update connection strings in appsettings.json under EmployeePortal.UI.
   - Run migrations:
      Database is auto migrated or you can migrate using following command
      - dotnet ef database update --project EmployeePortal.Infrastructure
3. Build and run the application:
    - dotnet build
    - dotnet run --project EmployeePortal.UI

4. Login
    - Username : admin
    - Password : admin

### Key Functionalities
**Login**
**Employee Management:**
   - Navigate to "Employees" to view, add, edit employees.
**Import/Export:**
   - Import data from Excel/CSV files using the import button.
   - Export selected records using the export button (choose between Excel, CSV, or PDF).
**Search/Filter:**
   - Use filters on the top of the "Employees" page for targeted results.
**Sample API Integration:**
   - Visit the "Employees From External API" page to view employee data fetched from the external API.

### Validation Highlights
- Displays validation messages for incorrect data during import.
- Skips invalid rows during import and provides a summary of skipped rows.
- Prevents unauthorized actions and notifies the admin.
