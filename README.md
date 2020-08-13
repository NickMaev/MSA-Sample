Web application based on microservice architecture.

# Architecture
* ASP.NET Core 3.1, Web API
* [Docker](https://www.docker.com/), [Docker Compose](https://docs.docker.com/compose/) for Linux containers support
* [MassTransit](https://masstransit-project.com/) service bus
* [RabbitMQ](https://www.rabbitmq.com/) message brocker
* CQRS pattern based on [MediatR](https://github.com/jbogard/MediatR)
* [FluentValidation](https://github.com/FluentValidation/FluentValidation) - validation library
* [Swagger](https://swagger.io/) - API documetantion generator
* Logging via [Serilog](https://github.com/serilog/serilog)
* Object mapping via [Automapper](https://automapper.org/)
* Healthchecks using [AspNetCore.Diagnostics.HealthChecks](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks)

# Other tools
* [CoordinateSharp](https://github.com/Tronald/CoordinateSharp) for geographic distance calculation between two points.

# The main idea
The main idea is to demonstrate the skills of building scalable and resilient services.

# License
MIT