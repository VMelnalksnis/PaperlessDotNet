// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using NodaTime;

using VMelnalksnis.PaperlessDotNet.Documents;
using VMelnalksnis.PaperlessDotNet.Tags;

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
		var tags = new List<Tag>();
		foreach (var tag in new List<TagCreation> { new("Receipt"), new("Bill") })
		{
			tags.Add(await Client.Tags.Create(tag));
		}

		await using var documentStream = typeof(DocumentClientTests).GetResource(documentName);
		var documentCreation = new DocumentCreation(documentStream, documentName)
		{
			Created = Clock.GetCurrentInstant(),
			Title = "Lorem Ipsum",
			CorrespondentId = correspondent.Id,
			ArchiveSerialNumber = 1,
			TagIds = tags.Select(tag => tag.Id).ToArray(),
		};

		var result = await Client.Documents.Create(documentCreation);

		if (PaperlessVersion < new Version(2, 0))
		{
			result.Should().BeOfType<ImportStarted>();
			return;
		}

		var id = result.Should().BeOfType<DocumentCreated>().Subject.Id;
		var document = (await Client.Documents.Get(id))!;
		var documents = await Client.Documents.GetAll().ToListAsync();
		var metadata = await Client.Documents.GetMetadata(id);

		using var scope = new AssertionScope();

		var currentTime = SystemClock.Instance.GetCurrentInstant();
		var content = await typeof(DocumentClientTests).ReadResource(documentName);

		documents.Should().ContainSingle(d => d.Id == id).Which.Should().BeEquivalentTo(document);
		document.Should().NotBeNull();
		document.OriginalFileName.Should().Be(documentName);
		document.Created.ToInstant().Should().Be(documentCreation.Created.Value);
		document.Added.ToInstant().Should().BeInRange(currentTime - Duration.FromSeconds(10), currentTime);
		document.Modified.ToInstant().Should().BeInRange(currentTime - Duration.FromSeconds(10), currentTime);
		document.Title.Should().Be(documentCreation.Title);
		document.ArchiveSerialNumber.Should().Be(documentCreation.ArchiveSerialNumber);
		document.CorrespondentId.Should().Be(documentCreation.CorrespondentId);
		document.TagIds.Should().BeEquivalentTo(tags.Select(tag => tag.Id));
#if NET6_0_OR_GREATER
		document.Content.ReplaceLineEndings().Should().BeEquivalentTo(content);
#else
		document.Content.Replace("\n", Environment.NewLine).Replace("\r\n", Environment.NewLine).Should().Be(content);
#endif
		metadata.Should().BeEquivalentTo(new DocumentMetadata(
			$"{id:0000000}.txt",
			Environment.NewLine is "\r\n" ? "999853181bf31bb3f54be7c0bc20f6af" : "be37b4f97ce9f67970d878978e5db5eb",
			Environment.NewLine is "\r\n" ? 2868 : 2859,
			MediaTypeNames.Text.Plain,
			documentName,
			[],
			"ca",
			false));

		var update = new DocumentUpdate { Title = $"{document.Title}1" };
		var updatedDocument = await Client.Documents.Update(id, update);

		updatedDocument.Title.Should().Be(update.Title);

		await Client.Correspondents.Delete(correspondent.Id);
		foreach (var tag in tags)
		{
			await Client.Tags.Delete(tag.Id);
		}

		await Client.Documents.Delete(id);

		await FluentActions
			.Awaiting(() => Client.Documents.Get(id))
			.Should()
			.ThrowExactlyAsync<HttpRequestException>()
			.WithMessage("Response status code does not indicate success: 404 (Not Found).");
	}

	[Test]
	public async Task Download()
	{
		if (PaperlessVersion < new Version(2, 0))
		{
			Assert.Ignore($"Paperless v{PaperlessVersion} does not directly allow downloading documents.");
		}

		const string documentName = "Lorem Ipsum 3.txt";

		await using var documentStream = typeof(DocumentClientTests).GetResource(documentName);
		var documentCreation = new DocumentCreation(documentStream, documentName);

		var createResult = await Client.Documents.Create(documentCreation);
		var id = createResult.Should().BeOfType<DocumentCreated>().Subject.Id;

		var expectedDocumentContent = await typeof(DocumentClientTests).ReadResource(documentName);
		var expectedPartOfFileName = "Lorem Ipsum 3";

		// Download
		var documentDownload = await Client.Documents.Download(id);
		documentDownload.MediaTypeHeaderValue.MediaType.Should().Be("text/plain");
		documentDownload.ContentDisposition!.FileName.Should().Contain(expectedPartOfFileName);

		var downloadContent = await ReadStreamContentAsString(documentDownload.Content);
		downloadContent.Should().BeEquivalentTo(expectedDocumentContent);

		// Download Original
		var documentOriginalDownload = await Client.Documents.DownloadOriginal(id);
		documentOriginalDownload.MediaTypeHeaderValue.MediaType.Should().Be("text/plain");
		documentOriginalDownload.ContentDisposition!.FileName.Should().Contain(expectedPartOfFileName);

		var downloadOriginalContent = await ReadStreamContentAsString(documentOriginalDownload.Content);
		downloadOriginalContent.Should().BeEquivalentTo(expectedDocumentContent);

		// Download Preview
		var downloadPreview = await Client.Documents.DownloadPreview(id);
		downloadPreview.MediaTypeHeaderValue.MediaType.Should().Be("text/plain");
		downloadPreview.ContentDisposition!.FileName.Should().Contain(expectedPartOfFileName);

		var downloadPreviewContent = await ReadStreamContentAsString(downloadPreview.Content);
		downloadPreviewContent.Should().BeEquivalentTo(expectedDocumentContent);

		// Download Preview Original
		var downloadPreviewOriginal = await Client.Documents.DownloadPreview(id);
		downloadPreviewOriginal.MediaTypeHeaderValue.MediaType.Should().Be("text/plain");
		downloadPreviewOriginal.ContentDisposition!.FileName.Should().Contain(expectedPartOfFileName);

		var downloadPreviewOrignalContent = await ReadStreamContentAsString(downloadPreviewOriginal.Content);
		downloadPreviewOrignalContent.Should().BeEquivalentTo(expectedDocumentContent);

		// Download thumbnail
		var downloadThumbnail = await Client.Documents.DownloadThumbnail(id);
		downloadThumbnail.MediaTypeHeaderValue.MediaType.Should().Be("image/webp");
	}

	[Test]
	public async Task CustomFields()
	{
		if (PaperlessVersion < new Version(2, 0))
		{
			Assert.Ignore($"Paperless v{PaperlessVersion} does not support custom fields");
		}

		const string documentName = "Lorem Ipsum 2.txt";

		await Client.Documents.CreateCustomField(new("field1", CustomFieldType.String));
		await Client.Documents.CreateCustomField(new("field2", CustomFieldType.Url));
		await Client.Documents.CreateCustomField(new("field3", CustomFieldType.Date));
		await Client.Documents.CreateCustomField(new("field4", CustomFieldType.Boolean));
		await Client.Documents.CreateCustomField(new("field5", CustomFieldType.Integer));
		await Client.Documents.CreateCustomField(new("field6", CustomFieldType.Float));
		await Client.Documents.CreateCustomField(new("field7", CustomFieldType.Monetary));
		await Client.Documents.CreateCustomField(new("field8", CustomFieldType.DocumentLink));
		await Client.Documents.CreateCustomField(new("pfad", CustomFieldType.String));

		var customFields = await Client.Documents.GetCustomFields().ToListAsync();
		customFields.Should().HaveCount(9);

		var paginatedCustomFields = await Client.Documents.GetCustomFields(1).ToListAsync();
		paginatedCustomFields.Should().BeEquivalentTo(customFields);

		await using var documentStream = typeof(DocumentClientTests).GetResource(documentName);
		var documentCreation = new DocumentCreation(documentStream, documentName);

		var result = await Client.Documents.Create(documentCreation);

		SerializerOptions.CustomFields.Clear();

		var id = result.Should().BeOfType<DocumentCreated>().Subject.Id;
		var document = (await Client.Documents.Get<CustomFields>(id))!;

		var options = Services.GetRequiredService<IOptions<PaperlessOptions>>().Value;
		var client = new PaperlessNgxClient.PaperlessNgxClient(options.BaseAddress.AbsoluteUri, options.Token);
		var documents2 = await client.GetDocuments();
		foreach (var document2 in documents2 ?? [])
		{
			await client.UpdateCustomFieldPath(document2, Guid.NewGuid().ToString());
		}

		document.CustomFields.Should().BeNull("cannot create document with custom fields");

		var update = new DocumentUpdate<CustomFields>
		{
			CustomFields = new()
			{
				Field1 = "foo",
				Field2 = new("https://example.com/"),
				Field3 = new LocalDate(2024, 01, 19),
				Field4 = true,
				Field5 = 12,
				Field6 = 12.34567f,
				Field7 = 12.34f,
				Field8 = [id],
			},
		};

		SerializerOptions.CustomFields.Clear();
		document = await Client.Documents.Update(id, update);

		document.CustomFields.Should().BeEquivalentTo(update.CustomFields);

		SerializerOptions.CustomFields.Clear();
		var documents = await Client.Documents.GetAll<CustomFields>().ToListAsync();
		documents.Should().ContainSingle(d => d.Id == id).Which.Should().BeEquivalentTo(document);
	}

	private async Task<string> ReadStreamContentAsString(Stream stream)
	{
		await using (stream)
		{
			var reader = new StreamReader(stream);

			return await reader.ReadToEndAsync();
		}
	}
}
