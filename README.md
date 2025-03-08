Online Course Registration API

Overview

This project is a backend API for an online course registration system. It allows users to register, enroll in courses, and manage course content. The API also includes role-based authorization for admins, instructors, and students.

Features

User Authentication & Authorization

JWT-based authentication

Role-based access control (Admin, Instructor, Student)

User registration & login

Course Management

Create, update, and delete courses (Admin, Instructor)

Assign instructors to courses (Admin)

Browse available courses (Students)

Student Enrollment

Students can enroll in courses

Technologies Used

Backend: ASP.NET Core Web API (.NET 8)

Authentication: ASP.NET Core Identity & JWT

Database: SQL Server  with Entity Framework Core


Frontend: Blazor (Planned Integration)

Installation & Setup

1️⃣ Clone the Repository

git clone https://github.com/yourusername/OnlineCourseAPI.git
cd OnlineCourseAPI

2️⃣ Configure Database Connection

Modify appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=CourseRegistrationdb;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
}

3️⃣ Run Database Migrations

dotnet ef database update

4️⃣ Run the API

dotnet run

API will be available at http://localhost:5000.

API Endpoints

Authentication

POST /api/auth/register - Register a new user

POST /api/auth/login - Login and get JWT token

Admin

GET /api/admin/users - Get all users

POST /api/admin/courses - Create a course

PUT /api/admin/courses/{id} - Update a course

DELETE /api/admin/courses/{id} - Delete a course

Instructor

GET /api/instructor/courses - View assigned courses

POST /api/instructor/courses/{id}/content - Add course content

Student

GET /api/courses - Browse available courses

POST /api/courses/{id}/enroll - Enroll in a course

GET /api/courses/progress - Track progress

Payments

POST /api/payments - Process payment for a course

Authentication & Authorization

JWT tokens are required for accessing most API endpoints.

Admin: Full access to manage courses and users.

Instructor: Can manage their assigned courses.

Student: Can browse and enroll in courses.

Contribution

Fork the repo & create a new branch

Make your changes & commit

Submit a pull request
