using BibliotecaAPI.Data;
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

        public AuthorsController(AplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [HttpGet("/list")]
        public async Task<IEnumerable<Author>> Get()
        {
            return await context.Authors.ToListAsync();
        }
         
        [HttpGet("first")]
        public async Task<Author> GetFirst()
        {
            return await context.Authors.FirstAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Author>> Get([FromRoute] int id, [FromQuery] bool includeBooks)
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

        [HttpGet("{name:alpha}")]

        public async Task<IEnumerable<Author>> Get(string name)
        {
            return await context.Authors.Where(x => x.Name.Contains(name)).ToListAsync();

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Author author)
        {
            context.Add(author);
            await context.SaveChangesAsync();
            return Created();

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
