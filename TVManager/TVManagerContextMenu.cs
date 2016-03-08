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
        public static ToolStripMenuItem Toggle;

        public static ContextMenuStrip CreateContextMenu()
        {
            ContextMenuStrip Menu = new ContextMenuStrip();

            Toggle = new ToolStripMenuItem();
            Toggle.Text = "T&oggle";
            var PowerStatus = CECUtility.Instance.Lib.GetDevicePowerStatus(CecSharp.CecLogicalAddress.Tv);
            Toggle.Checked = PowerStatus == CecSharp.CecPowerStatus.InTransitionStandbyToOn || PowerStatus == CecSharp.CecPowerStatus.On;
            Toggle.Click += Toggle_Click;

            ToolStripMenuItem Startup = new ToolStripMenuItem();
            Startup.Text = "R&un at Windows Startup";
            Startup.Checked = Utility.IsStartupSet();
            Startup.CheckOnClick = true;
            Startup.CheckedChanged += Startup_CheckedChanged;

            ToolStripMenuItem SetDefaultConfig = new ToolStripMenuItem();
            SetDefaultConfig.Text = "Set Current Display Setup as Default";
            SetDefaultConfig.Click += SetDefaultConfig_Click;

            ToolStripMenuItem SetTVConfig = new ToolStripMenuItem();
            SetTVConfig.Text = "Set Current Display Setup as TV";
            SetTVConfig.Click += SetTVConfig_Click;

            ToolStripMenuItem Options = new ToolStripMenuItem();
            Options.Text = "O&ptions";
            Options.DropDownItems.Add(Startup);
            Options.DropDownItems.Add(SetDefaultConfig);
            Options.DropDownItems.Add(SetTVConfig);



            ToolStripMenuItem Exit = new ToolStripMenuItem();
            Exit.Text = "E&xit";
            Exit.Click += new EventHandler(Exit_Click);


            Menu.Items.Add(Toggle);
            Menu.Items.Add(Options);
            Menu.Items.Add(Exit);

            return Menu;
        }

        private static void Toggle_Click(object sender, EventArgs e)
        {
            Program.SetMode(Config.Instance.ConfigData.Mode == ModeType.Default ? ModeType.TVMode : ModeType.Default);
        }

        private static void SetTVConfig_Click(object sender, EventArgs e)
        {
            DisplayConfig Config = new DisplayConfig();
            WDDM.TopologyId Topology;
            WDDM.CCD.QueryDisplayConfig(WDDM.QueryDisplayFlags.DatabaseCurrent, out Config.PathInfos, out Config.ModeInfos, out Topology);
            TVManager.Config.Instance.SetTVConfig(Config);
        }

        private static void SetDefaultConfig_Click(object sender, EventArgs e)
        {
            DisplayConfig Config = new DisplayConfig();
            WDDM.TopologyId Topology;
            WDDM.CCD.QueryDisplayConfig(WDDM.QueryDisplayFlags.DatabaseCurrent, out Config.PathInfos, out Config.ModeInfos, out Topology);
            TVManager.Config.Instance.SetDefaultConfig(Config);
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
