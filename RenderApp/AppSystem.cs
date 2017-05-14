using System;

namespace RenderApp
{
    class AppSystem
    {
        private static string kiDirectory = null;
        public static string KIDirectory
        {
            get
            {
                if (kiDirectory == null)
                {
                    kiDirectory = Environment.GetEnvironmentVariable("KIProject");
                }
                return kiDirectory;
            }
        }
    }
}
