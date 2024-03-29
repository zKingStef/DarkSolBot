﻿using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using DarkBot.src.Common;

namespace DarkBot.src.SlashCommands
{
    public class Misc_SL : ApplicationCommandModule
    {

        [SlashCommand("avatar", "Zeigt die Avatar-URL eines Users an")]
        public static async Task Avatar(InteractionContext ctx,
                                [Option("user", "Der User, dessen Avatar angezeigt werden soll")] DiscordUser? user = null)
        {
            var targetUser = user ?? ctx.User;

            var avatarUrl = targetUser.AvatarUrl;

            var embed = new DiscordEmbedBuilder
            {
                Title = $"{targetUser.Username}'s Avatar",
                ImageUrl = avatarUrl,
                Color = DiscordColor.HotPink,
                Description = ctx.User.AvatarUrl,
            };

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed.Build()));
        }

        [SlashCommand("server", "Zeigt Informationen zum Server an")]
        public async Task ServerEmbed(InteractionContext ctx)
        {
            string serverDescription = $"**Servername:** {ctx.Guild.Name}\n" +
                                        $"**Server ID:** {ctx.Guild.Id}\n" +
                                        $"**Erstellt am:** {ctx.Guild.CreationTimestamp:dd/M/yyyy}\n" +
                                        $"**Owner:** {ctx.Guild.Owner.Mention}\n\n" +
                                        $"**Users:** {ctx.Guild.MemberCount}\n" +
                                        $"**Channels:** {ctx.Guild.Channels.Count}\n" +
                                        $"**Rollen:** {ctx.Guild.Roles.Count}\n" +
                                        $"**Emojis:** {ctx.Guild.Emojis.Count}";

            var serverInformation = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Gold,
                Title = "Server Informationen",
                Description = serverDescription
            };

            var response = new DiscordInteractionResponseBuilder().AddEmbed(serverInformation.WithImageUrl(ctx.Guild.IconUrl));
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
        }
    }
}