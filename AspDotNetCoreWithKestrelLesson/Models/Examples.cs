using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AspDotNetCoreWithKestrelLesson.Models
{
	public class UserExample : IExamplesProvider<User>
	{
		public User GetExamples() => new User(101, "TestUser", "test.user@server.net");
	}

	public class PostExample : IExamplesProvider<Post>
	{
		public Post GetExamples() => new Post(201, 101, "Test post");
	}

	public class CommentExample : IExamplesProvider<Comment>
	{
		public Comment GetExamples() => new Comment(301, 201, 101, "Test comment");
	}

	public class PatchRequestExample<T> : IExamplesProvider<PatchRequest<T>>
	{
		public PatchRequest<T> GetExamples()
		{
			return new PatchRequest<T>
			(
				(
					Activator.CreateInstance
					(
						Assembly
							.GetExecutingAssembly()
							.GetExportedTypes()
							.Where
							(
								x => x.IsAssignableTo(typeof(IExamplesProvider<T>))
							)
							.FirstOrDefault(),
						Array.Empty<object>()
					)
					as IExamplesProvider<T>
				)
				.GetExamples()
			);
		}
	}
}