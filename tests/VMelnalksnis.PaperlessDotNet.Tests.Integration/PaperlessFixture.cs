// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using NodaTime;
using NodaTime.Testing;

using Testcontainers.Redis;

using VMelnalksnis.PaperlessDotNet.DependencyInjection;
using VMelnalksnis.PaperlessDotNet.Serialization;
using VMelnalksnis.PaperlessDotNet.Tests.Integration.Documents;
using VMelnalksnis.Testcontainers.Paperless;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration;

public sealed class PaperlessFixture : IAsyncDisposable
{
	private readonly INetwork _network;
	private readonly RedisContainer _redis;
	private readonly PaperlessContainer _paperless;

	public PaperlessFixture(string paperlessVersion)
		: this(paperlessVersion, paperlessVersion, builder => builder)
	{
	}

	public PaperlessFixture(string paperlessVersion, string name, Func<PaperlessBuilder, PaperlessBuilder> config)
	{
		const string redis = "redis";

		Name = name;

		_network = new NetworkBuilder().Build();

		_redis = new RedisBuilder()
			.WithImage("docker.io/library/redis:7")
			.WithNetwork(_network)
			.WithNetworkAliases(redis)
			.Build();

		var builder = new PaperlessBuilder()
			.WithImage($"{PaperlessBuilder.PaperlessImage}:{paperlessVersion}")
			.WithNetwork(_network)
			.DependsOn(_redis)
			.WithRedis($"redis://{redis}:{RedisBuilder.RedisPort}");

		builder = config(builder);

		_paperless = builder.Build();
	}

	internal string Name { get; }

	internal PaperlessOptions Options { get; private set; } = null!;

	internal IClock Clock { get; } = new FakeClock(Instant.FromUtc(2024, 01, 17, 18, 8, 23), Duration.Zero);

	/// <inheritdoc />
	public async ValueTask DisposeAsync()
	{
		await _paperless.DisposeAsync();
		await _redis.DisposeAsync();
		await _network.DisposeAsync();
	}

	internal async Task InitializeAsync()
	{
		await _paperless.StartAsync();

		var baseAddress = _paperless.GetBaseAddress();
		var token = await _paperless.GetAdminToken();
		Options = new() { BaseAddress = baseAddress, Token = token };
	}

	internal ServiceProvider GetServiceProvider()
	{
		var configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(new List<KeyValuePair<string, string?>>
			{
				new("Paperless:BaseAddress", Options.BaseAddress.ToString()),
				new("Paperless:Token", Options.Token),
			})
			.Build();

		var serviceCollection = new ServiceCollection();
		serviceCollection
			.AddSingleton(DateTimeZoneProviders.Tzdb)
			.AddSingleton(Clock)
			.AddPaperlessDotNet(
				configuration,
				options =>
				{
					options.Options.Converters.Add(new CustomFieldsConverter<CustomFields>(options));
					options.Options.TypeInfoResolverChain.Add(SerializerContext.Default);
				})
			.ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(5));

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
