using System.Text.Json;
using project.Data;
using project.Interfaces;
using project.Models;

public class AuthorServiceDB : IService<AuthorDb>
{
    private readonly AppDbContext _context;
    private readonly CurrentUserService _currentUserService;

    public AuthorServiceDB(AppDbContext context, CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public void DeleteAllAuthors()
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
            List<AuthorDb> authors = JsonSerializer.Deserialize<List<AuthorDb>>(jsonContent);

            if (authors == null || !authors.Any())
            {
                Console.WriteLine("No authors found in JSON file.");
                return;
            }

            foreach (var author in authors)
            {
                var existingAuthor = _context.Authors.Find(author.Id);
                if (existingAuthor == null)
                {
                    _context.Authors.Add(author);
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing authors: {ex.Message}");
        }
    }

    public List<AuthorDb> Get()
    {
        var authorRole = _currentUserService.Role;

        if (authorRole == "Admin")
        {
            return _context.Authors.ToList();
        }
        else if (authorRole == "Author")
        {
            var id = _currentUserService.Id;
            return _context.Authors.Where(a => a.Id == id).ToList();
        }

        return null; // Unauthorized
    }

    public AuthorDb Get(int id)
    {
        var authorRole = _currentUserService.Role;

        if (authorRole == "Admin")
        {
            return _context.Authors.Find(id);
        }
        else if (authorRole == "Author" && id == _currentUserService.Id)
        {
            return _context.Authors.Find(id);
        }

        return null; // Unauthorized
    }

    public int Insert(AuthorDb newAuthor)
    {
        if (newAuthor == null) throw new ArgumentNullException(nameof(newAuthor));
        if (string.IsNullOrWhiteSpace(newAuthor.Name)) throw new ArgumentException("Name is required.");
        if (string.IsNullOrWhiteSpace(newAuthor.Address)) throw new ArgumentException("Address is required.");
        if (newAuthor.BirthDate > DateTime.Now)
            throw new ArgumentException("BirthDate must be in the past.");

        _context.Authors.Add(newAuthor);
        _context.SaveChanges();
        return newAuthor.Id;
    }

    public bool Update(int id, AuthorDb updatedAuthor)
    {
        var authorRole = _currentUserService.Role;

        if (authorRole == "Admin" || (authorRole == "Author" && id == _currentUserService.Id))
        {
            var existingAuthor = _context.Authors.Find(id);
            if (existingAuthor == null) return false;

            existingAuthor.Name = updatedAuthor.Name;
            existingAuthor.Address = updatedAuthor.Address;
            existingAuthor.BirthDate = updatedAuthor.BirthDate;

            _context.SaveChanges();
            return true;
        }

        return false; // Unauthorized
    }

    public bool Delete(int id)
    {
        var authorRole = _currentUserService.Role;

        if (authorRole == "Admin")
        {
            var author = _context.Authors.Find(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                _context.SaveChanges();
                return true;
            }
        }

        return false; // Unauthorized
    }
}
