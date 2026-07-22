# Lost & Found

A small Lost & Found backend system built with ASP.NET Core Web API, PostgreSQL, Docker Compose, and xUnit.

The application allows users to register found items and allows employees to mark items as claimed and returned.

## Tech Stack

* ASP.NET Core Web API
* DotNetEnv
* Entity Framework Core
* PostgreSQL 16
* Docker Compose
* Swagger / OpenAPI
* xUnit

## Features

* Create found items
* List found items
* Filter items by status and category
* Search items
* Claim found items
* Return items to owners
* Delete available items
* PostgreSQL persistence through Docker volume

## Running the Application with Docker

Make sure Docker Desktop is running.

Start the application:

```bash
docker compose up --build
```

This starts:

* ASP.NET Core API
* PostgreSQL database

The API will be available at:

```
http://localhost:8080/api/items
```

Swagger UI:

```
http://localhost:8080/swagger/index.html
```

## Environment Configuration

Database configuration is provided through environment variables.

Copy the example environment file:

```bash
cp .env.example .env
```

The PostgreSQL data is stored in a Docker volume so data survives container restarts.

## Running Tests

Run all tests:

```bash
dotnet test
```

The test suite verifies:

* New items are created with Available status
* Found timestamps are generated in UTC
* Claim rules
* Return rules
* Delete rules
* Repository filtering by status
* API responses for creation, return, delete, validation and missing resources

## Database Access

To access PostgreSQL inside Docker:

```bash
docker exec -it module3_backend_02_lostandfound-db-1 psql -U your_username -d your_database_name
```

Example query:

```sql
SELECT * FROM "Items";
```

Exit PostgreSQL:

```sql
\q
```

## Development Notes

The application uses:

* Repository abstraction for persistence
* In-memory repository for fast tests
* PostgreSQL repository for production-like execution
* Entity Framework Core `EnsureCreated()` for automatic database setup during development

## API Documentation

Swagger UI is available at:

http://localhost:8080/swagger

Use Swagger to explore and test the available endpoints.