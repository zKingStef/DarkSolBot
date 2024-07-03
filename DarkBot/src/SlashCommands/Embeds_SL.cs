using DarkBot.src.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DarkBot.src.CommandHandler.PokeDiary;

namespace DarkBot.src.SlashCommands
{
    public class Embeds_SL
    {
        [SlashCommand("embeds", "Send an embed message by the bot")]
        public static async Task SendEmbed(InteractionContext ctx,
                                [Choice("pokecoins", 0)]
                                [Choice("xp-service", 1)]
                                [Option("form", "Choose a embed")] long choice)
        {
            // Pre Execution Checks
            await CmdShortener.CheckIfUserHasCeoRole(ctx);

            if (choice == 0)
            {
                var filePath = "path/to/your/image.png"; // Replace with the path to your image file

                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await command.RespondWithFileAsync(stream, Path.GetFileName(filePath), "Here is your image");
                }


                var embedTicketButtons = new DiscordEmbedBuilder()
                    .WithTitle("")
                    .WithColor(DiscordColor.Yellow)
                    .WithDescription("**Current Prices :**\r\n:Pokecoin1: 2500 - €12\r\n:Pokecoin1: 5200 - €23\r\n:Pokecoin1: 14.500 - €54\r\n:Pokecoin1: 19700 - €69\r\n:Pokecoin1: 29000 - €94\r\n:Pokecoin1: 43500 - €130\r\n:Pokecoin1: 58000 - €165")
                    .WithThumbnail(ctx.User.AvatarUrl) // get the user darksolutions user
                    .WithImageUrl("https://i.ebayimg.com/images/g/TncAAOSwz7FfP~5R/s-l400.jpg");

                var createTicketBtn = new DiscordButtonComponent(ButtonStyle.Primary, "Button_TicketPokecoin", "📩 Create Ticket");

                var messageBuilder = new DiscordMessageBuilder()
                    .WithEmbed(embedTicketButtons)
                    .AddComponents(pokemonGoBtn);

                await ctx.Channel.SendMessageAsync(messageBuilder);
            }
            else if (choice == 1)
            {
                ;
            }
            else if (choice == 2)
            {
                ;
            }
            else if (choice == 3)
            {
                ;
            }
        }
    }
}
