using DarkBot.src.Database;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkBot.src.DatabaseDarkSol
{
    public class DB_Commands
    {
        public string connectionString = "Host=bunjwqxejjdivx62llgi-postgresql.services.clever-cloud.com;" +
                                         "Username=ugaseupaiagerifnbppr;" +
                                         "Password=7BnA9hzCfoL46bt5W0n8Cf4jp80DKJ;" +
                                         "Database=bunjwqxejjdivx62llgi";


        // -- Example of Insert in Sales Table 
        //
        // INSERT INTO bmocfdpnmiqmcbuykudg.SALES
        // (SALES_Id, ART_Nr, CUS_Id, SALES_Price, SALES_Profit, PLAT_Id, PAYMENT_Id, SALES_Desc, SALES_GenDate, SALES_ModDate)
        // VALUES(208, 7, 29, 55, 5, 2, 1, 'TestArtikel - Discord', now(), now());
        public async Task<bool> NewSalesEntry(SalesData sales)
        {
            var SALES_Id = await GetTopSalesId();
            var SALES_GenDate = DateTime.Now;
            var SALES_ModDate = DateTime.Now;

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string query = "insert into bmocfdpnmiqmcbuykudg.SALES ("                       +
                                                                    "SALES_Id, "                    +
                                                                    "ART_Nr, "                      +
                                                                    "CUS_Id, "                      +
                                                                    "SALES_Price, "                 +
                                                                    "SALES_Profit, "                +
                                                                    "PLAT_Id, "                     +
                                                                    "PAYMENT_Id, "                  +
                                                                    "SALES_Desc, "                  +
                                                                    "SALES_GenDate, "               +
                                                                    "SALES_ModDate)"                +
                                                           $"values ("                              +
                                                                    $" '{SALES_Id + 1}',"           +
                                                                    $" '{sales.ART_Nr}',"           +
                                                                    $" '{sales.CUS_Id}',"           +
                                                                    $" '{sales.SALES_Price}',"      +
                                                                    $" '{sales.SALES_Profit}',"     +
                                                                    $" '{sales.PLAT_Id}',"          +
                                                                    $" '{sales.PAYMENT_Id}',"       +
                                                                    $" '{sales.SALES_Desc}',"       +
                                                                    $" '{sales.SALES_GenDate}',"    +
                                                                    $" '{sales.SALES_ModDate}')";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<long> GetTopSalesId()
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string query = "select SALES_Id " +
                                   "from SALES " +
                                   "order by SALES_Id desc " +
                                   "limit 1;";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        var SALES_Id = await cmd.ExecuteScalarAsync();
                        return Convert.ToInt64(SALES_Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }

        public async Task<long> GetSalesTable()
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string query = "select * from SALES;";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        var userCount = await cmd.ExecuteScalarAsync();
                        return Convert.ToInt64(userCount);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }
    }
}
