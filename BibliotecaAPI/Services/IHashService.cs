using BibliotecaAPI.DTOs.Auth;

namespace BibliotecaAPI.Services
{
    public interface IHashService
    {
        HashResponseDTO Hash(string input);
        HashResponseDTO Hash(string input, byte[] sal);
    }
}