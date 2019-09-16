using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockWin
{
    class MoneyFlowMgr
    {
        public class MoneyFlowItem
        {
            public string ts_code; // TS代码
            public DateTime trade_date; // 交易日期
            public int buy_sm_vol; //    小单买入量（手）
            public double buy_sm_amount; //    小单买入金额（万元）
            public int sell_sm_vol; //    小单卖出量（手）
            public double sell_sm_amount; //    小单卖出金额（万元）
            public int buy_md_vol; //    中单买入量（手）
            public double buy_md_amount; //    中单买入金额（万元）
            public int sell_md_vol; //    中单卖出量（手）
            public double sell_md_amount; //    中单卖出金额（万元）
            public int buy_lg_vol; //    大单买入量（手）
            public double buy_lg_amount; //    大单买入金额（万元）
            public int sell_lg_vol; //    大单卖出量（手）
            public double sell_lg_amount; //    大单卖出金额（万元）
            public int buy_elg_vol; //    特大单买入量（手）
            public double buy_elg_amount; //    特大单买入金额（万元）
            public int sell_elg_vol; //    特大单卖出量（手）
            public double sell_elg_amount; //    特大单卖出金额（万元）
            public int net_mf_vol; //    净流入量（手）
            public double net_mf_amount; //    净流入额（万元）

            public MoneyFlowItem(string ts_code, string trade_date, string buy_sm_vol, string buy_sm_amount, string sell_sm_vol, string sell_sm_amount, string buy_md_vol, string buy_md_amount, string sell_md_vol, string sell_md_amount, string buy_lg_vol, string buy_lg_amount, string sell_lg_vol, string sell_lg_amount, string buy_elg_vol, string buy_elg_amount, string sell_elg_vol, string sell_elg_amount, string net_mf_vol, string net_mf_amount)
            {
                this.ts_code = ts_code;
                this.trade_date = DateTime.ParseExact(trade_date, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture); 
                this.buy_sm_vol = Convert.ToInt32(buy_sm_vol);
                this.buy_sm_amount = Convert.ToDouble(buy_sm_amount);
                this.sell_sm_vol = Convert.ToInt32(sell_sm_vol);
                this.sell_sm_amount = Convert.ToDouble(sell_sm_amount);
                this.buy_md_vol = Convert.ToInt32(buy_md_vol);
                this.buy_md_amount = Convert.ToDouble(buy_md_amount);
                this.sell_md_vol = Convert.ToInt32(sell_md_vol);
                this.sell_md_amount = Convert.ToDouble(sell_md_amount);
                this.buy_lg_vol = Convert.ToInt32(buy_lg_vol);
                this.buy_lg_amount = Convert.ToDouble(buy_lg_amount);
                this.sell_lg_vol = Convert.ToInt32(sell_lg_vol);
                this.sell_lg_amount = Convert.ToDouble(sell_lg_amount);
                this.buy_elg_vol = Convert.ToInt32(buy_elg_vol);
                this.buy_elg_amount = Convert.ToDouble(buy_elg_amount);
                this.sell_elg_vol = Convert.ToInt32(sell_elg_vol);
                this.sell_elg_amount = Convert.ToDouble(sell_elg_amount);
                this.net_mf_vol = Convert.ToInt32(net_mf_vol);
                this.net_mf_amount = Convert.ToDouble(net_mf_amount);
            }
        }

        List<MoneyFlowItem> m_MoneyFlowList = new List<MoneyFlowItem>();

        DataTable m_Table = null;

        private void Init(string ts_code, DateTime stime, DateTime etime)
        {
            m_MoneyFlowList.Clear();
            string st = stime.ToString("yyyyMMdd");
            string et = etime.ToString("yyyyMMdd");
            string cmd = @"SELECT * FROM money_flow where trade_date >= " + st + " and trade_date <= " + et + " and ts_code = '" + ts_code + "'";
            m_Table = SQLiteHelper.GetDataTable(cmd, new SQLiteParameter[0]);
            if (m_Table != null)
            {
                for (int i = 0; i < m_Table.Rows.Count; i++)
                {
                    m_MoneyFlowList.Add(new MoneyFlowItem(
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
                        m_Table.Rows[i][16].ToString(),
                        m_Table.Rows[i][17].ToString(),
                        m_Table.Rows[i][18].ToString(),
                        m_Table.Rows[i][19].ToString(),
                        m_Table.Rows[i][20].ToString()
                        ));
                }
            }
        }

        public List<MoneyFlowItem> GetMoneyFlow(string ts_code, DateTime stime, DateTime etime)
        {
            Init(ts_code, stime, etime);
            return m_MoneyFlowList;
        }
    }
}
