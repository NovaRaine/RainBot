using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RainBot.Core;
using RainBot.Domain;

namespace RainBot.Data
{
    public class BotContext : DbContext
    {
        public DbSet<SpellEntity> Spells { get; set; }
        public DbSet<SoundReactEntity> SoundReact { get; set; }
        public DbSet<GameLocationEntity> GaleLocation { get; set; }
        public DbSet<ActiveEffectEntity> ActiveEffects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(BotConfig.GetValue("ConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<GameLocationEntity>(ConfigureGameLocation);
            builder.Entity<SoundReactEntity>(ConfigureSoundReacts);
            builder.Entity<SpellEntity>(ConfigureSpells);
            builder.Entity<ActiveEffectEntity>(ConfigureActiveEffects);
        }

        private void ConfigureGameLocation(EntityTypeBuilder<GameLocationEntity> builder)
        {
            builder.ToTable("GameLocations", "RainBot");
            builder.Property(e => e.Guid).HasColumnName("guid");
            builder.Property(e => e.OptionTitle).HasColumnName("optiontitle");
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.Parent).HasColumnName("parent");
            builder.Property(e => e.StoryId).HasColumnName("storyid");
            builder.Property(e => e.ChapterId).HasColumnName("chapterid");
        }

        private void ConfigureSoundReacts(EntityTypeBuilder<SoundReactEntity> builder)
        {
            builder.ToTable("SoundReacts", "RainBot");
            builder.Property(e => e.Guid).HasColumnName("guid");
            builder.Property(e => e.Url).HasColumnName("url");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.Type).HasColumnName("type");
        }

        private void ConfigureSpells(EntityTypeBuilder<SpellEntity> builder)
        {
            builder.ToTable("Spells", "RainBot");
            builder.Property(e => e.Guid).HasColumnName("guid");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.Type).HasColumnName("type");
        }

        private void ConfigureActiveEffects(EntityTypeBuilder<ActiveEffectEntity> builder)
        {
            builder.ToTable("ActiveEffects", "RainBot");
            builder.Property(e => e.Guid).HasColumnName("guid");
            builder.Property(e => e.DiscordId).HasColumnName("discordid");
            builder.Property(e => e.SpellName).HasColumnName("spellname");
            builder.Property(e => e.StartTime).HasColumnName("starttime");
            builder.Property(e => e.EndTime).HasColumnName("endtime");
        }
    }
}