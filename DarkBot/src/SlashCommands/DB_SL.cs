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

        /*
        [SlashCommand("AddSalesEntry", "Performs an insert into the Sales Table")]
        public static async Task AddSalesEntry(InteractionContext ctx,
                                              [Option("ART_Nr", "")]        long ART_Nr,
                                              [Option("CUS_Id", "")]        long CUS_Id,
                                              [Option("SALES_Price", "")]   double SALES_Price,
                                              [Option("SALES_Profit", "")]  double SALES_Profit,
                                              [Option("PLAT_Id", "")]       long PLAT_Id,
                                              [Option("PAYMENT_Id", "")]    long PAYMENT_Id,
                                              [Option("SALES_Desc", "")]    string SALES_Desc)
        {
            SalesData sales = new SalesData();
            sales.ART_Nr = (int)ART_Nr;
            sales.CUS_Id = (int)CUS_Id;
            sales.SALES_Price = SALES_Price;
            sales.SALES_Profit = SALES_Profit;
            sales.PLAT_Id = (int)PLAT_Id;
            sales.PAYMENT_Id = (int)PAYMENT_Id;
            sales.SALES_Desc = SALES_Desc;

            var dbCommands = new DB_Commands();
            var SALES_Id = await dbCommands.NewSalesEntry(sales);
            await ctx.CreateResponseAsync($"New Entry in Sales Table successfully added! ID: {sales.SALES_Id}");
        }*/
    }
}
