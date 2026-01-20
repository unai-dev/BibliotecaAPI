using BibliotecaAPI.Entitys;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Data
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

    }
}
