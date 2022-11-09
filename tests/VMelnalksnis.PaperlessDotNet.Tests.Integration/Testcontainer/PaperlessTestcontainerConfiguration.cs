// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.IO;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration.Testcontainer;

internal sealed class PaperlessTestcontainerConfiguration : HostedServiceConfiguration
{
	private const string _redisImage = "ghcr.io/paperless-ngx/paperless-ngx:1.9";
	private const int _paperlessPort = 8000;

	internal PaperlessTestcontainerConfiguration()
		: base(_redisImage, _paperlessPort)
	{
	}

	/// <inheritdoc />
	public override string Username { get; set; } = "admin";

	/// <inheritdoc />
	public override string Password { get; set; } = Guid.NewGuid().ToString("N");

	/// <inheritdoc />
	public override IOutputConsumer OutputConsumer => Consume
		.RedirectStdoutAndStderrToStream(new MemoryStream(), new MemoryStream());

	/// <inheritdoc />
	public override IWaitForContainerOS WaitStrategy => Wait
		.ForUnixContainer()
		.UntilPortIsAvailable(DefaultPort);
}
