// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NodaTime;

using Serilog;

using Testcontainers.Redis;

using VMelnalksnis.PaperlessDotNet.DependencyInjection;
using VMelnalksnis.Testcontainers.Paperless;

using Xunit.Abstractions;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public sealed class PaperlessFixture : IAsyncLifetime
{
	private readonly INetwork _network;
	private readonly RedisContainer _redis;
	private readonly PaperlessContainer _paperless;

	public PaperlessFixture()
	{
		const string redis = "redis";

		_network = new NetworkBuilder().Build();

		_redis = new RedisBuilder()
			.WithImage("docker.io/library/redis:7")
			.WithNetwork(_network)
			.WithNetworkAliases(redis)
			.Build();

		_paperless = new PaperlessBuilder()
			.WithNetwork(_network)
			.DependsOn(_redis)
			.WithRedis($"redis://{redis}:{RedisBuilder.RedisPort}")
			.Build();
	}

	internal PaperlessOptions Options { get; private set; } = null!;

	/// <inheritdoc />
	public async Task InitializeAsync()
	{
		await _paperless.StartAsync();

		var baseAddress = _paperless.GetBaseAddress();
		var token = await _paperless.GetAdminToken();
		Options = new() { BaseAddress = baseAddress, Token = token };
	}

	/// <inheritdoc />
	public async Task DisposeAsync()
	{
		await _paperless.DisposeAsync();
		await _redis.DisposeAsync();
		await _network.DisposeAsync();
	}

	internal IPaperlessClient GetPaperlessClient(ITestOutputHelper testOutputHelper)
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
			.AddPaperlessDotNet(configuration);

		serviceCollection.AddLogging(builder =>
		{
			var logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.TestOutput(
					testOutputHelper,
					outputTemplate:
					"{Timestamp:HH:mm:ss} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
				.Enrich.FromLogContext()
				.CreateLogger();

			builder.AddSerilog(logger);
		});

		return serviceCollection.BuildServiceProvider().GetRequiredService<IPaperlessClient>();
	}
}
