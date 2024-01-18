// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using NodaTime;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>Information needed to create a new <see cref="Document"/>.</summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required endpoints for testing not implemented")]
public sealed class DocumentCreation
{
	/// <summary>Initializes a new instance of the <see cref="DocumentCreation"/> class.</summary>
	/// <param name="document">The document content.</param>
	/// <param name="fileName">The name of the file.</param>
	public DocumentCreation(Stream document, string fileName)
	{
		Document = document;
		FileName = fileName;

		TagIds = Array.Empty<int>();
	}

	/// <summary>Gets the content of the document.</summary>
	public Stream Document { get; }

	/// <inheritdoc cref="Document.OriginalFileName"/>
	public string FileName { get; }

	/// <inheritdoc cref="Document.Created"/>
	public Instant? Created { get; init; }

	/// <inheritdoc cref="Document.Title"/>
	public string? Title { get; init; }

	/// <inheritdoc cref="Document.CorrespondentId"/>
	public int? CorrespondentId { get; init; }

	/// <inheritdoc cref="Document.DocumentTypeId"/>
	public int? DocumentTypeId { get; init; }

	/// <summary>Gets the id of the storage path.</summary>
	public int? StoragePathId { get; init; }

	/// <inheritdoc cref="Document.TagIds"/>
	public int[] TagIds { get; init; }

	/// <inheritdoc cref="Document.ArchivedFileName"/>
	public uint? ArchiveSerialNumber { get; init; }
}
