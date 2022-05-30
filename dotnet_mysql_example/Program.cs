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