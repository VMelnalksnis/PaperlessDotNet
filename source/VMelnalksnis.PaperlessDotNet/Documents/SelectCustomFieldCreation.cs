// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;

using Ardalis.SmartEnum;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>Information needed to create a <see cref="CustomField"/> with type <see cref="CustomFieldType.Select"/>.</summary>
/// <typeparam name="TEnum">The enum type.</typeparam>
public class SelectCustomFieldCreation<TEnum> : CustomFieldCreation
	where TEnum : SmartEnum<TEnum, int>
{
	private static readonly string[] Names = SmartEnum<TEnum, int>
		.List
		.Select(smartEnum => smartEnum.Name)
		.ToArray();

	/// <summary>Initializes a new instance of the <see cref="SelectCustomFieldCreation{TEnum}"/> class.</summary>
	/// <param name="name">The name of the custom field.</param>
	public SelectCustomFieldCreation(string name)
		: base(name, CustomFieldType.Select, new() { SelectOptions = Names })
	{
	}
}
