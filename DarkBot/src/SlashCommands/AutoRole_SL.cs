﻿using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkBot.src.Common;
using DSharpPlus.CommandsNext.Attributes;

namespace DarkBot.src.SlashCommands
{
    public class AutoRole_SL : ApplicationCommandModule
    {
        [SlashCommand("autorole", "Summon Autorole System")]
        [RequireRoles(RoleCheckMode.Any, "🧰 CEO")]
        public static async Task AutomatedRoleSystem(InteractionContext ctx)
        {
            if (!CmdShortener.CheckPermissions(ctx, Permissions.Administrator))
            {
                await CmdShortener.SendNotification(ctx, "Keinen Zugriff", "Du hast nicht die nötigen Rechte, um diesen Befehl auszuführen.", DiscordColor.Red, 0);
                return;
            }

            var options = new List<DiscordSelectComponentOption>()
                {
                    new (
                        "Pokemon Go",
                        "dd_RolePokemonGo",
                        "Access to all Pokemon Go related Channels!",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":PokemonGo:"))),

                    new (
                        "Gamer",
                        "dd_RoleGamer",
                        "Access to all Gaming Channels",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":video_game:"))),

                    new (
                        "DarkServices",
                        "dd_RoleDarkServices",
                        "Access to all DarkServices Channels",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":DarkServices:"))),
                   };

            var autoRoleDropdown = new DiscordSelectComponent("autoRoleDropdown", "Choose a Role", options, false, 0, 1);

            var embedAutoRoleDropdown = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.SpringGreen)
                .WithTitle("Grant yourself a Role")
                .WithDescription("The choosen Role will be added/removed if you select it from the Menu!")
                )
                .AddComponents(autoRoleDropdown);

            await ctx.Channel.SendMessageAsync(embedAutoRoleDropdown);

            await CmdShortener.SendAsEphemeral(ctx, "Auto-Rollensystem geladen...");
        }
    }
}
