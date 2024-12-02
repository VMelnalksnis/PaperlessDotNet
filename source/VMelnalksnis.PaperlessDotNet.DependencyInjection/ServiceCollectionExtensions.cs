// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using NodaTime;

using VMelnalksnis.PaperlessDotNet.Correspondents;
using VMelnalksnis.PaperlessDotNet.Documents;
using VMelnalksnis.PaperlessDotNet.Serialization;
using VMelnalksnis.PaperlessDotNet.Tags;
using VMelnalksnis.PaperlessDotNet.Tasks;

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
	/// <param name="config">A delegate that is used to configure <see cref="PaperlessJsonSerializerOptions"/>.</param>
	/// <returns>The <see cref="IHttpClientBuilder"/> for the <see cref="HttpClient"/> used by <see cref="IPaperlessClient"/>.</returns>
#if NETSTANDARD2_0
	[SuppressMessage("Trimming", "IL2026", Justification = $"{nameof(PaperlessOptions)} contains only system types.")]
#else
	[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026", Justification = $"{nameof(PaperlessOptions)} contains only system types.")]
#endif
	public static IHttpClientBuilder AddPaperlessDotNet(
		this IServiceCollection serviceCollection,
		Action<PaperlessJsonSerializerOptions>? config = null)
	{
		serviceCollection
			.AddOptions<PaperlessOptions>()
			.BindConfiguration(PaperlessOptions.Name)
			.ValidateDataAnnotations();

		return serviceCollection.AddClient(config);
	}

	/// <summary>Adds all required services for <see cref="IPaperlessClient"/>, excluding external dependencies.</summary>
	/// <param name="serviceCollection">The service collection in which to register the services.</param>
	/// <param name="configuration">The configuration to which to bind options models.</param>
	/// <param name="config">A delegate that is used to configure <see cref="PaperlessJsonSerializerOptions"/>.</param>
	/// <returns>The <see cref="IHttpClientBuilder"/> for the <see cref="HttpClient"/> used by <see cref="IPaperlessClient"/>.</returns>
#if NETSTANDARD2_0
	[SuppressMessage("Trimming", "IL2026", Justification = $"{nameof(PaperlessOptions)} contains only system types.")]
#else
	[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026", Justification = $"{nameof(PaperlessOptions)} contains only system types.")]
#endif
	public static IHttpClientBuilder AddPaperlessDotNet(
		this IServiceCollection serviceCollection,
		IConfiguration configuration,
		Action<PaperlessJsonSerializerOptions>? config = null)
	{
		serviceCollection
			.AddOptions<PaperlessOptions>()
			.Bind(configuration.GetSection(PaperlessOptions.Name))
			.ValidateDataAnnotations();

		return serviceCollection.AddClient(config);
	}

	private static IHttpClientBuilder AddClient(
		this IServiceCollection serviceCollection,
		Action<PaperlessJsonSerializerOptions>? config)
	{
		return serviceCollection
			.AddSingleton<PaperlessJsonSerializerOptions>(provider =>
			{
				var options = new PaperlessJsonSerializerOptions(provider.GetRequiredService<IDateTimeZoneProvider>());
				config?.Invoke(options);
				return options;
			})
			.AddScoped<IPaperlessClient, PaperlessClient>()
			.AddScoped<ITaskClient, TaskClient>(provider =>
			{
				var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(PaperlessOptions.Name);
				var options = provider.GetRequiredService<PaperlessJsonSerializerOptions>();
				return new(httpClient, options);
			})
			.AddScoped<ICorrespondentClient, CorrespondentClient>(provider =>
			{
				var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(PaperlessOptions.Name);
				var options = provider.GetRequiredService<PaperlessJsonSerializerOptions>();
				return new(httpClient, options);
			})
			.AddScoped<IDocumentClient, DocumentClient>(provider =>
			{
				var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient(PaperlessOptions.Name);
				var options = provider.GetRequiredService<PaperlessJsonSerializerOptions>();
				var taskClient = provider.GetRequiredService<ITaskClient>();
				var paperlessOptions = provider.GetRequiredService<IOptionsMonitor<PaperlessOptions>>();
				return new(httpClient, options, taskClient, paperlessOptions.CurrentValue.TaskPollingDelay);
			})
			.AddScoped<ITagClient, TagClient>(provider =>
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
				client.DefaultRequestHeaders.Add("Accept", "application/json; version=2");
				client.DefaultRequestHeaders.Authorization = new("Token", options.Token);
			});
	}
}
