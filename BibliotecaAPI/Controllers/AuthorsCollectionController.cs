using AutoMapper;
using BibliotecaAPI.Data;
using BibliotecaAPI.DTOs.Authors;
using BibliotecaAPI.DTOs.AuthorsBooks;
using BibliotecaAPI.Entitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/authors-collection")]
    [Authorize(Policy = "isadmin")]
    public class AutoresColeccionController : ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresColeccionController(AplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{ids}", Name = "GetAuthorsById")]
        public async Task<ActionResult<List<AuthorsWithBooksDTO>>> Get(string ids)
        {
            var idsColeccion = new List<int>();

            foreach (var id in ids.Split(","))
            {
                if (int.TryParse(id, out int idInt))
                {
                    idsColeccion.Add(idInt);
                }
            }

            if (!idsColeccion.Any())
            {
                ModelState.AddModelError(nameof(ids), "Not found ids");
                return ValidationProblem();
            }

            var autores = await context.Authors
                            .Include(x => x.Books)
                                .ThenInclude(x => x.Book)
                            .Where(x => idsColeccion.Contains(x.Id))
                            .ToListAsync();

            if (autores.Count != idsColeccion.Count)
            {
                return NotFound();
            }

            var autoresDTO = mapper.Map<List<AuthorsWithBooksDTO>>(autores);
            return autoresDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post(IEnumerable<AuthorCreationDTO> autoresCreacionDTO)
        {
            var autores = mapper.Map<IEnumerable<Author>>(autoresCreacionDTO);
            context.AddRange(autores);
            await context.SaveChangesAsync();

            var autoresDTO = mapper.Map<IEnumerable<AuthorsDTO>>(autores);
            var ids = autores.Select(x => x.Id);
            var idsString = string.Join(",", ids);
            return CreatedAtRoute("GetAuthorsById", new { ids = idsString }, autoresDTO);
        }
    }
}