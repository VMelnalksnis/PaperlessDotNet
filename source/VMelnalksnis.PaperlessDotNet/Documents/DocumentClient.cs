// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
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
	private readonly ITaskClient _taskClient;
	private readonly TimeSpan _taskPollingDelay;
	private readonly JsonSerializerOptions _options;
	private readonly PaperlessJsonSerializerOptions _paperlessOptions;

	/// <summary>Initializes a new instance of the <see cref="DocumentClient"/> class.</summary>
	/// <param name="httpClient">Http client configured for making requests to the Paperless API.</param>
	/// <param name="serializerOptions">Paperless specific instance of <see cref="JsonSerializerOptions"/>.</param>
	/// <param name="taskClient">Paperless task API client.</param>
	/// <param name="taskPollingDelay">The delay in ms between polling for import task completion.</param>
	public DocumentClient(HttpClient httpClient, PaperlessJsonSerializerOptions serializerOptions, ITaskClient taskClient, TimeSpan taskPollingDelay)
	{
		_httpClient = httpClient;
		_taskClient = taskClient;
		_taskPollingDelay = taskPollingDelay;
		_options = serializerOptions.Options;
		_paperlessOptions = serializerOptions;
	}

	/// <inheritdoc />
	public IAsyncEnumerable<Document> GetAll(CancellationToken cancellationToken = default)
	{
		return GetAllCore<Document>(Routes.Documents.Uri, cancellationToken);
	}

	/// <inheritdoc />
	public async IAsyncEnumerable<Document<TFields>> GetAll<TFields>([EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		if (_paperlessOptions.CustomFields.Count is 0)
		{
			await foreach (var unused in GetCustomFields(cancellationToken).ConfigureAwait(false))
			{
			}
		}

		var documents = GetAllCore<Document<TFields>>(Routes.Documents.Uri, cancellationToken);
		await foreach (var document in documents.ConfigureAwait(false))
		{
			yield return document;
		}
	}

	/// <inheritdoc />
	public IAsyncEnumerable<Document> GetAll(int pageSize, CancellationToken cancellationToken = default)
	{
		return GetAllCore<Document>(Routes.Documents.PagedUri(pageSize), cancellationToken);
	}

	/// <inheritdoc />
	public async IAsyncEnumerable<Document<TFields>> GetAll<TFields>(int pageSize, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		if (_paperlessOptions.CustomFields.Count is 0)
		{
			await foreach (var unused in GetCustomFields(cancellationToken).ConfigureAwait(false))
			{
			}
		}

		var documents = GetAllCore<Document<TFields>>(Routes.Documents.PagedUri(pageSize), cancellationToken);
		await foreach (var document in documents.ConfigureAwait(false))
		{
			yield return document;
		}
	}

	/// <inheritdoc />
	public Task<Document?> Get(int id, CancellationToken cancellationToken = default)
	{
		return GetCore<Document>(id, cancellationToken);
	}

	/// <inheritdoc />
	public async Task<Document<TFields>?> Get<TFields>(int id, CancellationToken cancellationToken = default)
	{
		if (_paperlessOptions.CustomFields.Count is 0)
		{
			await foreach (var unused in GetCustomFields(cancellationToken).ConfigureAwait(false))
			{
			}
		}

		return await GetCore<Document<TFields>>(id, cancellationToken).ConfigureAwait(false);
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

		foreach (var tag in document.TagIds ?? Array.Empty<int>())
		{
			content.Add(new StringContent(tag.ToString()), "tags");
		}

		if (document.ArchiveSerialNumber is { } archiveSerialNumber)
		{
			content.Add(new StringContent(archiveSerialNumber.ToString()), "archive_serial_number");
		}

		using var response = await _httpClient.PostAsync(Routes.Documents.CreateUri, content).ConfigureAwait(false);
		await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);

		// Until v1.9.2 paperless did not return the document import task id,
		// so it is not possible to get the document id
		var versionHeader = response.Headers.GetValues("x-version").SingleOrDefault();
		if (versionHeader is null || !Version.TryParse(versionHeader, out var version) || version <= _documentIdVersion)
		{
			return new ImportStarted();
		}

		var id = await response.Content.ReadFromJsonAsync(_options.GetTypeInfo<Guid>()).ConfigureAwait(false);
		var task = await _taskClient.Get(id).ConfigureAwait(false);

		while (task is not null && !task.Status.IsCompleted)
		{
			await Task.Delay(_taskPollingDelay).ConfigureAwait(false);
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

	/// <inheritdoc />
	public Task<Document> Update(int id, DocumentUpdate document)
	{
		return UpdateCore<Document, DocumentUpdate>(id, document);
	}

	/// <inheritdoc />
	public async Task<Document<TFields>> Update<TFields>(int id, DocumentUpdate<TFields> document)
	{
		if (_paperlessOptions.CustomFields.Count is 0)
		{
			await foreach (var unused in GetCustomFields().ConfigureAwait(false))
			{
			}
		}

		return await UpdateCore<Document<TFields>, DocumentUpdate<TFields>>(id, document).ConfigureAwait(false);
	}

	/// <inheritdoc />
	public IAsyncEnumerable<CustomField> GetCustomFields(CancellationToken cancellationToken = default)
	{
		return GetCustomFieldsCore(Routes.CustomFields.Uri, cancellationToken);
	}

	/// <inheritdoc />
	public IAsyncEnumerable<CustomField> GetCustomFields(int pageSize, CancellationToken cancellationToken = default)
	{
		return GetCustomFieldsCore(Routes.CustomFields.PagedUri(pageSize), cancellationToken);
	}

	/// <inheritdoc />
	public async Task<CustomField> CreateCustomField(CustomFieldCreation field)
	{
		using var response = await _httpClient
			.PostAsJsonAsync(Routes.CustomFields.Uri, field, _options.GetTypeInfo<CustomFieldCreation>())
			.ConfigureAwait(false);

		await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);

		var createdField = (await response.Content.ReadFromJsonAsync(_options.GetTypeInfo<CustomField>()).ConfigureAwait(false))!;
		_paperlessOptions.CustomFields.AddOrUpdate(createdField.Id, createdField, (_, _) => createdField);

		return createdField;
	}

	private IAsyncEnumerable<TDocument> GetAllCore<TDocument>(Uri requestUri, CancellationToken cancellationToken)
		where TDocument : Document
	{
		return _httpClient.GetPaginated(
			requestUri,
			_options.GetTypeInfo<PaginatedList<TDocument>>(),
			cancellationToken);
	}

	private Task<TDocument?> GetCore<TDocument>(int id, CancellationToken cancellationToken)
		where TDocument : Document
	{
		return _httpClient.GetFromJsonAsync(
			Routes.Documents.IdUri(id),
			_options.GetTypeInfo<TDocument>(),
			cancellationToken);
	}

	private async Task<TDocument> UpdateCore<TDocument, TUpdate>(int id, TUpdate update)
		where TDocument : Document
		where TUpdate : DocumentUpdate
	{
		using var response = await _httpClient
			.PatchAsJsonAsync(Routes.Documents.IdUri(id), update, _options.GetTypeInfo<TUpdate>())
			.ConfigureAwait(false);

		await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);

		return (await response.Content.ReadFromJsonAsync(_options.GetTypeInfo<TDocument>()).ConfigureAwait(false))!;
	}

	private async IAsyncEnumerable<CustomField> GetCustomFieldsCore(Uri requestUri, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		var fields = _httpClient.GetPaginated(requestUri, _options.GetTypeInfo<PaginatedList<CustomField>>(), cancellationToken);

		await foreach (var field in fields.ConfigureAwait(false))
		{
			_paperlessOptions.CustomFields.AddOrUpdate(field.Id, field, (_, _) => field);
			yield return field;
		}
	}
}
