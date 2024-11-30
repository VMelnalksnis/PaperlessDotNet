// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>Read-only metadata about a document.</summary>
/// <remarks>
/// This information is not served along with the document objects,
/// since it requires reading files and would therefore slow down document lists considerably.
/// </remarks>
public sealed class DocumentMetadata
{
	/// <summary>Initializes a new instance of the <see cref="DocumentMetadata"/> class.</summary>
	/// <param name="mediaFilename">The current filename of the document.</param>
	/// <param name="originalChecksum">The <see cref="MD5"/> checksum of the original document.</param>
	/// <param name="originalSize">The size of the original document.</param>
	/// <param name="originalMimeType">The MIME type of the original document.</param>
	/// <param name="originalFilename">The filename of the original document.</param>
	/// <param name="originalMetadata">The metadata associated with the original document.</param>
	/// <param name="language">The language of the document.</param>
	/// <param name="hasArchiveVersion">Whether the document is archived or not.</param>
	[JsonConstructor]
	public DocumentMetadata(
		string mediaFilename,
		string originalChecksum,
		int originalSize,
		string originalMimeType,
		string originalFilename,
		DocumentMetadataProperty[] originalMetadata,
		string language,
		bool hasArchiveVersion)
	{
		MediaFilename = mediaFilename;
		OriginalChecksum = originalChecksum;
		OriginalSize = originalSize;
		OriginalMimeType = originalMimeType;
		OriginalFilename = originalFilename;
		OriginalMetadata = originalMetadata;
		Language = language;
		HasArchiveVersion = hasArchiveVersion;
	}

	/// <summary>Gets the current filename of the document, under which it is stored inside the media directory.</summary>
	[JsonPropertyName("media_filename")]
	public string MediaFilename { get; }

	/// <summary>Gets the <see cref="MD5"/> checksum of the original document.</summary>
	[JsonPropertyName("original_checksum")]
	public string OriginalChecksum { get; }

	/// <summary>Gets the size of the original document, in bytes.</summary>
	[JsonPropertyName("original_size")]
	public int OriginalSize { get; }

	/// <summary>Gets the MIME type of the original document.</summary>
	[JsonPropertyName("original_mime_type")]
	public string OriginalMimeType { get; }

	/// <summary>Gets the filename of the original document.</summary>
	[JsonPropertyName("original_filename")]
	public string OriginalFilename { get; }

	/// <summary>Gets the metadata associated with the original document.</summary>
	[JsonPropertyName("original_metadata")]
	public DocumentMetadataProperty[] OriginalMetadata { get; }

	/// <summary>Gets the language of the document.</summary>
	[JsonPropertyName("lang")]
	public string Language { get; }

	/// <summary>Gets a value indicating whether the document is archived or not.</summary>
	[JsonPropertyName("has_archive_version")]
	public bool HasArchiveVersion { get; }

	/// <summary>Gets the current filename of the archived document.</summary>
	[JsonPropertyName("archive_media_filename")]
	public string? ArchiveMediaFilename { get; init; }

	/// <summary>Gets the <see cref="MD5"/> checksum of the archived document.</summary>
	[JsonPropertyName("archive_checksum")]
	public string? ArchiveChecksum { get; init; }

	/// <summary>Gets the size of the archived document, in bytes.</summary>
	[JsonPropertyName("archive_size")]
	public int? ArchiveSize { get; init; }

	/// <summary>Gets the metadata associated with the archived document.</summary>
	[JsonPropertyName("archive_metadata")]
	public DocumentMetadataProperty[]? ArchiveMetadata { get; init; }
}
