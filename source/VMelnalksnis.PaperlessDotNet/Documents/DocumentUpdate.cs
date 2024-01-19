// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

namespace VMelnalksnis.PaperlessDotNet.Documents;

/// <summary>Information needed to update a <see cref="Document"/>.</summary>
public class DocumentUpdate : DocumentUpdateBase;

/// <summary>Information needed to update a <see cref="Document{TFields}"/>.</summary>
/// <typeparam name="TFields">The type containing the custom fields.</typeparam>
#pragma warning disable SA1402
public class DocumentUpdate<TFields> : DocumentUpdate
{
	/// <summary>Gets or sets the custom fields of the document.</summary>
	public TFields? CustomFields { get; set; }
}
#pragma warning restore SA1402
