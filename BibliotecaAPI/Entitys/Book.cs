using BibliotecaAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Entitys
{
    public class Book
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(400 ,MinimumLength = 3)]
        [FirstUpperLetter]
        public required string Title { get; set; }

        public int AuthorId { get; set; }

        public Author? Author { get; set; }



    }
}
