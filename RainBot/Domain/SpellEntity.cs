using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RainBot.Domain
{
    public class SpellEntity
    {
        [Key]
        [Column("guid")]
        public int Guid { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("type")]
        public SpellTypeEnum Type { get; set; }
    }
}