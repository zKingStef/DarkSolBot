using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using DarkBot.src.Handler;
using DarkBot.src.Common;
using DarkBot.src.CommandHandler;

namespace DarkBot.src.SlashCommands
{
    [SlashCommandGroup("ticket", "Alle Ticket Befehle")]
    public class Ticket_SL : ApplicationCommandModule
    {
        [SlashCommand("system", "Ticket System")]
        public static async Task Ticketsystem(InteractionContext ctx,
                                [Choice("PokemonGo", 0)]
                                [Choice("Technic", 1)]
                                [Option("form", "Choose a form")] long systemChoice)
        {
            // Pre Execution Checks
            await CmdShortener.CheckIfUserHasCeoRole(ctx);

            if (systemChoice == 0)
            {
                var embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("**Pokemon Go Service**")
                    .WithColor(DiscordColor.CornflowerBlue)
                    .WithDescription("Open a Ticket here")
                    .WithImageUrl("https://i.ebayimg.com/images/g/TncAAOSwz7FfP~5R/s-l400.jpg");

                var pokemonGoBtn = new DiscordButtonComponent(ButtonStyle.Primary, "Button_TicketPokemonGo", "📩 Create Ticket");

                var messageBuilder = new DiscordMessageBuilder()
                    .WithEmbed(embedTicketButtons)
                    .AddComponents(pokemonGoBtn);

                await ctx.Channel.SendMessageAsync(messageBuilder);
            }
            else if (systemChoice == 1)
            {
                ;
            }
            else if (systemChoice == 2)
            {
                ;
            }
            else if (systemChoice == 3)
            {
                ;
            }
        }

        [SlashCommand("add", "Add a User to the Ticket")]
        [RequireRoles(RoleCheckMode.Any, "🧰 CEO")]
        public async Task Add(InteractionContext ctx,
                             [Option("User", "The user which will be added to the ticket")] DiscordUser user)
        {
            // Pre Execution Checks
            await Ticket_Handler.CheckIfUserHasTicketPermissions(ctx);
            await CheckIfChannelIsTicket(ctx);

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "Ticket",
                Description = $"{user.Mention} has been added to the ticket by {ctx.User.Mention}!\n",
                Timestamp = DateTime.UtcNow
            };
            await ctx.CreateResponseAsync(embedMessage);

            await ctx.Channel.AddOverwriteAsync((DiscordMember)user, Permissions.AccessChannels);
        }

        [SlashCommand("remove", "Remove a User from the Ticket")]
        [RequireRoles(RoleCheckMode.Any, "🧰 CEO")]
        public async Task Remove(InteractionContext ctx,
                                [Option("User", "The user, which will be removed from the ticket")] DiscordUser user)
        {
            // Pre Execution Checks
            await Ticket_Handler.CheckIfUserHasTicketPermissions(ctx);
            await CheckIfChannelIsTicket(ctx);

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "Ticket",
                Description = $"{user.Mention} has been removed from the ticket by {ctx.User.Mention}!\n",
                Timestamp = DateTime.UtcNow
            };
            await ctx.CreateResponseAsync(embedMessage);

            await ctx.Channel.AddOverwriteAsync((DiscordMember)user, Permissions.None);
        }

        [SlashCommand("rename", "Ändere den Namen eines Tickets")]
        [RequireRoles(RoleCheckMode.Any, "🧰 CEO")]
        public async Task Rename(InteractionContext ctx,
                             [Option("Name", "Neuer Ticketname")] string newChannelName)
        {
            // Pre Execution Checks
            await Ticket_Handler.CheckIfUserHasTicketPermissions(ctx);
            await CheckIfChannelIsTicket(ctx);

            var oldChannelName = ctx.Channel.Mention;

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "Ticket",
                Description = $"Ticket {ctx.Channel.Mention} renamed by {ctx.User.Mention}!\n\n" +
                              $"New Ticketname: ```{newChannelName}```",
                Timestamp = DateTime.UtcNow
            };

            await ctx.CreateResponseAsync(embedMessage);

            await ctx.Channel.ModifyAsync(properties => properties.Name = newChannelName);
        }


        private async Task<bool> CheckIfChannelIsTicket(InteractionContext ctx)
        {
            const ulong categoryId = 1207086767623381092;

            if (ctx.Channel.Parent.Id != categoryId || ctx.Channel.Parent == null)
            {
                await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent(":warning: **This command is for tickets only!**").AsEphemeral(true));

                return true;
            }

            return false;
        }
    }
}
