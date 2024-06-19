using System;
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
        private static DiscordMessage ticketMessage;

        private static Dictionary<ulong, ulong> ticketChannelMap = new Dictionary<ulong, ulong>();

        public string username { get; set; }
        public string issue { get; set; }
        public ulong ticketId { get; set; }
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
            var claimButton = new DiscordButtonComponent(ButtonStyle.Primary, "claimTicketButton", "☑️ Claim");
            DiscordChannel ticketChannel = e.Interaction.Channel;

            var overwrites = new List<DiscordOverwriteBuilder>
            {
                new DiscordOverwriteBuilder(guild.EveryoneRole).Deny(Permissions.AccessChannels),
                new DiscordOverwriteBuilder(guild.GetRole(1209284430229803008)).Allow(Permissions.AccessChannels), // Techniker Rolle
            };

            switch (e.Interaction.Data.CustomId)
            {
                case "modalPokemonGoForm":
                    ticketDesc = $"**Your Order:** {e.Values["orderTextBox"]}\n\n" +
                                 $"**Login Method:** {e.Values["loginTextBox"]}\n\n" +
                                 $"**Payment Method:** {e.Values["paymethodTextBox"]}\n\n" +
                                 "Thank you for submitting your Order.";
                    ticketTitle = "DarkSolutions - Pokemon Go ";

                    roleId = 978346565225816152;

                    overwrites =
                    [
                        new DiscordOverwriteBuilder(guild.EveryoneRole).Deny(Permissions.AccessChannels),
                        new DiscordOverwriteBuilder(user).Allow(Permissions.AccessChannels).Deny(Permissions.None),
                        new DiscordOverwriteBuilder(guild.GetRole(roleId)).Allow(Permissions.AccessChannels), // CEO Role
                    ];
                    break;
                case "modalTechnicForm":
                    ticketDesc = $"**Problem:** {e.Values["issueTextBox"]}\n\n" +
                                 "Danke für deine Anfrage. Wir werden uns sobald wie möglich bei dir melden!";
                    ticketTitle = "Technische Hilfe";

                    roleId = 978346565225816152;

                    overwrites =
                    [
                        new DiscordOverwriteBuilder(guild.EveryoneRole).Deny(Permissions.AccessChannels),
                        new DiscordOverwriteBuilder(user).Allow(Permissions.AccessChannels).Deny(Permissions.None),
                        new DiscordOverwriteBuilder(guild.GetRole(roleId)).Allow(Permissions.AccessChannels), // CEO Role
                    ];
                    break;
            }

            ticketChannel = await guild.CreateTextChannelAsync($"{e.Interaction.User.Username}-Ticket", category, overwrites: overwrites, position: 0);

            //var random = new Random();
            //
            //ulong minValue = 1000000000000000000;
            //ulong maxValue = 9999999999999999999;
            //
            //ulong randomNumber = (ulong)random.Next((int)(minValue >> 32), int.MaxValue) << 32 | (ulong) random.Next(); 
            //ulong result = randomNumber % (maxValue - minValue + 1) + minValue;

            //var supportTicket = new Ticket_Handler()
            //{
            //    username = e.Interaction.User.Username,
            //    issue = e.Values.Values.First(),
            //    ticketId = 1 //result
            //};

            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Ticket erstellt: {ticketChannel.Mention}").AsEphemeral(true));

            var ticketEmbed = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Cyan)
                    .WithTitle($"__{ticketTitle}__")
                    .WithThumbnail(guild.IconUrl)
                    .WithDescription(ticketDesc))
                    .AddComponents(closeButton, closeReasonButton, claimButton);

            // Mention the User in the Chat and then remove the Message
            var mentionMessage = await ticketChannel.SendMessageAsync(user.Mention + $"<@&{roleId}>");
            await ticketChannel.DeleteMessageAsync(mentionMessage);

            await ticketChannel.SendMessageAsync(ticketEmbed);
        }

        public static async Task RemoveClaimButtonAsync(ComponentInteractionCreateEventArgs e)
        {
            // Überprüfen, ob der Button claimTicketButton angeklickt wurde
            if (e.Interaction.Data.CustomId == "claimTicketButton")
            {
                // Überprüfen, ob die ticketMessage vorhanden ist und einen claimButton enthält
                if (ticketMessage != null && ticketMessage.Components.Any(c => c.CustomId == "claimTicketButton"))
                {
                    // Entfernen des claimButton aus der Nachricht
                    var components = ticketMessage.Components.ToList();
                    var claimButtonIndex = components.FindIndex(c => c.CustomId == "claimTicketButton");
                    components.RemoveAt(claimButtonIndex);

                    // Bearbeiten der Nachricht, um den entfernten Button anzuwenden
                    await ticketMessage.ModifyAsync(message =>
                    {
                        message.ClearComponents();
                        foreach (var component in components)
                        {
                            message.AddComponents(component);
                        }
                    });
                }
            }
        }



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

        public static async Task CheckIfUserHasTicketPermissions(InteractionContext ctx)
        {
            if (!(CmdShortener.CheckRole(ctx, 978346565225816151) // Manager Role
             || !CmdShortener.CheckRole(ctx, 978346565225816152) // CEO Role
             || !CmdShortener.CheckRole(ctx, 1216171388830744686) // DarkBot Role
             || !CmdShortener.CheckRole(ctx, 1239551770238255147))) // Spezial Rolle 
            {
                await CmdShortener.SendNotification(ctx, "Error", "You are not allowed to use Ticket Commands!", DiscordColor.Red, 0);
                return;
            }
        }

        public static bool CheckIfUserHasTicketPermissions(ComponentInteractionCreateEventArgs ctx)
        {
            if (!(CmdShortener.CheckRole(ctx, 978346565225816151) // Manager Role
             || !CmdShortener.CheckRole(ctx, 978346565225816152) // CEO Role
             || !CmdShortener.CheckRole(ctx, 1216171388830744686) // DarkBot Role
             || !CmdShortener.CheckRole(ctx, 1239551770238255147))) // Spezial Rolle 
            {
                CmdShortener.SendAsEphemeral(ctx, "You are not allowed to use Ticket Commands!");
                return false;
            }
            return true;

        }

        public static bool CheckIfUserHasTicketPermissions(ModalSubmitEventArgs ctx)
        {
            if (!(CmdShortener.CheckRole(ctx, 978346565225816151) // Manager Role
             || !CmdShortener.CheckRole(ctx, 978346565225816152) // CEO Role
             || !CmdShortener.CheckRole(ctx, 1216171388830744686) // DarkBot Role
             || !CmdShortener.CheckRole(ctx, 1239551770238255147))) // Spezial Rolle 
            {
                CmdShortener.SendAsEphemeral(ctx, "You are not allowed to use Ticket Commands!");
                return false;
            }
            return true;
        }
    }
}
