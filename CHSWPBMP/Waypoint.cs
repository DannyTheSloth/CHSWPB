using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHSWPBMP
{
    [Serializable]
    public class Waypoint
    {
        public int Index { get; set; }
        public List<string> Actions { get; set; }
        public Position startPose { get; set; }
        public Position endPose { get; set; }
    }
}
