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
                                        [Choice("Stardust+Shadow", 2)]
                                        [Choice("XP", 3)]
                                        [Choice("Raids", 4)]
                                        [Choice("Custom", 5)]
                                        [Option("ArticleType", "Which Article is being purchased ?")] long ART_Type,
                                        [Option("Article", "Further Description of the Article")] string Article,
                                        [Option("Price", "Price of the Article")] string SALES_Price,
                                        [Choice("Ebay", 0)]
                                        [Choice("Discord", 1)]
                                        [Option("Platform", "Selling platform")] long Platform,
                                        [Option("Customer", "Name of the Customer")] string CUS_Name)
        {
            // Pre Execution Checks
            await CmdShortener.CheckIfUserHasCeoRole(ctx);

            string pictureURL = "Error.pictureURL";
            string platformName = "Error.platformName";

            switch (Platform)
            {
                case 0:
                    platformName = "Ebay";
                    break;
                case 1:
                    platformName = "Discord";
                    break;

            }    

            switch (ART_Type)
            {  
                case 0:
                    pictureURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTZdSP0XQrBiuPJTPLN-DYFRbuWkKUFajY7cw&s";
                    break;
                case 1:
                    pictureURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQtPs4Cf0pQpN_EVeISKk4TaeCVoAvz68AvgQ&s";
                    break;
                case 2:
                    pictureURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxiMVLiCD_zCwC007NHW9g4tUpScVMQwpdXA&s";
                    break;
                case 3:
                    pictureURL = "https://cdn-icons-png.flaticon.com/256/6712/6712589.png";
                    break;
                case 4:
                    pictureURL = "https://gogames.news/wp-content/uploads/2019/12/tipps-fuer-den-lapras-raid-tag-guide-1.png";
                    break;

                default:
                    pictureURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT3lNWPVHvbBJiFpnJ_Yo3kspQKdvh24grZYA&s";
                    break;
            }

            var startProcessBtn   = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_StartProcess", "🚩 Start Process");
            var orderCancelBtn  = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_OrderCancel",   "❌ Cancel Order");
            var accDetailsBtn   = new DiscordButtonComponent(ButtonStyle.Primary, "Button_AccDetails",    "🛃 Account Details");
            var orderDetailsBtn = new DiscordButtonComponent(ButtonStyle.Primary, "Button_OrderDetails",  "🛄 Order Details");

            var orderEmbed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Cyan)
                .WithTitle("Order: " + Article)
                .WithThumbnail(pictureURL)
                .WithDescription($"🙎🏻‍♂️ Customer:  **{CUS_Name}**\n🛒 Platform:  **{platformName}**\n" +
                                 $"💰 Article Price:  **{SALES_Price}€**\n\n" +
                                  "🚦 Order Status: **:no_entry: Process not started**");

            var responseBuilder = new DiscordInteractionResponseBuilder()
                .AddEmbed(orderEmbed)
                .AddComponents(startProcessBtn, orderCancelBtn)
                .AddComponents(accDetailsBtn, orderDetailsBtn);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, responseBuilder);
        }
    }
}
