namespace BibliotecaAPI.DTOs
{
    public class AuthorsDTO
    {
        public int Id { get; set; }
        public required string FullName { get; set; }

        public List<BooksDTO> Books { get; set; } = []; 
    }
}
