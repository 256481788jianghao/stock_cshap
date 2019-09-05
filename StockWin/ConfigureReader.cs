using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StockWin
{
    public class ConfigureReader
    {
        static string m_ConfigFilePath = "./SystemConfig.json";
        public static ConfigParam ConfigP;

        

        public ConfigureReader()
        {
            
        }

        public static void Init()
        {
            if (File.Exists(m_ConfigFilePath))
            {
                try
                {
                    string json_info_str = File.ReadAllText(m_ConfigFilePath);
                    ConfigP = JsonConvert.DeserializeObject<ConfigParam>(json_info_str);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("sysconfig:" + ex.ToString());
                }
            }
            else
            {
                ConfigP = new ConfigParam();
            }
        }

        public static void Save()
        {
            try
            {
                string ans = JsonConvert.SerializeObject(ConfigP);
                File.WriteAllText(m_ConfigFilePath, ans);
            }
            catch (Exception ex)
            {
                MessageBox.Show("sysconfig:" + ex.ToString());
            }
        }
    }

    

    public class ConfigParam
    {
        public StockDB StockDBP = new StockDB();
    }

    public class StockDB
    {
        public string StockDBPath = "";
    }

    
}
