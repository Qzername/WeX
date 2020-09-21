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

        [Command("invite")]
        public async Task Invite()
        {
            await ReplyAsync("Here is my invite: https://discord.com/api/oauth2/authorize?client_id=665514955985911818&permissions=8&scope=bot");
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
                .AddField("Oficial Discord:", "https://discord.gg/sWzvgZB")
                .WithFooter("Bot version: 0.6v")
                .WithColor(Color.Green);

            await Context.Channel.SendMessageAsync("", false, bud.Build());
        }

        [Command("wexstatus")]
        public async Task WexStatus()
        {
            if (Context.User.Id != 273438920870461441)
                return;

            var embed = new EmbedBuilder();

            int people = 0, realpeople =0;

            foreach(SocketGuild x in Context.Client.Guilds)
                people += x.Users.Count;

            foreach (SocketGuild x in Context.Client.Guilds)
                if (x.Id != 446425626988249089 || x.Id != 264445053596991498 || x.Id != 110373943822540800)
                    realpeople += x.Users.Count;

            embed.WithTitle("Wex is current status")
                .AddField("Servers:", Context.Client.Guilds.Count)
                .AddField("Users:", people)
                .AddField("Users, not including dlist stuff:", realpeople)
                .AddField("Latency:",Context.Client.Latency);

            await ReplyAsync(embed: embed.Build());
        }
    }
}