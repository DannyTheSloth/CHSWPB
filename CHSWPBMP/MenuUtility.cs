using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHSWPBMP
{
    public static class MenuUtility
    {
        public static string GetInput(string header)
        {
            Console.Clear();
            Console.WriteLine(header);
            StringBuilder sb = new();
            for (int i = 0; i < header.Length; i++)
                sb.Append("-");
            Console.WriteLine(sb.ToString());
            return Console.ReadLine();
        }
    }
}
