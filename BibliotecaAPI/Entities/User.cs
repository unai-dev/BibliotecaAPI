using Microsoft.AspNetCore.Identity;

namespace BibliotecaAPI.Entities
{
    public class User: IdentityUser
    {
        public DateTime Birthday { get; set; }
    }
}
