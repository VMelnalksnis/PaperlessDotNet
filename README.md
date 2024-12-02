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
       .AddSingleton<IClock>(SystemClock.Instance)
       .AddSingleton(DateTimeZoneProviders.Tzdb)
       .AddPaperlessDotNet();
   ```

3. Use `IPaperlessClient` to access all endpoints, or one of the specific clients defined in `IPaperlessClient`

## Custom fields

Paperless supports adding [custom fields](https://docs.paperless-ngx.com/usage/#custom-fields) to documents.
In order to use custom fields, first define a class with a property for each field:

```csharp
internal sealed class CustomFields
{
    public string? Field1 { get; set; }

    public Uri? Field2 { get; set; }

    public LocalDate? Field3 { get; set; }

    public bool? Field4 { get; set; }

    public int? Field5 { get; set; }

    public float? Field6 { get; set; }

    public float? Field7 { get; set; }

    public int[]? Field8 { get; set; }
}
```

Then create a `JsonSerializerContext` with all the API models that use custom fields:

```csharp
[JsonSerializable(typeof(PaginatedList<Document<CustomFields>>))]
[JsonSerializable(typeof(DocumentUpdate<CustomFields>))]
internal sealed partial class SerializerContext : JsonSerializerContext;
```

And configure JSON serialization for the client:

```csharp
serviceCollection.AddPaperlessDotNet(
    configuration,
    options =>
    {
        options.Options.Converters.Add(new CustomFieldsConverter<CustomFields>(options));
        options.Options.TypeInfoResolverChain.Add(SerializerContext.Default);
    });
```

For a working example
see [unit tests](tests/VMelnalksnis.PaperlessDotNet.Tests/Serialization/CustomFieldConverterTests.cs)
and [integration tests](tests/VMelnalksnis.PaperlessDotNet.Tests.Integration/Documents/DocumentClientTests.cs).
