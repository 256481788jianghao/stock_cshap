using Microsoft.Win32;
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
    /// ConfigForm.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigForm : Window
    {
        public ConfigForm()
        {
            InitializeComponent();
            InitP();
        }

        private void InitP()
        {
            TextBox_DBPath.Text = ConfigureReader.ConfigP.StockDBP.StockDBPath;
        }

        private void Button_DBSelectPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                TextBox_DBPath.Text = dialog.FileName;
            }
        }

        private void Button_DBSave_Click(object sender, RoutedEventArgs e)
        {
            ConfigureReader.ConfigP.StockDBP.StockDBPath = TextBox_DBPath.Text;
            ConfigureReader.Save();
            MessageBox.Show("save good");
        }
    }
}
