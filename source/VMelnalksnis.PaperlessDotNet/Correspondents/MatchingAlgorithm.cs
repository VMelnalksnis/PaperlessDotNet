// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using Ardalis.SmartEnum;

using JetBrains.Annotations;

namespace VMelnalksnis.PaperlessDotNet.Correspondents;

/// <summary>Algorithms for matching a <see cref="Correspondent"/> to a <see cref="Documents.Document"/>.</summary>
[UsedImplicitly(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.Members)]
public sealed class MatchingAlgorithm : SmartEnum<MatchingAlgorithm>
{
	/// <summary>Document contains any of these words (space separated).</summary>
	public static readonly MatchingAlgorithm AnyWord = new("Any word", 1);

	/// <summary>Document contains all of these words (space separated).</summary>
	public static readonly MatchingAlgorithm AllWords = new("All words", 2);

	/// <summary>Document contains this string.</summary>
	public static readonly MatchingAlgorithm ExactMatch = new("Exact match", 3);

	/// <summary>Document matches this regular expression.</summary>
	public static readonly MatchingAlgorithm RegularExpression = new("Regular expression", 4);

	/// <summary>Document contains a word similar to this word.</summary>
	public static readonly MatchingAlgorithm FuzzyWord = new("Fuzzy Word", 5);

	/// <summary>Learn matching automatically.</summary>
	public static readonly MatchingAlgorithm Automatic = new("Automatic", 6);

	private MatchingAlgorithm(string name, int value)
		: base(name, value)
	{
	}
}
