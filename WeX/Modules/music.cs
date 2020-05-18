using Discord.Commands;
using System;
using System.Text;
using Victoria;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Victoria.Enums;
using Discord.WebSocket;

namespace WeX.Modules
{
    public class Music : ModuleBase<SocketCommandContext>
    {
        public LavaNode link { get; set; }

        public async Task SlientLeft()
        {
            try
            {
                var player = link.GetPlayer(Context.Guild as IGuild);
                await link.LeaveAsync(player.VoiceChannel);
                if (player.StopAsync() == null)
                    throw new Exception("DUPE KURWA");
            }
            catch(Exception)
            {

            }
        }

        [Command("join")]
        [RequireBotPermission(GuildPermission.Connect)]
        public async Task Join()
        {
            IGuildUser user = (Context.Guild.GetUser(665514955985911818) as IGuildUser);

            if (user.VoiceChannel == null)
                await SlientLeft();

            if (link.HasPlayer(Context.Guild as IGuild))
            {
                await ReplyAsync("I'm already in voice channel.");
                return;
            }

            if ((Context.User as IVoiceState).VoiceChannel is null)
            {
                await ReplyAsync("You must be in a voice channel!");
                return;
            }
                
            try
            {
                await link.JoinAsync((Context.User as IVoiceState).VoiceChannel, (Context.Channel as ITextChannel));
                await ReplyAsync( $"Joined {(Context.User as IVoiceState).VoiceChannel.Name}.");
                return;
            }
            catch (Exception)
            {
                await ReplyAsync("Something went wrong"); 
                return;
            }
        }

        [Command("leave")]
        [RequireBotPermission(GuildPermission.Connect)]
        public async Task Leave()
        {
            IGuildUser user = (Context.Guild.GetUser(665514955985911818) as IGuildUser);
            if (user.VoiceChannel == null)
                await SlientLeft();

            try
            {
                if(user.VoiceChannel is null)
                {
                    await ReplyAsync("But I'm not in any channel!"); 
                    return;
                }

                var player = link.GetPlayer(Context.Guild as IGuild);
                await link.LeaveAsync(player.VoiceChannel);

                if (player.PlayerState is PlayerState.Playing)
                    await player.StopAsync();

                await ReplyAsync($"I've left. Thank you for using.");
            }
            catch (Exception)
            {
                await ReplyAsync("Something went wrong.");
            }
            return;
        }

        [Command("play")]
        [RequireBotPermission(GuildPermission.Connect)]
        public async Task Play([Remainder]string music)
        {
            IGuildUser user = (Context.Guild.GetUser(665514955985911818) as IGuildUser);
            if (user.VoiceChannel is null && link.HasPlayer(Context.Guild as IGuild))
                await SlientLeft();

            if ((Context.User as SocketGuildUser).VoiceChannel == null && (Context.User as IVoiceState).VoiceChannel is null)
            {
                await ReplyAsync("You must be in a voice channel!");
                return;
            }
            
            if(!link.HasPlayer(Context.Guild as IGuild))
                await Join();

            if (!link.HasPlayer(Context.Guild as IGuild))
            {
                await ReplyAsync("I'm not connected to a voice channel.");
                return;
            }

            try
            {
                var player = link.GetPlayer(Context.Guild as IGuild);

                LavaTrack track;
                var search = await link.SearchYouTubeAsync(music);

                if (search.LoadStatus == LoadStatus.NoMatches)
                {
                    await ReplyAsync($"I wasn't able to find anything for {music}.");
                    return;
                }

                track = search.Tracks.FirstOrDefault();

                if (player.Track != null && player.PlayerState is PlayerState.Playing || player.PlayerState is PlayerState.Paused)
                {
                    if(player.Queue.Count > 10)
                    {
                        await ReplyAsync("Playlist can't be longer than 10 songs.");
                        return;
                    }

                    player.Queue.Enqueue(track);
                    await ReplyAsync($"{track.Title} has been added to queue.");
                    return;
                }

                await player.PlayAsync(track);
                await ReplyAsync($"Now Playing: {track.Title}\nUrl: {track.Url}");

                return;
            }
            catch (Exception)
            {
                await ReplyAsync("Something went wrong");
                return;
            }
        }

        [Command("queue")]
        [RequireBotPermission(GuildPermission.Connect)]
        public async Task Queue()
        {
            IGuildUser user = (Context.Guild.GetUser(665514955985911818) as IGuildUser);
            if (user.VoiceChannel is null && link.HasPlayer(Context.Guild as IGuild))
                await SlientLeft();

            try
            {
                var descriptionBuilder = new StringBuilder();

                var player = link.GetPlayer(Context.Guild as IGuild);

                if (player == null)
                {
                    await ReplyAsync($"Could not aquire player. Are you using the bot right now?");
                    return;
                }

                if (player.PlayerState is PlayerState.Playing)
                {
                    if (player.Queue.Count < 1 && player.Track != null)
                    {
                        await ReplyAsync($"Now playing: {player.Track.Title}, nothing else is queued.");
                        return;
                    }
                    else
                    {
                        var embed = new EmbedBuilder();

                        string text = string.Empty;
                        byte trackNum = 0;
                        text += $"Now playing: {player.Track.Title}";
                        foreach (LavaTrack track in player.Queue.Items)
                        {
                            text += $"{trackNum}: [{track.Title}]({track.Url}) - {track.Id}\n";
                            trackNum++;
                        }

                        embed.WithTitle("Playlist")
                            .WithDescription(text);

                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                    }
                }
                else
                {
                    await ReplyAsync("Player doesn't seem to be playing anything right now.");
                    return;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("The given key"))
                {
                    await ReplyAsync("I'm not even connected...");
                    return;
                }

                await ReplyAsync("Something went wrong.");
                return;
            }
        }

        [Command("skip")]
        [RequireBotPermission(GuildPermission.Connect)]
        public async Task Skip()
        {
            IGuildUser user = (Context.Guild.GetUser(665514955985911818) as IGuildUser);
            if (user.VoiceChannel is null && link.HasPlayer(Context.Guild as IGuild))
                await SlientLeft();

            try
            {
                var player = link.GetPlayer(Context.Guild as IGuild);

                if(player.PlayerState == PlayerState.Disconnected || player.Queue.Count == 0)
                {
                    await ReplyAsync("Queue is clear, there is nothing to skip");
                    return;
                }

                if (player == null)
                {
                    await ReplyAsync($"Could not aquire player.\nAre you using the bot right now?");
                    return;
                }

                if (player.Queue.Count < 1)
                {
                    var currentTrack = player.Track;
                    await ReplyAsync($"I have successfully skiped {currentTrack.Title}");
                    await player.StopAsync();
                    return;
                }
                else
                {
                    try
                    {
                        var currentTrack = player.Track;
                        await player.SkipAsync();
                        await ReplyAsync($"I have successfully skiped {currentTrack.Title}");
                        return;
                    }
                    catch (Exception)
                    {
                        await ReplyAsync("Something went wrong");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("The given key"))
                {
                    await ReplyAsync("I'm not even connected...");
                    return;
                }

                await ReplyAsync("Something went wrong"+ex.InnerException.ToString());
                return;
            }
        }

        [Command("volume")]
        [RequireUserPermission(GuildPermission.DeafenMembers)]
        [RequireBotPermission(GuildPermission.Connect)]
        public async Task Volume(int volume)
        {
            if ((volume > 150 || volume <= 0) && Context.User.Id != 273438920870461441)
            {
                await ReplyAsync($"Volume must be between 1 and 150.");
                return;
            }
                
            try
            {
                var player = link.GetPlayer(Context.Guild as IGuild);
                await player.UpdateVolumeAsync((ushort)volume);
                await ReplyAsync($"Volume has been set to {volume}.");
                return;
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("The given key"))
                {
                    await ReplyAsync("I'm not even connected...");
                    return;
                }

                await ReplyAsync($"Something went wrong");
                return;
            }
        }

        [Command("stop")]
        [RequireUserPermission(GuildPermission.DeafenMembers)]
        [RequireBotPermission(GuildPermission.Connect)]
        public async Task Stop()
        {
            IGuildUser user = (Context.Guild.GetUser(665514955985911818) as IGuildUser);
            if (user.VoiceChannel is null && link.HasPlayer(Context.Guild as IGuild))
                await SlientLeft();

            try
            {
                var player = link.GetPlayer(Context.Guild as IGuild);

                if (player == null)
                {
                    await ReplyAsync($"Could not aquire player.Are you using the bot right now?");
                    return;
                }
               
                if (player.PlayerState is PlayerState.Playing)
                    await player.StopAsync();

                await ReplyAsync("I Have stopped playback & the playlist has been cleared.");
                return;
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("The given key"))
                {
                    await ReplyAsync("I'm not even connected...");
                    return;
                }

                await ReplyAsync("Something went wrong");
                return;
            }
        }

        [Command("pause")]
        [RequireBotPermission(GuildPermission.Connect)]
        public async Task Pause()
        {
            IGuildUser user = (Context.Guild.GetUser(665514955985911818) as IGuildUser);
            if (user.VoiceChannel is null && link.HasPlayer(Context.Guild as IGuild))
                await SlientLeft();

            try
            {
                var player = link.GetPlayer(Context.Guild as IGuild);
                if (!(player.PlayerState is PlayerState.Playing))
                {
                    await player.PauseAsync();
                    await ReplyAsync($"There is nothing to pause.");
                    return;
                }

                await player.PauseAsync();
                await ReplyAsync($"Paused!");
                return;
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("The given key"))
                {
                    await ReplyAsync("I'm not even connected...");
                    return;
                }

                await ReplyAsync("Something went wrong");
                return;
            }
        }

        [Command("resume")]
        [RequireBotPermission(GuildPermission.Connect)]
        public async Task Resume()
        {
            IGuildUser user = (Context.Guild.GetUser(665514955985911818) as IGuildUser);
            if (user.VoiceChannel is null && link.HasPlayer(Context.Guild as IGuild))
                await SlientLeft();

            try
            {
                var player = link.GetPlayer(Context.Guild as IGuild);

                if (player.PlayerState is PlayerState.Paused)
                    await player.ResumeAsync();

                await ReplyAsync($"Resumed! Now playing: {player.Track.Title}");
                return;
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("The given key"))
                {
                    await ReplyAsync("I'm not even connected...");
                    return;
                }

                await ReplyAsync("Something went wrong");
                return;
            }
        }
    }
}