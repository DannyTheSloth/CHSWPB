using System.Collections.ObjectModel;

namespace CHSWPBMP
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

        public Config()
        {
            Waypoints = new ObservableCollection<Waypoint>();
            Actions = new ObservableCollection<Action>();
        }
    }
}
