using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockWin.Item
{
    class ConceptFormItem
    {
        public string ConceptName { get; set; }
        public double SNum { get; set; }
        public double HStockNum { get; set; }
        public double MeanChange { get; set; }
        public double MidChange { get; set; }
        public double RelationShipRate { get; set; }

        public ConceptFormItem(string conceptName, double sNum, double hStockNum, double meanChange, double midChange, double relationShipRate) : this(conceptName, sNum, hStockNum, meanChange, midChange)
        {
            RelationShipRate = relationShipRate;
        }

        public ConceptFormItem(string conceptName, double sNum, double hStockNum, double meanChange, double midChange) : this(conceptName, sNum, hStockNum, meanChange)
        {
            MidChange = midChange;
        }

        public ConceptFormItem(string conceptName, double sNum, double hStockNum, double meanChange)
        {
            ConceptName = conceptName;
            SNum = sNum;
            HStockNum = hStockNum;
            MeanChange = meanChange;
        }
    }
}
