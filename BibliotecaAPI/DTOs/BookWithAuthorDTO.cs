namespace BibliotecaAPI.DTOs
{
    public class BookWithAuthorDTO: BooksDTO
    {
        public int AuthorId { get; set; }
        public required string AuthorName { get; set; }
    }
}
