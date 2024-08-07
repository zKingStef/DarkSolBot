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
        [SlashCommand("autorole", "Summon The Autorole System")]
        [RequireRoles(RoleCheckMode.Any, "🧰 CEO")]
        public static async Task AutomatedRoleSystem(InteractionContext ctx)
        {
            if (!CmdShortener.CheckPermissions(ctx, Permissions.ManageEvents))
            {
                await CmdShortener.SendAsEphemeral(ctx, "You don't have the necessary permissions to execute this command");
                return;
            }

            var options = new List<DiscordSelectComponentOption>()
                {
                    new (
                        "DarkSolutions",
                        "dd_RoleDarkServices",
                        "Access to all DarkSolutions Channels",
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
        }
    }
}
