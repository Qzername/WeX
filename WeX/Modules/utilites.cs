using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        //MAL API
        [Command("mal")]
        public async Task MALUser(string User)
        {

        }
    }
}
