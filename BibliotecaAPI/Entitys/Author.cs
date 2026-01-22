using BibliotecaAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Entitys
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(100, ErrorMessage = "The field {0} must contain {1} chars or less")]
        [FirstUpperLetter]
        public required string Name { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();


    }
}
