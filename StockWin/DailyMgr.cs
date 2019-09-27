using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockWin
{
    class DailyMgr
    {
        public class DailyItem
        {
            public string ts_code; // 股票代码
            public DateTime trade_date;// 交易日期
            public double open; //	开盘价
            public double high; //	最高价
            public double low; //	最低价
            public double close; //	收盘价
            public double pre_close; //	昨收价
            public double change; //	涨跌额
            public double pct_change; //	涨跌幅
            public double vol; //	成交量 （手）
            public double amount; //	成交额 （千元）

            public DailyItem(string ts_code, string trade_date, string open, string high, string low, string close, string pre_close, string change, string pct_change, string vol, string amount)
            {
                this.ts_code = ts_code;
                this.trade_date = GVL.StrToDt(trade_date);
                this.open = Convert.ToDouble(open);
                this.high = Convert.ToDouble(high);
                this.low = Convert.ToDouble(low);
                this.close = Convert.ToDouble(close);
                this.pre_close = Convert.ToDouble(pre_close);
                this.change = Convert.ToDouble(change);
                this.pct_change = Convert.ToDouble(pct_change);
                this.vol = Convert.ToDouble(vol);
                this.amount = Convert.ToDouble(amount);
            }
        }

        List<DailyItem> m_DailyList = new List<DailyItem>();
        DataTable m_Table = null;

        private void Init(string ts_code, DateTime stime, DateTime etime)
        {
            m_DailyList.Clear();
            string st = stime.ToString("yyyyMMdd");
            string et = etime.ToString("yyyyMMdd");
            string cmd = @"SELECT * FROM daily where trade_date >= "+st+" and trade_date <= "+et+" and ts_code = '"+ts_code+"'";
            
            m_Table = SQLiteHelper.GetDataTable(cmd, new SQLiteParameter[0]);
            if (m_Table != null)
            {
                for (int i = 0; i < m_Table.Rows.Count; i++)
                {
                    m_DailyList.Add(new DailyItem(
                        m_Table.Rows[i][1].ToString(),
                        m_Table.Rows[i][2].ToString(),
                        m_Table.Rows[i][3].ToString(),
                        m_Table.Rows[i][4].ToString(),
                        m_Table.Rows[i][5].ToString(),
                        m_Table.Rows[i][6].ToString(),
                        m_Table.Rows[i][7].ToString(),
                        m_Table.Rows[i][8].ToString(),
                        m_Table.Rows[i][9].ToString(),
                        m_Table.Rows[i][10].ToString(),
                        m_Table.Rows[i][11].ToString()
                        ));
                }
            }
        }

        public List<DailyItem> GetDaily(string ts_code, DateTime stime,DateTime etime)
        {
            Init(ts_code, stime, etime);
            return m_DailyList;
        }

        public List<DailyItem> GetDaily(DateTime stime, DateTime etime)
        {
            List<DailyItem> m_ret = new List<DailyItem>();
            string st = stime.ToString("yyyyMMdd");
            string et = etime.ToString("yyyyMMdd");
            string cmd = @"SELECT * FROM daily where trade_date >= " + st + " and trade_date <= " + et;

            m_Table = SQLiteHelper.GetDataTable(cmd, new SQLiteParameter[0]);
            if (m_Table != null)
            {
                for (int i = 0; i < m_Table.Rows.Count; i++)
                {
                    m_ret.Add(new DailyItem(
                        m_Table.Rows[i][1].ToString(),
                        m_Table.Rows[i][2].ToString(),
                        m_Table.Rows[i][3].ToString(),
                        m_Table.Rows[i][4].ToString(),
                        m_Table.Rows[i][5].ToString(),
                        m_Table.Rows[i][6].ToString(),
                        m_Table.Rows[i][7].ToString(),
                        m_Table.Rows[i][8].ToString(),
                        m_Table.Rows[i][9].ToString(),
                        m_Table.Rows[i][10].ToString(),
                        m_Table.Rows[i][11].ToString()
                        ));
                }
            }
            return m_ret;
        }
    }
}
