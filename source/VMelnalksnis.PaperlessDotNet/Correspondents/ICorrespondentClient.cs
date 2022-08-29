// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.PaperlessDotNet.Correspondents;

/// <summary>Paperless API client for working with correspondents.</summary>
public interface ICorrespondentClient
{
	/// <summary>Gets all correspondents.</summary>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A enumerable which will asynchronously iterate over all available pages of correspondents.</returns>
	IAsyncEnumerable<Correspondent> GetAll(CancellationToken cancellationToken = default);

	/// <summary>Gets all correspondents.</summary>
	/// <param name="pageSize">The number of correspondents to get in a single request.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>A enumerable which will asynchronously iterate over all available pages of correspondents.</returns>
	IAsyncEnumerable<Correspondent> GetAll(int pageSize, CancellationToken cancellationToken = default);

	/// <summary>Gets the correspondent with the specified id.</summary>
	/// <param name="id">The id of the correspondent to get.</param>
	/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	/// <returns>The correspondent with the specified id if it exists; otherwise <see langword="null"/>.</returns>
	Task<Correspondent?> Get(int id, CancellationToken cancellationToken = default);
}
