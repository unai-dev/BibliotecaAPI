using AutoMapper;
using Azure;
using BibliotecaAPI.Data;
using BibliotecaAPI.DTOs.Authors;
using BibliotecaAPI.DTOs.AuthorsBooks;
using BibliotecaAPI.Entitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{

    [ApiController]
    [Route("api/authors")]
    [Authorize(Policy = "isadmin")]
    public class AuthorsController : ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public AuthorsController(AplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<AuthorsDTO>> Get()
        {
            var authors = await context.Authors.ToListAsync();
            var authorsDTO = mapper.Map<IEnumerable<AuthorsDTO>>(authors);
            return authorsDTO;
        }

        [HttpGet("{id:int}", Name = "GetAuthor")]
        public async Task<ActionResult<AuthorsWithBooksDTO>> Get(int id)
        {
            var author = await context.Authors
                .Include(x => x.Books).ThenInclude(x => x.Book)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (author is null)
            {
                return NotFound($"Author with id {id} not found");
            }

            var authorDTO = mapper.Map<AuthorsWithBooksDTO>(author);

            return authorDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post(AuthorCreationDTO authorCreationDTO)
        {
            var author = mapper.Map<Author>(authorCreationDTO);

            context.Add(author);
            await context.SaveChangesAsync();
            var authorDTO = mapper.Map<AuthorsDTO>(author);

            return CreatedAtRoute("GetAuthor", new { id = author.Id }, authorDTO);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, AuthorCreationDTO authorCreationDTO)
        {
            var author = mapper.Map<Author>(authorCreationDTO);
            author.Id = id;

            context.Update(author);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registerDeleted = await context.Authors.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (registerDeleted == 0)
            {
                return NotFound($"Author with id {id} not found");
            }

            return NoContent();
        }
    }
}
