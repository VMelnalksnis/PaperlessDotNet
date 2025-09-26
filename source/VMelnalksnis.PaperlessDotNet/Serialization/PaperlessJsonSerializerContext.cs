﻿// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using VMelnalksnis.PaperlessDotNet.Correspondents;
using VMelnalksnis.PaperlessDotNet.Documents;
using VMelnalksnis.PaperlessDotNet.DocumentTypes;
using VMelnalksnis.PaperlessDotNet.StoragePaths;
using VMelnalksnis.PaperlessDotNet.Tags;
using VMelnalksnis.PaperlessDotNet.Tasks;

namespace VMelnalksnis.PaperlessDotNet.Serialization;

/// <inheritdoc cref="JsonSerializerContext" />
[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(PaginatedList<Correspondent>))]
[JsonSerializable(typeof(Correspondent))]
[JsonSerializable(typeof(CorrespondentCreation))]
[JsonSerializable(typeof(PaginatedList<Document>))]
[JsonSerializable(typeof(Document))]
[JsonSerializable(typeof(DocumentType))]
[JsonSerializable(typeof(StoragePath))]
[JsonSerializable(typeof(List<PaperlessTask>))]
[JsonSerializable(typeof(CustomFieldCreation))]
[JsonSerializable(typeof(List<CustomField>))]
[JsonSerializable(typeof(DocumentUpdate))]
[JsonSerializable(typeof(Dictionary<string, JsonNode>))]
[JsonSerializable(typeof(List<CustomFieldValue>))]
[JsonSerializable(typeof(PaginatedList<CustomField>))]
[JsonSerializable(typeof(PaginatedList<Tag>))]
[JsonSerializable(typeof(TagCreation))]
[JsonSerializable(typeof(DocumentMetadata))]
internal sealed partial class PaperlessJsonSerializerContext : JsonSerializerContext;
