using System.ComponentModel.DataAnnotations;

namespace RainBot.Domain
{
    public class UserProfileEntity
    {
        [Key]
        public int Guid { get; set; }

        [Required]
        public ulong DiscordId { get; set; }

        [Required]
        public int BuffsCasted { get; set; }

        [Required]
        public int BuffsReceived { get; set; }

        [Required]
        public int DamageCasted { get; set; }

        [Required]
        public int DamageReceived { get; set; }

        [Required]
        public int HexCasted { get; set; }

        [Required]
        public int HexReceived { get; set; }

        [Required]
        public int GamesStarted { get; set; }
    }
}