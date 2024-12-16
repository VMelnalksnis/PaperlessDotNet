// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

using VMelnalksnis.PaperlessDotNet.Documents;
using VMelnalksnis.PaperlessDotNet.Serialization;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration.Documents;

/// <inheritdoc cref="System.Text.Json.Serialization.JsonSerializerContext" />
[JsonSerializable(typeof(PaginatedList<Document<CustomFields>>))]
[JsonSerializable(typeof(DocumentUpdate<CustomFields>))]
[JsonSerializable(typeof(SelectCustomFieldCreation<SelectOptions>))]
internal sealed partial class SerializerContext : JsonSerializerContext;
