MOBİLE PROVİDER BILLING API - MİDTERM PROJECT

This is a REST API developed for a mobile provider's billing system as a midterm assignment. It handles mobile usage tracking, bill calculation, and authentication through JWT.

Swagger UI
Swagger is available at: https://localhost:7291/swagger/index.html

//Technologies Used

-ASP.NET Core 8.0

-Entity Framework Core (In-Memory Database)

-JWT Authentication (with Bearer token)

-Swagger / OpenAPI

//NuGet Packages Used

-Microsoft.AspNetCore.Authentication.JwtBearer

-Microsoft.EntityFrameworkCore.InMemory

-Swashbuckle.AspNetCore

//Authentication

To access protected endpoints, login using the credentials below to receive a JWT token.
POST /api/auth/login
{
  "username": "admin",
  "password": "1234"
}
You will receive a token and To use this token, click the Authorize button on the Swagger UI and enter: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9....

Method     Endpoint               Description

Post       /api/auth/login       Login to get JWT token
Post       /api/usage            Add phone or internet usage    
Post       /api/usage/calculate  Calculates bill based on usage
Get        /api/bill             Returns basic bill info
Get        /api/bill/detail      Returns detailed bill breakdown
Post       /api/bill/pay         Simulates bill payment

Ex:
POST /api/bill/pay
{
  "subscriberNo": "12345",
  "month": 4,
  "year": 2025,
  "amountPaid": 50
}

{
  "message": "Payment successful. Bill is fully paid.",
  "paid": 50,
  "remaining": 0
}
