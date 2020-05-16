using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Database;
using System.Linq;
using Victoria;

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
            _client.UserLeft += AnnounceLeftUser;
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
        }

        public async Task AnnounceJoinedUser(SocketGuildUser user) 
        {
            if (SQLiteHandler.NoServer(user.Guild.Id))
                return;

            Messages mess = SQLiteHandler.GetMessage(user.Guild.Id, true);

            if (mess.ismessagesonline != "false")
            {
                SocketTextChannel x = user.Guild.GetChannel(mess.channelid) as SocketTextChannel;
                string final = mess.text;
                final = final.Replace("[user]", "<@" + user.Id + ">");
                final = final.Replace("[server]", user.Guild.ToString());
                await x.SendMessageAsync(final);
            }

            MainConfig main = SQLiteHandler.GetMessage(user.Guild.Id);

            if (user.Guild.Id == 0)
                return;

            if( user.Guild.GetRole(main.autoroleid) == null)
            {
                if (user.Guild.TextChannels.Count == 0)
                {
                    var owner = user.Guild.GetUser(user.Guild.OwnerId);
                    await owner.SendMessageAsync("Cannot give autorole. Please check if role exists or if I got permisions.");
                    return;
                }

                SocketTextChannel x = user.Guild.TextChannels.ToList()[0];
                await x.SendMessageAsync("Cannot give autorole. Please check if role exists or if I got permisions.");
                return;
            }

            await user.AddRoleAsync(user.Guild.GetRole(main.autoroleid));
        }

        public async Task AnnounceLeftUser(SocketGuildUser user)
        {
            if (SQLiteHandler.NoServer(user.Guild.Id))
                return;

            Messages mess = SQLiteHandler.GetMessage(user.Guild.Id, false);

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
