using DarkBot.src.DatabaseDarkSol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkBot.src.SlashCommands
{
    public class DarkSolutions_SL
    {
        [SlashCommand("new-order", "Creates a new Order")]
        public static async Task CreateNewOrder(InteractionContext ctx)
        {
            var embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("**New Order created**")
                    .WithColor(DiscordColor.CornflowerBlue)
                    .WithDescription("change the order description")
                    .WithImageUrl("https://cdn.discordapp.com/attachments/1113081965525094452/1156932587458138112/Comp_1_1.gif?ex=6674cb2f&is=667379af&hm=999a49db3990141596640974b0dced388966863f7e2bd30dc7ef69d62466e14a&");

            var orderDeliverBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "orderDeliverBtn",  "✅ Order delivered");
            var orderPendingBtn = new DiscordButtonComponent(ButtonStyle.Secondary, "orderPendingBtn",  "🕖 Delivery pending");
            var orderCancelBtn  = new DiscordButtonComponent(ButtonStyle.Secondary, "orderCancelBtn",   "❌ Order canceled");
            var accDetailsBtn   = new DiscordButtonComponent(ButtonStyle.Primary,   "AccDetailsBtn",    "🛃 Account Details");
            var orderDetailsBtn = new DiscordButtonComponent(ButtonStyle.Primary,   "OrderDetailsBtn",  "🛄 Order Details");

            var orderEmbed = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Cyan)
                    .WithTitle($"__{ticketTitle}__")
                    .WithThumbnail(guild.IconUrl)
                    .WithDescription(ticketDesc))
                    .AddComponents(orderDeliverBtn, orderPendingBtn, orderCancelBtn, accDetailsBtn, orderDetailsBtn);

            await ctx.Channel.SendMessageAsync(orderEmbed);
        }
    }
}
