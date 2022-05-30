*NB: I wrote this following the guide laid out in this [video](https://www.youtube.com/watch?v=wGb2IWlZNl4)*

### So, you wanna use MySQL in .NET? 
Well, we're going to go through the absolute basics in best practices for setting up an environment to integrate a MySQL database context into a .NET Core application.

For this example, I have a containerized instance of MySQL running on local Docker. I set it up using this command: `docker run -p 3331:3306 --name somedb -e MYSQL_ROOT_PASSWORD=asterisks -d mysql:8.0.0`

*Remember your port # and password!*

I made a very simple .NET Core console application. The first thing I will do is use the NuGet Package Manager to get Entity Framework, a very popular package that will act as your constructor and entry point for a database.

Nuget:
**Microsoft.EntityFrameworkCore**
**MySql.EntityFramework**

I'm also going to set up a class where all of our `using` statements will live so that we don't have to import them all over the place.

*GlobalUsing.cs*
```
global using System.Text;
global using Microsoft.EntityFrameworkCore;
global using System.ComponentModel.DataAnnotations.Schema;
```

Now we'll define our schemas. Make a folder titled `Models` and add two classes, `Book.cs` and `Author.cs`. These will be relational tables, so we'll define foreign keys, etc thusly:

**Book.cs**
```
namespace dotnet_mysql_example.Models
{
    public class Book
    {
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        
        [ForeignKey("AuthorId")]

        public virtual Author? Author { get; set; }

        public override string ToString()
        {
            var txt = new StringBuilder();
            txt.AppendLine($"ISBN: {ISBN}");
            txt.AppendLine($"Title: {Title}");
            txt.AppendLine($"Author: {Author.Name}");

            return txt.ToString();
        }
    }
}
```

**Author.cs**
```
namespace dotnet_mysql_example.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public override string ToString()
        {
            var txt = new StringBuilder();
            txt.AppendLine($"ID: {AuthorId}");
            txt.AppendLine($"Name: {Name}");

            return txt.ToString();
        }
    }
}
```
We're going to be using these models throughout our app, so don't forget to add a new `using` statement to our `GlobalUsing.cs` class!

<hr>

### OKAY.
Now we need to give the app some context.

Make a new class `LibraryContext.cs` in a new directory named `Data`. This is how we're going to configure our database context so it can do cool stuff like, uhh, connect to it. And seed it with data!

**LibraryContext.cs**
```
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
```
We're gonna be using this elsewhere, so go ahead and add a new `global using` statement to your `GlobalUsing.cs`!

<hr>

Alright, if everything went to plan, then we should be able to instantiated a database context that we can interact with. This will be the simplest app in the world, as its sole purpose is to put out our database entries to a console.

**Program.cs**
```
CreateDbSeedData(new LibraryContext());
PrintBooks();

static void CreateDbSeedData(LibraryContext context)
{
    using(LibraryContext? _context = context)
    {
        context.Database.EnsureCreated();
    }
}

static void PrintBooks()
{
    using (var context = new LibraryContext())
    {
        var books = context.Books!
                .Include(p => p.Author);
        Console.WriteLine("Printing database contents to the console...");
        foreach (var book in books)
        {
            Console.WriteLine($"Title: {book.Title}");
            Console.WriteLine($"Author: {book.Author}");
            Console.WriteLine("==============");
        }
    }
}
```

*NB: You may need to unload/reload your project to make sure that EntityFramework is working correctly*

If you go into the root folder (containing your `.csproj` file) and run, you will print the contents of your database to the console! Holy cow wowowowowow!!!
