// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

using Ardalis.SmartEnum.SystemTextJson;

using NodaTime;
using NodaTime.Serialization.SystemTextJson;

using VMelnalksnis.PaperlessDotNet.Correspondents;
using VMelnalksnis.PaperlessDotNet.Documents;
using VMelnalksnis.PaperlessDotNet.Tasks;

namespace VMelnalksnis.PaperlessDotNet.Serialization;

/// <summary><see cref="JsonSerializerOptions"/> for <see cref="HttpClient"/> used by <see cref="IPaperlessClient"/>.</summary>
public sealed class PaperlessJsonSerializerOptions
{
	/// <summary>Initializes a new instance of the <see cref="PaperlessJsonSerializerOptions"/> class.</summary>
	/// <param name="dateTimeZoneProvider">Time zone provider for date and time serialization.</param>
	public PaperlessJsonSerializerOptions(IDateTimeZoneProvider dateTimeZoneProvider)
	{
		Options = new(JsonSerializerDefaults.Web)
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
			WriteIndented = true,
		};

		Options.ConfigureForNodaTime(dateTimeZoneProvider);
		Options.Converters.Add(new SmartEnumValueConverter<MatchingAlgorithm, int>());
		Options.Converters.Add(new SmartEnumNameConverter<PaperlessTaskStatus, int>());
		Options.Converters.Add(new SmartEnumNameConverter<CustomFieldType, int>());

		Options.TypeInfoResolverChain.Add(PaperlessJsonSerializerContext.Default);

		CustomFields = new();
	}

	/// <summary>Gets the options to use for serialization of paperless API data.</summary>
	public JsonSerializerOptions Options { get; }

	/// <summary>Gets the custom field definitions.</summary>
	public ConcurrentDictionary<int, CustomField> CustomFields { get; }
}
