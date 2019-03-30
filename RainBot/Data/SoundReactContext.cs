using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RainBot.Domain;

namespace RainBot.Data
{
    public class SoundReactContext : DbContext
    {
        public DbSet<SoundReactEntity> SoundReact { get; set; }

        public SoundReactContext(DbContextOptions<SoundReactContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SoundReactEntity>(ConfigureSoundReacts);
        }

        private void ConfigureSoundReacts(EntityTypeBuilder<SoundReactEntity> builder)
        {
            builder.ToTable("SoundReacts");
        }
    }
}
