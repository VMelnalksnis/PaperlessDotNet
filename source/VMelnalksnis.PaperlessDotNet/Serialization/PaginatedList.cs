// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace VMelnalksnis.PaperlessDotNet.Serialization;

/// <summary>A paginated response wrapper for <see cref="List{T}"/>.</summary>
/// <typeparam name="TResult">The response type.</typeparam>
public sealed class PaginatedList<TResult>
	where TResult : class
{
	/// <summary>Gets or sets the total count of results.</summary>
	public int? Count { get; set; }

	/// <summary>Gets or sets the link to the next page.</summary>
	public Uri? Next { get; set; }

	/// <summary>Gets or sets the link to the previous page.</summary>
	public Uri? Previous { get; set; }

	/// <summary>Gets or sets a collection of all the ids.</summary>
	public List<int>? All { get; set; }

	/// <summary>Gets or sets the results of this page.</summary>
	public List<TResult>? Results { get; set; }
}
