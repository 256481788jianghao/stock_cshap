using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockWin.Item
{
    class ConceptPFormItem
    {
        public string DateStr { get; set; }
        public string name_1 { get; set; }
        public string name_2 { get; set; }
        public string name_3 { get; set; }
        public string last_name_1 { get; set; }
        public string last_name_2 { get; set; }
        public string last_name_3 { get; set; }

        public ConceptPFormItem(string dateStr, string name_1, string name_2, string name_3, string last_name_1, string last_name_2, string last_name_3) : this(dateStr, name_1, name_2, name_3)
        {
            this.last_name_1 = last_name_1;
            this.last_name_2 = last_name_2;
            this.last_name_3 = last_name_3;
        }

        public ConceptPFormItem(string dateStr, string name_1, string name_2, string name_3)
        {
            DateStr = dateStr;
            this.name_1 = name_1;
            this.name_2 = name_2;
            this.name_3 = name_3;
        }
    }
}
