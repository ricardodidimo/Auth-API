using api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Extensions
{
    public static class ModelCreatingExtension
    {
        /// <summary>Elaborate constraints for columns in the app database schema, such as indexes, unique and not null constraints.</summary>
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