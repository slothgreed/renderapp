using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
