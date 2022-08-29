[![Codecov](https://img.shields.io/codecov/c/github/VMelnalksnis/PaperlessDotNet)](https://app.codecov.io/gh/VMelnalksnis/PaperlessDotNet)
[![Run tests](https://github.com/VMelnalksnis/PaperlessDotNet/actions/workflows/test.yml/badge.svg?branch=master)](https://github.com/VMelnalksnis/PaperlessDotNet/actions/workflows/test.yml?query=branch%3Amaster)

# PaperlessDotNet
.NET client for the [Paperless-ngx](https://github.com/paperless-ngx/paperless-ngx) API.

# Usage
1. Add configuration (for optional values see [options](source/VMelnalksnis.PaperlessDotNet.DependencyInjection/PaperlessOptions.cs))
   ```yaml
   "Paperless": {
       "BaseAddress": "",
       "Token": ""
   }
   ```

2. Register required services (see [tests](tests/VMelnalksnis.PaperlessDotNet.DependencyInjection.Tests/ServiceCollectionExtensionsTests.cs))
   ```csharp
   serviceCollection
       .AddSingleton<IClock>(SystemClock.Instance)
       .AddSingleton(DateTimeZoneProviders.Tzdb)
       .AddPaperlessDotNet(Configuration);
   ```

3. Use `IPaperlessClient` to access all endpoints, or one of the specific clients defined in `IPaperlessClient` 
