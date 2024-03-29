using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkBot.src.SlashCommands
{
    public class DarkServices_SL : ApplicationCommandModule
    {
        [SlashCommand("currencytable", "Show Currency")]
        [RequireRoles(RoleCheckMode.Any, "🧰 CEO")]
        public static async Task CurrencyTable(InteractionContext ctx)
        {
            if (!ctx.Member.Roles.Any(r => r.Name == "🧰 CEO"))
            {
                await ctx.CreateResponseAsync("You need a higher role to execute this command.");
                return;
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = "Pricelist Table",
                Color = DiscordColor.Green
            };

            var sb = new StringBuilder();

            sb.AppendLine("```md");
            sb.AppendLine("Service        |   Price   |   DarkPrice     |    Profit");
            sb.AppendLine("--------------------------------------------------------");

            sb.AppendLine("```");

            embed.Description = sb.ToString();

            await ctx.CreateResponseAsync(embed: embed);
        }
    }
}
