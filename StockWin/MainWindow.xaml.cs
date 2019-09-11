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

        StockBasicMgr m_StockBasicMgr = new StockBasicMgr();
        TradeCalMgr m_TradeCalMgr = new TradeCalMgr();
        AdjFactorMgr m_AdjFactorMgr = new AdjFactorMgr();

        public MainWindow()
        {
            InitializeComponent();
            ConfigureReader.Init();
            SQLiteHelper.m_Path = ConfigureReader.ConfigP.StockDBP.StockDBPath;
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

            m_StockBasicMgr.Init();
            m_TradeCalMgr.Init();
            //m_AdjFactorMgr.Init();
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
            List<AdjFactorMgr.AdjFactorItem> list = m_AdjFactorMgr.GetAdjFactor("000001.SZ",DateTime.MinValue);
        }
    }
}
