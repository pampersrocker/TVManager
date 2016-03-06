using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TVManager
{
    class TVManagerContextMenu
    {

        public static ContextMenuStrip CreateContextMenu()
        {
            ContextMenuStrip Menu = new ContextMenuStrip();

            ToolStripMenuItem Toggle = new ToolStripMenuItem();
            Toggle.Text = "T&oggle";
            Toggle.CheckOnClick = true;
            var PowerStatus = CECUtility.Instance.Lib.GetDevicePowerStatus(CecSharp.CecLogicalAddress.Tv);
            Toggle.Checked = PowerStatus == CecSharp.CecPowerStatus.InTransitionStandbyToOn || PowerStatus == CecSharp.CecPowerStatus.On;
            Toggle.CheckedChanged += Toggle_CheckedChanged;


            ToolStripMenuItem Startup = new ToolStripMenuItem();
            Startup.Text = "R&un at Windows Startup";
            Startup.Checked = Utility.IsStartupSet();
            Startup.CheckOnClick = true;
            Startup.CheckedChanged += Startup_CheckedChanged; 
           

            ToolStripMenuItem Exit = new ToolStripMenuItem();
            Exit.Text = "E&xit";
            Exit.Click += new EventHandler(Exit_Click);


            Menu.Items.Add(Toggle);
            Menu.Items.Add(Startup);
            Menu.Items.Add(Exit);

            return Menu;
        }

        private static void Toggle_CheckedChanged(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Checked)
            {

                CECUtility.Instance.Lib.PowerOnDevices(CecSharp.CecLogicalAddress.Broadcast);
                System.Threading.Thread.Sleep(3000);
                CECUtility.Instance.Lib.SetActiveSource(CecSharp.CecDeviceType.Reserved);
            }
            else
            {
                CECUtility.Instance.Lib.StandbyDevices(CecSharp.CecLogicalAddress.Tv);
            }
            
        }

        private static void Startup_CheckedChanged(object sender, EventArgs e)
        {
            Utility.SetStartup(((ToolStripMenuItem)sender).Checked);
        }

        private static void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
