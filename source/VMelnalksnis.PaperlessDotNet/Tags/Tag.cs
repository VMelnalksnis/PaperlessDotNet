// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using VMelnalksnis.PaperlessDotNet.Correspondents;

namespace VMelnalksnis.PaperlessDotNet.Tags;

/// <summary>A label that can be assigned to a <see cref="Documents.Document"/>.</summary>
public sealed class Tag
{
	/// <summary>Gets or sets the id of the tag.</summary>
	public int Id { get; set; }

	/// <summary>Gets or sets the name of the tag.</summary>
	public string Name { get; set; } = null!;

	/// <summary>Gets or sets the normalized <see cref="Name"/> - lowercased and with whitespace replaced with '-'.</summary>
	public string Slug { get; set; } = null!;

	/// <summary>Gets or sets the id of the colour of the tag.</summary>
	public int? Colour { get; set; }

	/// <summary>Gets or sets the pattern by which to match the tag to documents.</summary>
	public string? Match { get; set; }

	/// <summary>Gets or sets the id of the matching algorithm used to match the tag to documents.</summary>
	public MatchingAlgorithm? MatchingAlgorithm { get; set; }

	/// <summary>Gets or sets a value indicating whether to ignore case when matching the tag to documents.</summary>
	public bool? IsInsensitive { get; set; }

	/// <summary>Gets or sets a value indicating whether all newly consumed documents will be tagged with this tag.</summary>
	public bool? IsInboxTag { get; set; }

	/// <summary>Gets or sets the number of documents with the tag.</summary>
	public int? DocumentCount { get; set; }

	/// <summary>Gets or sets the id of the owner of the tag.</summary>
	public int? Owner { get; set; }
}
