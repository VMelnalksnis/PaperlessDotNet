// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;

namespace VMelnalksnis.PaperlessDotNet.DependencyInjection;

/// <inheritdoc />
[OptionsValidator]
public sealed partial class PaperlessOptionsValidator : IValidateOptions<PaperlessOptions>;
