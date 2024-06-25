using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkBot.src.CommandHandler;

namespace DarkBot.src.SlashCommands
{
	public class Troll_SL : ApplicationCommandModule
	{
        [SlashCommand("pingspam", "Spam Ping any User")]
        public static async Task PingSpam(InteractionContext ctx,
                            [Option("User", "Target User", autocomplete: false)] DiscordUser user,
                            [Option("Amount", "Amount of the Pings", autocomplete: false)] double amtPing)
        {
            await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                     .WithContent(($"PingSpam started...")).AsEphemeral(true));

            if (amtPing > 50)
            {
                await ctx.Channel.SendMessageAsync("Bro chill, why would you do this");
                return;
            }

            for (int i = 0; i < amtPing; i++)
                await ctx.Channel.SendMessageAsync(user.Mention);
        }

        [SlashCommand("troll", "Sends a random troll message or image")]
        public async Task TrollCommand(InteractionContext ctx)
        {
            var random = new Random();
            int choice = random.Next(3); // Drei mögliche Trolling-Aktionen

            switch (choice)
            {
                case 0:
                    await Troll_Handler.SendFunnyMessage(ctx);
                    break;
                case 1:
                    await Troll_Handler.SendFunnyImage(ctx);
                    break;
                case 2:
                    await Troll_Handler.SendWeirdEmoji(ctx);
                    break;
            }
        }
    }
}
