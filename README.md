# EmployeeManagementAPI

EmployeeManagementAPI is a comprehensive .NET Core Web API designed to manage various aspects of employee data within an organization. This API is part of an Employee Management System, offering a range of features from basic CRUD operations to more complex functionalities such as authentication, authorization, hierarchical data management, project management, and task management.

## Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Project Structure](#project-structure)
- [Configuration](#configuration)
- [Database](#database)
- [Authentication](#authentication)
- [Usage](#usage)
  - [API Endpoints](#api-endpoints)
- [Error Handling](#error-handling)
- [Logging](#logging)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Features

- **Employee Management**: Full CRUD (Create, Read, Update, Delete) operations on employee records.
- **Department Management**: Handle operations related to departments within the organization.
- **Project Management**: Manage projects, including project creation, updates, and deletion.
- **Task Management**: Handle tasks within projects, including task assignments, status updates, and more.
- **Authentication & Authorization**: Secure API endpoints using JWT-based authentication.
- **Model Validation**: Ensure data integrity with comprehensive validation rules.
- **Error Handling**: Centralized error handling for consistent API responses.
- **Logging**: Integrated logging for tracking API requests and troubleshooting.

## Technologies Used

- **.NET Core 7**: The core framework used for building the API.
- **Entity Framework Core**: ORM (Object-Relational Mapping) for database interactions.
- **SQL Server**: Database management system used for storing data.
- **Swagger**: Tool for API documentation and testing.
- **JWT (JSON Web Token)**: Used for securing API endpoints through authentication and authorization.
- **AutoMapper**: Simplifies the mapping of objects to DTOs (Data Transfer Objects).
- **Serilog**: A logging library that allows for structured logging.

## Getting Started

### Prerequisites

Ensure that the following software is installed on your machine:

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or another compatible IDE.
- [Postman](https://www.postman.com/) for testing the API.

### Installation

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/princeragh1801/EmployeeManagementAPI.git
   cd EmployeeManagementAPI
2. **Restore Dependencies:**

   ```bash
   dotnet restore
### Database Setup

1. **Update the Connection String:**

   - Open the `appsettings.json` file.
   - Locate the `ConnectionStrings` section.
   - Update the connection string to match your SQL Server configuration.

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=YOUR_DATABASE_NAME;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;"
   }

2. **Apply Migrations**

Run the following command to apply migrations and set up the database schema:

    ```bash
    dotnet ef database update

### Run the Application

To start the application, use the following command:

    ```bash
    dotnet run

## Project Structure

The project follows a clean architecture with the following key directories:

- **Controllers**: Contains API controllers that handle incoming HTTP requests.
- **Models**: Defines the data models and DTOs (Data Transfer Objects).
- **Services**: Contains business logic and service classes that interact with the repositories and controllers.
- **Repositories**: Manages data access logic, interacting with the database using Entity Framework Core.
- **Migrations**: Contains migration files that track changes to the database schema.

## Configuration

The application configuration is managed through `appsettings.json`. Key configurations include:

- **ConnectionStrings**: Database connection strings.
- **JWT**: Settings related to JWT authentication, such as the secret key and token expiration.
- **Logging**: Configuration for Serilog to handle logging.

## Database

The application uses Entity Framework Core for database interactions. The database schema includes tables for:

- **Employees**: Stores employee details such as name, department, and contact information.
- **Departments**: Stores information related to different departments within the organization.
- **Projects**: Manages project-related data, including project details, timelines, and statuses.
- **Tasks**: Handles task-related data within projects, including task assignments, progress tracking, and deadlines.
- **Users**: Handles authentication and authorization data.

## Usage

### API Endpoints

Here is an overview of the main API endpoints:

- **Authentication:**
  - `POST /api/auth/login`: Authenticates a user and returns a JWT.

- **Employees:**
  - `GET /api/employees`: Retrieves all employees.
  - `GET /api/employees/{id}`: Retrieves a specific employee by ID.
  - `POST /api/employees`: Creates a new employee.
  - `PUT /api/employees/{id}`: Updates an existing employee.
  - `DELETE /api/employees/{id}`: Deletes an employee.

- **Departments:**
  - `GET /api/departments`: Retrieves all departments.
  - `GET /api/departments/{id}`: Retrieves a specific department by ID.
  - `POST /api/departments`: Creates a new department.
  - `PUT /api/departments/{id}`: Updates an existing department.
  - `DELETE /api/departments/{id}`: Deletes a department.

- **Projects:**
  - `GET /api/projects`: Retrieves all projects.
  - `GET /api/projects/{id}`: Retrieves a specific project by ID.
  - `POST /api/projects`: Creates a new project.
  - `PUT /api/projects/{id}`: Updates an existing project.
  - `DELETE /api/projects/{id}`: Deletes a project.

- **Tasks:**
  - `GET /api/tasks`: Retrieves all tasks.
  - `GET /api/tasks/{id}`: Retrieves a specific task by ID.
  - `POST /api/tasks`: Creates a new task.
  - `PUT /api/tasks/{id}`: Updates an existing task.
  - `DELETE /api/tasks/{id}`: Deletes a task.

## Error Handling

The API implements centralized error handling using middleware. This ensures that all exceptions are captured and returned in a consistent format, improving the developer experience during debugging and integration.

## Logging

Logging is handled using Serilog, which provides structured logging. Logs are written to the console, files, or other configured sinks. This is crucial for tracking API usage and diagnosing issues.

## Contributing

Contributions are welcome! To contribute:

- Fork the repository.
- Create a feature branch.
- Commit your changes.
- Push to the branch.
- Open a pull request.
