using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkBot.src.CommandHandler;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Options;
using System.ComponentModel.Design;
using DarkBot.src.Common;
using System.Reflection;

namespace DarkBot.src.Handler
{
    public static class UserInteraction_Handler
    {
        public static async Task HandleInteraction(DiscordClient client, ComponentInteractionCreateEventArgs e)
        {
            var selectedOption = e.Interaction.Data.Values.FirstOrDefault();

            switch (selectedOption)
            {
                case "dd_RoleDarkServices":
                case "dd_RolePokemonGo":
                    AutoRole_Handler.GiveRoleToUser(e, selectedOption);
                    break;

                default:
                    break;
            }

            // Used for the New-Order Buttons
            var originalEmbed = e.Message.Embeds.FirstOrDefault();

            switch (e.Interaction.Data.CustomId)
            {
                // Cases for Ticket Buttons
                case "ticketPokemonGoBtn":
                    await Modals.CreatePokemonGoModal(e, "modalPokemonGoForm");
                    break;
                case "ticketTechnicBtn":
                    await Modals.CreateTechnicModal(e, "modalTechnicForm");
                    break;
                case "claimTicketButton":
                    if (Ticket_Handler.CheckIfUserHasTicketPermissions(e))
                    {
                        await Ticket_Handler.RemoveClaimButtonAsync(e);
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent($"Ticket claimed by {e.User.Mention}"));
                    }
                    break;
                case "closeTicketButton":
                    await Ticket_Handler.CloseTicket(e);
                    break;
                case "closeReasonTicketButton":
                    await Modals.CreateReasonModal(e, "modalCloseReasonForm");
                    break;

                // Cases for New-Order Buttons
                case "orderDeliverBtn":
                    
                    if (originalEmbed != null)
                    {

                        var newEmbed = new DiscordEmbedBuilder(originalEmbed)
                            .WithDescription(originalEmbed.Description.Replace(":orange_square: Delivery pending", ":green_square: Order delivered"));

                        // Original Buttons wieder hinzufügen
                        var orderDeliverBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "orderDeliverBtn", "✅ Order delivered");
                        var orderPendingBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "orderPendingBtn", "🕖 Delivery pending");
                        var orderCancelBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "orderCancelBtn", "❌ Order canceled");
                        var accDetailsBtn = new DiscordButtonComponent(ButtonStyle.Primary, "AccDetailsBtn", "🛃 Account Details");
                        var orderDetailsBtn = new DiscordButtonComponent(ButtonStyle.Primary, "OrderDetailsBtn", "🛄 Order Details");

                        var responseBuilder = new DiscordInteractionResponseBuilder()
                            .AddEmbed(newEmbed)
                            .AddComponents(orderDeliverBtn, orderPendingBtn, orderCancelBtn)
                            .AddComponents(accDetailsBtn, orderDetailsBtn);

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);
                    }
                    break;
                case "orderPendingBtn":
                    if (originalEmbed != null)
                    {
                        var newEmbed = new DiscordEmbedBuilder(originalEmbed)
                            .WithDescription(originalEmbed.Description.Replace(":green_square: Order delivered", ":orange_square: Delivery pending"));

                        // Original Buttons wieder hinzufügen
                        var orderDeliverBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "orderDeliverBtn", "✅ Order delivered");
                        var orderPendingBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "orderPendingBtn", "🕖 Delivery pending");
                        var orderCancelBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "orderCancelBtn", "❌ Order canceled");
                        var accDetailsBtn = new DiscordButtonComponent(ButtonStyle.Primary, "AccDetailsBtn", "🛃 Account Details");
                        var orderDetailsBtn = new DiscordButtonComponent(ButtonStyle.Primary, "OrderDetailsBtn", "🛄 Order Details");

                        var responseBuilder = new DiscordInteractionResponseBuilder()
                            .AddEmbed(newEmbed)
                            .AddComponents(orderDeliverBtn, orderPendingBtn, orderCancelBtn)
                            .AddComponents(accDetailsBtn, orderDetailsBtn);

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);
                    }
                    break;
                case "orderCancelBtn":
                    if (originalEmbed != null)
                    {
                        var newEmbed = new DiscordEmbedBuilder(originalEmbed)
                            .WithDescription(originalEmbed.Description.Replace(":orange_square: Delivery pending", ":red_square: Order canceled").Replace(":green_square: Order delivered", ":red_square: Order canceled"));

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(newEmbed));
                    }
                    break;
                case "AccDetailsBtn":
                    break;
                case "OrderDetailsBtn":
                    break;
                case "PictureBtn":
                    break;
                default:
                    Console.WriteLine(e.Message);
                    break;
            }
        }
    }
}