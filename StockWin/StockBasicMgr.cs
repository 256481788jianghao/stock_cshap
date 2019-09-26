using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockWin
{
    public class StockBasicMgr
    {
        DataTable m_Table = null;

        public class StockBasicItem
        {
            public string ts_code { get; set; } // TS代码
            public string symbol { get; set; } // 股票代码
            public string name {get;set;} // 股票名称
            public string area {get;set;} // 所在地域
            public string industry {get;set;} // 所属行业
            //public string fullname {get;set;} // 股票全称
            //public string enname {get;set;} // 英文全称
            //public string market {get;set;} // 市场类型 （主板/中小板/创业板）
            //public string exchange {get;set;} // 交易所代码
            //public string curr_type {get;set;} // 交易货币
            //public string list_status {get;set;} // 上市状态： L上市 D退市 P暂停上市
            public DateTime list_date {get;set;} // 上市日期
            //public string delist_date {get;set;} // 退市日期
            //public string is_hs {get;set;} // 是否沪深港通标的，N否 H沪股通 S深股通

            public StockBasicItem()
            {
            }

            public StockBasicItem(string ts_code, string symbol, string name, string area, string industry, string list_date)
            {
                this.ts_code = ts_code;
                this.symbol = symbol;
                this.name = name;
                this.area = area;
                this.industry = industry;
                this.list_date = DateTime.ParseExact(list_date,"yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            }
        }

        List<StockBasicItem> m_StockBasicList = new List<StockBasicItem>();

        public void Init()
        {
            m_Table = SQLiteHelper.GetDataTable(@"SELECT * FROM stock_basic", new SQLiteParameter[0]);
            if(m_Table != null)
            {
                for(int i = 0; i < m_Table.Rows.Count; i++)
                {
                    m_StockBasicList.Add( new StockBasicItem(
                        m_Table.Rows[i][1].ToString(),
                        m_Table.Rows[i][2].ToString(),
                        m_Table.Rows[i][3].ToString(),
                        m_Table.Rows[i][4].ToString(),
                        m_Table.Rows[i][5].ToString(),
                        m_Table.Rows[i][7].ToString())
                        );
                }
            }
        }

        public List<StockBasicItem> GetStockBasic()
        {
            return m_StockBasicList;
        }

        public List<String> GetIndustrys()
        {
            List<String> ret = new List<string>();
            foreach(StockBasicItem item in m_StockBasicList)
            {
                if(!String.IsNullOrEmpty(item.industry) && ret.FindIndex(it => it == item.industry) < 0)
                {
                    ret.Add(item.industry);
                }
            }
            return ret;
        }
    }
}
