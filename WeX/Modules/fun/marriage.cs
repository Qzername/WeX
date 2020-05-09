using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System;
using Database;
using System.ComponentModel.Design;
using Discord.Addons.Interactive;
using System.Net.Sockets;
using static Database.SQLiteHandler;

namespace WeX.Modules
{
    public class MarriageCommands : InteractiveBase
    {
        [Command("marriage")]
        public async Task Marriage()
        {
            if (SQLiteHandler.Marriage.NoInMarriage(Context.Guild.Id, Context.User.Id))
                SQLiteHandler.Marriage.NewPerson(Context.Guild.Id, Context.User.Id);

            MarriageItem item = SQLiteHandler.Marriage.GetMarriage(Context.Guild.Id, Context.User.Id);

            string status, marriedwith, marrieddate;

            if (item.user2id == 0)
            {
                status = "Single";
                marriedwith = "None";
                marrieddate = "None";
            }
            else
            {
                status = "Married";
                marrieddate = item.date;

                if (Context.Guild.GetUser(item.user2id) != null)
                    marriedwith = Context.Guild.GetUser(item.user2id).ToString();
                else
                {
                    await Context.Channel.SendMessageAsync("User, you was married with, is no more on this server... Use again this command.");
                    SQLiteHandler.Marriage.BrokeMarriage(Context.Guild.Id, Context.User.Id, item.user2id);
                    return;
                }
            }

            var embed = new EmbedBuilder();

            embed.WithTitle("Server Marriage - " + Context.User.ToString())
                .AddField("­­ ", "Info:")
                .AddField("Status:", status, true)
                .AddField("Married with:", marriedwith, true)
                .AddField("Marriage date:", marrieddate, true)
                .AddField("­­­ ", "Marriage Commands:")
                .AddField("Request marriage", "marriage request", true)
                .AddField("Accept marriage", "marriage accept", true)
                .AddField("Cancel marriage", "cancel marriage", true)
                .WithColor(Color.Blue);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("marriage")]
        public async Task Marriage(IGuildUser user)
        {
            if (SQLiteHandler.Marriage.NoInMarriage(Context.Guild.Id, user.Id))
                SQLiteHandler.Marriage.NewPerson(Context.Guild.Id, user.Id);

            MarriageItem item = SQLiteHandler.Marriage.GetMarriage(Context.Guild.Id, user.Id);

            string status, marriedwith, marrieddate;

            if (item.user2id == 0)
            {
                status = "Single";
                marriedwith = "None";
                marrieddate = "None";
            }
            else
            {
                status = "Married";
                marrieddate = item.date;

                if (Context.Guild.GetUser(item.user2id) != null)
                    marriedwith = Context.Guild.GetUser(item.user2id).ToString();
                else
                {
                    await Context.Channel.SendMessageAsync("User, you was married with, is no more on this server... Use again this command.");
                    SQLiteHandler.Marriage.BrokeMarriage(Context.Guild.Id, Context.User.Id, item.user2id);
                    return;
                }
            }

            var embed = new EmbedBuilder();

            embed.WithTitle("Server Marriage - " +user.ToString())
                .AddField("­­ ", "Info:")
                .AddField("Status:", status, true)
                .AddField("Married with:", marriedwith, true)
                .AddField("Marriage date:", marrieddate, true)
                .AddField("­­­ ", "Marriage Commands:")
                .AddField("Request marriage", "marriage request", true)
                .AddField("Accept marriage", "marriage accept", true)
                .AddField("Cancel marriage", "cancel marriage", true)
                .WithColor(Color.Blue);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("marriage request", RunMode = RunMode.Async)]
        public async Task MarriageRequest(IGuildUser user)
        {
            if (SQLiteHandler.Marriage.NoInMarriage(Context.Guild.Id, Context.User.Id))
                SQLiteHandler.Marriage.NewPerson(Context.Guild.Id, Context.User.Id);

            if (user.Id == Context.User.Id)
            {
                await ReplyAsync("You can't marry yourself...");
                return;
            }
            else if (user.Id == 665514955985911818)
            {
                await ReplyAsync("You can't marry me...");
                return;
            }
            else if (user.IsBot)
            {
                await ReplyAsync("You can't marry bot...");
                return;
            }

             MarriageItem marr = SQLiteHandler.Marriage.GetMarriage(Context.Guild.Id, Context.User.Id);

            if(marr.user2id != 0)
            {
                await ReplyAsync("You are already married...");
                return;
            }

            await ReplyAsync(user.Mention + ", You have 15 secounds from now to say 'wex yes' to accept marriage! *If you don't want to say 'wex no'...* Btw. nobody can't write in this time or It will be canceled");
            var response = await NextMessageAsync(false); 

            if (response != null)
            {
                if (response.Author.Id == user.Id && response.Content.ToLower() == "wex yes")
                {
                    await ReplyAsync(user.Mention + " has accepted marriage! " + Context.User.Mention + " and " + user.Mention + " are now marriage! Congratulations!");
                    SQLiteHandler.Marriage.NewMarriage(Context.Guild.Id, Context.User.Id, response.Author.Id);
                }
                else if (response.Author.Id == user.Id && response.Content.ToLower() == "wex no")
                    await ReplyAsync(user.Mention + " just refused marriage request...");
                else if (response.Author.Id == user.Id)
                    await ReplyAsync("Wrong message...");
                else
                    await ReplyAsync(response.Author.Mention + " just broke " + Context.User.Mention + " request...");
            }
            else
                await ReplyAsync("You did not reply before the timeout. Marriage canceled.");

        }

        [Command("cancel marriage")]
        public async Task MarriageCancel()
        {
            if (SQLiteHandler.Marriage.NoInMarriage(Context.Guild.Id, Context.User.Id))
                SQLiteHandler.Marriage.NewPerson(Context.Guild.Id, Context.User.Id);

            MarriageItem item = SQLiteHandler.Marriage.GetMarriage(Context.Guild.Id, Context.User.Id);

            if(item.user2id == 0)
            {
                await Context.Channel.SendMessageAsync("You haven't marriage yet...");
                return;
            }

            if (Context.Guild.GetUser(item.user2id) != null)
            {
                var user = Context.Guild.GetUser(item.user2id);
                await Context.Channel.SendMessageAsync(Context.User.ToString() + " just broke marriage with " + user.Mention);
                SQLiteHandler.Marriage.BrokeMarriage(Context.Guild.Id, Context.User.Id, item.user2id);
            }
            else
            {
                await Context.Channel.SendMessageAsync("User, you was married with, is no more on this server... Use again this command.");
                SQLiteHandler.Marriage.BrokeMarriage(Context.Guild.Id, Context.User.Id, item.user2id);
                return;
            }
        }
    }
}
