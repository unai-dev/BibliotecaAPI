using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs.Users
{
    public class EditClaimDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
