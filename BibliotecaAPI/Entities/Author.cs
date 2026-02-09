using BibliotecaAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Entitys
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(150, ErrorMessage = "The field {0} must contain {1} chars or less")]
        [FirstUpperLetter]
        public required string Names { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(150, ErrorMessage = "The field {0} must contain {1} chars or less")]
        [FirstUpperLetter]
        public required string Surnames { get; set; }
        [StringLength(20, ErrorMessage = "The field {0} must contain {1} chars or less")]
        public string? Identity { get; set; }

        public List<AuthorBook> Books { get; set; } = [];


    }
}
