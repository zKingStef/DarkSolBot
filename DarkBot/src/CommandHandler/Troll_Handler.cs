using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkBot.src.CommandHandler
{
    public class Troll_Handler
    {
        public static async Task SendFunnyMessage(InteractionContext ctx)
        {
            string[] funnyMessages = {
            "Warum können Geister so schlecht lügen? Weil man durch sie hindurchsehen kann!",
            "Was ist orange und klingt wie ein Papagei? Eine Karotte!",
            "Hast du schon gehört? Der erste Computervirus wurde in einer E-Mail von einem Affen geschickt!"
        };

            var random = new Random();
            int index = random.Next(funnyMessages.Length);

            var embed = new DiscordEmbedBuilder()
                .WithTitle("Troll Alert!")
                .WithDescription(funnyMessages[index])
                .WithColor(DiscordColor.HotPink);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }

        public static async Task SendFunnyImage(InteractionContext ctx)
        {
            string[] funnyImages = {
            "https://i.imgur.com/8K3hYMe.png",
            "https://i.imgur.com/aFYIIxl.png",
            "https://i.imgur.com/ZhUPaQA.png"
        };

            var random = new Random();
            int index = random.Next(funnyImages.Length);

            var embed = new DiscordEmbedBuilder()
                .WithTitle("Troll Alert!")
                .WithImageUrl(funnyImages[index])
                .WithColor(DiscordColor.HotPink);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }

        public static async Task SendWeirdEmoji(InteractionContext ctx)
        {
            string[] weirdEmojis = {
            "🤡", "👾", "🦄", "🌈", "🍕"
        };

            var random = new Random();
            int index = random.Next(weirdEmojis.Length);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Troll Emoji: {weirdEmojis[index]}"));
        }
    }
}
