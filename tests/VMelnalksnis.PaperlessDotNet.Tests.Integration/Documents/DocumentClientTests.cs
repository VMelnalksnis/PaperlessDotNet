// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

#if !NET6_0_OR_GREATER
using System;
#endif
using System.Linq;
using System.Threading.Tasks;

using NodaTime;

using VMelnalksnis.PaperlessDotNet.Documents;

using Xunit.Abstractions;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration.Documents;

[Collection("Paperless")]
public sealed class DocumentClientTests
{
	private readonly IPaperlessClient _paperlessClient;
	private readonly IClock _clock;

	public DocumentClientTests(ITestOutputHelper testOutputHelper, PaperlessFixture paperlessFixture)
	{
		_paperlessClient = paperlessFixture.GetPaperlessClient(testOutputHelper);
		_clock = paperlessFixture.Clock;
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

	[Fact]
	public async Task Create()
	{
		const string documentName = "Lorem Ipsum.txt";

		var correspondent = await _paperlessClient.Correspondents.Create(new("Foo"));
		await using var documentStream = typeof(DocumentClientTests).GetResource(documentName);
		var documentCreation = new DocumentCreation(documentStream, documentName)
		{
			Created = _clock.GetCurrentInstant(),
			Title = "Lorem Ipsum",
			CorrespondentId = correspondent.Id,
			ArchiveSerialNumber = 1,
		};

		var result = await _paperlessClient.Documents.Create(documentCreation);

		var id = result.Should().BeOfType<DocumentCreated>().Subject.Id;
		var document = (await _paperlessClient.Documents.Get(id))!;

		using var scope = new AssertionScope();

		var currentTime = SystemClock.Instance.GetCurrentInstant();
		var content = await typeof(DocumentClientTests).ReadResource(documentName);

		document.Should().NotBeNull();
		document.OriginalFileName.Should().Be(documentName);
		document.Created.ToInstant().Should().Be(documentCreation.Created.Value);
		document.Added.ToInstant().Should().BeInRange(currentTime - Duration.FromSeconds(10), currentTime);
		document.Modified.ToInstant().Should().BeInRange(currentTime - Duration.FromSeconds(10), currentTime);
		document.Title.Should().Be(documentCreation.Title);
		document.ArchiveSerialNumber.Should().Be(documentCreation.ArchiveSerialNumber);
		document.CorrespondentId.Should().Be(documentCreation.CorrespondentId);
#if NET6_0_OR_GREATER
		document.Content.ReplaceLineEndings().Should().BeEquivalentTo(content);
#else
		document.Content.Replace("\n", Environment.NewLine).Replace("\r\n", Environment.NewLine).Should().Be(content);
#endif

		await _paperlessClient.Correspondents.Delete(correspondent.Id);
	}
}
