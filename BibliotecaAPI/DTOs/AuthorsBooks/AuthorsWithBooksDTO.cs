using BibliotecaAPI.DTOs.Authors;
using BibliotecaAPI.DTOs.Books;

namespace BibliotecaAPI.DTOs.AuthorsBooks
{
    public class AuthorsWithBooksDTO: AuthorsDTO
    {
        public List<BooksDTO> Books { get; set; } = [];
    }
}
