using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHSWPB
{
    [Serializable]
    public class Config
    {
        public string Name { get; set; }
        public ObservableCollection<Waypoint> Waypoints;
        public ObservableCollection<Action> Actions;
        
        public Config(string name)
        {
            Name = name;
            Waypoints = new ObservableCollection<Waypoint>();
            Actions = new ObservableCollection<Action>();
        }
    }
}
