using Discord.Commands;
using Discord;
using System.Threading.Tasks;

namespace WeX.Modules
{
    public class Basic : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("Pong!");
        }

        [Command("info")]
        public async Task Info()
        {
            var bud = new EmbedBuilder();

            bud.WithTitle("Thank you for adding this bot!")
                .WithAuthor(new EmbedAuthorBuilder() { Name = "Uzer", IconUrl = "https://cdn.discordapp.com/attachments/281857417564651521/689474313409396796/1011200ac4bfc854e7330f87340ac69fc1f38fc7_full.png" })
                .WithDescription("If you want contact with me in order to suggest new commands or troubleshooting here is info:")
                .AddField("Author: ", "Uzer#9084")
                .AddField("Special thanks: ", "Blasstah#5656")
                .AddField("Oficial Discord:", "https://discord.gg/jYhuRJC")
                .WithFooter("Bot version: 0.3v");

            await Context.Channel.SendMessageAsync("", false, bud.Build());
        }
    }
}