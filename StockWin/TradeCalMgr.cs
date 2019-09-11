using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockWin
{
    class TradeCalMgr
    {
        public class TradeCalItem
        {
            public string exchange; // 交易所 SSE上交所 SZSE深交所
            public DateTime cal_date; //    str 日历日期
            public bool is_open; // int 是否交易 0休市 1交易

            public TradeCalItem(string exchange, string cal_date, string is_open)
            {
                this.exchange = exchange;
                this.cal_date = DateTime.ParseExact(cal_date, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture); ;
                this.is_open = is_open == "1";
            }
        }

        List<TradeCalItem> m_TradeCalList = new List<TradeCalItem>();
        DataTable m_Table = null;

        public void Init()
        {
            m_Table = SQLiteHelper.GetDataTable(@"SELECT * FROM trade_cal", new SQLiteParameter[0]);
            if (m_Table != null)
            {
                for (int i = 0; i < m_Table.Rows.Count; i++)
                {
                    m_TradeCalList.Add(new TradeCalItem(
                        m_Table.Rows[i][1].ToString(),
                        m_Table.Rows[i][2].ToString(),
                        m_Table.Rows[i][3].ToString()
                        ));
                }
            }
        }

        public List<TradeCalItem> GetTradeCal()
        {
            return m_TradeCalList;
        }

        public List<TradeCalItem> GetTradeCalOpen()
        {
            return m_TradeCalList.FindAll(it => it.is_open);
        }
    }
}
