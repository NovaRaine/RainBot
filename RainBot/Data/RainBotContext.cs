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
        public DbSet<SoundReactEntity> SoundReacts { get; set; }
        public DbSet<GuildConfigEntity> GuildConfigs { get; set; }

        public RainBotContext(DbContextOptions<RainBotContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SoundReactEntity>(SetupSoundReacts);
            builder.Entity<SpellEntity>(SetupSpells);
            builder.Entity<GuildConfigEntity>(SetupConfiguration);
        }

        private void SetupSoundReacts(EntityTypeBuilder<SoundReactEntity> builder)
        {
            builder.ToTable("SoundReacts");
        }

        private void SetupSpells(EntityTypeBuilder<SpellEntity> builder)
        {
            builder.ToTable("Spells");
        }
        private void SetupConfiguration(EntityTypeBuilder<GuildConfigEntity> builder)
        {
            builder.ToTable("GuildSpecificConfig");
        }
    }
}
