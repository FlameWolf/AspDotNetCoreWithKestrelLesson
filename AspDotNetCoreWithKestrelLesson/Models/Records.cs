using AspDotNetCoreWithKestrelLesson.Attributes;

namespace AspDotNetCoreWithKestrelLesson.Models
{
	[GenerateController("api/user")]
	public record User
	(
		uint Id,
		string Handle,
		string Email
	);

	[GenerateController("api/post")]
	public record Post
	(
		uint Id,
		uint UserId,
		string Content
	);

	[GenerateController("api/comment")]
	public record Comment
	(
		uint Id,
		uint PostId,
		uint UserId,
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