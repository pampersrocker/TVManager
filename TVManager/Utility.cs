using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TVManager
{
    class Utility
    {
        public static void SetStartup(bool RunWithStartup)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (RunWithStartup)
                rk.SetValue(Application.ProductName, Application.ExecutablePath.ToString());
            else
                rk.DeleteValue(Application.ProductName, false);

        }

        public static bool IsStartupSet()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            return rk.GetValue(Application.ProductName) != null;
        }
    }
}
