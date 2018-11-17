using Discord;
using Discord.Commands;
using DiscordHex.Services;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordHex.Modules
{
    public class ProfileModule : ModuleBase<SocketCommandContext>
    {
        public ProfileService ProfileService { get; set; }

        [Command("profile")]
        [Summary("Show various stats for a user")]
        [Remarks("profile | profile @user")]
        public async Task Profile(IUser user = null)
        {
            if (user == null)
                user = Context.Message.Author;

            var wrapper = ProfileService.GetUserProfile(user.Id);
            
            var text = wrapper.Profile == null
                ? "Failed to find user profile."
                : $"Buffs given: {wrapper.Profile.BuffsCasted}\n" +
                  $"Buffs received: {wrapper.Profile.BuffsReceived}\n" +
                  $"Damage spells cast: {wrapper.Profile.DamageCasted}\n" +
                  $"Damage spells received: {wrapper.Profile.DamageReceived}\n" +
                  $"Hexes thrown: {wrapper.Profile.HexCasted}\n" +
                  $"Hexes received: {wrapper.Profile.HexReceived}\n" +
                  $"Games played: {wrapper.Profile.GamesStarted}\n" +
                  $"Active effects: {string.Join(", ", wrapper.Effects.Select(x => x.SpellName).Distinct())}";

            var emb = new EmbedBuilder()
                .WithTitle($"Stats for {user.Username}")
                .WithDescription(text)
                .Build();

            await ReplyAsync("", false, emb);
        }
    }
}
