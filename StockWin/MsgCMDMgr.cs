using StockWin.Item;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockWin
{
    class MsgCMDMgr
    {
        public enum MsgCMD
        {
            UpdateConceptPListView
        }
        public class Msg
        {
            public MsgCMD command;
            public DateTime sDate;
            public DateTime eDate;
        }

        List<Msg> m_MsgList = new List<Msg>();
        Semaphore m_MsgSem = new Semaphore(0, 1000);
        object m_MsgListLock = new object();
        Thread m_msgThread;
        bool m_threadloop = true;

        public delegate void UpdateConceptPListView(string dateStr, string name_1, string name_2, string name_3, string last_name_1, string last_name_2, string last_name_3);

        public event UpdateConceptPListView OnUpdateConceptPListView;

        public MsgCMDMgr()
        {
            m_msgThread = new Thread(new ThreadStart(MsgThread));
            m_msgThread.Start();
        }

        public void Init()
        {

        }

        public void Release()
        {
            m_threadloop = false;
        }

        public void PushCMD(Msg msg)
        {
            lock (m_MsgListLock)
            {
                m_MsgList.Add(msg);
                if(m_MsgList.Count == 1)
                {
                    m_MsgSem.Release();
                }
            }
            
        }

        private void MsgThread()
        {
            while (m_threadloop)
            {
                m_MsgSem.WaitOne();
                lock (m_MsgListLock)
                {
                    if (m_MsgList.Count > 0)
                    {
                        Msg msg = m_MsgList.First();
                        if(msg != null)
                        {
                            if (msg.command == MsgCMD.UpdateConceptPListView)
                            {
                                DateTime sDate = msg.sDate;
                                DateTime eDate = msg.eDate;
                                List<DailyMgr.DailyItem> daily_list = GVL.DailyMgr.GetDaily(sDate, eDate);

                                List<string> id_list = new List<string>();
                                DataTable tables_info_concept_info = SQLiteHelper.GetDataTable(@"SELECT code FROM concept_info", new SQLiteParameter[0]);
                                for (int i = 0; i < tables_info_concept_info.Rows.Count; i++)
                                {
                                    id_list.Add(tables_info_concept_info.Rows[i][0].ToString());
                                }

                                for (DateTime tDate = sDate; tDate <= eDate; tDate = tDate.AddDays(1))
                                {
                                    if (!GVL.TradeCalMgr.IsOpenDate(tDate)) { continue; }
                                    List<ConceptFormItem> m_items = new List<ConceptFormItem>();
                                    foreach (string id in id_list)
                                    {
                                        List<ConceptInfoMgr.ConceptInfoItem> subList = GVL.ConceptInfoMgr.GetConceptInfo().FindAll(it => it.code == id);
                                        double tNum = subList.Count;
                                        string cName = "";
                                        double HpNum = 0;
                                        double MeanPNum = 0;
                                        List<double> pct_change_list = new List<double>();
                                        for (int i = 0; i < tNum; i++)
                                        {
                                            cName = subList[i].concept_name;
                                            string ts_code = subList[i].ts_code;
                                            DailyMgr.DailyItem daily_item = daily_list.Find(it => it.ts_code == ts_code && it.trade_date == tDate);
                                            if (daily_item != null)
                                            {
                                                double pct_change = daily_item.pct_change;
                                                if (pct_change > 9.9)
                                                {
                                                    HpNum += 1;
                                                }
                                                MeanPNum += pct_change;
                                                pct_change_list.Add(pct_change);
                                            }
                                        }
                                        MeanPNum = MeanPNum / tNum;
                                        double MidPValue = GVL.FindMidValue(pct_change_list);
                                        m_items.Add(new ConceptFormItem(cName, tNum, HpNum, MeanPNum, MidPValue));
                                    }

                                    if (m_items.Count < 3) { continue; }

                                    m_items.Sort((a, b) =>
                                    {
                                        if (a.MeanChange > b.MeanChange)
                                        {
                                            return -1;
                                        }
                                        else if (a.MeanChange == b.MeanChange)
                                        {
                                            return 0;
                                        }
                                        else
                                        {
                                            return 1;
                                        }
                                    });

                                    string tDateStr = tDate.ToString("yyyyMMdd");
                                    int items_cout = m_items.Count;
                                    OnUpdateConceptPListView?.Invoke(tDateStr, m_items[0].ConceptName, m_items[1].ConceptName, m_items[2].ConceptName, m_items[items_cout-1].ConceptName, m_items[items_cout - 2].ConceptName, m_items[items_cout - 3].ConceptName);
                                }
                            }
                        }

                        m_MsgList.RemoveAt(0);
                        if (m_MsgList.Count > 0)
                        {
                            m_MsgSem.Release();
                        }
                    }
                }
            }
        }
    }
}
