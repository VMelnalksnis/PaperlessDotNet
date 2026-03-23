// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

namespace VMelnalksnis.PaperlessDotNet.Correspondents;

/// <summary>Information needed to update a <see cref="Correspondent"/>.</summary>
public sealed class CorrespondentUpdate
{
	/// <inheritdoc cref="Correspondent.Name"/>
	public string? Name { get; set; }

	/// <inheritdoc cref="Correspondent.MatchingPattern"/>
	public string? Match { get; set; }

	/// <inheritdoc cref="Correspondent.MatchingAlgorithm"/>
	public MatchingAlgorithm? MatchingAlgorithm { get; set; }

	/// <inheritdoc cref="Correspondent.IsInsensitive"/>
	public bool? IsInsensitive { get; set; }

	/// <summary>Gets or sets the id of the owner of the correspondent.</summary>
	public int? Owner { get; set; }
}
