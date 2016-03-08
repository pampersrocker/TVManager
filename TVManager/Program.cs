using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TVManager.Properties;
using SharpYaml.Serialization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TVManager
{

    static class Program
    {
        static NotifyIcon TVManagerIcon;

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


                Application.Run();

                TVManagerIcon.Visible = false;
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

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        private static void TVManagerIcon_DoubleClick(object sender, EventArgs e)
        {
            TVManagerContextMenu.Toggle.Checked = !TVManagerContextMenu.Toggle.Checked;
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
    }
}
