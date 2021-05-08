using System.Diagnostics.CodeAnalysis;
using AspDotNetCoreWithKestrelLesson.Models;
using Microsoft.EntityFrameworkCore;

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
			// Configure key fields.
			modelBuilder.Entity<User>().HasKey(user => user.Id);
			modelBuilder.Entity<Post>().HasKey(post => post.Id);
			modelBuilder.Entity<Comment>().HasKey(comment => comment.Id);
			// Configure data constraints.
			modelBuilder.Entity<User>().Property(user => user.Id).ValueGeneratedNever();
			modelBuilder.Entity<User>().Property(user => user.Handle).IsRequired(true);
			modelBuilder.Entity<User>().Property(user => user.Email).IsRequired(true);
			modelBuilder.Entity<Post>().Property(post => post.Id).ValueGeneratedNever();
			modelBuilder.Entity<Post>().Property(post => post.UserId).IsRequired(true);
			modelBuilder.Entity<Post>().Property(post => post.Content).IsRequired(true);
			modelBuilder.Entity<Comment>().Property(comment => comment.Id).ValueGeneratedNever();
			modelBuilder.Entity<Comment>().Property(comment => comment.PostId).IsRequired(true);
			modelBuilder.Entity<Comment>().Property(comment => comment.UserId).IsRequired(true);
			modelBuilder.Entity<Comment>().Property(comment => comment.Content).IsRequired(true);
			// Configure relationships.
			modelBuilder.Entity<User>().HasMany(user => user.Posts).WithOne();
			modelBuilder.Entity<User>().HasMany(user => user.Comments).WithOne();
			modelBuilder.Entity<Post>().HasMany(post => post.Comments).WithOne();
			// Seed data.
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