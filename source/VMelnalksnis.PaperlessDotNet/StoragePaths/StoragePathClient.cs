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
using VMelnalksnis.PaperlessDotNet.StoragePaths;

namespace VMelnalksnis.PaperlessDotNet.StoragePaths;

/// <inheritdoc />
public sealed class StoragePathClient : IStoragePathClient
{
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _options;

	/// <summary>Initializes a new instance of the <see cref="StoragePathClient"/> class.</summary>
	/// <param name="httpClient">Http client configured for making requests to the Paperless API.</param>
	/// <param name="serializerOptions">Paperless specific instance of <see cref="JsonSerializerOptions"/>.</param>
	public StoragePathClient(HttpClient httpClient, PaperlessJsonSerializerOptions serializerOptions)
	{
		_httpClient = httpClient;
		_options = serializerOptions.Options;
	}

	/// <inheritdoc />
	public IAsyncEnumerable<StoragePath> GetAll(CancellationToken cancellationToken = default)
	{
		return GetAllCore(Routes.StoragePaths.Uri, cancellationToken);
	}

	/// <inheritdoc />
	public IAsyncEnumerable<StoragePath> GetAll(int pageSize, CancellationToken cancellationToken = default)
	{
		return GetAllCore(Routes.StoragePaths.PagedUri(pageSize), cancellationToken);
	}

	/// <inheritdoc />
	public Task<StoragePath?> Get(int id, CancellationToken cancellationToken = default)
	{
		return _httpClient.GetFromJsonAsync(
			Routes.StoragePaths.IdUri(id),
			_options.GetTypeInfo<StoragePath>(),
			cancellationToken);
	}

	/// <inheritdoc />
	public Task<StoragePath> Create(StoragePathCreation storagePath)
	{
		return _httpClient.PostAsJsonAsync(
			Routes.StoragePaths.Uri,
			storagePath,
			_options.GetTypeInfo<StoragePathCreation>(),
			_options.GetTypeInfo<StoragePath>());
	}

	/// <inheritdoc />
	public async Task Delete(int id)
	{
		using var response = await _httpClient.DeleteAsync(Routes.StoragePaths.IdUri(id)).ConfigureAwait(false);
		await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
	}

	private IAsyncEnumerable<StoragePath> GetAllCore(Uri requestUri, CancellationToken cancellationToken)
	{
		return _httpClient.GetPaginated(
			requestUri,
			_options.GetTypeInfo<PaginatedList<StoragePath>>(),
			cancellationToken);
	}
}
