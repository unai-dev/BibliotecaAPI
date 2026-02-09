using BibliotecaAPI.DTOs.Books;
using BibliotecaAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs.Authors
{
    public class AuthorCreationDTO
    {
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

        public List<BookCreationDTO> Books { get; set; } = [];

    }
}
