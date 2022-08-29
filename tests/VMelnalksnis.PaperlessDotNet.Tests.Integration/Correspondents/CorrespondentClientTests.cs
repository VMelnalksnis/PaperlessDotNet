// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration.Correspondents;

public sealed class CorrespondentClientTests : IClassFixture<ServiceProviderFixture>
{
	private readonly IPaperlessClient _paperlessClient;

	public CorrespondentClientTests(ITestOutputHelper testOutputHelper, ServiceProviderFixture serviceProviderFixture)
	{
		_paperlessClient = serviceProviderFixture.GetPaperlessClient(testOutputHelper);
	}

	[Fact(Skip = "Requires a running Paperless instance")]
	public async Task GetAll_ShouldReturnExpected()
	{
		var correspondents = await _paperlessClient.Correspondents.GetAll().ToListAsync();

		correspondents.Should().HaveCount(37);

		var expectedCorrespondent = correspondents.First();

		var correspondent = await _paperlessClient.Correspondents.Get(expectedCorrespondent.Id);

		correspondent.Should().BeEquivalentTo(expectedCorrespondent);
	}

	[Fact(Skip = "Requires a running Paperless instance")]
	public async Task GetAll_PageSizeShouldNotChangeResult()
	{
		var correspondents = await _paperlessClient.Correspondents.GetAll().ToListAsync();
		var pageSizeCorrespondents = await _paperlessClient.Correspondents.GetAll(1).ToListAsync();

		correspondents.Should().BeEquivalentTo(pageSizeCorrespondents);
	}
}
