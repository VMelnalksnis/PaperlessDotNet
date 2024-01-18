// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

using NodaTime;

using VMelnalksnis.PaperlessDotNet.Correspondents;
using VMelnalksnis.PaperlessDotNet.Documents;
using VMelnalksnis.PaperlessDotNet.Serialization;
using VMelnalksnis.PaperlessDotNet.Tasks;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration;

[Collection("Paperless")]
public sealed class MinimalExampleTests
{
	private readonly IPaperlessClient _paperlessClient;

	public MinimalExampleTests(PaperlessFixture paperlessFixture)
	{
		var options = paperlessFixture.Options;

		var httpClient = new HttpClient { BaseAddress = options.BaseAddress };
		httpClient.DefaultRequestHeaders.Add("Accept", $"{MediaTypeNames.Application.Json}; version=2");
		httpClient.DefaultRequestHeaders.Authorization = new("Token", options.Token);

		var serializerOptions = new PaperlessJsonSerializerOptions(DateTimeZoneProviders.Tzdb);
		var taskClient = new TaskClient(httpClient, serializerOptions);
		var correspondentClient = new CorrespondentClient(httpClient, serializerOptions);
		var documentClient = new DocumentClient(httpClient, serializerOptions, taskClient);

		_paperlessClient = new PaperlessClient(correspondentClient, documentClient);
	}

	[Fact]
	public async Task ShouldNotThrow()
	{
		await FluentActions
			.Awaiting(() => _paperlessClient.Correspondents.GetAll().ToListAsync().AsTask())
			.Should()
			.NotThrowAsync();
	}
}
