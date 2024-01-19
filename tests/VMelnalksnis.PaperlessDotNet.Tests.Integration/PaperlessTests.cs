// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using NodaTime;

using VMelnalksnis.PaperlessDotNet.Serialization;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration;

[TestFixtureSource(typeof(PaperlessFixtureSource))]
public abstract class PaperlessTests
{
	private readonly ServiceProvider _serviceProvider;
	private AsyncServiceScope _serviceScope;

	protected PaperlessTests(PaperlessFixture paperlessFixture)
	{
		_serviceProvider = paperlessFixture.GetServiceProvider();
		Clock = _serviceProvider.GetRequiredService<IClock>();
		SerializerOptions = _serviceProvider.GetRequiredService<PaperlessJsonSerializerOptions>();
		PaperlessVersion = Version.Parse(paperlessFixture.Name.Split(' ').First());
	}

	protected Version PaperlessVersion { get; }

	protected IClock Clock { get; }

	protected PaperlessJsonSerializerOptions SerializerOptions { get; }

	protected IPaperlessClient Client { get; private set; } = null!;

	[SetUp]
	public void SetUp()
	{
		_serviceScope = _serviceProvider.CreateAsyncScope();
		Client = _serviceScope.ServiceProvider.GetRequiredService<IPaperlessClient>();
	}

	[TearDown]
	public async Task TearDown()
	{
		await _serviceScope.DisposeAsync();
	}

	[OneTimeTearDown]
	public async Task OneTimeTearDown()
	{
		await _serviceProvider.DisposeAsync();
	}
}
