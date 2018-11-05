using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordHex.Domain
{
    public class UserProfileEntity
    {
        public int ProfileGuid { get; set; }
        public int DiscordUserGuid { get; set; }
        public string DiscordId { get; set; }
        public int BuffsCasted { get; set; }
        public int BuffsReceived { get; set; }
        public int DamageCasted { get; set; }
        public int DamageReceived { get; set; }
        public int HexCasted { get; set; }
        public int HexReceived { get; set; }
        public int GamesStarted { get; set; }
        public List<SpellEffectEntity> ActiveEffects { get; set; }
    }
}
