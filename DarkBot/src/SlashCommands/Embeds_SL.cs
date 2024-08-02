using DarkBot.src.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using static System.Net.Mime.MediaTypeNames;
using DSharpPlus.CommandsNext;
using System.Reflection;
using DSharpPlus.Interactivity.Extensions;

namespace DarkBot.src.SlashCommands
{
    [SlashCommandGroup("embed", "Send an embed message by the Bot")]
    public class Embeds_SL : ApplicationCommandModule
    {
        [SlashCommand("services", "service display")]
        public static async Task SendShopEmbed(InteractionContext ctx,
                                [Choice("pokecoins", 0)]
                                [Choice("xp-service", 1)]
                                [Choice("raids", 2)]
                                [Choice("shundo", 3)]
                                [Choice("comday", 4)]
                                [Choice("100iv", 5)]
                                [Choice("raidpass", 6)]
                                [Choice("stardust", 7)]
                                [Option("form", "Choose an embed")] long choice)
        {
            if (!CmdShortener.CheckPermissions(ctx, Permissions.ManageEvents))
            {
                await CmdShortener.SendAsEphemeral(ctx, "You don't have the necessary permissions to execute this command");
                return;
            }

            var embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("");
            string buttonName = "Button_Error";

            switch (choice)
            {
                case 0:
                    embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("__Pokecoin Service__")
                    .WithColor(DiscordColor.Yellow)
                    .WithDescription("**What is the process?**\n" +
                                     "After you confirm the payment and send me the login details the coins will be added to your account within a few minutes.\n\n" +
                                     "**Is it safe?**\n" +
                                     "Absolutely! All transactions are conducted through the original App, ensuring your account remains safe and risk-free.\n\n" +
                                     "**Do I need to share my login information?**\n" +
                                     "Yes, for this service, we require access to your account.\n\n" +
                                     "**Can I access my account during this process?**\n" +
                                     "Yes, you can stay logged in while we add the coins.\n\n");

                    buttonName = "Button_TicketPokecoin";
                    break;
                case 1:
                    embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("__XP Service__")
                    .WithColor(DiscordColor.Cyan)
                    .WithDescription("**What is the process?**\n" +
                                     "I will catch all pokemon from the wild including spinning pokestops, completing research takes and hatching eggs.\n\n" +
                                     "**How long does it take before I can login to my account again?**\n" +
                                     "You can log back in to your account 2h after we've send you a message that we finished the service.\n\n" +
                                     "**Is it safe?**\n" +
                                     "The method that we use is considered the safest method. We would not offer our services if we didn't consider them safe. Account safety if our top priority\n\n" +
                                     "**Do I need to share my login information?**\n" +
                                     "Yes, for this service, we require access to your account.\n\n" +
                                     "**Can I access my account during this process?**\n" +
                                     "To ensure security, it's important that you stay logged out during the process.\n\n");

                    buttonName = "Button_TicketXP";
                    break;
                case 2:
                    embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("__Raid Service__")
                    .WithColor(DiscordColor.Green)
                    .WithDescription("**What is the process?**\n" +
                                     "You can select any raid that is currently available. Event Raid Days are also possible. **The Raid Passes have to be in your Inventory**!\n\n" +
                                     "**How long does it take before I can login to my account again?**\n" +
                                     "You can log back in to your account 2h after we've send you a message that we finished the service.\n\n" +
                                     "**Is it safe?**\n" +
                                     "The method that we use is considered the safest method. We would not offer our services if we didn't consider them safe. Account safety if our top priority\n\n" +
                                     "**Do I need to share my login information?**\n" +
                                     "Yes, for this service, we require access to your account.\n\n" +
                                     "**Can I access my account during this process?**\n" +
                                     "To ensure security, it's important that you stay logged out during the process.\n\n");

                    buttonName = "Button_TicketRaids";
                    break;
                case 3:
                    embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("__Shundo Service__")
                    .WithColor(DiscordColor.White)
                    .WithDescription("**What is the process?**\n" +
                                     "The exact amount of Shundos you bought will be caught on your Account. Each Shundo takes about 2 hours.\n" +
                                     "The Pokemon you get will be random but it's also possible to select specific ones. List of available Shinys -> https://discord.com/channels/978346565209042984/1258359048944750613/1263734314437443655\n\n" +
                                     "**What is a Shundo?**\n" +
                                     "Shundo is a term used in PokemonGo that combines Shiny and Hundo. So, a Pokemon that is shiny and also has perfect IVs is called a Shundo\n\n" +
                                     "**How long does it take before I can login to my account again?**\n" +
                                     "You can log back in to your account 2h after we've send you a message that we finished the service.\n\n" +
                                     "**Is it safe?**\n" +
                                     "The method that we use is considered the safest method. We would not offer our services if we didn't consider them safe. Account safety if our top priority\n\n" +
                                     "**Do I need to share my login information?**\n" +
                                     "Yes, for this service, we require access to your account.\n\n" +
                                     "**Can I access my account during this process?**\n" +
                                     "To ensure security, it's important that you stay logged out during the process.\n\n");

                    buttonName = "Button_TicketShundo";
                    break;
                case 4:
                    embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("__Community Day Service__")
                    .WithColor(DiscordColor.IndianRed)
                    .WithDescription("**What is the process?**\n" +
                                     "This Service includes the full 3 hours of the Community Day." +
                                     "The average loot in this time is:" +
                                     "\n- 70+ Shinys\n- 1000 XL Candy (mega evolution needed for more XL)\n- 20000 Candy\n- 2 Million Stardust\n- 25 Million XP\n\n" +
                                     "Additionally you can order one or multiple Shundos of the Com-Day Pokemon.\n\n" +
                                     "**How long does it take before I can login to my account again?**\n" +
                                     "You can log back in to your account 2h after we've send you a message that we finished the service.\n\n" +
                                     "**Is it safe?**\n" +
                                     "The method that we use is considered the safest method. We would not offer our services if we didn't consider them safe. Account safety if our top priority\n\n" +
                                     "**Do I need to share my login information?**\n" +
                                     "Yes, for this service, we require access to your account.\n\n" +
                                     "**Can I access my account during this process?**\n" +
                                     "To ensure security, it's important that you stay logged out during the process.\n\n");

                    buttonName = "Button_TicketComday";
                    break;
                case 5:
                    embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("__100IV Service__")
                    .WithColor(DiscordColor.Orange)
                    .WithDescription("**What is the process?**\n" +
                                     "I will catch any specific Pokemon with Perfect IVs for you.\n" +
                                     "Not available: Legendary/Mythical/Shadow\n\n" +
                                     "**How long does it take before I can login to my account again?**\n" +
                                     "You can log back in to your account 2h after we've send you a message that we finished the service.\n\n" +
                                     "**Is it safe?**\n" +
                                     "The method that we use is considered the safest method. We would not offer our services if we didn't consider them safe. Account safety if our top priority\n\n" +
                                     "**Do I need to share my login information?**\n" +
                                     "Yes, for this service, we require access to your account.\n\n" +
                                     "**Can I access my account during this process?**\n" +
                                     "To ensure security, it's important that you stay logged out during the process.\n\n");

                    buttonName = "Button_Ticket100IV";
                    break;
                case 6:
                    embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("__Raidpass Service__")
                    .WithColor(DiscordColor.SapGreen)
                    .WithDescription("**What is the process?**\n" +
                                     "With referral feature, i will create clone accounts, add friend and do referral task.\n" +
                                     "All you have to do is accept my clone's friend requests, claim the passes and delete friends.\n" +
                                     "- Minimum order is 100 Passes!\n" +
                                     "- Make sure to have space on your friendlist (100 Passes = 100 friends)\n\n" +
                                     "**Is it safe?**\n" +
                                     "There is no risk at all because i am not logged in your account.\n\n" +
                                     "**Do I need to share my login information?**\n" +
                                     "No, i just need your friend code and referral code.\n\n" +
                                     "**Can I access my account during this process?**\n" +
                                     "Yes, you can play the game normally.");

                    buttonName = "Button_TicketRaidpass";
                    break;
                case 7:
                    embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("__Stardust Service__")
                    .WithColor(DiscordColor.HotPink)
                    .WithDescription("**What is the process?**\n" +
                                     "I can get upto 2 Million Stardust per day by mass catching pokemon.\n\n" +
                                     "**How long does it take before I can login to my account again?**\n" +
                                     "You can log back in to your account 2h after we've send you a message that we finished the service.\n\n" +
                                     "**Is it safe?**\n" +
                                     "The method that we use is considered the safest method. We would not offer our services if we didn't consider them safe. Account safety if our top priority\n\n" +
                                     "**Do I need to share my login information?**\n" +
                                     "Yes, for this service, we require access to your account.\n\n" +
                                     "**Can I access my account during this process?**\n" +
                                     "To ensure security, it's important that you stay logged out during the process.\n\n");

                    buttonName = "Button_TicketStardust";
                    break;
                case 8:
                    break;
                case 9:
                    break;
            }

            var createTicketBtn = new DiscordButtonComponent(ButtonStyle.Primary, buttonName, "📩 Create Ticket");

            var messageBuilder = new DiscordMessageBuilder()
                    .WithEmbed(embedTicketButtons)
                    .AddComponents(createTicketBtn);

            await ctx.Channel.SendMessageAsync(messageBuilder);
        }

        //
        [SlashCommand("license", "add license")]
        public static async Task SendLicenseEmbed(InteractionContext ctx,
                                [Choice("PAC", 0)]
                                [Choice("Shungo", 1)] 
                                [Option("form", "Choose an embed")] long choice,
                                [Option("License", "License ID/Name")] string license)
        {
            if (!CmdShortener.CheckPermissions(ctx, Permissions.ManageEvents))
            {
                await CmdShortener.SendAsEphemeral(ctx, "You don't have the necessary permissions to execute this command");
                return;
            }

            var embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("");

            switch (choice)
            {
                case 0:
                    embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("PAC License: " + license)
                    .WithColor(DiscordColor.Yellow)
                    .WithThumbnail("https://management.pgtools.net/static/media/logo.94746d37694b283b5f0b.png")
                    .WithDescription(":bust_in_silhouette: Used by: None\n\n" +
                                     ":mobile_phone: Used Phone: None\n\n" +
                                     ":calendar_spiral: Used since: <t:1722605220:R>");

                    break;
                case 1:
                    embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("Shungo License")
                    .WithColor(DiscordColor.White)
                    .WithThumbnail("https://www.pokewiki.de/images/0/0d/Sugimori_Premierball.png")
                    .WithDescription(":bust_in_silhouette: Used by: None\n\n" +
                                     ":mobile_phone: Used Phone: None\n\n" +
                                     ":calendar_spiral: Used since: <t:1722605220:R>");

                    break;
            }

            var TimerBtn = new DiscordButtonComponent(ButtonStyle.Primary, "Button_ResetTimer", "⌛ Reset Timer");

            var messageBuilder = new DiscordMessageBuilder()
                    .WithEmbed(embedTicketButtons)
                    .AddComponents(TimerBtn);


            var channel = ctx.Guild.GetChannel(1268820158835724372);
            var message =  await channel.SendMessageAsync(messageBuilder);

            await CmdShortener.SendAsEphemeral(ctx, "New License has been created!  " + message.JumpLink);

            // 
            var options = new List<DiscordSelectComponentOption>()
                {
                    new ( "Carbon OnePlus", "dd_CarbonOnePlus", "",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":mobile_phone:"))),
                    new ( "Hellblau OnePlus", "dd_HellblauOnePlus", "",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":mobile_phone:"))),
                    new ( "Google Pixel", "dd_GooglePixel", "",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":mobile_phone:"))),
                    new ( "No Phone", "dd_NoPhone", "",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":no_mobile_phones:"))),
                 };

            var phoneDropdown = new DiscordSelectComponent("phoneDropdown", "", options, false, 0, 1);

            var builder = new DiscordMessageBuilder()
                .WithContent("**Phone:**")
                .AddComponents(phoneDropdown);

            await channel.SendMessageAsync(builder);

            //
            ulong categoryId = 1263000023822762035;

            var category = ctx.Guild.GetChannel(categoryId);
            if (category == null || category.Type != ChannelType.Category)
            {
                await ctx.Channel.SendMessageAsync("Category not found!");
                return;
            }

            var channels = category.Children
                    .Where(c => c.Type == ChannelType.Text) // Nur Textkanäle anzeigen
                    .Select(c => new DiscordSelectComponentOption(c.Name, c.Id.ToString(), $"Channel ID: {c.Id}"))
                    .ToList();

            if (channels.Count == 0)
            {
                await ctx.Channel.SendMessageAsync("No channels in this category!");
                return;
            }

            var selectMenu = new DiscordSelectComponent(
                    customId: "channelDropdown",
                    placeholder: "",
                    options: channels,
                    minOptions: 1,
                    maxOptions: 1
                );

            // Nachricht mit Dropdown-Menü senden
            var builder2 = new DiscordMessageBuilder()
                .WithContent("**User:**")
                .AddComponents(selectMenu);

            await ctx.Channel.SendMessageAsync(builder2);
        }
    }
}
