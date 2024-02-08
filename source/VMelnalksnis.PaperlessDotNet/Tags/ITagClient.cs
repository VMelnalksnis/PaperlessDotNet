// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.PaperlessDotNet.Tags;

/// <summary>Paperless API client for working with tags.</summary>
public interface ITagClient
{
	/// <summary>Gets all tags.</summary>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A enumerable which will asynchronously iterate over all available pages of tags.</returns>
	IAsyncEnumerable<Tag> GetAll(CancellationToken cancellationToken = default);

	/// <summary>Gets all tags.</summary>
	/// <param name="pageSize">The number of tags to get in a single request.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A enumerable which will asynchronously iterate over all available pages of tags.</returns>
	IAsyncEnumerable<Tag> GetAll(int pageSize, CancellationToken cancellationToken = default);

	/// <summary>Gets the tag with the specified id.</summary>
	/// <param name="id">The id of the tag to get.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The tag with the specified id if it exists; otherwise <see langword="null"/>.</returns>
	Task<Tag?> Get(int id, CancellationToken cancellationToken = default);

	/// <summary>Creates a new tag.</summary>
	/// <param name="tag">The tag to create.</param>
	/// <returns>The created tag.</returns>
	Task<Tag> Create(TagCreation tag);

	/// <summary>Deletes a tag.</summary>
	/// <param name="id">The id of the tag to delete.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	Task Delete(int id);
}
