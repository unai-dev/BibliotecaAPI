using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs
{
    public class ComentsCreationDTO
    {
        [Required]
        public required string Body { get; set; }

    }

}
