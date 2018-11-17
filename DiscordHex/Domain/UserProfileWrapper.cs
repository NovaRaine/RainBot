
using System.Collections.Generic;

namespace DiscordHex.Domain
{
    public class UserProfileWrapper
    {
        public UserProfileEntity Profile { get; set; }
        public List<ActiveEffectEntity> Effects { get; set; }
    }
}
