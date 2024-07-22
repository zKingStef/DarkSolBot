using DarkBot.src.CommandHandler;
using DarkBot.src.Common;
using DarkBot.src.Database;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DarkBot.src.SlashCommands
{
    [SlashCommandGroup("pokediary", "Slash Commands for the Pokemon Go Diary.")]
    public class PokeDiary_SL : PokeDiary
    {
        [SlashCommand("addstats", "Add daily statistics")]
        public static async Task AddStats(InteractionContext ctx,
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

            string query = "INSERT INTO bmocfdpnmiqmcbuykudg.POKEDIARY " +
                           "(ENTRY_DATE, POKEMON_CAUGHT, POKESTOPS_VISITED, " +
                           "DISTANCE_WALKED, TOTAL_XP, STARDUST, " +
                           "WEEKLY_DISTANCE, POKECOINS, RAIDPASSES, " +
                           "SHINYS, LEGENDARYS, HUNDOS, SHUNDOS)" +
                           $"VALUES('{date}', '{pokemon}', '{pokestops}', '{distance}', " +
                           $"'{totalXP}', '{stardust}', '{weeklyKilometers}', '{pokecoins}'," +
                           $" '{raidpasses}', '{shinys}', '{legendarys}', '{hundos}', '{shundos}');";

            await ctx.CreateResponseAsync($":white_check_mark: Daily statistics for {DateTime.Now.ToShortDateString()} saved!");

            var embed = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Rose)
                .WithTitle("Daily Statistics for " + DateTime.Now.ToShortDateString())
                .WithDescription(query));

            var logChannel = ctx.Guild.GetChannel(1264833483629662208);
            await logChannel.SendMessageAsync(embed);
        }
    }
}
