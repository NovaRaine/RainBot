using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RainBot.Domain;

namespace RainBot.Data
{
    public class RainBotContext : DbContext
    {
        public DbSet<SpellEntity> Spells { get; set; }
        public DbSet<SoundReactEntity> SoundReact { get; set; }

        public RainBotContext(DbContextOptions<RainBotContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SoundReactEntity>(ConfigureSoundReacts);
            builder.Entity<SpellEntity>(ConfigureSpells);
        }

        private void ConfigureSoundReacts(EntityTypeBuilder<SoundReactEntity> builder)
        {
            builder.ToTable("SoundReacts");
        }

        private void ConfigureSpells(EntityTypeBuilder<SpellEntity> builder)
        {
            builder.ToTable("Spells");
        }
    }
}
