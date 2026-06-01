# FinanceTracker

FinanceTracker is a REST API built with ASP.NET 10 that allows users to manage personal finances, track income and expenses, monitor budgets, view statistics, and work with multiple currencies.

## Features

### Authentication

* User registration
* User login
* JWT Bearer Authentication
* Password hashing
* User data isolation

### Categories

* Create category
* Get all categories
* Update category
* Delete category
* Budget limit support
* Income and Expense category types

### Transactions

* Create transaction
* Update transaction
* Delete transaction
* Pagination
* Filtering
* Recurring transactions
* Generate next recurring transaction

### Statistics

* Monthly summary
* Expenses by category
* Monthly trend
* Budget status

### Exchange Rates

* GEL, USD, EUR support
* Automatic conversion to GEL for statistics
* Database cache for exchange rates
* 24-hour cache expiration
* Integration with National Bank of Georgia API

## Architecture

The project follows Clean Architecture principles and is divided into four projects:

### FinanceTracker.API

Contains controllers, middleware, authentication configuration, and application startup configuration.

### FinanceTracker.Application

Contains DTOs, service interfaces, repository interfaces, error response and exceptions, mapping profile.

### FinanceTracker.Domain

Contains entities, enums.

### FinanceTracker.Infrastructure

Contains Entity Framework Core, repositories, services, database configuration, and external API integrations.

## Technologies

* ASP.NET Core 10
* Entity Framework Core
* SQLite
* JWT Authentication
* AutoMapper
* Swagger
* Clean Architecture

## Error Handling

The application uses a global exception middleware and returns errors in a consistent format:

```
{
  "code": "ERROR_CODE",
  "message": "Error description",
  "details": null
}
```


## Running the Application

### Clone Repository

```
git clone https://github.com/giodiasa/FinanceTracker.git
cd FinanceTracker
```

### Apply Migrations

```
add-migration
update-database
```

### Run Application

```
dotnet run --project FinanceTracker.API
```

### Swagger

After starting the application:

```
http://localhost:5248/swagger/index.html
```

## API Endpoints

### Authentication

* POST /auth/register
* POST /auth/login

### Categories

* POST /categories
* GET /categories
* PUT /categories/{id}
* DELETE /categories/{id}

### Transactions

* POST /transactions
* GET /transactions
* PUT /transactions/{id}
* DELETE /transactions/{id}
* GET /transactions/recurring
* POST /transactions/{id}/generate-next

### Statistics

* GET /stats/summary?month=YYYY-MM
* GET /stats/by-category?month=YYYY-MM
* GET /stats/monthly-trend?year=YYYY
* GET /stats/budget-status

### Exchange Rates

* GET /exchange-rates

## Future Improvements

* CSV export
* PDF export
* Unit tests
* PostgreSQL support
* Docker support
