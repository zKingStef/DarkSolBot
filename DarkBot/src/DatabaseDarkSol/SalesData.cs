using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkBot.src.DatabaseDarkSol
{
    public class SalesData
    {
        public int SALES_Id { get; set; }
        public int ART_Nr { get; set; }
        public int CUS_Id { get; set; }
        public double SALES_Price { get; set; }
        public double SALES_Profit { get; set; }
        public int PLAT_Id { get; set; }
        public int PAYMENT_Id { get; set; }
        public string SALES_Desc { get; set; }
        public DateTime SALES_GenDate { get; set; }
        public DateTime SALES_ModDate { get; set; }

    }
}
