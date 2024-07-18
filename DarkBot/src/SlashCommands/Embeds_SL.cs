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

namespace DarkBot.src.SlashCommands
{
    public class Embeds_SL : ApplicationCommandModule
    {
        [SlashCommand("embeds", "Send an embed message by the bot")]
        public static async Task SendEmbed(InteractionContext ctx,
                                [Choice("pokecoins", 0)]
                                [Choice("xp-service", 1)]
                                [Choice("raids", 2)]
                                [Option("form", "Choose a embed")] long choice)
        {
            // Pre Execution Checks
            await CmdShortener.CheckIfUserHasCeoRole(ctx);

            var embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("");
            string buttonName = "Button_Error";

            switch (choice)
            {
                case 0:
                    embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("")
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
                    .WithTitle("")
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
                    .WithTitle("")
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
                    break;
                case 4:
                    break;
            }

            var createTicketBtn = new DiscordButtonComponent(ButtonStyle.Primary, buttonName, "📩 Create Ticket");

            var messageBuilder = new DiscordMessageBuilder()
                    .WithEmbed(embedTicketButtons)
                    .AddComponents(createTicketBtn);

            await ctx.Channel.SendMessageAsync(messageBuilder);
        }
    }
}
