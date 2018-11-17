
namespace DiscordHex.Domain
{
    public class BotConfigurationEntity
    {
        public string ConnectionString { get; set; }
        public string Prefix { get; set; }
        public string DiscordToken { get; set; }
        public string DiscordTokenDebug { get; set; }
        public string GiphyToken { get; set; }
    }
}
