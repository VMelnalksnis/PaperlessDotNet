// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using VMelnalksnis.PaperlessDotNet.Documents;

using static System.Text.Json.JsonValueKind;
using static System.Text.Json.Serialization.JsonIgnoreCondition;

namespace VMelnalksnis.PaperlessDotNet.Serialization;

/// <summary>Converts <typeparamref name="TFields"/> to and from list of <see cref="CustomField"/> ids and values.</summary>
/// <typeparam name="TFields">The type to which to map the custom fields.</typeparam>
public sealed class CustomFieldsConverter<TFields> : JsonConverter<TFields>
	where TFields : class
{
	private readonly PaperlessJsonSerializerOptions _options;

	/// <summary>Initializes a new instance of the <see cref="CustomFieldsConverter{TFields}"/> class.</summary>
	/// <param name="options">Options containing information about custom fields.</param>
	public CustomFieldsConverter(PaperlessJsonSerializerOptions options)
	{
		_options = options;
	}

	/// <inheritdoc />
	public override TFields? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		// First pass - list with field ids and values to an object with actual custom field names
		if (reader.TokenType is JsonTokenType.StartArray)
		{
			var fields = JsonSerializer.Deserialize(ref reader, options.GetTypeInfo<List<CustomFieldValue>>());
			if (fields is null || fields.Count is 0)
			{
				return null;
			}

			var namedFields = fields.ToDictionary(
				response => _options.CustomFields[response.Field].Name,
				response => response.Value);

			var text = JsonSerializer.Serialize(namedFields, _options.Options.GetTypeInfo<Dictionary<string, JsonNode>>());
			return JsonSerializer.Deserialize(text, _options.Options.GetTypeInfo<TFields>());
		}

		var cleanedOptions = GetCleanOptions(options);
		return JsonSerializer.Deserialize(ref reader, cleanedOptions.GetTypeInfo<TFields>());
	}

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, TFields value, JsonSerializerOptions options)
	{
		var cleanedOptions = GetCleanOptions(options);
		var document = JsonSerializer.SerializeToDocument(value, cleanedOptions.GetTypeInfo<TFields>());

		writer.WriteStartArray();

		foreach (var property in document.RootElement.EnumerateObject())
		{
			if (property.Value.ValueKind is Null && options.DefaultIgnoreCondition is not Never)
			{
				continue;
			}

			var customField = _options.CustomFields.Values.SingleOrDefault(field => field.Name == property.Name);
			if (customField is null)
			{
				throw new JsonException();
			}

			writer.WriteStartObject();
			writer.WritePropertyName("value");

			if (customField.DataType == CustomFieldType.String ||
				customField.DataType == CustomFieldType.Url ||
				customField.DataType == CustomFieldType.Date)
			{
				writer.WriteStringValue(property.Value.GetString());
			}
			else if (customField.DataType == CustomFieldType.Boolean)
			{
				writer.WriteBooleanValue(property.Value.GetBoolean());
			}
			else if (customField.DataType == CustomFieldType.Integer ||
					 customField.DataType == CustomFieldType.Select)
			{
				writer.WriteNumberValue(property.Value.GetInt32());
			}
			else if (customField.DataType == CustomFieldType.Float)
			{
				writer.WriteNumberValue(property.Value.GetSingle());
			}
			else if (customField.DataType == CustomFieldType.Monetary)
			{
				writer.WriteNumberValue(Math.Round(property.Value.GetSingle(), 2));
			}
			else if (customField.DataType == CustomFieldType.DocumentLink)
			{
				writer.WriteStartArray();

				foreach (var element in property.Value.EnumerateArray())
				{
					writer.WriteNumberValue(element.GetInt32());
				}

				writer.WriteEndArray();
			}

			writer.WriteNumber("field", customField.Id);

			writer.WriteEndObject();
		}

		writer.WriteEndArray();
	}

	/// <summary>Copy the options and remove this converter in order to fall back to the actual serialization logic.</summary>
	private static JsonSerializerOptions GetCleanOptions(JsonSerializerOptions options)
	{
		var clonedOptions = new JsonSerializerOptions(options);
		var converter = clonedOptions.Converters.OfType<CustomFieldsConverter<TFields>>().Single();
		clonedOptions.Converters.Remove(converter);

		return clonedOptions;
	}
}
