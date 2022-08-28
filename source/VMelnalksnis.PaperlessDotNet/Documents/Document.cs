// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

using NodaTime;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>An archived document.</summary>
public sealed class Document
{
	/// <summary>Gets or sets the verbose filename if the archived document, if available.</summary>
	[JsonPropertyName("archived_file_name")]
	public string? ArchivedFileName { get; set; }

	/// <summary>Gets or sets the archive serial number.</summary>
	[JsonPropertyName("archive_serial_number")]
	public int? ArchiveSerialNumber { get; set; }

	/// <summary>Gets or sets the correspondent id.</summary>
	[JsonPropertyName("correspondent")]
	public int? CorrespondentId { get; set; }

	/// <summary>Gets or sets the id of the document type.</summary>
	[JsonPropertyName("document_type")]
	public int? DocumentTypeId { get; set; }

	/// <summary>Gets or sets the verbose filename of the original document.</summary>
	[JsonPropertyName("original_file_name")]
	public string OriginalFileName { get; set; } = null!;

	/// <summary>Gets or sets the instant at which the document was added to paperless.</summary>
	public Instant Added { get; set; }

	/// <summary>Gets or sets the instant at which the document was last modified at.</summary>
	public Instant Modified { get; set; }

	/// <summary>Gets or sets the instant at which the document was created at.</summary>
	public Instant Created { get; set; }

	/// <summary>Gets or sets ids of the tags assigned to the document.</summary>
	[JsonPropertyName("tags")]
	public List<int> TagIds { get; set; } = null!;

	/// <summary>Gets or sets plain text content of the document.</summary>
	public string Content { get; set; } = null!;

	/// <summary>Gets or sets the title of the document.</summary>
	public string Title { get; set; } = null!;

	/// <summary>Gets or sets the id of the document.</summary>
	public int Id { get; set; }
}
