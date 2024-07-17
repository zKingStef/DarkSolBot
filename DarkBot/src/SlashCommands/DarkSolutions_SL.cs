using DarkBot.src.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using DarkBot.src.Common;
using DSharpPlus.EventArgs;
using Npgsql.Replication.PgOutput.Messages;
using static System.Net.WebRequestMethods;
using DarkBot.src.CommandHandler;

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
                                        [Choice("RareCandy", 5)]
                                        [Choice("Custom", 6)]
                                        [Option("ArticleType", "Which Article is being purchased ?")] long ART_Type,
                                        [Option("Quantity", "Quantity of the Article")] string qty,
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
            string embedTitle = "Error.embedTitle";
            DiscordColor embedColor = DiscordColor.Black;

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
                    embedTitle = " Pokecoins";
                    embedColor = DiscordColor.Yellow;
                    break;
                case 1:
                    pictureURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQtPs4Cf0pQpN_EVeISKk4TaeCVoAvz68AvgQ&s";
                    embedTitle = " Million Stardust";
                    embedColor = DiscordColor.Magenta;
                    break;
                case 2:
                    pictureURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxiMVLiCD_zCwC007NHW9g4tUpScVMQwpdXA&s";
                    embedTitle = " Million Stardust + Shadow";
                    embedColor = DiscordColor.Magenta;
                    break;
                case 3:
                    pictureURL = "https://cdn-icons-png.flaticon.com/256/6712/6712589.png";
                    embedTitle = " Million XP";
                    embedColor = DiscordColor.Magenta;
                    break;
                case 4:
                    pictureURL = "https://gogames.news/wp-content/uploads/2019/12/tipps-fuer-den-lapras-raid-tag-guide-1.png";
                    embedTitle = " Raids";
                    embedColor = DiscordColor.Green;
                    break;
                case 5:
                    pictureURL = "https://static.wikia.nocookie.net/pokemongo/images/1/12/Rare_Candies.png/revision/latest?cb=20230208171511";
                    embedTitle = " RareCandies";
                    embedColor = DiscordColor.Red;
                    break;

                default:
                    pictureURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT3lNWPVHvbBJiFpnJ_Yo3kspQKdvh24grZYA&s";
                    break;
            }

            var startProcessBtn   = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_StartProcess", "🚩 Start Process");
            var orderCancelBtn  = new DiscordButtonComponent(ButtonStyle.Danger, "Button_OrderCancel",   "❌ Cancel Order");
            var accDetailsBtn   = new DiscordButtonComponent(ButtonStyle.Primary, "Button_AccDetails",    "🛃 Account Details");
            var databaseDoneBtn = new DiscordButtonComponent(ButtonStyle.Danger, "Button_DatabaseDone",  "🗂️ Database done");

            var orderEmbed = new DiscordEmbedBuilder() 
                .WithColor(embedColor)
                .WithTitle(qty + embedTitle)
                .WithThumbnail(pictureURL)
                .WithDescription($"🙎🏻‍♂️ Customer:  **{CUS_Name}**\n🛒 Platform:  **{platformName}**\n" +
                                 $"💰 Article Price:  **{SALES_Price}€**\n\n" +
                                  "🚦 Order Status: **:no_entry: Process not started**");
            
            var orderMessage = new DiscordMessageBuilder()
                    .AddEmbed(orderEmbed)
                    .AddComponents(startProcessBtn, orderCancelBtn)
                .AddComponents(accDetailsBtn, databaseDoneBtn);

            if (ctx.Interaction.Guild.GetChannel(1263000023822762035) is not DiscordChannel category || category.Type != ChannelType.Category)
            {
                await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent("Error occured while creating a new Order: No Order category found!").AsEphemeral(true));
                return;
            }

            var overwrites = new List<DiscordOverwriteBuilder>
            {
                new DiscordOverwriteBuilder(ctx.Interaction.Guild.EveryoneRole).Deny(Permissions.AccessChannels),
                new DiscordOverwriteBuilder(ctx.Interaction.Guild.GetRole(1210230414011011124)).Allow(Permissions.AccessChannels), // Developer Role
            };

            DiscordChannel orderChannel = await ctx.Interaction.Guild.CreateTextChannelAsync($"{CUS_Name} {platformName}", category, overwrites: overwrites, position: 0);

            await orderChannel.SendMessageAsync(orderMessage);
        }
    }
}
