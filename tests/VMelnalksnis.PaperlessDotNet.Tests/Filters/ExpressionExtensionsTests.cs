// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;

using VMelnalksnis.PaperlessDotNet.Documents;
using VMelnalksnis.PaperlessDotNet.Filters;

namespace VMelnalksnis.PaperlessDotNet.Tests.Filters;

public class ExpressionExtensionsTests
{
	[Theory]
	[ClassData(typeof(FilterExpressionTestCases))]
	public void GetQueryString(
		Expression<Func<DocumentFilter, bool>> filter,
		Expression<Func<Document, object>>? ordering,
		string query)
	{
		filter.GetQueryString(ordering).Should().Be(query);
	}
}
