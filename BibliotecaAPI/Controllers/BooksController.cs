using BibliotecaAPI.Data;
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

        public BooksController(AplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> Get()
        {
            return await context.Books.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> Get(int id)
        {

            var book = await context.Books
                .Include(x => x.Author).
                FirstOrDefaultAsync(x => x.Id == id);

            if (book is null)
            {
                return NotFound($"Book with id {id} not found");
            }
            return book;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Post(Book book)
        {
            var author = await context.Authors.AnyAsync(x => x.Id == book.AuthorId);

            if (!author)
            {
                return BadRequest($"Author with id {book.AuthorId} not found");
            }

            context.Add(book);
            await context.SaveChangesAsync();
            return Created();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Book>> Put(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest("Id's not same");
            }

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