using AspDotNetCoreWithKestrelLesson.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AspDotNetCoreWithKestrelLesson.Database
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<User> Users { set; get; }
		public DbSet<Post> Posts { set; get; }
		public DbSet<Comment> Comments { set; get; }

		public ApplicationDbContext([NotNull] DbContextOptions options) : base(options)
		{
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasData
			(
				new User(101, "TestUSer1", "test.user1@server.net"),
				new User(102, "TestUSer2", "test.user2@server.net")
			);
			modelBuilder.Entity<Post>().HasData
			(
				new Post(201, 101, "Test post 1"),
				new Post(202, 102, "Test post 2")
			);
			modelBuilder.Entity<Comment>().HasData
			(
				new Comment(301, 201, 101, "Test comment 1"),
				new Comment(302, 202, 102, "Test comment 2")
			);
		}
	}
}