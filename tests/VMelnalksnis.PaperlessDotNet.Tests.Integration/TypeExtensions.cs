// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Resources;
using System.Threading.Tasks;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration;

internal static class TypeExtensions
{
	internal static Stream GetResource(this Type type, string resourceName)
	{
		var stream = type.Assembly.GetManifestResourceStream(type, resourceName);
		if (stream is not null)
		{
			return stream;
		}

		throw new MissingManifestResourceException($"Could not find {resourceName} is namespace {type.Namespace}");
	}

	internal static async Task<string> ReadResource(this Type type, string resourceName)
	{
		await using var stream = type.GetResource(resourceName);
		using var streamReader = new StreamReader(stream);
		return await streamReader.ReadToEndAsync();
	}
}
