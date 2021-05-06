using System.Collections.Generic;
using AspDotNetCoreWithKestrelLesson.Attributes;
using Newtonsoft.Json;

namespace AspDotNetCoreWithKestrelLesson.Models
{
	[GenerateController("api/user")]
	[GenerateExample(103, "TestUser3", "test.user3@server.net")]
	public record User
	(
		int Id,
		string Handle,
		string Email
	)
	{
		[JsonIgnore]
		public ICollection<Post> Posts { set; get; }
		[JsonIgnore]
		public ICollection<Comment> Comments { set; get; }
	}

	[GenerateController("api/post")]
	[GenerateExample(203, 103, "Test post 3")]
	public record Post
	(
		int Id,
		int UserId,
		string Content
	)
	{
		[JsonIgnore]
		public ICollection<Comment> Comments { set; get; }
	}

	[GenerateController("api/comment")]
	[GenerateExample(303, 203, 103, "Test comment 3")]
	public record Comment
	(
		int Id,
		int PostId,
		int UserId,
		string Content
	);

	public partial record PatchOperation<TRequest>
	(
		string Op,
		string From,
		string Path,
		string Value
	);
}