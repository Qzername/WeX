using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Database;

namespace WeX
{
    class Program
    {
        private readonly DiscordSocketClient _client;
        private CommandHandler comm;
        private readonly CommandService service;

        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public Program()
        {
            _client = new DiscordSocketClient();
            service = new CommandService();
            comm = new CommandHandler(_client);
            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += MessageReceivedAsync;
            _client.UserJoined += AnnounceJoinedUser;
        }

        public async Task MainAsync()
        {
            Config x = (Config)JSONhandler.GetElement(JsonFile.config, 0);

            await _client.LoginAsync(TokenType.Bot, x.token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(SocketMessage message)
        {
            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            if (message.Content == "wex sex")
                await message.Channel.SendMessageAsync("That's not funny");
            else if (message.Content == "wex cringe")
                await message.Channel.SendMessageAsync("no u");
            else if (message.Content == "wex welcomemessages")
                await message.Channel.SendMessageAsync("You made mistake, It's wex welcomemessage not wex welcomemessages");
        }

        public async Task AnnounceJoinedUser(SocketGuildUser user) 
        {
            if (SQLiteHandler.NoServer(user.Guild.Id))
                return;

            Messages mess = SQLiteHandler.GetMessage(user.Guild.Id, true);

            if (mess.ismessagesonline == "false")
                return;

            SocketTextChannel x = user.Guild.GetChannel(mess.channelid) as SocketTextChannel;
            string final = mess.text;
            final = final.Replace("[user]", "<@" + user.Id + ">");
            final = final.Replace("[server]", user.Guild.ToString());
            await x.SendMessageAsync(final);
        }
    }
}
