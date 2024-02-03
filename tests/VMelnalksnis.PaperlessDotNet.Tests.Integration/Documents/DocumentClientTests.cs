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

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration.Documents;

public sealed class DocumentClientTests(PaperlessFixture paperlessFixture) : PaperlessTests(paperlessFixture)
{
	[Test]
	[Order(1)]
	public async Task GetAll_ShouldReturnExpected()
	{
		var documents = await Client.Documents.GetAll().ToListAsync();

		documents.Should().BeEmpty();
	}

	[Test]
	public async Task GetAll_PageSizeShouldNotChangeResult()
	{
		var documents = await Client.Documents.GetAll().ToListAsync();
		var pageSizeDocuments = await Client.Documents.GetAll(1).ToListAsync();

		documents.Should().BeEquivalentTo(pageSizeDocuments);
	}

	[Test]
	public async Task Create()
	{
		const string documentName = "Lorem Ipsum.txt";

		var correspondent = await Client.Correspondents.Create(new("Foo"));
		await using var documentStream = typeof(DocumentClientTests).GetResource(documentName);
		var documentCreation = new DocumentCreation(documentStream, documentName)
		{
			Created = Clock.GetCurrentInstant(),
			Title = "Lorem Ipsum",
			CorrespondentId = correspondent.Id,
			ArchiveSerialNumber = 1,
		};

		var result = await Client.Documents.Create(documentCreation);

		if (Fixture.Name.StartsWith("1.9.2"))
		{
			result.Should().BeOfType<ImportStarted>();
			return;
		}

		var id = result.Should().BeOfType<DocumentCreated>().Subject.Id;
		var document = (await Client.Documents.Get(id))!;

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

		await Client.Correspondents.Delete(correspondent.Id);
	}
}
