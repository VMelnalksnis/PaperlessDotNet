// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>Extra data about a <see cref="CustomField"/>.</summary>
public sealed class CustomFieldExtraData
{
	/// <summary>Gets or sets the option names for custom fields of type <see cref="CustomFieldType.Select"/>.</summary>
	public string?[] SelectOptions { get; set; } = null!;

	/// <summary>Gets or sets the default currency for custom fields of type <see cref="CustomFieldType.Monetary"/>.</summary>
	public string? DefaultCurrency { get; set; }
}
