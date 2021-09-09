# ProblemDetails

[![Build](https://github.com/ProblemDetails/ProblemDetails/actions/workflows/build.yml/badge.svg)](https://github.com/ProblemDetails/ProblemDetails/actions/workflows/build.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ProblemDetails_ProblemDetails&metric=alert_status)](https://sonarcloud.io/dashboard?id=ProblemDetails_ProblemDetails)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ProblemDetails_ProblemDetails&metric=coverage)](https://sonarcloud.io/dashboard?id=ProblemDetails_ProblemDetails)
[![NuGet](https://img.shields.io/nuget/vpre/Problem.Details.svg)](https://www.nuget.org/packages/Problem.Details) 


## Problem

By default, the ASP.NET MVC Core framework doesn't return consistent error responses across different statuses. For example, the framework will return a status 500 with an empty body when encountering an internal server error.  Model validation errors will send back a JSON body, but the field names don't follow the casing rules. This package is meant to unify the responses.

## Solution

All response codes above `400` will have a nice JSON response, for example:

### Status `404` Not Found
```json
{
  "type": "https://httpstatuses.com/404",
  "title": "Error: Not Found",
  "status": 404
}
```

### Status `400` Bad Request (note that the field names are camel-cased)
```json
{
  "errors": {
    "requiredField": ["The requiredField field is required."]
  },
  "type": "https://httpstatuses.com/400",
  "title": "Error: Bad Request",
  "status": 400
}
```

### Status `500` Internal Server Error
```json
{
  "type": "https://httpstatuses.com/500",
  "title": "Error: Internal Server Error",
  "status": 500,
  "exception": "System.Exception: Testing 500\n   at Sample.WebApi.Controllers.WeatherForecastController.Get(Int32 id) in /Users/hanneskarask/dev/Problem.Details/samples/Sample.WebApi/Controllers/WeatherForecastController.cs:line 42\n   at lambda_method3(Closure , Object , Object[] )..."
}
```
*(exception is only visible when explicitly turned on, i.e. in dev environments)*

You can also **override** the title values and map **custom exceptions**.

## Getting started
1. Install the [package](https://www.nuget.org/packages/Problem.Details)
```sh
Install-Package Problem.Details
```
Or via the .NET Core command line interface:
```sh
dotnet add package Problem.Details
```

2. Call `services.AddProblemDetails()` in services configure method:

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddProblemDetails();
```

3. Call `app.UseProblemDetails()` in app Configure method:

```c#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseProblemDetails(configure => configure
        // Optional: Override title for Bad Request
        .MapStatusToTitle(HttpStatusCode.BadRequest, "One or more validation errors occurred")
        // Optional: Map your custom exception
        .MapException<NotFoundException>(HttpStatusCode.NotFound)
        // Optional: Show detailed error
        .ShowErrorDetails(env.IsDevelopment())
    );
```

Check the [sample project](https://github.com/Problem.Details/Problem.Details/tree/main/samples/Sample.WebApi) or browse [source](https://github.com/Problem.Details/Problem.Details) 
