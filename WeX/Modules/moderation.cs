using Database;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Runtime.InteropServices;
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

        #region Welcome messages
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [Command("welcomemessage")]
        public async Task WelcomeMessage()
        {
            if (SQLiteHandler.NoServer(Context.Guild.Id))
                SQLiteHandler.NewServer(Context.Guild.Id);

            Messages welcome = SQLiteHandler.GetMessage(Context.Guild.Id, true);

            var embed = new EmbedBuilder();

            string noyes;
            if (welcome.ismessagesonline == "true")
                noyes = "Yes";
            else
                noyes = "No";

            string channel;
            if (welcome.channelid == 0)
                channel = "No channel";
            else
                channel = "<#" + welcome.channelid.ToString() + ">";


            embed.WithTitle("Welcome messages")
                .AddField("­­ ", "Special Commands:")
                .AddField("Set on/off", "welcomemessage [on/off]", true)
                .AddField("Set welcome channel", "welcomechannel", true)
                .AddField("Set welcome message", "welcometext", true)
                .AddField("­­ ", "Status:")
                .AddField("Is welcome message online?", noyes, true)
                .AddField("Welcome channel", channel, true)
                .AddField("Welcome text", welcome.text, true)
                .WithColor(Color.Green);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [Command("welcometext")]
        public async Task Welcometext([Remainder]string text)
        {
            if (SQLiteHandler.NoServer(Context.Guild.Id))
                SQLiteHandler.NewServer(Context.Guild.Id);

            Messages mess = SQLiteHandler.GetMessage(Context.Channel.Id, true);
            mess.text = text;
            SQLiteHandler.Update(mess, true, Context.Channel.Id);

            await Context.Channel.SendMessageAsync("New welcome text has been set");
        }

        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [Command("welcomechannel")]
        public async Task Welcomechannel(IGuildChannel channel)
        {
            if (SQLiteHandler.NoServer(Context.Guild.Id))
                SQLiteHandler.NewServer(Context.Guild.Id);

            try
            {
                Context.Guild.GetChannel(channel.Id);
            }
            catch(Exception)
            {
                await Context.Channel.SendMessageAsync("Channel doesn't exist");
                return;
            }

            Messages mess = SQLiteHandler.GetMessage(Context.Channel.Id, true);
            mess.channelid = channel.Id;
            SQLiteHandler.Update(mess, true, Context.Channel.Id);

            await Context.Channel.SendMessageAsync("New welcome channel has been set");
        }

        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [Command("welcomemessage")]
        public async Task Welcomemessage(string status)
        {
            if (SQLiteHandler.NoServer(Context.Guild.Id))
                SQLiteHandler.NewServer(Context.Guild.Id);

            Messages mess;
            mess = SQLiteHandler.GetMessage(Context.Guild.Id, true);

            if(mess.channelid == 0)
            {
                await Context.Channel.SendMessageAsync("Please, set Welcome Channel firstly by using welcomechannel command");
                return;
            }

            if(status.ToLower() == "on")
            {
                await Context.Channel.SendMessageAsync("Welcome Messages changed to ON");
                mess.ismessagesonline = "true";
            }
            else if(status.ToLower() == "off")
            {
                await Context.Channel.SendMessageAsync("Welcome Messages changed to OFF");
                mess.ismessagesonline = "false";
            }
            else
            {
                await Context.Channel.SendMessageAsync("Invalid entry.");
                return;
            }

            SQLiteHandler.Update(mess, true, Context.Guild.Id);
        }
        #endregion

        #region Bye messages
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [Command("byemessage")]
        public async Task ByeMessage()
        {
            if (SQLiteHandler.NoServer(Context.Guild.Id))
                SQLiteHandler.NewServer(Context.Guild.Id);

            Messages welcome = SQLiteHandler.GetMessage(Context.Guild.Id, false);

            var embed = new EmbedBuilder();

            string noyes;
            if (welcome.ismessagesonline == "true")
                noyes = "Yes";
            else
                noyes = "No";

            string channel;
            if (welcome.channelid == 0)
                channel = "No channel";
            else
                channel = "<#" + welcome.channelid.ToString() + ">";


            embed.WithTitle("Bye messages")
                .AddField("­­ ", "Special Commands:")
                .AddField("Set on/off", "byemessage [on/off]", true)
                .AddField("Set bye channel", "welcomechannel", true)
                .AddField("Set bye message", "welcometext", true)
                .AddField("­­ ", "Status:")
                .AddField("Is bye message online?", noyes, true)
                .AddField("Bye channel", channel, true)
                .AddField("Bye text", welcome.text, true)
                .WithColor(Color.Green);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [Command("byetext")]
        public async Task Byetext([Remainder]string text)
        {
            if (SQLiteHandler.NoServer(Context.Guild.Id))
                SQLiteHandler.NewServer(Context.Guild.Id);

            Messages mess = SQLiteHandler.GetMessage(Context.Channel.Id, false);
            mess.text = text;
            SQLiteHandler.Update(mess, false, Context.Channel.Id);

            await Context.Channel.SendMessageAsync("New bye text has been set");
        }

        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [Command("byechannel")]
        public async Task Byechannel(IGuildChannel channel)
        {
            if (SQLiteHandler.NoServer(Context.Guild.Id))
                SQLiteHandler.NewServer(Context.Guild.Id);

            try
            {
                Context.Guild.GetChannel(channel.Id);
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Channel doesn't exist");
                return;
            }

            Messages mess = SQLiteHandler.GetMessage(Context.Channel.Id, false);
            mess.channelid = channel.Id;
            SQLiteHandler.Update(mess,false, Context.Channel.Id);

            await Context.Channel.SendMessageAsync("New bye channel has been set");
        }

        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [Command("byemessage")]
        public async Task Byemessage(string status)
        {
            if (SQLiteHandler.NoServer(Context.Guild.Id))
                SQLiteHandler.NewServer(Context.Guild.Id);

            Messages mess;
            mess = SQLiteHandler.GetMessage(Context.Guild.Id, false);

            if (mess.channelid == 0)
            {
                await Context.Channel.SendMessageAsync("Please, set Bye Channel firstly by using byechannel command");
                return;
            }

            if (status.ToLower() == "on")
            {
                await Context.Channel.SendMessageAsync("Bye Messages changed to ON");
                mess.ismessagesonline = "true";
            }
            else if (status.ToLower() == "off")
            {
                await Context.Channel.SendMessageAsync("Bye Messages changed to OFF");
                mess.ismessagesonline = "false";
            }
            else
            {
                await Context.Channel.SendMessageAsync("Invalid entry.");
                return;
            }

            SQLiteHandler.Update(mess, false, Context.Guild.Id);
        }
        #endregion
    }
}
