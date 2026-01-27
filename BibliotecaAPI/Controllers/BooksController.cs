using AutoMapper;
using BibliotecaAPI.Data;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{

    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public BooksController(AplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<BooksDTO>> Get()
        {
            var books =  await context.Books.ToListAsync();

            var booksDTO = mapper.Map<IEnumerable<BooksDTO>>(books);

            return booksDTO;
        }

        [HttpGet("{id:int}", Name ="GetBook")]
        public async Task<ActionResult<BooksDTO>> Get(int id)
        {

            var book = await context.Books
                .Include(x => x.Author).
                FirstOrDefaultAsync(x => x.Id == id);

            if (book is null)
            {
                return NotFound($"Book with id {id} not found");
            }

            var bookDTO = mapper.Map<BooksDTO>(book);
            return bookDTO;
        }

        [HttpPost]
        public async Task<ActionResult<BookCreationDTO>> Post(BookCreationDTO bookCreationDTO)
        {
            var book = mapper.Map<Book>(bookCreationDTO);
            var author = await context.Authors.AnyAsync(x => x.Id == book.AuthorId);

            if (!author)
            {
                ModelState.AddModelError(nameof(book.AuthorId), $"Author with id {book.AuthorId} not found");
                return ValidationProblem();
            }

            context.Add(book);
            await context.SaveChangesAsync();

            var bookDTO = mapper.Map<BooksDTO>(book);

            return CreatedAtRoute("GetBook", new {id = book.Id}, bookDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<BookCreationDTO>> Put(int id, BookCreationDTO bookCreationDTO)
        {
            var book = mapper.Map<Book>(bookCreationDTO);
            book.Id = id;

            var author = await context.Authors.AnyAsync(x => x.Id == book.AuthorId);

            if (!author)
            {
                return BadRequest($"Author with id {book.AuthorId} not found");
            }

            context.Update(book);
            await context.SaveChangesAsync();
            return Accepted();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registerDeleted = await context.Books.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (registerDeleted == 0)
            {
                return NotFound($"Book with id {id} not found");
            }

            return NoContent();
        }
    }
}