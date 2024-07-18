using DarkBot.src.CommandHandler;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus;
using DSharpPlus.Interactivity;
using Microsoft.VisualBasic;

namespace DarkBot.src.Common
{
    public class Modals
    {
        public static async Task HandleModal(DiscordClient client, ModalSubmitEventArgs e)
        {
            if (e.Interaction.Type == InteractionType.ModalSubmit
             && e.Interaction.Data.CustomId == "modalPokemonGoForm")
            {
                await Ticket_Handler.HandleGeneralTickets(e);
            }
            if (e.Interaction.Type == InteractionType.ModalSubmit
             && e.Interaction.Data.CustomId == "modalPokecoin")
            {
                await Ticket_Handler.HandleGeneralTickets(e);
            }
            if (e.Interaction.Type == InteractionType.ModalSubmit
             && e.Interaction.Data.CustomId == "modalXP")
            {
                await Ticket_Handler.HandleGeneralTickets(e);
            }
            if (e.Interaction.Type == InteractionType.ModalSubmit
             && e.Interaction.Data.CustomId == "modalCloseReasonForm")
            {
                await Ticket_Handler.CloseTicket(e);
            }
        }

        public static async Task CreatePokemonGoModal(ComponentInteractionCreateEventArgs e, string modalId)
        {
            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("DarkSolutions")
                .WithCustomId(modalId)
                .AddComponents(
                    new TextInputComponent("What do you want to order", "orderTextBox", value: ""))
                .AddComponents(
                    new TextInputComponent("Payment Method", "paymethodTextBox", value: ""))
                .AddComponents(
                new TextInputComponent("Login Method (PTC, Google, FB)", "loginTextBox", value: ""));

            await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }

        public static async Task CreatePokecoinModal(ComponentInteractionCreateEventArgs e, string modalId)
        {
            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("DarkSolutions")
                .WithCustomId(modalId)
                .AddComponents(
                    new TextInputComponent("How many Coins do you want to order?", "orderTextBox", value: ""))
                .AddComponents(
                    new TextInputComponent("Payment Method", "paymethodTextBox", value: ""))
                .AddComponents(
                new TextInputComponent("Login Method (PTC, Google, FB)", "loginTextBox", value: ""));

            await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }

        public static async Task CreateXPModal(ComponentInteractionCreateEventArgs e, string modalId)
        {
            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("DarkSolutions")
                .WithCustomId(modalId)
                .AddComponents(
                    new TextInputComponent("How much XP do you want to order?", "orderTextBox", value: ""))
                .AddComponents(
                    new TextInputComponent("Payment Method", "paymethodTextBox", value: ""))
                .AddComponents(
                new TextInputComponent("Login Method (PTC, Google, FB)", "loginTextBox", value: ""));

            await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }


        public static async Task CreateReasonModal(ComponentInteractionCreateEventArgs e, string modalId)
        {
            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("Close Ticket")
                .WithCustomId(modalId)
                .AddComponents(
                    new TextInputComponent(label: "Reason", customId: "closeReasonTextBox", value: "")
                );

            await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }
    }
}
