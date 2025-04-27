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

### Select custom fields
Version `2.11.0` introduced `select` custom fields, which require additional setup in order to serialize/deserialize properly.
First, you'll need to define `SmartEnum` class for each `select` custom field:
```csharp
public sealed class SelectOptions : SmartEnum<SelectOptions>
{
    public static readonly SelectOptions Option1 = new("First option", 0);
    public static readonly SelectOptions Option2 = new("Second option", 1);

    private SelectOptions(string name, int value)
        : base(name, value)
    {
    }
}
```
**NOTE:** the values **MUST** be sequential and start at 0 in order to match how they are stored in paperless.

Then you can add the property to your `CustomFields` class
```csharp
    public SelectOptions? Field9 { get; set; }
```
and add the `SmartEnumValueConverter<TEnum, TValue>` in one of the possible ways:
```csharp
    [JsonConverter(typeof(SmartEnumValueConverter<SelectOptions, int>))]
    public SelectOptions? Field9 { get; set; }
```
or
```csharp
serviceCollection.AddPaperlessDotNet(
    configuration,
    options =>
    {
        options.Options.Converters.Add(new SmartEnumValueConverter<SelectOptions, int>())
        options.Options.Converters.Add(new CustomFieldsConverter<CustomFields>(options));
        options.Options.TypeInfoResolverChain.Add(SerializerContext.Default);
    });
```

In order to create a `select` custom field, you also need to use 
either `SelectCustomFieldCreation<TEnum, TValue>` or `SelectCustomFieldCreation<TEnum>` and add it to the `JsonSerializerContext`. 

