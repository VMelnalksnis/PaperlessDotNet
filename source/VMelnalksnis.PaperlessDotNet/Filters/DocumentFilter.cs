// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace VMelnalksnis.PaperlessDotNet.Filters;

/// <summary>Fields by which <see cref="Documents.Document"/> can be filtered by.</summary>
[UsedImplicitly(ImplicitUseKindFlags.Access | ImplicitUseKindFlags.Assign, ImplicitUseTargetFlags.Members)]
#if NET8_0_OR_GREATER
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
public sealed class DocumentFilter : PaginatedFilter
{
	/// <inheritdoc cref="Documents.Document.Added"/>
	public DateTime Added { get; }

	/// <inheritdoc cref="Documents.Document.ArchiveSerialNumber"/>
	[DataMember(Name = "archive_serial_number")]
	public uint? ArchiveSerialNumber { get; }

	/// <summary>Gets the checksum of the document content.</summary>
	public string Checksum { get; } = null!;

	/// <inheritdoc cref="Documents.Document.Content"/>
	public string Content { get; } = null!;

	/// <inheritdoc cref="Documents.Document.CorrespondentId"/>
	public NameFilter? Correspondent { get; }

	/// <inheritdoc cref="Documents.Document.Created"/>
	public DateTime Created { get; }

	/// <inheritdoc cref="Documents.Document.DocumentTypeId"/>
	[DataMember(Name = "document_type")]
	public NameFilter? DocumentType { get; }

	/// <inheritdoc cref="Documents.Document.Id"/>
	public int? Id { get; }

	/// <inheritdoc cref="Documents.Document.Added"/>
	[DataMember(Name = "is_in_inbox")]
	public bool IsInInbox { get; }

	/// <summary>Gets a value indicating whether the document has tags.</summary>
	[DataMember(Name = "is_tagged")]
	public bool IsTagged { get; }

	/// <inheritdoc cref="Documents.Document.Modified"/>
	public DateTime Modified { get; }

	/// <inheritdoc cref="Documents.Document.OriginalFileName"/>
	[DataMember(Name = "original_filename")]
	public string OriginalFilename { get; } = null!;

	/// <inheritdoc cref="Documents.Document.OwnerId"/>
	public IdFilter? Owner { get; }

	/// <inheritdoc cref="Documents.Document.StoragePathId"/>
	[DataMember(Name = "storage_path")]
	public NameFilter? StoragePath { get; }

	/// <inheritdoc cref="Documents.Document.TagIds"/>
	public NameFilter? Tags { get; }

	/// <inheritdoc cref="Documents.Document.Title"/>
	public string Title { get; } = null!;
}
