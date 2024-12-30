// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using VMelnalksnis.PaperlessDotNet.Correspondents;

namespace VMelnalksnis.PaperlessDotNet.StoragePaths;

/// <summary>Information needed to create a new <see cref="StoragePath"/>.</summary>
public sealed class StoragePathCreation
{
	/// <summary>Initializes a new instance of the <see cref="StoragePathCreation"/> class.</summary>
	/// <param name="name">The name of the document type.</param>
	public StoragePathCreation(string name)
	{
		Name = name;
	}

	/// <inheritdoc cref="StoragePath.Slug"/>
	public string? Slug { get; set; }

	/// <inheritdoc cref="StoragePath.Name"/>
	public string Name { get; set; }

	/// <inheritdoc cref="StoragePath.Path"/>
	public string Path { get; set; }

	/// <inheritdoc cref="StoragePath.MatchingPattern"/>
	public string? Match { get; set; }

	/// <inheritdoc cref="StoragePath.MatchingAlgorithm"/>
	public MatchingAlgorithm? MatchingAlgorithm { get; set; }

	/// <inheritdoc cref="StoragePath.IsInsensitive"/>
	public bool? IsInsensitive { get; set; }
}
