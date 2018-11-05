using DiscordHex.Data;
using DiscordHex.Domain;
using System.Threading.Tasks;
using System;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Services
{
    public class ProfileService
    {
        private ProfileRepository ProfileRepository { get; set; }

        public ProfileService()
        {
            ProfileRepository = new ProfileRepository();
        }

        public UserProfileEntity GetUserProfile(ulong id)
        {
            var profile = ProfileRepository.GetUserProfile(id.ToString());
            if (string.IsNullOrEmpty(profile.DiscordId))
            {
                ProfileRepository.SaveNewUserProfile(id.ToString());
                profile = ProfileRepository.GetUserProfile(id.ToString());
            }
            return profile;
        }

        internal void IncreaseCount(ulong id, SpellTypeEnum type, bool caster)
        {
            Task.Run(() => UpdateProfile(id, type, caster));
        }

        private void UpdateProfile(ulong id, SpellTypeEnum type, bool caster)
        {
            var profile = ProfileRepository.GetUserProfile(id.ToString());

            switch (type)
            {
                case SpellTypeEnum.Buff:
                    if (caster) profile.BuffsCasted++; else profile.BuffsReceived++;
                    break;
                case SpellTypeEnum.DirectDamage:
                    if (caster) profile.DamageCasted++; else profile.DamageReceived++;
                    break;
                case SpellTypeEnum.Hex:
                    if (caster) profile.HexCasted++; else profile.HexReceived++;
                    break;
                default:
                    break;
            }

            ProfileRepository.UpdateProfile(profile);
        }

        public void AddSpellEffect(IReadOnlyCollection<SocketUser> mentionedUsers, string name, int hours)
        {
            ProfileRepository.AddSpellEffect(mentionedUsers.Select(x => x.Id.ToString()).ToList(), name, hours);
        }
    }
}
