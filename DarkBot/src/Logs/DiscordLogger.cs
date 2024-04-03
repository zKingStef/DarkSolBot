using DarkBot.src.Common;
using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkBot.src.Logs
{
    public class DiscordLogger
    {
        public static async Task Log_JoinLeave(DiscordClient sender, DSharpPlus.EventArgs.GuildMemberAddEventArgs e)
        {
            var welcomeChannel = e.Guild.GetChannel(978346565418770433);

            var welcomeEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Magenta,
                Title = $"Welcome {e.Member.Nickname} !",
                Description = $"{e.Member.Mention}, enjoy your stay here. Check out <@#1221919932372095178> to claim some Roles!",
                Timestamp = DateTimeOffset.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = e.Guild.Name
                },
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    IconUrl = e.Member.AvatarUrl,
                    Name = e.Member.Username
                }
            };

            await welcomeChannel.SendMessageAsync(embed: welcomeEmbed);
        }
    }
}
