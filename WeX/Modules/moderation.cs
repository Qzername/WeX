using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeX.Modules
{
    public class Moderation : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task Kick(IGuildUser user)
        {
            await user.KickAsync();
            await Context.Channel.SendMessageAsync("**" + user.ToString() + "** has been kicked by: **" + Context.User.ToString()+ "**");
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task Ban(IGuildUser user)
        {
            await user.BanAsync(7);
            await Context.Channel.SendMessageAsync("**" + user.ToString() + "** has been banned by: **" + Context.User.ToString() + "**");
        }

        [Command("clear")]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task Purge(string number)
        {
            int total;
            try
            {
                total = Convert.ToInt32(number);

                if(total < 1)
                {
                    await Context.Channel.SendMessageAsync("Hey! The amount of messages to remove must be positive!");
                    return;
                }
            }
            catch(Exception)
            {
                await Context.Channel.SendMessageAsync("Hey! I don't know how much messages I need to delete!");
                return;
            }

            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(total + 1).FlattenAsync();
            await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
            const int delay = 3000;
            IUserMessage m = await ReplyAsync($"I have deleted {total} messages :)");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }

    }  
}
