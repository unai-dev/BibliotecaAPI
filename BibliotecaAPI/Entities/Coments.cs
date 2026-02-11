using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Entitys
{
    public class Coments
    {

        public Guid Id { get; set; }

        [Required]
        public required string Body { get; set; }
        public DateTime PublicDate { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public required string UserId { get; set; }
        public IdentityUser? User { get; set; }

        
    }
}
