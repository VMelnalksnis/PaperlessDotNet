// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using VMelnalksnis.PaperlessDotNet.Correspondents;

namespace VMelnalksnis.PaperlessDotNet.Tags;

/// <summary>Information needed to create a <see cref="Tag"/>.</summary>
public sealed class TagCreation
{
	/// <summary>Initializes a new instance of the <see cref="TagCreation"/> class.</summary>
	/// <param name="name">The name of the tag.</param>
	public TagCreation(string name)
	{
		Name = name;
	}

	/// <inheritdoc cref="Tag.Name"/>
	public string Name { get; set; }

	/// <inheritdoc cref="Tag.Colour"/>
	public int? Colour { get; set; }

	/// <inheritdoc cref="Tag.Match"/>
	public string? Match { get; set; }

	/// <inheritdoc cref="Tag.MatchingAlgorithm"/>
	public MatchingAlgorithm? MatchingAlgorithm { get; set; }

	/// <inheritdoc cref="Tag.IsInsensitive"/>
	public bool? IsInsensitive { get; set; }

	/// <inheritdoc cref="Tag.IsInboxTag"/>
	public bool? IsInboxTag { get; set; }

	/// <inheritdoc cref="Tag.Owner"/>
	public int? Owner { get; set; }
}
