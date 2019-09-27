using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockWin
{
    class ConceptInfoMgr
    {
        public class ConceptInfoItem
        {
            public string code { get; set; }
            public string concept_name { get; set; }
            public string ts_code { get; set; }

            public ConceptInfoItem(string code, string concept_name, string ts_code)
            {
                this.code = code;
                this.concept_name = concept_name;
                this.ts_code = ts_code;
            }
        }

        List<ConceptInfoItem> m_List = new List<ConceptInfoItem>();
        DataTable m_Table = null;

        public void Init()
        {
            m_Table = SQLiteHelper.GetDataTable(@"SELECT id,concept_name,ts_code FROM concept_detail", new SQLiteParameter[0]);
            if (m_Table != null)
            {
                for (int i = 0; i < m_Table.Rows.Count; i++)
                {
                    m_List.Add(new ConceptInfoItem(
                        m_Table.Rows[i][0].ToString(),
                        m_Table.Rows[i][1].ToString(),
                        m_Table.Rows[i][2].ToString()
                        ));
                }
            }
        }

        public List<ConceptInfoItem> GetConceptInfo()
        {
            //Init();
            return m_List;
        }
    }
}
