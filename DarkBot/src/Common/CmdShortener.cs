﻿using DSharpPlus.Entities;
using DSharpPlus;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Exceptions;
using DSharpPlus.SlashCommands;
using DSharpPlus.CommandsNext;
using DarkBot.src.Common;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System.IO;
using System.Net.Http;

namespace DarkBot.src.Common
{
    public static class CmdShortener
    {
        // Methode zum Senden von Benachrichtigungen
        public static async Task SendNotification(InteractionContext ctx,
                                                  string title,
                                                  string description,
                                                  DiscordColor color,
                                                  ulong channelId)
        {
            var message = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = color,
                Timestamp = DateTime.UtcNow,
            };

            if (channelId == 0)
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(message));
            else if (channelId == 1)
                await ctx.Channel.SendMessageAsync(message);
            else
                await ctx.Guild.GetChannel(channelId).SendMessageAsync(message);
        }

        public static async Task SendAsEphemeral(InteractionContext ctx,
                                                  string text)
        {
            await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                     .WithContent((text)).AsEphemeral(true));
        }

        public static async Task SendLogMessage(DiscordClient client, ulong channelId, AuditLogActionType alaType, string title, string description, DiscordColor color)
        {
            var guild = await client.GetGuildAsync(978346565209042984);
            var auditLogs = await guild.GetAuditLogsAsync(1, null, alaType);
            var lastLog = auditLogs.FirstOrDefault();
            var responsible = lastLog?.UserResponsible;

            var channel = await client.GetChannelAsync(channelId);

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description + responsible.Mention,
                Color = color,
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    IconUrl = responsible?.AvatarUrl,
                    Name = responsible?.Username
                }
            };

            var embed = embedBuilder.Build();
            await channel.SendMessageAsync(embed: embed);
        }

        public static async Task SendLogMessage(DiscordClient client, ulong channelId, string title, string description, DiscordColor color)
        {
            var channel = await client.GetChannelAsync(channelId);

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = color,
            };

            var embed = embedBuilder.Build();
            await channel.SendMessageAsync(embed: embed);
        }


        public static async Task SendDirectMessage(InteractionContext ctx, DiscordMember user, string title, string description, DiscordColor color)
        {
            var message = new DiscordEmbedBuilder
            {
                Title = title,
                Description = "**Server:** " + ctx.Guild.Name +
                              "\n**Grund:** " + description +
                              "\n\n**Verantwortlicher Moderator:** " + ctx.Member.Mention,
                Color = color,
                Timestamp = DateTime.UtcNow
            };

            var channel = await user.CreateDmChannelAsync();
            await channel.SendMessageAsync(message);
        }

        // Methode zur Berechtigungsprüfung
        public static bool CheckPermissions(InteractionContext ctx, Permissions requiredPermissions)
        {
            return ctx.Member.Permissions.HasPermission(requiredPermissions);
        }

        // Methode zur Fehlerbehandlung
        public static async Task HandleException(InteractionContext ctx, Exception e)
        {
            string errorMessage = $"Ein Fehler ist aufgetreten: {e.Message}";
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(errorMessage));
        }
    }
}
