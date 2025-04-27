// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using JetBrains.Annotations;

#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace VMelnalksnis.PaperlessDotNet.Filters;

/// <summary>Filter for related objects by id.</summary>
[UsedImplicitly(ImplicitUseKindFlags.Access | ImplicitUseKindFlags.Assign, ImplicitUseTargetFlags.Members)]
#if NET8_0_OR_GREATER
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
public class IdFilter
{
	/// <summary>Gets the id by which to filter.</summary>
	public int Id { get; }
}
