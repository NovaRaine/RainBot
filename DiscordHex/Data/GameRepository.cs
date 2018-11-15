using Dapper;
using DiscordHex.Core;
using DiscordHex.Domain;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordHex.Data
{
    public class GameRepository
    {
        public IEnumerable<TreeItem<GameLocationEntity>> GetStoryArc(int storyId)
        {
            IEnumerable<GameLocationEntity> data;
            using (var conn = new NpgsqlConnection(BotSettings.Instance.Config.ConnectionString))
            {
                conn.Open();
                data = conn.Query<GameLocationEntity>(@"SELECT * FROM ""RainBot"".""GameLocations"" WHERE storyId = @storyId", new { storyId });
            }

            return BuildStory(data.ToList());
        }

        private IEnumerable<TreeItem<GameLocationEntity>> BuildStory(List<GameLocationEntity> gameEntities)
        {
            var root = gameEntities.GenerateTree(c => c.Guid, c => c.Parent);
            return root;
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
