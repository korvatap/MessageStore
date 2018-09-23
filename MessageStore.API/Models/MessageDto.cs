using System.ComponentModel.DataAnnotations;

namespace MessageStore.API.Models
{
    public class MessageDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Body { get; set; }
    }
}