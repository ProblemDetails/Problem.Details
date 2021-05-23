# ProblemDetails

## Problem

By default the MVC Core framework doesn't return consistent error responses accross different statuses.

## Solution

By calling few extension method you can fix that. All response codes `> 400` will have a nice JSON response, ex:

```json
{
  "type": "https://httpstatuses.com/404",
  "title": "Error 404",
  "status": 404
}
```

```json
{
  "errors": {
    "requiredField": ["The RequiredField field is required."]
  },
  "type": "https://httpstatuses.com/400",
  "title": "One or more validation errors occurred.",
  "status": 400
}
```

```json
{
  "type": "https://httpstatuses.com/500",
  "title": "Error 500",
  "status": 500
}
```

## Getting started

1. Call `services.AddProblemDetails()` in services configure method:

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddProblemDetails();
```

2. Call `app.UseProblemDetails()` in app Configure method:

```c#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseProblemDetails();
```

Check the [sample project](/ProblemDetails/Sample.WebApi).
