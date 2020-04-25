using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Photos;
using ImageProcessor;
using System.Drawing;
using Newtonsoft.Json.Linq;

namespace WeX.Modules
{
    public class Fun : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        public async Task Say(string text)
        {
            await Context.Channel.SendMessageAsync(text);
        }

        [Command("dice")]
        public async Task Dice()
        {
            Random x = new Random();
            await Context.Channel.SendMessageAsync("I rolled :game_die: " + x.Next(1, 7).ToString() + " :game_die:");
        }

        [Command("8ball")]
        public async Task EightBall([Remainder]string text)
        {
            try
            {
                EightBall[] final = (EightBall[])JSONhandler.GetElement(JsonFile.EightBall);

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(text);
                string converted = System.Convert.ToBase64String(plainTextBytes);
                byte[] LogoDataBy = Encoding.ASCII.GetBytes(converted);
                int seed = 0;
                foreach(byte byt in LogoDataBy)
                    seed += byt;

                Random x = new Random(seed);
                
                await Context.Channel.SendMessageAsync(final[x.Next(1, final.Length)].text);
            }
            catch (HttpRequestException)
            {
                await Context.Channel.SendMessageAsync("Can't connect to API. Please, contact with author.");
            }
        }

        [Command("computer")]
        public async Task Computer(IGuildUser user)
        {
            try
            {
                System.Drawing.Image img = HTTPrequest.RequestImage(user.GetAvatarUrl());
                PhotoCommands.Computer(img);
                await Context.Channel.SendFileAsync("./Images/final.png");
            }
            catch(Exception)
            {
                await Context.Channel.SendMessageAsync("Avatar not found");
            }
        }

        [Command("cat")]
        public async Task Cat()
        {
            HttpClient client = new HttpClient();
            var json = client.GetStringAsync("https://api.thecatapi.com/v1/images/search");
            Cat[] x = JsonConvert.DeserializeObject<Cat[]>(json.Result);
            System.Drawing.Image img = HTTPrequest.RequestImage(x[0].url);
            img.Save("./Images/final.png");
            await Context.Channel.SendFileAsync("./Images/final.png");
        }

        [Command("dog")]
        public async Task Dog()
        {
            HttpClient client = new HttpClient();
            var json = client.GetStringAsync("https://dog.ceo/api/breeds/image/random");
            Dog x = JsonConvert.DeserializeObject<Dog>(json.Result);
            System.Drawing.Image img = HTTPrequest.RequestImage(x.message);
            img.Save("./Images/final.png");
            await Context.Channel.SendFileAsync("./Images/final.png");
        }
        
        [Command("hug")]
        public async Task Hug(IGuildUser user)
        {
            HttpClient client = new HttpClient();
            var json = client.GetStringAsync("https://some-random-api.ml/animu/hug");
            Bird x = JsonConvert.DeserializeObject<Bird>(json.Result);

            if(user.Id == Context.User.Id)
            {
                await Context.Channel.SendMessageAsync("You can't hug yourself!");
                return;
            }
            else if(user.Id == 665514955985911818)
            {
                await Context.Channel.SendMessageAsync("You can't hug me! *because im shy*");
                return;
            }
            else if(user.IsBot == true)
            {
                await Context.Channel.SendMessageAsync("You can't hug bot!");
                return;
            }
    
            var y = new EmbedBuilder();
            y.WithDescription(user.ToString() + ", you got a hug from " + Context.User.ToString() + "!")
            .WithImageUrl(x.link);

            await Context.Channel.SendMessageAsync("", false, y.Build());
        }

        [Command("bird")]
        public async Task Bird()
        {
            HttpClient client = new HttpClient();
            var json = client.GetStringAsync("https://some-random-api.ml/img/birb");
            Bird x = JsonConvert.DeserializeObject<Bird>(json.Result);
            System.Drawing.Image img = HTTPrequest.RequestImage(x.link);
            img.Save("./Images/final.png");
            await Context.Channel.SendFileAsync("./Images/final.png");
        }

        [Command("rps")]
        public async Task Prs(string choice)
        {
            //1 S | 2 R | 3 P

            Random x = new Random();
            int botchoice = x.Next(1, 4);

            if (choice == "scissors")
            {
                if (botchoice == 1)
                    await Context.Channel.SendMessageAsync("I choose... Scissors! Draw!");
                else if (botchoice == 2)
                    await Context.Channel.SendMessageAsync("I choose... Rock! I won!");
                else
                    await Context.Channel.SendMessageAsync("I choose... Paper! You won!");
            }
            else if (choice == "rock")
            {
                if (botchoice == 1)
                    await Context.Channel.SendMessageAsync("I choose... Scissors! You won!");
                else if (botchoice == 2)
                    await Context.Channel.SendMessageAsync("I choose... Rock! Draw!");
                else
                    await Context.Channel.SendMessageAsync("I choose... Paper! I won!");
            }
            else if(choice == "paper")
            {
                if (botchoice == 1)
                    await Context.Channel.SendMessageAsync("I choose... Scissors! I won!");
                else if (botchoice == 2)
                    await Context.Channel.SendMessageAsync("I choose... Rock! You won!");
                else
                    await Context.Channel.SendMessageAsync("I choose... Paper! Draw!");
            }
            else
                await Context.Channel.SendMessageAsync("Hey! We are playing Rock, paper, scissors!");
        }
        
        [Command("lenny")]
        public async Task Lenny()
        {
            await Context.Channel.SendMessageAsync("( ͡° ͜ʖ ͡°)");
        }

        [Command("toilet")]
        public async Task Toilet(IGuildUser user)
        {
            try
            {
                System.Drawing.Image img = HTTPrequest.RequestImage(user.GetAvatarUrl());
                PhotoCommands.Toilet(img);
                await Context.Channel.SendFileAsync("./Images/final.png");
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Avatar not found.");
            }
        }

        [Command("spoiler")]
        public async Task Spoiler([Remainder]string text)
        {
            char[] textbutchar = text.ToCharArray();
            string final = string.Empty;

            foreach (char x in textbutchar)
                final += "||" + x + "||";

            await Context.Channel.SendMessageAsync(final);
        }
 
        [Command("emojileters")]
        public async Task Emojileters([Remainder]string text)
        {
            char[] x = text.ToLower().ToCharArray();
            string final = string.Empty;

            foreach(char y in x)
            {
                if (y == 'a')
                    final += ":regional_indicator_a:";
                else if (y == 'b')
                    final += ":regional_indicator_b:";
                else if (y == 'c')
                    final += ":regional_indicator_c:";
                else if (y == 'd')
                    final += ":regional_indicator_d:";
                else if (y == 'e')
                    final += ":regional_indicator_e:";
                else if (y == 'f')
                    final += ":regional_indicator_f:";
                else if (y == 'g')
                    final += ":regional_indicator_g:";
                else if (y == 'h')
                    final += ":regional_indicator_h:";
                else if (y == 'i')
                    final += ":regional_indicator_i:";
                else if (y == 'j')
                    final += ":regional_indicator_j:";
                else if (y == 'k')
                    final += ":regional_indicator_k:";
                else if (y == 'l')
                    final += ":regional_indicator_l:";
                else if (y == 'm')
                    final += ":regional_indicator_m:";
                else if (y == 'n')
                    final += ":regional_indicator_n:";
                else if (y == 'o')
                    final += ":regional_indicator_o:";
                else if (y == 'p')
                    final += ":regional_indicator_p:";
                else if (y == 'r')
                    final += ":regional_indicator_r:";
                else if (y == 's')
                    final += ":regional_indicator_s:";
                else if (y == 't')
                    final += ":regional_indicator_t:";
                else if (y == 'w')
                    final += ":regional_indicator_w:";
                else if (y == 'z')
                    final += ":regional_indicator_z:";
                else if (y == 'x')
                    final += ":regional_indicator_x:";
                else if (y == 'd')
                    final += ":regional_indicator_d:";
                else if (y == 'q')
                    final += ":regional_indicator_q:";
                else if (y == 'y')
                    final += ":regional_indicator_y:";
                else if (y == 'u')
                    final += ":regional_indicator_u:";
                else if (y == 'v')
                    final += ":regional_indicator_v:";
                else if (y == '0')
                    final += ":zero:";
                else if (y == '1')
                    final += ":one:";
                else if (y == '2')
                    final += ":two:";
                else if (y == '3')
                    final += ":three:";
                else if (y == '4')
                    final += ":four:";
                else if (y == '5')
                    final += ":five:";
                else if (y == '6')
                    final += ":six:";
                else if (y == '7')
                    final += ":seven:";
                else if (y == '8')
                    final += ":eight:";
                else if (y == '9')
                    final += ":nine:";
                else if (y == '!')
                    final += ":grey_exclamation:";
                else if (y == '?')
                    final += ":grey_question:";
                else
                    final += y;
            }

            await Context.Channel.SendMessageAsync(final);
        }

        [Command("lovemeter")]
        public async Task Lovemeter(IGuildUser use1, IGuildUser use2)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(use1.ToString());
            string converted = Convert.ToBase64String(plainTextBytes);
            byte[] LogoDataBy = Encoding.ASCII.GetBytes(converted);
            int seed = 0;
            foreach (byte byt in LogoDataBy)
                seed += byt;

            var plainTextBytes2 = Encoding.UTF8.GetBytes(use2.ToString());
            string converted2 = Convert.ToBase64String(plainTextBytes2);
            byte[] LogoDataBy2 = Encoding.ASCII.GetBytes(converted2);
            foreach (byte byt in LogoDataBy2)
                seed += byt;

            Random x = new Random(seed);
            long loveprocent = x.Next(1, 101);

            System.Drawing.Image img1;
            System.Drawing.Image img2;
            try
            {
                img1 = HTTPrequest.RequestImage(use1.GetAvatarUrl());
                img2 = HTTPrequest.RequestImage(use2.GetAvatarUrl());
            }

            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Avatar not found.");
                return;
            }

            PhotoCommands.Lovemeter(img1, img2);
            await Context.Channel.SendFileAsync("Images/final.png", "They are loving each other in " + loveprocent + "%!");
        }

        [Command("drake")]
        public async Task Drake(IGuildUser use1, IGuildUser use2)
        {
            try
            { 
            System.Drawing.Image img1 = HTTPrequest.RequestImage(use1.GetAvatarUrl());
            System.Drawing.Image img2 = HTTPrequest.RequestImage(use2.GetAvatarUrl());
            PhotoCommands.Drake(img1, img2);
            await Context.Channel.SendFileAsync("Images/final.png");
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Avatar not found.");
            }
        }

        [Command("pickle")]
        public async Task Pickle(IGuildUser user)
        {
            try
            {
                System.Drawing.Image img = HTTPrequest.RequestImage(user.GetAvatarUrl());
                PhotoCommands.Pickle(img);
                await Context.Channel.SendFileAsync("./Images/final.png");
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Avatar not found.");
            }
        }

        [Command("food")]
        public async Task Food()
        {
            HttpClient client = new HttpClient();
            var jsonstring = client.GetStringAsync("https://api.spoonacular.com/recipes/random?apiKey=e36467522cb742619b0fa63942e25bfd");
            JObject json = JObject.Parse(jsonstring.Result);
            Food x = JsonConvert.DeserializeObject<Food>(json["recipes"][0].ToString());
            System.Drawing.Image img = HTTPrequest.RequestImage(x.image);
            img.Save("./Images/final.png");
            await Context.Channel.SendFileAsync("./Images/final.png");
        }
    }
}
