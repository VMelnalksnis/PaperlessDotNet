// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using NodaTime;

namespace VMelnalksnis.PaperlessDotNet.Correspondents;

/// <summary>Someone with whom documents were exchanged with.</summary>
public sealed class Correspondent
{
	/// <summary>Gets or sets the id of the correspondent.</summary>
	public int Id { get; set; }

	/// <summary>Gets or sets the normalized <see cref="Name"/> - lowercased and with whitespace replaced with '-'.</summary>
	public string Slug { get; set; } = null!;

	/// <summary>Gets or sets the name of the correspondent.</summary>
	public string Name { get; set; } = null!;

	/// <summary>Gets or sets the pattern by which to match the correspondent to documents.</summary>
	public string MatchingPattern { get; set; } = null!;

	/// <summary>Gets or sets the id of the matching algorithm used to match the correspondent to documents.</summary>
	public int MatchingAlgorithmId { get; set; }

	/// <summary>Gets or sets a value indicating whether to ignore case when matching the correspondent to documents.</summary>
	public bool CaseInsensitiveMatching { get; set; }

	/// <summary>Gets or sets the number of documents with the correspondent.</summary>
	public int DocumentCount { get; set; }

	/// <summary>Gets or sets the instant when the last document with the correspondent was created.</summary>
	public Instant? LastCorrespondence { get; set; }
}
