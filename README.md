# Departments API
A .NET Core API that reads and exposes department hierarchy data from configured files. Built with clean architecture principles using a Business Layer Service for reading files and a controller that serves the data over HTTP requests.

## Structure
- Departments.API
- Departments.BusinessLayer
- Departments.Tests

## Endpoint
GET Departments/hierarchy

## Features
- Parses a flat file retrieved from the disk and returns hierarchical department data in JSON format.
- Uses dependency injection for a clean service structure.
- Utilizes asynchronous principles to improve performance.
- Environment-based configuration via appsettings.json.
- Supports any depth in the department hierarchy structure.
- Includes unit tests for controllers and class libraries.
- Global error message handling using middleware.

## File Location
- The default file location is set in the appsettings.json: "DepartmentFiles:Path": "./Files/Complex"
- There are two sample files in the project under the folders Complex and Simple.
- You can override this file location by setting your own in Department.Api/usersecrets.json, for example: "DepartmentFiles:Path": "C://Test/MyFile"


