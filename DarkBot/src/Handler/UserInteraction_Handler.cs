using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkBot.src.CommandHandler;

namespace DarkBot.src.Handler
{
    public static class UserInteraction_Handler
    {
        public static async Task RespondToInteraction(ComponentInteractionCreateEventArgs e)
        {
            var selectedOption = e.Interaction.Data.Values.FirstOrDefault();

            switch (selectedOption)
            {
                case "ticketPokecoinDropdown":
                case "ticketStardustDropdown":
                case "ticketXpDropdown":
                    Ticket_Handler.HandlePoGoTickets(e, selectedOption);
                    break;
                case "ticketSupportDropdown":
                case "ticketUnbanDropdown":
                case "ticketDonationDropdown":
                case "ticketOwnerDropdown":
                case "ticketApplyDropdown":
                    Ticket_Handler.HandleGeneralTickets(e, selectedOption);
                    break;
                case "dd_RolePokemonGo":
                case "dd_RoleGamer":
                    AutoRole_Handler.GiveRoleToUser(e, selectedOption);
                    break;
            }

            switch (e.Interaction.Data.CustomId)
            {
                case "entryGiveawayButton":
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                    .WithContent("Du bist dem **Gewinnspiel** erfolgreich beigetreten! Viel Glück:tada:").AsEphemeral(true));
                    break;

                default:
                    //await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Ein Fehler ist aufgetreten. Bitte kontaktiere einen <@&1210230414011011124>"));
                    break;
            }
        }
    }
}
