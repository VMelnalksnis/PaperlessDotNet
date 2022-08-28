// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Text.Json;

using NodaTime;
using NodaTime.Serialization.SystemTextJson;

namespace VMelnalksnis.PaperlessDotNet.Serialization;

/// <summary><see cref="JsonSerializerOptions"/> for <see cref="HttpClient"/> used by <see cref="IPaperlessClient"/>.</summary>
public sealed class PaperlessJsonSerializerOptions
{
	/// <summary>Initializes a new instance of the <see cref="PaperlessJsonSerializerOptions"/> class.</summary>
	/// <param name="dateTimeZoneProvider">Time zone provider for date and time serialization.</param>
	public PaperlessJsonSerializerOptions(IDateTimeZoneProvider dateTimeZoneProvider)
	{
		var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
			.ConfigureForNodaTime(dateTimeZoneProvider);

		Context = new(options);
	}

	internal PaperlessJsonSerializerContext Context { get; }
}
