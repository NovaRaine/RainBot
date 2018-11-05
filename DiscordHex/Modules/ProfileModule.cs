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
        [Summary("")]
        public async Task Profile(IUser user = null)
        {
            if (user == null)
                user = Context.Message.Author;

            var profile = ProfileService.GetUserProfile(user.Id);
            
            var text = profile == null
                ? "Failed to find user profile."
                : $"Buffs given: {profile.BuffsCasted}\n" +
                  $"Buffs received: {profile.BuffsReceived}\n" +
                  $"Damage spells casted: {profile.DamageCasted}\n" +
                  $"Damage spells received: {profile.DamageReceived}\n" +
                  $"Hexes thrown: {profile.HexCasted}\n" +
                  $"Hexes received: {profile.HexReceived}\n" +
                  $"Games played: {profile.GamesStarted}\n" +
                  $"Active effects: {string.Join(", ", profile.ActiveEffects.Select(x => x.SpellName))}";

            var emb = new EmbedBuilder()
                .WithTitle($"Stats for {user.Username}")
                .WithDescription(text)
                .Build();

            await ReplyAsync("", false, emb);
        }
    }
}
