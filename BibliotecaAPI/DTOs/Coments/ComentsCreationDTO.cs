using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs.Coments
{
    public class ComentsCreationDTO
    {
        [Required]
        public required string Body { get; set; }

    }

}
