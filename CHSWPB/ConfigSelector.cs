using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHSWPB
{
    public partial class ConfigSelector : Form
    {
        public ConfigSelector()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                Config config = Program.ConfigManager.CreateNewConfig(textBox1.Text);
                Program.MainForm.SelectedConfig = config;
                Hide();
                Program.MainForm.Closed += (s, args) => Close();
                Program.MainForm.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Config config = Program.ConfigManager.OpenConfig();
            Program.MainForm.SelectedConfig = config;
            Hide();
            Program.MainForm.Closed += (s, args) => Close();
            Program.MainForm.Show();
        }
    }
}
