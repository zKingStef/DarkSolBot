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

            string DelPending = ":orange_square: Delivery pending";
            string InProgress = ":gear: In Progress";
            string OrdDel = ":green_square: Order delivered";
            string OrdCancel = ":red_square: Order canceled";

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
                case "Button_OrderDelivered":
                    
                    if (originalEmbed != null)
                    {

                        var newEmbed = new DiscordEmbedBuilder(originalEmbed)
                            .WithDescription(originalEmbed.Description
                            .Replace(DelPending, OrdDel)
                            .Replace(InProgress, OrdDel));

                        // Original Buttons wieder hinzufügen
                        var accDetailsBtn = new DiscordButtonComponent(ButtonStyle.Primary, "AccDetailsBtn", "🛃 Account Details");
                        var orderDetailsBtn = new DiscordButtonComponent(ButtonStyle.Primary, "OrderDetailsBtn", "🛄 Order Details");

                        var responseBuilder = new DiscordInteractionResponseBuilder()
                            .AddEmbed(newEmbed)
                            .AddComponents(accDetailsBtn, orderDetailsBtn);

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);
                    }
                    break;
                case "Button_InProgress":
                    if (originalEmbed != null)
                    {
                        var newEmbed = new DiscordEmbedBuilder(originalEmbed)
                            .WithDescription(originalEmbed.Description
                            .Replace(DelPending, InProgress));

                        // Original Buttons wieder hinzufügen
                        var orderDeliverBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_OrderDelivered", "✅ Order delivered");
                        var deliveryPendingBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_DeliveryPending", "🕖 Delivery pending");
                        var accDetailsBtn = new DiscordButtonComponent(ButtonStyle.Primary, "Button_AccDetails", "🛃 Account Details");
                        var orderDetailsBtn = new DiscordButtonComponent(ButtonStyle.Primary, "Button_OrderDetails", "🛄 Order Details");

                        var responseBuilder = new DiscordInteractionResponseBuilder()
                            .AddEmbed(newEmbed)
                            .AddComponents(orderDeliverBtn, inProgressBtn, deliveryPendingBtn)
                            .AddComponents(accDetailsBtn, orderDetailsBtn);

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);
                    }
                    break;
                case "DeliveryPendingBtn":
                    if (originalEmbed != null)
                    {
                        var newEmbed = new DiscordEmbedBuilder(originalEmbed)
                            .WithDescription(originalEmbed.Description.Replace(OrdDel, ":orange_square: Delivery pending"));

                        // Original Buttons wieder hinzufügen
                        var orderDeliverBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_OrderDelivered", "✅ Order delivered");
                        var inProgressBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_InProgress", "⚙️ In Progress");
                        var orderCancelBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_OrderCancel", "❌ Order canceled");
                        var accDetailsBtn = new DiscordButtonComponent(ButtonStyle.Primary, "Button_AccDetails", "🛃 Account Details");
                        var orderDetailsBtn = new DiscordButtonComponent(ButtonStyle.Primary, "Button_OrderDetails", "🛄 Order Details");

                        var responseBuilder = new DiscordInteractionResponseBuilder()
                            .AddEmbed(newEmbed)
                            .AddComponents(orderDeliverBtn, inProgressBtn, deliveryPendingBtn, orderCancelBtn)
                            .AddComponents(accDetailsBtn, orderDetailsBtn);

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);
                    }
                    break;
                case "Button_OrderCancel":
                    if (originalEmbed != null)
                    {
                        var newEmbed = new DiscordEmbedBuilder(originalEmbed)
                            .WithDescription(originalEmbed.Description
                            .Replace(DelPending, OrdCancel)
                            .Replace(OrdDel, OrdCancel)
                            .Replace(InPr, OrdCancel));

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(newEmbed));
                    }
                    break;
                case "Button_AccDetails":
                    break;
                case "Button_OrderDetails":
                    break;
                default:
                    Console.WriteLine(e.Message);
                    break;
            }
        }
    }
}