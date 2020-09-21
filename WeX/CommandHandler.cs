using System;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using Microsoft.Extensions.DependencyInjection;
using Database;
using Victoria;
using Victoria.EventArgs;
using System.Linq;
using Discord;

namespace WeX
{
    public class CommandHandler
    {
        private CommandService _Service;
        private DiscordSocketClient _Client;
        IServiceProvider provider;

        public CommandHandler(DiscordSocketClient Client)
        {
            _Client = Client;
            var services = new ServiceCollection()
                .AddSingleton(_Client)
                .AddSingleton<InteractiveService>();

            provider = services.BuildServiceProvider();

            _Service = new CommandService();
            _Service.AddModulesAsync(Assembly.GetEntryAssembly(), provider);
            _Client.JoinedGuild += JoinGuild;
            _Client.MessageReceived += HandleCommandAsync;

            _Client.SetGameAsync("Use 'wex help'!");
        }

        public async Task JoinGuild(SocketGuild guild)
        {
            var embed = new EmbedBuilder();

            embed.WithTitle("Welcome to WeX!")
                .WithDescription("Thanks for adding! If you wanna know what can I do, use `wex help` command! \n\n Have an idea for command? Maybe found bug? Or just help bot grow? Please join to my Discord Support Server! \nhttps://discord.gg/jYhuRJC")
                .WithFooter("Sample Text, uh yes");

            await guild.TextChannels.First().SendMessageAsync(embed: embed.Build());
        }

        private async Task HandleCommandAsync(SocketMessage M)
        {
            var Message = M as SocketUserMessage;
            if (Message == null || Message.Author.IsBot) return;

            var context = new SocketCommandContext(_Client, Message);

            int argPos = 0;

            MainConfig config = SQLiteHandler.GetMessage(context.Guild.Id);
            string prefix = "wex ";

            if (!(config is null))
                prefix = config.prefix;

            if (M.Author.Id == 285031189956263936)
                M.Channel.SendMessageAsync("ok");

            if (Message.HasStringPrefix("wex ", ref argPos) || Message.HasStringPrefix(prefix, ref argPos) || Message.HasMentionPrefix(_Client.CurrentUser, ref argPos))
            {
                var result = await _Service.ExecuteAsync(context, argPos, provider);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    if(result.Error == CommandError.BadArgCount)
                        await context.Channel.SendMessageAsync("You wrote too much or too less arguments!");
                    else if(result.Error == CommandError.UnmetPrecondition)
                        await context.Channel.SendMessageAsync("You (or I) do not have permission to be able to do this command");
                    else if(result.Error == CommandError.ObjectNotFound)
                        await context.Channel.SendMessageAsync("User/Role not found");
                    else
                        await context.Channel.SendMessageAsync("Something went wrong.");
                        
                }
            }
        }
    }
}
