using BibliotecaAPI.Entities;
using Microsoft.AspNetCore.Identity;

namespace BibliotecaAPI.Services
{
    public interface IUsersService
    {
        Task<User?> GetUser();
    }
}