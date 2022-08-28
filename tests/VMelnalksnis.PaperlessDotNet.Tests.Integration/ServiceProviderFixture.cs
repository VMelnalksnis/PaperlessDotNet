// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NodaTime;
using NodaTime.Testing;

using Serilog;

using VMelnalksnis.PaperlessDotNet.DependencyInjection;

using Xunit.Abstractions;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public sealed class ServiceProviderFixture
{
	internal FakeClock Clock { get; } = new(SystemClock.Instance.GetCurrentInstant());

	internal IPaperlessClient GetPaperlessClient(ITestOutputHelper testOutputHelper)
	{
		var configuration = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.AddUserSecrets<ServiceProviderFixture>()
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
