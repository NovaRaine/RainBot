using System;

namespace RainBot.Domain
{
    public class SpellEffectEntity
    {
        public int Guid { get; set; }
        public string DiscordId { get; set; }
        public string SpellName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}