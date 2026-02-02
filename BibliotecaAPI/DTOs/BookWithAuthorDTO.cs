namespace BibliotecaAPI.DTOs
{
    public class BookWithAuthorDTO: BooksDTO
    {
        public List<AuthorsDTO> Authors { get; set; } = [];
    }
}
