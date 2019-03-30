using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RainBot.Domain;

namespace RainBot.Data
{
    public class SpellsContext : DbContext
    {
        public DbSet<SpellEntity> Spells { get; set; }

        public SpellsContext(DbContextOptions<SpellsContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SpellEntity>(ConfigureSpells);
        }

        private void ConfigureSpells(EntityTypeBuilder<SpellEntity> builder)
        {
            builder.ToTable("Spells");
        }
    }
}
