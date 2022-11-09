// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;

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
	public async Task GetAll_ShouldReturnExpected()
	{
		var correspondents = await _paperlessClient.Correspondents.GetAll().ToListAsync();

		correspondents.Should().BeEmpty();
	}

	[Fact]
	public async Task GetAll_PageSizeShouldNotChangeResult()
	{
		var correspondents = await _paperlessClient.Correspondents.GetAll().ToListAsync();
		var pageSizeCorrespondents = await _paperlessClient.Correspondents.GetAll(1).ToListAsync();

		correspondents.Should().BeEquivalentTo(pageSizeCorrespondents);
	}
}
