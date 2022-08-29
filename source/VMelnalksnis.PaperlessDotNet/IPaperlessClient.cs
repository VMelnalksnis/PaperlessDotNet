// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using VMelnalksnis.PaperlessDotNet.Correspondents;
using VMelnalksnis.PaperlessDotNet.Documents;

namespace VMelnalksnis.PaperlessDotNet;

/// <summary>All available Paperless APIs.</summary>
public interface IPaperlessClient
{
	/// <summary>Gets the documents API client.</summary>
	ICorrespondentClient Correspondents { get; }

	/// <summary>Gets the documents API client.</summary>
	IDocumentClient Documents { get; }
}
