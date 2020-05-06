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
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Discord.Rest;

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
                PhotoCommands.Picture("computer",img, new Size(235, 235), new Point(325, 20));
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
            y.WithTitle(user.ToString() + ", you got a hug from " + Context.User.ToString() + "!")
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
                PhotoCommands.Picture("toilet", img, new Size(50, 50), new Point(120, 65));
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
                PhotoCommands.Picture("pickle", img, new Size(250, 250), new Point(404, 142));
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

        [Command("joker")] 
        public async Task Joker(IGuildUser user)
        {
            try
            {
                System.Drawing.Image img = HTTPrequest.RequestImage(user.GetAvatarUrl());
                PhotoCommands.Picture("joker", img, new Size(200, 200), new Point(120, 30));
                await Context.Channel.SendFileAsync("./Images/final.png");
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Avatar not found.");
            }
        }

        [Command("keanu")]
        public async Task Keanu(IGuildUser user)
        {
            try
            {
                System.Drawing.Image img = HTTPrequest.RequestImage(user.GetAvatarUrl());
                PhotoCommands.Picture("keanu", img, new Size(100, 100), new Point(150, 20));
                await Context.Channel.SendFileAsync("./Images/final.png");
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Avatar not found.");
            }
        }

        [Command("washer")]
        public async Task Washer(IGuildUser user)
        {
            try
            {
                System.Drawing.Image img = HTTPrequest.RequestImage(user.GetAvatarUrl());
                PhotoCommands.Picture("washer", img, new Size(260, 260), new Point(60, 120));
                await Context.Channel.SendFileAsync("./Images/final.png");
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Avatar not found.");
            }
        }

        [Command("howsimp")]
        public async Task HowSimp(IGuildUser user)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(user.ToString());
            string converted = Convert.ToBase64String(plainTextBytes);
            byte[] LogoDataBy = Encoding.ASCII.GetBytes(converted);
            int seed = 0;
            foreach (byte byt in LogoDataBy)
                seed += byt * 2;

            Random x = new Random(seed);

            await Context.Channel.SendMessageAsync(user.Mention + " is simp in " + x.Next(1, 101) + "%");
        }
    
        [Command("reverse")]
        public async Task Reverse([Remainder] string text)
        {
            char[] textinchar = text.ToCharArray();

            for(int i = 0; i < textinchar.Length; i++)
                textinchar[i] = text.ToCharArray()[textinchar.Length - i - 1];

            string final = string.Empty;
            foreach (char x in textinchar)
                final += x;

            await Context.Channel.SendMessageAsync(final);
        }

        [Command("howweeb")]
        public async Task HowWeeb(IGuildUser user)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(user.ToString());
            string converted = Convert.ToBase64String(plainTextBytes);
            byte[] LogoDataBy = Encoding.ASCII.GetBytes(converted);
            int seed = 0;
            foreach (byte byt in LogoDataBy)
                seed += byt * 4;

            Random x = new Random(seed);

            await Context.Channel.SendMessageAsync(user.Mention + " is weeb in " + x.Next(1, 101) + "%");
        }

        [Command("choose")]
        public async Task Choose([Remainder] string text)
        {
            string[] types = text.Split(", ");

            if(types.Length < 2)
            {
                await Context.Channel.SendMessageAsync("That was simple. " + types[0]);
                return;
            }

            Random rand = new Random();
            await Context.Channel.SendMessageAsync(types[rand.Next(0,types.Length)]);
        }

        [Command("howgay")]
        public async Task HowGay(IGuildUser user)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(user.ToString());
            string converted = Convert.ToBase64String(plainTextBytes);
            byte[] LogoDataBy = Encoding.ASCII.GetBytes(converted);
            int seed = 0;
            foreach (byte byt in LogoDataBy)
                seed += byt * 3;

            Random x = new Random(seed);

            await Context.Channel.SendMessageAsync(user.Mention + " is gay in " + x.Next(1, 101) + "%");
        }

        [Command("fox")]
        public async Task Fox()
        {
            HttpClient client = new HttpClient();
            var json = client.GetStringAsync("https://some-random-api.ml/img/fox");
            Bird x = JsonConvert.DeserializeObject<Bird>(json.Result);
            System.Drawing.Image img = HTTPrequest.RequestImage(x.link);
            img.Save("./Images/final.png");
            await Context.Channel.SendFileAsync("./Images/final.png");
        }

        [Command("meme")]
        public async Task Meme()
        {
            HttpClient client = new HttpClient();
            var json = client.GetStringAsync("https://some-random-api.ml/meme");
            Meme x = JsonConvert.DeserializeObject<Meme>(json.Result);
            System.Drawing.Image img = HTTPrequest.RequestImage(x.image);
            img.Save("./Images/final.png");
            await Context.Channel.SendFileAsync("./Images/final.png");
        }

        [Command("random")]
        public async Task Random(string smin, string smax)
        {
            int min, max;

            try
            {
                min = Convert.ToInt32(smin);
                max= Convert.ToInt32(smax);
            }
            catch(Exception)
            {
                await Context.Channel.SendMessageAsync("Hey! Text is not allowed!");
                return;
            }

            if(max <= min)
            {
                await Context.Channel.SendMessageAsync("Minimum value cannot be greater than maximum value");
                return;
            }

            Random rand = new Random();
            await Context.Channel.SendMessageAsync(rand.Next(min, max).ToString());
        }

        [Command("coinflip")]
        public async Task Coinflip()
        {
            Random rand = new Random();
            int number = rand.Next(1, 3);

            if (number == 1)
                await Context.Channel.SendMessageAsync("It's... **Tails!**");
            else
                await Context.Channel.SendMessageAsync("It's... **Heads!**");
        }

        [Command("animalfact")]
        public async Task Animalfact()
        {
            Random rand = new Random();
            int number = rand.Next(1, 7);
            string api = string.Empty;

            switch(number)
            {
                case 1:
                    api = "https://some-random-api.ml/facts/dog";
                    break;
                case 2:
                    api = "https://some-random-api.ml/facts/cat";
                    break;
                case 3:
                    api = "https://some-random-api.ml/facts/bird";
                    break;
                case 4:
                    api = "https://some-random-api.ml/facts/fox";
                    break;
                case 5:
                    api = "https://some-random-api.ml/facts/panda";
                    break;
                case 6:
                    api = "https://some-random-api.ml/facts/koala";
                    break;
            }

            HttpClient client = new HttpClient();
            var json = client.GetStringAsync(api);
            Fact x = JsonConvert.DeserializeObject<Fact>(json.Result);
            await Context.Channel.SendMessageAsync(x.fact);
        }

        [Command("tobecontinued")]
        public async Task Tobecontinued(IGuildUser user)
        {
            try
            {
                System.Drawing.Image img = HTTPrequest.RequestImage(user.GetAvatarUrl());
                PhotoCommands.Tobecontinued(img);
                await Context.Channel.SendFileAsync("./Images/final.png");
            }
            catch (Exception)
            {
                await Context.Channel.SendMessageAsync("Avatar not found.");
            }
        }

        [Command("pat")]
        public async Task Pat(IGuildUser user)
        {
            HttpClient client = new HttpClient();
            var json = client.GetStringAsync("https://some-random-api.ml/animu/pat");
            Bird x = JsonConvert.DeserializeObject<Bird>(json.Result);

            if (user.Id == Context.User.Id)
            {
                await Context.Channel.SendMessageAsync("You can't pat yourself!");
                return;
            }
            else if (user.Id == 665514955985911818)
            {
                await Context.Channel.SendMessageAsync("You can't pat me! *because im shy*");
                return;
            }
            else if (user.IsBot == true)
            {
                await Context.Channel.SendMessageAsync("You can't pat bot!");
                return;
            }

            EmbedBuilder build = new EmbedBuilder();

            build.WithTitle(Context.User.ToString() + " is patting " + user.ToString() + "!")
                .WithImageUrl(x.link);

            await Context.Channel.SendMessageAsync("", false, build.Build());
        }

        [Command("slap")]
        public async Task Slap(IGuildUser user)
        {
            Bird[] final = (Bird[])JSONhandler.GetElement(JsonFile.slap);

            if (user.Id == Context.User.Id)
            {
                await Context.Channel.SendMessageAsync("You can't slap yourself! *or can you?*");
                return;
            }
            else if (user.Id == 665514955985911818)
            {
                await Context.Channel.SendMessageAsync("Don't slap me!!");
                return;
            }
            else if (user.IsBot == true)
            {
                await Context.Channel.SendMessageAsync("You can't slap bot!");
                return;
            }

            EmbedBuilder build = new EmbedBuilder();
            Random rand = new Random();
            int choose = rand.Next(1, 6);

            switch(choose)
            {
                case 1:
                    build.WithTitle(Context.User.ToString() + " is slaping " + user.ToString() + "!")
                        .WithImageUrl(final[0].link);
                    break;
                case 2:
                    build.WithTitle(Context.User.ToString() + " is slaping " + user.ToString() + "!")
                        .WithImageUrl(final[1].link);
                    break;
                case 3:
                    build.WithTitle(Context.User.ToString() + " is slaping " + user.ToString() + "!")
                        .WithImageUrl(final[2].link);
                    break;
                case 4:
                    build.WithTitle(Context.User.ToString() + " is slaping " + user.ToString() + "!")
                        .WithImageUrl(final[3].link);
                    break;
                case 5:
                    build.WithTitle(Context.User.ToString() + " is slaping " + user.ToString() + "!")
                        .WithImageUrl(final[4].link);
                    break;
            }
            
            await Context.Channel.SendMessageAsync("", false, build.Build());
        }

        [Command("Wink")]
        public async Task Wink()
        {
            HttpClient client = new HttpClient();
            var json = client.GetStringAsync("https://some-random-api.ml/animu/wink");
            Bird x = JsonConvert.DeserializeObject<Bird>(json.Result);

            EmbedBuilder build = new EmbedBuilder();

            build.WithTitle(Context.User.ToString() + " just winked!")
                .WithImageUrl(x.link);

            await Context.Channel.SendMessageAsync("", false, build.Build());
        }

        [Command("kiss")]
        public async Task Kiss(IGuildUser user)
        {
            Bird[] final = (Bird[])JSONhandler.GetElement(JsonFile.kiss);

            if (user.Id == Context.User.Id)
            {
                await Context.Channel.SendMessageAsync("You can't kiss yourself!");
                return;
            }
            else if (user.Id == 665514955985911818)
            {
                await Context.Channel.SendMessageAsync("Don't kiss me! *Because I'm shy*");
                return;
            }
            else if (user.IsBot == true)
            {
                await Context.Channel.SendMessageAsync("You can't kiss bot!");
                return;
            }

            EmbedBuilder build = new EmbedBuilder();
            Random rand = new Random();
            int choose = rand.Next(1, 6);

            switch (choose)
            {
                case 1:
                    build.WithTitle(Context.User.ToString() + " is kissing " + user.ToString() + "!")
                        .WithImageUrl(final[0].link);
                    break;
                case 2:
                    build.WithTitle(Context.User.ToString() + " is kissing " + user.ToString() + "!")
                        .WithImageUrl(final[1].link);
                    break;
                case 3:
                    build.WithTitle(Context.User.ToString() + " is kissing " + user.ToString() + "!")
                        .WithImageUrl(final[2].link);
                    break;
                case 4:
                    build.WithTitle(Context.User.ToString() + " is kissing " + user.ToString() + "!")
                        .WithImageUrl(final[3].link);
                    break;
                case 5:
                    build.WithTitle(Context.User.ToString() + " is kissing " + user.ToString() + "!")
                        .WithImageUrl(final[4].link);
                    break;
            }

            await Context.Channel.SendMessageAsync("", false, build.Build());
        }
    }
}
