namespace BibliotecaAPI.DTOs.Auth
{
    public class HashResponseDTO
    {
        public required string Hash { get; set; }
        public required byte[] Sal { get; set; }
    }
}
