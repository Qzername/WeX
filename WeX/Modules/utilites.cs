using Database;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeX.Modules
{
    public class Utilities : ModuleBase<SocketCommandContext>
    {
        #region Math
        [Command("add")]
        public async Task Add(string ones, string twos)
        {
            float one, two;

            try
            {
                one = Convert.ToSingle(ones);
                two = Convert.ToSingle(twos);
                await Context.Channel.SendMessageAsync("The end result is: " + (one + two).ToString());
            }
            catch(Exception)
            {
                await Context.Channel.SendMessageAsync("Hey! You can't add text!");
            }
        }

        [Command("minus")]
        public async Task Minus(string ones, string twos)
        {
            float one, two;

            try
            {
                one = Convert.ToSingle(ones);
                two = Convert.ToSingle(twos);
                await Context.Channel.SendMessageAsync("The end result is: " + (one - two).ToString());
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Hey! You can't minus text!");
            }
        }

        [Command("multiply")]
        public async Task Multiply(string ones, string twos)
        {
            float one, two;

            try
            {
                one = Convert.ToSingle(ones);
                two = Convert.ToSingle(twos);
                await Context.Channel.SendMessageAsync("The end result is: " + (one * two).ToString());
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Hey! You can't multiply text!");
            }
        }

        [Command("devide")]
        public async Task Devide(string ones, string twos)
        {
            float one, two;

            try
            {
                one = Convert.ToSingle(ones);
                two = Convert.ToSingle(twos);
                await Context.Channel.SendMessageAsync("The end result is: " + (one / two).ToString());
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Hey! You can't devide text!");
            }
        }

        [Command("root")]
        public async Task Root(string numberString)
        {
            float number;

            try
            {
                number = Convert.ToSingle(numberString);
                await Context.Channel.SendMessageAsync("The end result is: " +  Math.Sqrt(number));
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Hey! You can't root text!");
            }
        }

        [Command("square")]
        public async Task Square(string numberString)
        {
            float number;

            try
            {
                number = Convert.ToSingle(numberString);
                await Context.Channel.SendMessageAsync("The end result is: " + Math.Pow(number, 2));
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Hey! You can't root text!");
            }
        }
       
        [Command("power")]
        public async Task Power(string numberString, string powerString)
        {
            float number, power;

            try
            {
                number = Convert.ToSingle(numberString);
                power = Convert.ToSingle(powerString);
                await Context.Channel.SendMessageAsync("The end result is: " + Math.Pow(number, power));
            }
            catch(Exception)
            {
                await Context.Channel.SendMessageAsync("Hey! You can't root text!");
            }
        }

        [Command("min")]
        public async Task Min(params string[] numbersString)
        {
            float[] numbers = new float[numbersString.Length];

            try
            {
                for (int i = 0; i < numbersString.Length; i++)
                    numbers[i] = Convert.ToSingle(numbersString[i]);

                float min = numbers[0];

                for (int i = 0; i < numbersString.Length; i++)
                    if (min > numbers[i])
                        min = numbers[i];

                await Context.Channel.SendMessageAsync("The end result is: " + min);
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Hey! You can't here text!");
            }
        }

        [Command("max")]
        public async Task Max(params string[] numbersString)
        {
            float[] numbers = new float[numbersString.Length];

            try
            {
                for (int i = 0; i < numbersString.Length; i++)
                    numbers[i] = Convert.ToSingle(numbersString[i]);

                float min = numbers[0];

                for (int i = 0; i < numbersString.Length; i++)
                    if (min < numbers[i])
                        min = numbers[i];

                await Context.Channel.SendMessageAsync("The end result is: " + min);
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Hey! I want numbers, not text!");
            }
        }

        [Command("avg")]
        public async Task Avg(params string[] numbersString)
        {
            float[] numbers = new float[numbersString.Length];
            float added = 0f;

            try
            {
                for (int i = 0; i < numbersString.Length; i++)
                {
                    numbers[i] = Convert.ToSingle(numbersString[i]);
                    added += numbers[i];
                }

                await Context.Channel.SendMessageAsync("The end result is: " + (added / numbers.Length).ToString());
            }
            catch(Exception)
            {
                await Context.Channel.SendMessageAsync("Hey! I want numbers, not text!");
            }
        }
        #endregion

        [Command("avatar")]
        public async Task Avatar(IGuildUser User)
        {
            if(User.GetAvatarUrl() != null)
                await Context.Channel.SendMessageAsync(User.GetAvatarUrl());
            else
                await Context.Channel.SendMessageAsync("This person doesn't have profile picture");
        }

        [Command("idinfo")]
        public async Task IdInfo()
        {
            var embed = new EmbedBuilder();

            embed.WithTitle("ID info command")
                .AddField("Server ID:", Context.Guild.Id)
                .AddField("Channel ID:", Context.Channel.Id)
                .AddField(Context.User.ToString() + " ID:", Context.User.Id)
                .WithColor(Color.Green);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        #region MAL
        [Command("mal")]
        public async Task MALUser([Remainder]string User)
        {
            try
            {
                HttpClient client = new HttpClient();
                var json = client.GetStringAsync("https://api.jikan.moe/v3/user/" + User);
                MALUser x = JsonConvert.DeserializeObject<MALUser>(json.Result);

                var MAL = new EmbedBuilder();

                EmbedFieldBuilder watched = new EmbedFieldBuilder();
                watched.IsInline = true;
                watched.Name = "Days Watched: ";
                watched.Value = x.anime_stats.days_watched;

                EmbedFieldBuilder total = new EmbedFieldBuilder();
                total.IsInline = false;
                total.Name = "Total Entries: ";
                total.Value = x.anime_stats.total_entries;

                MAL.WithTitle(x.username)
                .WithThumbnailUrl(x.image_url)
                .AddField(watched)
                .AddField(total)
                .AddField("Completed:", x.anime_stats.completed, true)
                .AddField("Watching:", x.anime_stats.watching, true)
                .AddField("Dropped:", x.anime_stats.dropped, true);

                await Context.Channel.SendMessageAsync("", false, MAL.Build());
            }
            catch(Exception)
            {
                await Context.Channel.SendMessageAsync("Couldn't find any user with that nickname!");
            }
        }

        [Command("anime")]
        public async Task Anime([Remainder]string Anime)
        {
            try
            {
                HttpClient client = new HttpClient();
                var json = client.GetStringAsync("https://api.jikan.moe/v3/search/anime?q=" + Anime + "&page=1");
                Anime x = JsonConvert.DeserializeObject<Anime>(json.Result);

                var MAL = new EmbedBuilder();

                string airing;

                if (x.results[0].airing)
                    airing = "Yes";
                else
                    airing = "No";

                MAL.WithTitle(x.results[0].title)
                .WithThumbnailUrl(x.results[0].image_url)
                .WithDescription(x.results[0].synopsis)
                .AddField("Episodes: ", x.results[0].episodes, true)
                .AddField("Score: ", x.results[0].score, true)
                .AddField("Type: ", x.results[0].type, true)
                .AddField("Rating: ", x.results[0].rated, true)
                .AddField("Airing: ", airing, true)
                .AddField("Members: ", x.results[0].members, true);

                await Context.Channel.SendMessageAsync("", false, MAL.Build());
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Couldn't find any anime with that name!");
            }
        }
        #endregion 

        [Command("crypt base64")]
        public async Task Crypt([Remainder] string text)
        {
            if(text.Length > 1000)
            {
                await ReplyAsync("Text is too long.");
                return;
            }
            var plainTextBytes = Encoding.UTF8.GetBytes(text);
            string converted = Convert.ToBase64String(plainTextBytes);

            await ReplyAsync(converted);
        }

        [Command("replace")]
        public async Task Replace(string replace, string withtext, [Remainder]string text)
        {
            text.Replace(replace, withtext);
            await ReplyAsync(text);
        }

        [Command("serverstats")]
        public async Task ServerStats()
        {
            var embed = new EmbedBuilder();

            embed.WithTitle("Server stats")
                .AddField("Users:", Context.Guild.Users.Count.ToString(), true)
                .AddField("Voice channels:", Context.Guild.VoiceChannels.Count, true)
                .AddField("Text channels:", Context.Guild.TextChannels.Count, true);

            await Context.Channel.SendMessageAsync("",false,embed.Build());
        }
    }
}
