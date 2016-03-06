using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TVManager.Properties;

namespace TVManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            NotifyIcon TVManagerIcon = new NotifyIcon();
            TVManagerIcon.Icon = Resources.TrayIconDisabled;
            TVManagerIcon.Visible = true;

            CECUtility.CreateUtility();

            TVManagerIcon.ContextMenuStrip = TVManagerContextMenu.CreateContextMenu();

            Application.Run();

        }
    }
}
