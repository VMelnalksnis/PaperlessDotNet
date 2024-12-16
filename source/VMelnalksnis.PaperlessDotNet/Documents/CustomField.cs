// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>A custom field that can be attached to <see cref="Document"/>.</summary>
[DebuggerDisplay("{Id} - {Name} ({DataType.Name})")]
public sealed class CustomField
{
	/// <summary>Gets or sets the id of the custom field.</summary>
	public int Id { get; set; }

	/// <summary>Gets or sets the name of the custom field.</summary>
	public string Name { get; set; } = null!;

	/// <summary>Gets or sets the type of the custom field.</summary>
	public CustomFieldType DataType { get; set; } = null!;

	/// <summary>Gets or sets extra data about the custom field.</summary>
	public CustomFieldExtraData? ExtraData { get; set; }
}
