using System;


namespace CHSWPBMP
{
    public static class Program
    {
        public static ConfigManager ConfigManager;
        public static MainApplication MainApplication;
        public static void Main(string[] args)
        {
            ConfigManager = new ConfigManager();
            MainApplication = new MainApplication();
            MainApplication.Run();
           
        }
    }
}