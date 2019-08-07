# Rekreatistika
A .NET Core MVC project that's designed to log results and scorers of sport games. It uses .NET Core's built-in Dependency Injection, logging with Entity Framework Core and Identity using PostgreSQL
The project was developed with code-first approach, using standard migrations.

For easier setup SQLite is currently used but that can be changed by uncommenting 
```
 services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
```
in Startup.cs

This project was developed in order to test functionalities of .NET Core 2.1 and to check out some nice JS libraries like
list.js and flatpickr.js that showed themselves as a real joy to use.

Keep in mind that even though I've implemented localization it was not applied on 99% of the application so it's all Serbian in text.
