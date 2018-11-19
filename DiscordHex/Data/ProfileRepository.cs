using DiscordHex.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Data
{
    public class ProfileRepository
    {
        private BotContext _dbContext;

        public ProfileRepository(BotContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserProfileWrapper GetUserProfile(ulong id)
        {
            var profile = new UserProfileWrapper();
            
            profile.Profile = _dbContext.UserProfiles.FirstOrDefault(x => x.DiscordId == id);

            if (profile.Profile == null)
            {
                SaveNewUserProfile(id);
                using (var db = new BotContext())
                {
                    profile.Profile = db.UserProfiles.FirstOrDefault(x => x.DiscordId == id);
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
            _dbContext.Update(profile);
            _dbContext.SaveChanges();
        }

        public bool SaveNewUserProfile(ulong id)
        {
            var profile = new UserProfileEntity()
            {
                DiscordId = id,
            };

            _dbContext.Add(profile);

            return _dbContext.SaveChanges() > 0;
        }

        public void AddSpellEffect(List<ulong> userIds, string spellName, int duration)
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
                _dbContext.Add(effect);
            }
            _dbContext.SaveChanges();
        }
    }
}
