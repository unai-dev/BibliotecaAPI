namespace BibliotecaAPI.DTOs
{
    public class AuthorsWithBooksDTO: AuthorsDTO
    {
        public List<BooksDTO> Books { get; set; } = [];
    }
}
