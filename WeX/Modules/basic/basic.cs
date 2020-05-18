using Discord.Commands;
using Discord;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace WeX.Modules
{
    public class Basic : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("Pong! Latency: " + Context.Client.Latency);
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
                .WithFooter("Bot version: 0.4v");

            await Context.Channel.SendMessageAsync("", false, bud.Build());
        }

        [Command("wexstatus")]
        public async Task WexStatus()
        {
            if (Context.User.Id != 273438920870461441)
                return;

            var embed = new EmbedBuilder();

            int people = 0;

            foreach(SocketGuild x in Context.Client.Guilds)
                people += x.Users.Count;

            embed.WithTitle("Wex is current status")
                .AddField("Servers:", Context.Client.Guilds.Count)
                .AddField("Users:", people)
                .AddField("Latency:",Context.Client.Latency);


            await ReplyAsync(embed: embed.Build());
        }
    }
}