// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

using NodaTime;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>Common information needed to create or update a document.</summary>
public abstract class DocumentUpdateBase
{
	/// <inheritdoc cref="Document.Created"/>
	public Instant? Created { get; init; }

	/// <inheritdoc cref="Document.Title"/>
	public string? Title { get; init; }

	/// <inheritdoc cref="Document.CorrespondentId"/>
	[JsonPropertyName("correspondent")]
	public int? CorrespondentId { get; init; }

	/// <inheritdoc cref="Document.DocumentTypeId"/>
	[JsonPropertyName("document_type")]
	public int? DocumentTypeId { get; init; }

	/// <summary>Gets the id of the storage path.</summary>
	[JsonPropertyName("storage_path")]
	public int? StoragePathId { get; init; }

	/// <inheritdoc cref="Document.TagIds"/>
	[JsonPropertyName("tags")]
	public int[]? TagIds { get; init; }

	/// <inheritdoc cref="Document.ArchivedFileName"/>
	public uint? ArchiveSerialNumber { get; init; }
}
