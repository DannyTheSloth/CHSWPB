using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHSWPB
{
    [Serializable]
    public class Position
    {
        public double x { get; set; }
        public double y { get; set; }
        public double t { get; set; }
    }
}
