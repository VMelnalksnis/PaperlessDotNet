// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;

namespace VMelnalksnis.PaperlessDotNet.Filters;

internal static class TypeExtensions
{
	internal static bool IsNullableValueType(this Type type) =>
		type.IsConstructedGenericType &&
		type.GetGenericTypeDefinition() == typeof(Nullable<>);
}
