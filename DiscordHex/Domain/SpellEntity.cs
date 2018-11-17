
using System.ComponentModel.DataAnnotations;

namespace DiscordHex.Domain
{
    public class SpellEntity
    {
        [Key]
        public int Guid { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public SpellTypeEnum Type { get; set; }
    }
}
