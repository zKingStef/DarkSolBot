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
                                [Option("form", "Choose a embed")] long choice)
        {
            // Pre Execution Checks
            await CmdShortener.CheckIfUserHasCeoRole(ctx);

            if (choice == 0)
            {
                var embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("")
                    .WithColor(DiscordColor.Yellow)
                    .WithDescription("**What is the process?**\n" +
                                     "After you confirm the payment and send me the login details the coins will be added to your account within a few minutes.\n\n" +
                                     "**Is it safe?**\n" +
                                     "Absolutely! All transactions are conducted through the original App, ensuring your account remains safe and risk-free.\n\n" +
                                     "**Do I need to share my login information?**\n" +
                                     "Yes, for this offer, we require access to your account.\n\n" +
                                     "**Can I access my account during this process?**\n" +
                                     "To ensure security, it's important that you stay logged out during the process. The security of your account is the highest priority.\n\n" +
                                     "**What login methods are accepted?**\n" +
                                     "Google, Facebook, or the Pokemon Trainer Club (PTC). PTC can now be linked and unlinked at any time, offering flexibility and security for both parties.\n\n" +
                                     "**Don't hesitate to send me a message before purchasing if you have any questions.**");

                var createTicketBtn = new DiscordButtonComponent(ButtonStyle.Primary, "Button_TicketPokecoin", "📩 Create Ticket");

                var messageBuilder = new DiscordMessageBuilder()
                    .WithEmbed(embedTicketButtons)
                    .AddComponents(createTicketBtn);

                await ctx.Channel.SendMessageAsync(messageBuilder);
            }
            else if (choice == 1)
            {
                Console.WriteLine("XP");
            }
            else if (choice == 2)
            {
                ;
            }
            else if (choice == 3)
            {
                ;
            }
        }
    }
}
