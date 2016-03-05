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
            Toggle.Click += new EventHandler(Toggle_Click);


            ToolStripMenuItem Startup = new ToolStripMenuItem();
            Startup.Text = "T&oggle";
            Startup.Checked = Utility.IsStartupSet();
            Startup.CheckOnClick = true;
            Startup.CheckedChanged += Startup_CheckedChanged; 
           

            ToolStripMenuItem Exit = new ToolStripMenuItem();
            Exit.Text = "E&xit";
            Exit.Click += new EventHandler(Exit_Click);


            Menu.Items.Add(Toggle);
            Menu.Items.Add(Exit);

            return Menu;
        }

        private static void Startup_CheckedChanged(object sender, EventArgs e)
        {
            Utility.SetStartup(((ToolStripMenuItem)sender).Checked);
        }

        private static void Toggle_Click(object sender, EventArgs e)
        {

        }

        private static void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
