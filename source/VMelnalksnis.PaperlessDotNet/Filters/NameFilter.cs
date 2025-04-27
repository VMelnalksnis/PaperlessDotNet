// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace VMelnalksnis.PaperlessDotNet.Filters;

/// <summary>Filter for related objects by id or name.</summary>
#if NET8_0_OR_GREATER
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
public sealed class NameFilter : IdFilter
{
	/// <summary>Gets the name by which to filter.</summary>
	public string Name { get; } = null!;
}
