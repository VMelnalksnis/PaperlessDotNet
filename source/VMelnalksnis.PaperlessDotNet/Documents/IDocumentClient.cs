// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>Paperless API client for working with documents.</summary>
public interface IDocumentClient
{
	/// <summary>Gets all documents.</summary>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A enumerable which will asynchronously iterate over all available pages of documents.</returns>
	IAsyncEnumerable<Document> GetAll(CancellationToken cancellationToken = default);

	/// <summary>Gets all documents.</summary>
	/// <param name="pageSize">The number of documents to get in a single request.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A enumerable which will asynchronously iterate over all available pages of documents.</returns>
	IAsyncEnumerable<Document> GetAll(int pageSize, CancellationToken cancellationToken = default);

	/// <summary>Gets the document with the specified id.</summary>
	/// <param name="id">The id of the document to get.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The document with the specified id if it exists; otherwise <see langword="null"/>.</returns>
	Task<Document?> Get(int id, CancellationToken cancellationToken = default);

	/// <summary>Creates a new document.</summary>
	/// <param name="document">The document to create.</param>
	/// <returns>Result of creating the document.</returns>
	Task<DocumentCreationResult> Create(DocumentCreation document);
}
