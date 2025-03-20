using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NodaTime.Text;

namespace VMelnalksnis.PaperlessDotNet.Documents;

internal static class DocumentFilterExtensions
{
    public static string ToQueryString(this DocumentFilter filter)
    {
        var parameters = new List<string>();

        AddParameter(parameters, "id__in", filter.Ids);
        AddParameter(parameters, "id", filter.Id);
        
        AddParameter(parameters, "title__istartswith", filter.TitleStartsWith);
        AddParameter(parameters, "title__iendswith", filter.TitleEndsWith);
        AddParameter(parameters, "title__icontains", filter.TitleContains);
        AddParameter(parameters, "title__iexact", filter.TitleExact);

        AddParameter(parameters, "content__istartswith", filter.ContentStartsWith);
        AddParameter(parameters, "content__iendswith", filter.ContentEndsWith);
        AddParameter(parameters, "content__icontains", filter.ContentContains);
        AddParameter(parameters, "content__iexact", filter.ContentExact);

        AddParameter(parameters, "archive_serial_number", filter.ArchiveSerialNumber);
        AddParameter(parameters, "archive_serial_number__gt", filter.ArchiveSerialNumberGt);
        AddParameter(parameters, "archive_serial_number__gte", filter.ArchiveSerialNumberGte);
        AddParameter(parameters, "archive_serial_number__lt", filter.ArchiveSerialNumberLt);
        AddParameter(parameters, "archive_serial_number__lte", filter.ArchiveSerialNumberLte);
        AddParameter(parameters, "archive_serial_number__isnull", filter.ArchiveSerialNumberIsNull);

        AddParameter(parameters, "created__year", filter.CreatedYear);
        AddParameter(parameters, "created__month", filter.CreatedMonth);
        AddParameter(parameters, "created__day", filter.CreatedDay);
        AddInstantParameter(parameters, "created__date__gt", filter.CreatedDateGt);
        AddInstantParameter(parameters, "created__gt", filter.CreatedGt);
        AddInstantParameter(parameters, "created__date__lt", filter.CreatedDateLt);
        AddInstantParameter(parameters, "created__lt", filter.CreatedLt);

        AddParameter(parameters, "added__year", filter.AddedYear);
        AddParameter(parameters, "added__month", filter.AddedMonth);
        AddParameter(parameters, "added__day", filter.AddedDay);
        AddInstantParameter(parameters, "added__date__gt", filter.AddedDateGt);
        AddInstantParameter(parameters, "added__gt", filter.AddedGt);
        AddInstantParameter(parameters, "added__date__lt", filter.AddedDateLt);
        AddInstantParameter(parameters, "added__lt", filter.AddedLt);

        AddParameter(parameters, "modified__year", filter.ModifiedYear);
        AddParameter(parameters, "modified__month", filter.ModifiedMonth);
        AddParameter(parameters, "modified__day", filter.ModifiedDay);
        AddInstantParameter(parameters, "modified__date__gt", filter.ModifiedDateGt);
        AddInstantParameter(parameters, "modified__gt", filter.ModifiedGt);
        AddInstantParameter(parameters, "modified__date__lt", filter.ModifiedDateLt);
        AddInstantParameter(parameters, "modified__lt", filter.ModifiedLt);

        AddParameter(parameters, "original_filename__istartswith", filter.OriginalFilenameStartsWith);
        AddParameter(parameters, "original_filename__iendswith", filter.OriginalFilenameEndsWith);
        AddParameter(parameters, "original_filename__icontains", filter.OriginalFilenameContains);
        AddParameter(parameters, "original_filename__iexact", filter.OriginalFilenameExact);

        AddParameter(parameters, "checksum__istartswith", filter.ChecksumStartsWith);
        AddParameter(parameters, "checksum__iendswith", filter.ChecksumEndsWith);
        AddParameter(parameters, "checksum__icontains", filter.ChecksumContains);
        AddParameter(parameters, "checksum__iexact", filter.ChecksumExact);

        AddParameter(parameters, "correspondent__isnull", filter.CorrespondentIsNull);
        AddParameter(parameters, "correspondent__id__in", filter.CorrespondentIds);
        AddParameter(parameters, "correspondent__id", filter.CorrespondentId);
        AddParameter(parameters, "correspondent__name__istartswith", filter.CorrespondentNameStartsWith);
        AddParameter(parameters, "correspondent__name__iendswith", filter.CorrespondentNameEndsWith);
        AddParameter(parameters, "correspondent__name__icontains", filter.CorrespondentNameContains);
        AddParameter(parameters, "correspondent__name__iexact", filter.CorrespondentNameExact);

        AddParameter(parameters, "tags__id__in", filter.TagIds);
        AddParameter(parameters, "tags__id", filter.TagsId);
        AddParameter(parameters, "tags__name__istartswith", filter.TagsNameStartsWith);
        AddParameter(parameters, "tags__name__iendswith", filter.TagsNameEndsWith);
        AddParameter(parameters, "tags__name__icontains", filter.TagsNameContains);
        AddParameter(parameters, "tags__name__iexact", filter.TagsNameExact);

        AddParameter(parameters, "document_type__isnull", filter.DocumentTypeIsNull);
        AddParameter(parameters, "document_type__id__in", filter.DocumentTypeIds);
        AddParameter(parameters, "document_type__id", filter.DocumentTypeId);
        AddParameter(parameters, "document_type__name__istartswith", filter.DocumentTypeNameStartsWith);
        AddParameter(parameters, "document_type__name__iendswith", filter.DocumentTypeNameEndsWith);
        AddParameter(parameters, "document_type__name__icontains", filter.DocumentTypeNameContains);
        AddParameter(parameters, "document_type__name__iexact", filter.DocumentTypeNameExact);

        AddParameter(parameters, "storage_path__isnull", filter.StoragePathIsNull);
        AddParameter(parameters, "storage_path__id__in", filter.StoragePathIds);
        AddParameter(parameters, "storage_path__id", filter.StoragePathId);
        AddParameter(parameters, "storage_path__name__istartswith", filter.StoragePathNameStartsWith);
        AddParameter(parameters, "storage_path__name__iendswith", filter.StoragePathNameEndsWith);
        AddParameter(parameters, "storage_path__name__icontains", filter.StoragePathNameContains);
        AddParameter(parameters, "storage_path__name__iexact", filter.StoragePathNameExact);

        AddParameter(parameters, "owner__isnull", filter.OwnerIsNull);
        AddParameter(parameters, "owner__id__in", filter.OwnerIds);
        AddParameter(parameters, "owner__id", filter.OwnerId);

        AddParameter(parameters, "custom_fields__icontains", filter.CustomFieldsContains);
        AddParameter(parameters, "is_tagged", filter.IsTagged);
        AddParameter(parameters, "tags__id__all", filter.RequiredTagIds);
        AddParameter(parameters, "tags__id__none", filter.ExcludedTagIds);
        AddParameter(parameters, "correspondent__id__none", filter.ExcludedCorrespondentIds);
        AddParameter(parameters, "document_type__id__none", filter.ExcludedDocumentTypeIds);
        AddParameter(parameters, "storage_path__id__none", filter.ExcludedStoragePathIds);
        AddParameter(parameters, "is_in_inbox", filter.IsInInbox);
        AddParameter(parameters, "title_content", filter.TitleContent);
        AddParameter(parameters, "owner__id__none", filter.ExcludedOwnerIds);
        AddParameter(parameters, "custom_fields__id__all", filter.RequiredCustomFieldIds);
        AddParameter(parameters, "custom_fields__id__none", filter.ExcludedCustomFieldIds);
        AddParameter(parameters, "custom_fields__id__in", filter.CustomFieldIds);
        AddParameter(parameters, "has_custom_fields", filter.HasCustomFields);
        AddParameter(parameters, "custom_field_query", filter.CustomFieldQuery);
        AddParameter(parameters, "shared_by__id", filter.SharedById);

        AddParameter(parameters, "ordering", filter.Ordering);

        return parameters.Count > 0 ? "?" + string.Join("&", parameters) : string.Empty;
    }

    private static void AddParameter(List<string> parameters, string name, object? value)
    {
        if (value == null)
        {
            return;
        }

        if (value is IEnumerable<int> intValues)
        {
            var valueString = string.Join(",", intValues);
            if (!string.IsNullOrEmpty(valueString))
            {
                parameters.Add($"{name}={HttpUtility.UrlEncode(valueString)}");
            }
        }
        else
        {
            parameters.Add($"{name}={HttpUtility.UrlEncode(value.ToString())}");
        }
    }

    private static void AddInstantParameter(List<string> parameters, string name, NodaTime.Instant? value)
    {
        if (value.HasValue)
        {
            parameters.Add($"{name}={HttpUtility.UrlEncode(InstantPattern.General.Format(value.Value))}");
        }
    }
} 