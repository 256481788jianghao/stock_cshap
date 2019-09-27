﻿using System;
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
    }
}
