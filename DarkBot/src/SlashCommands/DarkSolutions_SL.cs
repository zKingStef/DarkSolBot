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
                                        [Choice("Raidpasses", 6)]
                                        [Choice("Custom", 7)]
                                        [Option("ArticleType", "Which Article is being purchased ?")] long ART_Type,
                                        [Option("Quantity", "Quantity of the Article")] string qty,
                                        [Option("Price", "Price of the Article")] string SALES_Price,
                                        [Choice("Ebay", 0)]
                                        [Choice("Discord", 1)]
                                        [Choice("Whatsapp", 2)]
                                        [Option("Platform", "Selling platform")] long Platform,
                                        [Option("Customer", "Name of the Customer")] string CUS_Name)
        {
            if (!CmdShortener.CheckPermissions(ctx, Permissions.ManageEvents))
            {
                await CmdShortener.SendAsEphemeral(ctx, "You don't have the necessary permissions to execute this command");
                return;
            }

            // Define a dictionary for mapping the article type
            var articleTypeMapping = new Dictionary<long, string>
            {
                { 0, "Pokecoins" },
                { 1, "Stardust" },
                { 2, "Stardust+Shadow" },
                { 3, "XP" },
                { 4, "Raids" },
                { 5, "RareCandy" },
                { 6, "Raidpasses" },
                { 7, "Custom" }
            };

            // Define a dictionary for mapping the platform
            var platformMapping = new Dictionary<long, string>
            {
                { 0, "Ebay" },
                { 1, "Discord" },
                { 2, "Whatsapp" }
            };

            // Get the string values based on the provided choices
            var articleTypeString = articleTypeMapping.TryGetValue(ART_Type, out var articleType) ? articleType : "Unknown";
            var platformString = platformMapping.TryGetValue(Platform, out var platform) ? platform : "Unknown";

            string pictureURL = "Error.pictureURL";
            string embedTitle = "Error.embedTitle";
            DiscordColor embedColor = DiscordColor.Black;

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
                    embedColor = DiscordColor.Purple;
                    break;
                case 2:
                    pictureURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxiMVLiCD_zCwC007NHW9g4tUpScVMQwpdXA&s";
                    embedTitle = " Million Stardust + Shadow";
                    embedColor = DiscordColor.Magenta;
                    break;
                case 3:
                    pictureURL = "https://cdn-icons-png.flaticon.com/256/6712/6712589.png";
                    embedTitle = " Million XP";
                    embedColor = DiscordColor.White;
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
                case 6:
                    pictureURL = "https://static.wikia.nocookie.net/pokemongo/images/1/12/Rare_Candies.png/revision/latest?cb=20230208171511";
                    embedTitle = " Raid Passes";
                    embedColor = DiscordColor.SapGreen;
                    break;
                case 7:
                    pictureURL = "https://assets.bigcartel.com/product_images/356421655/Custom+Order.png?auto=format&fit=max&h=1200&w=1200";
                    embedTitle = "";
                    embedColor = DiscordColor.HotPink;
                    break;

                default:
                    pictureURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT3lNWPVHvbBJiFpnJ_Yo3kspQKdvh24grZYA&s";
                    break;
            }

            var startProcessBtn   = new DiscordButtonComponent(ButtonStyle.Secondary, "Button_StartProcess", "🚩 Start Process");
            var orderCancelBtn  = new DiscordButtonComponent(ButtonStyle.Danger, "Button_OrderCancel",   "❌ Cancel Order");

            var orderEmbed = new DiscordEmbedBuilder() 
                .WithColor(embedColor)
                .WithTitle(qty + embedTitle)
                .WithThumbnail(pictureURL)
                .WithDescription($"🙎🏻‍♂️ Customer:  **{CUS_Name}**\n🛒 Platform:  **{platformString}**\n" +
                                 $"💰 Article Price:  **{SALES_Price}€**\n\n" +
                                  "🚦 Order Status: **:no_entry: Process not started**");
            
            var orderMessage = new DiscordMessageBuilder()
                    .AddEmbed(orderEmbed)
                    .AddComponents(startProcessBtn, orderCancelBtn);

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

            DiscordChannel orderChannel = await ctx.Interaction.Guild.CreateTextChannelAsync($"{CUS_Name} {articleTypeString}", category, overwrites: overwrites, position: 0);

            await orderChannel.SendMessageAsync(orderMessage);
            await DarkSolutions_Handler.SendPhoneDropdown(ctx, orderChannel);
            await CmdShortener.SendAsEphemeral(ctx, "New Order has been created!  " + orderChannel.Mention);
        }
    }
}
