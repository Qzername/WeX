using System;
using System.Collections.Generic;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace WeX
{
    public class CommandHandler
    {
        private CommandService _Service;
        private DiscordSocketClient _Client;
        private IServiceProvider Services;

        public CommandHandler(DiscordSocketClient Client)
        {
            _Client = Client;
            _Service = new CommandService();
            _Service.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
            _Client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage M)
        {
            var Message = M as SocketUserMessage;
            if (Message == null || Message.Author.IsBot) return;

            var context = new SocketCommandContext(_Client, Message);

            int argPos = 0;
            if (Message.HasStringPrefix("wex ", ref argPos))
            {
                var result = await _Service.ExecuteAsync(context, argPos, Services);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    if(result.Error == CommandError.BadArgCount)
                        await context.Channel.SendMessageAsync("You wrote too much or too less arguments!");
                    else if(result.Error == CommandError.UnmetPrecondition)
                        await context.Channel.SendMessageAsync("You (or I) do not have permission to be able to do this command");
                    else if(result.Error == CommandError.ObjectNotFound)
                        await context.Channel.SendMessageAsync("User not found");
                    else
                        await context.Channel.SendMessageAsync(result.ErrorReason +  result.Error.ToString());
                }
            }
        }
    }
}
