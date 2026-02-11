using BibliotecaAPI.Entitys;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs.Coments
{
    public class ComentsDTO
    {
        public Guid Id { get; set; }
        public required string Body { get; set; }
        public DateTime PublicDate { get; set; }
        public required string UserId { get; set; }
        public required string UserEmail { get; set; }
    }
}
