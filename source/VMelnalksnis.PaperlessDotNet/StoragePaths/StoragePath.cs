// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using NodaTime;

using VMelnalksnis.PaperlessDotNet.Correspondents;

namespace VMelnalksnis.PaperlessDotNet.StoragePaths;

/// <summary>A type of document with whom documents were exchanged with.</summary>
public sealed class StoragePath
{
	/// <summary>Gets or sets the id of the storage path.</summary>
	public int Id { get; set; }

	/// <summary>Gets or sets the normalized <see cref="Name"/> - lowercased and with whitespace replaced with '-'.</summary>
	public string Slug { get; set; } = null!;

	/// <summary>Gets or sets the name of the storage path.</summary>
	public string Name { get; set; } = null!;

	/// <summary>Gets or sets the path of the storage path.</summary>
	public string Path { get; set; } = null!;

	/// <summary>Gets or sets the pattern by which to match the storage path to documents.</summary>
	public string MatchingPattern { get; set; } = null!;

	/// <summary>Gets or sets the id of the matching algorithm used to match the storage path to documents.</summary>
	public MatchingAlgorithm MatchingAlgorithm { get; set; } = null!;

	/// <summary>Gets or sets a value indicating whether to ignore case when matching the storage path to documents.</summary>
	public bool IsInsensitive { get; set; }

	/// <summary>Gets or sets the number of documents with the storage path.</summary>
	public int DocumentCount { get; set; }
}
