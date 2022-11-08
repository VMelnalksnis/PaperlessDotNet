// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using VMelnalksnis.PaperlessDotNet.Correspondents;
using VMelnalksnis.PaperlessDotNet.Documents;
using VMelnalksnis.PaperlessDotNet.Serialization;

#if NET6_0_OR_GREATER
using System.Net.Mime;
#endif

namespace VMelnalksnis.PaperlessDotNet.DependencyInjection;

/// <summary>Methods for configuring <see cref="IPaperlessClient"/> within <see cref="IServiceCollection"/>.</summary>
public static class ServiceCollectionExtensions
{
	private static readonly ProductInfoHeaderValue _userAgent;

	static ServiceCollectionExtensions()
	{
		var assemblyName = typeof(IPaperlessClient).Assembly.GetName();
		var assemblyShortName = assemblyName.Name ?? assemblyName.FullName.Split(',').First();
		_userAgent = new(assemblyShortName, assemblyName.Version?.ToString());
	}

	/// <summary>Adds all required services for <see cref="IPaperlessClient"/>, excluding external dependencies.</summary>
	/// <param name="serviceCollection">The service collection in which to register the services.</param>
	/// <param name="configuration">The configuration to which to bind options models.</param>
	/// <returns>The <see cref="IHttpClientBuilder"/> for the <see cref="HttpClient"/> used by <see cref="IPaperlessClient"/>.</returns>
#if NET6_0_OR_GREATER
	[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026", Justification = $"{nameof(PaperlessOptions)} contains only system types.")]
#else
	[SuppressMessage("Trimming", "IL2026", Justification = $"{nameof(PaperlessOptions)} contains only system types.")]
#endif
	public static IHttpClientBuilder AddPaperlessDotNet(
		this IServiceCollection serviceCollection,
		IConfiguration configuration)
	{
		serviceCollection
			.AddOptions<PaperlessOptions>()
			.Bind(configuration.GetSection(PaperlessOptions.Name))
			.ValidateDataAnnotations();

		return serviceCollection
			.AddSingleton<PaperlessJsonSerializerOptions>()
			.AddTransient<IPaperlessClient, PaperlessClient>()
			.AddTransient<ICorrespondentClient, CorrespondentClient>(provider =>
			{
				var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(PaperlessOptions.Name);
				var options = provider.GetRequiredService<PaperlessJsonSerializerOptions>();
				return new(httpClient, options);
			})
			.AddTransient<IDocumentClient, DocumentClient>(provider =>
			{
				var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(PaperlessOptions.Name);
				var options = provider.GetRequiredService<PaperlessJsonSerializerOptions>();
				return new(httpClient, options);
			})
			.AddHttpClient(PaperlessOptions.Name, (provider, client) =>
			{
				var options = provider.GetRequiredService<IOptionsMonitor<PaperlessOptions>>().CurrentValue;

				client.BaseAddress = options.BaseAddress;
				client.DefaultRequestHeaders.UserAgent.Add(_userAgent);
#if NET6_0_OR_GREATER
				client.DefaultRequestHeaders.Add("Accept", $"{MediaTypeNames.Application.Json}; version=2");
#else
				client.DefaultRequestHeaders.Add("Accept", "application/json; version=2");
#endif
				client.DefaultRequestHeaders.Authorization = new("Token", options.Token);
			});
	}
}
