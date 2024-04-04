using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkBot.src.Common;

namespace DarkBot.src.SlashCommands
{
	public class Poll_SL : ApplicationCommandModule
	{
        [SlashCommand("poll", "Start a poll")]
        [RequireRoles(RoleCheckMode.Any, "🧰 CEO")]
        public  async Task Poll(InteractionContext ctx,
                        [Option("Title", "Poll Title")] string pollTitle,
                        [Option("Time", "Poll Time in Minutes")] long pollTime,
                        [Option("option1", "Option 1")] string option1,
                        [Option("option2", "Option 2")] string option2,
                        [Option("option3", "Option 3")] string option3)
        {
            var interactivity = ctx.Client.GetInteractivity();
            DateTimeOffset endTime = DateTimeOffset.UtcNow.AddMinutes(pollTime);

            await CmdShortener.CheckIfUserHasCeoRole(ctx);

            var options = new List<DiscordSelectComponentOption>()
                {
                    new (
                        option1,
                        "dd_PollOpt1",
                        $"Vote for {option1}",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":one:"))),

                    new (
                        option2,
                        "dd_PollOpt2",
                        $"Vote for {option2}",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":two:"))),
                   
                    new (
                        option3,
                        "dd_PollOpt3",
                        $"Vote for {option3}",
                        emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":three:")))
                };

            var autoRoleDropdown = new DiscordSelectComponent("PollDropdown", "Choose one", options, false, 0, 1);

            var embedAutoRoleDropdown = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Azure)
                .WithTitle(pollTitle)
                .WithDescription("Open the Menu to submit your Vote. You can only vote 1 time\n" +
                                $"End of Voting <t:{endTime.ToUnixTimeSeconds()}:R>")
                )
                .AddComponents(autoRoleDropdown);

            await ctx.Channel.SendMessageAsync(embedAutoRoleDropdown);

            await CmdShortener.SendAsEphemeral(ctx, "New Poll started...");

            // Dictionary zum Speichern der Anzahl der Stimmen für jede Option
            var voteCounts = new Dictionary<string, int>
            {
                { "Option 1", 0 },
                { "Option 2", 0 },
                { "Option 3", 0 }
            };

            // Warte bis zum Ende der Abstimmungszeit
            await Task.Delay(TimeSpan.FromMinutes(pollTime));

            int totalVotes = voteCounts.Values.Sum();

            var pollResultEmbed = new DiscordEmbedBuilder
            {
                Title = "Results of the Poll",
                Description = $"```{pollTitle}```\n" +
                              $":tada: **{option2} won**\n\n" +
                              $"Total Votes: **{totalVotes}**\n\n" +
                              $"- {voteCounts["Option 1"]} - **{option1}**\n" +
                              $"- {voteCounts["Option 2"]} - **{option2}**\n" +
                              $"- {voteCounts["Option 3"]} - **{option3}**\n",
                Color = DiscordColor.Green
            }.Build();

            // Ergebnis der Abstimmung senden
            await ctx.Channel.SendMessageAsync(embed: pollResultEmbed);
        }
    }
}
