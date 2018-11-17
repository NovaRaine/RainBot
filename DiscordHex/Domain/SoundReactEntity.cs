
using System.ComponentModel.DataAnnotations;

namespace DiscordHex.Domain
{
    public class SoundReactEntity
    {
        [Key]
        public int Guid { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public SoundReactTypeEnum Type { get; set; }
    }
}
