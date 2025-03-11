# OrderProcessingService

## Overview

The **OrderProcessingService** is a example service built to handle order management functionality (create, get) in a distributed system. This system is designed using **Domain-Driven Design** principles, ensuring separation of concerns, maintainability, and testability.

It exposes an HTTP API for managing orders and processes commands and queries related to order creation, retrieval, and validation.

## Architecture

The system follows a layered architecture with the following main components:

### 1. **API Layer (Web API)**

- The **API** layer is responsible for exposing HTTP endpoints that interact with the application’s services. It communicates with the application layer via MediatR commands and queries.
- The API layer uses **Serilog** for structured logging to monitor and trace requests.

### 2. **Application Layer**

- The **Application** layer contains business logic such as handling commands, queries, validation, and interaction with repositories.
- This layer uses **FluentValidation** for input validation and **MediatR** for handling commands and queries, maintaining a clean separation of concerns.

### 3. **Domain Layer**

- The **Domain** layer contains the core business objects, such as the `Order` aggregate and `OrderItem` value objects. It is responsible for maintaining the consistency and integrity of the domain.

### 4. **Infrastructure Layer**

- The **Infrastructure** layer provides implementations for the repository interfaces defined in the application layer and database interaction using **Entity Framework Core** with **SQL Server**.
  
### 5. **Persistence Layer**

- **Entity Framework Core** (EF Core) is used for the persistence of domain objects, providing a repository pattern for interacting with the underlying database.

### 6. **Test Layer**

- The **Test** layer contains unit and integration tests ensuring that the application's functionality is correct.
- **XUnit** and **Moq** are used for writing tests and mocking dependencies.

## Folder Structure

```
/src
    /OrderProcessing.Application     - Application logic, queries, commands, and validation
    /OrderProcessing.Domain          - Domain entities and value objects
    /OrderProcessing.Infrastructure  - Data access, repositories, and infrastructure
    /OrderProcessingService.API      - Web API controllers and HTTP-specific logic

/tests
    /OrderProcessing.Application.Tests    - Unit tests for application layer
    /OrderProcessing.Infrastructure.Tests - Unit and integration tests for infrastructure
    /OrderProcessing.Doamain.Tests        - Unit tests for Domain layer
```

## Key Technologies

- **ASP.NET Core 9** for building the API.
- **FluentValidation** for input validation.
- **MediatR** for handling commands and queries.
- **Serilog** for logging.
- **Entity Framework Core** for persistence.
- **SQL Server** as the database.
- **XUnit** for testing and **Moq** for mocking dependencies in tests.

### **Key Features**:

- **Domain-Driven Design (DDD)**: The code is organized around the core business domain, with entities, aggregates, and value objects representing the heart of the system. This design helps in clearly defining the business logic and enhances the maintainability of the application.

- **Clean Separation of Layers**: The application is divided into multiple layers, including:
        API Layer: Handles HTTP requests and responses.
        Application Layer: Contains use cases, commands, queries, and DTOs, processing the application logic.
        Domain Layer: Contains the core business logic (Entities, Aggregates, Value Objects).
        Infrastructure Layer: Handles external concerns, like persistence (e.g., Entity Framework).

- **Exception Handling**:
        The application uses exception middleware to handle and log exceptions globally, ensuring that errors are properly captured and returned to the user in a consistent format.
        The middleware allows the system to handle different types of exceptions, providing appropriate HTTP status codes (e.g., 400 for bad requests, 404 for not found) and helpful error messages to the client.

- **Validation via MediatR Preprocessor**:
        MediatR Preprocessor is used for validating requests before they are passed to the handler. This approach leverages the pre-processing pipeline, ensuring that validation logic is handled centrally and decoupled from the business logic.
        FluentValidation is used for creating validation rules, and the validation is performed asynchronously in the request pipeline, throwing appropriate exceptions when validation fails (e.g., ValidationException).

- **Logging via Serilog**:
        Serilog is used for logging throughout the application, ensuring that critical application events, errors, and system behavior are captured. This helps with debugging, auditing, and monitoring application performance.

- **Swagger for API Documentation**:
        The application uses Swagger to generate interactive API documentation. Swagger is integrated into the project for better visibility and easier testing of the API endpoints. The interactive UI allows developers and clients to quickly understand the API's structure, test the endpoints directly, and view response types and status codes.
        The Swagger UI is available at /swagger in the running application, allowing users to explore and interact with the API.

## Future Enhancements

- Add events e.g. update inventory after order creation
- User Docker
