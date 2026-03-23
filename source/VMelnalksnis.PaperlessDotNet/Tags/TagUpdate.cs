// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using VMelnalksnis.PaperlessDotNet.Correspondents;

namespace VMelnalksnis.PaperlessDotNet.Tags;

/// <summary>Information needed to update a <see cref="Tag"/>.</summary>
public sealed class TagUpdate
{
	/// <inheritdoc cref="Tag.Name"/>
	public string? Name { get; set; }

	/// <summary>Gets or sets the color of the tag in hex format (e.g. <c>#ff0000</c>). API v2 only.</summary>
	public string? Color { get; set; }

	/// <inheritdoc cref="Tag.Match"/>
	public string? Match { get; set; }

	/// <inheritdoc cref="Tag.MatchingAlgorithm"/>
	public MatchingAlgorithm? MatchingAlgorithm { get; set; }

	/// <inheritdoc cref="Tag.IsInsensitive"/>
	public bool? IsInsensitive { get; set; }

	/// <inheritdoc cref="Tag.IsInboxTag"/>
	public bool? IsInboxTag { get; set; }

	/// <summary>Gets or sets the id of the parent tag.</summary>
	public int? Parent { get; set; }

	/// <inheritdoc cref="Tag.Owner"/>
	public int? Owner { get; set; }
}
