using DiscordHex.Core;
using DiscordHex.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Data
{
    public class GameRepository
    {
        public IEnumerable<TreeItem<GameLocationEntity>> GetStoryArc(int storyId)
        {
            using (var db = new GameContext())
            {
                var data = db.GaleLocation.Where(x => x.StoryId == storyId).ToList();
                return BuildStory(data.ToList());
            }
        }

        private IEnumerable<TreeItem<GameLocationEntity>> BuildStory(List<GameLocationEntity> gameEntities)
        {
            var root = gameEntities.GenerateTree(c => c.Guid, c => c.Parent);
            return root;
        }
    }

    public class GameContext : DbContext
    {
        public virtual DbSet<GameLocationEntity> GaleLocation { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(BotSettings.Instance.Config.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameLocationEntity>(entity =>
            {
                entity.ToTable("GameLocations", "RainBot");
                entity.Property(e => e.Guid).HasColumnName("guid");
                entity.Property(e => e.OptionTitle).HasColumnName("optiontitle");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Parent).HasColumnName("parent");
                entity.Property(e => e.StoryId).HasColumnName("storyid");
                entity.Property(e => e.ChapterId).HasColumnName("chapterid");
            });
        }
    }

    public class TreeItem<T>
    {
        public T Item { get; set; }
        public IEnumerable<TreeItem<T>> Children { get; set; }
    }

    internal static class GenericHelpers
    {
        public static IEnumerable<TreeItem<T>> GenerateTree<T, TK>(
            this IEnumerable<T> collection,
            Func<T, TK> idSelector,
            Func<T, TK> parentIdSelector,
            TK rootId = default(TK))
        {
            return collection.Where(c => parentIdSelector(c).Equals(rootId)).Select(c => new TreeItem<T>
            {
                Item = c,
                Children = collection.GenerateTree(idSelector, parentIdSelector, idSelector(c))
            });
        }
    }
}
