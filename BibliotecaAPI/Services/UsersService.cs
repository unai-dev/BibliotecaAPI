using Microsoft.AspNetCore.Identity;

namespace BibliotecaAPI.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UsersService(UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityUser?> GetUser()
        {
            var emailClaim = httpContextAccessor.HttpContext!.User.Claims.Where(x => x.Type == "email")
                .FirstOrDefault();

            if (emailClaim is null)
            {
                return null;
            }

            var email = emailClaim.Value;
            return await userManager.FindByEmailAsync(email);
        }
    }
}
