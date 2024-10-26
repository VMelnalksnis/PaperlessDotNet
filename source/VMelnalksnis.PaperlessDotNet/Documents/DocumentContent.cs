// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net.Http.Headers;

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>
/// The downloaded content of a document.
/// </summary>
public sealed class DocumentContent : IDisposable
{
	/// <summary>Initializes a new instance of the <see cref="DocumentContent"/> class.</summary>
	/// <param name="content">The document content.</param>
	/// <param name="contentDisposition">The ContentDisposition header of the download.</param>
	/// <param name="mediaTypeHeaderValue">The MediaType header of the download.</param>
	public DocumentContent(Stream content, ContentDispositionHeaderValue? contentDisposition, MediaTypeHeaderValue mediaTypeHeaderValue)
	{
		Content = content;
		ContentDisposition = contentDisposition;
		MediaTypeHeaderValue = mediaTypeHeaderValue;
	}

	/// <summary>
	/// Gets the document content.
	/// </summary>
	public Stream Content { get; }

	/// <summary>
	/// Gets the ContentDisposition header of the download.
	/// </summary>
	public ContentDispositionHeaderValue? ContentDisposition { get; }

	/// <summary>
	/// Gets the MediaType header of the download.
	/// </summary>
	public MediaTypeHeaderValue MediaTypeHeaderValue { get; }

	/// <inheritdoc />
	public void Dispose() => Content.Dispose();
}
