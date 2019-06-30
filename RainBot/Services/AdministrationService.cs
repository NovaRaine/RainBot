using Discord.WebSocket;
using RainBot.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RainBot.Services
{
    public class AdministrationService
    {
        private GuildConfigRepository _guildConfigRepository;
        private DiscordSocketClient _discordSocketClient;

        public AdministrationService(RainBotContext context, DiscordSocketClient discordSocketClient)
        {
            _guildConfigRepository = new GuildConfigRepository(context);
            _discordSocketClient = discordSocketClient;
        }

        public void SetPrefix(string prefix, ulong guildId)
        {
            _guildConfigRepository.SetConfig("Prefix", prefix, guildId);
        }
    }
}
