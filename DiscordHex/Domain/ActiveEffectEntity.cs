using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DiscordHex.Domain
{
    public class ActiveEffectEntity
    {
        [Key]
        public int Guid { get; set; }
        [Required]
        public ulong DiscordId { get; set; }
        [Required]
        public string SpellName { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
    }
}
