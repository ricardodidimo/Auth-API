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

    }
}