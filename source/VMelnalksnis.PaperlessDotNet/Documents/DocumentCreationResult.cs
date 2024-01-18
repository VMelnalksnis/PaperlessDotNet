// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

namespace VMelnalksnis.PaperlessDotNet.Documents;
#pragma warning disable SA1402

/// <summary>Base class for possible results of creating a new document.</summary>
/// <seealso cref="ImportStarted"/>
/// <seealso cref="DocumentCreated"/>
/// <seealso cref="ImportFailed"/>
public abstract class DocumentCreationResult;

/// <summary>Document was successfully created.</summary>
/// <param name="id">The id of the created document.</param>
public sealed class DocumentCreated(int id) : DocumentCreationResult
{
	/// <summary>Gets the id of the created document.</summary>
	public int Id { get; } = id;
}

/// <summary>Document was successfully submitted and import was started.</summary>
/// <remarks>This is only returned for version below or equal to 1.9.2.</remarks>
public sealed class ImportStarted : DocumentCreationResult;

/// <summary>Document import process failed.</summary>
/// <param name="result">The result message returned by paperless.</param>
public sealed class ImportFailed(string? result) : DocumentCreationResult
{
	/// <summary>Gets the result message returned by paperless.</summary>
	public string? Result { get; } = result;
}
