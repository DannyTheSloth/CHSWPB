using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHSWPB
{
    public class ConfigManager
    {
        public ConfigManager()
        {

        }

      
        public void SaveConfig(Config config)
        {
            using (FileDialog fd = new SaveFileDialog())
            {
                fd.InitialDirectory = Environment.CurrentDirectory;
                DialogResult dr = fd.ShowDialog();

                if (dr != DialogResult.OK) throw new Exception("No File saved");

                using (FileStream fs = new FileStream(fd.FileName, FileMode.OpenOrCreate))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, config);
                }
            }
        }
        public Config CreateNewConfig(string name)
        {
            return new Config(name);
        }

        public Config OpenConfig()
        {
            using (FileDialog fd = new OpenFileDialog())
            {
                fd.InitialDirectory = Environment.CurrentDirectory;
                DialogResult dr = fd.ShowDialog();

                if (dr != DialogResult.OK) return null;

                using (FileStream fs = new FileStream(fd.FileName, FileMode.Open))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    return (Config) binaryFormatter.Deserialize(fs);
                }
            }
        }
    }
}
