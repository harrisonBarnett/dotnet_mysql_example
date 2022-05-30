using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_mysql_example.Data
{
    internal class LibraryContext : DbContext
    {
        public DbSet<Book>? Books { get; set; }
        public DbSet<Author>? Author { get; set; }
        
        // configure our database connection
        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            ob.UseMySQL("server=localhost;database=library;user=root;port=3331;password=asterisks;SslMode=none;");
        }

        // informs the context of our model shapes and seeds the db
        protected override void OnModelCreating(ModelBuilder mb)
        {
            // Author properties
            mb.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.AuthorId);
                entity.Property(e => e.Name);
            });
            // Book properties
            mb.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.ISBN);
                entity.Property(e => e.Title).IsRequired();
                entity.HasOne(a => a.Author)
                    .WithMany(b => b.Books);
            });
            // seeding authors
            mb.Entity<Author>().HasData(
                new Author
                {
                    AuthorId = 1,
                    Name = "Joh Doe"
                },
                new Author
                {
                    AuthorId = 2,
                    Name = "Some Guy"
                }
                );
            // seeding books
            mb.Entity<Book>().HasData(
                new Book
                {
                    ISBN = "123123",
                    Title = "A Very Good Book",
                    AuthorId = 1
                },
                new Book
                {
                    ISBN = "456456",
                    Title = "A Very Bad Book",
                    AuthorId = 2
                },
                new Book
                {
                    ISBN = "789789",
                    Title = "A Very Mediocre Book",
                    AuthorId = 1
                }
                );
        }
    }
}
