using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkBot.src.Common;

namespace DarkBot.src.CommandHandler
{
	public class Moderation_Handler
	{
        // Methode für Ban-Aktionen
        public static async Task ExecuteBanAction(InteractionContext ctx, DiscordMember user, int deleteDays, string reason)
        {
            if (!CmdShortener.CheckPermissions(ctx, Permissions.BanMembers))
            {
                await CmdShortener.SendNotification(ctx, "Keinen Zugriff", "Du hast nicht die nötigen Rechte, um diesen Benutzer zu bannen.", DiscordColor.Red, 0);
                return;
            }

            try
            {
                var member = await ctx.Guild.GetMemberAsync(user.Id);

                await CmdShortener.SendDirectMessage(ctx, user, "Du wurdest gebannt!", $"{reason}", DiscordColor.Red);

                await ctx.Guild.BanMemberAsync(member, deleteDays, reason + $" - **MOD:** {ctx.User.Mention} - **TIME:** {ctx.User.CreationTimestamp}");

                // BotChannel
                await CmdShortener.SendNotification(ctx, $"{member.DisplayName} wurde vom Server gebannt",
                                            $"**User:** {member.Mention}\n" +
                                            $"**Verantwortlicher Moderator:** {ctx.User.Mention}",
                                            DiscordColor.IndianRed,
                                            0);

                // LogChannel
                await CmdShortener.SendNotification(ctx, $"User durch SlashCommand gebannt!",
                                                $"**Discord Name:** ```{member.Username}``` - {member.Mention}\n" +
                                                $"**Verantwortlicher Moderator:** {ctx.User.Mention}",
                                                DiscordColor.Grayple,
                                                1143518462111658034);
            }
            catch (Exception e)
            {
                await CmdShortener.HandleException(ctx, e);
            }
        }

        public static async Task ExecuteBanAction(InteractionContext ctx, string userId, int deleteDays, string reason)
        {
            if (!CmdShortener.CheckPermissions(ctx, Permissions.BanMembers))
            {
                await CmdShortener.SendNotification(ctx, "Keinen Zugriff", "Du hast nicht die nötigen Rechte, um diesen Benutzer zu bannen.", DiscordColor.Red, 0);
                return;
            }

            try
            {
                if (ulong.TryParse(userId, out ulong userBanId))
                {
                    var member = await ctx.Guild.GetMemberAsync(userBanId);

                    await ctx.Guild.BanMemberAsync(userBanId, deleteDays, reason + $"MOD: {ctx.User.Mention} TIME: {ctx.User.CreationTimestamp}");

                    // BotChannel
                    await CmdShortener.SendNotification(ctx, $"User wurde vom Server gebannt",
                                                $"User: **{member.Mention}**",
                                                DiscordColor.IndianRed,
                                                0);

                    // LogChannel
                    await CmdShortener.SendNotification(ctx, $"User durch SlashCommand gebannt!",
                                                $"**User:** {member.Mention} - {member.Username}\n" +
                                                $"**Verantwortlicher Moderator:** {ctx.User.Mention}",
                                                DiscordColor.Grayple,
                                                1143518462111658034);
                }
            }
            catch (Exception e)
            {
                await CmdShortener.HandleException(ctx, e);
            }
        }

        // Methode für Unban-Aktionen
        public static async Task ExecuteUnbanAction(InteractionContext ctx, string userId, string reason)
        {
            if (!ctx.Member.Permissions.HasPermission(Permissions.BanMembers))
            {
                await CmdShortener.SendNotification(ctx, "Keinen Zugriff", "Du hast nicht die nötigen Rechte, um diesen Benutzer zu entbannen.", DiscordColor.Red, 0);
                return;
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                await ShowBanList(ctx);
            }
            else
            {
                // Benutzer-ID wurde angegeben, versuche zu entbannen
                if (ulong.TryParse(userId, out ulong userBanId))
                {
                    var bans = await ctx.Guild.GetBansAsync();
                    var ban = bans.FirstOrDefault(b => b.User.Id == userBanId);
                    if (ban == null)
                    {
                        await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("Benutzer nicht in der Banliste gefunden."));
                        return;
                    }

                    await ctx.Guild.UnbanMemberAsync(userBanId, reason);
                    await CmdShortener.SendNotification(ctx, $"{ban.User.Username} wurde vom Server entbannt",
                                                $"User: **{ban.User.Mention}**\n" +
                                                $"Verantwortlicher Moderator: {ctx.User.Mention}",
                                                DiscordColor.SpringGreen,
                                                0);

                    // LogChannel
                    await CmdShortener.SendNotification(ctx, $"User durch SlashCommand entbannt!",
                                                $"Discord Name: **{ban.User.Username} - {ban.User.Mention}**\n" +
                                                $"Verantwortlicher Moderator: {ctx.User.Mention}",
                                                DiscordColor.Grayple,
                                                1143518613777678336);
                }
                else
                {
                    await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("Ungültige User-ID."));
                }
            }
        }

        // Methode für Ban-Liste
        public static async Task ShowBanList(InteractionContext ctx)
        {
            if (!CmdShortener.CheckPermissions(ctx, Permissions.BanMembers))
            {
                await CmdShortener.SendNotification(ctx, "Keinen Zugriff", "Du hast nicht die nötigen Rechte, um die Ban-Liste aufzurufen.", DiscordColor.Red, 0);
                return;
            }

            try
            {
                var bans = await ctx.Guild.GetBansAsync();
                if (bans.Count == 0)
                {
                    await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent("Es gibt aktuell keine gebannten Benutzer."));
                    return;
                }

                var banListStringBuilder = new StringBuilder("**Gebannte Benutzer:**\n");
                foreach (var ban in bans)
                {
                    banListStringBuilder.AppendLine($"{ban.User.Username}#{ban.User.Discriminator} - **ID:** {ban.User.Id} - **Grund:** {ban.Reason}");
                }

                // Nachrichtenaufteilung bei Bedarf
                if (banListStringBuilder.Length >= 2000)
                {
                    // Implementiere eine Aufteilungslogik oder sende die Liste in Teilen
                }
                else
                {
                    await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent(banListStringBuilder.ToString()));
                }
            }
            catch (Exception e)
            {
                await CmdShortener.HandleException(ctx, e);
            }
        }

        // Methode für Timeout
        public static async Task ExecuteTimeoutAction(InteractionContext ctx, DiscordMember user, long durationInMinutes, string reason)
        {
            // Berechtigungsprüfung
            if (!CmdShortener.CheckPermissions(ctx, Permissions.ModerateMembers))
            {
                await CmdShortener.SendNotification(ctx, "Keinen Zugriff", "Du hast nicht die nötigen Rechte, um diesen Befehl auszuführen.", DiscordColor.Red, 0);
                return;
            }

            try
            {
                var member = await ctx.Guild.GetMemberAsync(user.Id);
                var timeoutUntil = DateTime.UtcNow.AddMinutes(durationInMinutes);

                await CmdShortener.SendDirectMessage(ctx, user, "Du wurdest stumm geschalten!", $"{reason}", DiscordColor.DarkRed);

                await member.TimeoutAsync(timeoutUntil, reason);

                await CmdShortener.SendNotification(ctx, $"{member.DisplayName} hat einen Timeout erhalten",
                                            $"User: **{member.Mention}**\n" +
                                            $"Verantwortlicher Moderator: {ctx.User.Mention}",
                                            DiscordColor.IndianRed,
                                            0);

                await CmdShortener.SendNotification(ctx, $"{member.DisplayName} hat einen Timeout erhalten, der in {durationInMinutes} Minuten abläuft.",
                                            $"Discord Name: **{member.Mention}**\n" +
                                            $"Discord ID: {member.Id}\n\n" +
                                            $"Grund: **{reason}**\n" +
                                            $"Verantwortlicher Moderator: {ctx.User.Mention}",
                                            DiscordColor.IndianRed,
                                            1143512137914921101);
            }
            catch (NotFoundException)
            {
                await CmdShortener.SendNotification(ctx, "Nicht gefunden", "Der Benutzer wurde auf dem Server nicht gefunden.", DiscordColor.Red, 0);
            }
            catch (Exception e)
            {
                await CmdShortener.HandleException(ctx, e);
            }
        }

        public static async Task ExecuteRemoveTimeoutAction(InteractionContext ctx, DiscordMember user, string reason)
        {

            // Berechtigungsprüfung
            if (!CmdShortener.CheckPermissions(ctx, Permissions.ModerateMembers))
            {
                await CmdShortener.SendNotification(ctx, "Keinen Zugriff", "Du hast nicht die nötigen Rechte, um diesen Befehl auszuführen.", DiscordColor.Red, 0);
                return;
            }

            try
            {
                var member = await ctx.Guild.GetMemberAsync(user.Id);

                await CmdShortener.SendDirectMessage(ctx, user, "Deine Timeout wurde aufgehoben!", $"{reason}", DiscordColor.SpringGreen);

                // Setze das Timeout-Enddatum auf null, um den Timeout aufzuheben
                await member.TimeoutAsync(null, reason);
                await CmdShortener.SendNotification(ctx, $"Der Timeout für {member.DisplayName} wurde aufgehoben.",
                                            $"User: **{member.Mention}**\n" +
                                            $"Verantwortlicher Moderator: {ctx.User.Mention}",
                                            DiscordColor.SpringGreen,
                                            0);

                await CmdShortener.SendNotification(ctx, $"Der Timeout für {member.DisplayName} wurde aufgehoben.",
                                            $"Discord Name: **{member.Mention}**\n" +
                                            $"Discord ID: {member.Id}\n\n" +
                                            $"Grund: **{reason}**\n" +
                                            $"Verantwortlicher Moderator: {ctx.User.Mention}",
                                            DiscordColor.SpringGreen,
                                            1143512137914921101);
            }
            catch (NotFoundException)
            {
                await CmdShortener.SendNotification(ctx, "Nicht gefunden", "Der Benutzer wurde auf dem Server nicht gefunden.", DiscordColor.Red, 0);
            }
            catch (Exception e)
            {
                await CmdShortener.HandleException(ctx, e);
            }
        }
    }
}
