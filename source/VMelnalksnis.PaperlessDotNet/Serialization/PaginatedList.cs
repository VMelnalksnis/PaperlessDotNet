// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace VMelnalksnis.PaperlessDotNet.Serialization;

internal sealed class PaginatedList<TResult>
	where TResult : class
{
	public int? Count { get; set; }

	public Uri? Next { get; set; }

	public Uri? Previous { get; set; }

	public List<TResult>? Results { get; set; }
}
