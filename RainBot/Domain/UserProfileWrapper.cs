using System.Collections.Generic;

namespace RainBot.Domain
{
    public class UserProfileWrapper
    {
        public UserProfileEntity Profile { get; set; }
        public List<ActiveEffectEntity> Effects { get; set; }
    }
}