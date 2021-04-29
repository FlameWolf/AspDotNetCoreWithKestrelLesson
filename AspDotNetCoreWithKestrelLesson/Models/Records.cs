using AspDotNetCoreWithKestrelLesson.Attributes;

namespace AspDotNetCoreWithKestrelLesson.Models
{
	[GenerateController("api/user")]
	[GenerateExample(101, "TestUser", "test.user@server.net")]
	public record User
	(
		int Id,
		string Handle,
		string Email
	);

	[GenerateController("api/post")]
	[GenerateExample(201, 101, "Test post")]
	public record Post
	(
		int Id,
		int UserId,
		string Content
	);

	[GenerateController("api/comment")]
	[GenerateExample(301, 201, 101, "Test comment")]
	public record Comment
	(
		int Id,
		int PostId,
		int UserId,
		string Content
	);

	public partial record PatchRequest<TRequest>
	(
		string Op,
		string From,
		string Path,
		string Value
	);
}