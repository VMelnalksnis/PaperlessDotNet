// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Text.Json.Serialization;

using Ardalis.SmartEnum.SystemTextJson;

using NodaTime;

namespace VMelnalksnis.PaperlessDotNet.Tests.Serialization;

internal sealed class TestCustomFields
{
	public string? Field1 { get; set; }

	public Uri? Field2 { get; set; }

	public LocalDate? Field3 { get; set; }

	public bool? Field4 { get; set; }

	public int? Field5 { get; set; }

	public float? Field6 { get; set; }

	public float? Field7 { get; set; }

	public int[]? Field8 { get; set; }

	[JsonConverter(typeof(SmartEnumValueConverter<SelectOptions, int>))]
	public SelectOptions? Field9 { get; set; }
}
