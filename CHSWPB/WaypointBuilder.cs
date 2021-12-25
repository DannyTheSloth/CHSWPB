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
    public partial class WaypointBuilder : Form
    {
        public int index;
        public string[] ActionTypes = { "SPIN_CAROUSEL", "PLACE_PRELOAD", "PLACE_CUBE_FROM_WAREHOUSE" };
        public Waypoint editWaypoint;

        public WaypointBuilder()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(ActionTypes);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           

            Waypoint waypoint = new Waypoint();

            waypoint.startPose = new Position
            {
                x = Convert.ToInt16(textBox1.Text),
                y = Convert.ToInt16(textBox2.Text),
                t = Convert.ToInt16(textBox3.Text)
            };

            waypoint.endPose = new Position
            {
                x = Convert.ToInt16(textBox4.Text),
                y = Convert.ToInt16(textBox5.Text),
                t = Convert.ToInt16(textBox6.Text)
            };

            waypoint.Actions = new List<string>();

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                waypoint.Actions.Add(listBox1.Items[i].ToString());
            }

            if (editWaypoint != null)
            {
                waypoint.Index = editWaypoint.Index;
                Program.MainForm.Invoke((MethodInvoker)(delegate
                {
                    Program.MainForm.editWaypoint(waypoint);
                }));
                Close();
                return;
            }

            waypoint.Index = index;

            Program.MainForm.Invoke((MethodInvoker)(delegate
            {
                Program.MainForm.addNewWaypoint(waypoint);
            }));

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox1.SelectedItem.ToString()))
            {
                listBox1.Items.Add(comboBox1.Text);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void WaypointBuilder_Shown(object sender, EventArgs e)
        {
            if (editWaypoint != null)
            {
                textBox1.Text = editWaypoint.startPose.x.ToString();
                textBox2.Text = editWaypoint.startPose.y.ToString();
                textBox3.Text = editWaypoint.startPose.t.ToString();
                textBox4.Text = editWaypoint.endPose.x.ToString();
                textBox5.Text = editWaypoint.endPose.y.ToString();
                textBox6.Text = editWaypoint.endPose.t.ToString();

                listBox1.Items.AddRange(editWaypoint.Actions.ToArray());
            }
        }
    }
}
