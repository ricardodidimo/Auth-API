using api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Extensions
{
    public static class ModelCreatingExtension
    {
        public static ModelBuilder BuildUser(this ModelBuilder builder)
        {
            builder.Entity<User>().Property("username")
                .IsRequired()
                .HasColumnType("varchar(25)");

            builder.Entity<User>().Property("normalized_username")
                .IsRequired()
                .HasColumnType("varchar(25)");
            builder.Entity<User>().HasIndex("normalized_username").IsUnique();

            builder.Entity<User>().Property("password")
                .IsRequired();

            return builder;
        }
        public static ModelBuilder BuildCategory(this ModelBuilder builder)
        {
            builder.Entity<Category>().Property("title")
                .IsRequired()
                .HasColumnType("varchar(25)");
            builder.Entity<Category>().HasIndex("title").IsUnique();

            builder.Entity<Category>().Property("description")
                .IsRequired();

            return builder;
        }
        public static ModelBuilder BuildLink(this ModelBuilder builder)
        {
            builder.Entity<Link>().Property("url")
                .IsRequired()
                .HasColumnType("varchar(25)");

            return builder;
        }
         public static ModelBuilder BuildArtist(this ModelBuilder builder)
        {
            builder.Entity<Artist>().Property("name")
                .IsRequired()
                .HasColumnType("varchar(25)");
            builder.Entity<Artist>().HasIndex("name").IsUnique();

            builder.Entity<Artist>().Property("description")
                .IsRequired();

            return builder;
        }
    }
}