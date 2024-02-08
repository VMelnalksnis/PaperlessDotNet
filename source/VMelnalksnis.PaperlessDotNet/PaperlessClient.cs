// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using VMelnalksnis.PaperlessDotNet.Correspondents;
using VMelnalksnis.PaperlessDotNet.Documents;
using VMelnalksnis.PaperlessDotNet.Tags;

namespace VMelnalksnis.PaperlessDotNet;

/// <inheritdoc />
public sealed class PaperlessClient : IPaperlessClient
{
	/// <summary>Initializes a new instance of the <see cref="PaperlessClient"/> class.</summary>
	/// <param name="correspondents">Correspondents API client.</param>
	/// <param name="documents">Documents API client.</param>
	/// <param name="tags">Tags API client.</param>
	public PaperlessClient(ICorrespondentClient correspondents, IDocumentClient documents, ITagClient tags)
	{
		Correspondents = correspondents;
		Documents = documents;
		Tags = tags;
	}

	/// <inheritdoc />
	public ICorrespondentClient Correspondents { get; }

	/// <inheritdoc />
	public IDocumentClient Documents { get; }

	/// <inheritdoc />
	public ITagClient Tags { get; }
}
