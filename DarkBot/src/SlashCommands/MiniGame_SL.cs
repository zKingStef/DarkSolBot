﻿using DarkBot.src.CommandHandler;
using DarkBot.src.Common;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkBot.src.SlashCommands
{
	public class MiniGame_SL : ApplicationCommandModule
	{
        [SlashCommand("coinflip", "Throw a Coin.")]
        public static async Task GetCoinFlip(InteractionContext ctx)
        {
            await CmdShortener.SendResponseAsync(ctx, $":coin:  **{ctx.User.Username}** threw a Coin and got **{Formatter.Bold(Convert.ToBoolean(new Random().Next(0, 2)) ? "Heads" : "Tails")}**").ConfigureAwait(false);
        }

        [SlashCommand("rockpaper", "Play a game of Rock-Paper-Scissors against the Bot.")]
        [Cooldown(1, 3, CooldownBucketType.Channel)]
        public async Task SSP(InteractionContext ctx,
                             [Choice("Scissors", "Scissors")]
                             [Choice("Rock", "Rock")]
                             [Choice("Paper", "Paper")]
                             [Option("Auswahl", "Rock/Paper/Scissors")] string choice)
        {
            string[] choices = new string[3] { "Rock", "Paper", "Scissors" };

            Random rnd = new Random();
            int n = rnd.Next(0, 12);
            if (n > 2)
            { n %= 3; }
            //the resulting win
            string[] resultStr = [$"**{ctx.User.Username}** won"/* 0 */, "It's a **Tie**" /* 1 */ , "Haha, you lost" /* 2 */];
            //emded for the winning result
            var resultEmbed = new DiscordEmbedBuilder { Title = "Rock-Paper-Scissors" };
            StringBuilder resultSb = new();

            resultSb.AppendLine($"{ctx.User.Username} chooses **{choice}**");
            resultSb.AppendLine($"Bot chooses **{choices[n]}**\n");
            resultSb.AppendLine(resultStr[MiniGame_Handler.HandleRpsResult(choice, choices[n])]);

            resultEmbed.Description = resultSb.ToString();

            switch (MiniGame_Handler.HandleRpsResult(choice, choices[n]))
            {
                case 0:
                    resultEmbed.WithThumbnail(ctx.User.AvatarUrl);
                    resultEmbed.Color = DiscordColor.SapGreen;
                    break;
                case 1:
                    resultEmbed.Color = DiscordColor.Cyan;
                    break;
                case 2:
                    resultEmbed.WithThumbnail("https://cdn.discordapp.com/embed/avatars/0.png");
                    resultEmbed.Color = DiscordColor.DarkRed;
                    break;
            }

            await ctx.Channel.SendMessageAsync(embed: resultEmbed.Build()).ConfigureAwait(false);
        }
    }
}
