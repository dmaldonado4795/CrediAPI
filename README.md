# Financial Products and Loan Amortization API

This repository contains the backend component developed as a technical test solution for a Senior Developer role. It implements a RESTful API for managing financial products and calculating loan payment plans, adhering to specified technical requirements and best practices.

## 🎯 Project Goal

To evaluate technical capability in designing and implementing a complete solution using a .NET Framework REST API, focusing on secure authentication, robust business logic, and efficient data interaction.

## ✨ Features

* **Financial Product Management:**
    * Create and retrieve financial products with properties such as name, annual interest rate, minimum, and maximum allowed amounts.
* **Loan Payment Plan Calculation:**
    * Calculate and return a detailed loan amortization schedule (list of installments) based on a requested loan amount, a selected financial product, and a specific term in months.
    * Each installment includes the installment number, due date, principal amount, interest amount, and total payment.
* **Secure User Authentication:**
    * User login mechanism.
    * Password storage using **SHA256 hash and unique salt** for enhanced security.
    * API access protection using **JWT (JSON Web Token) authentication** for all protected endpoints.

## 🚀 Technologies Used

* **Backend Framework:** C# .NET Framework 4.5.2 (ASP.NET Web API)
* **Database:** SQL Server (tested with LocalDB)
* **ORM (Object-Relational Mapper):** Entity Framework 6.x
* **Authentication:** JWT (JSON Web Tokens) with OWIN/Katana middleware
* **Dependency Injection:** Unity Framework
* **Serialization:** Newtonsoft.Json
* **Asynchronous Operations:** `async/await`

## 🏗️ Architecture Overview

The API follows a layered architecture to ensure separation of concerns and maintainability:

* **Presentation Layer (`CrediAPI.Web.Controllers`):** Exposes RESTful endpoints, handles HTTP requests/responses, and performs model validation.
* **Domain/Business Logic Layer (`CrediAPI.Domain.Services` / `CrediAPI.Domain.Services.Impl`):** Contains the core business rules and logic, such as payment plan calculations and user authentication.
* **Infrastructure/Data Access Layer (`CrediAPI.Infrastructure.Context` / `CrediAPI.Infrastructure.Security`):** Manages database interactions (Entity Framework `DbContext`), password hashing utilities, and external configurations.

Dependency Injection (Unity) is used to manage the lifetime and injection of services and database contexts into controllers and other services.

## ⚙️ Setup and Running the API Locally

### Prerequisites

* [.NET Framework 4.5.2 Developer Pack](https://dotnet.microsoft.com/download/dotnet-framework/net452)
* [Visual Studio 2017 or later](https://visualstudio.microsoft.com/downloads/) (Community Edition is fine)
* [SQL Server (e.g., SQL Server 2019 Express or LocalDB)](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
* [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (Optional, but highly recommended for database management)

### Database Setup

1.  **Create Database:**
    Open SQL Server Management Studio (SSMS) and connect to your SQL Server instance (e.g., `(localdb)\MSSQLLocalDB` or `localhost`).
    Execute the provided SQL script to create the database and tables:

    ```sql
    -- Create the database
    IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'CrediDB')
    BEGIN
        CREATE DATABASE CrediDB;
    END
    GO

    USE CrediDB;
    GO

    -- Table: Usuarios
    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Usuarios' and xtype='U')
    BEGIN
        CREATE TABLE Usuarios (
            Id INT PRIMARY KEY IDENTITY(1,1),
            Username NVARCHAR(50) NOT NULL UNIQUE,
            PasswordHash NVARCHAR(256) NOT NULL,
            PasswordSalt NVARCHAR(128) NOT NULL
        );
    END
    GO

    -- Table: Productos
    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Productos' and xtype='U')
    BEGIN
        CREATE TABLE Productos (
            Id INT PRIMARY KEY IDENTITY(1,1),
            Nombre NVARCHAR(100) NOT NULL UNIQUE,
            TasaInteres DECIMAL(5,4) NOT NULL,
            MontoMinimo DECIMAL(18,2) NOT NULL,
            MontoMaximo DECIMAL(18,2) NOT NULL
        );
    END
    GO

    -- Table: Plazos
    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Plazos' and xtype='U')
    BEGIN
        CREATE TABLE Plazos (
            Id INT PRIMARY KEY IDENTITY(1,1),
            Meses INT NOT NULL UNIQUE
        );
    END
    GO

    -- Insert initial data for Plazos (if not exists)
    INSERT INTO Plazos (Meses)
    VALUES (6), (12), (18), (24), (36), (48);

    -- Insert initial data for Products (if not exists)
    INSERT INTO Productos (Nombre, TasaInteres, MontoMinimo, MontoMaximo)
    VALUES
        ('Crédito Personal', 0.0150, 1000.00, 50000.00),
        ('Préstamo Hipotecario', 0.0075, 50000.00, 500000.00),
        ('Crédito Automotriz', 0.0120, 5000.00, 100000.00);
    GO
    ```

2.  **Add Test User:**
    To enable login, you need a test user. **Do NOT insert passwords directly.** Use your `PasswordHasher` from the API project to generate a hash and salt for a test password (e.g., "admin" for "admin").

    * **Generate Hash/Salt:**
        Create a temporary Console Application in your solution, reference the API project, and use the `PasswordHasher` utility to get the values:
        ```csharp
        // In a temporary Console Application's Program.cs
        using System;
        using CrediAPI.Infrastructure.Security; // Adjust namespace if needed

        class Program
        {
            static void Main(string[] args)
            {
                Console.Write("Enter password to hash: ");
                string password = Console.ReadLine();
                var (hash, salt) = PasswordHasher.HashPassword(password);
                Console.WriteLine($"\nHash: {hash}\nSalt: {salt}");
                Console.ReadKey();
            }
        }
        ```
    * **Insert User:**
        Use the generated hash and salt to insert the user into the `Usuarios` table:
        ```sql
        USE CrediDB;
        -- Replace YOUR_GENERATED_HASH and YOUR_GENERATED_SALT with the values from the console app
        INSERT INTO Usuarios (Username, PasswordHash, PasswordSalt) VALUES ('admin', 'YOUR_GENERATED_HASH_HERE', 'YOUR_GENERATED_SALT_HERE');
        ```

3.  **Configure Connection String:**
    Update the `connectionStrings` section in your `Web.config` file to point to your SQL Server instance:
    ```xml
    <connectionStrings>
        <add name="CrediDB" connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CrediDB;Integrated Security=True;" providerName="System.Data.SqlClient" />
    </connectionStrings>
    ```
    (Adjust `Data Source` if your SQL Server instance name is different).

4.  **Configure JWT Secret/Issuer/Audience:**
    Update the `appSettings` section in your `Web.config` for JWT configuration:
    ```xml
    <appSettings>
        <add key="JwtSecret" value="YOUR_LONG_AND_COMPLEX_JWT_SECRET_KEY_AT_LEAST_32_BYTES" />
        <add key="JwtExpiresInMinutes" value="60" />
        <add key="JwtIssuer" value="[http://credi-api.com](http://credi-api.com)" />
        <add key="JwtAudience" value="CrediWinFormsApp" />
    </appSettings>
    ```
    **IMPORTANT:** Replace `YOUR_LONG_AND_COMPLEX_JWT_SECRET_KEY_AT_LEAST_32_BYTES` with a truly random and secure string.

### Running the API

1.  Open the solution (`.sln` file) in Visual Studio.
2.  Set the API project (`CrediAPI`) as the startup project (Right-click project -> `Set as Startup Project`).
3.  Press `F5` to run the API in debug mode. This will launch IIS Express (usually on `https://localhost:44372/` or similar).
    * **Troubleshooting `500.19` or OWIN startup errors:** Ensure `Startup.cs` is in the project root, the `[assembly: OwinStartup(...)]` attribute is present and correct, and `Global.asax.cs` has been simplified as per the development notes.

## 💻 API Endpoints

All API communication is JSON-based. Use tools like [Postman](https://www.postman.com/) or [Insomnia](https://insomnia.rest/) for testing.

**Base URL:** `https://localhost:XXXX/` (replace `XXXX` with your actual port, usually displayed when you run the API)

### 1. User Authentication

* **Endpoint:** `POST /api/auth/login`
* **Description:** Authenticates a user and returns a JWT token for subsequent protected requests.
* **Request Body (`application/json`):**
    ```json
    {
        "Username": "admin",
        "Password": "*****"
    }
    ```
* **Success Response (200 OK):**
    ```json
    {
        "statusCode": 200,
        "title": "Login Successful",
        "description": "User authenticated successfully.",
        "data": {
            "accessToken": "eyJhbGciOiJIUzI1Ni...",
            "tokenType": "Bearer",
            "ExpiresInMinutes": 3600
        }
    }
    ```
* **Error Responses:** `401 Unauthorized` (Invalid credentials), `500 Internal Server Error` (API issue).

### 2. Financial Products

* **Endpoint:** `GET /api/productos/find-all`
* **Description:** Retrieves a list of all available financial products.
* **Authorization:** `Bearer Token` (Required, obtained from `/api/auth/login`)
* **Success Response (200 OK):**
    ```json
    {
        "statusCode": 200,
        "title": "Success",
        "description": "Productos retrieved successfully.",
        "data": [
            { "id": 1, "nombre": "Crédito Personal", "tasaInteres": 0.0150, "montoMinimo": 1000.00, "montoMaximo": 50000.00 },
            { "id": 2, "nombre": "Préstamo Hipotecario", "tasaInteres": 0.0075, "montoMinimo": 50000.00, "montoMaximo": 500000.00 }
        ]
    }
    ```

### 3. Payment Terms

* **Endpoint:** `GET /api/plazos/find-all`
* **Description:** Retrieves a list of available payment terms in months.
* **Authorization:** `Bearer Token` (Required)
* **Success Response (200 OK):**
    ```json
    {
        "statusCode": 200,
        "title": "Success",
        "description": "Plazos retrieved successfully.",
        "data": [
            { "id": 1, "meses": 6 },
            { "id": 2, "meses": 12 },
            { "id": 3, "meses": 18 },
            { "id": 4, "meses": 24 }
        ]
    }
    ```

### 4. Loan Payment Plan Calculation

* **Endpoint:** `POST /api/planpago/calcular`
* **Description:** Calculates and returns a detailed amortization schedule for a loan.
* **Authorization:** `Bearer Token` (Required)
* **Request Body (`application/json`):**
    ```json
    {
        "MontoSolicitado": 10000.00,
        "ProductoId": 1,        
        "PlazoMeses": 24
    }
    ```
* **Success Response (200 OK):**
    ```json
    {
        "statusCode": 200,
        "title": "Calculated Payment Plan",
        "description": "The payment plan has been successfully calculated",
        "data": [
            {
                "numeroCuota": 1,
                "fechaVencimiento": "2025-08-01T00:00:00",
                "montoCapital": 408.01,
                "interes": 12.50,
                "cuotaTotal": 420.51,
                "saldoPendiente": 9591.99
            },
            {
                "numeroCuota": 2,
                "fechaVencimiento": "2025-09-01T00:00:00",
                "montoCapital": 413.11,
                "interes": 12.00,
                "cuotaTotal": 425.11,
                "saldoPendiente": 9178.88
            }
            // ... more installments ...
        ]
    }
    ```
* **Error Responses:** `400 Bad Request` (Validation errors, e.g., amount out of range, product not found), `401 Unauthorized` (Invalid/missing token), `500 Internal Server Error`.

---
