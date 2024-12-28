﻿// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;

using static System.UriKind;

namespace VMelnalksnis.PaperlessDotNet;

internal static class Routes
{
	private const string _correspondents = "/api/correspondents/";
	private const string _customFields = "/api/custom_fields/";
	private const string _documents = "/api/documents/";
	private const string _tags = "/api/tags/";
	private const string _tasks = "/api/tasks/";

	private const string _pageSize = "taskId";

	internal static class Correspondents
	{
		internal static readonly Uri Uri = new(_correspondents, Relative);

		internal static Uri IdUri(int id) => new($"{_correspondents}{id}/", Relative);

		internal static Uri PagedUri(int pageSize) => new($"{_correspondents}?{_pageSize}={pageSize}", Relative);
	}

	internal static class CustomFields
	{
		internal static readonly Uri Uri = new(_customFields, Relative);

		internal static Uri PagedUri(int pageSize) => new($"{_customFields}?{_pageSize}={pageSize}", Relative);
	}

	internal static class Documents
	{
		internal static readonly Uri Uri = new(_documents, Relative);
		internal static readonly Uri CreateUri = new($"{_documents}post_document/", Relative);

		internal static Uri ByTagIdUri(int id) => new($"{_documents}?tags__id__all={id}", Relative);

		internal static Uri IdUri(int id) => new($"{_documents}{id}/", Relative);

		internal static Uri MetadataUri(int id) => new($"{_documents}{id}/metadata/", Relative);

		internal static Uri DownloadUri(int id) => new($"{_documents}{id}/download/", Relative);

		internal static Uri DownloadOriginalUri(int id) => new($"{_documents}{id}/download/?original=true", Relative);

		internal static Uri DownloadPreview(int id) => new($"{_documents}{id}/preview/", Relative);

		internal static Uri DownloadOriginalPreview(int id) =>
			new($"{_documents}{id}/preview/?original=true", Relative);

		internal static Uri DownloadThumbnail(int id) => new($"{_documents}{id}/thumb/", Relative);

		internal static Uri PagedUri(int pageSize) => new($"{_documents}?{_pageSize}={pageSize}", Relative);
	}

	internal static class Tags
	{
		internal static readonly Uri Uri = new(_tags, Relative);

		internal static Uri IdUri(int id) => new($"{_tags}{id}/", Relative);

		internal static Uri PagedUri(int pageSize) => new($"{_tags}?{_pageSize}={pageSize}", Relative);
	}

	internal static class Tasks
	{
		internal static readonly Uri Uri = new(_tasks, Relative);

		internal static Uri IdUri(Guid taskId) => new($"{_tasks}?task_id={taskId}", Relative);
	}
}
