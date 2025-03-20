using System;
using System.Collections.Generic;
using NodaTime;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>
/// Filter parameters for document queries.
/// </summary>
public class DocumentFilter
{
    // ID filters
    public int? Id { get; set; }
    public IEnumerable<int>? Ids { get; set; }

    // Title filters
    public string? TitleStartsWith { get; set; }
    public string? TitleEndsWith { get; set; }
    public string? TitleContains { get; set; }
    public string? TitleExact { get; set; }

    // Content filters
    public string? ContentStartsWith { get; set; }
    public string? ContentEndsWith { get; set; }
    public string? ContentContains { get; set; }
    public string? ContentExact { get; set; }

    // Archive Serial Number filters
    public int? ArchiveSerialNumber { get; set; }
    public int? ArchiveSerialNumberGt { get; set; }
    public int? ArchiveSerialNumberGte { get; set; }
    public int? ArchiveSerialNumberLt { get; set; }
    public int? ArchiveSerialNumberLte { get; set; }
    public bool? ArchiveSerialNumberIsNull { get; set; }

    // Created date filters
    public int? CreatedYear { get; set; }
    public int? CreatedMonth { get; set; }
    public int? CreatedDay { get; set; }
    public Instant? CreatedDateGt { get; set; }
    public Instant? CreatedGt { get; set; }
    public Instant? CreatedDateLt { get; set; }
    public Instant? CreatedLt { get; set; }

    // Added date filters
    public int? AddedYear { get; set; }
    public int? AddedMonth { get; set; }
    public int? AddedDay { get; set; }
    public Instant? AddedDateGt { get; set; }
    public Instant? AddedGt { get; set; }
    public Instant? AddedDateLt { get; set; }
    public Instant? AddedLt { get; set; }

    // Modified date filters
    public int? ModifiedYear { get; set; }
    public int? ModifiedMonth { get; set; }
    public int? ModifiedDay { get; set; }
    public Instant? ModifiedDateGt { get; set; }
    public Instant? ModifiedGt { get; set; }
    public Instant? ModifiedDateLt { get; set; }
    public Instant? ModifiedLt { get; set; }

    // Original filename filters
    public string? OriginalFilenameStartsWith { get; set; }
    public string? OriginalFilenameEndsWith { get; set; }
    public string? OriginalFilenameContains { get; set; }
    public string? OriginalFilenameExact { get; set; }

    // Checksum filters
    public string? ChecksumStartsWith { get; set; }
    public string? ChecksumEndsWith { get; set; }
    public string? ChecksumContains { get; set; }
    public string? ChecksumExact { get; set; }

    // Correspondent filters
    public bool? CorrespondentIsNull { get; set; }
    public IEnumerable<int>? CorrespondentIds { get; set; }
    public int? CorrespondentId { get; set; }
    public string? CorrespondentNameStartsWith { get; set; }
    public string? CorrespondentNameEndsWith { get; set; }
    public string? CorrespondentNameContains { get; set; }
    public string? CorrespondentNameExact { get; set; }

    // Tags filters
    public IEnumerable<int>? TagIds { get; set; }
    public int? TagsId { get; set; }
    public string? TagsNameStartsWith { get; set; }
    public string? TagsNameEndsWith { get; set; }
    public string? TagsNameContains { get; set; }
    public string? TagsNameExact { get; set; }

    // Document type filters
    public bool? DocumentTypeIsNull { get; set; }
    public IEnumerable<int>? DocumentTypeIds { get; set; }
    public int? DocumentTypeId { get; set; }
    public string? DocumentTypeNameStartsWith { get; set; }
    public string? DocumentTypeNameEndsWith { get; set; }
    public string? DocumentTypeNameContains { get; set; }
    public string? DocumentTypeNameExact { get; set; }

    // Storage path filters
    public bool? StoragePathIsNull { get; set; }
    public IEnumerable<int>? StoragePathIds { get; set; }
    public int? StoragePathId { get; set; }
    public string? StoragePathNameStartsWith { get; set; }
    public string? StoragePathNameEndsWith { get; set; }
    public string? StoragePathNameContains { get; set; }
    public string? StoragePathNameExact { get; set; }

    // Owner filters
    public bool? OwnerIsNull { get; set; }
    public IEnumerable<int>? OwnerIds { get; set; }
    public int? OwnerId { get; set; }

    // Custom fields filters
    public string? CustomFieldsContains { get; set; }
    public bool? IsTagged { get; set; }
    public IEnumerable<int>? RequiredTagIds { get; set; }
    public IEnumerable<int>? ExcludedTagIds { get; set; }
    public IEnumerable<int>? ExcludedCorrespondentIds { get; set; }
    public IEnumerable<int>? ExcludedDocumentTypeIds { get; set; }
    public IEnumerable<int>? ExcludedStoragePathIds { get; set; }
    public bool? IsInInbox { get; set; }
    public string? TitleContent { get; set; }
    public IEnumerable<int>? ExcludedOwnerIds { get; set; }
    public IEnumerable<int>? RequiredCustomFieldIds { get; set; }
    public IEnumerable<int>? ExcludedCustomFieldIds { get; set; }
    public IEnumerable<int>? CustomFieldIds { get; set; }
    public bool? HasCustomFields { get; set; }
    public string? CustomFieldQuery { get; set; }
    public int? SharedById { get; set; }

    // Ordering
    public string? Ordering { get; set; }
} 