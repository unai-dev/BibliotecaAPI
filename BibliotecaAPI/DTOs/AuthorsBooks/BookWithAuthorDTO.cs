using BibliotecaAPI.DTOs.Authors;
using BibliotecaAPI.DTOs.Books;

namespace BibliotecaAPI.DTOs.AuthorsBooks
{
    public class BookWithAuthorDTO: BooksDTO
    {
        public List<AuthorsDTO> Authors { get; set; } = [];
    }
}
