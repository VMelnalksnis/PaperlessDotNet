// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
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

		if (PaperlessVersion < new Version(2, 0))
		{
			result.Should().BeOfType<ImportStarted>();
			return;
		}

		var id = result.Should().BeOfType<DocumentCreated>().Subject.Id;
		var document = (await Client.Documents.Get(id))!;
		var documents = await Client.Documents.GetAll().ToListAsync();

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
#if NET6_0_OR_GREATER
		document.Content.ReplaceLineEndings().Should().BeEquivalentTo(content);
#else
		document.Content.Replace("\n", Environment.NewLine).Replace("\r\n", Environment.NewLine).Should().Be(content);
#endif

		var update = new DocumentUpdate { Title = $"{document.Title}1" };
		var updatedDocument = await Client.Documents.Update(id, update);

		updatedDocument.Title.Should().Be(update.Title);

		await Client.Correspondents.Delete(correspondent.Id);
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

		var customFields = await Client.Documents.GetCustomFields().ToListAsync();
		customFields.Should().HaveCount(8);

		var paginatedCustomFields = await Client.Documents.GetCustomFields(1).ToListAsync();
		paginatedCustomFields.Should().BeEquivalentTo(customFields);

		await using var documentStream = typeof(DocumentClientTests).GetResource(documentName);
		var documentCreation = new DocumentCreation(documentStream, documentName);

		var result = await Client.Documents.Create(documentCreation);

		SerializerOptions.CustomFields.Clear();

		var id = result.Should().BeOfType<DocumentCreated>().Subject.Id;
		var document = (await Client.Documents.Get<CustomFields>(id))!;

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
}
