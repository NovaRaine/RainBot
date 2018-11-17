using System.ComponentModel.DataAnnotations;

namespace DiscordHex.Domain
{
    public class DiscordUser
    {
        [Key]
        public int Guid { get; set; }
        [Required]
        public string DiscordId { get; set; }
    }
}
