// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using NodaTime;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration;

[TestFixtureSource(typeof(PaperlessFixtureSource))]
public abstract class PaperlessTests(PaperlessFixture paperlessFixture)
{
	protected PaperlessFixture Fixture { get; } = paperlessFixture;

	protected IPaperlessClient Client { get; } = paperlessFixture.GetPaperlessClient();

	protected IClock Clock { get; } = paperlessFixture.Clock;
}
