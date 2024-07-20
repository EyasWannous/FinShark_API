# FinShark API

FinShark API is a web application designed to manage stock portfolios and user comments. It includes user authentication, stock management, portfolio tracking, and comment functionalities. The application leverages .NET technologies and provides a set of RESTful APIs.

## Features

- **User Authentication**: Register and login users.
- **Stock Management**: Create, read, update, and delete stocks.
- **Portfolio Management**: Manage user portfolios.
- **Comment System**: Add, update, and delete comments on stocks.
- **Admin Management**: Assign roles to users.
- **Rate Limiting**: Middleware to limit the rate of requests.
- **Profiling Middleware**: Middleware to profile application performance.

## Project Structure

- **Controllers**: Handles HTTP requests and responses.
- **DTOs (Data Transfer Objects)**: Defines the data structures for communication between client and server.
- **Data**: Contains database context and configurations.
- **Helpers**: Utility classes for the application.
- **Interfaces**: Defines the contract for the repositories and services.
- **Mappers**: Contains mapping logic between models and DTOs.
- **Middleware**: Custom middleware components.
- **Models**: Represents the data structure of the application.
- **Repositories**: Contains the data access logic.
- **Services**: Provides additional business logic.

## API Endpoints

### AccountController

- `POST /api/account/register`: Register a new user
- `POST /api/account/login`: Log in a user

### CommentController

- `GET /api/comments`: Get all comments
- `GET /api/comments/{commentId}`: Get a comment by ID
- `POST /api/comments`: Create a new comment
- `PUT /api/comments/{commentId}`: Update a comment
- `DELETE /api/comments/{commentId}`: Delete a comment

### PortfolioController

- `GET /api/portfolios`: Get all portfolios
- `GET /api/portfolios/{portfolioId}`: Get a portfolio by ID
- `POST /api/portfolios`: Create a new portfolio
- `PUT /api/portfolios/{portfolioId}`: Update a portfolio
- `DELETE /api/portfolios/{portfolioId}`: Delete a portfolio

### SalafiUserController

- `GET /api/salafiusers`: Get all Salafi users
- `GET /api/salafiusers/{userId}`: Get a Salafi user by ID
- `POST /api/salafiusers`: Create a new Salafi user
- `PUT /api/salafiusers/{userId}`: Update a Salafi user
- `DELETE /api/salafiusers/{userId}`: Delete a Salafi user

### StockController

- `GET /api/stocks`: Get all stocks
- `GET /api/stocks/{stockId}`: Get a stock by ID
- `POST /api/stocks`: Create a new stock
- `PUT /api/stocks/{stockId}`: Update a stock
- `DELETE /api/stocks/{stockId}`: Delete a stock

## Special Features

- **JWT Token-Based Authentication**: Ensures secure user authentication and authorization.
- **Role Management**: Admin can manage user roles, enhancing the application's security and flexibility.
- **Rate Limiting Middleware**: Limits the number of requests to prevent abuse and ensure fair usage.
- **Profiling Middleware**: Custom middleware to profile and log performance metrics, aiding in the optimization and monitoring of the application.

## Getting Started

### Prerequisites

- .NET SDK
- SQL Server

### Setup

1. Clone the repository
   ```bash
   git clone https://github.com/EyasWannous/FinShark_API.git
