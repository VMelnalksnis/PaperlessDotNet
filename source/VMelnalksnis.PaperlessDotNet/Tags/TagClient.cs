// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using VMelnalksnis.PaperlessDotNet.Serialization;

namespace VMelnalksnis.PaperlessDotNet.Tags;

/// <inheritdoc />
public sealed class TagClient : ITagClient
{
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _options;

	/// <summary>Initializes a new instance of the <see cref="TagClient"/> class.</summary>
	/// <param name="httpClient">Http client configured for making requests to the Paperless API.</param>
	/// <param name="serializerOptions">Paperless specific instance of <see cref="JsonSerializerOptions"/>.</param>
	public TagClient(HttpClient httpClient, PaperlessJsonSerializerOptions serializerOptions)
	{
		_httpClient = httpClient;
		_options = serializerOptions.Options;
	}

	/// <inheritdoc />
	public IAsyncEnumerable<Tag> GetAll(CancellationToken cancellationToken = default)
	{
		return GetAllCore(Routes.Tags.Uri, cancellationToken);
	}

	/// <inheritdoc />
	public IAsyncEnumerable<Tag> GetAll(int pageSize, CancellationToken cancellationToken = default)
	{
		return GetAllCore(Routes.Tags.PagedUri(pageSize), cancellationToken);
	}

	/// <inheritdoc />
	public Task<Tag?> Get(int id, CancellationToken cancellationToken = default)
	{
		return _httpClient.GetFromJsonAsync(
			Routes.Tags.IdUri(id),
			_options.GetTypeInfo<Tag>(),
			cancellationToken);
	}

	/// <inheritdoc />
	public Task<Tag> Create(TagCreation tag)
	{
		return _httpClient.PostAsJsonAsync(
			Routes.Tags.Uri,
			tag,
			_options.GetTypeInfo<TagCreation>(),
			_options.GetTypeInfo<Tag>());
	}

	/// <inheritdoc />
	public async Task Delete(int id)
	{
		using var response = await _httpClient.DeleteAsync(Routes.Tags.IdUri(id)).ConfigureAwait(false);
		await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
	}

	private IAsyncEnumerable<Tag> GetAllCore(Uri requestUri, CancellationToken cancellationToken)
	{
		return _httpClient.GetPaginated(
			requestUri,
			_options.GetTypeInfo<PaginatedList<Tag>>(),
			cancellationToken);
	}
}
