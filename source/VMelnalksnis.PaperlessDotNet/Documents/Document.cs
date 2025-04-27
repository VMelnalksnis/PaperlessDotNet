// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

using NodaTime;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>An archived document.</summary>
public class Document
{
	/// <summary>Gets or sets the verbose filename if the archived document, if available.</summary>
	public string? ArchivedFileName { get; set; }

	/// <summary>Gets or sets the archive serial number.</summary>
	public uint? ArchiveSerialNumber { get; set; }

	/// <summary>Gets or sets the <see cref="Correspondents.Correspondent"/> id.</summary>
	[JsonPropertyName("correspondent")]
	public int? CorrespondentId { get; set; }

	/// <summary>Gets or sets the id of the document type.</summary>
	[JsonPropertyName("document_type")]
	public int? DocumentTypeId { get; set; }

	/// <summary>Gets or sets the id of the document type.</summary>
	[JsonPropertyName("storage_path")]
	public int? StoragePathId { get; set; }

	/// <summary>Gets or sets the verbose filename of the original document.</summary>
	public string OriginalFileName { get; set; } = null!;

	/// <summary>Gets or sets the datetime when the document was added to paperless.</summary>
	public OffsetDateTime Added { get; set; }

	/// <summary>Gets or sets the datetime when the document was last modified at.</summary>
	public OffsetDateTime Modified { get; set; }

	/// <summary>Gets or sets the datetime when the document was created at.</summary>
	public OffsetDateTime Created { get; set; }

	/// <summary>Gets or sets ids of the tags assigned to the document.</summary>
	[JsonPropertyName("tags")]
	public List<int> TagIds { get; set; } = null!;

	/// <summary>Gets or sets plain text content of the document.</summary>
	public string Content { get; set; } = null!;

	/// <summary>Gets or sets the title of the document.</summary>
	public string Title { get; set; } = null!;

	/// <summary>Gets or sets the id of the document.</summary>
	public int Id { get; set; }

	/// <summary>Gets or sets the owner id.</summary>
	[JsonPropertyName("owner")]
	public int? OwnerId { get; set; }
}

/// <summary>An archived document with custom fields.</summary>
/// <typeparam name="TFields">The type containing the custom fields.</typeparam>
#pragma warning disable SA1402
public class Document<TFields> : Document
{
	/// <summary>Gets or sets the custom fields of the document.</summary>
	public TFields? CustomFields { get; set; }
}
#pragma warning restore SA1402
