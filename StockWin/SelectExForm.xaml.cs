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
    /// SelectExForm.xaml 的交互逻辑
    /// </summary>
    public partial class SelectExForm : Window
    {
        public delegate void UpdateList(List<StockBasicMgr.StockBasicItem> list);

        public event UpdateList OnUpdateList;

        List<StockBasicMgr.StockBasicItem> m_list;

        public SelectExForm()
        {
            InitializeComponent();
            m_list = GVL.StockBasicMgr.GetStockBasic();
        }

        private void Button_ReSet_Click(object sender, RoutedEventArgs e)
        {
            m_list = GVL.StockBasicMgr.GetStockBasic();
        }

        private void Button_Select_Click(object sender, RoutedEventArgs e)
        {
            OnUpdateList?.Invoke(m_list);
        }
    }
}
