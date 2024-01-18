// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.PaperlessDotNet.Tasks;

/// <summary>Paperless API client for working with tasks.</summary>
public interface ITaskClient
{
	/// <summary>Gets all tasks.</summary>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A collection of all tasks.</returns>
	Task<List<PaperlessTask>> GetAll(CancellationToken cancellationToken = default);

	/// <summary>Gets the task with the specific id.</summary>
	/// <param name="taskId">The id of the task.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The task with the specified id if it exists; otherwise <c>null</c>.</returns>
	Task<PaperlessTask?> Get(Guid taskId, CancellationToken cancellationToken = default);
}
