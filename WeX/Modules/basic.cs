using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeX.Modules
{
    public class Basic : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task Help()
        {
            var basic = new EmbedBuilder();
            var fun = new EmbedBuilder();
            var utilites = new EmbedBuilder();
            var moderation = new EmbedBuilder();

            basic.WithTitle("BASIC module")
            .AddField("help", "list of commands")
            .AddField("info", "information about bot")
            .AddField("ping", "Pong!")
            .WithFooter(footer => footer.Text = "use 'wex ' before any command!")
            .WithColor(Color.Blue)
            .Build();

            fun.WithTitle("FUN module")
            .AddField("say", "I will say what you want me to say")
            .AddField("dice", "rolling the dice")
            .AddField("8ball", "Ask the magical 8ball a question, and get an answer.")
            .AddField("computer", "Ping an user and she/he will be in computer.")
            .AddField("toilet", "Ping an user and she/he will be in toilet.")
            .AddField("cat", "Sends random cat picture")
            .AddField("dog", "Sends random dog picture")
            .AddField("bird", "Sends random bird picture")
            .AddField("rps", "Rock, paper, scissors!")
            .AddField("lenny", "Sends lennyface")
            .AddField("spoiler", "It will spoiler text that you write down")
            .AddField("emojileters", "It will change your text into emoji")
            .AddField("hug", "Hug a person!")
            .AddField("lovemeter", "Check in what procent two person are loving each other")
            .AddField("drake", "Drake meme generator, ping two persons.")
            .AddField("pickle", "Ping an user and she/he will be in pickle.")
            .AddField("food", "Sends random food picture")
            .WithFooter(footer => footer.Text = "use 'wex ' before any command!")
            .WithColor(Color.Blue);

            utilites.WithTitle("UTILITES module")
            .AddField("add", "I will add numbers and show result")
            .AddField("minus", "I will minus numbers and show result")
            .AddField("multiply", "I will multiply numbers and show result")
            .AddField("divide", "I will devide numbers and show result")
            .AddField("root", "I will get root of number and show result")
            .AddField("square", "I will get square of number and show result")
            .AddField("power", "I will get power (that you write after number) of number and show result")
            .AddField("min", "I will get the smallest of numbers you write me down and show result")
            .AddField("max", "I will get the biggest of numbers you write me down and show result")
            .AddField("avg", "I will get average of numbers you write me down and show result")
            .AddField("avatar", "Mention a user and I will get that user avatar URL!")
            .AddField("idinfo", "Sends serverID, channelID and userID")
            .AddField("mal", "Searches for user by nickname on MyAnimeList")
            .AddField("anime", "Searches for anime by its name")
            .WithFooter(footer => footer.Text = "use 'wex ' before any command!")
            .WithColor(Color.Blue);

            moderation.WithTitle("MODERATION module")
            .AddField("clear", "Clears the amount you give messages")
            .AddField("kick", "Kick an user")
            .AddField("ban", "Bans an user and deletes all user messages written in this week")
            .WithFooter(footer => footer.Text = "use 'wex ' before any command!")
            .WithColor(Color.Blue);

            await Context.User.SendMessageAsync("", false, basic.Build());
            await Context.User.SendMessageAsync("", false, fun.Build());
            await Context.User.SendMessageAsync("", false, utilites.Build());
            await Context.User.SendMessageAsync("", false, moderation.Build());
            await Context.Channel.SendMessageAsync("Check your DM!");
        }

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
                .AddField("Oficial Discord:", "https://discord.gg/bwPspBJ")
                .WithFooter("Bot version: 1.0v, API version: 1.0v");

            await Context.Channel.SendMessageAsync("", false, bud.Build());
        }
    }
}