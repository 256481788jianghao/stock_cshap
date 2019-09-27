using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockWin.Item
{
    class ConceptPStaFormItem
    {
        public string concept_name { get; set; }
        public int name_1_num { get; set; }
        public int name_2_num { get; set; }
        public int name_3_num { get; set; }
        public int last_name_1_num { get; set; }
        public int last_name_2_num { get; set; }
        public int last_name_3_num { get; set; }

        public ConceptPStaFormItem(string concept_name)
        {
            this.concept_name = concept_name;
        }
    }
}
