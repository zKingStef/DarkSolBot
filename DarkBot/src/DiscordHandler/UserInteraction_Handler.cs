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
                case "dd_TicketPokecoins":
                case "dd_TicketStardust":
                case "dd_TicketXp":
                    Ticket_Handler.HandlePoGoTickets(e, selectedOption);
                    break;
                case "dd_TicketSupport":
                case "dd_TicketUnban":
                case "dd_TicketDonation":
                case "dd_TicketOwner":
                case "dd_TicketApplication":
                    Ticket_Handler.HandleGeneralTickets(e, selectedOption);
                    break;
                case "dd_RolePokemonGo":
                case "dd_RoleGamer":
                    AutoRole_Handler.GiveRoleToUser(e, selectedOption);
                    break;

                default:
                    await e.Channel.SendMessageAsync("Error occured in RespondToInteraction(). No Respond Handler Method found");
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
