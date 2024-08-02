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
                                        [Choice("Shundo", 7)]
                                        [Choice("Trade", 8)]
                                        [Choice("Custom", 9)]
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
                { 7, "Shundo" },
                { 8, "Trade" },
                { 9, "Custom" }
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
                    pictureURL = "https://pokemodgo.wordpress.com/wp-content/uploads/2018/06/stardust.png?w=256";
                    embedTitle = " Million Stardust";
                    embedColor = DiscordColor.Purple;
                    break;
                case 2:
                    pictureURL = "https://pokemodgo.wordpress.com/wp-content/uploads/2018/06/stardust.png?w=256";
                    embedTitle = " Million Stardust + Shadow";
                    embedColor = DiscordColor.Magenta;
                    break;
                case 3:
                    pictureURL = "https://cdn-icons-png.flaticon.com/256/6712/6712589.png";
                    embedTitle = " Million XP";
                    embedColor = DiscordColor.Cyan;
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
                    pictureURL = "https://img.ws.mms.shopee.com.my/efedacaa87eba6e775a3def2f9c06bb2";
                    embedTitle = " Shundo";
                    embedColor = DiscordColor.DarkRed;
                    break;
                case 8:
                    pictureURL = "https://static.wikia.nocookie.net/pokemongo/images/3/35/Icon_Friendship_Trade.png/revision/latest?cb=20190416114051";
                    embedTitle = "";
                    embedColor = DiscordColor.White;
                    break;
                case 9:
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

            DiscordChannel orderChannel = await ctx.Interaction.Guild.CreateTextChannelAsync($"🛑{CUS_Name} {articleTypeString}", category, overwrites: overwrites, position: 0);
            await CmdShortener.SendAsEphemeral(ctx, "New Order has been created!  " + orderChannel.Mention);

            await orderChannel.SendMessageAsync(orderMessage);
        }


        //
        [SlashCommand("new-sale", "Add Entry in Sales Table")]
        public static async Task NewSale(InteractionContext ctx,
                          [Option("date", "DateTime Format: (YYYY-MM-dd)")] string date,
                          [Option("distance", "Distance walked in kilometers")] double distance,
                          [Option("pokemon", "Number of Pokémon caught")] long pokemon,
                          [Option("pokestops", "Number of PokéStops visited")] long pokestops,
                          [Option("total_xp", "Total XP gained")] long totalXP,
                          [Option("stardust", "Amount of Stardust collected")] long stardust,
                          [Option("weekly_kilometers", "Kilometers walked in the week")] long weeklyKilometers,
                          [Option("pokecoins", "Amount of Pokecoins")] long pokecoins,
                          [Option("raidpasses", "Amount of Raidpasses")] long raidpasses,
                          [Option("shinys", "Shiny Pokemon")] long shinys,
                          [Option("legendarys", "Legendary Pokemon")] long legendarys,
                          [Option("hundos", "Hundo Pokemon")] long hundos,
                          [Option("shundos", "Shiny Hundo Pokemon")] long shundos)

        {
            CmdShortener.CheckIfUserHasCeoRole(ctx);

            string query = "INSERT INTO bmocfdpnmiqmcbuykudg.SALES " +
                           "(ENTRY_DATE, POKEMON_CAUGHT, POKESTOPS_VISITED, " +
                           "DISTANCE_WALKED, TOTAL_XP, STARDUST, " +
                           "WEEKLY_DISTANCE, POKECOINS, RAIDPASSES, " +
                           "SHINYS, LEGENDARYS, HUNDOS, SHUNDOS)" +
                           $"VALUES('{date}', '{pokemon}', '{pokestops}', '{distance}', " +
                           $"'{totalXP}', '{stardust}', '{weeklyKilometers}', '{pokecoins}'," +
                           $" '{raidpasses}', '{shinys}', '{legendarys}', '{hundos}', '{shundos}');";

            await ctx.CreateResponseAsync($":white_check_mark: New Sales Insert printed!");

            var embed = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Rose)
                .WithTitle("Sales Insert")
                .WithDescription(query));

            var logChannel = ctx.Guild.GetChannel(1226142440050659410);
            await logChannel.SendMessageAsync(embed);
        }

        //
        [SlashCommand("new-customer", "Add Entry in Customer Table")]
        public static async Task NewCustomer(InteractionContext ctx,
                          [Option("date", "DateTime Format: (YYYY-MM-dd)")] string date,
                          [Option("distance", "Distance walked in kilometers")] double distance,
                          [Option("pokemon", "Number of Pokémon caught")] long pokemon,
                          [Option("pokestops", "Number of PokéStops visited")] long pokestops,
                          [Option("total_xp", "Total XP gained")] long totalXP,
                          [Option("stardust", "Amount of Stardust collected")] long stardust,
                          [Option("weekly_kilometers", "Kilometers walked in the week")] long weeklyKilometers,
                          [Option("pokecoins", "Amount of Pokecoins")] long pokecoins,
                          [Option("raidpasses", "Amount of Raidpasses")] long raidpasses,
                          [Option("shinys", "Shiny Pokemon")] long shinys,
                          [Option("legendarys", "Legendary Pokemon")] long legendarys,
                          [Option("hundos", "Hundo Pokemon")] long hundos,
                          [Option("shundos", "Shiny Hundo Pokemon")] long shundos)

        {
            CmdShortener.CheckIfUserHasCeoRole(ctx);

            string query = "INSERT INTO bmocfdpnmiqmcbuykudg.CUSTOMER " +
                           "(ENTRY_DATE, POKEMON_CAUGHT, POKESTOPS_VISITED, " +
                           "DISTANCE_WALKED, TOTAL_XP, STARDUST, " +
                           "WEEKLY_DISTANCE, POKECOINS, RAIDPASSES, " +
                           "SHINYS, LEGENDARYS, HUNDOS, SHUNDOS)" +
                           $"VALUES('{date}', '{pokemon}', '{pokestops}', '{distance}', " +
                           $"'{totalXP}', '{stardust}', '{weeklyKilometers}', '{pokecoins}'," +
                           $" '{raidpasses}', '{shinys}', '{legendarys}', '{hundos}', '{shundos}');";

            await ctx.CreateResponseAsync($":white_check_mark: New Customer Insert printed!");

            var embed = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Rose)
                .WithTitle("Customer Insert")
                .WithDescription(query));

            var logChannel = ctx.Guild.GetChannel(1226142440050659410);
            await logChannel.SendMessageAsync(embed);
        }

        [SlashCommand("new-article", "Add Entry in Sales Table")]
        public static async Task NewArticle(InteractionContext ctx,
                          [Option("date", "DateTime Format: (YYYY-MM-dd)")] string date,
                          [Option("distance", "Distance walked in kilometers")] double distance,
                          [Option("pokemon", "Number of Pokémon caught")] long pokemon,
                          [Option("pokestops", "Number of PokéStops visited")] long pokestops,
                          [Option("total_xp", "Total XP gained")] long totalXP,
                          [Option("stardust", "Amount of Stardust collected")] long stardust,
                          [Option("weekly_kilometers", "Kilometers walked in the week")] long weeklyKilometers,
                          [Option("pokecoins", "Amount of Pokecoins")] long pokecoins,
                          [Option("raidpasses", "Amount of Raidpasses")] long raidpasses,
                          [Option("shinys", "Shiny Pokemon")] long shinys,
                          [Option("legendarys", "Legendary Pokemon")] long legendarys,
                          [Option("hundos", "Hundo Pokemon")] long hundos,
                          [Option("shundos", "Shiny Hundo Pokemon")] long shundos)

        {
            CmdShortener.CheckIfUserHasCeoRole(ctx);

            string query = "INSERT INTO bmocfdpnmiqmcbuykudg.ARTICLE " +
                           "(ENTRY_DATE, POKEMON_CAUGHT, POKESTOPS_VISITED, " +
                           "DISTANCE_WALKED, TOTAL_XP, STARDUST, " +
                           "WEEKLY_DISTANCE, POKECOINS, RAIDPASSES, " +
                           "SHINYS, LEGENDARYS, HUNDOS, SHUNDOS)" +
                           $"VALUES('{date}', '{pokemon}', '{pokestops}', '{distance}', " +
                           $"'{totalXP}', '{stardust}', '{weeklyKilometers}', '{pokecoins}'," +
                           $" '{raidpasses}', '{shinys}', '{legendarys}', '{hundos}', '{shundos}');";

            await ctx.CreateResponseAsync($":white_check_mark: New Article Insert printed!");

            var embed = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Rose)
                .WithTitle("Article Insert")
                .WithDescription(query));

            var logChannel = ctx.Guild.GetChannel(1226142440050659410);
            await logChannel.SendMessageAsync(embed);
        }
    }
}
