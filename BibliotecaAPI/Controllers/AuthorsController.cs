using AutoMapper;
using BibliotecaAPI.Data;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BibliotecaAPI.Controllers
{

    [ApiController]
    [Route("api/authors")]
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
        public async Task<IEnumerable<AuthorsDTO>> Get()
        {
            var authors = await context.Authors.ToListAsync();
            var authorsDTO = mapper.Map<IEnumerable<AuthorsDTO>>(authors);
            return authorsDTO;
        }

        [HttpGet("{id:int}", Name = "GetAuthor")]
        public async Task<ActionResult<Author>> Get(int id)
        {
            var author = await context.Authors
                .Include(x => x.Books)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (author is null)
            {
                return NotFound($"Author with id {id} not found");
            }

            return author;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Author author)
        {
            context.Add(author);
            await context.SaveChangesAsync();
            return CreatedAtRoute("GetAuthor", new { id = author.Id }, author);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Author author)
        {
            if (id != author.Id)
            {
                return BadRequest($"The id's not same {id}");
            }
            context.Update(author);
            await context.SaveChangesAsync();
            return Accepted();
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
