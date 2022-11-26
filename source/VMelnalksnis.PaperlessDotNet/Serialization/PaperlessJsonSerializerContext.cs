// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

using VMelnalksnis.PaperlessDotNet.Correspondents;
using VMelnalksnis.PaperlessDotNet.Documents;

namespace VMelnalksnis.PaperlessDotNet.Serialization;

/// <inheritdoc cref="JsonSerializerContext" />
[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(PaginatedList<Correspondent>))]
[JsonSerializable(typeof(Correspondent))]
[JsonSerializable(typeof(CorrespondentCreation))]
[JsonSerializable(typeof(PaginatedList<Document>))]
[JsonSerializable(typeof(Document))]
internal partial class PaperlessJsonSerializerContext : JsonSerializerContext
{
}
