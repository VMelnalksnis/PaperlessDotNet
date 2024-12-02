// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NodaTime;

namespace VMelnalksnis.PaperlessDotNet.DependencyInjection.Tests;

public sealed class ServiceCollectionExtensionsTests
{
	private readonly IConfiguration _configuration = new ConfigurationBuilder()
		.AddInMemoryCollection(new List<KeyValuePair<string, string?>>
		{
			new($"{PaperlessOptions.Name}:{nameof(PaperlessOptions.BaseAddress)}", "https://localhost:5002/"),
			new($"{PaperlessOptions.Name}:{nameof(PaperlessOptions.Token)}", Guid.NewGuid().ToString("N")),
		})
		.Build();

	[Fact]
	public void AddPaperlessDotNet_ExplicitConfiguration_ShouldRegisterRequiredServices()
	{
		var serviceCollection = new ServiceCollection();

		serviceCollection
			.AddSingleton<IClock>(SystemClock.Instance)
			.AddSingleton(DateTimeZoneProviders.Tzdb)
			.AddPaperlessDotNet(_configuration);

		using var serviceProvider = serviceCollection.BuildServiceProvider();
		var paperlessClient = serviceProvider.GetRequiredService<IPaperlessClient>();

		paperlessClient.Should().NotBeNull();
	}

	[Fact]
	public void AddPaperlessDotNet_ShouldRegisterRequiredServices()
	{
		var serviceCollection = new ServiceCollection();

		serviceCollection
			.AddSingleton(_configuration)
			.AddSingleton<IClock>(SystemClock.Instance)
			.AddSingleton(DateTimeZoneProviders.Tzdb)
			.AddPaperlessDotNet();

		using var serviceProvider = serviceCollection.BuildServiceProvider();
		var paperlessClient = serviceProvider.GetRequiredService<IPaperlessClient>();

		paperlessClient.Should().NotBeNull();
	}
}
