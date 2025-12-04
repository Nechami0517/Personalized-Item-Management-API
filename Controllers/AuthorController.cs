using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
namespace project.Controllers;



[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IService<AuthorDb> service;
    private readonly IServiceItems<BookDb> BookService;

    public AuthorController(IService<AuthorDb> service,IServiceItems<BookDb> BookService)
    {
        this.service = service;
        this.BookService = BookService;
        System.Console.WriteLine("AuthorController constructor called");
    }

    [HttpGet]
    [Authorize(policy: "Author")]
    public ActionResult<IEnumerable<AuthorDb>> Get()
    {
        var authors = service.Get();
        if (authors == null)
        {
            return BadRequest("Unauthorized access");
        }
        return authors;
    }

    [HttpGet("{id}")]
    [Authorize(policy: "Admin")]
    public ActionResult<AuthorDb> Get(int id)
    {
        var author = service.Get(id);
        if (author == null)
            throw new ApplicationException("Author not found");
        return author;
    }

    [HttpPost]
    [Authorize(policy: "Admin")]
    public ActionResult Post(AuthorDb newUser)
    {
        var newId = service.Insert(newUser);
        if (newId == -1)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

    [HttpPut("{id}")]
    [Authorize(policy: "Author")]
    public ActionResult Put(int id, AuthorDb author)
    {
        var result = service.Update(id, author);
        if (!result)
        {
            return BadRequest("Unauthorized access");
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(policy: "Admin")]
    public ActionResult Delete(int id)
    {
        var isDeleted = service.Delete(id);
        BookService.deleteUsersItem(id);
        if (isDeleted)
            return Ok();
        return NotFound();
    }
}

