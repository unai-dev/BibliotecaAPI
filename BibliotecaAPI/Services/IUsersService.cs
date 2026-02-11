using Microsoft.AspNetCore.Identity;

namespace BibliotecaAPI.Services
{
    public interface IUsersService
    {
        Task<IdentityUser?> GetUser();
    }
}