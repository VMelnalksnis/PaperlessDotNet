// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[assembly: Parallelizable(ParallelScope.None)]

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration;

[SetUpFixture]
public static class PaperlessSetup
{
	internal static IEnumerable<PaperlessFixture> Fixtures { get; } =
	[
		new("1.9.2"),
		new("1.9.2", "1.9.2 with timezone", builder => builder.WithEnvironment("PAPERLESS_TIME_ZONE", "America/Chicago")),
		new("2.13.5"),
		new("2.13.5", "2.13.5 with timezone", builder => builder.WithEnvironment("PAPERLESS_TIME_ZONE", "America/Chicago")),
	];

	[OneTimeSetUp]
	public static Task OneTimeSetUpAsync() =>
		Task.WhenAll(Fixtures.Select(fixture => fixture.InitializeAsync()));

	[OneTimeTearDown]
	public static Task OneTimeTearDownAsync() =>
		Task.WhenAll(Fixtures.Select(fixture => fixture.DisposeAsync().AsTask()));
}
