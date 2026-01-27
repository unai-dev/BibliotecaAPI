namespace BibliotecaAPI.DTOs
{
    public class BooksDTO
    {

        public int Id { get; set; }
        public required string Title { get; set; }

        public int AuthorId { get; set; }

    }
}
