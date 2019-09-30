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
            public string name { get; set; }

            public ConceptInfoItem(string code, string concept_name, string ts_code, string name) : this(code, concept_name, ts_code)
            {
                this.name = name;
            }

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
            m_Table = SQLiteHelper.GetDataTable(@"SELECT id,concept_name,ts_code,name FROM concept_detail", new SQLiteParameter[0]);
            if (m_Table != null)
            {
                for (int i = 0; i < m_Table.Rows.Count; i++)
                {
                    m_List.Add(new ConceptInfoItem(
                        m_Table.Rows[i][0].ToString(),
                        m_Table.Rows[i][1].ToString(),
                        m_Table.Rows[i][2].ToString(),
                        m_Table.Rows[i][3].ToString()
                        ));
                }
            }
        }

        public List<ConceptInfoItem> GetConceptInfo()
        {
            //Init();
            return m_List;
        }

        public double GetRelationShipEx(string ts_no_a, string name_b)
        {
            ConceptInfoItem item_a = m_List.Find(it => it.code == ts_no_a);
            if(item_a != null)
            {
                string name_a = item_a.concept_name;
                return GetRelationShip(name_a, name_b);
            }
            return 0;
        }
        public double GetRelationShip(string name_a,string name_b)
        {
            if(name_a == name_b)
            {
                return 100;
            }
            else
            {
                List<ConceptInfoItem> list_a = m_List.FindAll(it => it.concept_name == name_a);
                List<ConceptInfoItem> list_b = m_List.FindAll(it => it.concept_name == name_b);

                double public_tscode_num = 0;
                foreach(ConceptInfoItem item_a in list_a)
                {
                    foreach(ConceptInfoItem item_b in list_b)
                    {
                        if(item_a.ts_code == item_b.ts_code)
                        {
                            public_tscode_num += 1;
                        }
                    }
                }
                return public_tscode_num * 100 / (list_a.Count + list_b.Count - public_tscode_num);
            }
        }
    }
}
