// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace VMelnalksnis.PaperlessDotNet.Serialization;

internal static class HttpClientExtensions
{
	internal static async IAsyncEnumerable<TResult> GetPaginated<TResult>(
		this HttpClient httpClient,
		string requestUri,
		JsonTypeInfo<PaginatedList<TResult>> typeInfo,
		[EnumeratorCancellation] CancellationToken cancellationToken)
		where TResult : class
	{
		var next = requestUri;
		while (next is not null && !cancellationToken.IsCancellationRequested)
		{
			var paginatedList = await httpClient.GetFromJsonAsync(next, typeInfo, cancellationToken).ConfigureAwait(false);
			if (paginatedList?.Results is null)
			{
				yield break;
			}

			foreach (var result in paginatedList.Results)
			{
				yield return result;
			}

			next = paginatedList.Next?.PathAndQuery;
		}
	}

	internal static async Task<TResponse> PostAsJsonAsync<TRequest, TResponse>(
		this HttpClient httpClient,
		string requestUri,
		TRequest request,
		JsonTypeInfo<TRequest> requestTypeInfo,
		JsonTypeInfo<TResponse> responseTypeInfo)
	{
		using var response = await httpClient
			.PostAsJsonAsync(requestUri, request,  requestTypeInfo)
			.ConfigureAwait(false);

		await response.EnsureSuccessStatusCodeAsync().ConfigureAwait(false);
		return (await response.Content.ReadFromJsonAsync(responseTypeInfo).ConfigureAwait(false))!;
	}

	internal static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(
		this HttpClient httpClient,
		string requestUri,
		TValue value,
		JsonTypeInfo<TValue> typeInfo)
	{
		// PostAsJsonAsync sends chunked data, and does not set Content-Length;
		// Paperless interprets missing Content-Length as 0, and thus ignores any content
		// https://github.com/aspnet/AspNetWebStack/issues/252
		var json = JsonSerializer.Serialize(value, typeInfo);
		var content = new StringContent(json, Encoding.UTF8, "application/json");
		return httpClient.PostAsync(requestUri, content);
	}

	internal static Task<HttpResponseMessage> PatchAsJsonAsync<TValue>(
		this HttpClient httpClient,
		string requestUri,
		TValue value,
		JsonTypeInfo<TValue> typeInfo)
	{
		var json = JsonSerializer.Serialize(value, typeInfo);
		var content = new StringContent(json, Encoding.UTF8, "application/json");
#if NETSTANDARD2_0
		var message = new HttpRequestMessage(new("PATCH"), requestUri)
		{
			Content = content,
		};
		return httpClient.SendAsync(message);
#else
		return httpClient.PatchAsync(requestUri, content);
#endif
	}

	internal static async Task EnsureSuccessStatusCodeAsync(this HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode)
		{
			return;
		}

		var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
		throw new HttpRequestException(message);
	}
}
