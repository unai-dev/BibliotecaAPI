using AutoMapper;
using BibliotecaAPI.Data;
using BibliotecaAPI.DTOs.Coments;
using BibliotecaAPI.Entitys;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/books/{bookId:int}/coments")]
    [Authorize]
    public class ComentsController: ControllerBase
    {
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IUsersService usersService;

        public ComentsController(AplicationDbContext context, IMapper mapper, IUsersService usersService)
        {
            this.context = context;
            this.mapper = mapper;
            this.usersService = usersService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentsDTO>>> Get(int bookId)
        {
            var book = await context.Books.AnyAsync(x => x.Id == bookId);

            if (!book)
            {
                return NotFound();
            }

            var coments = await context.Coments.Include(u => u.User)
                .Where(x => x.BookId == bookId)
                .OrderByDescending(x => x.PublicDate)
                .ToListAsync();
            return mapper.Map<List<ComentsDTO>>(coments);


        }

        [HttpGet("{id}", Name = "GetComent")]
        public async Task<ActionResult<ComentsDTO>> Get(Guid id)
        {
            var coment = await context.Coments.Include(u => u.User).FirstOrDefaultAsync(x => x.Id == id);

            if (coment is null)
            {
                return NotFound();
            }

            return mapper.Map<ComentsDTO>(coment);


        }

        [HttpPost]
        public async Task<ActionResult> Post(int bookId, ComentsCreationDTO comentaCreeationDto)
        {
            var book = await context.Books.AnyAsync(x => x.Id == bookId);

            if (!book)
            {
                return NotFound();
            }

            var user = await usersService.GetUser();

            if (user is null) return NotFound();



            var coment = mapper.Map<Coments>(comentaCreeationDto);

            coment.BookId = bookId;
            coment.PublicDate = DateTime.UtcNow;
            coment.UserId = user.Id;
            context.Add(coment);
            await context.SaveChangesAsync();

            var comentDTO = mapper.Map<ComentsDTO>(coment);
            return CreatedAtRoute("GetComent", new { id = coment.Id, bookId }, comentDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id, int bookId)
        {
            var book = await context.Books.AnyAsync(x => x.Id == bookId);

            if (!book)
            {
                return NotFound();
            }

            var user = await usersService.GetUser();
            if (user is null) return NotFound();

            var comentDB = await context.Coments.FirstOrDefaultAsync(x => x.Id == id);

            if (comentDB is null) return NotFound();

            if (comentDB.UserId != user.Id) return Forbid();

            context.Remove(comentDB);
            await context.SaveChangesAsync();

            return NoContent();
        }



    }
}
