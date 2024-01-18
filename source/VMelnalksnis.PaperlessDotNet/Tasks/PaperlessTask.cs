// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Text.Json.Serialization;

using NodaTime;

using VMelnalksnis.PaperlessDotNet.Documents;

namespace VMelnalksnis.PaperlessDotNet.Tasks;

/// <summary>A long running/background task managed by paperless.</summary>
public sealed class PaperlessTask
{
	/// <summary>Gets or sets the sequential id of the task.</summary>
	public int Id { get; set; }

	/// <summary>Gets or sets the unique id of the task.</summary>
	[JsonPropertyName("task_id")]
	public Guid TaskId { get; set; }

	/// <summary>Gets or sets the name of the file related to this task.</summary>
	[JsonPropertyName("task_file_name")]
	public string? TaskFileName { get; set; }

	/// <summary>Gets or sets the datetime when the task was created.</summary>
	[JsonPropertyName("date_created")]
	public OffsetDateTime DateCreated { get; set; }

	/// <summary>Gets or sets the datetime when the task was completed.</summary>
	[JsonPropertyName("date_done")]
	public OffsetDateTime? DateDone { get; set; }

	/// <summary>Gets or sets the type of the task.</summary>
	public string? Type { get; set; }

	/// <summary>Gets or sets the status of the task.</summary>
	public PaperlessTaskStatus Status { get; set; } = null!;

	/// <summary>Gets or sets detailed message about the result of the task.</summary>
	public string? Result { get; set; }

	/// <summary>Gets or sets a value indicating whether a user has acknowledged the result of the task.</summary>
	public bool Acknowledged { get; set; }

	/// <summary>Gets or sets the id of the <see cref="Document"/> related to this task.</summary>
	[JsonPropertyName("related_document")]
	[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
	public int? RelatedDocument { get; set; }
}
