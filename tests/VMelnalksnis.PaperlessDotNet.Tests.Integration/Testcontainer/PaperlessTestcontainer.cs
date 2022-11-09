// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Net.Http;

using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

using Microsoft.Extensions.Logging;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration.Testcontainer;

internal class PaperlessTestcontainer : HostedServiceContainer
{
	internal PaperlessTestcontainer(ITestcontainersConfiguration configuration, ILogger logger)
		: base(configuration, logger)
	{
	}

	internal FormUrlEncodedContent Login => new(new List<KeyValuePair<string?, string?>>
	{
		new("username", Username),
		new("password", Password),
	});
}
