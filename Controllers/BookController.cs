using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.Interfaces;
using project.Models;
using project.Services;
using System.Collections.Generic;
using System.Security.Claims;

namespace project.Controllers;

[ApiController]
[Authorize(Policy = "Author")]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IServiceItems<BookDb> service;
  

    public BookController(IServiceItems<BookDb> service)
    {
       System.Console.WriteLine("BookController constructor called");
        this.service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<BookDb>> Get()
    {
        System.Console.WriteLine("BookController Get method called");
         return service.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<BookDb> Get(int id)
    {
        try
        {
            return service.Get(id);
            System.Console.WriteLine("BookController Get method called with id: " + id);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public ActionResult Post(BookDb newBook)
    {
        var result = service.Insert(newBook);
        if (result == -1)
        {
            return BadRequest("Failed to create new book.");
        }
        if (result == -2)
        {
            return Forbid("Unauthorized: Author name does not match the logged-in user.");
        }
        return CreatedAtAction(nameof(Post), new { Id = result });
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, BookDb book)
    {
        var result = service.Update(id, book);
        
        if (result == false)
        {
            return Forbid("Unauthorized access");
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var result = service.Delete(id);
        if (result == false)
        {
            return NotFound("Book not found or unauthorized access");
        }
        return Ok();
    }
}