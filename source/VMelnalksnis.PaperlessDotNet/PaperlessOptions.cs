// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel.DataAnnotations;

namespace VMelnalksnis.PaperlessDotNet;

/// <summary>Options for configuring <see cref="IPaperlessClient"/>.</summary>
public sealed class PaperlessOptions
{
	/// <summary>The name of the configuration section.</summary>
	public const string Name = "Paperless";

	/// <summary>Gets or sets the base address of the Paperless API.</summary>
	[Required]
	public Uri BaseAddress { get; set; } = null!;

	/// <summary>Gets or sets the authentication token to use with all requests.</summary>
	[Required]
	public string Token { get; set; } = null!;

	/// <summary> Gets or sets the time delay between each polling of tasks in milliseconds.</summary>
	[Required]
	public TimeSpan TaskPollingDelay { get; set; } = TimeSpan.FromMilliseconds(250);
}
