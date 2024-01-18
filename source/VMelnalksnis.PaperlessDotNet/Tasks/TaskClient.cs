// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using VMelnalksnis.PaperlessDotNet.Serialization;

namespace VMelnalksnis.PaperlessDotNet.Tasks;

/// <inheritdoc />
public sealed class TaskClient : ITaskClient
{
	private readonly HttpClient _httpClient;
	private readonly PaperlessJsonSerializerContext _context;

	/// <summary>Initializes a new instance of the <see cref="TaskClient"/> class.</summary>
	/// <param name="httpClient">Http client configured for making requests to the Paperless API.</param>
	/// <param name="serializerOptions">Paperless specific instance of <see cref="JsonSerializerOptions"/>.</param>
	public TaskClient(HttpClient httpClient, PaperlessJsonSerializerOptions serializerOptions)
	{
		_httpClient = httpClient;
		_context = serializerOptions.Context;
	}

	/// <inheritdoc />
	public Task<List<PaperlessTask>> GetAll(CancellationToken cancellationToken = default)
	{
		return _httpClient.GetFromJsonAsync("/api/tasks/", _context.ListPaperlessTask, cancellationToken)!;
	}

	/// <inheritdoc />
	public async Task<PaperlessTask?> Get(Guid taskId, CancellationToken cancellationToken = default)
	{
		var tasks = await _httpClient
			.GetFromJsonAsync($"/api/tasks/?task_id={taskId}", _context.ListPaperlessTask, cancellationToken)
			.ConfigureAwait(false);

		return tasks?.SingleOrDefault();
	}
}
