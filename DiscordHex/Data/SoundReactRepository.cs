using DiscordHex.Core;
using DiscordHex.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Data
{
    public class SoundReactRepository
    {
        public List<SoundReactEntity> GetSoundReacts()
        {
            using (var db = new SoundReactContext())
            {
                return db.SoundReact.ToList();
            }
        }
    }

    public class SoundReactContext : DbContext
    {
        public virtual DbSet<SoundReactEntity> SoundReact { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(BotSettings.Instance.Config.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SoundReactEntity>(entity =>
            {
                entity.ToTable("SoundReacts", "RainBot");
                entity.Property(e => e.Guid).HasColumnName("guid");
                entity.Property(e => e.Url).HasColumnName("url");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Type).HasColumnName("type");
            });
        }
    }
}
