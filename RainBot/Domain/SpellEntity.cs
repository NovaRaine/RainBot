using System.ComponentModel.DataAnnotations;
using DiscordHex.Domain;

namespace RainBot.Domain
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