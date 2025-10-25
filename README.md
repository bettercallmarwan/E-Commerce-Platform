# E-Commerce Platform: Scalable Full Stack Web App

<p align="center">
  <img src="assets/ecommerce-preview.png" alt="E-Commerce Platform Preview" width="550">
</p>

<p align="center">
  <strong>Full-Stack E-Commerce Solution with Clean Architecture</strong><br>
  A comprehensive e-commerce application built with ASP.NET Core Web API and Angular, featuring secure payments, role-based access control, and a modern shopping experience.
</p>

---

## Table of Contents

- [Features](#features)
- [Project Structure](#project-structure)
- [Tech Stack](#tech-stack)
- [Requirements](#requirements)
- [Setup](#setup)
- [Usage](#usage)
- [Key Components](#key-components)

---

## Features

- **Clean Architecture:** Separation of concerns with layered architecture for maintainability and scalability.
- **Authentication & Authorization:** JWT-based authentication with role-based access control (Admin/User).
- **Product Management:** Full CRUD operations with pagination, filtering, and search capabilities.
- **Shopping Cart:** Redis-cached basket system for fast and efficient cart management.
- **Secure Checkout:** Integrated Stripe payment processing (both client and server-side).
- **Order Management:** Complete order tracking and history with secure endpoints.
- **Admin Dashboard:** Comprehensive user and product management interface.
- **Entity Auditing:** Automatic tracking of entity changes with CreatedBy/LastModifiedBy metadata.
- **Generic Repository Pattern:** Clean data access layer with Unit of Work implementation.
- **Request Validation:** FluentValidation for robust input validation.
- **Error Handling:** Centralized middleware for consistent error responses.
- **Database Seeding:** Automated setup with initial products, users, and roles.

---

## Project Structure

```
LinkDev.Talabat/
├── LinkDev.Talabat.APIs/
│   ├── Extensions/
│   ├── Middlewares/
│   └── Program.cs
├── LinkDev.Talabat.APIs.Controllers/
│   ├── Controllers/
│   ├── Errors/
│   ├── Mapping/
│   └── Models/
├── LinkDev.Talabat.Core.Application/
│   ├── Exceptions/
│   ├── Extensions/
│   ├── Mapping/
│   └── Services/
├── LinkDev.Talabat.Core.Application.Abstraction/
│   ├── Common/
│   ├── Models/
│   └── Services/
├── LinkDev.Talabat.Core.Domain/
│   ├── Common/
│   ├── Contracts/
│   ├── Entities/
│   └── Specifications/
├── LinkDev.Talabat.Infrastructure.Presistence/
│   ├── _Common/
│   ├── _Data/
│   ├── _Identity/
│   ├── GenericRepository/
│   └── UnitOfWork/
├── LinkDev.Talabat.Infrastructure/
│   ├── BasketRepository/
│   └── PaymentService/
└── LinkDev.Talabat.Shared/
    └── Models/
```

---

## Tech Stack

### Backend
- **ASP.NET Core 8 Web API** - RESTful API framework
- **Entity Framework Core** - ORM with SQL Server
- **ASP.NET Identity** - User authentication and management
- **JWT Bearer Authentication** - Token-based security
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Request validation
- **Redis** - Distributed caching for shopping basket
- **Stripe API** - Payment processing
- **Clean Architecture** - Maintainable code structure
- **Repository & Unit of Work Pattern** - Data access abstraction

### Frontend
- **Angular** (TypeScript) - SPA framework
- **Angular Material** - UI component library

---

## Requirements

### System Requirements
- **.NET 8.0 SDK** or later
- **Node.js 18+** and **npm**
- **SQL Server**
- **Redis**
- **Angular CLI**

### API Keys
- **Stripe API Keys**

---

## Setup

### 1. Clone Repository
```bash
git clone https://github.com/yourusername/ecommerce-platform.git
cd ecommerce-platform
```

### 2. Backend Setup

#### Install Dependencies
```bash
cd backend
dotnet restore
```

#### Install and Start Redis

**Windows:**
```bash
# Using Chocolatey
choco install redis-64

# Start Redis
redis-server
```

#### Database Configuration

Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TalabatDB;Trusted_Connection=True;TrustServerCertificate=True",
    "IdentityConnection": "Server=.;Database=TalabatIdentityDB;Trusted_Connection=True;TrustServerCertificate=True",
    "Redis": "localhost:6379"
  },
  "Stripe": {
    "SecretKey": "sk_test_your_stripe_secret_key",
    "PublishableKey": "pk_test_your_stripe_publishable_key"
  },
  "JWT": {
    "SecretKey": "your_jwt_secret_key_min_32_characters",
    "Issuer": "TalabatAPI",
    "Audience": "TalabatClient",
    "DurationInMinutes": 60
  }
}
```

#### Run Backend
```bash
cd LinkDev.Talabat.APIs
dotnet run
```

### 3. Frontend Setup

```bash
cd frontend
npm install
```
---

## Usage

### API Endpoints

#### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login and receive JWT token
- `GET /api/auth/current-user` - Get current user info

#### Products
- `GET /api/products` - Get all products (with pagination, sorting, filtering)
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create product (Admin only)
- `PUT /api/products/{id}` - Update product (Admin only)
- `DELETE /api/products/{id}` - Delete product (Admin only)

#### Basket (Shopping Cart)
- `GET /api/basket/{id}` - Get user's basket
- `POST /api/basket` - Create or update basket
- `DELETE /api/basket/{id}` - Delete basket

#### Orders
- `POST /api/orders` - Create new order
- `GET /api/orders` - Get user's orders
- `GET /api/orders/{id}` - Get order by ID

#### Payment
- `POST /api/payment/{basketId}` - Create or update payment intent
- `POST /api/payment/webhook` - Stripe webhook handler

---

## Key Components

### 1. Clean Architecture Layers

**Core.Domain:**
- Entities and domain models
- Business rules and specifications
- Domain contracts and interfaces

**Core.Application:**
- Business logic and services
- DTOs and mapping profiles
- Application-level exceptions

**Infrastructure:**
- Data persistence implementation
- External service integrations
- Repository implementations
- Redis caching
- Payment service

**APIs:**
- REST controllers
- Middleware components
- Dependency injection configuration

### 2. Authentication & Authorization (`ASP.NET Identity + JWT`)
- Token-based authentication
- Role-based authorization (Admin/User)
- Secure password hashing
- Token refresh mechanism

### 3. Generic Repository Pattern (`GenericRepository`)
- Abstraction over data access
- Specification pattern for queries
- Unit of Work for transaction management
- Reduces code duplication

### 4. Caching Service (`RedisService`)
- Distributed basket storage
- Fast cart retrieval
- Session management
- Scalable across multiple servers

### 5. Payment Service (`PaymentService`)
- Stripe integration
- Payment intent creation
- Webhook handling
- Secure transaction processing

### 6. Entity Auditing Interceptor
- Automatic timestamp tracking
- User action logging
- `CreatedBy`, `CreatedOn`, `LastModifiedBy`, `LastModifiedOn`
- Transparent implementation

### 7. AutoMapper Configuration
- DTO to Entity mapping
- Reduces boilerplate code
- Centralized mapping profiles
- Type-safe transformations

### 8. Error Handling Middleware
- Centralized exception handling
- Consistent error responses
- Logging integration
- User-friendly error messages

### 9. FluentValidation
- Request validation
- Custom validation rules
- Clear error messages
- Separation of validation logic

### 10. Data Seeding
- Initial database setup
- Sample products and categories
- Default users and roles
- Development data

---
