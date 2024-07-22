using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DarkBot.src.CommandHandler
{
    public class PokeDiary : ApplicationCommandModule
    {
        public class DailyStatsEntry
        {
            public DateTime Date { get; set; }
            public double Distance { get; set; }
            public long Pokemon { get; set; }
            public long Pokestops { get; set; }
            public long TotalXP { get; set; }
            public long Stardust { get; set; }
            public long WeeklyKilometers { get; set; }
            public string? ImageUrl { get; set; } // URL zum Bild
        }
    }
}
