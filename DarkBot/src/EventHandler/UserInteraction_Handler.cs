using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus;
using DarkBot.src.CommandHandler;
using DarkBot.src.Common;
using System.Collections.Concurrent;

namespace DarkBot.src.Handler
{
    public static class UserInteraction_Handler
    {
        public static async Task HandleInteraction(DiscordClient client, ComponentInteractionCreateEventArgs e)
        {
            var selectedOption = e.Interaction.Data.Values.FirstOrDefault();

            switch (selectedOption)
            {
                case "dd_CarbonOnePlus":
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                                                            new DiscordInteractionResponseBuilder().WithContent("Carbon OnePlus used for this Account!"));
                    break;
                case "dd_HellblauOnePlus":
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                                                            new DiscordInteractionResponseBuilder().WithContent("Hellblau OnePlus used for this Account!"));
                    break;
                case "dd_GooglePixel":
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                                                            new DiscordInteractionResponseBuilder().WithContent("Google Pixel used for this Account!"));
                    break;
                case "dd_NoPhone":
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                                                            new DiscordInteractionResponseBuilder().WithContent("No Phone needed for this Account!"));
                    break;

                default:
                    break;
            }

            // Used for the New-Order Buttons
            var originalEmbed = e.Message.Embeds.FirstOrDefault();

            string delPending = ":orange_square: Delivery pending";
            string inProgress = ":gear: In Progress";
            string ordDel = ":green_square: Order delivered";
            string ordCancel = ":red_square: Order canceled";
            string progressPaused = ":pause_button: Progress paused";
            string startProcess = ":no_entry: Process not started";

            switch (e.Interaction.Data.CustomId)
            {
                // Cases for Ticket Buttons
                case "Button_TicketPokemonGo":
                    await Modals.CreatePokemonGoModal(e, "modalPokemonGoForm");
                    break;
                case "Button_TicketPokecoin":
                    await Modals.CreatePokecoinModal(e, "modalPokecoin");
                    break;
                case "Button_TicketXP":
                    await Modals.CreateXPModal(e, "modalXP");
                    break;
                case "Button_TicketRaids":
                    await Modals.CreateRaidsModal(e, "modalRaids");
                    break;
                case "Button_TicketShundo":
                    await Modals.CreateShundoModal(e, "modalShundo");
                    break;
                case "Button_TicketComday":
                    await Modals.CreateComdayModal(e, "modalComday");
                    break;
                case "Button_Ticket100IV":
                    await Modals.Create100IVModal(e, "modalComday");
                    break;
                case "Button_TicketRaidpass":
                    await Modals.CreateRaidpassModal(e, "modalComday");
                    break;
                case "Button_TicketStardust":
                    await Modals.CreateStardustModal(e, "modalStardust");
                    break;

                case "closeTicketButton":
                    await Ticket_Handler.CloseTicket(e);
                    break;
                case "closeReasonTicketButton":
                    await Modals.CreateReasonModal(e, "modalCloseReasonForm");
                    break;

                // Cases for New-Order Buttons
                case "Button_StartProcess":

                    if (originalEmbed != null)
                    {

                        var newEmbed = new DiscordEmbedBuilder(originalEmbed)
                            .WithDescription(originalEmbed.Description
                            .Replace(startProcess, inProgress));

                        // Original Buttons wieder hinzufügen
                        var orderDeliverBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_OrderDelivered", "✅ Complete Order");
                        var progressPausedBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_ProgressPaused", "⏸️ Pause Progress");

                        var responseBuilder = new DiscordInteractionResponseBuilder()
                            .AddEmbed(newEmbed)
                            .AddComponents(orderDeliverBtn, progressPausedBtn);

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);

                        var originalChannelName = e.Channel.Name;

                        // Überprüfen, ob der Kanalname nicht leer ist
                        if (!string.IsNullOrEmpty(originalChannelName) && originalChannelName.Length > 1)
                        {
                            // Das erste Zeichen vom Kanalnamen entfernen
                            var newChannelName = string.Concat("🔧", originalChannelName.AsSpan(1));

                            // Kanalname aktualisieren
                            await e.Channel.ModifyAsync(properties => properties.Name = newChannelName);
                        }
                        else
                        {
                            // Fallback, wenn der Kanalname leer oder zu kurz ist
                            var newChannelName = "🔧";
                            await e.Channel.ModifyAsync(properties => properties.Name = newChannelName);
                        }
                    }
                    break;
                case "Button_OrderDelivered":

                    if (originalEmbed != null)
                    {

                        var newEmbed = new DiscordEmbedBuilder(originalEmbed)
                            .WithDescription(originalEmbed.Description
                            .Replace(delPending, ordDel)
                            .Replace(progressPaused, ordDel)
                            .Replace(inProgress, ordDel));

                        var responseBuilder = new DiscordInteractionResponseBuilder()
                            .AddEmbed(newEmbed);

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);

                        var originalChannelName = e.Channel.Name;

                        // Überprüfen, ob der Kanalname nicht leer ist
                        if (!string.IsNullOrEmpty(originalChannelName) && originalChannelName.Length > 1)
                        {
                            // Das erste Zeichen vom Kanalnamen entfernen
                            var newChannelName = string.Concat("✅", originalChannelName.AsSpan(1));

                            // Kanalname aktualisieren
                            await e.Channel.ModifyAsync(properties => properties.Name = newChannelName);
                        }
                        else
                        {
                            // Fallback, wenn der Kanalname leer oder zu kurz ist
                            var newChannelName = "✅";
                            await e.Channel.ModifyAsync(properties => properties.Name = newChannelName);
                        }
                    }
                    break;
                case "Button_InProgress":
                    if (originalEmbed != null)
                    {
                        var newEmbed = new DiscordEmbedBuilder(originalEmbed)
                            .WithDescription(originalEmbed.Description
                            .Replace(progressPaused, inProgress)
                            .Replace(delPending, inProgress));

                        // Original Buttons wieder hinzufügen
                        var orderDeliverBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_OrderDelivered", "✅ Complete Order");
                        var progressPausedBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_ProgressPaused", "⏸️ Pause Progress");

                        var responseBuilder = new DiscordInteractionResponseBuilder()
                            .AddEmbed(newEmbed)
                            .AddComponents(orderDeliverBtn, progressPausedBtn);

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);
                     
                        var originalChannelName = e.Channel.Name;

                        // Überprüfen, ob der Kanalname nicht leer ist
                        if (!string.IsNullOrEmpty(originalChannelName) && originalChannelName.Length > 1)
                        {
                            // Das erste Zeichen vom Kanalnamen entfernen
                            var newChannelName = string.Concat("🔧", originalChannelName.AsSpan(1));

                            // Kanalname aktualisieren
                            await e.Channel.ModifyAsync(properties => properties.Name = newChannelName);
                        }
                        else
                        {
                            // Fallback, wenn der Kanalname leer oder zu kurz ist
                            var newChannelName = "🔧";
                            await e.Channel.ModifyAsync(properties => properties.Name = newChannelName);
                        }
                    }
                    break;
                case "Button_ProgressPaused":
                    if (originalEmbed != null)
                    {
                        var newEmbed = new DiscordEmbedBuilder(originalEmbed)
                            .WithDescription(originalEmbed.Description
                            .Replace(inProgress, progressPaused));

                        // Original Buttons wieder hinzufügen
                        var orderDeliverBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_OrderDelivered", "✅ Complete Order");
                        var inProgressBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_InProgress", "⚙️ Resume Progress");

                        var responseBuilder = new DiscordInteractionResponseBuilder()
                            .AddEmbed(newEmbed)
                            .AddComponents(orderDeliverBtn, inProgressBtn);

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, responseBuilder);;

                        var originalChannelName = e.Channel.Name;

                        // Überprüfen, ob der Kanalname nicht leer ist
                        if (!string.IsNullOrEmpty(originalChannelName) && originalChannelName.Length > 1)
                        {
                            // Das erste Zeichen vom Kanalnamen entfernen
                            var newChannelName = "⏸️" + originalChannelName.Substring(1);

                            // Kanalname aktualisieren
                            await e.Channel.ModifyAsync(properties => properties.Name = newChannelName);
                        }
                        else
                        {
                            // Fallback, wenn der Kanalname leer oder zu kurz ist
                            var newChannelName = "⏸️";
                            await e.Channel.ModifyAsync(properties => properties.Name = newChannelName);
                        }
                    }
                    break;
                case "Button_OrderCancel":
                    if (originalEmbed != null)
                    {
                        var newEmbed = new DiscordEmbedBuilder(originalEmbed)
                            .WithDescription(originalEmbed.Description
                            .Replace(startProcess, ordCancel)
                            .Replace(delPending, ordCancel));

                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(newEmbed));

                        var newChannelName = "☑️" + e.Channel.Name;

                        await e.Channel.ModifyAsync(properties => properties.Name = newChannelName);
                    }
                    break;

                default:
                    Console.WriteLine(e.Message);
                    break;
            }
        }
    }
}
