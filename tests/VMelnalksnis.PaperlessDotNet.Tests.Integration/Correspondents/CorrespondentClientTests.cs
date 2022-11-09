// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using VMelnalksnis.PaperlessDotNet.Correspondents;

using Xunit.Abstractions;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration.Correspondents;

[Collection("Paperless")]
public sealed class CorrespondentClientTests
{
	private readonly IPaperlessClient _paperlessClient;

	public CorrespondentClientTests(ITestOutputHelper testOutputHelper, PaperlessFixture paperlessFixture)
	{
		_paperlessClient = paperlessFixture.GetPaperlessClient(testOutputHelper);
	}

	[Fact]
	public async Task Create_ShouldCreateExpected()
	{
		var creation = new CorrespondentCreation("Acme Company")
		{
			Slug = "acme-company",
			Match = "acme company",
			IsInsensitive = true,
			MatchingAlgorithm = MatchingAlgorithm.ExactMatch,
		};

		var correspondent = await _paperlessClient.Correspondents.Create(creation);

		using (new AssertionScope())
		{
			correspondent.LastCorrespondence.Should().BeNull();
			correspondent.DocumentCount.Should().Be(0);
			correspondent.IsInsensitive.Should().Be(creation.IsInsensitive.Value);
			correspondent.MatchingAlgorithm.Should().Be(creation.MatchingAlgorithm);
			(await _paperlessClient.Correspondents.Get(correspondent.Id))
				.Should()
				.BeEquivalentTo(correspondent);

			var correspondents = await _paperlessClient.Correspondents.GetAll().ToListAsync();
			correspondents.Should().ContainSingle().Which.Should().BeEquivalentTo(correspondent);
		}

		await _paperlessClient.Correspondents.Delete(correspondent.Id);

		(await _paperlessClient.Correspondents.GetAll().ToListAsync()).Should().BeEmpty();
	}

	[Fact]
	public async Task GetAll_PageSizeShouldNotChangeResult()
	{
		var correspondents = new List<Correspondent>();
		for (var i = 0; i < 5; i++)
		{
			var creation = new CorrespondentCreation(Guid.NewGuid().ToString("N"));
			correspondents.Add(await _paperlessClient.Correspondents.Create(creation));
		}

		using (new AssertionScope())
		{
			(await _paperlessClient.Correspondents.GetAll().ToListAsync()).Should().BeEquivalentTo(correspondents);
			(await _paperlessClient.Correspondents.GetAll(1).ToListAsync()).Should().BeEquivalentTo(correspondents);
		}

		foreach (var correspondent in correspondents)
		{
			await _paperlessClient.Correspondents.Delete(correspondent.Id);
		}
	}
}
