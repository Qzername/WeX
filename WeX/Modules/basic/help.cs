using Discord.Commands;
using Discord;
using System.Threading.Tasks;
using Database;

namespace WeX.Modules
{
    public class BasicHelp : ModuleBase<SocketCommandContext>
    {
        Command[] commands;

        public BasicHelp() => commands = JSONhandler.GetCommands();

        [Command("help")]
        public async Task Help()
        {
            var commands = new EmbedBuilder();

            commands.WithTitle("Available commands on WeX")
                .WithDescription("Use 'wex help [command]' to know more about command.")
                .AddField("Basic:", "help, info, ping, invite")
                .AddField("Fun:", "say, dice, 8ball, computer, toilet, cat, dog, bird, rps, lenny, spoiler, emojileters, hug, lovemeter, drake, pickle, food, reverse, coinflip, random, meme, fox, howgay, choose, joke, keanu, tobecontinued, washer, animalfact, cookie, f, pat, kiss, wasted, potato, howhorny, pixel, slap, howsimp, wink, marriage, care, shibe, russianroulette, joke")
                .AddField("Utilites:", "add, minus, multiply, devide, root, square, power, min, max, avg, avatar, idinfo, mal, anime, crypt base64, replace, serverstats")
                .AddField("Moderation:", "clear, kick, ban, softban, hackban, welcomemessage, byemessage, mute, unmute, setmute, autorole, autoroleclear, prefix, clearprefix")
                .AddField("Music:", "join, play, leave, stop, skip, queue, volume, pause, resume")
                .WithFooter("Default prefix: 'wex ' (remember to add prefix before commands), to check server prefix use 'wex prefix'")
                .WithColor(Color.Blue);

            await Context.User.SendMessageAsync("", false, commands.Build());
            await Context.Channel.SendMessageAsync("Check your DM! :mailbox:");
        }

        [Command("help")]
        public async Task Help([Remainder]string Command)
        {
            foreach(Command x in commands)
            {
                if(Command.ToLower() == x.command)
                {
                    var embed = new EmbedBuilder();

                    if (x.secone == "null")
                        embed.WithTitle("Help command - " + x.command)
                            .WithDescription(x.firstone + "\nUsage: " + x.firstusage)
                            .WithFooter("Remember to add 'wex ' before any command!")
                            .WithColor(Color.Green);
                    else
                        embed.WithTitle("Help command - " + x.command)
                            .WithDescription(x.firstone + "\nUsage: " + x.firstusage+"\n \n" + x.secone + "\nUsage: " +x.secusage)
                            .WithFooter("Remember to add 'wex ' before any command!")
                            .WithColor(Color.Green);

                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                    return;
                }
            }

            await Context.Channel.SendMessageAsync("Command not found.");
        }
    }
}
