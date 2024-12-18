# Library Management System - Backend

## Overview
This repository contains the backend API for the Library Management System. It is a RESTful API developed using C# .NET Core and SQLite to support CRUD operations for managing book records.

## Features
- CRUD operations for books:
  - **POST /books**: Create a new book record.
  - **GET /books**: Retrieve all book records.
  - **GET /books/{id}**: Retrieve a single book record by ID.
  - **PUT /books/{id}**: Update a specific book record.
  - **DELETE /books/{id}**: Delete a book record.
  - **POST /auth/login**: Login to the system.
  - **POST /auth/register**: Register new user.
- Input validation for book funtions and the auth functions.
- Comprehensive error handling with appropriate HTTP status codes.
- API documentation with Swagger.

## Technologies Used
- **Language**: C#
- **Framework**: .NET Core
- **Database**: SQLite (using Entity Framework Core)
- **Tools**: Visual Studio, Hoppscotch (for API testing)

## Prerequisites
- Visual Studio 2019 or later
- .NET SDK (6.0 or later)
- SQLite database

## Setup and Installation
1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd library-management-backend
   ```
2. Configure the SQLite database in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=library.db"
     }
   }
   ```
3. Run database migrations:
   ```bash
   dotnet ef database update
   ```
4. Build and run the application:
   ```bash
   dotnet run
   ```

## API Documentation
API documentation is available via Swagger:
- Navigate to `http://localhost:<port>/swagger` while the application is running.

## Additional Features
- User authentication with JSON Web Tokens (JWT).

## Known Issues
- None reported.

## Contributing
Contributions are welcome! Please create a pull request with detailed information about your changes.

## License
This project is licensed under the MIT License.
