// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.PaperlessDotNet.StoragePaths;

/// <summary>Paperless API client for working with correspondents.</summary>
public interface IStoragePathClient
{
	/// <summary>Gets all storage paths.</summary>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A enumerable which will asynchronously iterate over all available pages of storage paths.</returns>
	IAsyncEnumerable<StoragePath> GetAll(CancellationToken cancellationToken = default);

	/// <summary>Gets all storage paths.</summary>
	/// <param name="pageSize">The number of storage paths to get in a single request.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A enumerable which will asynchronously iterate over all available pages of storage paths.</returns>
	IAsyncEnumerable<StoragePath> GetAll(int pageSize, CancellationToken cancellationToken = default);

	/// <summary>Gets the storage path with the specified id.</summary>
	/// <param name="id">The id of the storage path to get.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The storage path with the specified id if it exists; otherwise <see langword="null"/>.</returns>
	Task<StoragePath?> Get(int id, CancellationToken cancellationToken = default);

	/// <summary>Creates a new storage path.</summary>
	/// <param name="storagePath">The correspondent to create.</param>
	/// <returns>The created correspondent.</returns>
	Task<StoragePath> Create(StoragePathCreation storagePath);

	/// <summary>Deletes a storage path.</summary>
	/// <param name="id">The id of the storage path to delete.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	Task Delete(int id);
}
