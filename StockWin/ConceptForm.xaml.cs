using StockWin.Item;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// ConceptForm.xaml 的交互逻辑
    /// </summary>
    public partial class ConceptForm : Window
    {
        public ConceptForm()
        {
            InitializeComponent();
            DatePicker_sDate.Text = DateTime.Now.ToShortDateString();
            DatePicker_sDate_p.Text = DateTime.Now.AddDays(-30).ToShortDateString();
            DatePicker_eDate_p.Text = DateTime.Now.ToShortDateString();

            GVL.MsgCMDMgr.OnUpdateConceptPListView += new MsgCMDMgr.UpdateConceptPListView(UpdateConceptPListView);
            DataTable tables_info_concept_info = SQLiteHelper.GetDataTable(@"SELECT name FROM concept_info", new SQLiteParameter[0]);
            for (int i = 0; i < tables_info_concept_info.Rows.Count; i++)
            {
                ComboBox_ConceptList.Items.Add(tables_info_concept_info.Rows[i][0].ToString());
            }
        }

        

        private void Button_Do_Click(object sender, RoutedEventArgs e)
        {
            List<string> id_list = new List<string>();
            DataTable tables_info_concept_info = SQLiteHelper.GetDataTable(@"SELECT code FROM concept_info", new SQLiteParameter[0]);
            for(int i=0;i< tables_info_concept_info.Rows.Count; i++)
            {
                id_list.Add(tables_info_concept_info.Rows[i][0].ToString());
            }

            DateTime sDate = Convert.ToDateTime(DatePicker_sDate.Text);
            string sDateStr = sDate.ToString("yyyyMMdd");

            List<ConceptFormItem> m_items = new List<ConceptFormItem>();
            List<DailyMgr.DailyItem> daily_list = GVL.DailyMgr.GetDaily(sDate, sDate);

            string base_concept_name = null;
            if(ComboBox_ConceptList.SelectedIndex > -1)
            {
                base_concept_name = ComboBox_ConceptList.SelectedItem as string;
            }
            foreach (string id in id_list)
            {
                List<ConceptInfoMgr.ConceptInfoItem> subList = GVL.ConceptInfoMgr.GetConceptInfo().FindAll(it => it.code == id);
                double tNum = subList.Count;
                double ac_tNum = tNum;
                string cName = "";
                double HpNum = 0;
                double MeanPNum = 0;
                List<double> pct_change_list = new List<double>();
                for (int i = 0; i < tNum; i++)
                {
                    cName = subList[i].concept_name;
                    string ts_code = subList[i].ts_code;
                    DailyMgr.DailyItem daily_item = daily_list.Find(it => it.ts_code == ts_code);
                    if(daily_item != null)
                    {
                        double pct_change = daily_item.pct_change;
                        if(pct_change > 9.9)
                        {
                            HpNum += 1;
                        }
                        MeanPNum += pct_change;
                        pct_change_list.Add(pct_change);
                    }
                    else
                    {
                        ac_tNum -= 1;
                    }
                }
                MeanPNum = MeanPNum / ac_tNum;
                double MidPValue = GVL.FindMidValue(pct_change_list);
                double relationship = 0;
                if (!string.IsNullOrEmpty(base_concept_name))
                {
                    relationship = GVL.ConceptInfoMgr.GetRelationShipEx(id, base_concept_name);
                }
                m_items.Add(new ConceptFormItem(cName, tNum, HpNum, MeanPNum, MidPValue, relationship));
            }

            m_items.Sort((a,b)=> {
                if(a.MeanChange > b.MeanChange)
                {
                    return -1;
                }
                else if(a.MeanChange == b.MeanChange)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            });
            ListView_Main.ItemsSource = m_items;
        }

        List<ConceptPStaFormItem> m_ConceptPStaFormItemList = new List<ConceptPStaFormItem>();
        void UpdateConceptPListView(string dateStr, string name_1, string name_2, string name_3,string last_name_1, string last_name_2, string last_name_3,bool isEnd)
        {
            if (!isEnd)
            {
                SetConceptPStaFormItemList(name_1, 1);
                SetConceptPStaFormItemList(name_2, 2);
                SetConceptPStaFormItemList(name_3, 3);
                SetConceptPStaFormItemList(last_name_1, -1);
                SetConceptPStaFormItemList(last_name_2, -2);
                SetConceptPStaFormItemList(last_name_3, -3);
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    ListView_Main_P.Items.Insert(0, new ConceptPFormItem(dateStr, name_1, name_2, name_3, last_name_1, last_name_2, last_name_3));
                }));
            }
            else
            {
                m_ConceptPStaFormItemList.Sort((a, b) =>
                {
                    int f_num_a = a.name_1_num + a.name_2_num + a.name_3_num;
                    int b_num_a = a.last_name_1_num + a.last_name_2_num + a.last_name_3_num;
                    int f_num_b = b.name_1_num + b.name_2_num + b.name_3_num;
                    int b_num_b = b.last_name_1_num + b.last_name_2_num + b.last_name_3_num;

                    if(f_num_a > f_num_b)
                    {
                        return -1;
                    }else if(f_num_a == f_num_b)
                    {
                        if(b_num_a < b_num_b) { return -1; }
                        else if(b_num_a == b_num_b) { return 0; }
                        else { return 1; }
                    }
                    else { return 1; }

                });
                Dispatcher.BeginInvoke(new Action(delegate {
                    ListView_Main_P_Sta.ItemsSource = m_ConceptPStaFormItemList;
                }));
                
            }
            
        }

        private void SetConceptPStaFormItemList(string name,int type)
        {
            ConceptPStaFormItem item = m_ConceptPStaFormItemList.Find(it => it.concept_name == name);
            if(item == null)
            {
                item = new ConceptPStaFormItem(name);
                m_ConceptPStaFormItemList.Add(item);
            }
            switch (type)
            {
                case 1: { item.name_1_num += 1;break; }
                case 2: { item.name_2_num += 1; break; }
                case 3: { item.name_3_num += 1; break; }
                case -1: { item.last_name_1_num += 1; break; }
                case -2: { item.last_name_2_num += 1; break; }
                case -3: { item.last_name_3_num += 1; break; }
                default:break;
            }
        }

        private void Button_Do_P_Click(object sender, RoutedEventArgs e)
        {
            ListView_Main_P.Items.Clear();
            m_ConceptPStaFormItemList = new List<ConceptPStaFormItem>();

            DateTime sDate = Convert.ToDateTime(DatePicker_sDate_p.Text);
            DateTime eDate = Convert.ToDateTime(DatePicker_eDate_p.Text);

            MsgCMDMgr.Msg msg = new MsgCMDMgr.Msg();
            msg.command = MsgCMDMgr.MsgCMD.UpdateConceptPListView;
            msg.sDate = sDate;
            msg.eDate = eDate;
            if (Convert.ToBoolean(CheckBox_USE_Qian.IsChecked))
            {
                msg.use_qian = true;
                msg.use_qian_value = Convert.ToDouble(TextBox_Use_Qian.Text);
            }
            else
            {
                msg.use_qian = false;
            }
            GVL.MsgCMDMgr.PushCMD(msg);
        }

        private void ListView_Main_P_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void ComboBox_ConceptList_TextInput(object sender, TextCompositionEventArgs e)
        {
            //string key = e.Text;
            //if (!string.IsNullOrEmpty(key))
            //{
            //    ComboBox_ConceptList.Items.Clear();
            //}
        }
    }
}
