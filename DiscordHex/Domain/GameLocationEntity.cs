
using System.ComponentModel.DataAnnotations;

namespace DiscordHex.Domain
{
    public class GameLocationEntity
    {
        [Key]
        public int Guid { get; set; }
        [Required]
        public string OptionTitle { get; set; }
        [Required]
        public string Description { get; set; }
        public int? Parent { get; set; }
        [Required]
        public int StoryId { get; set; }
        [Required]
        public int ChapterId { get; set; }
    }
}
