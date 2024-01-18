// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using Ardalis.SmartEnum;

namespace VMelnalksnis.PaperlessDotNet.Tasks;

/// <summary>Paperless background task status.</summary>
public sealed class PaperlessTaskStatus : SmartEnum<PaperlessTaskStatus>
{
	/// <summary>Task is queued and has not been started yet.</summary>
	public static readonly PaperlessTaskStatus Pending = new("PENDING", 1);

	/// <summary>Task has been started and is currently running.</summary>
	public static readonly PaperlessTaskStatus Started = new("STARTED", 2);

	/// <summary>Task completed successfully.</summary>
	public static readonly PaperlessTaskStatus Success = new("SUCCESS", 3);

	/// <summary>Task completed unsuccessfully.</summary>
	public static readonly PaperlessTaskStatus Failure = new("FAILURE", 4);

	private PaperlessTaskStatus(string name, int value)
		: base(name, value)
	{
	}

	/// <summary>Gets a value indicating whether this status represents a completed task.</summary>
	public bool IsCompleted => Equals(Success) || Equals(Failure);
}
