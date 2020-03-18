using System;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class SocialNetworkContext : DbContext 
    {
        public SocialNetworkContext(DbContextOptions<SocialNetworkContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Skill> Skills { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Profile>().ToTable("Profile");
            modelBuilder.Entity<Post>().ToTable("Post");
            modelBuilder.Entity<Like>().ToTable("Like");
            modelBuilder.Entity<Comment>().ToTable("Comment");
            modelBuilder.Entity<Education>().ToTable("Education");
            modelBuilder.Entity<Experience>().ToTable("Experience");
            modelBuilder.Entity<Photo>().ToTable("Photo");
            modelBuilder.Entity<Video>().ToTable("Video");
            modelBuilder.Entity<Skill>().ToTable("Skill");

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Profile>().HasKey(p => p.UserId);
            modelBuilder.Entity<Post>().HasKey(p => p.Id);
            modelBuilder.Entity<Like>().HasKey(l => new { l.UserId, l.PostId });
            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
            modelBuilder.Entity<Education>().HasKey(e => e.Id);
            modelBuilder.Entity<Experience>().HasKey(e => e.Id);
            modelBuilder.Entity<Photo>().HasKey(p => p.Id);
            modelBuilder.Entity<Video>().HasKey(v => v.Id);
            modelBuilder.Entity<Skill>().HasKey(s => s.Id);


            modelBuilder.Entity<User>().HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.UserId);

            modelBuilder.Entity<Profile>().HasMany(p => p.Skills)
                .WithOne()
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Profile>().HasMany(p => p.Experiences)
                .WithOne()
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Profile>().HasMany(p => p.Educations)
                .WithOne()
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Profile>().HasMany(p => p.Likes)
                .WithOne(l => l.UserProfile)
                .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<Profile>().HasMany(p => p.Comments)
                .WithOne(c => c.UserProfile)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Profile>().HasOne(p => p.Avatar)
                .WithOne()
                .HasForeignKey<Profile>(p => p.AvatarId);

            modelBuilder.Entity<Post>().HasMany(p => p.Likes)
                .WithOne(l => l.Post)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>().HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>().HasOne(p => p.Photo)
                .WithOne()
                .HasForeignKey<Post>(p => p.PhotoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>().HasOne(p => p.Video)
                .WithOne()
                .HasForeignKey<Post>(p => p.VideoId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
