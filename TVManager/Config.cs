using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpYaml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace TVManager
{
    public struct DisplayConfig
    {
        public WDDM.PathInfo[] PathInfos;
        public WDDM.ModeInfo[] ModeInfos;
    }

    public struct WindowWatchlist
    {
        public string ClassName;
        public string WindowName;
    }

    public enum ModeType
    {
        Default,
        TVMode
    }

    public struct ConfigData
    {
        public DisplayConfig DefaultDisplay;
        public DisplayConfig TVDisplay;
        public ModeType Mode;
        public WindowWatchlist[] WindowWatchList;
        public string[] TVModeCommandLineCommands;
        public string[] DefaultModeCommandLineCommands;
    }

    public class Config
    {
        private static Config s_Instance = new Config();
        public static TVManager.Config Instance
        {
            get { return s_Instance; }
        }

        public ConfigData ConfigData;


        public void ReadConfig()
        {
            Serializer YAMLSerializer = new Serializer();
            string FolderPath = Path.GetDirectoryName(Application.ExecutablePath);

            string ConfigPath = Path.Combine(FolderPath, "config.yml");

            if (File.Exists(ConfigPath))
            {
                StreamReader Reader = File.OpenText(ConfigPath);
                ConfigData Deserialized = YAMLSerializer.Deserialize<ConfigData>(Reader);
                ConfigData = Deserialized;
                Reader.Close();
            }
        }

        public void WriteConfig()
        {
            Serializer YAMLSerializer = new Serializer();
            string FolderPath = Path.GetDirectoryName(Application.ExecutablePath);

            FileStream ConfigFile = File.Open(Path.Combine(FolderPath, "config.yml"), FileMode.OpenOrCreate);
            using (StreamWriter Writer = new StreamWriter(ConfigFile))
            {
                YAMLSerializer.Serialize(Writer, ConfigData);

            }
            ConfigFile.Close();
        }

        public void SetDefaultConfig(DisplayConfig DefaultConfig)
        {
            ConfigData.DefaultDisplay = DefaultConfig;
            WriteConfig();
        }

        public void SetTVConfig(DisplayConfig TVConfig)
        {
            ConfigData.TVDisplay = TVConfig;
            WriteConfig();
        }

        public bool IsProcessInWatchlistRunning()
        {
            foreach (WindowWatchlist Watchlist in ConfigData.WindowWatchList)
            {
                if (!Utility.FindWindow(Watchlist.ClassName, Watchlist.WindowName).Equals(IntPtr.Zero))
                {
                    return true;
                }
            }
            return false;
        }

        public void SetMode(ModeType Mode)
        {
            ConfigData.Mode = Mode;
            WriteConfig();
        }

    }
}
