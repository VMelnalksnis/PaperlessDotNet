// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>Information needed to create a <see cref="CustomField"/>.</summary>
/// <param name="name">The name of the custom field.</param>
/// <param name="dataType">The type of the custom field.</param>
[DebuggerDisplay("{Name} ({DataType.Name})")]
public sealed class CustomFieldCreation(string name, CustomFieldType dataType)
{
	/// <summary>Gets the name of the custom field.</summary>
	public string Name { get; } = name;

	/// <summary>Gets the type of the custom field.</summary>
	public CustomFieldType DataType { get; } = dataType;
}
