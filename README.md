# ProblemDetails

[![Build](https://github.com/ProblemDetails/ProblemDetails/actions/workflows/build.yml/badge.svg)](https://github.com/ProblemDetails/ProblemDetails/actions/workflows/build.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ProblemDetails_ProblemDetails&metric=alert_status)](https://sonarcloud.io/dashboard?id=ProblemDetails_ProblemDetails)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ProblemDetails_ProblemDetails&metric=coverage)](https://sonarcloud.io/dashboard?id=ProblemDetails_ProblemDetails)
[![NuGet](https://img.shields.io/nuget/vpre/ProblemDetails.svg)](https://www.nuget.org/packages/ProblemDetails) 


## Problem

By default, the ASP.NET MVC Core framework doesn't return consistent error responses across different statuses. For example, the framework will return a status 500 with an empty body when encountering an internal server error.  Model validation errors will send back a JSON body, but the field names don't follow the casing rules. This package is meant to unify the responses.

## Solution

All response codes above `400` will have a nice JSON response, for example:

### Status `404`
```json
{
  "type": "https://httpstatuses.com/404",
  "title": "Error 404",
  "status": 404
}
```

### Status `400` (note that the field names are camel-cased)
```json
{
  "errors": {
    "requiredField": ["The requiredField field is required."]
  },
  "type": "https://httpstatuses.com/400",
  "title": "One or more validation errors occurred.",
  "status": 400
}
```

### Status `500`
```json
{
  "type": "https://httpstatuses.com/500",
  "title": "Error 500",
  "status": 500
}
```

You can also **override** the title values

## Getting started
1. Install the [package](https://www.nuget.org/packages/ProblemDetails)
```sh
Install-Package ProblemDetails
```
Or via the .NET Core command line interface:
```sh
dotnet add package ProblemDetails
```

2. Call `services.AddProblemDetails()` in services configure method:

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddProblemDetails()
      .MapStatusToTitle(500, "500, Oops!"); // optional overrides
```

3. Call `app.UseProblemDetails()` in app Configure method:

```c#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseProblemDetails();
```

Check the [sample project](https://github.com/ProblemDetails/ProblemDetails/tree/main/samples/Sample.WebApi) or browse [source](https://github.com/ProblemDetails/ProblemDetails) 

