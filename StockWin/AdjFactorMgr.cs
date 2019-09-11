using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockWin
{
    class AdjFactorMgr
    {
        public class AdjFactorItem
        {
            public string ts_code; // str 股票代码
            public DateTime trade_date; //   str 交易日期
            public double adj_factor; //  float 复权因子

            public AdjFactorItem(string ts_code, string trade_date, string adj_factor)
            {
                this.ts_code = ts_code;
                this.trade_date = DateTime.ParseExact(trade_date, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture); 
                this.adj_factor = Convert.ToDouble(adj_factor);
            }
        }

        List<AdjFactorItem> m_AdjFactorList = new List<AdjFactorItem>();
        DataTable m_Table = null;

        private void Init(string ts_code, DateTime stime)
        {
            m_AdjFactorList.Clear();
            string cmd = @"SELECT * FROM adj_factor";
            if (String.IsNullOrEmpty(ts_code))
            {
                if(stime != DateTime.MinValue)
                {
                    string date = stime.ToString("yyyyMMdd");
                    cmd += " where trade_date = '" + date + "'";
                }
            }
            else
            {
                cmd += " where ts_code = '" + ts_code + "'";
                if (stime != DateTime.MinValue)
                {
                    string date = stime.ToString("yyyyMMdd");
                    cmd += " and trade_date = '" + date + "'";
                }
            }
            m_Table = SQLiteHelper.GetDataTable(cmd, new SQLiteParameter[0]);
            if (m_Table != null)
            {
                for (int i = 0; i < m_Table.Rows.Count; i++)
                {
                    m_AdjFactorList.Add(new AdjFactorItem(
                        m_Table.Rows[i][1].ToString(),
                        m_Table.Rows[i][2].ToString(),
                        m_Table.Rows[i][3].ToString()
                        ));
                }
            }
        }

        public List<AdjFactorItem> GetAdjFactor(string ts_code, DateTime stime)
        {
            Init(ts_code, stime);
            return m_AdjFactorList;
        }
    }
}
