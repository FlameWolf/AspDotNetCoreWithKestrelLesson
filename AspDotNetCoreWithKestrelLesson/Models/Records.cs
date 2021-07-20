using System;
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

	public partial record PatchOperation<T>
	(
		string Op,
		string Path,
		string From,
		object Value
	);

	public record LogEntry
	{
		public int Id { set; get; }
		public string Scheme { set; get; }
		public string Host { set; get; }
		public string User { set; get; }
		public string Claims { set; get; }
		public string Path { set; get; }
		public string QueryString { set; get; }
		public string Request { set; get; }
		public string Response { set; get; }
		public DateTime StartedAt { set; get; }
		public DateTime EndedAt { set; get; }
	}
}