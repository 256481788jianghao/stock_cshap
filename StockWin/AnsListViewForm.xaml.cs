﻿using System;
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
    /// AnsListViewForm.xaml 的交互逻辑
    /// </summary>
    public partial class AnsListViewForm : Window
    {
        public AnsListViewForm()
        {
            InitializeComponent();
        }

        public void SetTitle(string title)
        {
            this.Title = title;
        }

        public void SetColumns(string [] names)
        {
            foreach (string item in names)
            {
                GridViewColumn col = new GridViewColumn();
                col.Header = item;
                col.DisplayMemberBinding = new Binding(item);
                GridView_Ans.Columns.Add(col);
            }
        }

        public void SetColumns(string[] Headers, string [] BindingNames)
        {
            if(Headers.Count() != BindingNames.Count())
            {
                MessageBox.Show("名字和绑定必须相等！！！");
                return;
            }
            for(int i=0;i<Headers.Count();i++)
            {
                GridViewColumn col = new GridViewColumn();
                col.Header = Headers[i];
                col.DisplayMemberBinding = new Binding(BindingNames[i]);
                GridView_Ans.Columns.Add(col);
            }
        }

        public void SetItemsSource<T>(List<T> source)
        {
            ListView_Ans.ItemsSource = source;
        }
    }
}
