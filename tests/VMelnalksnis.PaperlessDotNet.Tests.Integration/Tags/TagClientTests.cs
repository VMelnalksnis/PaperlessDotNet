// Copyright 2022 Valters Melnalksnis
// Licensed under the Apache License 2.0.
// See LICENSE file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;

using VMelnalksnis.PaperlessDotNet.Correspondents;
using VMelnalksnis.PaperlessDotNet.Tags;

namespace VMelnalksnis.PaperlessDotNet.Tests.Integration.Tags;

public sealed class TagClientTests(PaperlessFixture paperlessFixture) : PaperlessTests(paperlessFixture)
{
	[Test]
	public async Task CreateGetDelete()
	{
		var createdTag = await Client.Tags.Create(new("Foo bar")
		{
			Match = "foo",
			MatchingAlgorithm = MatchingAlgorithm.ExactMatch,
			IsInsensitive = true,
			IsInboxTag = true,
		});

		var tag = (await Client.Tags.Get(createdTag.Id))!;
		var tags = await Client.Tags.GetAll().ToListAsync();
		var paginatedTags = await Client.Tags.GetAll(1).ToListAsync();

		using (new AssertionScope())
		{
			tag.Should().BeEquivalentTo(createdTag, ExcludingDocumentCount);
			tags.Should().ContainSingle(t => t.Id == tag.Id).Which.Should().BeEquivalentTo(tag, ExcludingDocumentCount);
			paginatedTags.Should().BeEquivalentTo(tags);

			createdTag.Name.Should().Be("Foo bar");
			createdTag.Slug.Should().Be("foo-bar");
			createdTag.Match.Should().Be("foo");
			createdTag.MatchingAlgorithm.Should().Be(MatchingAlgorithm.ExactMatch);
			createdTag.IsInsensitive.Should().BeTrue();
			createdTag.IsInboxTag.Should().BeTrue();

			createdTag.DocumentCount.Should().Be(null);
			tag.DocumentCount.Should().Be(0);
		}

		await Client.Tags.Delete(createdTag.Id);
		tags = await Client.Tags.GetAll().ToListAsync();

		tags.Should().NotContainEquivalentOf(createdTag);
	}

	private static EquivalencyAssertionOptions<Tag> ExcludingDocumentCount(EquivalencyAssertionOptions<Tag> options)
	{
		return options.Excluding(tag => tag.DocumentCount);
	}
}
