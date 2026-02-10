using AutoMapper;
using BibliotecaAPI.Data;
using BibliotecaAPI.DTOs.AuthorsBooks;
using BibliotecaAPI.DTOs.Books;
using BibliotecaAPI.Entitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{

    [ApiController]
    [Route("api/books")]
    [Authorize]
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
        public async Task<ActionResult<BookWithAuthorDTO>> Get(int id)
        {

            var book = await context.Books
                .Include(x => x.Authors).ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (book is null)
            {
                return NotFound($"Book with id {id} not found");
            }

            var bookDTO = mapper.Map<BookWithAuthorDTO>(book);
            return bookDTO;
        }

        [HttpPost]
        public async Task<ActionResult<BookCreationDTO>> Post(BookCreationDTO bookCreationDTO)
        {
            if (bookCreationDTO.AuthorsIds is null || bookCreationDTO.AuthorsIds.Count == 0)
            {
                ModelState.AddModelError(nameof(bookCreationDTO.AuthorsIds), "Authors missing");
                return ValidationProblem();
            }

            var authorsId = await context.Authors.Where(x => bookCreationDTO.AuthorsIds.Contains(x.Id))
                .Select(x => x.Id).ToListAsync();
            
            if (authorsId.Count != bookCreationDTO.AuthorsIds.Count)
            {
                var authorsNot = bookCreationDTO.AuthorsIds.Except(authorsId);
                var authorsNotFound = string.Join(",", authorsNot);
                var errorMessage = $"This authors not exists {authorsNotFound}";

                ModelState.AddModelError(nameof(bookCreationDTO.AuthorsIds), errorMessage);
                return ValidationProblem();
            }

            var book = mapper.Map<Book>(bookCreationDTO);
            AddOrderAuthors(book);
            

            context.Add(book);
            await context.SaveChangesAsync();

            var bookDTO = mapper.Map<BooksDTO>(book);

            return CreatedAtRoute("GetBook", new { id = book.Id }, bookDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<BookCreationDTO>> Put(int id, BookCreationDTO bookCreationDTO)
        {
            if (bookCreationDTO.AuthorsIds is null || bookCreationDTO.AuthorsIds.Count == 0)
            {
                ModelState.AddModelError(nameof(bookCreationDTO.AuthorsIds), "Authors missing");
                return ValidationProblem();
            }

            var authorsId = await context.Authors.Where(x => bookCreationDTO.AuthorsIds.Contains(x.Id))
                .Select(x => x.Id).ToListAsync();

            if (authorsId.Count != bookCreationDTO.AuthorsIds.Count)
            {
                var authorsNot = bookCreationDTO.AuthorsIds.Except(authorsId);
                var authorsNotFound = string.Join(",", authorsNot);
                var errorMessage = $"This authors not exists {authorsNotFound}";

                ModelState.AddModelError(nameof(bookCreationDTO.AuthorsIds), errorMessage);
                return ValidationProblem();
            }

            var bookDB = await context.Books.
                Include(x => x.Authors).FirstOrDefaultAsync(x => x.Id == id);

            if (bookDB is null)
            {
                return NotFound();
            }

            bookDB = mapper.Map(bookCreationDTO, bookDB);
            AddOrderAuthors(bookDB);

            await context.SaveChangesAsync();
            return NoContent();
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



        private void AddOrderAuthors(Book book)
        {
            if (book.Authors is not null)
            {
                for (int i = 0; i < book.Authors.Count; i++)
                {
                    book.Authors[i].Order = i;
                }
            }
        }
    }
}