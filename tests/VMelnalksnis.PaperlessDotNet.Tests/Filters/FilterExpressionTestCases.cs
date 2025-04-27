// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using VMelnalksnis.PaperlessDotNet.Documents;
using VMelnalksnis.PaperlessDotNet.Filters;

namespace VMelnalksnis.PaperlessDotNet.Tests.Filters;

public sealed class FilterExpressionTestCases : TheoryData<Expression<Func<DocumentFilter, bool>>,
	Expression<Func<Document, object>>?, string>
{
	private static readonly int[] _ids = [5, 23];

	public FilterExpressionTestCases()
	{
		var date = new DateTime(2025, 04, 23, 12, 23, 45);

		Add(filter => filter.ArchiveSerialNumber == 1, null, "archive_serial_number=1");
		Add(filter => filter.ArchiveSerialNumber == null, null, "archive_serial_number__isnull=true");
		Add(filter => filter.ArchiveSerialNumber != null, null, "archive_serial_number__isnull=false");
		Add(filter => filter.ArchiveSerialNumber > 1, null, "archive_serial_number__gt=1");
		Add(filter => filter.ArchiveSerialNumber >= 1, null, "archive_serial_number__gte=1");
		Add(filter => filter.ArchiveSerialNumber < 1, null, "archive_serial_number__lt=1");
		Add(filter => filter.ArchiveSerialNumber <= 1, null, "archive_serial_number__lte=1");

		Add(filter => filter.Content.Contains("foo"), null, "content__icontains=foo");
		Add(filter => filter.Content.StartsWith("foo"), null, "content__istartswith=foo");
		Add(filter => filter.Content.EndsWith("foo"), null, "content__iendswith=foo");
		Add(filter => filter.Content.Equals("foo"), null, "content__iexact=foo");
		Add(filter => filter.Content == "foo", null, "content__iexact=foo");

		Add(filter => filter.Correspondent!.Name.Contains("foo"), null, "correspondent__name__icontains=foo");
		Add(filter => filter.Correspondent!.Name == "foo", null, "correspondent__name__iexact=foo");
		Add(filter => _ids.Contains(filter.Correspondent!.Id), null, "correspondent__id__in=5,23");
		Add(filter => new List<int> { 1 }.Contains(filter.Correspondent!.Id), null, "correspondent__id__in=1");
		Add(filter => new[] { 1, 2 }.Contains(filter.Correspondent!.Id), null, "correspondent__id__in=1,2");

		Add(filter => filter.Added.Date <= date, null, "added__date__lte=2025-04-23");
		Add(filter => filter.Added <= date, null, "added__lte=2025-04-23T12%3A23%3A45");
		Add(filter => filter.Added.Day == 23, null, "added__day=23");
		Add(filter => filter.Added.Month == 4, null, "added__month=4");
		Add(filter => filter.Added.Year == 2025, null, "added__year=2025");
		Add(filter => filter.Added.Year == 2025, filter => filter.Added, "added__year=2025&ordering=added");

		Add(filter => filter.IsInInbox == true, null, "is_in_inbox=true");
		Add(filter => filter.IsInInbox != true, null, "is_in_inbox=false");

		Add(
			document =>
				document.Added.Date >= DateTime.Now.AddDays(-1) &&
				document.Added.Month == 2 &&
				document.ArchiveSerialNumber != null &&
				document.ArchiveSerialNumber.Value < 10 &&
				document.Checksum.Contains("foo"),
			null,
			$"added__date__gte={DateTime.Now.AddDays(-1):yyyy-MM-dd}&added__month=2&archive_serial_number__isnull=false&archive_serial_number__lt=10&checksum__icontains=foo");
	}
}
