using DiscordHex.Core;
using DiscordHex.Domain;
using Microsoft.EntityFrameworkCore;
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
            
            using (var db = new UserProfileContext())
            {
                profile.Profile = db.UserProfiles.Where(x => x.DiscordId == id).FirstOrDefault();
            }

            if (profile.Profile == null)
            {
                SaveNewUserProfile(id);
                using (var db = new UserProfileContext())
                {
                    profile.Profile = db.UserProfiles.Where(x => x.DiscordId == id).FirstOrDefault();
                }
            }

            if (profile.Profile != null)
            {
                using (var db = new ActiveEffectsContext())
                {
                    profile.Effects = db.ActiveEffects.Where(x => x.DiscordId == id).ToList();
                }
            }

            return profile;
            
        }

        public  void UpdateProfile(UserProfileEntity profile)
        {
            using (var db = new UserProfileContext())
            {
                db.Update(profile);
                db.SaveChanges();
            }
        }

        public bool SaveNewUserProfile(ulong id)
        {
            using (var db = new UserProfileContext())
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
            using (var db = new ActiveEffectsContext())
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

    public class UserProfileContext : DbContext
    {
        public virtual DbSet<UserProfileEntity> UserProfiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(BotSettings.Instance.Config.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfileEntity>(entity =>
            {
                entity.ToTable("UserProfiles", "RainBot");
                entity.Property(e => e.Guid).HasColumnName("guid");
                entity.Property(e => e.DiscordId).HasColumnName("discordid");
                entity.Property(e => e.BuffsCasted).HasColumnName("buffscasted");
                entity.Property(e => e.BuffsReceived).HasColumnName("buffsreceived");
                entity.Property(e => e.DamageCasted).HasColumnName("damagecasted");
                entity.Property(e => e.DamageReceived).HasColumnName("damagereceived");
                entity.Property(e => e.HexCasted).HasColumnName("hexcasted");
                entity.Property(e => e.HexReceived).HasColumnName("hexreceived");
                entity.Property(e => e.GamesStarted).HasColumnName("gamesstarted");
            });
        }
    }

    public class ActiveEffectsContext : DbContext
    {
        public virtual DbSet<ActiveEffectEntity> ActiveEffects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(BotSettings.Instance.Config.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActiveEffectEntity>(entity =>
            {
                entity.ToTable("ActiveEffects", "RainBot");
                entity.Property(e => e.Guid).HasColumnName("guid");
                entity.Property(e => e.DiscordId).HasColumnName("discordid");
                entity.Property(e => e.SpellName).HasColumnName("spellname");
                entity.Property(e => e.StartTime).HasColumnName("starttime");
                entity.Property(e => e.EndTime).HasColumnName("endtime");
            });
        }
    }
}
