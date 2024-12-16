// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;

using Ardalis.SmartEnum;

using NodaTime;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>Custom field data types supported by paperless.</summary>
public sealed class CustomFieldType : SmartEnum<CustomFieldType>
{
	/// <summary>Any text.</summary>
	/// <seealso cref="string"/>
	public static readonly CustomFieldType String = new("string", 1);

	/// <summary>A valid URL.</summary>
	/// <seealso cref="Uri"/>
	public static readonly CustomFieldType Url = new("url", 2);

	/// <summary>Date.</summary>
	/// <seealso cref="LocalDate"/>
	public static readonly CustomFieldType Date = new("date", 3);

	/// <summary>true / false (check / unchecked) field.</summary>
	/// <seealso cref="bool"/>
	public static readonly CustomFieldType Boolean = new("boolean", 4);

	/// <summary>Integer number.</summary>
	/// <example>12.</example>
	/// <seealso cref="int"/>
	public static readonly CustomFieldType Integer = new("integer", 5);

	/// <summary>Float number.</summary>
	/// <example>12.3456.</example>
	/// <seealso cref="float"/>
	public static readonly CustomFieldType Float = new("float", 6);

	/// <summary>Float number with exactly two decimals.</summary>
	/// <example>12.30.</example>
	/// <seealso cref="float"/>
	public static readonly CustomFieldType Monetary = new("monetary", 7);

	/// <summary>Reference(s) to other document(s) displayed as links, automatically creates a symmetrical link in reverse.</summary>
	/// <seealso cref="Array"/>
	/// <seealso cref="int"/>
	public static readonly CustomFieldType DocumentLink = new("documentlink", 8);

	/// <summary>A pre-defined list of strings from which the user can choose.</summary>
	public static readonly CustomFieldType Select = new("select", 9);

	private CustomFieldType(string name, int value)
		: base(name, value)
	{
	}
}
