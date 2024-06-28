using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkBot.src.CommandHandler
{
    public class DarkSolutions_Handler
    {
        public static async Task CreatePokecoinDropdown(InteractionContext ctx)
        {
            var options = new List<DiscordSelectComponentOption>()
                {
                    new (
                        "1200 Pokecoins",
                        "dd_1200coins",
                        "Pokecoin Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Pokecoin1:"))),
                    new (
                        "2500 Pokecoins",
                        "dd_2500coins",
                        "Pokecoin Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Pokecoin1:"))),
                    new (
                        "5200 Pokecoins",
                        "dd_5200coins",
                        "Pokecoin Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Pokecoin1:"))),
                    new (
                        "14500 Pokecoins",
                        "dd_14500coins",
                        "Pokecoin Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Pokecoin1:"))),
                    new (
                        "29000 Pokecoins",
                        "dd_29000coins",
                        "Pokecoin Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Pokecoin1:"))),
                 };

            var artQtyDropdown = new DiscordSelectComponent("Dropdown_ArticleQuantity", "Article Quantity", options, false, 0, 1);

            var embedartQtyDropdown = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.HotPink)
                .WithTitle("Please select the Amount of Pokecoins")
                )
                .AddComponents(artQtyDropdown);

            await ctx.Channel.SendMessageAsync(embedartQtyDropdown);
        }

        public static async Task CreateStardustDropdown(InteractionContext ctx)
        {
            var options = new List<DiscordSelectComponentOption>()
                {
                    new (
                        "1 Million Stardust",
                        "dd_1millstardust",
                        "Stardust Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Stardust:"))),
                    new (
                        "2 Million Stardust",
                        "dd_2millstardust",
                        "Stardust Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Stardust:"))),
                    new (
                        "5 Million Stardust",
                        "dd_5millstardust",
                        "Stardust Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Stardust:"))),
                    new (
                        "10 Million Stardust",
                        "dd_10millstardust",
                        "Stardust Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Stardust:"))),
                    new (
                        "25 Million Stardust",
                        "dd_25millstardust",
                        "Stardust Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Stardust:"))),
                 };

            var artQtyDropdown = new DiscordSelectComponent("Dropdown_ArticleQuantity", "Article Quantity", options, false, 0, 1);

            var embedartQtyDropdown = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.HotPink)
                .WithTitle("Please select the Amount of Stardust")
                )
                .AddComponents(artQtyDropdown);

            await ctx.Channel.SendMessageAsync(embedartQtyDropdown);
        }

        public static async Task CreateXpDropdown(InteractionContext ctx)
        {
            var options = new List<DiscordSelectComponentOption>()
                {
                    new (
                        "4 Million XP",
                        "dd_4millxp",
                        "XP Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Level40:"))),
                    new (
                        "8 Million XP",
                        "dd_8millxp",
                        "XP Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Level40:"))),
                    new (
                        "16 Million XP",
                        "dd_16millxp",
                        "XP Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Level40:"))),
                    new (
                        "32 Million XP",
                        "dd_32millxp",
                        "XP Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Level40:"))),
                 };

            var artQtyDropdown = new DiscordSelectComponent("Dropdown_ArticleQuantity", "Article Quantity", options, false, 0, 1);

            var embedartQtyDropdown = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.HotPink)
                .WithTitle("Please select the Amount of XP")
                )
                .AddComponents(artQtyDropdown);

            await ctx.Channel.SendMessageAsync(embedartQtyDropdown);
        }

        public static async Task CreateRaidsDropdown(InteractionContext ctx)
        {
            var options = new List<DiscordSelectComponentOption>()
                {
                    new (
                        "20 Raids",
                        "dd_20raids",
                        "Raid Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Pokecoin1:"))),
                    new (
                        "40 Raids",
                        "dd_40raids",
                        "Raid Service",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":Pokecoin1:"))),
                 };

            var artQtyDropdown = new DiscordSelectComponent("Dropdown_ArticleQuantity", "Article Quantity", options, false, 0, 1);

            var embedartQtyDropdown = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.HotPink)
                .WithTitle("Please select the Amount of Raids")
                )
                .AddComponents(artQtyDropdown);

            await ctx.Channel.SendMessageAsync(embedartQtyDropdown);
        }
    }
}
