using System.Collections.Generic;

namespace DiscordHex.Domain
{
    public class GameLocationEntity
    {
        public int Guid { get; set; }
        public string OptionTitle { get; set; }
        public string Description { get; set; }
        public int Parent { get; set; }
        public int StoryId { get; set; }
    }
}
