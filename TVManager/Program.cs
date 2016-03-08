using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TVManager.Properties;
using TVManager.WDDM;

namespace TVManager
{

    static class Program
    {
        static NotifyIcon TVManagerIcon;
        static volatile bool Shutdown = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                TVManagerIcon = new NotifyIcon();
                TVManagerIcon.Icon = Resources.TrayIconDisabled;
                TVManagerIcon.Visible = true;
                TVManagerIcon.DoubleClick += TVManagerIcon_DoubleClick;
                TVManagerIcon.Text = "TV Manager";

                CECUtility.CreateUtility();
                Config.Instance.ReadConfig();

                TVManagerIcon.ContextMenuStrip = TVManagerContextMenu.CreateContextMenu();


                Thread PollThread = new Thread(new ThreadStart(() =>
                {
                    while (!Shutdown)
                    {

                        bool IsInTVMode = Config.Instance.ConfigData.Mode == ModeType.TVMode;
                        bool IsWatchlistProcessRunning = Config.Instance.IsProcessInWatchlistRunning();
                        if(IsInTVMode != IsWatchlistProcessRunning)
                        {
                            if (IsWatchlistProcessRunning)
                            {
                                SetMode(ModeType.TVMode);
                            }
                            else
                            {
                                SetMode(ModeType.Default);
                            }
                        }
                        Thread.Sleep(1000);
                    }
                }));
                PollThread.Start();
                Application.ApplicationExit += Application_ApplicationExit;
                Application.Run();


            }
            catch (Exception Exc)
            {
                string FolderPath = Path.GetDirectoryName(Application.ExecutablePath);
                FileStream LogFile = File.Open(Path.Combine(FolderPath, "error.log"), FileMode.OpenOrCreate);
                using (StreamWriter Writer = new StreamWriter(LogFile))
                {
                    Writer.Write(Exc.ToString());
                }
                MessageBox.Show(Exc.ToString());
                LogFile.Close(); 
            }

        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            TVManagerIcon.Visible = false;
            Shutdown = true;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        private static void TVManagerIcon_DoubleClick(object sender, EventArgs e)
        {
            SetMode(Config.Instance.ConfigData.Mode == ModeType.Default ? ModeType.TVMode : ModeType.Default);
        }

        public static void OnActiveChanged(bool Active)
        {
            if (Active)
            {
                TVManagerIcon.Icon = Resources.TrayIconEnabled;
            }
            else
            {
                TVManagerIcon.Icon = Resources.TrayIconDisabled;
            }
        }

        public static void SetMode(ModeType Mode)
        {
            Config.Instance.SetMode(Mode);
            TVManagerContextMenu.Toggle.Checked = Mode == ModeType.TVMode;

            if (Mode == ModeType.TVMode)
            {
                if (Config.Instance.ConfigData.TVDisplay.PathInfos != null &&
                    Config.Instance.ConfigData.TVDisplay.ModeInfos != null)
                {

                    CCD.SetDisplayConfig(
                        SetDisplayConfigFlags.Apply | SetDisplayConfigFlags.UseSuppliedDisplayConfig | SetDisplayConfigFlags.SaveToDatabase, 
                        Config.Instance.ConfigData.TVDisplay.PathInfos, 
                        Config.Instance.ConfigData.TVDisplay.ModeInfos);
                }
                CECUtility.Instance.Lib.PowerOnDevices(CecSharp.CecLogicalAddress.Tv);
                Thread.Sleep(3000);
                CECUtility.Instance.Lib.SetActiveSource(CecSharp.CecDeviceType.Reserved);

                if (Config.Instance.ConfigData.TVModeCommandLineCommands != null)
                {
                    foreach (string Command in Config.Instance.ConfigData.TVModeCommandLineCommands)
                    {
                        System.Diagnostics.Process Process = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo StartInfo = new System.Diagnostics.ProcessStartInfo();
                        StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        StartInfo.FileName = "cmd.exe";
                        StartInfo.Arguments = "/C " + Command;
                        Process.StartInfo = StartInfo;
                        Process.Start();
                    }
                }
                
            }
            else
            {
                if (Config.Instance.ConfigData.DefaultDisplay.PathInfos != null &&
                    Config.Instance.ConfigData.DefaultDisplay.ModeInfos != null)
                {
                    CCD.SetDisplayConfig(
                        SetDisplayConfigFlags.Apply | SetDisplayConfigFlags.UseSuppliedDisplayConfig | SetDisplayConfigFlags.SaveToDatabase,
                        Config.Instance.ConfigData.DefaultDisplay.PathInfos,
                        Config.Instance.ConfigData.DefaultDisplay.ModeInfos);
                }
                CECUtility.Instance.Lib.StandbyDevices(CecSharp.CecLogicalAddress.Tv);

                if (Config.Instance.ConfigData.DefaultModeCommandLineCommands != null)
                {
                    foreach (string Command in Config.Instance.ConfigData.DefaultModeCommandLineCommands)
                    {
                        System.Diagnostics.Process Process = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo StartInfo = new System.Diagnostics.ProcessStartInfo();
                        StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        StartInfo.FileName = "cmd.exe";
                        StartInfo.Arguments = "/C " + Command;
                        Process.StartInfo = StartInfo;
                        Process.Start();
                    }
                }

            }

            OnActiveChanged(Mode == ModeType.TVMode);
        }
    }
}
