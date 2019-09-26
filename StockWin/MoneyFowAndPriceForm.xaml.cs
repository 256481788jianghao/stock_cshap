using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace StockWin
{
    /// <summary>
    /// MoneyFowAndPriceForm.xaml 的交互逻辑
    /// </summary>
    public partial class MoneyFowAndPriceForm : Window
    {

        public SeriesCollection SeriesCollection { get; set; }

        string m_CurrentTsCode = "";

        public MoneyFowAndPriceForm()
        {
            InitializeComponent();

            m_CurrentDataList = GVL.StockBasicMgr.GetStockBasic();
            ListView_StockList.ItemsSource = m_CurrentDataList;

            DatePicker_SDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            DatePicker_EDate.Text = DateTime.Now.ToShortDateString();


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> { 3, 5, 7, 4 }
                },
                 new ColumnSeries
                {
                    Values = new ChartValues<decimal> { 5, 6, 2}
                }
            };
            DataContext = this;

        }

        private void Carts_MP_MouseWheel(object sender, MouseWheelEventArgs e)
        {
        }

        private void ListView_StockList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                StockBasicMgr.StockBasicItem item = ListView_StockList.SelectedItem as StockBasicMgr.StockBasicItem;
                Label_StockName.Content = item.name;
                m_CurrentTsCode = item.ts_code;
            }catch(Exception ex)
            {

            }
        }

        private void Button_Do_Click(object sender, RoutedEventArgs e)
        {
            DateTime sDate = Convert.ToDateTime(DatePicker_SDate.Text);
            DateTime eDate = Convert.ToDateTime(DatePicker_EDate.Text);
            TimeSpan dTime = eDate - sDate;
            int DaysNum = dTime.Days + 1;
            if(DaysNum < 1)
            {
                MessageBox.Show("日期设置有问题");
                return;
            }
            ProgressBar_Progress.Value = 0;
            if (!String.IsNullOrEmpty(m_CurrentTsCode))
            {
                SeriesCollection.Clear();
                List<DailyMgr.DailyItem> dailyList = GVL.DailyMgr.GetDaily(m_CurrentTsCode, sDate, eDate);
                if(dailyList != null)
                {
                    List<double> ClosePriceList = new List<double>();
                    foreach(DailyMgr.DailyItem item in dailyList)
                    {
                        ClosePriceList.Add(item.close);
                    }

                    ChartValues<double> priceValue = new ChartValues<double>(GVL.MakeUniqList(ClosePriceList));
                    LineSeries closePriceLineSeries = new LineSeries();
                    closePriceLineSeries.Values = priceValue;
                    SeriesCollection.Add(closePriceLineSeries);
                    ProgressBar_Progress.Value = 50;

                    List<MoneyFlowMgr.MoneyFlowItem> moneyflowList = GVL.MoneyFlowMgr.GetMoneyFlow(m_CurrentTsCode, sDate, eDate);
                    if(moneyflowList != null)
                    {
                        List<double> valueList = new List<double>();
                        foreach (MoneyFlowMgr.MoneyFlowItem item in moneyflowList)
                        {
                            switch (ComboBox_MoneyType.SelectedIndex)
                            {
                                case 1:
                                    {
                                        valueList.Add(item.buy_elg_amount + item.buy_lg_amount); break;
                                    }
                                case 2:
                                    {
                                        valueList.Add(item.buy_elg_amount + item.buy_lg_amount - item.sell_elg_amount - item.sell_lg_amount); break;
                                    }
                                case 0:
                                default: valueList.Add(item.buy_elg_amount);break;
                            }
                            
                        }

                        ChartValues<double> moneyValue = new ChartValues<double>(GVL.MakeUniqList(valueList));
                        LineSeries moneyLineSeries = new LineSeries();
                        moneyLineSeries.Values = moneyValue;
                        SeriesCollection.Add(moneyLineSeries);
                        ProgressBar_Progress.Value = 100;
                    }
                }
                else
                {
                    MessageBox.Show("没有当日股价数据数据");
                }
            }
            else
            {
                MessageBox.Show("未选择个股");
            }
        }

        private void Button_Select_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TextBox_KeyWord.Text))
            {
                m_CurrentDataList = GVL.StockBasicMgr.GetStockBasic();
                ListView_StockList.ItemsSource = m_CurrentDataList;
            }
            else
            {
                string keyValue = TextBox_KeyWord.Text;
                //List<StockBasicMgr.StockBasicItem> list = GVL.StockBasicMgr.GetStockBasic();
                List<StockBasicMgr.StockBasicItem> sublist = m_CurrentDataList.FindAll(it => it.name.IndexOf(keyValue) >= 0);
                if(sublist != null)
                {
                    //m_CurrentDataList = sublist;
                    ListView_StockList.ItemsSource = sublist;
                }
            }
        }

        List<StockBasicMgr.StockBasicItem> m_CurrentDataList = null;

        void UpdateList(List<StockBasicMgr.StockBasicItem> list)
        {
            ListView_StockList.ItemsSource = list;
        }

        private void Button_Select_Ex_Click(object sender, RoutedEventArgs e)
        {
            SelectExForm form = new SelectExForm();
            form.OnUpdateList += new SelectExForm.UpdateList(UpdateList);
            form.Show();
        }
    }
}
