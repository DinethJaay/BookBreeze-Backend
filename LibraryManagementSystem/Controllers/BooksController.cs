using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a book list.
        /// </summary>
        /// <returns>list of BookDto.</returns>
        /// <response code="200">Returns the list of books</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var books = await _context.Books.ToListAsync();

            //Map books 
            var bookDtos = books.Select(book => new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description
            }).ToList();

            return bookDtos;
        }

        /// <summary>
        /// Gets a book by Id.
        /// </summary>
        /// <returns>A book Dto.</returns>
        /// <response code="200">Return the book Details</response>
        /// <response code="404">If the book is not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            var bookDto = new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description
            };

            return bookDto;
        }

        /// <summary>
        /// Add a new book.
        /// </summary>
        /// <param name="bookDto">The book Dto for create a new book</param>
        /// <returns>The created book</returns>
        /// <response code="201">Book created successfully</response>
        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBook(BookDTO bookDto)
        {
            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Description = bookDto.Description
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            //return the book with its id
            var createdBookDto = new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description
            };

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, createdBookDto);
        }

        /// <summary>
        /// Update a book by id.
        /// </summary>
        /// <param name="id">the id of the book</param>
        /// <param name="bookDto">Book dto with updated information</param>
        /// <returns>No content if the update was successful</returns>
        /// <response code="204">Update successful</response>
        /// <response code="400">If the ID does not match</response>
        /// <response code="404">If the book is not found</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, BookDTO bookDto)
        {
            if (id != bookDto.Id) return BadRequest();

            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.Description = bookDto.Description;

            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Delete a book by id.
        /// </summary>
        /// <param name="id">id of the book</param>
        /// <returns>No content if the delete was successful</returns>
        /// <response code="204">Delete successful</response>
        /// <response code="404">If the book is not found</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
