using DiscordHex.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Data
{
    public class ProfileRepository
    {
        public UserProfileWrapper GetUserProfile(ulong id)
        {
            var profile = new UserProfileWrapper();
            
            using (var db = new BotContext())
            {
                profile.Profile = db.UserProfiles.Where(x => x.DiscordId == id).FirstOrDefault();
            }

            if (profile.Profile == null)
            {
                SaveNewUserProfile(id);
                using (var db = new BotContext())
                {
                    profile.Profile = db.UserProfiles.Where(x => x.DiscordId == id).FirstOrDefault();
                }
            }

            if (profile.Profile != null)
            {
                using (var db = new BotContext())
                {
                    profile.Effects = db.ActiveEffects.Where(x => x.DiscordId == id).ToList();
                }
            }

            return profile;
            
        }

        public  void UpdateProfile(UserProfileEntity profile)
        {
            using (var db = new BotContext())
            {
                db.Update(profile);
                db.SaveChanges();
            }
        }

        public bool SaveNewUserProfile(ulong id)
        {
            using (var db = new BotContext())
            {
                var profile = new UserProfileEntity()
                {
                    DiscordId = id,
                };
                db.Add(profile);
                return db.SaveChanges() > 0;
            }
        }

        public void AddSpellEffect(List<ulong> userIds, string spellName, int duration)
        {
            using (var db = new BotContext())
            {
                foreach (var id in userIds)
                {
                    var effect = new ActiveEffectEntity()
                    {
                        DiscordId = id,
                        SpellName = spellName,
                        StartTime = DateTime.Now,
                        EndTime = DateTime.Now.AddHours(duration)
                    };
                    db.Add(effect);
                }
                db.SaveChanges();
            }
        }
    }
}
