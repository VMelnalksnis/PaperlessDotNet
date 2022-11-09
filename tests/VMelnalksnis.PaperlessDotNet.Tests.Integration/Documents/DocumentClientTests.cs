// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration.Documents;

[Collection("Paperless")]
public sealed class DocumentClientTests
{
	private readonly IPaperlessClient _paperlessClient;

	public DocumentClientTests(ITestOutputHelper testOutputHelper, PaperlessFixture paperlessFixture)
	{
		_paperlessClient = paperlessFixture.GetPaperlessClient(testOutputHelper);
	}

	[Fact]
	public async Task GetAll_ShouldReturnExpected()
	{
		var documents = await _paperlessClient.Documents.GetAll().ToListAsync();

		documents.Should().BeEmpty();
	}

	[Fact]
	public async Task GetAll_PageSizeShouldNotChangeResult()
	{
		var documents = await _paperlessClient.Documents.GetAll().ToListAsync();
		var pageSizeDocuments = await _paperlessClient.Documents.GetAll(1).ToListAsync();

		documents.Should().BeEquivalentTo(pageSizeDocuments);
	}
}
