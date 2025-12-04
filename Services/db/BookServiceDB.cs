using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using project.Data;
using project.Interfaces;
using project.Models;

public class BookServiceDB : IServiceItems<BookDb>
{
    private readonly AppDbContext _context;

    public BookServiceDB(AppDbContext context)
    {
        _context = context;
    }

    public void DeleteAllBooks()
    {
        try
        {
            var allBooks = _context.Books.ToList();

            if (allBooks.Any())
            {
                _context.Books.RemoveRange(allBooks);
                _context.SaveChanges();
                Console.WriteLine("All books have been deleted successfully.");
            }
            else
            {
                Console.WriteLine("No books found to delete.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while deleting all books: {ex.Message}");
        }
    }

  public void ImportBooksFromJson(string filePath)
{
    Console.WriteLine("Importing books from JSON file: " + filePath);
    try
    {
        string jsonContent = File.ReadAllText(filePath);
        List<BookDb> books = JsonSerializer.Deserialize<List<BookDb>>(jsonContent);

        if (books == null || !books.Any())
        {
            Console.WriteLine("No books found in JSON file.");
            return;
        }

        foreach (var book in books)
        {
            var author = _context.Authors.Find(book.AuthorId);
            if (author == null)
            {
                Console.WriteLine($"Author with ID {book.AuthorId} not found for book '{book.Name}'. Skipping this book.");
                continue;
            }

            var existingBook = _context.Books
                .FirstOrDefault(b => b.Name == book.Name && b.AuthorId == book.AuthorId);
            if (existingBook != null)
            {
                Console.WriteLine($"Duplicate book '{book.Name}' for Author ID {book.AuthorId}. Skipping.");
                continue;
            }

            _context.Books.Add(book);
        }

        _context.SaveChanges();
        Console.WriteLine("Books successfully imported!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error importing books: {ex.Message}");
    }
}    public void DeleteAllAuthors()
    {
        try
        {
            var allAuthors = _context.Authors.ToList();

            if (allAuthors.Any())
            {
                _context.Authors.RemoveRange(allAuthors);
                _context.SaveChanges();
                Console.WriteLine("All authors have been deleted successfully.");
            }
            else
            {
                Console.WriteLine("No authors found to delete.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while deleting all authors: {ex.Message}");
        }
    }

    public void ImportAuthorsFromJson(string filePath)
    {
        Console.WriteLine("Importing authors from JSON file: " + filePath);
        try
        {
            string jsonContent = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };
            List<AuthorDb> authors = JsonSerializer.Deserialize<List<AuthorDb>>(jsonContent, options);

            if (authors == null || !authors.Any())
            {
                Console.WriteLine("No authors found in JSON file.");
                return;
            }

            foreach (var author in authors)
            {
                Console.WriteLine($"Processing author: {author.Name}, ID: {author.Id}");
                var existingAuthor = _context.Authors.Find(author.Id);
                if (existingAuthor == null)
                {
                    author.Books = null; // למנוע בעיות
                    _context.Authors.Add(new AuthorDb
                    {
                        Name = author.Name,
                        Password = author.Password,
                        Role = author.Role,
                        Address = author.Address,
                        BirthDate = author.BirthDate
                    });
                    Console.WriteLine($"Added new author: {author.Name}, ID: {author.Id}");
                }
                else
                {
                    existingAuthor.Name = author.Name;
                    existingAuthor.Address = author.Address;
                    existingAuthor.BirthDate = author.BirthDate;
                    Console.WriteLine($"Updated existing author: {author.Name}, ID: {author.Id}");
                }
            }

            _context.SaveChanges();
            Console.WriteLine("Authors successfully imported!");
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database Update Error: {dbEx.Message}");
            if (dbEx.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {dbEx.InnerException.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing authors: {ex.Message}");
        }
    }
    List<BookDb> IService<BookDb>.Get()
    {
        // // שים לב: לא מוחקים כאן מחברים!
        
        // ImportAuthorsFromJson(@"C:\Users\user1\Desktop\נחמי תיכנות\יד\core\Core\MyprojectCoreWithNechami\data\author.json");
        // ImportBooksFromJson(@"C:\Users\user1\Desktop\נחמי תיכנות\יד\core\Core\MyprojectCoreWithNechami\data\book.json");
         Console.WriteLine("Get method called in BookServiceDB");

       var books = from b in _context.Books
                join a in _context.Authors on b.AuthorId equals a.Id into bookAuthor
                from author in bookAuthor.DefaultIfEmpty()
                select new BookDb
                {
                    Id = b.Id,
                    Name = b.Name,
                    AuthorId= author.Id ,
                    Price = b.Price,
                    Date = b.Date
                };
                

    return books.ToList();

       
    }

    public BookDb Get(int id)
    {
        return _context.Books.Find(id);
    }

    public int Insert(BookDb newBook)
    {
        if (newBook == null) throw new ArgumentNullException(nameof(newBook));

        _context.Books.Add(newBook);
        _context.SaveChanges();
        return newBook.Id;
    }

    public bool Update(int id, BookDb book)
    {
        var existingBook = _context.Books.Find(id);
        if (existingBook == null) return false;

        existingBook.Name = book.Name;
        existingBook.Price = book.Price;
        existingBook.AuthorId = book.AuthorId;
        existingBook.Date = book.Date;

        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null) return false;

        _context.Books.Remove(book);
        _context.SaveChanges();
        return true;
    }

    public void deleteUsersItem(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid Author ID.", nameof(id));
        }

        var booksByAuthor = _context.Books.Where(book => book.AuthorId == id).ToList();

        if (booksByAuthor.Any())
        {
            _context.Books.RemoveRange(booksByAuthor);
            _context.SaveChanges();
        }
    }


}