// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NodaTime;
using NodaTime.Testing;

using Serilog;

using VMelnalksnis.PaperlessDotNet.DependencyInjection;
using VMelnalksnis.Testcontainers.Paperless;

using Xunit.Abstractions;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public sealed class PaperlessFixture : IAsyncLifetime
{
	private const string _redisImage = "docker.io/library/redis:7";

	private readonly List<ITestcontainersContainer> _containers = new();
	private PaperlessOptions _options = null!;

	internal FakeClock Clock { get; } = new(SystemClock.Instance.GetCurrentInstant());

	public async Task InitializeAsync()
	{
		var redisConfiguration = new RedisTestcontainerConfiguration(_redisImage);
		var redis = new TestcontainersBuilder<RedisTestcontainer>().WithDatabase(redisConfiguration).Build();
		_containers.Add(redis);
		await redis.StartAsync();

		var paperlessConfiguration = new PaperlessTestcontainerConfiguration();
		var paperless = new TestcontainersBuilder<PaperlessTestcontainer>().WithPaperless(paperlessConfiguration, redis).Build();
		_containers.Add(paperless);
		await paperless.StartAsync();

		var baseAddress = paperless.GetBaseAddress();
		var token = await paperless.GetAdminToken();
		_options = new() { BaseAddress = baseAddress, Token = token };
	}

	public Task DisposeAsync() => Task.WhenAll(_containers.Select(container => container.StopAsync()));

	internal IPaperlessClient GetPaperlessClient(ITestOutputHelper testOutputHelper)
	{
		var configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(new List<KeyValuePair<string, string?>>
			{
				new("Paperless:BaseAddress", _options.BaseAddress.ToString()),
				new("Paperless:Token", _options.Token),
			})
			.Build();

		var serviceCollection = new ServiceCollection();
		serviceCollection
			.AddSingleton<IClock>(Clock)
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
