// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using NodaTime.Text;

using VMelnalksnis.PaperlessDotNet.Serialization;
using VMelnalksnis.PaperlessDotNet.Tasks;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <inheritdoc />
public sealed class DocumentClient : IDocumentClient
{
	private static readonly Version _documentIdVersion = new(1, 9, 2);

	private readonly HttpClient _httpClient;
	private readonly PaperlessJsonSerializerContext _context;
	private readonly ITaskClient _taskClient;

	/// <summary>Initializes a new instance of the <see cref="DocumentClient"/> class.</summary>
	/// <param name="httpClient">Http client configured for making requests to the Paperless API.</param>
	/// <param name="serializerOptions">Paperless specific instance of <see cref="JsonSerializerOptions"/>.</param>
	/// <param name="taskClient">Paperless task API client.</param>
	public DocumentClient(HttpClient httpClient, PaperlessJsonSerializerOptions serializerOptions, ITaskClient taskClient)
	{
		_httpClient = httpClient;
		_taskClient = taskClient;
		_context = serializerOptions.Context;
	}

	/// <inheritdoc />
	public IAsyncEnumerable<Document> GetAll(CancellationToken cancellationToken = default)
	{
		return _httpClient.GetPaginated(
			"/api/documents/",
			_context.PaginatedListDocument,
			cancellationToken);
	}

	/// <inheritdoc />
	public IAsyncEnumerable<Document> GetAll(int pageSize, CancellationToken cancellationToken = default)
	{
		return _httpClient.GetPaginated(
			$"/api/documents/?page_size={pageSize}",
			_context.PaginatedListDocument,
			cancellationToken);
	}

	/// <inheritdoc />
	public Task<Document?> Get(int id, CancellationToken cancellationToken = default)
	{
		return _httpClient.GetFromJsonAsync(
			$"/api/documents/{id}/",
			_context.Document,
			cancellationToken);
	}

	/// <inheritdoc />
	public async Task<DocumentCreationResult> Create(DocumentCreation document)
	{
		var content = new MultipartFormDataContent();
		content.Add(new StreamContent(document.Document), "document", document.FileName);

		if (document.Title is { } title)
		{
			content.Add(new StringContent(title), "title");
		}

		if (document.Created is { } created)
		{
			content.Add(new StringContent(InstantPattern.General.Format(created)), "created");
		}

		if (document.CorrespondentId is { } correspondent)
		{
			content.Add(new StringContent(correspondent.ToString()), "correspondent");
		}

		if (document.DocumentTypeId is { } documentType)
		{
			content.Add(new StringContent(documentType.ToString()), "document_type");
		}

		if (document.StoragePathId is { } storagePath)
		{
			content.Add(new StringContent(storagePath.ToString()), "storage_path");
		}

		foreach (var tag in document.TagIds)
		{
			content.Add(new StringContent(tag.ToString()), "tags");
		}

		if (document.ArchiveSerialNumber is { } archiveSerialNumber)
		{
			content.Add(new StringContent(archiveSerialNumber.ToString()), "archive_serial_number");
		}

		var response = await _httpClient.PostAsync("/api/documents/post_document/", content).ConfigureAwait(false);
		await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);

		// Until v1.9.2 paperless did not return the document import task id,
		// so it is not possible to get the document id
		var versionHeader = response.Headers.GetValues("x-version").SingleOrDefault();
		if (versionHeader is null || !Version.TryParse(versionHeader, out var version) || version <= _documentIdVersion)
		{
			return new ImportStarted();
		}

		var id = await response.Content.ReadFromJsonAsync(_context.Guid).ConfigureAwait(false);
		var task = await _taskClient.Get(id).ConfigureAwait(false);

		while (task is not null && !task.Status.IsCompleted)
		{
			await Task.Delay(100).ConfigureAwait(false);
			task = await _taskClient.Get(id).ConfigureAwait(false);
		}

		return task switch
		{
			null => new ImportFailed($"Could not find the import task by the given id {id}"),

			_ when task.RelatedDocument is { } documentId => new DocumentCreated(documentId),

			_ when task.Status == PaperlessTaskStatus.Success => new ImportFailed(
				$"Task status is {PaperlessTaskStatus.Success.Name}, but document id was not given"),
			_ when task.Status == PaperlessTaskStatus.Failure => new ImportFailed(task.Result),

			_ => throw new ArgumentOutOfRangeException(nameof(task.Status), task.Status, "Unexpected task result"),
		};
	}
}
