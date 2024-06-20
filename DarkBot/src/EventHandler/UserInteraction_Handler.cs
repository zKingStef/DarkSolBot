﻿using DSharpPlus.Entities;
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
                    break;
                case "orderPendingBtn":
                    break;
                case "orderCancelBtn":
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