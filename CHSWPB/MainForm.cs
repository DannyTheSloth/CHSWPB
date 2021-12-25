using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace CHSWPB
{
    public partial class MainForm : Form
    {
        public Config SelectedConfig;
        
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            updateListbox();
        }

        private void updateListbox()
        {
            listBox1.Items.Clear();

            foreach (Waypoint waypoint in SelectedConfig.Waypoints)
            {
                listBox1.Items.Add(
                    $"[{waypoint.Index}] | WAYPOINT @ {waypoint.startPose.x}, {waypoint.startPose.y}, {waypoint.startPose.t}  TO  {waypoint.endPose.x}, {waypoint.endPose.y}, {waypoint.endPose.t}");
                foreach (string action in waypoint.Actions)
                {
                    listBox1.Items.Add(action);
                }
            }
        }

        private int getWaypointCount()
        {
            int count = 0;

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.Items[i].ToString().Contains("WAYPOINT"))
                {
                    count++;
                }
            }

            return count;
        }

        public void addNewWaypoint(Waypoint waypoint)
        {
            if (waypoint.Index > SelectedConfig.Waypoints.Count)
                SelectedConfig.Waypoints.Add(waypoint);
            else
                SelectedConfig.Waypoints.Insert(waypoint.Index, waypoint);

            updateListbox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WaypointBuilder waypointBuilder = new WaypointBuilder();
            waypointBuilder.index = SelectedConfig.Waypoints.Count + 1;
            waypointBuilder.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.ConfigManager.SaveConfig(SelectedConfig);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem.ToString().Contains("WAYPOINT"))
            {
                int first = listBox1.SelectedItem.ToString().IndexOf("[") + "[".Length;
                int last = listBox1.SelectedItem.ToString().IndexOf("]");

                int index = Convert.ToInt16(listBox1.SelectedItem.ToString().Substring(first, last - first));
                SelectedConfig.Waypoints.RemoveAt(index - 1);
                foreach (Waypoint waypoint in SelectedConfig.Waypoints)
                {
                    if (waypoint.Index > index)
                    {
                        waypoint.Index -= 1;
                    }
                }
            }
            else
            {
                for (int i = listBox1.SelectedIndex; i > 0; i--)
                {
                    if (listBox1.Items[i].ToString().Contains("WAYPOINT"))
                    {
                        int first = listBox1.Items[i].ToString().IndexOf("[") + "[".Length;
                        int last = listBox1.Items[i].ToString().IndexOf("]");
                        int index = Convert.ToInt16(listBox1.Items[i].ToString().Substring(first, last - first));
                        foreach (Waypoint waypoint in SelectedConfig.Waypoints)
                        {
                            if (waypoint.Index != index) continue;

                            waypoint.Actions.RemoveAt((listBox1.SelectedIndex - i) - 1);
                            break;
                        }

                        break;
                    }
                }
            }

            updateListbox();
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem.ToString().Contains("WAYPOINT"))
            {
                int first = listBox1.SelectedItem.ToString().IndexOf("[") + "[".Length;
                int last = listBox1.SelectedItem.ToString().IndexOf("]");

                int index = Convert.ToInt16(listBox1.SelectedItem.ToString().Substring(first, last - first));
                SelectedConfig.Waypoints[index - 1].Index -= 1;

                try
                {
                    SelectedConfig.Waypoints[index - 2].Index += 1;
                } catch {}

                try
                {
                    SelectedConfig.Waypoints.Move(index -1, index -2);
                } catch {}
            }

            updateListbox();
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem.ToString().Contains("WAYPOINT"))
            {
                int first = listBox1.SelectedItem.ToString().IndexOf("[") + "[".Length;
                int last = listBox1.SelectedItem.ToString().IndexOf("]");

                int index = Convert.ToInt16(listBox1.SelectedItem.ToString().Substring(first, last - first));
                SelectedConfig.Waypoints[index - 1].Index += 1;

                try
                {
                    SelectedConfig.Waypoints[index].Index -= 1;
                } catch {}

                try
                {
                    SelectedConfig.Waypoints.Move(index -1, index);
                } catch {}
            }

            updateListbox();
        }

        public void editWaypoint(Waypoint editWaypoint)
        {
            for (int i = 0; i < SelectedConfig.Waypoints.Count; i++)
            {
                if (SelectedConfig.Waypoints[i].Index == editWaypoint.Index)
                {
                    SelectedConfig.Waypoints[i] = editWaypoint;
                    break;
                }
            }

            updateListbox();
        }

        private void editWaypointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!listBox1.SelectedItem.ToString().Contains("WAYPOINT"))
                return;
            int first = listBox1.SelectedItem.ToString().IndexOf("[") + "[".Length;
            int last = listBox1.SelectedItem.ToString().IndexOf("]");

            int index = Convert.ToInt16(listBox1.SelectedItem.ToString().Substring(first, last - first));
            foreach (Waypoint waypoint in SelectedConfig.Waypoints)
            {
                if (waypoint.Index == index)
                {
                    WaypointBuilder waypointBuilder = new WaypointBuilder();
                    waypointBuilder.editWaypoint = waypoint;
                    waypointBuilder.Show();
                    break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ipAd = string.Empty;

            if (string.IsNullOrEmpty(ipAd))
                return;

            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(ms, SelectedConfig);
                data = ms.ToArray();
            }

            TcpClient client = new TcpClient(ipAd, 13092);

            using (NetworkStream networkStream = client.GetStream())
            {
                networkStream.Write(data, 0, data.Length);
            }

            client.Close();
        }
    }
}
