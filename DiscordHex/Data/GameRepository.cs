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
            IEnumerable<GameLocationEntity> data = null;
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
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
            this IEnumerable<T> collection,
            Func<T, K> id_selector,
            Func<T, K> parent_id_selector,
            K root_id = default(K))
        {
            foreach (var c in collection.Where(c => parent_id_selector(c).Equals(root_id)))
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
                };
            }
        }
    }
}
