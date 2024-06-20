using DarkBot.src.DatabaseDarkSol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
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
using DSharpPlus.EventArgs;
using Npgsql.Replication.PgOutput.Messages;
using static System.Net.WebRequestMethods;

namespace DarkBot.src.SlashCommands
{
    public class DarkSolutions_SL : ApplicationCommandModule
    {
        [SlashCommand("new-order", "Creates a new Order")]
        public async Task CreateNewOrder(InteractionContext ctx,
                                [Choice("Pokecoins", 0)]
                                [Choice("Stardust", 1)]
                                [Choice("XP", 2)]
                                [Choice("Raids", 3)]
                                [Option("ArticleType", "Which Article is being purchased ?")] long ART_Type)
        {
            // Pre Execution Checks
            //await CmdShortener.CheckIfUserHasCeoRole(ctx);

            string pictureURL = "ERROR";

            switch (ART_Type)
            {  
                case 0:
                    pictureURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTZdSP0XQrBiuPJTPLN-DYFRbuWkKUFajY7cw&s";
                    break;
                case 1:
                    pictureURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQtPs4Cf0pQpN_EVeISKk4TaeCVoAvz68AvgQ&s";
                    break;
                case 2:
                    pictureURL = "https://cdn-icons-png.flaticon.com/256/6712/6712589.png";
                    break;
                case 3:
                    pictureURL = "https://gogames.news/wp-content/uploads/2019/12/tipps-fuer-den-lapras-raid-tag-guide-1.png";
                    break;
            }

            DiscordGuild guild = ctx.Interaction.Guild;

            var orderDeliverBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "orderDeliverBtn", "✅ Order delivered");
            var orderPendingBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "orderPendingBtn", "🕖 Delivery pending");
            var orderCancelBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "orderCancelBtn", "❌ Order canceled");
            var accDetailsBtn = new DiscordButtonComponent(ButtonStyle.Primary, "AccDetailsBtn", "🛃 Account Details");
            var orderDetailsBtn = new DiscordButtonComponent(ButtonStyle.Primary, "OrderDetailsBtn", "🛄 Order Details");

            var orderEmbed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Cyan)
                .WithTitle("**New Order created**")
                .WithThumbnail(pictureURL)
                .WithDescription("Order Status: 🕖 Delivery pending");

            var responseBuilder = new DiscordInteractionResponseBuilder()
                .AddEmbed(orderEmbed)
                .AddComponents(orderDeliverBtn, orderPendingBtn, orderCancelBtn)
                .AddComponents(accDetailsBtn, orderDetailsBtn);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, responseBuilder);
        }
    }
}
