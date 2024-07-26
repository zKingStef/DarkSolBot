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
            if (e.Interaction.Type == InteractionType.ModalSubmit)
            {
                // Liste der CustomIds, die dieselbe Handler-Methode aufrufen
                var customIdsForGeneralTickets = new HashSet<string>
                {
                    "modalPokemonGoForm",
                    "modalPokecoin",
                    "modalXP",
                    "modalRaids",
                    "modalShundo",
                    "modalComday",
                    "modal100IV",
                    "modalRaidpass",
                    "modalStardust"
                };

                // Überprüfen, ob die CustomId in der Sammlung enthalten ist
                if (customIdsForGeneralTickets.Contains(e.Interaction.Data.CustomId))
                {
                    await Ticket_Handler.HandleGeneralTickets(e);
                }
                // Spezielle Behandlung für modalCloseReasonForm
                else if (e.Interaction.Data.CustomId == "modalCloseReasonForm")
                {
                    await Ticket_Handler.CloseTicket(e);
                }
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

        public static async Task CreateRaidsModal(ComponentInteractionCreateEventArgs e, string modalId)
        {
            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("DarkSolutions")
                .WithCustomId(modalId)
                .AddComponents(
                    new TextInputComponent("How many Raids do you want to order?", "orderTextBox", value: ""))
                .AddComponents(
                    new TextInputComponent("Which Raid Pokemon(s)?", "raidpokeTextBox", value: ""))
                .AddComponents(
                    new TextInputComponent("Payment Method", "paymethodTextBox", value: ""))
                .AddComponents(
                new TextInputComponent("Login Method (Trainer-Club, Google, Facebook)", "loginTextBox", value: ""));

            await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }

        public static async Task CreateShundoModal(ComponentInteractionCreateEventArgs e, string modalId)
        {
            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("DarkSolutions")
                .WithCustomId(modalId)
                .AddComponents(
                    new TextInputComponent("How many Shundos do you want?", "orderTextBox", value: ""))
                .AddComponents(
                    new TextInputComponent("Payment Method", "paymethodTextBox", value: ""))
                .AddComponents(
                new TextInputComponent("Login Method (Trainer-Club, Google, Facebook)", "loginTextBox", value: ""));

            await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }

        public static async Task CreateComdayModal(ComponentInteractionCreateEventArgs e, string modalId)
        {
            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("DarkSolutions")
                .WithCustomId(modalId)
                .AddComponents(
                    new TextInputComponent("Additional ComDay Shundo ? (Y/N)", "orderTextBox", value: ""))
                .AddComponents(
                    new TextInputComponent("Payment Method", "paymethodTextBox", value: ""))
                .AddComponents(
                new TextInputComponent("Login Method (Trainer-Club, Google, Facebook)", "loginTextBox", value: ""));

            await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }

        public static async Task Create100IVModal(ComponentInteractionCreateEventArgs e, string modalId)
        {
            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("DarkSolutions")
                .WithCustomId(modalId)
                .AddComponents(
                    new TextInputComponent("Which Pokemon do you want ?", "orderTextBox", value: ""))
                .AddComponents(
                    new TextInputComponent("Payment Method", "paymethodTextBox", value: ""))
                .AddComponents(
                new TextInputComponent("Login Method (Trainer-Club, Google, Facebook)", "loginTextBox", value: ""));

            await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }

        public static async Task CreateRaidpassModal(ComponentInteractionCreateEventArgs e, string modalId)
        {
            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("DarkSolutions")
                .WithCustomId(modalId)
                .AddComponents(
                    new TextInputComponent("How many Raidpasses ?", "orderTextBox", value: ""))
                .AddComponents(
                    new TextInputComponent("Payment Method", "paymethodTextBox", value: ""));

            await e.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }

        public static async Task CreateStardustModal(ComponentInteractionCreateEventArgs e, string modalId)
        {
            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("DarkSolutions")
                .WithCustomId(modalId)
                .AddComponents(
                    new TextInputComponent("How much Stardust ?", "orderTextBox", value: ""))
                .AddComponents(
                    new TextInputComponent("Payment Method", "paymethodTextBox", value: ""))
                .AddComponents(
                new TextInputComponent("Login Method (Trainer-Club, Google, Facebook)", "loginTextBox", value: ""));

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
