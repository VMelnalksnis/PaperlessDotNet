// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using VMelnalksnis.PaperlessDotNet.Documents;

namespace VMelnalksnis.PaperlessDotNet;

/// <inheritdoc />
public sealed class PaperlessClient : IPaperlessClient
{
	/// <summary>Initializes a new instance of the <see cref="PaperlessClient"/> class.</summary>
	/// <param name="documents">Documents API client.</param>
	public PaperlessClient(IDocumentClient documents)
	{
		Documents = documents;
	}

	/// <inheritdoc />
	public IDocumentClient Documents { get; }
}
