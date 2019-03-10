using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RainBot.Domain
{
    public class SoundReactEntity
    {
        [Key]
        [Column("guid")]
        public int Guid { get; set; }

        [Required]
        [Column("url")]
        public string Url { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("type")]
        public SoundReactTypeEnum Type { get; set; }
    }
}