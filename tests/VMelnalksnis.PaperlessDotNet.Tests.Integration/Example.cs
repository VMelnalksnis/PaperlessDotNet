using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using NodaTime;
using NodaTime.Testing;

using VMelnalksnis.PaperlessDotNet;
using VMelnalksnis.PaperlessDotNet.DependencyInjection;
using VMelnalksnis.PaperlessDotNet.Documents;
using VMelnalksnis.PaperlessDotNet.Serialization;

namespace PaperlessNgxClient;

internal sealed class PaperlessNgxClient : IAsyncDisposable
{
	private readonly IPaperlessClient _paperlessClient;
	private readonly ServiceProvider _serviceProvider;

	public PaperlessNgxClient(string url, string token)
	{
		_serviceProvider = GetServiceProvider(url, token);
		_serviceProvider.GetRequiredService<PaperlessJsonSerializerOptions>();
		var serviceScope = _serviceProvider.CreateAsyncScope();
		_paperlessClient = serviceScope.ServiceProvider.GetRequiredService<IPaperlessClient>();

		var httpClient = new HttpClient { BaseAddress = new(url) };
		httpClient.DefaultRequestHeaders.Add("Accept", $"{MediaTypeNames.Application.Json}; version=2");
		httpClient.DefaultRequestHeaders.Authorization = new("Token", token);
	}

	public async Task<Document<CustomFields>> UpdateCustomFieldPath(Document document, string path)
	{
		var docUpdate = new DocumentUpdate<CustomFields>
		{
			CustomFields = new()
			{
				Pfad = path,
			},
		};
		return await _paperlessClient.Documents.Update(document.Id, docUpdate);
	}

	public async Task<List<Document<CustomFields>>?> GetDocuments()
	{
		try
		{
			return await _paperlessClient.Documents.GetAll<CustomFields>().ToListAsync();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			return null;
		}
	}

	/// <inheritdoc />
	public ValueTask DisposeAsync() => _serviceProvider.DisposeAsync();

	private static ServiceProvider GetServiceProvider(string url, string token)
	{
		var configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(new List<KeyValuePair<string, string?>>
			{
				new("Paperless:BaseAddress", url),
				new("Paperless:Token", token),
			})
			.Build();

		var serviceCollection = new ServiceCollection();
		serviceCollection
			.AddSingleton(DateTimeZoneProviders.Tzdb)
			.AddSingleton<IClock>(new FakeClock(Instant.FromUtc(2024, 01, 17, 18, 8, 23), Duration.Zero))
			.AddPaperlessDotNet(
				configuration,
				options =>
				{
					options.Options.Converters.Add(new CustomFieldsConverter<CustomFields>(options));
					options.Options.TypeInfoResolverChain.Add(TestSerializerContext.Default);
				})
			.ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(20));

		serviceCollection.AddLogging(builder => builder
			.SetMinimumLevel(LogLevel.Trace)
			.AddSimpleConsole(options =>
			{
				options.ColorBehavior = LoggerColorBehavior.Enabled;
				options.IncludeScopes = true;
			}));

		return serviceCollection.BuildServiceProvider(true);
	}
}

internal sealed class CustomFields
{
	public string? Pfad { get; set; }
}

/// <inheritdoc cref="System.Text.Json.Serialization.JsonSerializerContext" />
[JsonSerializable(typeof(PaginatedList<Document<CustomFields>>))]
[JsonSerializable(typeof(DocumentUpdate<CustomFields>))]
internal sealed partial class TestSerializerContext : JsonSerializerContext;
