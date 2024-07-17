using DSharpPlus;
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
        public static async Task SendPhoneDropdown(InteractionContext ctx)
        {
            var options = new List<DiscordSelectComponentOption>()
                {
                    new (
                        "Carbon OnePlus",
                        "dd_CarbonOnePlus",
                        "",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":mobile_phone:"))),
                    new (
                        "Hellblau OnePlus",
                        "dd_HellblauOnePlus",
                        "",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":mobile_phone:"))),
                    new (
                        "Google Pixel",
                        "dd_GooglePixel",
                        "",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":mobile_phone:"))),
                    new (
                        "No Phone",
                        "dd_NoPhone",
                        "",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":no_mobile_phones:"))),
                 };

            var phoneDropdown = new DiscordSelectComponent("phoneDropdown", "Choose a Phone", options, false, 0, 1);

            var dropdownEmbed = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.SpringGreen)
                .WithTitle("Phone")
                .WithDescription("Which Phone is being used for this Service?")
                )
                .AddComponents(phoneDropdown);

            await ctx.Channel.SendMessageAsync(dropdownEmbed);
        }
    }
}
