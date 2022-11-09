// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration.Testcontainer;

internal static class TestcontainersBuilderPaperlessExtensions
{
	internal static ITestcontainersBuilder<TPaperlessTestcontainer> WithPaperless<TPaperlessTestcontainer>(
		this ITestcontainersBuilder<TPaperlessTestcontainer> builder,
		PaperlessTestcontainerConfiguration configuration,
		RedisTestcontainer redis)
		where TPaperlessTestcontainer : PaperlessTestcontainer => builder
		.WithImage(configuration.Image)
		.WithExposedPort(configuration.DefaultPort)
		.WithPortBinding(configuration.Port, configuration.DefaultPort)
		.WithOutputConsumer(configuration.OutputConsumer)
		.WithWaitStrategy(configuration.WaitStrategy)
		.WithEnvironment("PAPERLESS_REDIS", $"redis://{redis.IpAddress}:{redis.ContainerPort}")
		.WithEnvironment("PAPERLESS_ADMIN_USER", configuration.Username)
		.WithEnvironment("PAPERLESS_ADMIN_PASSWORD", configuration.Password)
		.ConfigureContainer(paperless =>
		{
			paperless.Username = configuration.Username;
			paperless.Password = configuration.Password;
			paperless.ContainerPort = configuration.DefaultPort;
		});
}
