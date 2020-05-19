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
        LavaNode lavanode;

        public CommandHandler(DiscordSocketClient Client)
        {
            _Client = Client;
            var services = new ServiceCollection()
                .AddSingleton(_Client)
                .AddSingleton<InteractiveService>()
                .AddSingleton<LavaNode>()
                .AddSingleton<LavaConfig>();

            provider = services.BuildServiceProvider();

            lavanode = provider.GetRequiredService<LavaNode>();

            _Service = new CommandService();
            _Service.AddModulesAsync(Assembly.GetEntryAssembly(), provider);
            lavanode.OnTrackEnded += TrackEnded;
            _Client.JoinedGuild += JoinGuild;
            _Client.MessageReceived += HandleCommandAsync;
            _Client.Ready += Ready;
        }

        public async Task JoinGuild(SocketGuild guild)
        {
            var embed = new EmbedBuilder();

            embed.WithTitle("Welcome to WeX!")
                .WithDescription("Thanks for adding! If you wanna know what can I do, use `wex help` command! \n\n Have an idea for command? Maybe found bug? Or just help bot grow? Please join to my Discord Support Server! \nhttps://discord.gg/jYhuRJC")
                .WithFooter("Sample Text, uh yes");

            await guild.TextChannels.First().SendMessageAsync(embed: embed.Build());
        }

        public async Task TrackEnded(TrackEndedEventArgs args)
        { 
            if (!args.Reason.ShouldPlayNext())
                return;
                            
            if (!args.Player.Queue.TryDequeue(out var queueable))
                return;

            if (!(queueable is LavaTrack track))
            {
                await args.Player.TextChannel.SendMessageAsync("Next item in queue is not a track.");
                return;
            }
            await args.Player.PlayAsync(track);
            await args.Player.TextChannel.SendMessageAsync($"Now Playing[{track.Title}]({track.Url})");
        }

        async Task Ready()
        {
            try
            {
                Console.WriteLine("Connecting to LavaLink...");
                await lavanode.ConnectAsync();
                await _Client.SetGameAsync("Use 'wex help'!");
                Console.WriteLine("Connected to LavaLink!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + " " + ex.Message);
            }
        }

        private async Task HandleCommandAsync(SocketMessage M)
        {
            var Message = M as SocketUserMessage;
            if (Message == null || Message.Author.IsBot) return;

            var context = new SocketCommandContext(_Client, Message);

            int argPos = 0;

            MainConfig config = SQLiteHandler.GetMessage(context.Guild.Id);
            string prefix = "wex ";
            if(!(config is null))
                prefix = config.prefix;

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
                        await context.Channel.SendMessageAsync("User not found");
                    else
                        await context.Channel.SendMessageAsync("Something went wrong.");
                }
            }
        }
    }
}
