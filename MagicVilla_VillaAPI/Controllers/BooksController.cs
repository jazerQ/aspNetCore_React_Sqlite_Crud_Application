using Application;
using Core.Exceptions;
using Core.Models;
using DataAccess.Repositories;
using MagicVilla_VillaAPI.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        
        private readonly IBookService _bookService;
        public BooksController(IBookService service)
        {
            _bookService = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookResponse>>> GetBooks() 
        {
            var books = await _bookService.GetAllBooks();

            var response = books.Select(b => new BookResponse(b.Id, b.Title, b.Description, b.Price)).ToList();
            return Ok(response);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BookResponse>> GetBook(Guid id) 
        {
            try
            {
                var book = await _bookService.GetBook(id);
                var response = new BookResponse(book.Id, book.Title, book.Description, book.Price);
                return Ok(response);
            }
            catch (BookNotFoundException ex)
            {
                return BadRequest($"Книга не нашлась!\n{ex.Message}");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateBook([FromBody] BookRequest bookReq) 
        {
            (Book book, string exception) = Book.Create(
                Guid.NewGuid(),
                bookReq.Title,
                bookReq.Description,
                bookReq.Price
            );
            if (!string.IsNullOrEmpty(exception)) 
            {
                return BadRequest(exception);
            }
            Guid id = await _bookService.CreateBook(book);
            return Ok(id);
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateBook(Guid id, [FromBody] BookRequest bookReq) 
        {
            var responseId = await _bookService.UpdateBook(id, bookReq.Title, bookReq.Description, bookReq.Price);
            return Ok(responseId);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteBook(Guid id) 
        {
            var responseId = await _bookService.DeleteBook(id);
            return Ok(responseId);
        }
    }
}
