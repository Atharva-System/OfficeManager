# Office Manager


## Technologies

* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* [Angular 14](https://angular.io/)
* [MediatR](https://github.com/jbogard/MediatR)
* [AutoMapper](https://automapper.org/)
* [FluentValidation](https://fluentvalidation.net/)
* [NUnit](https://nunit.org/), [FluentAssertions](https://fluentassertions.com/), [Moq](https://github.com/moq) & [Respawn](https://github.com/jbogard/Respawn)


## Getting Started

The easiest way to get started is to install below packages:

1. Install the latest [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
2. Install the latest [Node.js LTS](https://nodejs.org/en/)


### Database Configuration

The template is configured to use an in-memory database by default. This ensures that all users will be able to run the solution without needing to set up additional infrastructure (e.g. SQL Server).

<br/>

<h1 align="center">CQRS with Mediator</h1>
<p align="center">Simple soap API made with .NET 6 using the concepts of <b>CQRS</b> and <b>Mediator pattern</b></p>

&nbsp;

## CQRS Explained
**CQRS** (Command Query Responsibility Segregation) is a pattern which separates the operations of read and update from a database. The implementation of this pattern can maximize the performance, scalability and security of the application.

## Mediator Pattern Explained
**Mediator** defines an object that encapsulates how a set of objects interact. This pattern is considered to be a behavioral pattern due to the way it can alter the program's running behaviour. [🌐 Wikipedia](https://en.wikipedia.org/wiki/Mediator_pattern)

## How it works?
The application uses [Entity Framework Core](https://docs.microsoft.com/pt-br/ef/core/) for running migrations and database connection, which we use in the **Commands** and **Queries** of the application, and we use [MediatR](https://github.com/jbogard/MediatR) for the Mediator Pattern implementation.
The controllers have a MediatR reference which we use the to execute the Queries and Commands, after that the Handlers execute your logic.

Thank You!

