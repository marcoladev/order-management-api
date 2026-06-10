# Order Management API

A backend application built with .NET 8 following Clean Architecture principles and event-driven architecture concepts.

## Features

* Create Orders
* Retrieve Orders
* Cancel Orders

## Solution Structure

```text
OrderManagement
│
├── src
│   ├── OrderManagement.Api
│   ├── OrderManagement.Application
│   ├── OrderManagement.Domain
│   ├── OrderManagement.Infrastructure
│   └── OrderManagement.Worker
│
├── tests
│   └── OrderManagement.UnitTests
│
└── OrderManagement.sln
```

## Architecture

The solution follows Clean Architecture principles:

* **Api**: HTTP endpoints and application configuration
* **Application**: Use cases, commands, handlers, and interfaces
* **Domain**: Entities, enums, and business rules
* **Infrastructure**: Persistence, repositories, and messaging
* **Worker**: Background processing and event consumers

## Event-Driven Workflow

### Order Creation

1. A client creates an order through the API.
2. The order is persisted in MySQL.
3. An `OrderCreatedEvent` is published to RabbitMQ.
4. The Worker consumes the event.
5. An audit log entry is created and stored in the database.

```text
API
 ↓
MySQL (Orders)
 ↓
RabbitMQ
 ↓
Worker
 ↓
MySQL (AuditLogs)
```

## Technologies

* .NET 8
* ASP.NET Core
* Entity Framework Core
* MySQL
* RabbitMQ
* FluentValidation
* Docker
* xUnit
* Swagger

## Running RabbitMQ

```bash
docker run -d \
  --name rabbitmq \
  -p 5672:5672 \
  -p 15672:15672 \
  rabbitmq:management
```

RabbitMQ Management UI:

http://localhost:15672

Default credentials:

* Username: guest
* Password: guest

## Future Improvements

* Order Updated Event
* Order Cancelled Event
* Notification Worker
* Email Notifications
* Retry Policies and Dead Letter Queues
* Integration Tests
* Unit Testing
* Validations
* Docker Compose Setup
* OpenTelemetry Monitoring
