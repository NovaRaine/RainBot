using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using DiscordHex.Domain;
using RainBot.Data;
using RainBot.Domain;

namespace RainBot.Services
{
    public class ProfileService
    {
        private ProfileRepository ProfileRepository { get; set; }

        public ProfileService(BotContext dbContext)
        {
            ProfileRepository = new ProfileRepository(dbContext);
        }

        public UserProfileWrapper GetUserProfile(ulong id)
        {
            var wrapper = ProfileRepository.GetUserProfile(id);
            return wrapper;
        }

        internal void IncreaseCount(ulong id, SpellTypeEnum type, bool caster)
        {
            var wrapper = ProfileRepository.GetUserProfile(id);

            switch (type)
            {
                case SpellTypeEnum.Buff:
                    if (caster) wrapper.Profile.BuffsCasted++; else wrapper.Profile.BuffsReceived++;
                    break;

                case SpellTypeEnum.DirectDamage:
                    if (caster) wrapper.Profile.DamageCasted++; else wrapper.Profile.DamageReceived++;
                    break;

                case SpellTypeEnum.Hex:
                    if (caster) wrapper.Profile.HexCasted++; else wrapper.Profile.HexReceived++;
                    break;

                default:
                    break;
            }

            ProfileRepository.UpdateProfile(wrapper.Profile);
        }

        public void AddSpellEffect(IReadOnlyCollection<SocketUser> mentionedUsers, string name, int hours)
        {
            ProfileRepository.AddSpellEffect(mentionedUsers.Select(x => x.Id).ToList(), name, hours);
        }
    }
}