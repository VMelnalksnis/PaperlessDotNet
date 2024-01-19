// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace VMelnalksnis.PaperlessDotNet.Serialization;

internal static class JsonSerializerOptionsExtensions
{
	internal static JsonTypeInfo<T> GetTypeInfo<T>(this JsonSerializerOptions options)
	{
		return (JsonTypeInfo<T>)options.GetTypeInfo(typeof(T));
	}
}
