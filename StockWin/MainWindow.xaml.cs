using LiveCharts;
using LiveCharts.Wpf;
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
        public SeriesCollection SeriesCollection { get; set; }

        string[] Line_Names_1 = new string[] { "收盘价"};
        string[] Line_Names_2 = new string[] { "成交量","换手率","换手率变化率","特大单+大单买入","特大单+大单净量" };


        public MainWindow()
        {
            InitializeComponent();
            ConfigureReader.Init();
            SQLiteHelper.m_Path = ConfigureReader.ConfigP.StockDBP.StockDBPath;
            DatePicker_Select_SDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            DatePicker_Select_EDate.Text = DateTime.Now.ToShortDateString();

            DataTable tables_info_concept_info = SQLiteHelper.GetDataTable(@"SELECT name FROM concept_info", new SQLiteParameter[0]);
            for (int i = 0; i < tables_info_concept_info.Rows.Count; i++)
            {
                ComboBox_Bankuai_Select.Items.Add(tables_info_concept_info.Rows[i][0].ToString());
            }

            SeriesCollection = new SeriesCollection();
            LineSeries l1 = new LineSeries();
            l1.Values = new ChartValues<double> { 3, 5, 7, 4 };
            LineSeries l2 = new LineSeries();
            l2.Values = new ChartValues<double> { 0.1, 0.2, 0.5 };
            l2.ScalesYAt = 1;
            SeriesCollection.Add(l1);
            SeriesCollection.Add(l2);
            DataContext = this;

            foreach(string item in Line_Names_1)
            {
                ComBox_Line1.Items.Add(item);
            }

            foreach (string item in Line_Names_2)
            {
                ComBox_Line2.Items.Add(item);
            }

            ComBox_Line1.SelectedIndex = 0;
            ComBox_Line2.SelectedIndex = 0;
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
            List<ShowStockItem> showList = new List<ShowStockItem>();
            foreach(StockBasicMgr.StockBasicItem item in GVL.StockBasicMgr.GetStockBasic())
            {
                showList.Add(new ShowStockItem(item.ts_code, item.name));
            }
            ListView_StockList.ItemsSource = showList;
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

        private void Button_Do_Bankuai_Click(object sender, RoutedEventArgs e)
        {
            if(ComboBox_Bankuai_Select.SelectedIndex > -1)
            {
                string concept_name = ComboBox_Bankuai_Select.SelectedItem as string;
                List<ConceptInfoMgr.ConceptInfoItem> m_list = GVL.ConceptInfoMgr.GetConceptInfo();
                List<ConceptInfoMgr.ConceptInfoItem> m_sub_list = m_list.FindAll(it => it.concept_name == concept_name);
                if(concept_name != null)
                {
                    List<ShowStockItem> showList = new List<ShowStockItem>();
                    for (int i = 0; i < m_sub_list.Count; i++)
                    {
                        showList.Add(new ShowStockItem(m_sub_list[i].ts_code, m_sub_list[i].name));
                    }
                    ListView_StockList.ItemsSource = showList;
                }
            }
        }

        private void ListView_StockList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ListView_StockList.SelectedIndex > -1)
            {
                m_CurrentSelectItem = ListView_StockList.SelectedItem as ShowStockItem;
                if(m_CurrentSelectItem != null)
                {
                    Label_Stock_Name.Content = m_CurrentSelectItem.name;
                }
            }
        }
        ShowStockItem m_CurrentSelectItem = null;
        private void Button_Do_DrawLine_Click(object sender, RoutedEventArgs e)
        {
            if(m_CurrentSelectItem == null) { return; }
            DateTime sTime = Convert.ToDateTime(DatePicker_Select_SDate.Text);
            DateTime eTime = Convert.ToDateTime(DatePicker_Select_EDate.Text);

            List<double> l1_values = new List<double>();
            List<double> l2_values = new List<double>();

            SeriesCollection.Clear();

            if (ComBox_Line1.Text == Line_Names_1[0])
            {
                List<DailyMgr.DailyItem> daily_list = GVL.DailyMgr.GetDaily(m_CurrentSelectItem.ts_code, sTime, eTime);
                if (daily_list != null)
                {
                    foreach (DailyMgr.DailyItem item in daily_list)
                    {
                        l1_values.Add(item.close);
                    }
                }
            }

            if(ComBox_Line2.Text == Line_Names_2[0])
            {
                List<DailyMgr.DailyItem> daily_list = GVL.DailyMgr.GetDaily(m_CurrentSelectItem.ts_code, sTime, eTime);
                if (daily_list != null)
                {
                    foreach (DailyMgr.DailyItem item in daily_list)
                    {
                        l2_values.Add(item.vol);
                    }
                }
            }
            else if(ComBox_Line2.Text == Line_Names_2[1])
            {
                List<DailyBasicMgr.DailyBasicItem> dailybasic_list = GVL.DailyBasicMgr.GetDailyBasic(m_CurrentSelectItem.ts_code, sTime, eTime);
                if(dailybasic_list != null)
                {
                    foreach (DailyBasicMgr.DailyBasicItem item in dailybasic_list)
                    {
                        l2_values.Add(item.turnover_rate_f);
                    }
                }
            }
            else if (ComBox_Line2.Text == Line_Names_2[2])
            {
                List<DailyBasicMgr.DailyBasicItem> dailybasic_list = GVL.DailyBasicMgr.GetDailyBasic(m_CurrentSelectItem.ts_code, sTime, eTime);
                if (dailybasic_list != null)
                {
                    DailyBasicMgr.DailyBasicItem last_item = null;
                    if(dailybasic_list.Count < 2) { return; }
                    for (int i = 0; i < dailybasic_list.Count; i++)
                    {
                        DailyBasicMgr.DailyBasicItem item = dailybasic_list[i];
                        if ( i == 0)
                        {
                            last_item = item;
                            l2_values.Add(0);
                        }
                        else
                        {
                            l2_values.Add((item.turnover_rate_f - last_item.turnover_rate_f) / last_item.turnover_rate_f);
                            last_item = item;
                        }
                        
                    }
                }
            }
            else if (ComBox_Line2.Text == Line_Names_2[3])
            {
                List<MoneyFlowMgr.MoneyFlowItem> MoneyFlowList = GVL.MoneyFlowMgr.GetMoneyFlow(m_CurrentSelectItem.ts_code,sTime, eTime);
                if(MoneyFlowList != null)
                {
                    foreach(MoneyFlowMgr.MoneyFlowItem item in MoneyFlowList)
                    {
                        l2_values.Add(item.buy_elg_vol + item.buy_lg_vol);
                    }
                }
            }
            else if (ComBox_Line2.Text == Line_Names_2[4])
            {
                List<MoneyFlowMgr.MoneyFlowItem> MoneyFlowList = GVL.MoneyFlowMgr.GetMoneyFlow(m_CurrentSelectItem.ts_code, sTime, eTime);
                if (MoneyFlowList != null)
                {
                    foreach (MoneyFlowMgr.MoneyFlowItem item in MoneyFlowList)
                    {
                        l2_values.Add(item.buy_elg_vol + item.buy_lg_vol - item.sell_elg_vol - item.sell_lg_vol);
                    }
                }
            }

            {
                //l1_values.Clear();
                //l2_values.Clear();
                //for(int i = 1; i < 13; i++)
                //{
                //    l1_values.Add(i);
                //    l2_values.Add(i);
                //}
                LineSeries l1 = new LineSeries();
                l1.Values = new ChartValues<double> (l1_values);
                LineSeries l2 = new LineSeries();
                l2.Values = new ChartValues<double> (l2_values);
                l2.ScalesYAt = 1;
                SeriesCollection.Add(l1);
                SeriesCollection.Add(l2);
                MessageBox.Show("相关度："+GVL.P_relation(l1_values, l2_values));
            }
            
        }
    }
}
