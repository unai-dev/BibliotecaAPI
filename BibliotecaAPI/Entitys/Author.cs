using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Entitys
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
    }
}
