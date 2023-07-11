<p align="center">
    <a href="https://imgbb.com/"><img src="https://i.ibb.co/YkKXTrm/00.png" alt="00" border="0"></a>
</p>

<p align="center">
  <img alt="Static Badge" src="https://img.shields.io/badge/ASP.NET%20-%20.NET%206%20-%20important?style=flat">
  <img alt="Static Badge" src="https://img.shields.io/badge/IdentityServer%20-%20v4.1.2%20-%20red?style=flat">
  <img alt="Static Badge" src="https://img.shields.io/badge/Hangfire%20-%20v1.8.3%20-%20%2300BFFF?style=flat">
</p>

# About

Task Manager X (TMX) is a web API for task management, developed as a backend-focused pet project. TMX provides a robust set of APIs for creating, 
tracking, and managing tasks, setting priorities, defining due dates, and receiving notifications for approaching deadlines. 
The project utilizes modern technologies and tools such as ASP.NET Core, Entity Framework, Hangfire, FluentValidation, and more. 
Please note that TMX does not include a frontend user interface and is intended to be integrated with other frontend applications. 
Additionally, the project incorporates authentication and authorization using Identity Server for secure access control.

## Documentation

### Overview
Task Manager X (TMX) is a web API for task management, designed as a backend-focused pet project. TMX provides a comprehensive set of APIs that allow users to create, track, and manage their tasks efficiently. The project is built using modern technologies and tools, including ASP.NET Core, Entity Framework, Hangfire, FluentValidation, and more.

### Features
1. Task Management: TMX enables users to create new tasks, update their status, set priorities, and define due dates.
2. Task Tracking: Users can easily track the progress of their tasks, monitor deadlines, and receive notifications for approaching due dates.
3. Authentication and Authorization: TMX incorporates Identity Server for secure access control, ensuring that only authorized users can access and manage their tasks.
4. Integration: TMX is designed to be integrated with frontend applications, providing a powerful backend solution for task management.

### Getting Started
To get started with TMX, follow these steps:

1. Clone the TMX repository to your local machine.
2. Configure the necessary environment variables and connection strings for your database and SMTP server (appsettings.json).
3. Build and run the TMX project.
4. Since TMX is developed by a junior backend developer, this project doesn't have a frontend part, and the interaction interface will be presented through Swagger UI in your browser..

### Known Issues
The "Logout" button in Swagger UI may not work as expected. As this issue is inherent to Swagger UI, it cannot be fixed directly. To log out, manually enter the logout URL: https://localhost:7002/connect/endsession.

### API Documentation
TMX exposes a set of APIs for task management. Below are the key endpoints:

- `GET /api/tasks`: Retrieve all tasks.
- `POST /api/tasks`: Create a new task.
- `GET /api/tasks/{id}`: Retrieve a specific task by its ID.
- `PUT /api/tasks/{id}`: Update an existing task.
- `DELETE /api/tasks/{id}`: Delete a task.



## License
TMX is released under the MIT License.

