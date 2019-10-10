using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockWin
{
    class GVL
    {
        public static StockBasicMgr StockBasicMgr = new StockBasicMgr();
        public static TradeCalMgr TradeCalMgr = new TradeCalMgr();
        public static AdjFactorMgr AdjFactorMgr = new AdjFactorMgr();
        public static MoneyFlowMgr MoneyFlowMgr = new MoneyFlowMgr();
        public static DailyMgr DailyMgr = new DailyMgr();
        public static DailyBasicMgr DailyBasicMgr = new DailyBasicMgr();
        public static ConceptInfoMgr ConceptInfoMgr = new ConceptInfoMgr();
        public static MsgCMDMgr MsgCMDMgr = new MsgCMDMgr();

        public static void Init()
        {
            StockBasicMgr.Init();
            TradeCalMgr.Init();
            ConceptInfoMgr.Init();
            MsgCMDMgr.Init();
            
        }

        public static DateTime StrToDt(string str)
        {
            return DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
        }

        public static List<double> MakeUniqList(List<double> source_list)
        {
            double maxValue = source_list.Max<double>();
            List<double> ans = new List<double>();
            if (maxValue == 0)
            {
                double minValue = source_list.Min<double>();
                if (minValue == 0)
                    return source_list;
                maxValue = -minValue;
            }
            foreach (double item in source_list)
            {
                ans.Add(item / maxValue);
            }
            return ans;
        }

        public static List<int> MakeUniqList(List<int> source_list)
        {
            int maxValue = source_list.Max<int>();
            List<int> ans = new List<int>();
            if (maxValue == 0)
            {
                int minValue = source_list.Min<int>();
                if (minValue == 0)
                    return source_list;
                maxValue = -minValue;
            }
            foreach (int item in source_list)
            {
                ans.Add(item / maxValue);
            }
            return ans;
        }

        public static double FindMidValue(List<double> list)
        {
            int num = list.Count;
            if (num == 0)
            {
                return -100000;
            }
            list.Sort((a, b) => {
                if (a > b) { return 1; }
                else if (a == b) { return 0; }
                else { return -1; }
            });
            return list[num / 2];
        }
        public static double Sum(List<double> list)
        {
            double ret = 0;
            foreach(double item in list)
            {
                ret += item;
            }
            return ret;
        }
        public static double Mean(List<double> list)
        {
            if(list.Count == 0)
            {
                return 0;
            }
            else
            {
                return Sum(list) / list.Count;
            }
        }

        public static double Var(List<double> list)
        {
            if(list.Count == 0)
            {
                return 0;
            }
            else
            {
                double m_v = Mean(list);
                double ret = 0;
                foreach(double item in list)
                {
                    ret += Math.Pow(item - m_v, 2);
                }
                return ret / list.Count;
            }
        }

        public static double Cov(List<double> list1, List<double> list2)
        {
            if(list1.Count != list2.Count || list1.Count == 0)
            {
                return 0;
            }
            else
            {
                List<double> r_list = new List<double>();
                double m_l1 = Mean(list1);
                double m_l2 = Mean(list2);
                for(int i = 0; i < list1.Count; i++)
                {
                    r_list.Add((list1[i] - m_l1) * (list2[i] - m_l2));
                }
                return Mean(r_list);
            }
        }

        public static double P_relation(List<double> list1, List<double> list2)
        {
            double v_l1 = Var(list1);
            double v_l2 = Var(list2);
            double ret = 0;
            if(v_l1 == 0 || v_l2 == 0)
            {
                return ret;
            }
            else
            {
                double v_cov = Cov(list1, list2);
                return v_cov / Math.Sqrt(v_l1) / Math.Sqrt(v_l2);
            }
        }
    }
}
