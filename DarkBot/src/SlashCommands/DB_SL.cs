using DarkBot.src.Common;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkBot.src.CommandHandler;
using DarkBot.src.DatabaseDarkSol;

namespace DarkBot.src.SlashCommands
{
    [SlashCommandGroup("db", "Database Commands")]
    public class DB_SL : ApplicationCommandModule
    {
        [SlashCommand("GetTopSalesId", "Performs a select * on the Sales Table")]
        public static async Task GetTopSalesId(InteractionContext ctx)
        {
            var dbCommands = new DB_Commands();
            var SALES_Id = await dbCommands.GetTopSalesId();
            await ctx.CreateResponseAsync($"The highest SALES_Id is: {SALES_Id}");
        }
    }
}
