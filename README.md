[![Nuget](https://img.shields.io/nuget/v/VMelnalksnis.PaperlessDotNet?label=PaperlessDotNet)](https://www.nuget.org/packages/VMelnalksnis.PaperlessDotNet/)
[![Nuget](https://img.shields.io/nuget/v/VMelnalksnis.PaperlessDotNet.DependencyInjection?label=PaperlessDotNet.DependencyInjection)](https://www.nuget.org/packages/VMelnalksnis.PaperlessDotNet.DependencyInjection/)
[![Codecov](https://img.shields.io/codecov/c/github/VMelnalksnis/PaperlessDotNet)](https://app.codecov.io/gh/VMelnalksnis/PaperlessDotNet)
[![Run tests](https://github.com/VMelnalksnis/PaperlessDotNet/actions/workflows/test.yml/badge.svg?branch=master)](https://github.com/VMelnalksnis/PaperlessDotNet/actions/workflows/test.yml?query=branch%3Amaster)

# PaperlessDotNet
.NET client for the [Paperless-ngx](https://github.com/paperless-ngx/paperless-ngx) API.

# Usage

A separate [NuGet package](https://www.nuget.org/packages/VMelnalksnis.PaperlessDotNet.DependencyInjection/)
is provided for ASP.NET Core
([IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration)
and [IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection))
.
For use outside of ASP.NET Core, see the
[example in tests](tests/VMelnalksnis.PaperlessDotNet.Tests.Integration/MinimalExampleTests.cs).

1. Add configuration (see [options](source/VMelnalksnis.PaperlessDotNet.DependencyInjection/PaperlessOptions.cs))
   ```yaml
   "Paperless": {
       "BaseAddress": "",
       "Token": ""
   }
   ```

2. Register required services (see [tests](tests/VMelnalksnis.PaperlessDotNet.DependencyInjection.Tests/ServiceCollectionExtensionsTests.cs))
   ```csharp
   serviceCollection
       .AddSingleton(DateTimeZoneProviders.Tzdb)
       .AddPaperlessDotNet(Configuration);
   ```

3. Use `IPaperlessClient` to access all endpoints, or one of the specific clients defined in `IPaperlessClient` 
