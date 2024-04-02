using DarkBot.src.CommandHandler;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkBot.src.SlashCommands
{
    [SlashCommandGroup("pokediary", "Slash Commands for the Pokemon Go Diary.")]
    public class PokeDiary_SL : PokeDiary
    {
        [SlashCommand("addstats", "Add daily statistics")]
        public async Task AddStats(InteractionContext ctx,
                                  [Option("distance", "Distance walked in kilometers")] double distance,
                                  [Option("pokemon", "Number of Pokémon caught")] long pokemon,
                                  [Option("pokestops", "Number of PokéStops visited")] long pokestops,
                                  [Option("total_xp", "Total XP gained")] long totalXP,
                                  [Option("stardust", "Amount of Stardust collected")] long stardust,
                                  [Option("weekly_kilometers", "Kilometers walked in the week")] long weeklyKilometers,
                                  [Option("image", "Image attachment")] DiscordAttachment image = null)
        {
            // Check if daily Stats already exist
            int hwCheckIfEntryExist;

            // Erstellen eines neuen DailyStatsEntry-Objekts
            var entry = new DailyStatsEntry
            {
                Date = DateTime.Now.Date, 
                Distance = distance,
                Pokemon = pokemon,
                Pokestops = pokestops,
                TotalXP = totalXP,
                Stardust = stardust,
                WeeklyKilometers = weeklyKilometers
            };

            // Speichern des Bilds, falls vorhanden
            if (image != null)
            {
                // Hier fügst du den Code ein, um das Bild zu speichern und die URL zu erhalten
                string imageUrl = await SaveImage(image);
                entry.ImageUrl = imageUrl;
            }

            // Laden der vorhandenen Statistiken aus der JSON-Datei
            var entries = LoadDailyStats();

            // Hinzufügen des neuen Eintrags zur Liste
            entries.Add(entry);

            // Speichern der aktualisierten Statistiken in der JSON-Datei
            SaveDailyStats(entries);

            await ctx.CreateResponseAsync($":white_check_mark: Daily statistics for {DateTime.Now.ToShortDateString()} added successfully. ");
        }


        [SlashCommand("showstats", "Get statistics for a specific date")]
        public async Task GetStats(InteractionContext ctx,
                                  [Option("date", "The date to get statistics for (format: yyyy-MM-dd)")] string dateString)
        {
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                // Lade die täglichen Statistiken aus der JSON-Datei
                List<DailyStatsEntry> entries = LoadDailyStats();

                // Suche nach den Statistiken für das angegebene Datum
                DailyStatsEntry stats = entries.FirstOrDefault(e => e.Date.Date == date.Date);

                if (stats != null)
                {
                    // Wenn Statistiken für das angegebene Datum gefunden wurden, zeige sie an
                    var embed = new DiscordEmbedBuilder
                    {
                        Title = $"Statistics for {date.ToShortDateString()}",
                        Color = DiscordColor.Green
                    };

                    // Hier kannst du den Embed nach deinen Wünschen formatieren und die Statistiken hinzufügen
                    embed.AddField("Distance walked", $"{stats.Distance} km", true);
                    embed.AddField("Pokémon caught", stats.Pokemon.ToString(), true);
                    embed.AddField("PokéStops visited", stats.Pokestops.ToString(), true);
                    embed.AddField("Total XP gained", stats.TotalXP.ToString(), true);
                    embed.AddField("Stardust collected", stats.Stardust.ToString(), true);
                    embed.AddField("Weekly kilometers", $"{stats.WeeklyKilometers} km", true);

                    if (!string.IsNullOrEmpty(stats.ImageUrl))
                    {
                        embed.WithImageUrl(stats.ImageUrl);
                    }

                    await ctx.CreateResponseAsync(embed: embed);
                }
                else
                {
                    // Wenn keine Statistiken für das angegebene Datum gefunden wurden, gib eine entsprechende Nachricht aus
                    await ctx.CreateResponseAsync($"No statistics found for {date.ToShortDateString()}.");
                }
            }
            else
            {
                // Wenn das angegebene Datum im falschen Format ist, gib eine Fehlermeldung aus
                await ctx.CreateResponseAsync("Invalid date format. Please use the format yyyy-MM-dd.");
            }
        }

        [SlashCommand("showallstats", "Show statistics for all days")]
        public async Task ShowAllStats(InteractionContext ctx)
        {
            // Lade alle täglichen Statistiken aus der JSON-Datei
            List<DailyStatsEntry> entries = LoadDailyStats();

            if (entries.Count == 0)
            {
                await ctx.CreateResponseAsync("No statistics available.");
                return;
            }

            int currentPageIndex = 0;
            int totalPages = (int)Math.Ceiling((double)entries.Count / 5); // Anzahl der Seiten, wobei 5 Einträge pro Seite angezeigt werden

            // Erstelle die Embed-Nachricht für die erste Seite
            var embed = BuildStatsPageEmbed(entries, currentPageIndex);
            var message = await ctx.CreateResponseAsync(embed: embed);

            // Füge die Buttons für die Navigation hinzu
            await message.CreateComponentsAsync(ButtonPaginator.BuildNavigationButtons(currentPageIndex, totalPages));

            // Warte auf Interaktionen mit den Buttons
            ComponentInteractionCreateEventArgs interaction;

            do
            {
                interaction = await ctx.WaitForComponentAsync();

                if (interaction != null)
                {
                    // Aktualisiere die Seite entsprechend der Button-Interaktion
                    if (ButtonPaginator.TryNavigate(interaction, ref currentPageIndex, totalPages))
                    {
                        // Erstelle und sende die neue Embed-Nachricht für die aktualisierte Seite
                        embed = BuildStatsPageEmbed(entries, currentPageIndex);
                        await interaction.UpdateMessageAsync(embed: embed);
                    }
                }
            } while (interaction != null);
        }

        private DiscordEmbedBuilder BuildStatsPageEmbed(List<DailyStatsEntry> entries, int pageIndex)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "Daily Statistics",
                Color = DiscordColor.Green
            };

            int startIndex = pageIndex * 5;
            int endIndex = Math.Min(startIndex + 5, entries.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                var entry = entries[i];
                embed.AddField($"{entry.Date.ToShortDateString()}",
                    $"Distance: {entry.Distance} km\n" +
                    $"Pokémon: {entry.Pokemon}\n" +
                    $"Pokestops: {entry.Pokestops}\n" +
                    $"Total XP: {entry.TotalXP}\n" +
                    $"Stardust: {entry.Stardust}\n" +
                    $"Weekly kilometers: {entry.WeeklyKilometers} km");
            }

            return embed;
        }





    }
}
