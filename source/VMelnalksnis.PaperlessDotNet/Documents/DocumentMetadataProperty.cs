// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Text.Json.Serialization;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>A namespaced property of document metadata.</summary>
public sealed class DocumentMetadataProperty
{
	/// <summary>Initializes a new instance of the <see cref="DocumentMetadataProperty"/> class.</summary>
	/// <param name="key">The name of the property.</param>
	/// <param name="value">The value of the property.</param>
	[JsonConstructor]
	public DocumentMetadataProperty(string key, string value)
	{
		Key = key;
		Value = value;
	}

	/// <summary>Gets the name of the metadata property.</summary>
	public string Key { get; }

	/// <summary>Gets the value of the metadata property..</summary>
	public string Value { get; }

	/// <summary>Gets the XML namespace Uri of the metadata property.</summary>
	public Uri? Namespace { get; init; }

	/// <summary>Gets the prefix of <see cref="Namespace"/>.</summary>
	public string? Prefix { get; init; }
}
