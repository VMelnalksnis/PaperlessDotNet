// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>Information needed to create a <see cref="CustomField"/>.</summary>
[DebuggerDisplay("{Name} ({DataType.Name})")]
public class CustomFieldCreation
{
	/// <summary>Initializes a new instance of the <see cref="CustomFieldCreation"/> class.</summary>
	/// <param name="name">The name of the custom field.</param>
	/// <param name="dataType">The type of the custom field.</param>
	public CustomFieldCreation(string name, CustomFieldType dataType)
		: this(name, dataType, null)
	{
		Name = name;
		DataType = dataType;
	}

	/// <summary>Initializes a new instance of the <see cref="CustomFieldCreation"/> class.</summary>
	/// <param name="name">The name of the custom field.</param>
	/// <param name="dataType">The type of the custom field.</param>
	/// <param name="extraData">Extra data about the custom field.</param>
	protected CustomFieldCreation(string name, CustomFieldType dataType, CustomFieldExtraData? extraData)
	{
		if (dataType == CustomFieldType.Select && extraData?.SelectOptions is not { Length: > 0 })
		{
			throw new ArgumentOutOfRangeException(nameof(dataType), dataType, "Must specify select options");
		}

		Name = name;
		DataType = dataType;
		ExtraData = extraData;
	}

	/// <summary>Gets the name of the custom field.</summary>
	public string Name { get; }

	/// <summary>Gets the type of the custom field.</summary>
	public CustomFieldType DataType { get; }

	/// <summary>Gets extra data about the field.</summary>
	public CustomFieldExtraData? ExtraData { get; }
}
