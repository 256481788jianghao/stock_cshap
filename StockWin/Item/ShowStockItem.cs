using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockWin
{
    class ShowStockItem
    {
        public string ts_code { get; set; }
        public string name { get; set; }

        public ShowStockItem(string ts_code, string name)
        {
            this.ts_code = ts_code;
            this.name = name;
        }
    }
}
