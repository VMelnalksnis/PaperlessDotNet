// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
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
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <typeparam name="TFields">The type containing the custom fields.</typeparam>
	/// <returns>A enumerable which will asynchronously iterate over all available pages of documents.</returns>
	IAsyncEnumerable<Document<TFields>> GetAll<TFields>(CancellationToken cancellationToken = default);

	/// <summary>Gets all documents.</summary>
	/// <param name="pageSize">The number of documents to get in a single request.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A enumerable which will asynchronously iterate over all available pages of documents.</returns>
	IAsyncEnumerable<Document> GetAll(int pageSize, CancellationToken cancellationToken = default);

	/// <summary>Gets all documents.</summary>
	/// <param name="pageSize">The number of documents to get in a single request.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <typeparam name="TFields">The type containing the custom fields.</typeparam>
	/// <returns>A enumerable which will asynchronously iterate over all available pages of documents.</returns>
	IAsyncEnumerable<Document<TFields>> GetAll<TFields>(int pageSize, CancellationToken cancellationToken = default);

	/// <summary>Gets the document with the specified id.</summary>
	/// <param name="id">The id of the document to get.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The document with the specified id if it exists; otherwise <see langword="null"/>.</returns>
	Task<Document?> Get(int id, CancellationToken cancellationToken = default);

	/// <summary>Gets the document with the specified id.</summary>
	/// <param name="id">The id of the document to get.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <typeparam name="TFields">The type containing the custom fields.</typeparam>
	/// <returns>The document with the specified id if it exists; otherwise <see langword="null"/>.</returns>
	Task<Document<TFields>?> Get<TFields>(int id, CancellationToken cancellationToken = default);

	/// <summary>Downloads the archived file of the document.</summary>
	/// <param name="id">The id of the document to download.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The content of the document.</returns>
	Task<DocumentContent> Download(int id, CancellationToken cancellationToken = default);

	/// <summary>Downloads the original file of the document.</summary>
	/// <param name="id">The id of the document to download.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The content of the document.</returns>
	Task<DocumentContent> DownloadOriginal(int id, CancellationToken cancellationToken = default);

	/// <summary>Display the document inline, without downloading it.</summary>
	/// <param name="id">The id of the document to download.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The content of the document.</returns>
	Task<DocumentContent> DownloadPreview(int id, CancellationToken cancellationToken = default);

	/// <summary>Display the original document inline, without downloading it.</summary>
	/// <param name="id">The id of the document to download.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The content of the document.</returns>
	Task<DocumentContent> DownloadOriginalPreview(int id, CancellationToken cancellationToken = default);

	/// <summary>Download the PNG thumbnail of a document.</summary>
	/// <param name="id">The id of the document to download.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The content of the document.</returns>
	Task<DocumentContent> DownloadThumbnail(int id, CancellationToken cancellationToken = default);

	/// <summary>Creates a new document.</summary>
	/// <param name="document">The document to create.</param>
	/// <returns>Result of creating the document.</returns>
	Task<DocumentCreationResult> Create(DocumentCreation document);

	/// <summary>Updates an existing document.</summary>
	/// <param name="id">The id of the document to update.</param>
	/// <param name="document">The fields of the document to update.</param>
	/// <returns>The updated document.</returns>
	Task<Document> Update(int id, DocumentUpdate document);

	/// <summary>Updates an existing document.</summary>
	/// <param name="id">The id of the document to update.</param>
	/// <param name="document">The fields of the document to update.</param>
	/// <typeparam name="TFields">The type containing the custom fields.</typeparam>
	/// <returns>The updated document.</returns>
	Task<Document<TFields>> Update<TFields>(int id, DocumentUpdate<TFields> document);

	/// <summary>Deletes the document with the specified id.</summary>
	/// <param name="id">The id of the document to delete.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	Task Delete(int id);

	/// <summary>Gets all custom fields.</summary>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A collection of all custom fields.</returns>
	IAsyncEnumerable<CustomField> GetCustomFields(CancellationToken cancellationToken = default);

	/// <summary>Gets all documents.</summary>
	/// <param name="pageSize">The number of documents to get in a single request.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A enumerable which will asynchronously iterate over all available pages of documents.</returns>
	IAsyncEnumerable<CustomField> GetCustomFields(int pageSize, CancellationToken cancellationToken = default);

	/// <summary>Creates a new custom field.</summary>
	/// <param name="field">The custom field to create.</param>
	/// <returns>The created field.</returns>
	Task<CustomField> CreateCustomField(CustomFieldCreation field);
}
