using DiscordHex.Core;
using DiscordHex.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Data
{
    public class SpellRepository
    {
        public List<SpellEntity> GetSpells()
        {
            using (var db = new SpellsContext())
            {
                return db.Spell.ToList();
            }
        }
    }

    public class SpellsContext : DbContext
    {
        public virtual DbSet<SpellEntity> Spell { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(BotSettings.Instance.Config.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpellEntity>(entity =>
            {
                entity.ToTable("Spells", "RainBot");
                entity.Property(e => e.Guid).HasColumnName("guid");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Type).HasColumnName("type");
            });
        }
    }
}
