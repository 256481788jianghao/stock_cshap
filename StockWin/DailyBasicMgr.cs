using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockWin
{
    class DailyBasicMgr
    {
        public class DailyBasicItem
        {
            public string ts_code; //	TS股票代码
            public string trade_date; //	交易日期
            public double close; //	当日收盘价
            public double turnover_rate; //	换手率
            public double turnover_rate_f; //	换手率（自由流通股）
            public double volume_ratio; //	量比
            public double pe; //	市盈率（总市值/净利润）
            public double pe_ttm; //	市盈率（TTM）
            public double pb; //	市净率（总市值/净资产）
            public double ps; //	市销率
            public double ps_ttm; //	市销率（TTM）
            public double total_share; //	总股本 （万）
            public double float_share; //	流通股本 （万）
            public double free_share; //	自由流通股本 （万）
            public double total_mv; //	总市值 （万元）
            public double circ_mv; //	流通市值（万元）

            public DailyBasicItem(string ts_code, string trade_date, string close, string turnover_rate, string turnover_rate_f, string volume_ratio, string pe, string pe_ttm, string pb, string ps, string ps_ttm, string total_share, string float_share, string free_share, string total_mv, string circ_mv)
            {
                this.ts_code = ts_code;
                this.trade_date = trade_date;
                this.close = Convert.ToDouble(close);
                this.turnover_rate = Convert.ToDouble(turnover_rate);
                this.turnover_rate_f = Convert.ToDouble(turnover_rate_f);
                this.volume_ratio = String.IsNullOrEmpty(volume_ratio) ? -1 : Convert.ToDouble(volume_ratio);
                this.pe = String.IsNullOrEmpty(pe) ? -1 : Convert.ToDouble(pe);
                this.pe_ttm = String.IsNullOrEmpty(pe_ttm) ? -1 : Convert.ToDouble(pe_ttm);
                this.pb = String.IsNullOrEmpty(pb) ? -1 : Convert.ToDouble(pb);
                this.ps = String.IsNullOrEmpty(ps) ? -1 : Convert.ToDouble(ps);
                this.ps_ttm = String.IsNullOrEmpty(ps_ttm) ? -1 : Convert.ToDouble(ps_ttm);
                this.total_share = Convert.ToDouble(total_share);
                this.float_share = Convert.ToDouble(float_share);
                this.free_share = Convert.ToDouble(free_share);
                this.total_mv = Convert.ToDouble(total_mv);
                this.circ_mv = Convert.ToDouble(circ_mv);
            }
        }

        List<DailyBasicItem> m_DailyBasicList = new List<DailyBasicItem>();
        DataTable m_Table = null;

        private void Init(DateTime stime, DateTime etime)
        {
            m_DailyBasicList.Clear();
            string st = stime.ToString("yyyyMMdd");
            string et = etime.ToString("yyyyMMdd");
            string cmd = @"SELECT * FROM daily_basic where trade_date >= " + st + " and trade_date <= " + et;

            m_Table = SQLiteHelper.GetDataTable(cmd, new SQLiteParameter[0]);
            if (m_Table != null)
            {
                for (int i = 0; i < m_Table.Rows.Count; i++)
                {
                    m_DailyBasicList.Add(new DailyBasicItem(
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
                        m_Table.Rows[i][11].ToString(),
                        m_Table.Rows[i][12].ToString(),
                        m_Table.Rows[i][13].ToString(),
                        m_Table.Rows[i][14].ToString(),
                        m_Table.Rows[i][15].ToString(),
                        m_Table.Rows[i][16].ToString()
                        ));
                }
            }
        }

        public List<DailyBasicItem> GetDailyBasic(string ts_code, DateTime stime, DateTime etime)
        {
            Init(stime, etime);
            return m_DailyBasicList.FindAll(it => it.ts_code == ts_code);
        }

        public List<DailyBasicItem> GetDailyBasic(DateTime stime, DateTime etime)
        {
            Init(stime, etime);
            return m_DailyBasicList;
        }
    }
}
