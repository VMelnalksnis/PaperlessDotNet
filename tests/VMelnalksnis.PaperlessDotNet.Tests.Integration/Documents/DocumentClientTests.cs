// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration.Documents;

public sealed class DocumentClientTests : IClassFixture<ServiceProviderFixture>
{
	private readonly IPaperlessClient _paperlessClient;

	public DocumentClientTests(ITestOutputHelper testOutputHelper, ServiceProviderFixture serviceProviderFixture)
	{
		_paperlessClient = serviceProviderFixture.GetPaperlessClient(testOutputHelper);
	}

	[Fact(Skip = "Requires a running Paperless instance")]
	public async Task GetAll_ShouldReturnExpected()
	{
		var documents = await _paperlessClient.Documents.GetAll().ToListAsync();

		documents.Should().HaveCount(197);

		var expectedDocument = documents.First();

		var document = await _paperlessClient.Documents.Get(expectedDocument.Id);

		document.Should().BeEquivalentTo(expectedDocument);
	}
}
