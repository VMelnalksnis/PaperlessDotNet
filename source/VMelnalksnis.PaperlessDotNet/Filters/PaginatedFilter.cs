// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Runtime.Serialization;

namespace VMelnalksnis.PaperlessDotNet.Filters;

/// <summary>Base filter for paginated endpoints.</summary>
public class PaginatedFilter
{
	/// <summary>Gets the page size.</summary>
	[DataMember(Name = "page_size")]
	public int PageSize { get; }
}
