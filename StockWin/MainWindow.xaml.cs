using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockWin
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TableNameItem> TableNameList = new List<TableNameItem>();
        List<TableNameItem> TableInfoList = new List<TableNameItem>();

        public MainWindow()
        {
            InitializeComponent();
            ConfigureReader.Init();
            SQLiteHelper.m_Path = ConfigureReader.ConfigP.StockDBP.StockDBPath;
            DatePicker_Select_SDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            DatePicker_Select_EDate.Text = DateTime.Now.ToShortDateString();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ConfigForm form = new ConfigForm();
            form.Show();
        }

        private void MenuItem_LoadDB_Click(object sender, RoutedEventArgs e)
        {
            DataTable table = SQLiteHelper.GetDataTable(@"SELECT name FROM sqlite_master", new SQLiteParameter[0]);
            for(int i = 0; i < table.Rows.Count; i++)
            {
                string TableName = table.Rows[i][0].ToString();
                if (TableName.StartsWith("ix"))
                    continue;
                TableNameList.Add(new TableNameItem(TableName));
            }

            DataTable tables_info = SQLiteHelper.GetDataTable(@"SELECT * FROM tables_info", new SQLiteParameter[0]);
            for(int i = 0; i < tables_info.Columns.Count; i++)
            {
                string cName = tables_info.Columns[i].ToString();
                string dateStr = tables_info.Rows[0][i].ToString();
                if (cName == "index")
                    continue;
                DateTime d = DateTime.ParseExact(dateStr,"yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                TableNameItem item = new TableNameItem(cName);
                item.date = d;
                TableInfoList.Add(item);
            }

            foreach(TableNameItem info in TableInfoList)
            {
                string key = info.name.Substring(0, info.name.Length - 5);
                TableNameItem NameItem = TableNameList.Find(it => it.name == key || it.name == info.name);
                if(NameItem != null)
                {
                    NameItem.date = info.date;
                }
            }
            
            ListView_TableTree.ItemsSource = TableNameList;

            GVL.Init();
            ListView_StockList.ItemsSource = GVL.StockBasicMgr.GetStockBasic();
        }

        class TableNameItem
        {
            public string name { get; set; }
            public DateTime date { get; set; }
            public TableNameItem(string na)
            {
                name = na;
                date = DateTime.MinValue;
            }
        }

        private void MenuItem_Test_Click(object sender, RoutedEventArgs e)
        {
            List<DailyBasicMgr.DailyBasicItem> list = GVL.DailyBasicMgr.GetDailyBasic(DateTime.Now.AddDays(-10),DateTime.Now.AddDays(-5));
        }

        private void Button_MoneyFlowAndPrice_Click(object sender, RoutedEventArgs e)
        {
            MoneyFowAndPriceForm form = new MoneyFowAndPriceForm();
            form.Show();
        }

        private void Button_Do_Select_Click(object sender, RoutedEventArgs e)
        {
            List<StockBasicMgr.StockBasicItem> stockSubList = GVL.StockBasicMgr.GetStockBasic();
            List<ShowStockItem> showList = new List<ShowStockItem>();
            
            if (!String.IsNullOrEmpty(TextBox_KeyWord.Text))
            {
                stockSubList = stockSubList.FindAll(it => it.name.IndexOf(TextBox_KeyWord.Text) > -1);
            }
            foreach(StockBasicMgr.StockBasicItem item in stockSubList)
            {
                showList.Add(new ShowStockItem(item.ts_code, item.name));
            }

            ListView_StockList.ItemsSource = showList;
            
        }

        private void Button_Do_ChangeSelect_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(TextBox_Turnover_low.Text) && !string.IsNullOrEmpty(TextBox_Turnover_height.Text))
            {
                DateTime sDate = Convert.ToDateTime(DatePicker_Select_SDate.Text);
                DateTime eDate = Convert.ToDateTime(DatePicker_Select_EDate.Text);

                string sDateStr = sDate.ToString("yyyyMMdd");
                string eDateStr = eDate.ToString("yyyyMMdd");

                string turnover_low = TextBox_Turnover_low.Text;
                string turnover_height = TextBox_Turnover_height.Text;

                DataTable tables_info = SQLiteHelper.GetDataTable(@"SELECT stock_basic.ts_code,stock_basic.name FROM stock_basic,daily_basic 
                                                                    where stock_basic.ts_code = daily_basic.ts_code and daily_basic.trade_date = '"+sDateStr+@"'
                                                                     and daily_basic.turnover_rate_f >="+turnover_low+@" and daily_basic.turnover_rate_f <="+turnover_height, new SQLiteParameter[0]);
                List<ShowStockItem> showList = new List<ShowStockItem>();
                for (int i = 0; i < tables_info.Rows.Count; i++)
                {
                    showList.Add(new ShowStockItem(tables_info.Rows[i][0].ToString(), tables_info.Rows[i][1].ToString()));
                }
                ListView_StockList.ItemsSource = showList;
            }
        }

        private void MenuItem_Concept_Click(object sender, RoutedEventArgs e)
        {
            ConceptForm form = new ConceptForm();
            form.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GVL.MsgCMDMgr.Release();
            System.Environment.Exit(0);
        }
    }
}
