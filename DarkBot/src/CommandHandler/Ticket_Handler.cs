﻿using System;
using System.Collections.Generic;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DarkBot.src.Common;
using DSharpPlus.SlashCommands;
using System.ComponentModel.Design;
using System.Text;

namespace DarkBot.src.CommandHandler
{
    public class Ticket_Handler
    {
        private static Dictionary<ulong, ulong> ticketChannelMap = new Dictionary<ulong, ulong>();

        public static async Task HandleGeneralTickets(ModalSubmitEventArgs e)
        {
            DiscordMember? user = e.Interaction.User as DiscordMember;
            DiscordGuild guild = e.Interaction.Guild;

            if (guild.GetChannel(1207086767623381092) is not DiscordChannel category || category.Type != ChannelType.Category)
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent("Error occured while creating a Ticket: No ticket category found!").AsEphemeral(true));
                return;
            }

            string ticketDesc = "Error! Please report to Developer";
            string ticketTitle = "**Error!**";
            ulong roleId = 999999999999;
            var closeButton = new DiscordButtonComponent(ButtonStyle.Secondary, "closeTicketButton", "🔒 Close");
            var closeReasonButton = new DiscordButtonComponent(ButtonStyle.Secondary, "closeReasonTicketButton", "🔒 Close Reason");
            // var claimButton = new DiscordButtonComponent(ButtonStyle.Primary, "claimTicketButton", "☑️ Claim");
            DiscordChannel ticketChannel = e.Interaction.Channel;

            var overwrites = new List<DiscordOverwriteBuilder>
            {
                new DiscordOverwriteBuilder(guild.EveryoneRole).Deny(Permissions.AccessChannels),
                new DiscordOverwriteBuilder(user).Allow(Permissions.AccessChannels).Deny(Permissions.None),
                new DiscordOverwriteBuilder(guild.GetRole(Roles.ceo)).Allow(Permissions.AccessChannels), // CEO Role
            };

            switch (e.Interaction.Data.CustomId)
            {
                case "modalPokemonGoForm":
                    ticketDesc = $"**Your Order:** {e.Values["orderTextBox"]}\n\n" +
                                 $"**Login Method:** {e.Values["loginTextBox"]}\n\n" +
                                 $"**Payment Method:** {e.Values["paymethodTextBox"]}\n\n" +
                                 "Thank you for submitting your Order.";
                    ticketTitle = "DarkSolutions - Pokemon Go ";
                    break;

                case "modalPokecoin":
                    ticketDesc = $"**Pokecoin Amount:** {e.Values["orderTextBox"]}\n\n" +
                                 $"**Payment Method:** {e.Values["paymethodTextBox"]}\n\n" +
                                 $"**Login Method:** {e.Values["loginTextBox"]}\n\n" +
                                 "Thank you for submitting your Order.";
                    ticketTitle = "DarkSolutions - Pokecoins ";
                    break;
                case "modalXP":
                    ticketDesc = $"**XP Amount:** {e.Values["orderTextBox"]}\n\n" +
                                 $"**Payment Method:** {e.Values["paymethodTextBox"]}\n\n" +
                                 $"**Login Method:** {e.Values["loginTextBox"]}\n\n" +
                                 "Thank you for submitting your Order.";
                    ticketTitle = "DarkSolutions - XP Service ";
                    break;
                case "modalRaids":
                    ticketDesc = $"**Raid Amount:** {e.Values["orderTextBox"]}\n\n" +
                                 $"**Raid Pokemon:** {e.Values["raidpokeTextBox"]}\n\n" +
                                 $"**Payment Method:** {e.Values["paymethodTextBox"]}\n\n" +
                                 $"**Login Method:** {e.Values["loginTextBox"]}\n\n" +
                                 "Thank you for submitting your Order.";
                    ticketTitle = "DarkSolutions - Raid Service ";
                    break;
                case "modalShundo":
                    ticketDesc = $"**Shundo Amount:** {e.Values["orderTextBox"]}\n\n" +
                                 $"**Payment Method:** {e.Values["paymethodTextBox"]}\n\n" +
                                 $"**Login Method:** {e.Values["loginTextBox"]}\n\n" +
                                 "Thank you for submitting your Order.";
                    ticketTitle = "DarkSolutions - Shundo Service ";
                    break;
                case "modalComday":
                    ticketDesc = $"**Additional Shundos ?:** {e.Values["orderTextBox"]}\n\n" +
                                 $"**Payment Method:** {e.Values["paymethodTextBox"]}\n\n" +
                                 $"**Login Method:** {e.Values["loginTextBox"]}\n\n" +
                                 "Thank you for submitting your Order.";
                    ticketTitle = "DarkSolutions - Community Day Service ";
                    break;
                case "modal100IV":
                    ticketDesc = $"**Pokemon:** {e.Values["orderTextBox"]}\n\n" +
                                 $"**Payment Method:** {e.Values["paymethodTextBox"]}\n\n" +
                                 $"**Login Method:** {e.Values["loginTextBox"]}\n\n" +
                                 "Thank you for submitting your Order.";
                    ticketTitle = "DarkSolutions - 100IV Pokemon Service ";
                    break;
                case "modalRaidpass":
                    ticketDesc = $"**Raidpass Amount:** {e.Values["orderTextBox"]}\n\n" +
                                 $"**Payment Method:** {e.Values["paymethodTextBox"]}\n\n" +
                                 "Thank you for submitting your Order.";
                    ticketTitle = "DarkSolutions - Raidpass Service ";
                    break;
                case "modalStardust":
                    ticketDesc = $"**Stardust Amount:** {e.Values["orderTextBox"]}\n\n" +
                                 $"**Payment Method:** {e.Values["paymethodTextBox"]}\n\n" +
                                 $"**Login Method:** {e.Values["loginTextBox"]}\n\n" +
                                 "Thank you for submitting your Order.";
                    ticketTitle = "DarkSolutions - Stardust Service ";
                    break;
            }

            ticketChannel = await guild.CreateTextChannelAsync($"{e.Interaction.User.Username}-Ticket", category, overwrites: overwrites, position: 0);

            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Ticket created: {ticketChannel.Mention}").AsEphemeral(true));

            var ticketEmbed = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Cyan)
                    .WithTitle($"__{ticketTitle}__")
                    .WithThumbnail(guild.IconUrl)
                    .WithDescription(ticketDesc))
                    .AddComponents(closeButton, closeReasonButton/*, claimButton*/);

            // Mention the User in the Chat and then remove the Message
            var mentionMessage = await ticketChannel.SendMessageAsync(user.Mention + $"<@&{roleId}>");
            await ticketChannel.DeleteMessageAsync(mentionMessage);

            await ticketChannel.SendMessageAsync(ticketEmbed);
        }
        
        //
        public static async Task CloseTicket(ComponentInteractionCreateEventArgs e)
        {
            if (!CheckIfUserHasTicketPermissions(e))
                return;

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "🔒 Ticket closed!",
                Description = $"Ticket closed by {e.User.Mention}!\n" +
                              $"Channel will be deleted in <t:{DateTimeOffset.UtcNow.AddSeconds(60).ToUnixTimeSeconds()}:R>.",
                Timestamp = DateTime.UtcNow
            };

            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                                                     new DiscordInteractionResponseBuilder().AddEmbed(embedMessage));


            var messages = await e.Channel.GetMessagesAsync(999);

            var content = new StringBuilder();
            content.AppendLine($"Ticket LOG: {e.Channel.Name}:");
            foreach (var message in messages)
            {
                content.AppendLine($"{message.Author.Username} ({message.Author.Id}) - {message.Content}");
            }

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content.ToString())))
            {
                var msg = await new DiscordMessageBuilder()
                    .AddFile($"{e.Interaction.Channel.Name}.txt", memoryStream)
                    .SendAsync(e.Guild.GetChannel(978669571483500574));
            }

            await Task.Delay(TimeSpan.FromSeconds(58));

            var ticketChannelId = e.Channel.Id;
            var guild = e.Guild;
            var ticketChannel = guild.GetChannel(ticketChannelId);
            await ticketChannel.DeleteAsync("Ticket closed");

            if (ticketChannelMap.TryGetValue(ticketChannelId, out var voiceChannelId))
            {
                var voiceChannel = guild.GetChannel(voiceChannelId);
                await voiceChannel.DeleteAsync("Ticket closed");

                // Remove the entry from the dictionary
                ticketChannelMap.Remove(ticketChannelId);
            }

            //await e.Channel.DeleteAsync("Ticket geschlossen");
        }

        //
        public static async Task CloseTicket(ModalSubmitEventArgs e)
        {

            if (!Ticket_Handler.CheckIfUserHasTicketPermissions(e))
                return;

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = "🔒 Ticket closed!",
                Description = $"Ticket closed by {e.Interaction.User.Mention} with reason **{e.Values.Values.First()}**.\n" +
                              $"Channel will be deleted in <t:{DateTimeOffset.UtcNow.AddSeconds(60).ToUnixTimeSeconds()}:R>.",
                Timestamp = DateTime.UtcNow
            };

            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                                                     new DiscordInteractionResponseBuilder().AddEmbed(embedMessage));


            var messages = await e.Interaction.Channel.GetMessagesAsync(999);

            var content = new StringBuilder();
            content.AppendLine($"Ticket closed by {e.Interaction.User.Mention} with the reason {e.Values.Values.First()}\n" +
                               $"Transcript of {e.Interaction.Channel.Name}:");
            foreach (var message in messages)
            {
                content.AppendLine($"{message.Author.Username} ({message.Author.Id}) - {message.Content}");
            }

            await Task.Delay(TimeSpan.FromSeconds(60));

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content.ToString())))
            {
                var msg = await new DiscordMessageBuilder()
                    .AddFile($"{e.Interaction.Channel.Name}.txt", memoryStream)
                    .SendAsync(e.Interaction.Guild.GetChannel(1209297588915015730));
            }

            await e.Interaction.Channel.DeleteAsync(e.Values.Values.First());
        }

        //
        public static bool CheckIfUserHasTicketPermissions(InteractionContext ctx)
        {
            if (!(CmdShortener.CheckRole(ctx, 978346565225816151) // Manager Role
             || !CmdShortener.CheckRole(ctx, 978346565225816152) // CEO Role
             || !CmdShortener.CheckRole(ctx, 1216171388830744686))) // DarkBot Role
            {
                CmdShortener.SendAsEphemeral(ctx, "You don't have the necessary permissions to execute this command");
                return false;
            }
            return true;
        }

        //
        public static bool CheckIfUserHasTicketPermissions(ComponentInteractionCreateEventArgs ctx)
        {
            if (!(CmdShortener.CheckRole(ctx, 978346565225816151) // Manager Role
             || !CmdShortener.CheckRole(ctx, 978346565225816152) // CEO Role
             || !CmdShortener.CheckRole(ctx, 1216171388830744686))) // DarkBot Role

            {
                CmdShortener.SendAsEphemeral(ctx, "You don't have the necessary permissions to execute this command");
                return false;
            }
            return true;

        }

        //
        public static bool CheckIfUserHasTicketPermissions(ModalSubmitEventArgs ctx)
        {
            if (!(CmdShortener.CheckRole(ctx, 978346565225816151) // Manager Role
             || !CmdShortener.CheckRole(ctx, 978346565225816152) // CEO Role
             || !CmdShortener.CheckRole(ctx, 1216171388830744686))) // DarkBot Role
            {
                CmdShortener.SendAsEphemeral(ctx, "You don't have the necessary permissions to execute this command");
                return false;
            }
            return true;
        }

        
    }
}
