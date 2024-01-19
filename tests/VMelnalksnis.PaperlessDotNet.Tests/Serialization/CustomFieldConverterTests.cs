// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

// Fails due to float inaccuracy in .NET 3.1/5
#if !(NETCOREAPP3_1 || NET5_0)
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

using NodaTime;

using VMelnalksnis.PaperlessDotNet.Documents;
using VMelnalksnis.PaperlessDotNet.Serialization;

namespace VMelnalksnis.PaperlessDotNet.Tests.Serialization;

public sealed class CustomFieldConverterTests
{
	[Fact]
	public void ShouldRoundtrip()
	{
#pragma warning disable SA1027
		const string json =
			"""
			[
			  {
			    "value": "foo",
			    "field": 1
			  },
			  {
			    "value": "https://example.com/",
			    "field": 2
			  },
			  {
			    "value": "2024-01-19",
			    "field": 3
			  },
			  {
			    "value": true,
			    "field": 4
			  },
			  {
			    "value": 12,
			    "field": 5
			  },
			  {
			    "value": 12.3456,
			    "field": 6
			  },
			  {
			    "value": 12.34,
			    "field": 7
			  },
			  {
			    "value": [
			      10
			    ],
			    "field": 8
			  }
			]
			""";
#pragma warning restore SA1027

		var options = new PaperlessJsonSerializerOptions(DateTimeZoneProviders.Tzdb)
		{
			CustomFields =
			{
				[1] = new() { Id = 1, Name = "field1", DataType = CustomFieldType.String },
				[2] = new() { Id = 2, Name = "field2", DataType = CustomFieldType.Url },
				[3] = new() { Id = 3, Name = "field3", DataType = CustomFieldType.Date },
				[4] = new() { Id = 4, Name = "field4", DataType = CustomFieldType.Boolean },
				[5] = new() { Id = 5, Name = "field5", DataType = CustomFieldType.Integer },
				[6] = new() { Id = 6, Name = "field6", DataType = CustomFieldType.Float },
				[7] = new() { Id = 7, Name = "field7", DataType = CustomFieldType.Monetary },
				[8] = new() { Id = 8, Name = "field8", DataType = CustomFieldType.DocumentLink },
			},
		};

		options.Options.Converters.Add(new CustomFieldsConverter<TestCustomFields>(options));
		options.Options.TypeInfoResolverChain.Add(TestSerializerContext.Default);

		var typeInfo = (JsonTypeInfo<TestCustomFields>)options.Options.GetTypeInfo(typeof(TestCustomFields));
		var customFields = JsonSerializer.Deserialize(json, typeInfo)!;

		customFields.Should().BeEquivalentTo(new TestCustomFields
		{
			Field1 = "foo",
			Field2 = new("https://example.com/"),
			Field3 = new LocalDate(2024, 01, 19),
			Field4 = true,
			Field5 = 12,
			Field6 = 12.3456f,
			Field7 = 12.34f,
			Field8 = [10],
		});

		JsonSerializer.Serialize(customFields, typeInfo).Should().Be(json);
	}
}
#endif
